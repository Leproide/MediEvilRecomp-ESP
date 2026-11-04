// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - memoria del gioco ampliata
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

// Problema: la memoria dinamica del gioco (poco piu' di 100 KB liberi in un livello) basta per gli
// oggetti che l'originale carica attorno a Dan, ma non per quelli che il fork carica da lontano
// (DistantObjectsPatch) nei livelli grandi: a Scarecrow Fields a 3x si arriva a ~80 oggetti, la
// memoria finisce, l'allocatore restituisce 0 e il gioco crasha dove usa quel puntatore nullo
// (distruzione degli oggetti, testi a schermo...).
//
// Come funziona l'allocatore del gioco:
//  - func_800A4028(base, dimensione) prepara la memoria una sola volta all'avvio: 12 liste libere
//    per dimensione a 0x800F3B08 (0x14 byte l'una, sentinella con dimensione 0x7FFFFFFF) e un
//    unico blocco libero grande quanto tutta la memoria;
//  - ogni blocco ha un'intestazione di 0x14 byte: +0x0 blocco fisico precedente, +0x4 successivo
//    (0 = nessuno), +0x8/+0xC collegamenti nella lista libera, +0x10 dimensione (positiva se
//    libero, negativa se occupato); i dati seguono l'intestazione;
//  - func_800A40B8 (alloca) cerca nelle liste e divide il blocco; func_800A4330 (libera) unisce il
//    blocco ai vicini fisici liberi; func_800A4264 inserisce un blocco libero nella sua lista.
//
// Correzione: la memoria emulata passa da 4 a 8 MB (Program.cs) e, subito dopo la preparazione
// della memoria del gioco, un secondo blocco libero di 2 MB a 0x80400000 (oltre le zone usate da
// PolyPoolPatch e TerrainPatch, 0x80200000-0x80400000) viene inserito nelle liste con la funzione
// del gioco. Non ha vicini fisici (+0x0 e +0x4 a 0), quindi non viene mai unito alla memoria
// originale: l'allocatore lo usa e lo divide come gli altri blocchi. Il codice del gioco non
// controlla che i puntatori stiano sotto i 2 MB (verificato nel codice generato); i controlli
// "indirizzo valido" delle patch del fork usano la dimensione reale della memoria.
// Con MEDIEVIL_DEBUG: riga "[HeapExpansion]" quando il blocco viene aggiunto.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class HeapExpansion
{
    public const uint Base = 0x80400000u;
    public const uint Size = 0x00200000u;   // 2 MB

    const uint HeaderSize = 0x14u;
    // liste libere per versione (config/versions.json): ITA 0x800F3B08, ESP 0x800F3B80
    const uint FreeBins = Addr.HeapFreeBins;
    const int BinCount = 12;

    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));

    // dopo func_800A4028 (preparazione della memoria del gioco)
    public static void AfterInitHeap(CpuContext c, IMemory m)
    {
        if (RecompOne.Runtime.Runtime.RamSize < (Base & 0x00FFFFFFu) + Size)
        {
            Console.WriteLine($"[HeapExpansion] memoria emulata troppo piccola ({RecompOne.Runtime.Runtime.RamSize:X}), blocco non aggiunto");
            return;
        }

        // intestazione del blocco libero: nessun vicino fisico, dimensione utile positiva
        m.WriteU32(Base + 0x0u, 0u);
        m.WriteU32(Base + 0x4u, 0u);
        m.WriteU32(Base + 0x8u, 0u);
        m.WriteU32(Base + 0xCu, 0u);
        m.WriteU32(Base + 0x10u, Size - HeaderSize);

        // inserimento con la funzione del gioco (func_800A4264, A0 = intestazione)
        uint a0 = c.A0, a1 = c.A1, a2 = c.A2, a3 = c.A3, v0 = c.V0, v1 = c.V1, ra = c.RA, sp = c.SP;
        c.A0 = Base;
        MediEvil_game.func_800A4264(c, m);
        c.A0 = a0; c.A1 = a1; c.A2 = a2; c.A3 = a3; c.V0 = v0; c.V1 = v1; c.RA = ra; c.SP = sp;

        if (DiagOn)
            Console.WriteLine($"[HeapExpansion] aggiunti {Size / 1024} KB a {Base:X8}, memoria libera {FreeBytes(m)} byte");
    }

    // somma dei blocchi nelle liste libere (anche per la protezione di DistantObjectsPatch)
    public static long FreeBytes(IMemory m)
    {
        long total = 0;
        for (uint i = 0; i < BinCount; i++)
        {
            uint head = FreeBins + i * 0x14u;
            uint block = m.ReadU32(head + 0xCu);
            for (int guard = 0; guard < 100000 && IsRam(block) && block != head; guard++)
            {
                total += (int)m.ReadU32(block + 0x10u);
                block = m.ReadU32(block + 0xCu);
            }
        }

        return total;
    }

    public static bool IsRam(uint address) =>
        (address & 0xFF000000u) == 0x80000000u && (address & 0x00FFFFFFu) < RecompOne.Runtime.Runtime.RamSize;
}
