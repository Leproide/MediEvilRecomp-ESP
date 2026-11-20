// SPDX-License-Identifier: GPL-3.0-or-later
//
// Filmati a schermo intero - mod opzionale per MediEvilRecomp (PAL italiana, spagnola,
// francese, tedesca, inglese; NTSC-U americana)
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

// I filmati sono 320x176 dentro uno schermo PAL da 320x256 (4:3): sullo schermo il
// video ha un formato di circa 1,94:1 ed e' circondato da bande nere.
// Durante un filmato (lettore del gioco func_80079660, lettore dell'eseguibile di
// avvio func_801B0978 per l'introduzione) questa mod:
//  - restringe la finestra mostrata alle sole righe del video, modificando
//    disp.y, disp.h e screen.h della DISPENV solo per la durata di PutDispEnv
//    (poi rimette i valori del gioco);
//  - imposta il formato d'uscita a 4/3 * 256 / altezza del video, cosi' l'immagine
//    viene adattata alla finestra senza deformazioni.
// Finito il filmato rimette il formato d'uscita precedente.
//
// 1.2: se il filmato viene saltato molto in fretta, il gioco puo' non chiamare piu'
// PutDispEnv dopo il filmato e lo schermo restava tagliato alle righe del video
// (per esempio nel dialogo del gargoyle). A fine filmato la mod rimanda quindi la
// DISPENV originale, non ritagliata, all'ultima PutDispEnv modificata.
// Con MEDIEVIL_DEBUG impostata le righe "[FilmatiSchermoIntero]" del log mostrano
// inizio e fine dei filmati e ogni ritaglio.
//
// Versioni: l'eseguibile di avvio e' lo stesso in tutte; in MEDIEVIL.EXE le versioni
// francese, tedesca e inglese spostano il codice, quindi gli hook del gioco hanno un
// indirizzo per versione (dalla mappa degli indirizzi di tools/pal-port; PutDispEnv
// invece ha lo stesso nome, PutDispEnv_game, in tutte le versioni). Ognuno e'
// l'inizio di una funzione solo nella sua versione: gli altri vengono ignorati dal
// caricatore delle mod (riga "function not found" nella console). Senza questi hook
// la parte del gioco mancava e la fine dell'introduzione non rimetteva lo schermo.
// Versione americana SCUS-94227 (progetto BlackLabelHQ/MediEvilRecomp): altri eseguibili,
// hook propri (lettore del gioco func_800795E0 con descrittore a $gp+0x774, lettore di
// avvio func_801B0968 con descrittore a $gp+0x4C, PutDispEnv di avvio 0x801B2A7C) e schermo
// NTSC di 240 righe. Indirizzi dall'allineamento delle funcmap e dal codice generato.
// Testi del pannello: costante Texts qui sotto (chiavi mods.filmati.*).
// Il launcher copia la mod anche nel progetto americano, il cui RecompOne usa
// Hexa.NET.ImGui e RecompOne.UI.Localization.

using System;
using ImGuiNET;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Modding;

public class FilmatiSchermoInteroMod : IMod
{
    const string SettingKey = "mods.filmati-schermo-intero.attivo";
    const int PalLines = 256, NtscLines = 240;
    // righe dello schermo della versione in uso (240 nell'americana)
    static int _screenLines = PalLines;

    public static bool Enabled = true;

    // offset (rispetto a $gp) del puntatore al descrittore del filmato in corso:
    // descrittore +6 = y del video, +0xA = altezza. 0 = nessun filmato.
    // Un valore separato per ciascun eseguibile: il lettore dell'eseguibile di avvio
    // puo' non "tornare" mai (l'avvio passa direttamente a MEDIEVIL.EXE), quindi il suo
    // stato non deve mai essere usato per le PutDispEnv del gioco.
    static uint _gameInfoSlot, _mainInfoSlot;
    static bool _aspectChanged;
    static float _savedAspect;

    // valori della DISPENV da rimettere dopo PutDispEnv
    static uint _env;
    static ushort _savedDispY, _savedDispH, _savedScrH;
    static bool _patched;

