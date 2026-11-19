// SPDX-License-Identifier: GPL-3.0-or-later
//
// Trucchi - mod per MediEvilRecomp (PAL italiana, spagnola, francese, tedesca, inglese;
// NTSC-U americana)
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

// Attiva i menu trucchi integrati nel gioco senza inserire le combinazioni.
//
// Come funziona il gioco (versione PAL italiana, MEDIEVIL.EXE):
//   - func_80022C2C gira a ogni frame di gioco e legge il pad; con L2 premuto
//     confronta i tasti con due tabelle:
//       0x800CE0B4: 10 tasti  "DUST TO DUST"  (Giu Su Quadrato Triangolo Triangolo
//                   Cerchio Giu Su Quadrato Triangolo)              -> stato ^= 1
//       0x800CE0DC: 25 tasti  (Modalita' Trucchi Estesa)            -> stato ^= 3
//   - lo stato e' un halfword a 0x800EF0A2: 0 = nessun trucco,
//     1 = Modalita' Trucchi, 3 = Modalita' Trucchi Estesa.
// Le altre versioni PAL hanno la stessa routine e le stesse tabelle, ad altri indirizzi
// (config/versions.json: CheatTable*, CheatState; routine dalla mappa degli indirizzi di
// tools/pal-port): spagnola SCES-01495, francese SCES-01492, tedesca SCES-01493,
// inglese SCES-00311. Anche l'americana SCUS-94227 (progetto BlackLabelHQ/MediEvilRecomp):
// routine func_80022BBC, tabelle a 0x800CDFEC / 0x800CE014, stato a 0x800EEE3E, ricavati
// dal codice generato della routine, identico a quello italiano salvo gli indirizzi.
//     Il menu di pausa mostra le voci in base a questo valore.
// La mod scrive lo stato scelto subito dopo func_80022C2C, quindi resta attivo
// anche dopo cambi di livello. Prima di scrivere riconosce la versione dalle due
// tabelle, dati costanti dell'eseguibile: su una versione sconosciuta non tocca nulla.
// Testi del pannello: costante Texts qui sotto (chiavi mods.trucchi.*).
// Il launcher copia la mod anche nel progetto americano, il cui RecompOne usa
// Hexa.NET.ImGui e RecompOne.UI.Localization: per questo il pannello usa solo chiamate
// ImGui presenti in tutte e due le librerie.

using ImGuiNET;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Modding;

public class TrucchiMod : IMod
{
    // --- indirizzi per versione (stessi valori di config/versions.json) --------------------
    // [0] italiana SCES-01494, [1] spagnola SCES-01495, [2] francese SCES-01492,
    // [3] tedesca SCES-01493, [4] inglese SCES-00311
    private static readonly uint[] CheatState =                                   // u16: 0 / 1 / 3
        { 0x800EF0A2u, 0x800EF11Au, 0x800EF1DEu, 0x800EF016u, 0x800EEE76u };
    private static readonly uint[] TableDust =                                    // primo tasto "DUST TO DUST" (Giu)
        { 0x800CE0B4u, 0x800CE09Cu, 0x800CE0C4u, 0x800CDFA8u, 0x800CE01Cu };
    private static readonly uint[] TableExtended =                                // primo tasto sequenza estesa (Triangolo)
        { 0x800CE0DCu, 0x800CE0C4u, 0x800CE0ECu, 0x800CDFD0u, 0x800CE044u };
    // americana SCUS-94227: riconosciuta dal suo hook (indirizzo solo suo), non dal ciclo
    private const int Usa = 5;
    private const uint UsaCheatState = 0x800EEE3Eu, UsaTableDust = 0x800CDFECu, UsaTableExtended = 0x800CE014u;
    private const uint PadDown = 0x00080000u;         // codifica del tasto Giu nelle tabelle
    private const uint PadTriangle = 0x00100000u;     // codifica del tasto Triangolo

    private const string SettingKey = "mods.trucchi-ita.modalita";

