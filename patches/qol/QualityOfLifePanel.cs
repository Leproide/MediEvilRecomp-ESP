// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

using System.Numerics;
using ImGuiNET;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public sealed class QualityOfLifePanel : IPanel
{
    public string Name => "Quality Of Life Options";
    public string TitleKey => "panel.qol";
    public bool IsOpen { get; set; }

    public void Draw()
    {
        ImGui.SetNextWindowSize(new Vector2(320, 260), ImGuiCond.FirstUseEver);
        var open = IsOpen;
        if (!ImGui.Begin(this.Title(), ref open))
        {
            IsOpen = open;
            ImGui.End();
            return;
        }

        ImGui.SeparatorText(Localization.T("qol.camera"));
        Toggle(QualityOfLifeOptions.CameraControl, "qol.camera_control");

        if (!QualityOfLife.CameraControl) ImGui.BeginDisabled();
        Toggle(QualityOfLifeOptions.CameraRecenter, "qol.camera_recenter");
        Toggle(QualityOfLifeOptions.InvertCameraX, "qol.invert_camera_x");
        Toggle(QualityOfLifeOptions.InvertCameraY, "qol.invert_camera_y");
        if (!QualityOfLife.CameraControl) ImGui.EndDisabled();

        IsOpen = open;
        ImGui.End();
    }

    private static void Toggle(Option<bool> option, string key)
    {
        SettingsWidgets.Toggle(option, key, key + ".hint");
    }
}
