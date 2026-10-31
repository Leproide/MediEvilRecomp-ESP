// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

using RecompOne.Runtime.Config;

namespace Recompiled;

public static class QualityOfLifeOptions
{
    public static readonly Option<bool> CameraControl = Options.Bool("qol.camera_control", true);
    public static readonly Option<bool> CameraRecenter = Options.Bool("qol.camera_recenter", true);
    public static readonly Option<bool> InvertCameraX = Options.Bool("qol.invert_camera_x");
    public static readonly Option<bool> InvertCameraY = Options.Bool("qol.invert_camera_y");

    public static IEnumerable<Option<bool>> All => [CameraControl, CameraRecenter, InvertCameraX, InvertCameraY];
}

internal static class QualityOfLife
{
    public static bool CameraControl;
    public static bool CameraRecenter;
    public static bool InvertCameraX;
    public static bool InvertCameraY;

    public static void Load()
    {
        CameraControl = QualityOfLifeOptions.CameraControl;
        CameraRecenter = QualityOfLifeOptions.CameraRecenter;
        InvertCameraX = QualityOfLifeOptions.InvertCameraX;
        InvertCameraY = QualityOfLifeOptions.InvertCameraY;
    }
}