    // 0 = disattivati, 1 = Modalita' Trucchi, 3 = Modalita' Trucchi Estesa
    private static volatile int _mode;
    // true quando l'utente ha appena spento i trucchi: azzera lo stato una volta sola
    private static volatile bool _clearPending;
    // esito dell'ultimo controllo di versione (mostrato nel pannello)
    private static volatile int _versionCheck; // 0 = non ancora verificato, 1 = ok, -1 = versione diversa

    static string T(string key) => RecompOne.Runtime.Host.Window.Localization.T(key);

    // testi del pannello in tutte le lingue: la mod li aggiunge a quelli del gioco al
    // caricamento (Localization.Merge), cosi' funzionano anche dove assets/languages.json
    // del progetto non li ha (versione americana)
    const string Texts = """
        {
            "strings": {
                "mods.trucchi.title": {
                    "en": "Game cheats",
                    "it-IT": "Trucchi del gioco",
                    "fr-fr": "Codes du jeu",
                    "es-419": "Trucos del juego",
                    "de-DE": "Spielcheats",
                    "pt-BR": "Trapaças do jogo"
                },
                "mods.trucchi.off": {
                    "en": "Off",
                    "it-IT": "Disattivati",
                    "fr-fr": "Désactivés",
                    "es-419": "Desactivados",
                    "de-DE": "Aus",
                    "pt-BR": "Desativadas"
                },
                "mods.trucchi.mode": {
                    "en": "Cheat Mode",
                    "it-IT": "Modalità Trucchi",
                    "fr-fr": "Mode triche",
                    "es-419": "Modo trucos",
                    "de-DE": "Cheat-Modus",
                    "pt-BR": "Modo trapaça"
                },
                "mods.trucchi.extended": {
                    "en": "Extended Cheat Mode",
                    "it-IT": "Modalità Trucchi Estesa",
                    "fr-fr": "Mode triche étendu",
                    "es-419": "Modo trucos ampliado",
                    "de-DE": "Erweiterter Cheat-Modus",
                    "pt-BR": "Modo trapaça estendido"
                },
                "mods.trucchi.hint": {
                    "en": "The entries appear in the pause menu (Start), as with the original button combinations.",
                    "it-IT": "Le voci compaiono nel menu di pausa (Start), come con le combinazioni originali.",
                    "fr-fr": "Les options apparaissent dans le menu pause (Start), comme avec les combinaisons d'origine.",
                    "es-419": "Las opciones aparecen en el menú de pausa (Start), como con las combinaciones originales.",
                    "de-DE": "Die Einträge erscheinen im Pausenmenü (Start), wie mit den originalen Tastenkombinationen.",
                    "pt-BR": "As opções aparecem no menu de pausa (Start), como com as combinações originais."
                },
                "mods.trucchi.unknown": {
                    "en": "Game version not recognised: this mod works with MediEvil PAL Italian, Spanish, French, German and English, and with the American version.",
                    "it-IT": "Versione del gioco non riconosciuta: la mod funziona con MediEvil PAL italiano, spagnolo, francese, tedesco e inglese, e con la versione americana.",
                    "fr-fr": "Version du jeu non reconnue : ce mod fonctionne avec MediEvil PAL italien, espagnol, français, allemand et anglais, et avec la version américaine.",
                    "es-419": "Versión del juego no reconocida: este mod funciona con MediEvil PAL italiano, español, francés, alemán e inglés, y con la versión estadounidense.",
                    "de-DE": "Spielversion nicht erkannt: Diese Mod funktioniert mit MediEvil PAL Italienisch, Spanisch, Französisch, Deutsch und Englisch sowie mit der amerikanischen Version.",
                    "pt-BR": "Versão do jogo não reconhecida: este mod funciona com MediEvil PAL italiano, espanhol, francês, alemão e inglês, e com a versão americana."
                },
                "mods.trucchi.waiting": {
                    "en": "Takes effect when you enter a level.",
                    "it-IT": "Si attiva quando entri in un livello.",
                    "fr-fr": "S'active quand vous entrez dans un niveau.",
                    "es-419": "Se activa al entrar en un nivel.",
                    "de-DE": "Wird beim Betreten eines Levels aktiv.",
                    "pt-BR": "É ativado ao entrar em uma fase."
                }
            }
        }
        """;

