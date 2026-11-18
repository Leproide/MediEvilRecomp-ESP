// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - main window: choose the disc, see which release it is, build it once,
// play. "Play" builds first when the version is not built or out of date.

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

    readonly LauncherSettings _settings = LauncherSettings.Load();
    readonly Builder _builder;
    readonly TextBox _disc = new();
    readonly Label _version = new();
    readonly Label _state = new();
    readonly Label _stage = new();
    readonly ProgressBar _progress = new();
    readonly Button _play = new(), _build = new(), _update = new(), _browse = new(), _showLog = new();
    readonly TextBox _log = new();
    readonly StringBuilder _pending = new();
    readonly System.Windows.Forms.Timer _logTimer = new() { Interval = 150 };
    GameVersion? _current;
    bool _busy;

    public MainForm()
    {
        _builder = new Builder(Builder.FindPalRoot(), Log);
        Text = T.S("title");
        BackColor = Back;
        ForeColor = Text1;
        Font = new Font("Segoe UI", 10f);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(720, 360);
        Size = new Size(820, 400);
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

        var title = new Label
        {
            Text = "MEDIEVIL", Font = new Font("Segoe UI", 26f, FontStyle.Bold), ForeColor = Accent,
            AutoSize = true, Margin = new Padding(0, 0, 0, 8),
        };
        var discLabel = new Label { Text = T.S("disc"), AutoSize = true, ForeColor = Text2 };
        _disc.ReadOnly = true;
        _disc.BackColor = Panel;
        _disc.ForeColor = Text1;
        _disc.BorderStyle = BorderStyle.FixedSingle;
        _disc.Dock = DockStyle.Fill;
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
        _state.MaximumSize = new Size(760, 0);
        _stage.AutoSize = true;
        _stage.ForeColor = Text2;
        _progress.Dock = DockStyle.Top;
        _progress.Height = 10;
        _progress.Visible = false;

        Style(_play, T.S("play"), primary: true);
        Style(_build, T.S("build"));
        Style(_update, T.S("update"));
        Style(_showLog, T.S("log"));
        _play.Click += async (_, _) => await Play();
        _build.Click += async (_, _) => await BuildCurrent();
        _update.Click += async (_, _) => await UpdateCurrent();
        _showLog.Click += (_, _) => ToggleLog();

        var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 10, 0, 0) };
        buttons.Controls.AddRange(new Control[] { _play, _build, _update, _showLog });

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
        foreach (var c in new Control[] { title, discLabel, discRow, _version, _state, _stage, _progress, buttons, note })
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
        if (_log.Visible && Height < 640) Height = 640;
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
        if (_settings.Disc.Length > 0 && File.Exists(_settings.Disc))
            _disc.Text = _settings.Disc;
        RefreshState();
    }

    void Browse()
    {
        using var dlg = new OpenFileDialog
        {
            Title = T.S("pick"),
            Filter = "PlayStation (*.cue;*.chd)|*.cue;*.chd|*.*|*.*",
        };
        if (File.Exists(_disc.Text)) dlg.InitialDirectory = Path.GetDirectoryName(_disc.Text);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        _disc.Text = dlg.FileName;
        _settings.Disc = dlg.FileName;
        _settings.Save();
        RefreshState();
    }

    void RefreshState()
    {
        _current = null;
        _version.Text = "";
        _state.Text = "";
        _stage.Text = "";
        if (_disc.Text.Length == 0 || !File.Exists(_disc.Text))
        {
            _state.Text = T.S("pick");
            UpdateButtons();
            return;
        }
        var cnf = Builder.ReadSystemCnf(_disc.Text);
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
        _version.Text = T.S("ver." + _current.Code);
        _state.Text = _builder.State(_current, _disc.Text) switch
        {
            BuildState.Ready => T.S("state.ready"),
            BuildState.Stale => T.S("state.stale"),
            BuildState.ProjectMissing => T.S("state.nousa"),
            _ => T.S("state.notbuilt"),
        };
        UpdateButtons();
    }

    void UpdateButtons()
    {
        var ok = _current != null && !_busy;
        _play.Enabled = ok;
        _build.Enabled = ok;
        _build.Text = ok && _builder.State(_current!, _disc.Text) == BuildState.Ready ? T.S("rebuild") : T.S("build");
        _update.Enabled = ok && _builder.ProjectPresent(_current!);
        _browse.Enabled = !_busy;
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

    async Task<bool> BuildCurrent()
    {
        if (_current == null) return false;
        var v = _current;
        var disc = _disc.Text;
        try
        {
            await Busy(() => Task.Run(() => _builder.Build(v, disc, (s, n, total) => BeginInvoke(() =>
            {
                _stage.Text = $"{n}/{total}  {s}...";
                _progress.Maximum = total;
                _progress.Value = Math.Min(n, total);
            }))));
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
        try
        {
            await Busy(() => Task.Run(() => _builder.Update(v, (s, n, total) => BeginInvoke(() =>
            {
                _stage.Text = $"{n}/{total}  {s}...";
                _progress.Maximum = total;
                _progress.Value = Math.Min(n, total);
            }))));
        }
        catch (Builder.StepFailed)
        {
            return;
        }
        RefreshState();
        _stage.Text = T.S("done.update");
    }

    async Task Play()
    {
        if (_current == null) return;
        if (_builder.State(_current, _disc.Text) != BuildState.Ready && !await BuildCurrent()) return;
        var game = _builder.Start(_current, _disc.Text);
        _stage.Text = T.S("running");
        WindowState = FormWindowState.Minimized;
        await game.WaitForExitAsync();
        WindowState = FormWindowState.Normal;
        Activate();
        _stage.Text = "";
    }
}