    // copia della DISPENV originale dell'ultimo ritaglio, per rimandarla a fine filmato
    static readonly byte[] _envCopy = new byte[0x14];
    static uint _envCopyAddr;
    static bool _croppedThisMovie;

    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static int _lines;
    static int _afterMovieLogs;   // PutDispEnv da registrare dopo la fine di un filmato
    static void Diag(string s)
    {
        if (DiagOn && _lines++ < 300) Console.WriteLine("[FilmatiSchermoIntero] " + s);
    }
    static string T(string key) => RecompOne.Runtime.Host.Window.Localization.T(key);
    static string _info = "";

    // testi del pannello in tutte le lingue: la mod li aggiunge a quelli del gioco al
    // caricamento (Localization.Merge), cosi' funzionano anche dove assets/languages.json
    // del progetto non li ha (versione americana)
    const string Texts = """
        {
            "strings": {
                "mods.filmati.toggle": {
                    "en": "Fullscreen movies",
                    "it-IT": "Filmati a schermo intero",
                    "fr-fr": "Cinématiques en plein écran",
                    "es-419": "Vídeos a pantalla completa",
                    "de-DE": "Filme im Vollbild",
                    "pt-BR": "Vídeos em tela cheia"
                },
                "mods.filmati.none": {
                    "en": "no movie",
                    "it-IT": "nessun filmato",
                    "fr-fr": "aucune cinématique",
                    "es-419": "ningún vídeo",
                    "de-DE": "kein Film",
                    "pt-BR": "nenhum vídeo"
                },
                "mods.filmati.info": {
                    "en": "movie {0} lines from y={1}, aspect {2}:1",
                    "it-IT": "filmato {0} righe da y={1}, formato {2}:1",
                    "fr-fr": "cinématique {0} lignes depuis y={1}, format {2}:1",
                    "es-419": "vídeo de {0} líneas desde y={1}, formato {2}:1",
                    "de-DE": "Film {0} Zeilen ab y={1}, Format {2}:1",
                    "pt-BR": "vídeo de {0} linhas a partir de y={1}, formato {2}:1"
                }
            }
        }
        """;

    public void OnLoad()
    {
        RecompOne.Runtime.Host.Window.Localization.Merge(Texts);
        Enabled = RecompOne.Runtime.Config.Settings.Get<string>(SettingKey, "1") != "0";
    }

    public void OnUnload()
    {
        _gameInfoSlot = _mainInfoSlot = 0;
        RestoreAspect();
    }

    public void DrawSettings()
    {
        if (ImGui.Checkbox(T("mods.filmati.toggle"), ref Enabled))
            RecompOne.Runtime.Config.Settings.Set(SettingKey, Enabled ? "1" : "0");
        ImGui.TextDisabled(_info.Length > 0 ? _info : T("mods.filmati.none"));
    }

    // lettore filmati del gioco (MEDIEVIL.EXE, func_80079660 nell'italiana): descrittore a
    // $gp+GpMovieInfo, diverso per versione. La versione si riconosce dalle tabelle dei
    // trucchi, dati costanti dell'eseguibile (config/versions.json: CheatTableDust,
    // CheatTableExtended, GpMovieInfo).
    // [0] italiana SCES-01494, [1] spagnola SCES-01495, [2] francese SCES-01492,
    // [3] tedesca SCES-01493, [4] inglese SCES-00311
    static readonly uint[] TableDust = { 0x800CE0B4u, 0x800CE09Cu, 0x800CE0C4u, 0x800CDFA8u, 0x800CE01Cu };
    static readonly uint[] TableExtended = { 0x800CE0DCu, 0x800CE0C4u, 0x800CE0ECu, 0x800CDFD0u, 0x800CE044u };
    static readonly uint[] GpMovieInfo = { 0x764u, 0x75Cu, 0x748u, 0x758u, 0x774u };

