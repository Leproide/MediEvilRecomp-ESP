// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - bordi della mappa sfumati nel nero
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

// Il terreno di MediEvil e' una griglia di quadrati (512 unita'): dove il livello finisce sul
// vuoto il bordo segue i quadrati e, con la telecamera libera o alta, si vedono gli scalini
// contro il nero. Qui ogni vertice del terreno vicino al vuoto viene scurito in base alla sua
// distanza (in pianta) dal quadrato vuoto piu' vicino: nero sul bordo, colore pieno a
// FadeSquares quadrati di distanza. Il bordo a scalini diventa nero su nero e la sfumatura,
// calcolata sulla distanza reale e non sui quadrati, e' morbida e arrotondata.
//
// Dati del gioco (letti dal renderer del terreno, CullPatch.func_80021D5C):
//  - griglia (mappa +0x6C): byte 0/1 = quadrati in X/Z, +0x08 tabella degli id (short, -1 = vuoto),
//    +0x0C quadrati da 8 byte (u16 numero di poligoni con bit 0x8000 = catturato, u32 puntatore
//    agli id dei poligoni);
//  - poligoni (*0x800EDDDC), 20 byte: indici dei vertici (short) a +0/+2/+4 (+6 se flag +0x0A & 1);
//  - vertici (*0x800EDDE0), 8 byte: short x, y, z e u16 colore usato come colore Gouraud
//    (R = bit 3-7, G = bit 11-15, B = bit 8-10 alti + bit 0-1 bassi).
// Verificato colorando i vertici di rosso dal pannello di diagnostica.
// I colori vengono riscritti ad ogni frame prima del disegno: se il gioco li cambia, il valore
// nuovo diventa l'originale da scurire. Spegnendo l'opzione si rimettono gli originali.
// Spento di default: scurisce anche oggetti di scena e, dove il livello ha molti quadrati vuoti
// sotto altra geometria (es. cimitero), zone lontane e vicine. Con gli shader il vuoto e' gia'
// nero (Dan's Curse V2). Si accende da Misc -> Quality of life -> "Map edge fade".
// Con MEDIEVIL_DEBUG: riga "[EdgeFade]" con i vertici scuriti per livello.

