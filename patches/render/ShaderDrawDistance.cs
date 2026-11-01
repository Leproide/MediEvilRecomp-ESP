// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - distanza di disegno del gioco passata agli shader
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

// Gli shader (Dan's Curse V2) sfumano al nero la geometria vicina al limite di disegno.
// Senza questo dato lo stimano dalla geometria visibile, che davanti a un muro e' vicina:
// la nebbia allora "saltava" addosso al giocatore. Qui ad ogni frame si legge la distanza
// di disegno corrente del gioco (gia' scalata dall'opzione "Distanza di disegno" del
// runtime) e la si passa come "SceneViewDistance" (x = distanza, y = 1 se valida).
// Con MEDIEVIL_DEBUG le righe "[DrawDistance]" del log mostrano ogni cambiamento.

using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;

namespace Recompiled;

public static class ShaderDrawDistance
{
    // distanza di disegno del terreno (short), la stessa usata da TerrainPatch/TerrainCapture:
    // FreeTerrData + 0x24, cioe' 0x800EF048 nella versione italiana e 0x800EF0C0 nella spagnola
    // (config/versions.json, costante ViewDistance)
    const uint ViewDistance = Addr.ViewDistance;
    const int MinDistance = 500;

    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static int _lines;
    static int _last = -1;

    public static void Register()
    {
        Event.AddListener<VSyncEvent>(OnVSync);
    }

    static void OnVSync(VSyncEvent e)
    {
        var m = RecompOne.Runtime.Runtime.Mem;
        if (m == null) return;

        int distance = (short)m.ReadU16(ViewDistance);
        bool valid = distance >= MinDistance;
        if (valid) ShaderScene.SetViewDistance(distance);
        else ShaderData.Remove("SceneViewDistance");

        if (!valid) distance = 0;
        if (distance == _last) return;
        _last = distance;
        if (DiagOn && _lines++ < 200)
            Console.WriteLine($"[DrawDistance] frame {e.Frame}: {(valid ? distance.ToString() : "non valida")}");
    }
}
