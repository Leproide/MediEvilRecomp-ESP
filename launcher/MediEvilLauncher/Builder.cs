// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - finds the projects, identifies the disc, builds and starts the game.
//
// The build is the one of windows_initial_build*.bat, run step by step: submodules, the
// fork's RecompOne fixes (PAL project only), RecompOne, the recompiler on the version's
// config, then the game in Release. The recompiler reads the player's own disc (.cue or .chd)
// through a copy of the config whose "cue" points at it, so the disc can stay where it is,
// with any file name. A stamp in the generated folder records what the build was made from
// (project commit, RecompOne commit, config, disc): when any of it changes, the version has
// to be rebuilt.

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using RecompOne.Runtime.Cdrom;

namespace MediEvilLauncher;

public enum BuildState { Ready, NotBuilt, Stale, ProjectMissing }

public sealed class Builder
{
    public string PalRoot { get; }
    public string UsaRoot => Path.Combine(PalRoot, "usa");
    readonly Action<string> _log;

    public Builder(string palRoot, Action<string> log)
    {
        PalRoot = palRoot;
        _log = log;
    }

    public string Root(GameVersion v) => v.Project == Project.Pal ? PalRoot : UsaRoot;

    // ------------------------------------------------------------------ projects

    /// <summary>Folder of the PAL project: the launcher's folder or one of its parents,
    /// else MediEvilRecomp-PAL next to the launcher (to be cloned).</summary>
    public static string FindPalRoot()
    {
        for (var d = new DirectoryInfo(AppContext.BaseDirectory); d != null; d = d.Parent)
            if (IsPalRoot(d.FullName)) return d.FullName;
        return Path.Combine(AppContext.BaseDirectory, "MediEvilRecomp-PAL");
    }

    public static bool IsPalRoot(string dir) =>
        File.Exists(Path.Combine(dir, "RecompOne.MediEvil.csproj")) &&
        File.Exists(Path.Combine(dir, "config", "versions.json"));

    public bool ProjectPresent(GameVersion v) =>
        File.Exists(Path.Combine(Root(v), "RecompOne.MediEvil.csproj"));

    // ------------------------------------------------------------------ disc

    /// <summary>Boot executable named by the disc's SYSTEM.CNF; null if it cannot be read.</summary>
    public static string? ReadSystemCnf(string discPath)
    {
        try
        {
            using var fs = DiscFs.Open(discPath);
            return Encoding.ASCII.GetString(fs.ReadFile("SYSTEM.CNF"));
        }
        catch
        {
            return null;
        }
    }

    // ------------------------------------------------------------------ state

    public BuildState State(GameVersion v, string disc)
    {
        if (!ProjectPresent(v)) return BuildState.ProjectMissing;
        if (!File.Exists(v.ExePath(Root(v)))) return BuildState.NotBuilt;
        var stamp = Path.Combine(Root(v), v.Generated, ".launcher-stamp");
        if (!File.Exists(stamp)) return BuildState.Stale;
        return File.ReadAllText(stamp) == Stamp(v, disc) ? BuildState.Ready : BuildState.Stale;
    }

    string Stamp(GameVersion v, string disc)
    {
        var root = Root(v);
        var sb = new StringBuilder();
        sb.AppendLine("project " + GitHead(root));
        sb.AppendLine("recompone " + GitHead(Path.Combine(root, "RecompOne")));
        sb.AppendLine("config " + Sha256(File.ReadAllBytes(Path.Combine(root, v.Config))));
        var fi = new FileInfo(disc);
        sb.AppendLine($"disc {fi.FullName} {fi.Length}");
        return sb.ToString();
    }

    static string Sha256(byte[] data) => Convert.ToHexString(SHA256.HashData(data));

    static string GitHead(string dir)
    {
        try
        {
            var p = Process.Start(new ProcessStartInfo(Tools.Git, "rev-parse HEAD")
            {
                WorkingDirectory = dir, RedirectStandardOutput = true, RedirectStandardError = true,
                UseShellExecute = false, CreateNoWindow = true,
            })!;
            var s = p.StandardOutput.ReadToEnd().Trim();
            p.WaitForExit();
            return p.ExitCode == 0 ? s : "?";
        }
        catch
        {
            return "?";
        }
    }

