// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - remembered choices (%APPDATA%\MediEvilLauncher\launcher.json).

using System.Text.Json;

namespace MediEvilLauncher;

public sealed class LauncherSettings
{
    /// <summary>Disc chosen last.</summary>
    public string Disc { get; set; } = "";

    /// <summary>Every disc chosen so far (most recent first), for the disc list.</summary>
    public List<string> Discs { get; set; } = new();

    /// <summary>Interface language (en, fr, it, es, de); null follows Windows.</summary>
    public string? Language { get; set; }

    public void Remember(string disc)
    {
        Disc = disc;
        Discs.RemoveAll(d => string.Equals(d, disc, StringComparison.OrdinalIgnoreCase));
        Discs.Insert(0, disc);
        Discs = Discs.Where(File.Exists).Take(12).ToList();
        Save();
    }

    static string FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MediEvilLauncher", "launcher.json");

    public static LauncherSettings Load()
    {
        try
        {
            return JsonSerializer.Deserialize<LauncherSettings>(File.ReadAllText(FilePath)) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
            // a launcher that cannot remember the disc still works
        }
    }
}
