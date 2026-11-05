// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - materiali delle texture per gli shader (Gallowmere)
// Copyright (C) 2026
// Author: https://github.com/Leproide
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

// Il gioco non dice di che materiale e' una superficie. Qui un elenco di texture marcate a mano
// (metallo, vetro, bagnato, ...) viene passato a RecompOne (ShaderMaterials) insieme all'acqua
// del gioco (WaterMaterialPatch), cosi' gli shader sanno dove fare riflessi e lucidita'.
//
// Una texture e' identificata da tpage (pagina e profondita' colore, maschera 0x19F), clut e il
// riquadro UV della texture nella lista del gioco (Texture.At).
//
// Tre livelli, in ordine di precedenza (RecompOne usa la prima marcatura che copre il punto):
//  1. assets/materials.json: marcature fatte con il selettore (file locale, non nel repo). Una voce con "Level" vale solo in
//     quel livello, senza vale ovunque; "none" toglie il materiale (anche a una voce fissa).
//  2. BuiltInMetal: metalli fissi (armatura di Dan, armi, cancelli...), sempre metallo anche con
//     la stima dal colore dello shader accesa. Si spengono da Misc -> Quality of life ->
//     "Default materials".
//  3. lo shader stima il resto dal colore, solo sulle texture senza marcature.
//
// Selettore: Misc -> Quality of life -> "Development mode". Con lo shader Gallowmere attivo,
// clic sinistro sul gioco per scegliere il punto, poi un pulsante per il materiale; "Togli"
// leva il materiale nel punto, "Annulla" torna indietro fino a 3 modifiche. Il file si salva subito.