    // ------------------------------------------------------------------ steps

    public sealed class StepFailed : Exception
    {
        public StepFailed(string step) : base(step) { }
    }

    /// <summary>Runs a command, streaming its output to the log; throws StepFailed on a
    /// non-zero exit unless allowFail.</summary>
    async Task<int> Run(string step, string dir, string file, string args, bool allowFail = false,
                        CancellationToken ct = default)
    {
        _log($"> {file} {args}");
        var exe = file switch { "dotnet" => Tools.Dotnet, "git" => Tools.Git, _ => file };
        var psi = new ProcessStartInfo(exe, args)
        {
            WorkingDirectory = dir, RedirectStandardOutput = true, RedirectStandardError = true,
            UseShellExecute = false, CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8, StandardErrorEncoding = Encoding.UTF8,
        };
        Tools.Apply(psi);
        using var p = new Process { StartInfo = psi };
        p.OutputDataReceived += (_, e) => { if (e.Data != null) _log(e.Data); };
        p.ErrorDataReceived += (_, e) => { if (e.Data != null) _log(e.Data); };
        p.Start();
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        await p.WaitForExitAsync(ct);
        if (p.ExitCode != 0 && !allowFail) throw new StepFailed(step);
        return p.ExitCode;
    }

    /// <summary>Git and the .NET 10 SDK, downloaded once when the computer has none.</summary>
    async Task EnsureTools()
    {
        try
        {
            await Tools.Ensure(_log);
        }
        catch (Exception e)
        {
            _log(e.Message);
            throw new StepFailed(T.S("step.tools"));
        }
    }

    public async Task ClonePal(Action<string> stage)
    {
        stage(T.S("step.tools"));
        await EnsureTools();
        stage(T.S("step.clone"));
        Directory.CreateDirectory(Path.GetDirectoryName(PalRoot)!);
        await Run(T.S("step.clone"), Path.GetDirectoryName(PalRoot)!, "git",
                  $"clone --recursive \"{GameVersions.PalRepository}\" \"{PalRoot}\"");
    }

    /// <summary>Full build of version v from the disc.</summary>
    public async Task Build(GameVersion v, string disc, Action<string, int, int> stage)
    {
        var root = Root(v);
        int n = 0, total = v.Project == Project.Pal ? 8 : 6;
        void Next(string key) => stage(T.S(key), ++n, total);

        Next("step.tools");
        await EnsureTools();

        if (v.Project == Project.Usa && !ProjectPresent(v))
        {
            Next("step.clone");
            await Run(T.S("step.clone"), PalRoot, "git", $"clone --recursive \"{GameVersions.UsaRepository}\" usa");
        }
        else
        {
            n++;
        }

        Next("step.submodules");
        await Run(T.S("step.submodules"), root, "git", "submodule update --init --recursive");

        if (v.Project == Project.Pal)
        {
            Next("step.fixes");
            await ApplyFixes(root);
        }

        Next("step.recompone");
        await Run(T.S("step.recompone"), root, "dotnet", "build \"RecompOne/RecompOne.sln\" -c Release");

        Next("step.generate");
        var cfg = WriteLauncherConfig(root, v, disc);
        try
        {
            await Run(T.S("step.generate"), root, "dotnet",
                      $"run -c Release --project \"RecompOne/RecompOne.Recompiler\" \"{cfg}\"");
        }
        finally
        {
            File.Delete(cfg);
        }

        Next("step.game");
        var lingua = v.Lingua.Length > 0 ? $" -p:Lingua={v.Lingua}" : "";
        await Run(T.S("step.game"), root, "dotnet", $"build RecompOne.MediEvil.csproj -c Release{lingua}");
        File.WriteAllText(Path.Combine(root, v.Generated, ".launcher-stamp"), Stamp(v, disc));

        if (v.Project == Project.Pal)
        {
            Next("step.shaders");
            await DownloadShaders(root);
        }
    }

    static readonly string[] Fixes = { "cd-whole-sector-mode", "shader-gbuffer", "effect-lights", "material-picker" };

