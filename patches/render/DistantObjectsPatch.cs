// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - oggetti visibili da lontano (porte, cancelli, decorazioni)
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

// Problema: con "Draw distance" oltre 1x il terreno arriva lontano, ma gli oggetti del livello
// (porte, cancelli, decorazioni) compaiono solo vicino a Dan: da lontano, per esempio, l'arco
// del cimitero sembra aperto. Succede con qualsiasi telecamera.
//
// Come gestisce gli oggetti il gioco (func_80041B84, una volta per frame):
//  - lista degli oggetti attorno a Dan a 0x800EEA50 (collegata a +0x4); distanza al quadrato
//    da un punto di riferimento (0x800EEB80) meno un margine del tipo (tipo +0x8 -> +0x54),
//    confrontata con due soglie al quadrato (puntatore a 0x800EEB94): [0] sveglia,
//    [4] disattivazione (al cimitero 2500 e 3500);
//  - "caricatore" (func_8004218C, quando il riferimento cambia cella): crea gli oggetti delle
//    celle vicine piu' vicini della soglia [0];
//  - entro la sveglia l'oggetto e' attivo; oltre dorme (flag 0x20000000 in +0x50) e il gioco
//    lo sostituisce con un elemento fermo (+0x70 -> [0]) marcato nascosto (0x40 in +0xA);
//    oltre la disattivazione esce dalla lista.
//
// Correzione (distanza = soglia di disattivazione x Draw distance, almeno x1.5 anche a 1x,
// perche' con lo shader la nebbia lontana mostra le comparse anche li', al massimo x3):
//  1. raggio di caricamento: durante il caricatore la soglia [0] vale la distanza allargata,
//     quindi gli oggetti vengono creati prima (addormentati);
//  2. durante il controllo la soglia [4] vale la distanza allargata: restano nella lista;
//  3. precaricamento: un oggetto addormentato mai stato sveglio non ha l'elemento fermo; se ha
//     il contenitore +0x70 (i nemici non lo hanno), una volta il contatore +0x35 viene messo
//     a 1 e il gioco lo considera a distanza 0 per un frame: riaddormentandolo crea
//     l'elemento fermo;
//  4. gli oggetti addormentati entro la distanza allargata hanno l'elemento fermo visibile.
// Le soglie vengono sempre rimesse com'erano: la sveglia (IA, eventi, trappole) resta quella
// del gioco. Gli oggetti lontani restano fermi nell'ultima posa. Anche le fiaccole (flag 0x80
// in +0x4C) vengono disegnate da lontano, ma spente: la fiamma la fanno solo da sveglie e il
// gioco non le sveglia in base alla distanza; si accendono avvicinandosi, come nell'originale.
// Oggetti senza elemento fermo (es. i muri distruttibili, tipo 800109F8): il gioco li disegna
// solo da svegli, quindi da lontano sparivano in qualsiasi stato (integro, crepato, in pezzi).
// Gli oggetti svegli stanno in una seconda lista (0x800EEAE8); il controllo li riaddormenta oltre
// la soglia di sveglia, a meno che il byte +0x36 sia minore di -1. Per i tipi in KeepAwake, entro
// la distanza allargata: se dormono, il contatore +0x35 li sveglia; se sono svegli, durante il
// controllo +0x36 vale -2 (poi torna com'era), cosi' restano svegli e disegnati. La loro logica
// (colpi, rottura) resta quella del gioco.
// Opzione: Misc -> Quality of life -> "Distant objects" (attiva di default).
// Con MEDIEVIL_DEBUG: righe "[DistantObjects]" con soglie, oggetti precaricati, mostrati, tenuti
// svegli e svegliati dal gioco, e ogni ~4 secondi l'elenco completo degli oggetti addormentati
// (tipo, flag, elemento fermo, posizione, distanza), per riconoscere i tipi da aggiungere a
// NoPreload o KeepAwake.