    public void OnLoad()
    {
        RecompOne.Runtime.Host.Window.Localization.Merge(Texts);
        _mode = ModeFromSetting(RecompOne.Runtime.Config.Settings.Get<string>(SettingKey, "0"));
    }

    public void OnUnload()
    {
        // allo scaricamento della mod rimette lo stato del gioco a zero
        _mode = 0;
        _clearPending = true;
    }

    public void DrawSettings()
    {
        int index = _mode switch { 1 => 1, 3 => 2, _ => 0 };

        string[] labels = { T("mods.trucchi.off"), T("mods.trucchi.mode"), T("mods.trucchi.extended") };
        ImGui.TextUnformatted(T("mods.trucchi.title"));
        ImGui.SetNextItemWidth(260f);
        int chosen = index;
        if (ImGui.BeginCombo("##trucchi", labels[index]))
        {
            for (int i = 0; i < labels.Length; i++)
                if (ImGui.Selectable(labels[i], i == index))
                    chosen = i;
            ImGui.EndCombo();
        }
        if (chosen != index)
        {
            int newMode = chosen switch { 1 => 1, 2 => 3, _ => 0 };
            if (newMode == 0 && _mode != 0) _clearPending = true;
            _mode = newMode;
            RecompOne.Runtime.Config.Settings.Set(SettingKey, newMode.ToString());
        }

        ImGui.TextWrapped(T("mods.trucchi.hint"));

        switch (_versionCheck)
        {
            case -1:
                ImGui.TextColored(new System.Numerics.Vector4(1f, 0.4f, 0.4f, 1f), T("mods.trucchi.unknown"));
                break;
            case 0:
                ImGui.TextDisabled(T("mods.trucchi.waiting"));
                break;
        }
    }

    // Eseguito dopo la routine per-frame che gestisce le combinazioni dei trucchi
    // (func_80022C2C nell'italiana), un indirizzo per versione: ognuno e' l'inizio di una
    // funzione solo nella sua versione, gli altri vengono ignorati dal caricatore delle mod
    // (riga "function not found" nella console).
    [PostHook("game", Address = 0x80022C2Cu)]   // italiana, spagnola
    [PostHook("game", Address = 0x80022C94u)]   // francese
    [PostHook("game", Address = 0x80022BC4u)]   // tedesca
    [PostHook("game", Address = 0x80022C04u)]   // inglese
    public static void AfterCheatCheck(CpuContext c, IMemory m) => Apply(m, DetectBuild(m));

    [PostHook("game", Address = 0x80022BBCu)]   // americana
    public static void AfterCheatCheckUsa(CpuContext c, IMemory m)
    {
        bool tables = m.ReadU32(UsaTableDust) == PadDown && m.ReadU32(UsaTableExtended) == PadTriangle;
        Apply(m, tables ? Usa : -1);
    }

    private static void Apply(IMemory m, int build)
    {
        if (build < 0)
        {
            _versionCheck = -1;
            return;
        }
        _versionCheck = 1;

        uint state = build == Usa ? UsaCheatState : CheatState[build];
        int mode = _mode;
        if (mode != 0)
        {
            if (m.ReadU16(state) != (ushort)mode)
                m.WriteU16(state, (ushort)mode);
        }
        else if (_clearPending)
        {
            m.WriteU16(state, 0);
            _clearPending = false;
        }
    }

    // Le due tabelle delle combinazioni sono dati costanti dell'eseguibile:
    // la versione e' quella le cui tabelle sono al loro posto. -1 = non riconosciuta.
    private static int DetectBuild(IMemory m)
    {
        for (int i = 0; i < TableDust.Length; i++)
            if (m.ReadU32(TableDust[i]) == PadDown && m.ReadU32(TableExtended[i]) == PadTriangle)
                return i;
        return -1;
    }

    private static int ModeFromSetting(string value)
    {
        return value switch { "1" => 1, "3" => 3, _ => 0 };
    }
}
