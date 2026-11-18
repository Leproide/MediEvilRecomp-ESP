// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - the main graphics settings, set before starting the game.
//
// They are the game's own options (RecompOne and the fork), stored by the game in
// settings.json under "options" ("display.render_scale" -> options.display.render_scale).
// The game reads them at start, so they are written only while it is not running; every
// other key of the file is kept as it is.

using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace MediEvilLauncher;

public sealed class SettingsForm : Form
{
    static readonly int[] FpsSteps = { 0, 30, 50, 60, 72, 75, 90, 100, 120, 144, 165, 240 };

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
        _root = LoadFile(_file);
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

    static JsonObject LoadFile(string file)
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

    JsonNode? Node(string key)
    {
        JsonNode? n = _root["options"];
        foreach (var part in key.Split('.'))
            n = n is JsonObject o ? o[part] : null;
        return n;
    }

    int GetInt(string key, int def) { try { return Node(key)?.GetValue<int>() ?? def; } catch { return def; } }
    bool GetBool(string key, bool def) { try { return Node(key)?.GetValue<bool>() ?? def; } catch { return def; } }

    void Set(string key, JsonNode value)
    {
        if (_root["options"] is not JsonObject o) _root["options"] = o = new JsonObject();
        var parts = key.Split('.');
        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (o[parts[i]] is not JsonObject next) o[parts[i]] = next = new JsonObject();
            o = next;
        }
        o[parts[^1]] = value;
    }

    void Save()
    {
        Set("display.render_scale", _scale.Value);
        Set("display.fps", FpsSteps[Math.Max(0, _fps.SelectedIndex)]);
        Set("display.fullscreen", _fullscreen.Checked);
        Set("display.vsync", _vsync.Checked);
        Set("widescreen.original_aspect", !_widescreen.Checked);
        Directory.CreateDirectory(Path.GetDirectoryName(_file)!);
        File.WriteAllText(_file, _root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }
}
