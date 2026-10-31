// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA/ESP - sottocodice Q dei settori LibCrypt (file .sbi / .lsd)
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

// Perche' serve
// -------------
// La versione PAL usa la protezione LibCrypt. Il codice a 0x80065D20..0x80065FC0
// (avviato dalla mappa: LANDMAP 0x80011EA0 ITA / 0x80011E98 ESP) legge con GetlocP la
// posizione di 16 settori del disco (tabella a 0x800DB2B4 ITA / 0x800DB334 ESP).
// Il codice e' identico nelle versioni italiana e spagnola: cambiano solo gli indirizzi
// dei dati (+0x80 nella spagnola), presi da GameAddresses.cs (config/versions.json). Sul disco originale 8 di questi hanno il
// sottocodice Q modificato con CRC errato: il lettore lo scarta e continua a riportare
// la posizione del settore precedente, quindi il gioco vede il fotogramma "saltare"
// (bit 1); sugli altri vede il fotogramma atteso (bit 0). Dai 16 bit ricava una chiave
// (0x800DB2AC ITA / 0x800DB32C ESP) con cui decifra il primo settore dei dati del Mausoleo in MEDRES.MWD
// (voce 0xD6, func_80065FC4). Senza "PP20" dopo la decifratura il gioco resta sulla
// schermata con il CD e il punto esclamativo.
//
// Un dump bin/cue non contiene il sottocodice: RecompOne riporta sempre la posizione
// esatta, la chiave risulta 0 e il Mausoleo non si apre.
//
// Questa patch riproduce il comportamento del lettore: quando il gioco controlla un
// settore, GetlocP riporta il settore stesso, oppure il successivo se e' uno di quelli
// con Q modificato (RecompOne da solo avanza di piu' settori tra due letture e il
// bersaglio non compare mai). L'elenco dei settori viene dal file .sbi/.lsd accanto al .cue (stesso
// nome, come per gli emulatori) oppure, per i dischi SCES-01494 e SCES-01495, dall'elenco
// incluso qui sotto (gli stessi 16 settori dei file .sbi Redump dei dischi).
//
// Chiave attesa con l'elenco incluso: ITA 0xAAA6, ESP 0x0E57 (8 bit a 1 nei primi 16
// controlli; verificabile con tools/pal-port/libcrypt_check.py).
//
// Log: la riga con l'elenco dei settori usato compare sempre. Il dettaglio di ogni
// controllo e della decifratura compare sempre nella versione italiana e, nella
// spagnola, solo con MEDIEVIL_DEBUG impostata (es. MEDIEVIL_DEBUG=libcrypt, oppure all).
//
// Agganci (config/MediEvil.json, mode "pre"): 0x80065E0C e 0x80065E8C, i due
// callback che ricevono in $a1 il risultato di GetlocP (8 byte:
// traccia, indice, mm ss ff relativi, mm ss ff assoluti).

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PalLibCrypt
{
    // Disco della versione compilata (patches/GameAddresses.cs): SCES-01494 o SCES-01495.
    const string GameId = GameVersion.Serial;

    // Settori con sottocodice Q modificato (LBA = MSF - 150), dai file .sbi Redump.
    static readonly int[] BuiltInIta =   // SCES-01494
    {
        13955, 14335, 14499, 14906, 15092, 15228, 15881, 15951,
        41895, 42282, 42521, 42862, 43139, 43258, 43904, 44009,
    };

    static readonly int[] BuiltInEsp =   // SCES-01495
    {
        14499, 14749, 14906, 15162, 15478, 15881, 15951, 16017,
        42521, 42663, 42862, 43204, 43484, 43904, 44009, 44162,
    };

    static readonly int[] BuiltIn = GameVersion.Spanish ? BuiltInEsp : BuiltInIta;

    static HashSet<int>? _lba;
    static readonly object _gate = new();

    static int Bcd(byte b) => (b >> 4) * 10 + (b & 0x0F);
    static byte ToBcd(int v) => (byte)(((v / 10) << 4) | (v % 10));
    static int Msf(byte mm, byte ss, byte ff) => (Bcd(mm) * 60 + Bcd(ss)) * 75 + Bcd(ff);

    static void WriteMsf(IMemory m, uint addr, int frames)
    {
        if (frames < 0) frames = 0;
        m.WriteU8(addr + 0, ToBcd(frames / 75 / 60));
        m.WriteU8(addr + 1, ToBcd(frames / 75 % 60));
        m.WriteU8(addr + 2, ToBcd(frames % 75));
    }

    // Indirizzi per versione (ITA / ESP) in patches/GameAddresses.cs
    const uint KeyAddr = Addr.LcKey;          // chiave a 16 bit
    const uint CountAddr = Addr.LcCount;      // numero di bit a 1
    const uint TablePtr = Addr.LcTablePtr;    // voce corrente della tabella dei 16 settori
    const uint TableBase = Addr.LcTableBase;

    // Log diagnostici: righe "[PalLibCrypt]" nella console / debug.txt. Nella versione
    // italiana sempre attivi (come prima); nella spagnola solo con la variabile
    // d'ambiente MEDIEVIL_DEBUG impostata (qualsiasi valore).
    static readonly bool Verbose = !GameVersion.Spanish ||
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static int _logLines;
    static void Diag(string s)
    {
        if (Verbose && _logLines++ < 400) Console.WriteLine("[PalLibCrypt] " + s);
    }

    static string Hex(IMemory m, uint a, int n)
    {
        var b = new byte[n];
        for (var i = 0; i < n; i++) b[i] = m.ReadU8(a + (uint)i);
        return Convert.ToHexString(b);
    }

    // pre su 0x80065E0C (primo GetlocP dopo ReadS)
    public static void OnGetlocPFirst(CpuContext c, IMemory m) => OnGetlocP(c, m, "E0C");

    // pre su 0x80065E8C (GetlocP successivi, decide il bit)
    public static void OnGetlocPBit(CpuContext c, IMemory m) => OnGetlocP(c, m, "E8C");

    // post su 0x80065E8C: stato dopo la decisione
    public static void AfterBit(CpuContext c, IMemory m)
    {
        var idx = (int)((m.ReadU32(TablePtr) - TableBase) / 4);
        Diag($"  chiave={m.ReadU32(KeyAddr):X4} bit1={m.ReadU32(CountAddr)} voce={idx}");
    }

    // pre/post su 0x80065FC4 (decifra il primo settore del Mausoleo)
    static uint _decBuf;
    public static void BeforeDecrypt(CpuContext c, IMemory m)
    {
        _decBuf = c.A0;
        Diag($"decifratura Mausoleo: chiave={m.ReadU32(KeyAddr):X4} dati={Hex(m, _decBuf, 8)}");
    }

    public static void AfterDecrypt(CpuContext c, IMemory m)
    {
        Diag($"dopo decifratura: {Hex(m, _decBuf, 8)} (atteso 50503230 = \"PP20\")");
    }

    static void OnGetlocP(CpuContext c, IMemory m, string who)
    {
        var set = Sectors();
        uint res = c.A1;
        if (set.Count == 0 || res == 0) return;

        var before = Hex(m, res, 8);
        var tp = m.ReadU32(TablePtr);
        var change = "";

        // Solo il callback che decide il bit. Il lettore reale riporta ogni settore
        // letto: il gioco vede esattamente il settore bersaglio (bit 0) oppure, se il
        // suo Q e' modificato e quindi scartato, il fotogramma successivo (bit 1).
        // RecompOne invece avanza di piu' settori tra due GetlocP e il bersaglio non
        // compare mai: qui si riporta la posizione che il lettore reale darebbe.
        if (who == "E8C")
        {
            var abs = Msf(m.ReadU8(res + 5), m.ReadU8(res + 6), m.ReadU8(res + 7)) - 150;
            // voce della tabella: mm, ss, settore di partenza, fotogramma bersaglio
            var target = Msf(m.ReadU8(tp + 0), m.ReadU8(tp + 1), m.ReadU8(tp + 3)) - 150;
            if (abs >= target)
            {
                var report = set.Contains(target) ? target + 1 : target;
                var rel = Msf(m.ReadU8(res + 2), m.ReadU8(res + 3), m.ReadU8(res + 4));
                WriteMsf(m, res + 2, rel + (report - abs));
                WriteMsf(m, res + 5, report + 150);
                change = " -> " + Hex(m, res, 8);
            }
        }

        Diag($"{who}: res={before}{change} voce={(int)((tp - TableBase) / 4)} target={Hex(m, tp, 4)}");
    }

    static HashSet<int> Sectors()
    {
        if (_lba != null) return _lba;
        lock (_gate)
        {
            if (_lba != null) return _lba;
            var set = new HashSet<int>();
            var file = FindFile();
            if (file != null)
            {
                try
                {
                    var data = File.ReadAllBytes(file);
                    if (file.EndsWith(".lsd", StringComparison.OrdinalIgnoreCase)) ParseLsd(data, set);
                    else ParseSbi(data, set);
                    Console.WriteLine($"[PalLibCrypt] {Path.GetFileName(file)}: {set.Count} settori");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PalLibCrypt] errore lettura {file}: {ex.Message}");
                    set.Clear();
                }
            }

            if (set.Count == 0)
            {
                var id = RecompOne.Runtime.Assets.AssetApi.GameId;
                if (id == GameId)
                {
                    foreach (var l in BuiltIn) set.Add(l);
                    Console.WriteLine($"[PalLibCrypt] elenco settori incluso per {GameId}: {set.Count} settori");
                }
                else
                {
                    Console.WriteLine($"[PalLibCrypt] ATTENZIONE: disco {id} senza file .sbi/.lsd: " +
                                      "il Mausoleo in Cima alla Collina potrebbe non aprirsi.");
                }
            }

            _lba = set;
            return _lba;
        }
    }

    // SBI: "SBI\0", poi voci da 3 byte MSF (BCD) + 1 byte tipo + dati
    // (tipo 1: 10 byte di Q; tipi 2 e 3: 3 byte).
    static void ParseSbi(byte[] d, HashSet<int> set)
    {
        if (d.Length < 4 || d[0] != 'S' || d[1] != 'B' || d[2] != 'I') throw new InvalidDataException("intestazione SBI non valida");
        var o = 4;
        while (o + 4 <= d.Length)
        {
            set.Add(Msf(d[o], d[o + 1], d[o + 2]) - 150);
            var type = d[o + 3];
            o += 4;
            if (type == 1) o += 10;
            else if (type is 2 or 3) o += 3;
            else throw new InvalidDataException($"tipo voce SBI sconosciuto: {type}");
        }
    }

    // LSD: voci da 15 byte, 3 byte MSF (BCD) + 12 byte di Q (10 + CRC).
    static void ParseLsd(byte[] d, HashSet<int> set)
    {
        for (var o = 0; o + 15 <= d.Length; o += 15)
            set.Add(Msf(d[o], d[o + 1], d[o + 2]) - 150);
    }

    // Stesso nome del .cue con estensione .sbi/.lsd; altrimenti l'unico .sbi/.lsd della cartella.
    static string? FindFile()
    {
        var dirs = new List<string>();
        var cue = RecompOne.Runtime.Runtime.CdPath;
        if (!string.IsNullOrEmpty(cue))
        {
            foreach (var ext in new[] { ".sbi", ".lsd" })
            {
                var f = Path.ChangeExtension(cue, ext);
                if (File.Exists(f)) return f;
            }

            var d = Path.GetDirectoryName(Path.GetFullPath(cue));
            if (d != null) dirs.Add(d);
        }

        dirs.Add(Path.Combine(AppContext.BaseDirectory, "disc"));
        dirs.Add(Path.Combine(Directory.GetCurrentDirectory(), "disc"));

        foreach (var dir in dirs.Distinct())
        {
            if (!Directory.Exists(dir)) continue;
            var found = Directory.GetFiles(dir, "*.sbi").Concat(Directory.GetFiles(dir, "*.lsd")).ToArray();
            if (found.Length == 1) return found[0];
        }

        return null;
    }
}