using RecompOne.Runtime.Config;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class TerrainEdgeFade
{
    public static readonly Option<bool> Setting = Options.Bool("render.terrain_edge_fade", false);

    // indirizzi per versione (config/versions.json): ITA 0x800EDDDC / 0x800EDDE0 / 0x800EF090 /
    // 0x800EF0E0, ESP 0x800EDE54 / 0x800EDE58 / 0x800EF108 / 0x800EF158
    const uint PolyArrayPtr = Addr.TerrainPolyArrayPtr;
    const uint VertexArrayPtr = Addr.TerrainVertexArrayPtr;
    const uint CurrentLevel = Addr.CurrentLevel;
    const uint CurrentMap = Addr.CurrentMap;
    const int PolyStride = 20;
    const uint PolyFlags = 0x0Au;
    const uint VertexColor = 0x06u;
    const ushort CapturedBit = 0x8000;
    const int MaxGrid = 256;
    const float FadeSquares = 1.5f;     // larghezza della sfumatura, in quadrati

    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));

    static volatile bool _enabled = Setting.Value;
    static (uint Grid, uint Vertices, uint Polys, uint Map, uint Level) _key;
    static bool _built;

    // per ogni vertice scurito: indirizzo del colore, fattore, colore originale, colore scritto
    static uint[] _address = [];
    static float[] _factor = [];
    static ushort[] _original = [];
    static ushort[] _written = [];
    static bool _applied;

    static TerrainEdgeFade()
    {
        Setting.Changed += value => _enabled = value;
    }

    // chiamata dalla cattura del terreno (TerrainCapture.Capture) ad ogni frame, prima del disegno
    public static void Update(IMemory m, uint grid, int shift, int baseX, int baseZ)
    {
        var key = (grid, m.ReadU32(VertexArrayPtr), m.ReadU32(PolyArrayPtr), m.ReadU32(CurrentMap), m.ReadU32(CurrentLevel));
        if (key != _key)
        {
            // livello o mappa cambiati: i dati sono nuovi, niente da rimettere
            _key = key;
            _built = false;
            _applied = false;
            _address = [];
        }

        if (!_enabled)
        {
            if (_applied) Restore(m);
            return;
        }

        if (!_built)
        {
            _built = true;
            Build(m, grid, shift, baseX, baseZ, key.Item2, key.Item3);
        }

        for (int i = 0; i < _address.Length; i++)
        {
            ushort current = m.ReadU16(_address[i]);
            if (_applied && current == _written[i]) continue;
            _original[i] = current;
            _written[i] = Scale(current, _factor[i]);
            m.WriteU16(_address[i], _written[i]);
        }
        _applied = true;
    }

    static void Restore(IMemory m)
    {
        for (int i = 0; i < _address.Length; i++)
            if (m.ReadU16(_address[i]) == _written[i]) m.WriteU16(_address[i], _original[i]);
        _applied = false;
    }

    // colore del gioco moltiplicato per f (0 = nero)
    static ushort Scale(ushort v, float f)
    {
        int r = v & 0xF8;
        int g = (v >> 8) & 0xF8;
        int b = (((v >> 8) & 7) << 5) | ((v & 3) << 3);
        r = (int)(r * f) & 0xF8;
        g = (int)(g * f) & 0xF8;
        int b5 = (int)(b * f) >> 3;
        return (ushort)(r | (g << 8) | ((b5 >> 2) << 8) | (b5 & 3));
    }

    static void Build(IMemory m, uint grid, int shift, int baseX, int baseZ, uint vertices, uint polys)
    {
        _address = [];
        if (!Ram(grid) || !Ram(vertices) || !Ram(polys) || shift is < 1 or > 15) return;
        int xnum = m.ReadU8(grid);
        int znum = m.ReadU8(grid + 1u);
        uint ids = m.ReadU32(grid + 0x08u);
        uint squares = m.ReadU32(grid + 0x0Cu);
        if (xnum is < 1 or > MaxGrid || znum is < 1 or > MaxGrid || !Ram(ids) || !Ram(squares)) return;

        // quadrati pieni (con poligoni); fuori dalla griglia e' vuoto
        var square = new uint[xnum, znum];
        for (int z = 0; z < znum; z++)
        for (int x = 0; x < xnum; x++)
        {
            int id = (short)m.ReadU16(ids + (uint)(x + z * xnum) * 2u);
            if (id < 0) continue;
            uint s = squares + (uint)id * 8u;
            if ((m.ReadU16(s) & ~CapturedBit) != 0 && Ram(m.ReadU32(s + 4u))) square[x, z] = s;
        }
        bool Empty(int x, int z) => x < 0 || z < 0 || x >= xnum || z >= znum || square[x, z] == 0u;

        int len = 1 << shift;
        float radius = len * FadeSquares;
        int reach = (int)MathF.Ceiling(FadeSquares) + 1;

        // quadrati pieni abbastanza vicini al vuoto da avere vertici da scurire
        var factor = new Dictionary<uint, float>();
        for (int z = 0; z < znum; z++)
        for (int x = 0; x < xnum; x++)
        {
            if (square[x, z] == 0u) continue;
            bool near = false;
            for (int dz = -reach; dz <= reach && !near; dz++)
            for (int dx = -reach; dx <= reach && !near; dx++)
                near = Empty(x + dx, z + dz);
            if (!near) continue;

            uint s = square[x, z];
            int count = m.ReadU16(s) & ~CapturedBit;
            uint list = m.ReadU32(s + 4u);
            for (int i = 0; i < count && i < 512; i++)
            {
                uint poly = polys + (uint)m.ReadU16(list + (uint)i * 2u) * PolyStride;
                if (!Ram(poly)) continue;
                int corners = (m.ReadU16(poly + PolyFlags) & 1) != 0 ? 4 : 3;
                for (int k = 0; k < corners; k++)
                {
                    uint vertex = vertices + (uint)((short)m.ReadU16(poly + (uint)k * 2u) << 3);
                    if (!Ram(vertex)) continue;
                    uint address = vertex + VertexColor;
                    if (factor.ContainsKey(address)) continue;

                    // distanza in pianta dal quadrato vuoto piu' vicino
                    int vx = (short)m.ReadU16(vertex) - baseX;
                    int vz = (short)m.ReadU16(vertex + 4u) - baseZ;
                    int cx = vx >> shift, cz = vz >> shift;
                    float best = float.MaxValue;
                    for (int dz = -reach; dz <= reach; dz++)
                    for (int dx = -reach; dx <= reach; dx++)
                    {
                        int qx = cx + dx, qz = cz + dz;
                        if (!Empty(qx, qz)) continue;
                        float ox = Math.Max(Math.Max((qx << shift) - vx, vx - ((qx + 1) << shift)), 0);
                        float oz = Math.Max(Math.Max((qz << shift) - vz, vz - ((qz + 1) << shift)), 0);
                        best = Math.Min(best, ox * ox + oz * oz);
                    }
                    if (best == float.MaxValue) continue;
                    float t = Math.Clamp(MathF.Sqrt(best) / radius, 0f, 1f);
                    float f = t * t * (3f - 2f * t);
                    if (f < 0.995f) factor[address] = f;
                }
            }
        }

        _address = factor.Keys.ToArray();
        _factor = factor.Values.ToArray();
        _original = new ushort[_address.Length];
        _written = new ushort[_address.Length];

        if (DiagOn)
            Console.WriteLine($"[EdgeFade] griglia {xnum}x{znum} quadrato {len}: {_address.Length} vertici sfumati " +
                              $"({factor.Values.Count(f => f == 0f)} neri)");
    }

    static bool Ram(uint address) => address >= 0x80010000u && address < 0x80000000u + RecompOne.Runtime.Runtime.RamSize;
}
