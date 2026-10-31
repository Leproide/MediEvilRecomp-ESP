// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - dissolvenza a iride in widescreen
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

// Issue #7 di MediEvilRecomp: la dissolvenza non copre gli angoli in widescreen.
// Con uno shader attivo, inoltre, l'anello sfumato viene reso nero opaco (vedi End).
//
// La dissolvenza "a iride" (func_8004EEE8 nella versione PAL) e' fatta di 32
// quadrilateri sfumati (POLY_G4) attorno al buco che si apre o si chiude, piu' 32
// quadrilateri neri pieni (POLY_F4) che riempiono lo spazio fino a un'ellisse fissa
// centrata in (256,128) con raggi 384 x 192. Basta per lo schermo 4:3 (512x256), non
// per quello allargato: restavano scoperti gli angoli.
// Dopo che il gioco ha preparato i poligoni, allarghiamo in orizzontale solo i
// vertici che stanno sull'ellisse esterna, in proporzione al margine widescreen.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class IrisFadePatch
{
    const uint PacketPtr = 0x1F800010u;   // puntatore ai pacchetti GPU liberi (scratchpad)
    const int ScreenWidth = 512;
    const int CenterX = 256, CenterY = 128;
    const float OuterRx = 384f, OuterRy = 192f;

    static uint _start;

    // pre su func_8004EEE8
    public static void Begin(CpuContext c, IMemory m) => _start = m.ReadU32(PacketPtr);

    // post su func_8004EEE8
    public static void End(CpuContext c, IMemory m)
    {
        uint start = _start, end = m.ReadU32(PacketPtr);
        _start = 0;
        if (start == 0 || end <= start || end - start > 0x4000) return;

        int margin = Display.WideAspect > 0f ? Display.WideMargin(ScreenWidth) : 0;
        bool shader = PostFx.Active;
        if (margin <= 0 && !shader) return;

        float scale = (CenterX + margin) / (float)CenterX;

        uint a = start;
        while (a + 8 <= end)
        {
            uint tag = m.ReadU32(a);
            int words = (int)(tag >> 24);
            if (words <= 0 || words > 16) break;
            byte cmd = m.ReadU8(a + 7);
            if ((cmd & 0xFC) == 0x38 && words >= 8)        // POLY_G4: xy a +8, +16, +24, +32
            {
                if (margin > 0)
                    for (uint off = 8; off <= 32; off += 8) FixVertex(m, a + off, scale);
                // Con uno shader attivo: l'anello sfumato dell'iride e' semitrasparente
                // in modalita' sottrattiva (tpage 0x40, abr 2), che il livello UI di
                // RecompOne non sa rappresentare: lo shader mostra luci e nebbia della
                // scena sopra il nero. L'anello diventa nero opaco (bordo netto).
                if (shader && cmd == 0x3A) MakeOpaqueBlack(m, a);
            }
            else if ((cmd & 0xFC) == 0x28 && words >= 5)   // POLY_F4: xy a +8, +12, +16, +20
            {
                if (margin > 0)
                    for (uint off = 8; off <= 20; off += 4) FixVertex(m, a + off, scale);
            }
            a += 4u + (uint)words * 4u;
        }
    }

    // POLY_G4: colore+comando a +4, poi colori dei vertici 1..3 a +12, +20, +28
    static void MakeOpaqueBlack(IMemory m, uint prim)
    {
        m.WriteU32(prim + 4, 0x38000000u);
        m.WriteU32(prim + 12, 0u);
        m.WriteU32(prim + 20, 0u);
        m.WriteU32(prim + 28, 0u);
    }

    static void FixVertex(IMemory m, uint addr, float scale)
    {
        uint xy = m.ReadU32(addr);
        int x = (short)(xy & 0xFFFF), y = (short)(xy >> 16);
        float dx = (x - CenterX) / OuterRx, dy = (y - CenterY) / OuterRy;
        if (dx * dx + dy * dy < 0.9f) return;            // solo i vertici dell'ellisse esterna
        int nx = CenterX + (int)MathF.Round((x - CenterX) * scale);
        nx = Math.Clamp(nx, -1024, 1023);
        m.WriteU32(addr, (uint)(ushort)nx | (xy & 0xFFFF0000u));
    }
}