using RecompOne.Runtime.Config;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class DistantObjectsPatch
{
    public static readonly Option<bool> Setting = Options.Bool("qol.distant_objects", true);

    const float MinFactor = 1.5f;
    const float MaxFactor = 3f;

    // indirizzi per versione (config/versions.json): ITA 0x800EEA50 / 0x800EEAE8 / 0x800EEB80 /
    // 0x800EEB94, ESP 0x800EEAC8 / 0x800EEB60 / 0x800EEBF8 / 0x800EEC0C (dati spostati di +0x78)
    const uint ThingList = Addr.ThingList;
    const uint AwakeList = Addr.ThingAwakeList;
    const uint ThingStayAwake = 0x36u;
    const uint RefPoint = Addr.ThingRefPoint;
    const uint RangePtr = Addr.ThingRangePtr;

    const uint ThingNext = 0x04u;
    const uint ThingType = 0x08u;
    const uint ThingPos = 0x24u;
    const uint ThingWakeCounter = 0x35u;
    const uint ThingFlags = 0x50u;
    const uint ThingItems = 0x70u;
    const uint TypeRadius = 0x54u;
    const uint ItemFlags = 0x0Au;

    // tipi esclusi dal precaricamento (svegliarli un frame fa partire suoni/eventi, es. le casse)
    static readonly HashSet<uint> NoPreload = new() { };

    // tipi senza elemento fermo, innocui da tenere svegli lontano (nessuna IA)
    // 0x800109F8 e' lo stesso nelle due versioni (overlay CR identico in quel punto)
    static readonly HashSet<uint> KeepAwake = new() { 0x800109F8u };   // muro distruttibile
    static readonly Dictionary<uint, byte> _stayAwake = new();         // oggetto -> +0x36 originale
    static readonly HashSet<uint> _keptLogged = new();

    const uint FlagAsleep = 0x20000000u;
    const ushort FlagHidden = 0x0040;

    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static readonly HashSet<uint> _shown = new();
    static readonly HashSet<uint> _preloaded = new();
    static bool _enabled = Setting.Value;
    static uint _range, _origWake, _origFar, _loaderSaved;
    static bool _farPatched, _loaderPatched;

    static DistantObjectsPatch()
    {
        Setting.Changed += value => _enabled = value;
    }

    static float Factor() => Math.Clamp(TerrainPatch.DrawDistanceScale, MinFactor, MaxFactor);

    static bool Active => _enabled;

    // diagnostica (MEDIEVIL_DEBUG): oggetti che il gioco sveglia, per riconoscere i tipi
    static readonly HashSet<uint> _asleepLast = new();
    static readonly HashSet<uint> _loggedShown = new();
    static int _lines;
    static long _calls;

    static void Log(string s)
    {
        if (DiagOn && _lines++ < 20000) Console.WriteLine("[DistantObjects] " + s);
    }

    static uint Extended(float factor)
    {
        double d = Math.Sqrt(_origFar) * factor;
        return (uint)Math.Min(d * d, uint.MaxValue);
    }

    // --- caricatore: raggio di caricamento allargato ---

    public static void BeforeLoader(CpuContext c, IMemory m)
    {
        _loaderPatched = false;
        uint range = m.ReadU32(RangePtr);
        if (!Active || !Ram(range) || range != _range) return;
        _loaderSaved = m.ReadU32(range);
        m.WriteU32(range, Extended(Factor()));
        _loaderPatched = true;
    }

    public static void AfterLoader(CpuContext c, IMemory m)
    {
        if (!_loaderPatched) return;
        uint range = m.ReadU32(RangePtr);
        if (Ram(range)) m.WriteU32(range, _loaderSaved);
        _loaderPatched = false;
    }

    // --- controllo degli oggetti ---

    public static void BeforeActivation(CpuContext c, IMemory m)
    {
        _farPatched = false;
        uint range = m.ReadU32(RangePtr);
        if (!Ram(range)) return;

        uint wake = m.ReadU32(range), far = m.ReadU32(range + 4u);
        if (range != _range || wake != _origWake)
        {
            _range = range;
            _origWake = wake;
            _origFar = far;
            _shown.Clear();
            _preloaded.Clear();
            _keptLogged.Clear();
            _asleepLast.Clear();
            _loggedShown.Clear();
            Log($"soglie: sveglia {Math.Sqrt(wake):0}, disattivazione {Math.Sqrt(far):0}");
        }

        if (!Active) return;
        m.WriteU32(range + 4u, Extended(Factor()));
        _farPatched = true;

        // oggetti da tenere svegli entro la distanza allargata
        _stayAwake.Clear();
        long keepLimit = Extended(Factor());
        KeepAwakeList(m, ThingList, keepLimit, false);
        KeepAwakeList(m, AwakeList, keepLimit, true);

        // precaricamento degli oggetti addormentati senza elemento fermo
        uint thing = m.ReadU32(ThingList + ThingNext);
        for (int guard = 0; guard < 2048 && Ram(thing) && thing != ThingList; guard++)
        {
            uint items = m.ReadU32(thing + ThingItems);
            if ((m.ReadU32(thing + ThingFlags) & FlagAsleep) != 0 && Ram(items) && !Ram(m.ReadU32(items)) &&
                !NoPreload.Contains(m.ReadU32(thing + ThingType)) && _preloaded.Add(thing))
            {
                m.WriteU8(thing + ThingWakeCounter, 1);
                Log($"precarico {thing:X8} tipo {m.ReadU32(thing + ThingType):X8}");
            }
            thing = m.ReadU32(thing + ThingNext);
        }
    }

    public static void AfterActivation(CpuContext c, IMemory m)
    {
        uint range = _range;
        if (!Ram(range)) return;
        if (_farPatched) m.WriteU32(range + 4u, _origFar);

        // +0x36 com'era (se il gioco non l'ha cambiato nel frattempo)
        foreach (var (kept, original) in _stayAwake)
            if (m.ReadU8(kept + ThingStayAwake) == 0xFE) m.WriteU8(kept + ThingStayAwake, original);
        _stayAwake.Clear();

        long wake = _origWake;
        long limit = _farPatched ? Extended(Factor()) : 0;
        int rx = (int)m.ReadU32(RefPoint), ry = (int)m.ReadU32(RefPoint + 4u), rz = (int)m.ReadU32(RefPoint + 8u);

        // diagnostica: ogni ~4 secondi gli oggetti addormentati senza elemento fermo (non disegnabili)
        bool summary = DiagOn && _calls++ % 120 == 0;
        int total = 0, asleep = 0, noItem = 0;
        var missing = summary ? new List<string>() : null;

        uint thing = m.ReadU32(ThingList + ThingNext);
        for (int guard = 0; guard < 2048 && Ram(thing) && thing != ThingList; guard++)
        {
            uint item = Item(m, thing);
            bool sleeping = (m.ReadU32(thing + ThingFlags) & FlagAsleep) != 0;
            if (summary)
            {
                // elenco completo degli oggetti nella lista (diagnostica)
                long fx = (int)m.ReadU32(thing + ThingPos) - rx, fy = (int)m.ReadU32(thing + ThingPos + 4u) - ry,
                    fz = (int)m.ReadU32(thing + ThingPos + 8u) - rz;
                missing!.Add($"  ogg {thing:X8} tipo {m.ReadU32(thing + ThingType):X8} 33={m.ReadU8(thing + 0x33u):X2} " +
                             $"35={m.ReadU8(thing + ThingWakeCounter):X2} 48={m.ReadU32(thing + 0x48u):X8} " +
                             $"4C={m.ReadU32(thing + 0x4Cu):X8} 50={m.ReadU32(thing + ThingFlags):X8} " +
                             $"cont={m.ReadU32(thing + ThingItems):X8} el={item:X8} " +
                             $"pos={(int)m.ReadU32(thing + ThingPos)},{(int)m.ReadU32(thing + ThingPos + 4u)},{(int)m.ReadU32(thing + ThingPos + 8u)} " +
                             $"a {Math.Sqrt(fx * fx + fy * fy + fz * fz):0}");
                total++;
                if (sleeping)
                {
                    asleep++;
                    if (item == 0)
                    {
                        noItem++;
                    }
                }
            }

            if (DiagOn)
            {
                if (sleeping) _asleepLast.Add(thing);
                else if (_asleepLast.Remove(thing))
                    Log($"il gioco sveglia {thing:X8} tipo {m.ReadU32(thing + ThingType):X8} pos {(int)m.ReadU32(thing + ThingPos)},{(int)m.ReadU32(thing + ThingPos + 4u)},{(int)m.ReadU32(thing + ThingPos + 8u)}");
            }

            if (item != 0 && sleeping)
            {
                long dx = (int)m.ReadU32(thing + ThingPos) - rx;
                long dy = (int)m.ReadU32(thing + ThingPos + 4u) - ry;
                long dz = (int)m.ReadU32(thing + ThingPos + 8u) - rz;
                uint type = m.ReadU32(thing + ThingType);
                long d = dx * dx + dy * dy + dz * dz - (Ram(type) ? (int)m.ReadU32(type + TypeRadius) : 0);

                ushort flags = m.ReadU16(item + ItemFlags);
                if (_farPatched && d >= wake && d < limit)
                {
                    if ((flags & FlagHidden) != 0) m.WriteU16(item + ItemFlags, (ushort)(flags & ~FlagHidden));
                    _shown.Add(thing);
                    if (DiagOn && _loggedShown.Add(thing))
                        Log($"mostro {thing:X8} tipo {type:X8} a {Math.Sqrt(Math.Max(d, 0)):0}");
                }
                else if (_shown.Remove(thing))
                    m.WriteU16(item + ItemFlags, (ushort)(flags | FlagHidden));
            }
            else _shown.Remove(thing);

            thing = m.ReadU32(thing + ThingNext);
        }

        if (summary)
        {
            Log($"riepilogo: punto {rx},{ry},{rz}, oggetti {total}, addormentati {asleep}, senza elemento fermo {noItem}, " +
                $"mostrati {_shown.Count}, limite {Math.Sqrt(limit):0}");
            foreach (var line in missing!) Log(line);
        }
    }

    static void KeepAwakeList(IMemory m, uint list, long limit, bool awake)
    {
        int rx = (int)m.ReadU32(RefPoint), ry = (int)m.ReadU32(RefPoint + 4u), rz = (int)m.ReadU32(RefPoint + 8u);
        uint thing = m.ReadU32(list + ThingNext);
        for (int guard = 0; guard < 2048 && Ram(thing) && thing != list; guard++)
        {
            uint kind = m.ReadU32(thing + ThingType);
            if (KeepAwake.Contains(kind))
            {
                long dx = (int)m.ReadU32(thing + ThingPos) - rx;
                long dy = (int)m.ReadU32(thing + ThingPos + 4u) - ry;
                long dz = (int)m.ReadU32(thing + ThingPos + 8u) - rz;
                if (dx * dx + dy * dy + dz * dz < limit)
                {
                    if (!awake) m.WriteU8(thing + ThingWakeCounter, 1);
                    else
                    {
                        byte stay = m.ReadU8(thing + ThingStayAwake);
                        if ((sbyte)stay >= -1)
                        {
                            _stayAwake[thing] = stay;
                            m.WriteU8(thing + ThingStayAwake, 0xFE);
                        }
                    }

                    if (_keptLogged.Add(thing)) Log($"tengo sveglio {thing:X8} tipo {kind:X8} ({(awake ? "sveglio" : "addormentato")})");
                }
            }

            thing = m.ReadU32(thing + ThingNext);
        }
    }

    static uint Item(IMemory m, uint thing)
    {
        uint items = m.ReadU32(thing + ThingItems);
        if (!Ram(items)) return 0;
        uint item = m.ReadU32(items);
        return Ram(item) ? item : 0;
    }

    static bool Ram(uint a) => (a & 0xFF000000u) == 0x80000000u && (a & 0x00FFFFFFu) < 0x00200000u;
}
