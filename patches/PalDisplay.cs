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

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalDisplay
{
    const int PalLines = 256;
    static uint _env;
    static bool _patched;

    // pre su PutDispEnv (overlay "game" ed eseguibile di avvio)
    public static void BeforePutDispEnv(CpuContext c, IMemory m)
    {
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
