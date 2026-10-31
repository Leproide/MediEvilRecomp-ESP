// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - altezza dello schermo PAL
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

// Filmati spostati in basso.
//
// PutDispEnv di RecompOne, quando DISPENV.screen.h vale 0 ("altezza predefinita"),
// usa 240 righe anche in modalita' PAL, dove sono 256. Il lettore dei filmati PAL
// mette il video (320x176) a y=40, centrato su 256 righe: con 240 righe mostrate il
// filmato risultava spostato in basso e tagliato sotto.
// Solo per la durata di PutDispEnv mettiamo screen.h = 256 quando vale 0, poi
// rimettiamo il valore del gioco.
//
// Schermo tagliato in basso (es. dialogo del gargoyle dopo lo skip di un filmato):
// PutDispEnv di RecompOne calcola le righe visibili in base al modo video registrato
// da SetVideoMode. Se a un certo punto risulta NTSC (modo 0), le 256 righe PAL vengono
// limitate a 226 (righe 32..258 del modo NTSC) e l'immagine perde la parte sotto.
// Questo e' il disco PAL: prima di ogni PutDispEnv rimettiamo il modo PAL.

using System.Reflection;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalDisplay
{
    const int PalLines = 256;
    static uint _env;
    static bool _patched;

    // modo video di LibGpu (privato): -1 non ancora deciso, 0 NTSC, 1 PAL
    static readonly FieldInfo? VideoMode =
        typeof(RecompOne.Runtime.Sdk.LibGpu).GetField("_videoMode", BindingFlags.Static | BindingFlags.NonPublic);
    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static int _ntscFixes;

    static void ForcePal()
    {
        if (VideoMode?.GetValue(null) is not int mode || mode == 1) return;
        VideoMode.SetValue(null, 1);
        if (DiagOn && mode == 0 && _ntscFixes++ < 20)
            Console.WriteLine("[PalDisplay] modo video NTSC trovato prima di PutDispEnv: rimesso PAL");
    }

    // pre su PutDispEnv (overlay "game" ed eseguibile di avvio)
    public static void BeforePutDispEnv(CpuContext c, IMemory m)
    {
        ForcePal();
        _patched = false;
        uint env = c.A0;
        if (m.ReadU16(env + 0xE) != 0) return;
        m.WriteU16(env + 0xE, PalLines);
        _env = env;
        _patched = true;
    }

    // post su PutDispEnv
    public static void AfterPutDispEnv(CpuContext c, IMemory m)
    {
        if (!_patched) return;
        m.WriteU16(_env + 0xE, 0);
        _patched = false;
    }
}