    [PreHook("game", Address = 0x80079660u)]   // italiana, spagnola
    [PreHook("game", Address = 0x800796C8u)]   // francese
    [PreHook("game", Address = 0x800795F8u)]   // tedesca
    [PreHook("game", Address = 0x80079658u)]   // inglese
    public static void GameMovieStart(CpuContext c, IMemory m)
    {
        _gameInfoSlot = 0;
        _screenLines = PalLines;
        for (int i = 0; i < TableDust.Length; i++)
            if (m.ReadU32(TableDust[i]) == 0x00080000u && m.ReadU32(TableExtended[i]) == 0x00100000u)
            {
                _gameInfoSlot = GpMovieInfo[i];
                break;
            }
        Diag($"inizio filmato (gioco), slot {_gameInfoSlot:X}");
    }

    [PreHook("game", Address = 0x800795E0u)]   // americana
    public static void GameMovieStartUsa(CpuContext c, IMemory m)
    {
        _gameInfoSlot = 0x774u;
        _screenLines = NtscLines;
        Diag("inizio filmato (gioco, americana)");
    }

    [PostHook("game", Address = 0x80079660u)]   // italiana, spagnola
    [PostHook("game", Address = 0x800796C8u)]   // francese
    [PostHook("game", Address = 0x800795F8u)]   // tedesca
    [PostHook("game", Address = 0x80079658u)]   // inglese
    [PostHook("game", Address = 0x800795E0u)]   // americana
    public static void GameMovieEnd(CpuContext c, IMemory m)
    {
        _gameInfoSlot = 0;
        Diag("fine filmato (gioco)");
        MovieEnd(c, m);
    }

    // lettore filmati dell'eseguibile di avvio (introduzione): descrittore a $gp+0x48
    [PreHook("main", Address = 0x801B0978u)]
    public static void MainMovieStart(CpuContext c, IMemory m)
    {
        _mainInfoSlot = 0x48u;
        _screenLines = PalLines;
        Diag("inizio filmato (avvio)");
    }

    // americana: descrittore a $gp+0x4C
    [PreHook("main", Address = 0x801B0968u)]
    public static void MainMovieStartUsa(CpuContext c, IMemory m)
    {
        _mainInfoSlot = 0x4Cu;
        _screenLines = NtscLines;
        Diag("inizio filmato (avvio, americana)");
    }

    [PostHook("main", Address = 0x801B0978u)]
    [PostHook("main", Address = 0x801B0968u)]   // americana
    public static void MainMovieEnd(CpuContext c, IMemory m)
    {
        _mainInfoSlot = 0;
        Diag("fine filmato (avvio)");
        MovieEnd(c, m);
    }

    static void MovieEnd(CpuContext c, IMemory m)
    {
        RestoreAspect();
        _info = "";
        Diag($"fine filmato: ritaglio fatto={_croppedThisMovie}, dispenv={_envCopyAddr:X8}");
        if (_croppedThisMovie) ReissueDisplay(c, m);
        _croppedThisMovie = false;
        _afterMovieLogs = 12;
    }

    // Rimanda a PutDispEnv la DISPENV originale dell'ultimo ritaglio: usa per un istante
    // la sua posizione in memoria e poi rimette quello che c'era.
    static void ReissueDisplay(CpuContext c, IMemory m)
    {
        uint addr = _envCopyAddr;
        if ((addr & 0xFF000000u) != 0x80000000u) return;

        var saved = new byte[_envCopy.Length];
        for (int i = 0; i < saved.Length; i++) saved[i] = m.ReadU8(addr + (uint)i);
        for (int i = 0; i < _envCopy.Length; i++) m.WriteU8(addr + (uint)i, _envCopy[i]);
        if (_screenLines == PalLines && m.ReadU16(addr + 0xEu) == 0)
            m.WriteU16(addr + 0xEu, PalLines);   // come PalDisplay (solo nel progetto PAL)

        uint a0 = c.A0, v0 = c.V0;
        c.A0 = addr;
        RecompOne.Runtime.Sdk.LibGpu.PutDispEnv(c, m);
        c.A0 = a0;
        c.V0 = v0;

        for (int i = 0; i < saved.Length; i++) m.WriteU8(addr + (uint)i, saved[i]);
        Diag($"schermo ripristinato: disp y={m.ReadU16(addr + 2u)} h={m.ReadU16(addr + 6u)}");
    }

