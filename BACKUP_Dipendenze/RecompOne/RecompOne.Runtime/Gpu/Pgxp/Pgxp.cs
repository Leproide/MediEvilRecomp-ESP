using RecompOne.Runtime.Config;

namespace RecompOne.Runtime.Pgxp;

public static class PgxpOptions
{
    public const float DefaultTolerance = 2f;

    public static readonly Option<bool> Enable = Options.Bool("pgxp.enable");
    public static readonly Option<bool> Culling = Options.Bool("pgxp.culling", true);
    public static readonly Option<bool> TextureCorrection = Options.Bool("pgxp.texture_correction", true);
    public static readonly Option<bool> VertexCache = Options.Bool("pgxp.vertex_cache", true);
    public static readonly Option<bool> CacheW = Options.Bool("pgxp.cache_w", true);
    public static readonly Option<bool> Cpu = Options.Bool("pgxp.cpu", true);
    public static readonly Option<bool> Memory = Options.Bool("pgxp.memory", true);
    public static readonly Option<float> Tolerance = Options.Float("pgxp.tolerance", DefaultTolerance);
}

public static class Pgxp
{
    private static bool _loaded;

    public static bool Enabled { get; private set; }
    public static bool Culling { get; private set; }
    public static bool TextureCorrection { get; private set; }
    public static bool VertexCache { get; private set; }
    public static bool CacheW { get; private set; }
    public static bool CpuTracking { get; private set; }
    public static bool MemoryTracking { get; private set; }
    public static float Tolerance { get; private set; }

    public static bool CullingCorrection => Enabled && Culling;

    public static void Load()
    {
        if (!_loaded)
        {
            _loaded = true;
            PgxpOptions.Enable.Changed += _ => Reload();
            PgxpOptions.Culling.Changed += _ => Reload();
            PgxpOptions.TextureCorrection.Changed += _ => Reload();
            PgxpOptions.VertexCache.Changed += _ => Reload();
            PgxpOptions.CacheW.Changed += _ => Reload();
            PgxpOptions.Cpu.Changed += _ => Reload();
            PgxpOptions.Memory.Changed += _ => Reload();
            PgxpOptions.Tolerance.Changed += _ => Reload();
        }

        Enabled = Features.Pgxp && PgxpOptions.Enable;
        Culling = PgxpOptions.Culling;
        TextureCorrection = PgxpOptions.TextureCorrection;
        VertexCache = PgxpOptions.VertexCache;
        CacheW = VertexCache && PgxpOptions.CacheW;
        CpuTracking = Enabled && PgxpOptions.Cpu;
        MemoryTracking = Enabled && PgxpOptions.Memory;
        Tolerance = PgxpOptions.Tolerance;
    }

    public static void EnsureLoaded()
    {
        if (!_loaded) Load();
    }

    private static void Reload()
    {
        Load();
        PgxpGte.Invalidate();
        if (!VertexCache) PgxpGpu.Free();
    }
}
