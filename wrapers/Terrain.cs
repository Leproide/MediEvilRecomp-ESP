// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

using RecompOne.Runtime.Memory;

namespace MediEvil;

public static class FreeTerrain
{
    public const uint DataAddr = Recompiled.Addr.FreeTerrData;
    public const uint DataPool = DataAddr + 0x00u;
    public const uint PrimBuffer0 = DataAddr + 0x04u;
    public const uint PrimBuffer1 = DataAddr + 0x08u;
    public const uint CaptureList = DataAddr + 0x0Cu;
    public const uint RenderPolyPtr = DataAddr + 0x10u;
    public const uint MaxCapture = DataAddr + 0x14u;
    public const uint BufferLimit0 = DataAddr + 0x18u;
    public const uint BufferLimit1 = DataAddr + 0x1Cu;
    public const uint ViewDistance = DataAddr + 0x24u;
    public const uint OldViewDistance = DataAddr + 0x26u;

    static IMemory M => Game.M;

    public static short CurrentViewDistance => (short)M.ReadU16(ViewDistance);
}

public static class TerrainFog
{
    public const uint DataAddr = Recompiled.Addr.TerrainFog;
    public const uint Table = DataAddr + 0x00u;
    public const uint Entries = DataAddr + 0x04u;

    static IMemory M => Game.M;

    public static uint TableAddr => M.ReadU32(Table);
    public static ushort EntryCount => M.ReadU16(Entries);
}
