// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - the build tools, without asking the player to install anything.
//
// The launcher needs the .NET 10 SDK (to recompile and build the game) and Git (to download
// the project and apply the RecompOne fixes). When the computer has them, those are used.
// Otherwise private copies are downloaded once from the official sources into
// %LOCALAPPDATA%\MediEvilLauncher\tools: the SDK with Microsoft's dotnet-install script, Git as
// MinGit from the Git for Windows releases. Nothing is installed system-wide: no PATH change,
// no registry, no entry in the installed programs; deleting the folder removes them.

using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;

namespace MediEvilLauncher;

public static class Tools
{
    static readonly string Dir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MediEvilLauncher", "tools");
    static readonly string PrivateDotnetDir = Path.Combine(Dir, "dotnet");
    static readonly string PrivateGitDir = Path.Combine(Dir, "git");

    /// <summary>dotnet executable to run ("dotnet" when the system one is suitable).</summary>
    public static string Dotnet { get; private set; } = "dotnet";
    /// <summary>git executable to run ("git" when the system one is present).</summary>
    public static string Git { get; private set; } = "git";
    /// <summary>DOTNET_ROOT for child processes (and the game) when the private SDK is used.</summary>
    public static string? DotnetRoot { get; private set; }

    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromMinutes(30) };

    static Tools()
    {
        Http.DefaultRequestHeaders.UserAgent.ParseAdd("MediEvilLauncher");
        Resolve();
    }

    /// <summary>Picks the system tools, else the private copies already downloaded.</summary>
    static void Resolve()
    {
        // MEDIEVIL_LAUNCHER_PRIVATE_TOOLS=1 ignores the system tools (to test a clean computer)
        var system = Environment.GetEnvironmentVariable("MEDIEVIL_LAUNCHER_PRIVATE_TOOLS") != "1";
        Dotnet = Path.Combine(PrivateDotnetDir, "dotnet.exe");
        Git = Path.Combine(PrivateGitDir, "cmd", "git.exe");
        DotnetRoot = PrivateDotnetDir;
        if (system && SdkMajor("dotnet") >= 10)
        {
            Dotnet = "dotnet";
            DotnetRoot = null;
        }
        if (system && Works("git", "--version"))
            Git = "git";
    }

    public static string Describe() => $"dotnet: {Dotnet} (SDK {SdkMajor(Dotnet)})  git: {Git} ({(HaveGit ? "ok" : "missing")})";

    public static bool HaveDotnet => SdkMajor(Dotnet) >= 10;
    public static bool HaveGit => Works(Git, "--version");

    /// <summary>Downloads whatever is missing. Throws on failure.</summary>
    public static async Task Ensure(Action<string> log)
    {
        if (!HaveGit)
        {
            log("Git: downloading MinGit (Git for Windows)...");
            await DownloadMinGit(log);
        }
        if (!HaveDotnet)
        {
            log(".NET 10 SDK: downloading with dotnet-install (Microsoft)...");
            await DownloadSdk(log);
        }
        Resolve();
        if (!HaveGit) throw new InvalidOperationException("Git could not be set up");
        if (!HaveDotnet) throw new InvalidOperationException(".NET 10 SDK could not be set up");
    }

    static async Task DownloadSdk(Action<string> log)
    {
        Directory.CreateDirectory(Dir);
        var script = Path.Combine(Dir, "dotnet-install.ps1");
        await File.WriteAllBytesAsync(script, await Http.GetByteArrayAsync("https://dot.net/v1/dotnet-install.ps1"));
        var psi = new ProcessStartInfo("powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -File \"{script}\" -Channel 10.0 -InstallDir \"{PrivateDotnetDir}\" -NoPath")
        {
            RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true,
        };
        using var p = new Process { StartInfo = psi };
        p.OutputDataReceived += (_, e) => { if (e.Data != null) log(e.Data); };
        p.ErrorDataReceived += (_, e) => { if (e.Data != null) log(e.Data); };
        p.Start();
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        await p.WaitForExitAsync();
    }

    static async Task DownloadMinGit(Action<string> log)
    {
        var json = await Http.GetStringAsync("https://api.github.com/repos/git-for-windows/git/releases/latest");
        using var doc = JsonDocument.Parse(json);
        var url = doc.RootElement.GetProperty("assets").EnumerateArray()
            .Select(a => (name: a.GetProperty("name").GetString() ?? "", url: a.GetProperty("browser_download_url").GetString() ?? ""))
            .FirstOrDefault(a => a.name.StartsWith("MinGit-") && a.name.EndsWith("-64-bit.zip") && !a.name.Contains("busybox"))
            .url;
        if (string.IsNullOrEmpty(url)) throw new InvalidOperationException("MinGit download not found");
        log(url);
        var zip = Path.Combine(Dir, "mingit.zip");
        Directory.CreateDirectory(Dir);
        await File.WriteAllBytesAsync(zip, await Http.GetByteArrayAsync(url));
        if (Directory.Exists(PrivateGitDir)) Directory.Delete(PrivateGitDir, true);
        ZipFile.ExtractToDirectory(zip, PrivateGitDir);
        File.Delete(zip);
    }

    /// <summary>Environment for the processes the launcher starts (build steps and the game).</summary>
    public static void Apply(ProcessStartInfo psi)
    {
        if (DotnetRoot != null)
        {
            psi.Environment["DOTNET_ROOT"] = DotnetRoot;
            psi.Environment["PATH"] = DotnetRoot + ";" + Environment.GetEnvironmentVariable("PATH");
        }
        if (Git != "git")
            psi.Environment["PATH"] = Path.GetDirectoryName(Git) + ";" + psi.Environment["PATH"];
        psi.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        psi.Environment["DOTNET_NOLOGO"] = "1";
    }

    static int SdkMajor(string dotnet)
    {
        var (code, output) = Capture(dotnet, "--list-sdks");
        if (code != 0) return 0;
        return output.Split('\n').Select(l => int.TryParse(l.Split('.')[0].Trim(), out var m) ? m : 0)
                     .DefaultIfEmpty(0).Max();
    }

    static bool Works(string file, string args) => Capture(file, args).code == 0;

    static (int code, string output) Capture(string file, string args)
    {
        try
        {
            var p = Process.Start(new ProcessStartInfo(file, args)
            {
                RedirectStandardOutput = true, RedirectStandardError = true,
                UseShellExecute = false, CreateNoWindow = true,
            })!;
            var s = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return (p.ExitCode, s);
        }
        catch
        {
            return (-1, "");
        }
    }
}
