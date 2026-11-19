// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - main window: choose the disc (or one remembered), see which release it
// is, build it once, play. "Play" builds first when the version is not built or out of date.
// Also: graphics settings before starting, saves folder and automatic backup before an
// update, desktop shortcut, and a notice when the version's project has updates.

using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MediEvilLauncher;

public sealed class MainForm : Form
{
    static readonly Color Back = Color.FromArgb(24, 22, 28);
    static readonly Color Panel = Color.FromArgb(38, 35, 44);
    static readonly Color Text1 = Color.FromArgb(232, 228, 236);
    static readonly Color Text2 = Color.FromArgb(160, 154, 170);
    static readonly Color Accent = Color.FromArgb(92, 168, 84);
    static readonly Color Warn = Color.FromArgb(230, 180, 70);

    readonly LauncherSettings _settings = LauncherSettings.Load();
    ComboBox? _langBox;
    readonly Builder _builder;
    readonly ComboBox _disc = new();
    readonly Label _version = new();
    readonly Label _state = new();
    readonly Label _updates = new();
    readonly Label _stage = new();
    readonly ProgressBar _progress = new();
    readonly Button _play = new(), _build = new(), _update = new(), _browse = new(), _showLog = new();
    readonly Button _gfx = new(), _saves = new(), _shortcut = new();
    readonly TextBox _log = new();
    readonly StringBuilder _pending = new();
    readonly System.Windows.Forms.Timer _logTimer = new() { Interval = 150 };
    readonly string? _autoPlay;
    GameVersion? _current;
    string _playable = "";
    bool _busy, _filling;
    int _checkId;

    public MainForm(string? autoPlayDisc = null)
    {
        _autoPlay = autoPlayDisc;
        _builder = new Builder(Builder.FindPalRoot(), Log);
        Text = T.S("title");
        BackColor = Back;
        ForeColor = Text1;
        Font = new Font("Segoe UI", 10f);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 420);
        Size = new Size(860, 440);
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

        var title = new Label
        {
            Text = "MEDIEVIL", Font = new Font("Segoe UI", 26f, FontStyle.Bold), ForeColor = Accent,
            AutoSize = true, Margin = new Padding(0, 0, 0, 8),
        };
        var discLabel = new Label { Text = T.S("disc"), AutoSize = true, ForeColor = Text2 };
        _disc.DropDownStyle = ComboBoxStyle.DropDownList;
        _disc.BackColor = Panel;
        _disc.ForeColor = Text1;
        _disc.FlatStyle = FlatStyle.Flat;
        _disc.Dock = DockStyle.Fill;
        _disc.SelectedIndexChanged += (_, _) => { if (!_filling) OnDiscChosen(_disc.SelectedItem as string); };
        Style(_browse, T.S("browse"));
        _browse.Click += (_, _) => Browse();

