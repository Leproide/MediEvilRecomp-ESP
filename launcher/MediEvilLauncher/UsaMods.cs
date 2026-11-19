// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - the PAL project's two game mods (Trucchi: cheats, FilmatiSchermoIntero:
// fullscreen movies) for the USA version too.
//
// The mods have hooks for the USA executable as well, but the USA project
// (BlackLabelHQ/MediEvilRecomp) does not have them: before the USA version starts, they are
// copied to usa\mods\, only on this computer. Its RecompOne is newer: ImGui comes from
// Hexa.NET.ImGui and Localization from RecompOne.UI, so two lines of the sources are
// adapted in the copy. The copies are listed in usa\.git\info\exclude, so git does not see
// them (the USA build stays up to date).

using System.Text;

namespace MediEvilLauncher;

public static class UsaMods
{
    static readonly string[] Mods = { "Trucchi", "FilmatiSchermoIntero" };

    static readonly (string Pal, string Usa)[] SourceChanges =
    {
        ("using ImGuiNET;", "using Hexa.NET.ImGui;"),
        ("RecompOne.Runtime.Host.Window.Localization", "RecompOne.UI.Localization"),
    };

    /// <summary>Copies the mods of the PAL project to the USA one, when they changed.</summary>
    public static void Install(string palRoot, string usaRoot)
    {
        foreach (var name in Mods)
        {
            var src = Path.Combine(palRoot, "mods", name);
            if (!File.Exists(Path.Combine(src, "mod.json"))) continue;
            var dst = Path.Combine(usaRoot, "mods", name);
            var copied = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var file in Directory.GetFiles(src, "*", SearchOption.AllDirectories))
            {
                var rel = Path.GetRelativePath(src, file);
                if (rel.StartsWith(".") || rel.Contains(Path.DirectorySeparatorChar + ".")) continue;
                var bytes = File.ReadAllBytes(file);
                if (file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var text = Encoding.UTF8.GetString(bytes);
                    foreach (var (pal, usa) in SourceChanges) text = text.Replace(pal, usa);
                    bytes = Encoding.UTF8.GetBytes(text);
                }
                var target = Path.Combine(dst, rel);
                copied.Add(target);
                if (File.Exists(target) && File.ReadAllBytes(target).AsSpan().SequenceEqual(bytes)) continue;
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.WriteAllBytes(target, bytes);
            }
            // sources removed from the PAL mod
            var sources = Path.Combine(dst, "source");
            if (Directory.Exists(sources))
                foreach (var file in Directory.GetFiles(sources, "*.cs", SearchOption.AllDirectories))
                    if (!copied.Contains(file)) File.Delete(file);
        }
        Exclude(usaRoot);
    }

    static void Exclude(string usaRoot)
    {
        var info = Path.Combine(usaRoot, ".git", "info");
        if (!Directory.Exists(Path.Combine(usaRoot, ".git"))) return;   // not a plain clone
        Directory.CreateDirectory(info);
        var file = Path.Combine(info, "exclude");
        var lines = File.Exists(file) ? File.ReadAllLines(file).ToList() : new List<string>();
        var missing = Mods.Select(m => $"/mods/{m}/").Where(l => !lines.Contains(l)).ToList();
        if (missing.Count == 0) return;
        lines.Add("# PAL project mods copied by MediEvilLauncher");
        lines.AddRange(missing);
        File.WriteAllLines(file, lines);
    }
}