    /// <summary>The fork's fixes to the RecompOne submodule, in order. They touch the same
    /// files, so "is this one applied" cannot be asked patch by patch once the later ones are
    /// in: a marker records the RecompOne commit and the patches applied on it. When it does
    /// not match, RecompOne's tracked files are reset and all the fixes applied again.</summary>
    async Task ApplyFixes(string root)
    {
        var rc = Path.Combine(root, "RecompOne");
        var marker = Path.Combine(root, ".recompone-fixes");
        var want = new StringBuilder("recompone " + GitHead(rc) + "\n");
        foreach (var fix in Fixes)
            want.AppendLine(fix + " " + Sha256(File.ReadAllBytes(Path.Combine(root, "recompone-fixes", fix + ".patch"))));
        if (File.Exists(marker) && File.ReadAllText(marker) == want.ToString())
        {
            _log("RecompOne fixes: already applied");
            return;
        }
        await Run(T.S("step.fixes"), rc, "git", "checkout -- .");
        foreach (var fix in Fixes)
            await Run(T.S("step.fixes"), rc, "git", $"apply \"../recompone-fixes/{fix}.patch\"");
        File.WriteAllText(marker, want.ToString());
    }

    /// <summary>Copy of the version's config next to it (relative paths unchanged) with
    /// "cue" pointing at the player's disc.</summary>
    static string WriteLauncherConfig(string root, GameVersion v, string disc)
    {
        var src = Path.Combine(root, v.Config);
        var node = JsonNode.Parse(File.ReadAllText(src))!;
        node["cue"] = Path.GetFullPath(disc);
        var dst = Path.Combine(Path.GetDirectoryName(src)!, "MediEvil.launcher.json");
        File.WriteAllText(dst, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        return dst;
    }

    /// <summary>Gallowmere (default) and Dan's Curse V2, like the build scripts; optional.</summary>
    async Task DownloadShaders(string root)
    {
        var shaders = Path.Combine(root, "shaders");
        Directory.CreateDirectory(shaders);
        foreach (var (repo, dir, marker) in new[]
                 {
                     ("https://github.com/Leproide/gallowmere-shader", "gallowmere-shader", "gallowmere.slangp"),
                     ("https://github.com/Leproide/DansCurseV2", "dans-curse-v2-shader", "dans-curse-v2.slangp"),
                 })
        {
            var fresh = Path.Combine(shaders, dir + ".new");
            var target = Path.Combine(shaders, dir);
            DeleteDir(fresh);
            await Run(T.S("step.shaders"), shaders, "git", $"clone -q --depth 1 {repo} \"{fresh}\"", allowFail: true);
            if (!File.Exists(Path.Combine(fresh, marker)))
            {
                DeleteDir(fresh);
                _log(T.S("noshaders"));
                continue;
            }
            DeleteDir(Path.Combine(fresh, ".git"));
            DeleteDir(target);
            Directory.Move(fresh, target);
        }
    }

    static void DeleteDir(string dir)
    {
        if (!Directory.Exists(dir)) return;
        foreach (var f in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories))
            File.SetAttributes(f, FileAttributes.Normal);
        Directory.Delete(dir, true);
    }

    /// <summary>git pull of the version's project. The PAL project's RecompOne fixes are
    /// taken out first (the submodule may move to another commit) and put back by the build.</summary>
    public async Task Update(GameVersion v, Action<string, int, int> stage)
    {
        var root = Root(v);
        await EnsureTools();
        stage(T.S("step.pull"), 1, 2);
        if (v.Project == Project.Pal)
            await Run(T.S("step.pull"), Path.Combine(root, "RecompOne"), "git", "checkout -- .", allowFail: true);
        await Run(T.S("step.pull"), root, "git", "pull --ff-only");
        stage(T.S("step.submodules"), 2, 2);
        await Run(T.S("step.submodules"), root, "git", "submodule update --init --recursive");
    }

    // ------------------------------------------------------------------ play

    public Process Start(GameVersion v, string disc)
    {
        var root = Root(v);
        var psi = new ProcessStartInfo(v.ExePath(root), $"\"{Path.GetFullPath(disc)}\"")
        {
            WorkingDirectory = root, UseShellExecute = false,
        };
        Tools.Apply(psi);   // the game needs the .NET 10 runtime: the private SDK's when used
        return Process.Start(psi)!;
    }
}
