// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - uscita dal menu Sistema senza blocchi
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

// La voce Sistema -> Esci di RecompOne chiama Environment.Exit(0) dal thread che disegna
// l'interfaccia: il processo esce senza fermare il thread dell'audio e senza chiudere il
// dispositivo OpenAL, e su Windows puo' restare bloccato. Chiudendo con la X della finestra
// invece il runtime fa la chiusura ordinata (audio, impostazioni, finestra) e non si blocca.
//
// Qui la voce Esci viene sostituita da una che segnala la chiusura della finestra come la X.
// Per sicurezza, se dopo qualche secondo il processo e' ancora vivo, viene terminato.
//
// Schermo intero: chiudere mentre il gioco ha lo schermo intero esclusivo puo' bloccare il
// processo dentro il driver video, tanto che nemmeno il Task Manager riesce a terminarlo
// (uscendo prima dallo schermo intero e chiudendo la finestra non succede). Per questo Esci
// prima torna in finestra, aspetta che il cambio sia avvenuto e solo dopo chiude.

using System.Diagnostics;
using System.Reflection;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Host;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public static class PalQuitFix
{
    const string SystemMenu = "menu.system";
    const string QuitItem = "menu.system.quit";
    const int WatchdogMs = 4000;
    const int WindowedDelayMs = 500;   // tempo per uscire dallo schermo intero prima di chiudere

    static bool _installed;

    public static void Register()
    {
        // le voci predefinite del menu esistono solo dopo l'avvio della finestra
        Event.AddListener<RuntimeReadyEvent>(_ => GpuJobs.Run(Install));
    }

    static void Install()
    {
        if (_installed) return;
        _installed = true;
        if (!MenuRegistry.Remove(QuitItem)) return;
        MenuRegistry.Menu(SystemMenu).Item(QuitItem, Quit);
    }

    static void Quit()
    {
        var window = typeof(HostWindow).GetField("_window", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
        var closing = window?.GetType().GetProperty("IsClosing");
        if (closing is not { CanWrite: true } && window != null)
            closing = window.GetType().GetInterfaces()
                .Select(i => i.GetProperty("IsClosing"))
                .FirstOrDefault(p => p is { CanWrite: true });
        if (window == null || closing == null || !closing.CanWrite)
        {
            Environment.Exit(0);
            return;
        }

        // prima fuori dallo schermo intero (senza cambiare l'impostazione salvata)
        var fullscreen = DisplayOptions.Fullscreen.Value;
        if (fullscreen) HostWindow.SetFullscreen(false);

        var closer = new Thread(() =>
        {
            if (fullscreen) Thread.Sleep(WindowedDelayMs);

            // come la X: si alza solo il segnale "finestra da chiudere" (IsClosing) e il runtime
            // fa la sua chiusura ordinata. Chiamare Close() durante il disegno del menu rallentava.
            closing.SetValue(window, true);

            Thread.Sleep(WatchdogMs);
            Process.GetCurrentProcess().Kill();
        }) { IsBackground = true, Name = "PalQuitFix" };
        closer.Start();
    }
}
