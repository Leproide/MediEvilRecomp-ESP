// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - the releases of MediEvil the launcher knows, and where each one is built.
//
// The PAL builds (Italian, Spanish, French) come from this repository: one set of patches,
// one address table (config/versions.json), one generated-code folder per version.
// The USA build is the original BlackLabelHQ project, cloned untouched into usa/ and
// updated from there, so its releases reach the launcher as they are published.

namespace MediEvilLauncher;

public enum Project { Pal, Usa }

public sealed record GameVersion(
    string Code,          // "ita", "esp", "fra", "usa"
    string Serial,        // SCES-01494 ...
    string BootFile,      // executable named by SYSTEM.CNF
    Project Project,
    string Config,        // recompiler config, relative to the project root
    string Generated,     // generated-code folder, relative to the project root
    string Lingua,        // -p:Lingua value of the PAL project ("" for the Italian default)
    string BinFolder)     // build output root, relative to the project root
{
    public string ExePath(string root) =>
        Path.Combine(root, BinFolder, "Release", "net10.0", "medievil.exe");
}

public static class GameVersions
{
    /// <summary>Repository of the PAL project, cloned when the launcher runs outside it.
    /// Set when the launcher is built (MSBuild property PalRepository, see the .csproj),
    /// so a fork can publish a launcher that downloads the fork.</summary>
    public static readonly string PalRepository =
        typeof(GameVersions).Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyMetadataAttribute), false)
            .Cast<System.Reflection.AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "PalRepository")?.Value
        ?? "https://github.com/Leproide/MediEvilRecomp-ESP";

    /// <summary>The original USA project (BlackLabelHQ), cloned into usa/.</summary>
    public const string UsaRepository = "https://github.com/BlackLabelHQ/MediEvilRecomp";

    public static readonly GameVersion[] All =
    {
        new("ita", "SCES-01494", "SCES_014.94", Project.Pal, "config/MediEvil.json", "generated", "", "bin"),
        new("esp", "SCES-01495", "SCES_014.95", Project.Pal, "config/es/MediEvil.json", "generated-es", "es", "bin-es"),
        new("fra", "SCES-01492", "SCES_014.92", Project.Pal, "config/fr/MediEvil.json", "generated-fr", "fr", "bin-fr"),
        new("deu", "SCES-01493", "SCES_014.93", Project.Pal, "config/de/MediEvil.json", "generated-de", "de", "bin-de"),
        new("eng", "SCES-00311", "SCES_003.11", Project.Pal, "config/en/MediEvil.json", "generated-en", "en", "bin-en"),
        new("usa", "SCUS-94227", "SCUS_942.27", Project.Usa, "config/MediEvil.json", "generated", "", "bin"),
    };

    /// <summary>The version whose boot executable SYSTEM.CNF names, or null.</summary>
    public static GameVersion? FromSystemCnf(string cnf) =>
        All.FirstOrDefault(v => cnf.Contains(v.BootFile, StringComparison.OrdinalIgnoreCase));
}