using System.Text.Json;
using MediEvil;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public static class MaterialTags
{
    public sealed record Entry(int TPage, int Clut, int U0, int V0, int U1, int V1, string Material, int? Level = null);

    public static readonly Option<bool> DevMode = Options.Bool("dev.mode", false);
    public static readonly Option<bool> UseBuiltIn = Options.Bool("render.default_materials", true);
    static volatile bool _useBuiltIn = UseBuiltIn.Value;

    static readonly (string Name, string Label, byte Id)[] Kinds =
    [
        ("metal", "Metallo", ShaderMaterials.Metal),
        ("glass", "Vetro", ShaderMaterials.Glass),
        ("wet", "Bagnato", ShaderMaterials.Wet),
        ("water", "Acqua", ShaderMaterials.Water),
        ("emissive", "Luminoso", ShaderMaterials.Emissive),
        ("stone", "Pietra", ShaderMaterials.Stone),
        ("wood", "Legno", ShaderMaterials.Wood),
    ];

    // metalli fissi (tpage, clut, u0, v0, u1, v1), presi dalle prime marcature fatte in gioco
    static readonly (int TPage, int Clut, int U0, int V0, int U1, int V1)[] BuiltInMetal =
    [
        (11, 31968, 177, 169, 217, 212),
        (27, 32761, 1, 1, 63, 63),
        (27, 32761, 65, 1, 127, 63),
        (9, 32417, 113, 51, 175, 113),
        (15, 31918, 161, 1, 223, 63),
        (10, 31909, 1, 93, 63, 141),
        (10, 31908, 65, 93, 127, 141),
        (13, 32416, 1, 1, 93, 53),
        (24, 32687, 225, 1, 254, 53),
        (30, 32757, 225, 33, 247, 55),
        (29, 32737, 193, 1, 230, 51),
        (25, 32766, 1, 1, 130, 130),
        (28, 32756, 129, 1, 177, 50),
        (28, 32757, 65, 1, 119, 55),
        (29, 32757, 241, 76, 243, 78),
        (13, 32358, 193, 33, 254, 95),
        (12, 32365, 65, 1, 127, 63),
        (25, 32764, 145, 1, 209, 65),
        (10, 32366, 193, 1, 254, 63),
        (12, 31798, 177, 129, 207, 159),
        (30, 31333, 113, 118, 129, 164),
        (10, 31392, 145, 199, 167, 247),
        (14, 32244, 193, 121, 231, 183),
        (14, 31420, 81, 219, 135, 249),
        (13, 31229, 225, 205, 241, 221),
        (26, 31268, 1, 155, 17, 177),
        (13, 32360, 193, 65, 247, 119),
        (15, 32356, 1, 65, 55, 119),
        (14, 32248, 1, 121, 81, 151),
        (11, 32432, 1, 1, 63, 63),
        (27, 31401, 177, 151, 209, 183),
        (14, 32231, 1, 187, 47, 233),
        (9, 31407, 65, 225, 111, 249),
        (27, 31403, 81, 153, 113, 185),
        (10, 32227, 49, 199, 129, 221),
        (9, 31332, 177, 231, 215, 253),
        (30, 31403, 1, 155, 33, 187),
        (26, 31411, 65, 146, 103, 184),
        (8, 32174, 113, 101, 175, 163),
        (26, 32762, 193, 1, 254, 63),
        (26, 32763, 129, 1, 191, 63),
        (9, 32352, 129, 65, 191, 127),
        (13, 32366, 145, 1, 207, 63),
    ];

    static readonly object Gate = new();
    static List<Entry> _entries = new();
    static (int TPage, int Clut, int U, int V)? _lastPick;
    static (int U0, int V0, int U1, int V1)? _pickBounds;
    static int _level = -1;
    static bool _thisLevelOnly = true;
    // ultime modifiche annullabili (copie dell'elenco prima di ogni modifica)
    const int UndoDepth = 3;
    static readonly LinkedList<List<Entry>> Undo = new();

    // cambia a ogni modifica dell'elenco: WaterMaterialPatch rimanda i materiali a RecompOne
    public static int Version { get; private set; }

    static string FilePath
    {
        get
        {
            var local = Path.Combine(Directory.GetCurrentDirectory(), "assets", "materials.json");
            if (File.Exists(local)) return local;
            var beside = Path.Combine(AppContext.BaseDirectory, "assets", "materials.json");
            return File.Exists(beside) ? beside : local;
        }
    }

    public static void Register()
    {
        try
        {
            if (File.Exists(FilePath))
                _entries = JsonSerializer.Deserialize<List<Entry>>(File.ReadAllText(FilePath)) ?? new();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Materiali] {FilePath} non leggibile: {e.Message}");
        }
        DevMode.Changed += on => { if (!on) ShaderMaterials.PickEnabled = false; };
        UseBuiltIn.Changed += on => { _useBuiltIn = on; Version++; };
        Version++;
    }

    // aggiunge le marcature all'elenco che WaterMaterialPatch passa a RecompOne (chiamata ogni
    // 15 frame con il livello attuale; il livello entra anche nella firma dell'elenco)
    public static void AddTo(List<(int TPage, int Clut, int U0, int V0, int U1, int V1, byte Material)> tags, int level)
    {
        _level = level;
        lock (Gate)
            foreach (var e in _entries)
            {
                if (e.Level is { } l && l != level) continue;
                var id = e.Material == "none" ? (byte)0 : Array.Find(Kinds, k => k.Name == e.Material).Id;
                if (id != 0 || e.Material == "none") tags.Add((e.TPage, e.Clut, e.U0, e.V0, e.U1, e.V1, id));
            }
        if (!_useBuiltIn) return;
        foreach (var b in BuiltInMetal)
            tags.Add((b.TPage, b.Clut, b.U0, b.V0, b.U1, b.V1, ShaderMaterials.Metal));
    }

    static void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            lock (Gate)
                File.WriteAllText(FilePath, JsonSerializer.Serialize(_entries, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                }));
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Materiali] salvataggio non riuscito: {e.Message}");
        }
        Version++;
    }

    // riquadro della texture del gioco che contiene il punto scelto (stessa pagina e clut)
    static (int, int, int, int)? FindBounds(int tpage, int clut, int u, int v)
    {
        for (var i = 0; i < MediEvil.Texture.MaxCount; i++)
        {
            if (MediEvil.Texture.At(i) is not { } t) continue;
            if ((t.TPage & 0x19F) != (tpage & 0x19F) || t.Clut != clut) continue;
            var b = t.Bounds;
            if (u >= b.U0 && u <= b.U1 && v >= b.V0 && v <= b.V1) return b;
        }
        return null;
    }

    static bool Contains(int tpage, int clut, int u0, int v0, int u1, int v1, int pt, int pc, int u, int v) =>
        (tpage & 0x19F) == (pt & 0x19F) && clut == pc &&
        u >= Math.Min(u0, u1) && u <= Math.Max(u0, u1) && v >= Math.Min(v0, v1) && v <= Math.Max(v0, v1);

    // marcature del file che coprono il punto in questo livello (come le cerca RecompOne)
    static List<Entry> Covering(int tpage, int clut, int u, int v)
    {
        lock (Gate)
            return _entries.FindAll(e => (e.Level == null || e.Level == _level) &&
                                         Contains(e.TPage, e.Clut, e.U0, e.V0, e.U1, e.V1, tpage, clut, u, v));
    }

    static bool BuiltIn(int tpage, int clut, int u, int v) => _useBuiltIn &&
        Array.Exists(BuiltInMetal, b => Contains(b.TPage, b.Clut, b.U0, b.V0, b.U1, b.V1, tpage, clut, u, v));

    // salva l'elenco attuale per "Annulla", poi applica la modifica e scrive il file
    static void Change(Action<List<Entry>> edit)
    {
        lock (Gate)
        {
            Undo.AddLast(new List<Entry>(_entries));
            while (Undo.Count > UndoDepth) Undo.RemoveFirst();
            edit(_entries);
        }
        Save();
    }

    static void DrawUndo()
    {
        int n;
        lock (Gate) n = Undo.Count;
        ImGuiNET.ImGui.BeginDisabled(n == 0);
        if (ImGuiNET.ImGui.Button($"Annulla ({n})##matundo"))
        {
            lock (Gate)
            {
                _entries = Undo.Last!.Value;
                Undo.RemoveLast();
            }
            Save();
        }
        ImGuiNET.ImGui.EndDisabled();
    }

    static string Label(string material) =>
        material == "none" ? "nessuno (tolto)" : Array.Find(Kinds, k => k.Name == material).Label ?? material;

    // sezione "Development" del pannello Quality of life
    public static void DrawPanel()
    {
        ImGuiNET.ImGui.SeparatorText(Localization.T("qol.dev"));
        SettingsWidgets.Toggle(DevMode, "qol.dev_mode", "qol.dev_mode.hint");
        if (!DevMode.Value) return;

        ImGuiNET.ImGui.TextDisabled("Materiali delle texture (shader Gallowmere)");
        bool on = ShaderMaterials.PickEnabled;
        if (ImGuiNET.ImGui.Checkbox("Selettore texture (clic sinistro sul gioco per scegliere il punto)", ref on))
            ShaderMaterials.PickEnabled = on;
        ImGuiNET.ImGui.TextDisabled($"{_entries.Count} marcature + {BuiltInMetal.Length} metalli fissi, livello {_level}, file {FilePath}");
        if (!on) return;

        bool flip = ShaderMaterials.PickFlipY;
        if (ImGuiNET.ImGui.Checkbox("Inverti verticale (se il mirino legge il punto opposto)", ref flip))
            ShaderMaterials.PickFlipY = flip;
        ImGuiNET.ImGui.Checkbox("Nuove marcature solo in questo livello", ref _thisLevelOnly);

        // mirino sul punto scelto: ViewRect e' il rettangolo dell'immagine del gioco sullo schermo
        var io = ImGuiNET.ImGui.GetIO();
        var rect = ShaderMaterials.ViewRect;
        if (rect.Z <= rect.X || rect.W <= rect.Y) rect = new System.Numerics.Vector4(0, 0, io.DisplaySize.X, io.DisplaySize.Y);
        var c = new System.Numerics.Vector2(rect.X + (rect.Z - rect.X) * ShaderMaterials.PickX,
                                            rect.Y + (rect.W - rect.Y) * ShaderMaterials.PickY);
        var draw = ImGuiNET.ImGui.GetForegroundDrawList();
        draw.AddLine(c - new System.Numerics.Vector2(12, 0), c + new System.Numerics.Vector2(12, 0), 0xFF00FFFF, 2f);
        draw.AddLine(c - new System.Numerics.Vector2(0, 12), c + new System.Numerics.Vector2(0, 12), 0xFF00FFFF, 2f);

        var pick = ShaderMaterials.Picked;
        if (pick is not { } p)
        {
            ImGuiNET.ImGui.Text("Nessuna texture sotto il mirino (serve lo shader Gallowmere attivo).");
            DrawUndo();
            return;
        }
        if (_lastPick != pick)
        {
            _lastPick = pick;
            _pickBounds = FindBounds(p.TPage, p.Clut, p.U, p.V);
        }
        var bounds = _pickBounds ?? (0, 0, 255, 255);
        var covering = Covering(p.TPage, p.Clut, p.U, p.V);
        var current = covering.Count > 0 ? covering[0] : null;
        var builtIn = BuiltIn(p.TPage, p.Clut, p.U, p.V);
        ImGuiNET.ImGui.Text($"tpage {p.TPage:X3}  clut {p.Clut:X4}  uv {p.U},{p.V}  texture {bounds.U0},{bounds.V0}-{bounds.U1},{bounds.V1}" +
                            (_pickBounds == null ? " (non trovata, pagina intera)" : ""));
        var what = current != null ? Label(current.Material) + (current.Level != null ? " (solo questo livello)" : "")
                 : builtIn ? "Metallo (fisso)" : "nessuno (stima dal colore)";
        ImGuiNET.ImGui.Text($"Materiale: {what}" + (covering.Count > 1 ? $" ({covering.Count} marcature sovrapposte)" : ""));

        int? level = _thisLevelOnly && _level >= 0 ? _level : null;
        foreach (var k in Kinds)
        {
            if (ImGuiNET.ImGui.Button(k.Label + "##mat" + k.Name))
            {
                var entry = new Entry(p.TPage & 0x19F, p.Clut, bounds.U0, bounds.V0, bounds.U1, bounds.V1, k.Name, level);
                Change(list =>
                {
                    foreach (var e in covering) list.Remove(e);
                    list.Add(entry);
                });
            }
            ImGuiNET.ImGui.SameLine();
        }
        // togli: leva le marcature del file; su un metallo fisso scrive "none" per spegnerlo
        if (ImGuiNET.ImGui.Button("Togli##matnone") && (covering.Count > 0 || builtIn))
        {
            var none = new Entry(p.TPage & 0x19F, p.Clut, bounds.U0, bounds.V0, bounds.U1, bounds.V1, "none", level);
            Change(list =>
            {
                foreach (var e in covering) list.Remove(e);
                if (builtIn) list.Add(none);
            });
        }
        ImGuiNET.ImGui.SameLine();
        DrawUndo();
    }
}
