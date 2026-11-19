// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - the main graphics settings, set before starting the game.
//
// They are the game's own options (RecompOne and the fork), stored by the game in
// settings.json under "options" as flat keys: options["display.render_scale"], not nested
// objects. The game reads them at start, so they are written only while it is not running;
// every other key of the file is kept as it is.
//
// GameSettings also writes system.discPath, the disc the game opens: the game ignores a disc
// given on its command line and asks for one when that path is missing or another version's.

using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace MediEvilLauncher;

public sealed class SettingsForm : Form
{
    // display.fps values the game accepts (RecompOne Interp.Steps; 0 = original frame rate)
    static readonly int[] FpsSteps = { 0, 24, 25, 30, 48, 50, 60, 72, 75, 90, 100, 120, 144, 165, 180, 200, 240 };

    readonly string _file;
    readonly JsonObject _root;
    readonly TrackBar _scale = new() { Minimum = 1, Maximum = 8, TickFrequency = 1, Width = 220 };
    readonly Label _scaleLabel = new() { AutoSize = true };
    readonly ComboBox _fps = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    readonly CheckBox _fullscreen = new() { AutoSize = true };
    readonly CheckBox _vsync = new() { AutoSize = true };
    readonly CheckBox _widescreen = new() { AutoSize = true };

    public SettingsForm(string gameRoot)
    {
        _file = Path.Combine(gameRoot, "settings.json");
        _root = GameSettings.Load(_file);
        Text = T.S("settings.title");
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Font = new Font("Segoe UI", 10f);
        Padding = new Padding(16);

        _scale.Value = Math.Clamp(GetInt("display.render_scale", 4), 1, 8);
        _scale.ValueChanged += (_, _) => UpdateScaleLabel();
        UpdateScaleLabel();
        foreach (var f in FpsSteps) _fps.Items.Add(f == 0 ? T.S("settings.fps.native") : $"{f} FPS");
        _fps.SelectedIndex = Math.Max(0, Array.IndexOf(FpsSteps, GetInt("display.fps", 0)));
        _fullscreen.Text = T.S("settings.fullscreen");
        _fullscreen.Checked = GetBool("display.fullscreen", false);
        _vsync.Text = T.S("settings.vsync");
        _vsync.Checked = GetBool("display.vsync", false);
        _widescreen.Text = T.S("settings.widescreen");
        _widescreen.Checked = !GetBool("widescreen.original_aspect", false);

        var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
        var cancel = new Button { Text = T.S("cancel"), DialogResult = DialogResult.Cancel, AutoSize = true };
        ok.Click += (_, _) => Save();
        AcceptButton = ok;
        CancelButton = cancel;

        var grid = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = DockStyle.Fill };
        void Row(string label, Control c)
        {
            grid.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 6, 12, 6) });
            grid.Controls.Add(c);
        }
        var scaleRow = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
        scaleRow.Controls.AddRange(new Control[] { _scale, _scaleLabel });
        Row(T.S("settings.scale"), scaleRow);
        Row(T.S("settings.fps"), _fps);
        Row("", _fullscreen);
        Row("", _vsync);
        Row("", _widescreen);
        var buttons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Bottom };
        buttons.Controls.AddRange(new Control[] { cancel, ok });
        var note = new Label { Text = T.S("settings.note"), AutoSize = true, MaximumSize = new Size(420, 0), ForeColor = Color.DimGray };
        var outer = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Dock = DockStyle.Fill };
        outer.Controls.AddRange(new Control[] { grid, note, buttons });
        Controls.Add(outer);
    }

    void UpdateScaleLabel()
    {
        int s = _scale.Value;
        _scaleLabel.Text = $"{s}x ({320 * s}x{240 * s})";
    }

    JsonNode? Node(string key) => GameSettings.Options(_root)[key];

    int GetInt(string key, int def) { try { return Node(key)?.GetValue<int>() ?? def; } catch { return def; } }
    bool GetBool(string key, bool def) { try { return Node(key)?.GetValue<bool>() ?? def; } catch { return def; } }

    void Save()
    {
        var o = GameSettings.Options(_root);
        o["display.render_scale"] = _scale.Value;
        o["display.fps"] = FpsSteps[Math.Max(0, _fps.SelectedIndex)];
        o["display.fullscreen"] = _fullscreen.Checked;
        o["display.vsync"] = _vsync.Checked;
        o["widescreen.original_aspect"] = !_widescreen.Checked;
        GameSettings.Write(_file, _root);
    }
}

/// <summary>Reads and writes the game's settings.json, keeping every key it does not touch.</summary>
public static class GameSettings
{
    public static JsonObject Load(string file)
    {
        try
        {
            if (File.Exists(file) && JsonNode.Parse(File.ReadAllText(file)) is JsonObject o) return o;
        }
        catch
        {
            // unreadable: start from an empty file, the game fills in the rest
        }
        return new JsonObject();
    }

    /// <summary>The "options" object. Nested objects such as "display": { "fps": 60 } were
    /// written by launcher 1.0 and are ignored by the game: their values move to the flat keys
    /// when those are not set, and the objects are removed.</summary>
    public static JsonObject Options(JsonObject root)
    {
        if (root["options"] is not JsonObject o) root["options"] = o = new JsonObject();
        foreach (var name in new[] { "display", "widescreen" })
        {
            if (o[name] is not JsonObject nested) continue;
            foreach (var (k, v) in nested.ToArray())
                if (!o.ContainsKey($"{name}.{k}")) o[$"{name}.{k}"] = v?.DeepClone();
            o.Remove(name);
        }
        return o;
    }

    public static void Write(string file, JsonObject root)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        var temp = file + ".tmp";
        File.WriteAllText(temp, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        File.Move(temp, file, true);
    }

    /// <summary>Sets system.discPath, so the game opens this disc without asking.</summary>
    public static void SetDisc(string gameRoot, string disc)
    {
        var file = Path.Combine(gameRoot, "settings.json");
        var root = Load(file);
        if (root["system"] is not JsonObject system) root["system"] = system = new JsonObject();
        system["discPath"] = disc;
        Options(root);
        Write(file, root);
    }

    const string LanguageKey = "interface.language";

    /// <summary>Sets the language of the game's interface (options["interface.language"]) to the
    /// launcher's: null (automatic) removes it, the game then follows Windows. With onlyIfUnset,
    /// a language already chosen in the game is kept.</summary>
    public static void SetLanguage(string gameRoot, string? launcherCode, bool onlyIfUnset = false)
    {
        if (!Directory.Exists(gameRoot)) return;
        var file = Path.Combine(gameRoot, "settings.json");
        var root = Load(file);
        var o = Options(root);
        if (onlyIfUnset && (o.ContainsKey(LanguageKey) || launcherCode == null)) return;
        var code = GameLanguage(launcherCode);
        if (code == null) o.Remove(LanguageKey);
        else o[LanguageKey] = code;
        Write(file, root);
    }

    /// <summary>The game's code for a launcher language (assets/languages.json; the game also
    /// matches by the first part, "fr" for "fr-FR").</summary>
    static string? GameLanguage(string? code) => code switch
    {
        null => null,
        "fr" => "fr-fr",
        "it" => "it-IT",
        "es" => "es-419",
        "de" => "de-DE",
        _ => code,
    };
}
