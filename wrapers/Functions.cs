// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

namespace MediEvil;

public static class Functions
{
    //stupid library i hate you
    public const uint MRAllocMem = Recompiled.Addr.FnMRAllocMem;
    public const uint MRFreeMem = Recompiled.Addr.FnMRFreeMem;
    public const uint MRInitialiseResources = Recompiled.Addr.FnMRInitialiseResources;
    public const uint MRLoadResource = Recompiled.Addr.FnMRLoadResource;
    public const uint MRProcessResource = Recompiled.Addr.FnMRProcessResource;
    public const uint MRUnloadResource = Recompiled.Addr.FnMRUnloadResource;
    public const uint PowerPackerDecrunch = Recompiled.Addr.FnPowerPackerDecrunch;
    public const uint MRAllocateDynamicPolys = Recompiled.Addr.FnMRAllocateDynamicPolys;
    public const uint MRFreeDynamicPolys = Recompiled.Addr.FnMRFreeDynamicPolys;
    public const uint MRRenderViewport = Recompiled.Addr.FnMRRenderViewport;
    public const uint MRKillViewport = Recompiled.Addr.FnMRKillViewport;
    public const uint MRKillAllViewports = Recompiled.Addr.FnMRKillAllViewports;
    public const uint MRUpdateViewportRenderMatrices = Recompiled.Addr.FnMRUpdateViewportRenderMatrices;

    //game stuffs
    public const uint CamUpdate = Recompiled.Addr.FnCamUpdate;
    public const uint FadeUpdate = Recompiled.Addr.FnFadeUpdate;
    public const uint EntLibPointOccluded = Recompiled.Addr.FnEntLibPointOccluded;
    public const uint LevelStartUp = Recompiled.Addr.FnLevelStartUp;
    public const uint LevelShutDown = Recompiled.Addr.FnLevelShutDown;
    public const uint FreeTerrShutDown = Recompiled.Addr.FnFreeTerrShutDown;
    public const uint TerrainFogShutdown = Recompiled.Addr.FnTerrainFogShutdown;
}
