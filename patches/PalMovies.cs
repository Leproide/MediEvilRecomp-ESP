// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - attesa dei fotogrammi dei filmati
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

// Issue #5 di MediEvilRecomp: filmati saltati dopo un secondo o due.
//
// Il lettore dei filmati (func_80079D90 nella versione PAL) chiede il fotogramma
// successivo con StGetNext in un ciclo di attesa attiva e, dopo 0x400000 tentativi
// a vuoto, considera il filmato finito. Su PlayStation quei tentativi durano alcuni
// secondi; ricompilati su un PC moderno una frazione di secondo, mentre RecompOne
// consegna i fotogrammi a velocita' reale da un thread separato. Basta un piccolo
// ritardo e il filmato viene chiuso. Piu' il processore e' veloce, piu' succede.
//
// Questa funzione sostituisce l'originale con la stessa logica, ma con il limite
// di attesa misurato in tempo reale (2 secondi).

using System.Diagnostics;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalMovies
{
    const uint PlayerState = Addr.MoviePlayerState; // struttura del lettore filmati (per versione)
    const uint EndFlag = PlayerState + 0x40u;  // 1 = filmato finito
    const int TimeoutMs = 2000;

    // Sostituisce func_80079D90: v0 = puntatore ai dati del fotogramma,
    // oppure 0 con il filmato segnato come finito.
    public static void func_80079D90(CpuContext c, IMemory m)
    {
        // stesse posizioni sullo stack del codice originale (sp-0x28+0x10/0x14)
        uint pData = c.SP - 0x28u + 0x10u;
        uint pHdr = c.SP - 0x28u + 0x14u;

        var clock = Stopwatch.StartNew();
        while (true)
        {
            c.A0 = pData;
            c.A1 = pHdr;
            RecompOne.Runtime.Sdk.LibCdStream.StGetNext(c, m);
            if (c.V0 == 0) break;

            if (clock.ElapsedMilliseconds > TimeoutMs)
            {
                m.WriteU32(EndFlag, 1);
                c.V0 = 0;
                return;
            }

            // mentre aspetta lascia girare interrupt e callback, come la console
            RecompOne.Runtime.Interrupts.PollNow(c, m);
            Thread.Sleep(0);
        }

        uint gp = c.GP;
        uint hdr = m.ReadU32(pHdr);
        uint info = m.ReadU32(gp + Addr.GpMovieInfo);
        uint prev = m.ReadU32(gp + Addr.GpMovieFrame);
        uint frame = m.ReadU32(hdr + 8u);
        uint frameCount = m.ReadU16(info + 0xCu);

        m.WriteU32(gp + Addr.GpMoviePrevFrame, prev);
        m.WriteU32(gp + Addr.GpMovieFrame, frame);

        // numero di fotogramma oltre la fine o tornato indietro: filmato finito
        if (frame >= frameCount || frame < prev) m.WriteU32(EndFlag, 1);
        // dimensioni diverse da quelle attese: filmato finito
        if (m.ReadU32(hdr + 0x10u) != m.ReadU32(info + 8u)) m.WriteU32(EndFlag, 1);

        c.V0 = m.ReadU32(pData);
    }
}