    // PutDispEnv del gioco (0x8009C990 nell'italiana): stesso nome in tutte le versioni
    [PreHook("game", "PutDispEnv_game")]
    public static void GameBefore(CpuContext c, IMemory m)
    {
        if (_mainInfoSlot != 0)
        {
            // il gioco e' partito: il lettore di avvio non e' piu' attivo e questa
            // PutDispEnv del gioco reimposta comunque lo schermo
            _mainInfoSlot = 0;
            if (_gameInfoSlot == 0) _croppedThisMovie = false;
        }
        BeforePutDispEnv(c, m, _gameInfoSlot);
    }

    [PostHook("game", "PutDispEnv_game")]
    public static void GameAfter(CpuContext c, IMemory m) => AfterPutDispEnv(m);

    [PreHook("main", Address = 0x801B2A8Cu)]
    [PreHook("main", Address = 0x801B2A7Cu)]   // americana
    public static void MainBefore(CpuContext c, IMemory m) => BeforePutDispEnv(c, m, _mainInfoSlot);

    [PostHook("main", Address = 0x801B2A8Cu)]
    [PostHook("main", Address = 0x801B2A7Cu)]   // americana
    public static void MainAfter(CpuContext c, IMemory m) => AfterPutDispEnv(m);

    static void BeforePutDispEnv(CpuContext c, IMemory m, uint infoSlot)
    {
        _patched = false;
        if (DiagOn && _afterMovieLogs > 0)
        {
            _afterMovieLogs--;
            uint e = c.A0;
            Diag($"PutDispEnv {e:X8} slot={infoSlot:X}: disp {m.ReadU16(e)},{m.ReadU16(e + 2u)} {m.ReadU16(e + 4u)}x{m.ReadU16(e + 6u)} screen y={m.ReadU16(e + 0xAu)} h={m.ReadU16(e + 0xEu)}");
        }
        if (!Enabled || infoSlot == 0)
        {
            RestoreAspect();
            _info = "";
            return;
        }

        uint info = m.ReadU32(c.GP + infoSlot);
        if ((info & 0xFF000000u) != 0x80000000u) return;
        int y = m.ReadU16(info + 6u);
        int h = m.ReadU16(info + 0xAu);
        if (h <= 0 || h >= _screenLines || y + h > _screenLines) return;

        uint env = c.A0;
        _env = env;
        for (int i = 0; i < _envCopy.Length; i++) _envCopy[i] = m.ReadU8(env + (uint)i);
        _envCopyAddr = env;
        if (!_croppedThisMovie) Diag($"ritaglio: video {h} righe da y={y}");
        _croppedThisMovie = true;
        _savedDispY = m.ReadU16(env + 0x2u);
        _savedDispH = m.ReadU16(env + 0x6u);
        _savedScrH = m.ReadU16(env + 0xEu);
        m.WriteU16(env + 0x2u, (ushort)(_savedDispY + y));   // disp.y: prima riga del video
        m.WriteU16(env + 0x6u, (ushort)h);                    // disp.h: altezza del video
        m.WriteU16(env + 0xEu, (ushort)h);                    // screen.h: solo le righe del video
        _patched = true;

        float aspect = 4f / 3f * _screenLines / h;
        if (!_aspectChanged)
        {
            _savedAspect = Display.OutputAspect;
            _aspectChanged = true;
        }
        Display.OutputAspect = aspect;
        _info = string.Format(T("mods.filmati.info"), h, y, aspect.ToString("0.00"));
    }

    static void AfterPutDispEnv(IMemory m)
    {
        if (!_patched) return;
        m.WriteU16(_env + 0x2u, _savedDispY);
        m.WriteU16(_env + 0x6u, _savedDispH);
        m.WriteU16(_env + 0xEu, _savedScrH);
        _patched = false;
    }

    static void RestoreAspect()
    {
        if (!_aspectChanged) return;
        Display.OutputAspect = _savedAspect;
        _aspectChanged = false;
    }
}
