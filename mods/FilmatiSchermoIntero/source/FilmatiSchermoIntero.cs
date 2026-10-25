// SPDX-License-Identifier: GPL-3.0-or-later
//
// Filmati a schermo intero - mod opzionale per MediEvilRecomp ITA
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

using System;
using ImGuiNET;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Modding;

public class FilmatiSchermoInteroMod : IMod
{
    const string SettingKey = "mods.filmati-schermo-intero.attivo";
    const int PalLines = 256;

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
    static string _info = "nessun filmato";

    public void OnLoad()
    {
        Enabled = RecompOne.Runtime.Runtime.View.GetString(SettingKey, "1") != "0";
    }

    public void OnUnload()
    {
        _gameInfoSlot = _mainInfoSlot = 0;
        RestoreAspect();
    }

    public void DrawSettings()
    {
        if (ImGui.Checkbox("Filmati a schermo intero", ref Enabled))
            RecompOne.Runtime.Runtime.View.SetString(SettingKey, Enabled ? "1" : "0");
        ImGui.TextDisabled(_info);
    }

    // lettore filmati del gioco (MEDIEVIL.EXE): descrittore a $gp+0x764 nella versione
    // italiana, $gp+0x75C nella spagnola (stesso codice, dati spostati di 8 byte rispetto
    // a $gp). La versione si riconosce dalla tabella dei trucchi, dato costante
    // dell'eseguibile (valori in config/versions.json: CheatTableDust, GpMovieInfo).
    [PreHook("game", Address = 0x80079660u)]
    public static void GameMovieStart(CpuContext c, IMemory m) =>
        _gameInfoSlot = m.ReadU32(0x800CE09Cu) == 0x00080000u && m.ReadU32(0x800CE0C4u) == 0x00100000u
            ? 0x75Cu   // PAL spagnola (SCES-01495)
            : 0x764u;  // PAL italiana (SCES-01494)

    [PostHook("game", Address = 0x80079660u)]
    public static void GameMovieEnd(CpuContext c, IMemory m) { _gameInfoSlot = 0; MovieEnd(); }

    // lettore filmati dell'eseguibile di avvio (introduzione): descrittore a $gp+0x48
    [PreHook("main", Address = 0x801B0978u)]
    public static void MainMovieStart(CpuContext c, IMemory m) => _mainInfoSlot = 0x48u;

    [PostHook("main", Address = 0x801B0978u)]
    public static void MainMovieEnd(CpuContext c, IMemory m) { _mainInfoSlot = 0; MovieEnd(); }

    static void MovieEnd()
    {
        RestoreAspect();
        _info = "nessun filmato";
    }

    [PreHook("game", Address = 0x8009C990u)]
    public static void GameBefore(CpuContext c, IMemory m)
    {
        _mainInfoSlot = 0;   // il gioco e' partito: il lettore di avvio non e' piu' attivo
        BeforePutDispEnv(c, m, _gameInfoSlot);
    }

    [PostHook("game", Address = 0x8009C990u)]
    public static void GameAfter(CpuContext c, IMemory m) => AfterPutDispEnv(m);

    [PreHook("main", Address = 0x801B2A8Cu)]
    public static void MainBefore(CpuContext c, IMemory m) => BeforePutDispEnv(c, m, _mainInfoSlot);

    [PostHook("main", Address = 0x801B2A8Cu)]
    public static void MainAfter(CpuContext c, IMemory m) => AfterPutDispEnv(m);

    static void BeforePutDispEnv(CpuContext c, IMemory m, uint infoSlot)
    {
        _patched = false;
        if (!Enabled || infoSlot == 0)
        {
            RestoreAspect();
            _info = "nessun filmato";
            return;
        }

        uint info = m.ReadU32(c.GP + infoSlot);
        if ((info & 0xFF000000u) != 0x80000000u) return;
        int y = m.ReadU16(info + 6u);
        int h = m.ReadU16(info + 0xAu);
        if (h <= 0 || h >= PalLines || y + h > PalLines) return;

        uint env = c.A0;
        _env = env;
        _savedDispY = m.ReadU16(env + 0x2u);
        _savedDispH = m.ReadU16(env + 0x6u);
        _savedScrH = m.ReadU16(env + 0xEu);
        m.WriteU16(env + 0x2u, (ushort)(_savedDispY + y));   // disp.y: prima riga del video
        m.WriteU16(env + 0x6u, (ushort)h);                    // disp.h: altezza del video
        m.WriteU16(env + 0xEu, (ushort)h);                    // screen.h: solo le righe del video
        _patched = true;

        float aspect = 4f / 3f * PalLines / h;
        if (!_aspectChanged)
        {
            _savedAspect = Display.OutputAspect;
            _aspectChanged = true;
        }
        Display.OutputAspect = aspect;
        _info = $"filmato {h} righe da y={y}, formato {aspect:0.00}:1";
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
