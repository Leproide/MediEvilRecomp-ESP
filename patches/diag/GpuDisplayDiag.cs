// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - diagnostica dei framebuffer mostrati (solo con MEDIEVIL_DEBUG)
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

// Serve a capire l'immagine tagliata in basso dopo lo skip di un filmato, che succede
// solo con risoluzione interna maggiore di 1x. Ad ogni cambiamento scrive nel log:
//  - le due aree di schermo note al runtime (GpuHle, una per buffer);
//  - i render target ad alta risoluzione di GlCore (privati: letti per riflessione);
//  - righe mostrate dalla GPU e modo video di LibGpu (0 NTSC, 1 PAL).
// Non modifica niente. Righe "[GpuDiag]", massimo 600.

using System.Reflection;
using System.Text;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;

namespace Recompiled;

public static class GpuDisplayDiag
{
    static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    static int _lines;
    static string _lastState = "";
    static object? _core;
    static FieldInfo? _rtsField;
    static bool _searched;

    static void Log(string s)
    {
        if (_lines++ < 600) Console.WriteLine("[GpuDiag] " + s);
    }

    public static void Register()
    {
        if (!DiagOn) return;
        Event.AddListener<VSyncEvent>(OnVSync);
        Event.AddListener<OverlayLoadedEvent>(e => Log($"overlay {e.Name}"));
    }

    static void OnVSync(VSyncEvent e)
    {
        try
        {
            var sb = new StringBuilder();
            var gpu = RecompOne.Runtime.Runtime.Gpu;
            var mode = typeof(RecompOne.Runtime.Sdk.LibGpu)
                .GetField("_videoMode", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
            sb.Append($"modo {mode} gpu {gpu?.DisplayWidth}x{gpu?.DisplayHeight} scala {GlVram.Scale} mostrato {GpuHle.LastDisplayW}x{GpuHle.LastDisplayH} | aree:");
            for (var i = 0; i < GpuHle.RectCount; i++)
            {
                var r = GpuHle.GetRect(i);
                if (r.Valid) sb.Append($" [{r.X},{r.Y} {r.W}x{r.H}]");
            }

            sb.Append(" | rt:");
            if (FindRts() is Array rts)
                foreach (var rt in rts)
                {
                    if (rt == null) continue;
                    var t = rt.GetType();
                    int Get(string n) => t.GetField(n)?.GetValue(rt) is int v ? v : -1;
                    sb.Append($" [{Get("X")},{Get("Y")} {Get("W")}x{Get("H")} m{Get("Margin")}]");
                }
            else sb.Append(" (non trovati)");

            var state = sb.ToString();
            if (state == _lastState) return;
            _lastState = state;
            Log($"frame {e.Frame}: {state}");
        }
        catch (Exception ex)
        {
            Log("errore: " + ex.Message);
        }
    }

    // cerca l'istanza di GlCore partendo da GpuHle.Backend (InterpBackend -> backend GL -> GlCore)
    static Array? FindRts()
    {
        if (!_searched && GpuHle.Backend is { } backend)
        {
            _searched = true;
            _core = Find(backend, 0);
            _rtsField = _core?.GetType().GetField("_rts", BindingFlags.Instance | BindingFlags.NonPublic);
            Log(_core != null ? "GlCore trovato" : "GlCore non trovato");
        }

        return _core != null ? _rtsField?.GetValue(_core) as Array : null;
    }

    static object? Find(object o, int depth)
    {
        if (o.GetType().Name == "GlCore") return o;
        if (depth >= 4) return null;
        foreach (var f in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (f.FieldType.IsValueType || f.FieldType == typeof(string)) continue;
            if (f.GetValue(o) is { } v && v.GetType().Namespace?.StartsWith("RecompOne") == true)
                if (Find(v, depth + 1) is { } found) return found;
        }

        return null;
    }
}