        var discRow = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Top, AutoSize = true };
        discRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        discRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        discRow.Controls.Add(_disc, 0, 0);
        discRow.Controls.Add(_browse, 1, 0);

        _version.AutoSize = true;
        _version.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        _version.Margin = new Padding(0, 10, 0, 2);
        _state.AutoSize = true;
        _state.ForeColor = Text2;
        _state.MaximumSize = new Size(800, 0);
        _updates.AutoSize = true;
        _updates.ForeColor = Warn;
        _stage.AutoSize = true;
        _stage.ForeColor = Text2;
        _progress.Dock = DockStyle.Top;
        _progress.Height = 10;
        _progress.Visible = false;

        Style(_play, T.S("play"), primary: true);
        Style(_build, T.S("build"));
        Style(_update, T.S("update"));
        Style(_gfx, T.S("settings"));
        Style(_saves, T.S("saves"));
        Style(_shortcut, T.S("shortcut"));
        Style(_showLog, T.S("log"));
        _play.Click += async (_, _) => await Play();
        _build.Click += async (_, _) => await BuildCurrent();
        _update.Click += async (_, _) => await UpdateCurrent();
        _gfx.Click += (_, _) => GraphicsSettings();
        _saves.Click += (_, _) => SavesMenu();
        _shortcut.Click += (_, _) => Shortcut();
        _showLog.Click += (_, _) => ToggleLog();

        var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 10, 0, 0) };
        buttons.Controls.AddRange(new Control[] { _play, _build, _update });
        var tools = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 6, 0, 0) };
        tools.Controls.AddRange(new Control[] { _gfx, _saves, _shortcut, _showLog });

        var note = new Label { Text = T.S("nogamedata"), AutoSize = true, ForeColor = Text2, Font = new Font("Segoe UI", 8.5f) };

        _log.Multiline = true;
        _log.ReadOnly = true;
        _log.ScrollBars = ScrollBars.Vertical;
        _log.BackColor = Color.FromArgb(14, 13, 17);
        _log.ForeColor = Text2;
        _log.Font = new Font("Consolas", 8.5f);
        _log.Dock = DockStyle.Fill;
        _log.Visible = false;

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), ColumnCount = 1 };
        // language of the launcher: saved in launcher.json, applied by restarting the launcher
        var lang = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, Width = 200,
            BackColor = Panel, ForeColor = Text1, Anchor = AnchorStyles.Top | AnchorStyles.Right,
        };
        lang.Items.Add(T.S("language.auto"));
        foreach (var (_, name) in T.Languages) lang.Items.Add(name);
        lang.SelectedIndex = Array.FindIndex(T.Languages, l => l.Code == _settings.Language) + 1;
        lang.SelectedIndexChanged += (_, _) => ChangeLanguage(lang.SelectedIndex == 0 ? null : T.Languages[lang.SelectedIndex - 1].Code);
        var langRow = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        langRow.Controls.Add(new Label { Text = T.S("language"), AutoSize = true, ForeColor = Text2, Margin = new Padding(0, 6, 6, 0) });
        langRow.Controls.Add(lang);
        var header = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Top, AutoSize = true };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.Controls.Add(title, 0, 0);
        header.Controls.Add(langRow, 1, 0);
        _langBox = lang;
        foreach (var c in new Control[] { header, discLabel, discRow, _version, _state, _updates, _stage, _progress, buttons, tools, note })
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(c);
        }
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(_log);
        Controls.Add(layout);

        _logTimer.Tick += (_, _) => FlushLog();
        _logTimer.Start();
        Shown += async (_, _) => await OnShownAsync();
    }

    void Style(Button b, string text, bool primary = false)
    {
        b.Text = text;
        b.AutoSize = true;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderColor = primary ? Accent : Color.FromArgb(70, 66, 78);
        b.BackColor = primary ? Accent : Panel;
        b.ForeColor = primary ? Color.Black : Text1;
        b.Padding = new Padding(14, 4, 14, 4);
        b.Margin = new Padding(0, 0, 8, 0);
        if (primary) b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
    }

    // ------------------------------------------------------------------ log

    void Log(string line)
    {
        lock (_pending) _pending.AppendLine(line);
    }

    void FlushLog()
    {
        string s;
        lock (_pending)
        {
            if (_pending.Length == 0) return;
            s = _pending.ToString();
            _pending.Clear();
        }
        if (_log.TextLength > 400_000) _log.Text = _log.Text[^200_000..];
        _log.AppendText(s);
    }

    void ToggleLog()
    {
        _log.Visible = !_log.Visible;
        if (_log.Visible && Height < 660) Height = 660;
    }

    // ------------------------------------------------------------------ discs

    void FillDiscs(string? select)
    {
        _filling = true;
        _disc.Items.Clear();
        foreach (var d in _settings.Discs.Where(File.Exists)) _disc.Items.Add(d);
        if (select != null && !_disc.Items.Contains(select)) _disc.Items.Insert(0, select);
        if (select != null) _disc.SelectedItem = select;
        _filling = false;
    }

    void Browse()
    {
        using var dlg = new OpenFileDialog
        {
            Title = T.S("pick"),
            Filter = "PlayStation (*.cue;*.chd;*.vcd)|*.cue;*.chd;*.vcd|*.*|*.*",
        };
        if (_disc.SelectedItem is string cur && File.Exists(cur)) dlg.InitialDirectory = Path.GetDirectoryName(cur);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        OnDiscChosen(dlg.FileName);
    }

    async void OnDiscChosen(string? path)
    {
        if (path == null || !File.Exists(path)) return;
        _settings.Remember(path);
        FillDiscs(path);
        await PrepareAndRefresh(path);
    }

    /// <summary>Converts a .VCD if needed, then identifies the disc.</summary>
    async Task PrepareAndRefresh(string path)
    {
        if (DiscSource.NeedsConversion(path))
        {
            _busy = true;
            UpdateButtons();
            _progress.Visible = true;
            _progress.Maximum = 100;
            _stage.Text = T.S("converting");
            try
            {
                await Task.Run(() => DiscSource.Prepare(path, p => BeginInvoke(() => _progress.Value = Math.Clamp(p, 0, 100))));
            }
            catch (Exception e)
            {
                Log(e.ToString());
                _stage.Text = T.F("failed", T.S("converting"));
            }
            finally
            {
                _busy = false;
                _progress.Visible = false;
            }
            _stage.Text = "";
        }
        RefreshState();
    }

    // ------------------------------------------------------------------ flow

    async Task OnShownAsync()
    {
        if (!Builder.IsPalRoot(_builder.PalRoot))
        {
            if (MessageBox.Show(this, T.F("need.project", _builder.PalRoot), T.S("title"),
                                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                Close();
                return;
            }
            try
            {
                await Busy(() => Task.Run(() => _builder.ClonePal(s => BeginInvoke(() => _stage.Text = s + "..."))));
            }
            catch (Builder.StepFailed)
            {
                return;
            }
        }
        var start = _autoPlay ?? (_settings.Disc.Length > 0 && File.Exists(_settings.Disc) ? _settings.Disc : null);
        FillDiscs(start);
        if (start != null)
        {
            if (_autoPlay != null) _settings.Remember(_autoPlay);
            await PrepareAndRefresh(start);
        }
        else
        {
            RefreshState();
        }
        if (_autoPlay != null && _current != null) await Play();
    }

    void RefreshState()
    {
        _current = null;
        _playable = "";
        _version.Text = "";
        _state.Text = "";
        _updates.Text = "";
        var path = _disc.SelectedItem as string;
        if (path == null || !File.Exists(path))
        {
            _state.Text = T.S("pick");
            UpdateButtons();
            return;
        }
        _playable = DiscSource.Playable(path);
        var cnf = File.Exists(_playable) ? Builder.ReadSystemCnf(_playable) : null;
        if (cnf == null)
        {
            _state.Text = T.S("unreadable");
            UpdateButtons();
            return;
        }
        _current = GameVersions.FromSystemCnf(cnf);
        if (_current == null)
        {
            var boot = cnf.Split('\n').FirstOrDefault(l => l.Contains("BOOT", StringComparison.OrdinalIgnoreCase))?.Trim() ?? "?";
            _state.Text = T.F("unknown", boot);
            UpdateButtons();
            return;
        }
        var variant = DiscSource.Variant(_playable);
        _version.Text = T.S("ver." + _current.Code) + (variant != null ? "  -  " + T.S(variant) : "");
        _state.Text = _builder.State(_current, _playable) switch
        {
            BuildState.Ready => T.S("state.ready"),
            BuildState.Stale => T.S("state.stale"),
            BuildState.ProjectMissing => T.S("state.nousa"),
            _ => T.S("state.notbuilt"),
        };
        UpdateButtons();
        CheckUpdatesInBackground(_current);
    }

    /// <summary>git fetch in the background; shows how many updates the project has.</summary>
    void CheckUpdatesInBackground(GameVersion v)
    {
        if (!_builder.ProjectPresent(v)) return;
        var id = ++_checkId;
        var root = _builder.Root(v);
        Task.Run(() =>
        {
            var n = Extras.CommitsBehind(root);
            BeginInvoke(() =>
            {
                if (id != _checkId || _current != v) return;
                _updates.Text = n > 0 ? T.F("updates.available", n) : "";
                _update.FlatAppearance.BorderColor = n > 0 ? Warn : Color.FromArgb(70, 66, 78);
            });
        });
    }

    void UpdateButtons()
    {
        var ok = _current != null && !_busy;
        var present = ok && _builder.ProjectPresent(_current!);
        _play.Enabled = ok;
        _build.Enabled = ok;
        _build.Text = ok && _builder.State(_current!, _playable) == BuildState.Ready ? T.S("rebuild") : T.S("build");
        _update.Enabled = present;
        _gfx.Enabled = present;
        _saves.Enabled = present;
        _shortcut.Enabled = ok;
        _browse.Enabled = !_busy;
        if (_langBox != null) _langBox.Enabled = !_busy;
        _disc.Enabled = !_busy;
    }

    async Task Busy(Func<Task> work)
    {
        _busy = true;
        UpdateButtons();
        _progress.Visible = true;
        try
        {
            await work();
        }
        catch (Builder.StepFailed e)
        {
            _stage.Text = T.F("failed", e.Message);
            if (!_log.Visible) ToggleLog();
            throw;
        }
        finally
        {
            _busy = false;
            _progress.Visible = false;
            FlushLog();
            UpdateButtons();
        }
    }

    Action<string, int, int> Stage => (s, n, total) => BeginInvoke(() =>
    {
        _stage.Text = $"{n}/{total}  {s}...";
        _progress.Maximum = total;
        _progress.Value = Math.Min(n, total);
    });

    async Task<bool> BuildCurrent()
    {
        if (_current == null) return false;
        var v = _current;
        var disc = _playable;
        try
        {
            await Busy(() => Task.Run(() => _builder.Build(v, disc, Stage)));
        }
        catch (Builder.StepFailed)
        {
            return false;
        }
        RefreshState();
        _stage.Text = T.S("done.build");
        return true;
    }

    async Task UpdateCurrent()
    {
        if (_current == null) return;
        var v = _current;
        var backup = Extras.BackupSaves(_builder.Root(v));
        if (backup != null) Log(T.F("backup.done", backup));
        try
        {
            await Busy(() => Task.Run(() => _builder.Update(v, Stage)));
        }
        catch (Builder.StepFailed)
        {
            return;
        }
        RefreshState();
        _stage.Text = T.S("done.update") + (backup != null ? "  " + T.S("backup.short") : "");
    }

    void GraphicsSettings()
    {
        if (_current == null) return;
        using var f = new SettingsForm(_builder.Root(_current));
        f.ShowDialog(this);
    }

    /// <summary>Saves the language and restarts the launcher in it (not while building or playing).</summary>
    void ChangeLanguage(string? code)
    {
        if (code == _settings.Language) return;
        _settings.Language = code;
        _settings.Save();
        // the games' interface follows the launcher
        foreach (var root in GameVersions.All.Select(_builder.Root).Distinct())
        {
            try
            {
                GameSettings.SetLanguage(root, code);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }
        if (_busy) return;   // applied at the next start
        try
        {
            Process.Start(new ProcessStartInfo(Environment.ProcessPath!)
            {
                WorkingDirectory = Environment.CurrentDirectory, UseShellExecute = false,
            });
            Close();
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }
    }

    void SavesMenu()
    {
        if (_current == null) return;
        var root = _builder.Root(_current);
        var menu = new ContextMenuStrip { Font = Font };
        menu.Items.Add(T.S("saves.open"), null, (_, _) => Extras.OpenFolder(root));
        menu.Items.Add(T.S("saves.backup"), null, (_, _) =>
        {
            var b = Extras.BackupSaves(root);
            _stage.Text = b != null ? T.F("backup.done", b) : T.S("saves.nothing");
        });
        var restore = new ToolStripMenuItem(T.S("saves.restore"));
        var backups = Extras.Backups(root);
        foreach (var b in backups)
        {
            var name = Path.GetFileName(b);
            restore.DropDownItems.Add(name, null, (_, _) =>
            {
                if (MessageBox.Show(this, T.F("saves.confirm", name), T.S("saves"), MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question) != DialogResult.OK) return;
                Extras.RestoreSaves(root, b);
                _stage.Text = T.F("saves.restored", name);
            });
        }
        if (backups.Length == 0) restore.DropDownItems.Add(new ToolStripMenuItem(T.S("saves.none")) { Enabled = false });
        menu.Items.Add(restore);
        menu.Items.Add(T.S("saves.backups"), null, (_, _) => Extras.OpenFolder(Path.Combine(root, "backups")));
        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(_saves, new Point(0, _saves.Height));
    }

    void Shortcut()
    {
        if (_current == null || _disc.SelectedItem is not string disc) return;
        try
        {
            var lnk = Extras.CreateShortcut(_version.Text.Split("  -  ")[0], disc);
            _stage.Text = T.F("shortcut.done", Path.GetFileName(lnk));
        }
        catch (Exception e)
        {
            Log(e.ToString());
            _stage.Text = T.F("failed", T.S("shortcut"));
        }
    }

    async Task Play()
    {
        if (_current == null) return;
        if (_builder.State(_current, _playable) != BuildState.Ready && !await BuildCurrent()) return;
        var game = _builder.Start(_current, _playable);
        _stage.Text = T.S("running");
        _busy = true;
        UpdateButtons();
        WindowState = FormWindowState.Minimized;
        await game.WaitForExitAsync();
        _busy = false;
        UpdateButtons();
        WindowState = FormWindowState.Normal;
        Activate();
        _stage.Text = "";
    }
}
