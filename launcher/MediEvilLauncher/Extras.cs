// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - saves (folder, backups), desktop shortcut, update check.
//
// The game keeps its memory cards (carda.sav, cardb.sav) and settings.json in the folder it
// runs from: the PAL project root for the PAL versions (shared by them), usa\ for the USA one.

using System.Diagnostics;

namespace MediEvilLauncher;

public static class Extras
{
    static readonly string[] SaveFiles = { "carda.sav", "cardb.sav", "settings.json" };

    /// <summary>Copies the memory cards and settings to backups\yyyy-MM-dd_HH-mm-ss\ in the
    /// game folder. Returns the backup folder, or null when there is nothing to back up.</summary>
    public static string? BackupSaves(string gameRoot)
    {
        var files = SaveFiles.Select(f => Path.Combine(gameRoot, f)).Where(File.Exists).ToArray();
        if (files.Length == 0) return null;
        var dir = Path.Combine(gameRoot, "backups", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Directory.CreateDirectory(dir);
        foreach (var f in files) File.Copy(f, Path.Combine(dir, Path.GetFileName(f)), true);
        return dir;
    }

    /// <summary>Backup folders of this game folder, newest first.</summary>
    public static string[] Backups(string gameRoot)
    {
        var dir = Path.Combine(gameRoot, "backups");
        return Directory.Exists(dir)
            ? Directory.GetDirectories(dir).OrderByDescending(Path.GetFileName, StringComparer.Ordinal).ToArray()
            : Array.Empty<string>();
    }

    /// <summary>Puts back the memory cards of a backup (settings.json stays as it is), after
    /// backing up the current ones. Returns the backup of the current cards, or null.</summary>
    public static string? RestoreSaves(string gameRoot, string backup)
    {
        var current = BackupSaves(gameRoot);
        foreach (var f in new[] { "carda.sav", "cardb.sav" })
        {
            var src = Path.Combine(backup, f);
            if (File.Exists(src)) File.Copy(src, Path.Combine(gameRoot, f), true);
        }
        return current;
    }

    public static void OpenFolder(string dir)
    {
        Directory.CreateDirectory(dir);
        Process.Start(new ProcessStartInfo("explorer.exe", $"\"{dir}\"") { UseShellExecute = true });
    }

    /// <summary>Desktop shortcut that starts the launcher on this disc and plays at once.</summary>
    public static string CreateShortcut(string title, string disc)
    {
        var exe = Environment.ProcessPath!;
        var name = string.Concat(title.Split(Path.GetInvalidFileNameChars()));
        var lnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), name + ".lnk");
        var shellType = Type.GetTypeFromProgID("WScript.Shell") ?? throw new InvalidOperationException("WScript.Shell");
        dynamic shell = Activator.CreateInstance(shellType)!;
        dynamic sc = shell.CreateShortcut(lnk);
        sc.TargetPath = exe;
        sc.Arguments = $"--play \"{disc}\"";
        sc.WorkingDirectory = Path.GetDirectoryName(exe);
        sc.IconLocation = exe + ",0";
        sc.Description = title;
        sc.Save();
        return lnk;
    }

    /// <summary>Number of commits the project's branch is behind its upstream after a fetch
    /// (0 = up to date, -1 = unknown: offline, no upstream, no git).</summary>
    public static int CommitsBehind(string root)
    {
        if (Run(root, "fetch --quiet") != 0) return -1;
        var (code, output) = Capture(root, "rev-list --count HEAD..@{u}");
        return code == 0 && int.TryParse(output.Trim(), out var n) ? n : -1;
    }

    static int Run(string dir, string args) => Capture(dir, args).code;

    static (int code, string output) Capture(string dir, string args)
    {
        try
        {
            var psi = new ProcessStartInfo(Tools.Git, args)
            {
                WorkingDirectory = dir, RedirectStandardOutput = true, RedirectStandardError = true,
                UseShellExecute = false, CreateNoWindow = true,
            };
            Tools.Apply(psi);
            using var p = Process.Start(psi)!;
            var s = p.StandardOutput.ReadToEnd();
            p.WaitForExit(60_000);
            return (p.HasExited ? p.ExitCode : -1, s);
        }
        catch
        {
            return (-1, "");
        }
    }
}
