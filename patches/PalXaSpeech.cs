// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - fine traccia dei dialoghi XA
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


// Il primo discorso del gargoyle nella cripta si chiudeva subito.
//
// Le tracce audio XA del gioco (musica e voci) finiscono con un settore dati
// "marcatore" (intestazione 0x0160) che indica il canale terminato. Il callback dati
// del gioco (func_8006D7A4) lo confronta con il canale della traccia corrente
// ($gp+0x6C4) e, se coincide, segnala "traccia finita" e il discorso viene chiuso.
//
// Quando un discorso parte mentre suona la musica del livello, il gioco prima sceglie
// il nuovo canale (func_8006D0D8) e solo dopo sposta la lettura sul file della voce
// (ReadS). In quell'intervallo RecompOne continua a consegnare i settori della musica:
// un marcatore di fine con lo stesso numero di canale veniva preso per la fine del
// discorso. Tra la scelta di una nuova traccia e il comando di lettura che la avvia,
// il controllo del marcatore viene saltato. Verificato in gioco: il discorso arriva
// fino in fondo e la musica riparte normalmente.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalXaSpeech
{
    const byte CdlReadN = 0x06, CdlReadS = 0x1B;

    static volatile bool _pending;   // nuova traccia scelta, lettura non ancora avviata

    // post su func_8006D0D8: scelta di file/canale della nuova traccia XA
    public static void TrackSelected(CpuContext c, IMemory m) => _pending = true;

    // pre su CdControl / CdControlF / CdControlB: un comando di lettura avvia la traccia
    public static void OnCdCommand(CpuContext c, IMemory m)
    {
        byte com = (byte)c.A0;
        if (com == CdlReadS || com == CdlReadN) _pending = false;
    }

    // pre su func_8006D7A4 (callback dati del lettore XA): false = salta la funzione
    public static bool DataCallback(CpuContext c, IMemory m) => !_pending;
}
