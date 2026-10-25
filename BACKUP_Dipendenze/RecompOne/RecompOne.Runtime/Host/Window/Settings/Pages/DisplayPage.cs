using ImGuiNET;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Pgxp;

namespace RecompOne.Runtime.Host.Window;

internal static class DisplayPage
{
    private static readonly string[] Backends = ["auto", "gl45", "gl33", "gl21"];

    public static void Register()
    {
        //some games may not need these so make it toglable by the game
        SettingsPages.Page(SettingsPages.Display, "settings.display", 5)
            .Add("window", DrawWindow, 0)
            .Add("renderer", DrawRenderer, 100)
            .Add("frame_rate", DrawFrameRate, 200, "settings.display.frame_rate",
                static () => Features.FrameInterpolation)
            .Add("pgxp", DrawPgxp, 300, "settings.display.pgxp", static () => Features.Pgxp);
    }

    private static void DrawWindow()
    {
        if (SettingsWidgets.Toggle(DisplayOptions.Fullscreen, "settings.display.fullscreen"))
            HostWindow.SetFullscreen(DisplayOptions.Fullscreen);

        if (SettingsWidgets.Toggle(DisplayOptions.VSync, "settings.display.vsync", "settings.display.vsync_hint"))
            HostWindow.SetVSync(DisplayOptions.VSync);
    }

    private static void DrawRenderer()
    {
        if (SettingsWidgets.Slider(DisplayOptions.RenderScale, "settings.display.render_scale", 1, 8, "%dx",
                "settings.display.render_scale_hint"))
            NoticePopup.Show(Localization.T("common.restart_required"));

        var scale = DisplayOptions.RenderScale.Value;
        var lines = Hle.GpuHle.LastDisplayH;
        var width = Hle.GpuHle.LastDisplayW;
        if (lines > 0)
            ImGui.TextDisabled(Localization.T("settings.display.render_scale_lines",
                width, lines, width * scale, lines * scale, scale));

        if (scale != Hle.GlVram.Scale)
            ImGui.TextDisabled(Localization.T("settings.display.restart_pending"));

        ImGui.Spacing();

        var backends = Backends.Select(static b => (b, b)).ToArray();
        if (SettingsWidgets.Combo(DisplayOptions.Backend, "settings.display.backend", backends))
            NoticePopup.Show(Localization.T("common.restart_required"));

        ImGui.TextDisabled(Localization.T("settings.display.backend_running", Hle.GpuBackendFactory.Selected));
    }

    private static void DrawFrameRate()
    {
        var available = Interp.Interp.Available;
        if (!available) ImGui.BeginDisabled();

        var steps = Interp.Interp.Steps;
        var index = Math.Max(Array.IndexOf(steps, Interp.InterpOptions.TargetFps.Value), 0);

        if (ImGui.SliderInt("##frame-rate", ref index, 0, steps.Length - 1, StepLabel(steps[index])))
        {
            Interp.InterpOptions.TargetFps.Value = steps[Math.Clamp(index, 0, steps.Length - 1)];
            HostWindow.RefreshVSync();
        }

        SettingsWidgets.Hint("settings.display.frame_rate_hint");

        if (!available) ImGui.EndDisabled();

        if (!available)
            ImGui.TextDisabled(Localization.T("settings.display.frame_rate_needs_pgxp"));
    }

    private static string StepLabel(int fps)
    {
        if (fps == Interp.Interp.Native) return Localization.T("settings.display.frame_rate_native");

        var effective = Interp.Interp.Resolve(fps, Interp.Interp.RefreshRate, Interp.Interp.VSync);
        return effective > 0 && effective != fps ? $"{fps} FPS ({effective})" : $"{fps} FPS";
    }

    private static void DrawPgxp()
    {
        SettingsWidgets.Toggle(PgxpOptions.Enable, "settings.display.pgxp_enable", "settings.display.pgxp_enable_hint");

        var enabled = PgxpOptions.Enable.Value;
        if (!enabled) ImGui.BeginDisabled();

        var column = ImGui.GetContentRegionAvail().X / 3f;
        
        //1
        SettingsWidgets.Toggle(PgxpOptions.TextureCorrection, "settings.display.pgxp_texture", "settings.display.pgxp_texture_hint");
        ImGui.SameLine(column);
        SettingsWidgets.Toggle(PgxpOptions.Culling, "settings.display.pgxp_culling", "settings.display.pgxp_culling_hint");
        ImGui.SameLine(column * 2f);
        SettingsWidgets.Toggle(PgxpOptions.Cpu, "settings.display.pgxp_cpu", "settings.display.pgxp_cpu_hint");
        //2
        SettingsWidgets.Toggle(PgxpOptions.VertexCache, "settings.display.pgxp_vertex_cache", "settings.display.pgxp_vertex_cache_hint");
        ImGui.SameLine(column);
        SettingsWidgets.Toggle(PgxpOptions.Memory, "settings.display.pgxp_memory", "settings.display.pgxp_memory_hint");
        ImGui.SameLine(column * 2f);
        SettingsWidgets.Toggle(PgxpOptions.CacheW, "settings.display.pgxp_cache_w", "settings.display.pgxp_cache_w_hint");

        var tolerance = PgxpOptions.Tolerance.Value;
        if (ImGui.SliderFloat(Localization.T("settings.display.pgxp_tolerance"), ref tolerance, -1f, 10f, tolerance < 0f ? Localization.T("settings.display.pgxp_tolerance_off") : "%.2f"))
            PgxpOptions.Tolerance.Value = tolerance;

        SettingsWidgets.Hint("settings.display.pgxp_tolerance_hint");

        if (!enabled) ImGui.EndDisabled();
    }
}
