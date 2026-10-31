// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - meno nebbia degli shader nel menu principale
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

// Lo shader Dan's Curse calcola la nebbia in base alla distanza. La scena del menu
// principale (overlay "tl") e' inquadrata da lontano e finisce quasi tutta nella
// nebbia. Il gioco non fornisce dati sulla nebbia agli shader, quindi qui, solo nel
// menu, si sostituiscono i parametri della nebbia con valori piu' leggeri.
// I valori passati con ShaderData hanno la precedenza sui parametri del preset e
// vengono tolti appena si carica un altro overlay. Gli shader che non usano questi
// nomi non vengono toccati.

using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;

namespace Recompiled;

public static class TitleShaderPatch
{
    const string TitleOverlay = "tl";

    static readonly (string Name, float Value)[] MenuValues =
    [
        // valori di Dan's Curse: nebbia 0.6, inizio 2600, foschia a terra 0.18
        ("DC_FOG", 0.3f),
        ("DC_FOG_START", 5000f),
        ("DC_GROUND_FOG", 0.1f),
    ];

    static bool _active;

    public static void Register()
    {
        Event.AddListener<OverlayLoadedEvent>(e => Apply(e.Name == TitleOverlay));
    }

    static void Apply(bool title)
    {
        if (title == _active) return;
        _active = title;
        foreach (var (name, value) in MenuValues)
        {
            if (title) ShaderData.Set(name, value);
            else ShaderData.Remove(name);
        }
    }
}
