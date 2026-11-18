// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - remembered choices (%APPDATA%\MediEvilLauncher\launcher.json).

using System.Text.Json;

namespace MediEvilLauncher;

public sealed class LauncherSettings
{
    public string Disc { get; set; } = "";

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
