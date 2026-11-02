// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp ITA - cursore e barra dei menu nascosti quando non servono
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

// In RecompOne il cursore del mouse resta visibile sopra il gioco (anche a schermo intero e
// giocando col controller) e la voce System -> "Autohide Menu Bar" e' solo un segnaposto
// disattivato. Qui:
//  - cursore: sparisce dopo qualche secondo senza muovere il mouse, se non ci sono menu,
//    popup o elementi dell'interfaccia sotto il puntatore; ricompare appena si muove;
//  - barra dei menu: la voce "Autohide Menu Bar" diventa attiva (stessa impostazione,
//    interface.autohide_menu_bar). Accesa, la barra si nasconde dopo qualche secondo senza
//    muovere il mouse e ricompare muovendolo. F1 continua a mostrarla/nasconderla a mano
//    quando l'opzione e' spenta.
// Si aggiorna ad ogni frame dell'interfaccia tramite un popup invisibile che non si apre mai.

using System.Diagnostics;
using System.Numerics;
using ImGuiNET;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Host;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public static class InterfaceAutoHide
{
    const string SystemMenu = "menu.system";
    const string AutoHideItem = "menu.system.autohide_menu_bar";
    const string ShowMenuItem = "menu.system.show_menu_bar";

    const long CursorIdleMs = 2500;   // cursore fermo per questo tempo -> nascosto
    const long MenuIdleMs = 3000;     // barra dei menu nascosta dopo questo tempo senza mouse

    static readonly Stopwatch _idle = Stopwatch.StartNew();
    static bool _installed;
    static bool _menuHiddenByUs;

    public static void Register()
    {
        PopupManager.Register(new Ticker());
        // le voci predefinite del menu esistono solo dopo l'avvio della finestra
        Event.AddListener<RuntimeReadyEvent>(_ => GpuJobs.Run(Install));
        InterfaceOptions.AutoHideMenuBar.Changed += on =>
        {
            if (!on) ShowMenuBar();
        };
    }

    static void Install()
    {
        if (_installed) return;
        _installed = true;
        if (!MenuRegistry.Remove(AutoHideItem)) return;
        MenuRegistry.Menu(SystemMenu)
            .Check(AutoHideItem,
                static () => InterfaceOptions.AutoHideMenuBar,
                static on => InterfaceOptions.AutoHideMenuBar.Value = on)
            .After(ShowMenuItem);
    }

    internal static void Tick()
    {
        var io = ImGui.GetIO();
        bool moved = io.MouseDelta.X != 0f || io.MouseDelta.Y != 0f || io.MouseWheel != 0f ||
                     io.MouseDown[0] || io.MouseDown[1] || io.MouseDown[2];

        // menu a tendina, popup o un elemento dell'interfaccia in uso: tutto resta visibile
        bool busy = PopupManager.AnyOpen ||
                    ImGui.IsPopupOpen("", ImGuiPopupFlags.AnyPopupId | ImGuiPopupFlags.AnyPopupLevel) ||
                    ImGui.IsAnyItemHovered() || ImGui.IsAnyItemActive();

        // puntatore sopra la barra dei menu (o dove ricompare): la barra non si nasconde
        bool overMenuBar = io.MousePos.Y >= 0f && io.MousePos.Y < ImGui.GetFrameHeight() * 1.5f;

        if (moved || busy) _idle.Restart();
        long idle = _idle.ElapsedMilliseconds;

        if (idle >= CursorIdleMs && !overMenuBar)
            ImGui.SetMouseCursor(ImGuiMouseCursor.None);

        if (!InterfaceOptions.AutoHideMenuBar) return;
        if (moved || busy) ShowMenuBar();
        else if (idle >= MenuIdleMs && !overMenuBar && !InterfaceOptions.HideMenuBar)
        {
            InterfaceOptions.HideMenuBar.Value = true;
            _menuHiddenByUs = true;
        }
    }

    static void ShowMenuBar()
    {
        if (!_menuHiddenByUs && !InterfaceOptions.AutoHideMenuBar) return;
        if (InterfaceOptions.HideMenuBar) InterfaceOptions.HideMenuBar.Value = false;
        _menuHiddenByUs = false;
    }

    // popup mai aperto: serve solo perche' Update viene chiamato ad ogni frame dell'interfaccia
    sealed class Ticker : Popup
    {
        protected override string TitleKey => "menu.system.autohide_menu_bar";
        protected override void Update() => Tick();
        protected override void DrawContent() { }
    }
}
