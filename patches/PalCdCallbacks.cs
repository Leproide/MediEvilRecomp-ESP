// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - correzioni dei callback del CD
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

// Due differenze tra l'HLE di libcd in RecompOne e la libcd originale del gioco,
// che dopo la Sala degli Eroi rendevano la mappa lentissima (1 fotogramma ogni ~1,2 s)
// e poi bloccavano il caricamento del livello.
//
// 1) StUnSetRing originale chiama CdDataCallback(0) e CdReadyCallback(0) (salvo la
//    modalita' "emulate"). Quello HLE no: dopo un filmato resta installato
//    StCdInterrupt come callback "ready" e viene richiamato centinaia di migliaia
//    di volte al secondo.
//
// 2) Il controllo LibCrypt (0x80065D20..0x80065FC0, vedi PalLibCrypt.cs) scrive i propri callback
//    direttamente nelle variabili di libcd (CD_cbsync ITA 0x800EB660 / ESP 0x800EB6E0,
//    CD_cbready ITA 0x800EB664 / ESP 0x800EB6E4) invece di usare CdSyncCallback/CdReadyCallback. L'HLE conserva i
//    callback in campi C# e non vedeva queste scritture: la macchina a stati restava
//    ferma e il gioco aspettava in eterno prima di caricare il livello.
//
// Le funzioni qui sotto sono agganciate come "post" in config/MediEvil.json e
// tengono allineati i due mondi. Verificato in gioco con la mod di diagnostica
// DiagMappa 0.4 (mappa fluida, livello caricato, macchina a stati che arriva in fondo).

using System.Reflection;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalCdCallbacks
{
    // Variabili della copia di libcd dentro MEDIEVIL.EXE (overlay "game"): indirizzi
    // per versione in GameAddresses.cs (config/versions.json).
    const uint SlotSync = Addr.CdCbSync;        // CD_cbsync
    const uint SlotReady = Addr.CdCbReady;      // CD_cbready
    const uint EmulateFlag = Addr.CdEmulateFlag; // se vale 1 lo StUnSetRing originale non tocca i callback

    // Campi privati di RecompOne.Runtime.Sdk.LibCd (letti via reflection per non
    // dover modificare RecompOne).
    static readonly FieldInfo? CbReady, CbSync, CbData;

    static PalCdCallbacks()
    {
        var t = typeof(IMemory).Assembly.GetType("RecompOne.Runtime.Sdk.LibCd");
        const BindingFlags F = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        CbReady = t?.GetField("_cbReady", F);
        CbSync = t?.GetField("_cbSync", F);
        CbData = t?.GetField("_cbData", F);
        if (CbReady == null || CbSync == null || CbData == null)
            Console.WriteLine("[PalCdCallbacks] ATTENZIONE: campi di LibCd non trovati, correzione CD inattiva");
    }

    static uint Get(FieldInfo? f) => f?.GetValue(null) is uint v ? v : 0u;
    static void Set(FieldInfo? f, uint v) => f?.SetValue(null, v);

    // Dopo le funzioni del controllo LibCrypt: il gioco ha scritto CD_cbsync/CD_cbready
    // in memoria, li copio nell'HLE.
    public static void SyncFromMemory(CpuContext c, IMemory m)
    {
        Set(CbReady, m.ReadU32(SlotReady));
        Set(CbSync, m.ReadU32(SlotSync));
    }

    // Dopo CdSyncCallback/CdReadyCallback (HLE): aggiorno le variabili del gioco,
    // che il codice PAL della mappa legge direttamente.
    public static void SyncToMemory(CpuContext c, IMemory m)
    {
        m.WriteU32(SlotReady, Get(CbReady));
        m.WriteU32(SlotSync, Get(CbSync));
    }

    // Dopo StUnSetRing dell'overlay "game": come l'originale.
    public static void AfterStUnSetRingGame(CpuContext c, IMemory m)
    {
        if (m.ReadU32(EmulateFlag) == 1) return;
        Set(CbData, 0);
        Set(CbReady, 0);
        m.WriteU32(SlotReady, 0);
    }

    // Dopo StUnSetRing dell'eseguibile di avvio (filmati iniziali).
    public static void AfterStUnSetRingMain(CpuContext c, IMemory m)
    {
        Set(CbData, 0);
        Set(CbReady, 0);
    }
}
