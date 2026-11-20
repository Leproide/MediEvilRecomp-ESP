// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - disc images the game cannot open directly, and known variants.
//
// RecompOne reads .cue/.bin and .chd. A PlayStation 2 POPS image (.VCD) is the same raw
// 2352-byte sectors after a 1 MiB header: the launcher writes the sectors once to a .bin with
// a .cue next to it, in %LOCALAPPDATA%\MediEvilLauncher\discs, and uses that. Variants are
// discs that boot a supported release with different data, such as the Portuguese fan
// translation of the French release: same code, so the French build plays it.

using System.Security.Cryptography;
using RecompOne.Runtime.Cdrom;

namespace MediEvilLauncher;

public static class DiscSource
{
    const long VcdHeader = 0x100000;
    const int Sector = 2352;

    static readonly string Cache = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MediEvilLauncher", "discs");

    /// <summary>MEDIEVIL.EXE (SHA-256) of known variants -> interface text key.</summary>
    static readonly Dictionary<string, string> Variants = new(StringComparer.OrdinalIgnoreCase)
    {
        ["848296bd947c7d290ebf10a86287b10b1cfd432eae099c7ea92a1c138d9aea91"] = "variant.pt",
    };

    public static bool NeedsConversion(string path) =>
        path.EndsWith(".vcd", StringComparison.OrdinalIgnoreCase);

    /// <summary>A path the game can open: the image itself, or its converted copy.</summary>
    public static string Playable(string path)
    {
        if (!NeedsConversion(path)) return path;
        var name = Path.GetFileNameWithoutExtension(path);
        return Path.Combine(Cache, name + ".cue");
    }

    /// <summary>Converts a .VCD once (again when the source changes). Returns the .cue.</summary>
    public static string Prepare(string path, Action<int>? percent = null)
    {
        if (!NeedsConversion(path)) return path;
        Directory.CreateDirectory(Cache);
        var cue = Playable(path);
        var bin = Path.ChangeExtension(cue, ".bin");
        var src = new FileInfo(path);
        long size = src.Length - VcdHeader;
        size -= size % Sector;
        var stampFile = cue + ".src";
        var stamp = $"{src.FullName}|{src.Length}|{src.LastWriteTimeUtc.Ticks}";
        if (File.Exists(cue) && File.Exists(bin) && new FileInfo(bin).Length == size &&
            File.Exists(stampFile) && File.ReadAllText(stampFile) == stamp)
            return cue;

        using (var input = File.OpenRead(path))
        using (var output = File.Create(bin))
        {
            input.Seek(VcdHeader, SeekOrigin.Begin);
            var buf = new byte[Sector * 2048];
            long done = 0;
            while (done < size)
            {
                int n = input.Read(buf, 0, (int)Math.Min(buf.Length, size - done));
                if (n <= 0) break;
                output.Write(buf, 0, n);
                done += n;
                percent?.Invoke((int)(done * 100 / size));
            }
        }
        File.WriteAllText(cue, $"FILE \"{Path.GetFileName(bin)}\" BINARY\r\n  TRACK 01 MODE2/2352\r\n    INDEX 01 00:00:00\r\n");
        // the LibCrypt sector list (.sbi/.lsd) is looked up next to the disc: copy it along
        foreach (var ext in new[] { ".sbi", ".lsd" })
        {
            var side = Path.ChangeExtension(path, ext);
            if (File.Exists(side)) File.Copy(side, Path.ChangeExtension(cue, ext), true);
        }
        File.WriteAllText(stampFile, stamp);
        return cue;
    }

    /// <summary>Interface key of a known variant of the disc ("variant.pt"), or null.</summary>
    public static string? Variant(string playable)
    {
        try
        {
            using var fs = DiscFs.Open(playable);
            var hash = Convert.ToHexString(SHA256.HashData(fs.ReadFile("MEDIEVIL.EXE")));
            return Variants.TryGetValue(hash, out var key) ? key : null;
        }
        catch
        {
            return null;
        }
    }
}
