using Views.Components;
using Views.ViewModels;

namespace Views.Forms;

public partial class MainView : Krypton.Toolkit.KryptonForm
{
    private readonly MainViewModel _viewModel;
    private bool _exitRequested;
    private string? _lastFolder;
    private bool _busy;
    private int _dots;
    private readonly System.Windows.Forms.Timer _animTimer = new() { Interval = 400 };
    private string _busyBaseText = string.Empty;
    private Controller.Sync.DirectoryWatchController? _watcher;
    private DateTime _lastActivity = DateTime.UtcNow;
    private ActivityMessageFilter? _activityFilter;
    private readonly System.Windows.Forms.Timer _maintenanceTimer = new() { Interval = 60_000 };
    private bool _backupRunning;
    private SubViews.LockView? _lockView;

    private void RebuildNotifications()
    {
        if (IsDisposed || Disposing) return;
        if (statusStrip.InvokeRequired)
        {
            try { BeginInvoke(RebuildNotifications); } catch { }
            return;
        }
        var items = NotificationCenter.Items;
        btnNotifications.Text = $"Notifications ({items.Count})";
        var dropDown = btnNotifications.DropDownItems;
        dropDown.Clear();
        if (items.Count == 0)
        {
            dropDown.Add(new ToolStripLabel("No notifications yet") { Enabled = false });
            return;
        }
        foreach (var item in items.Take(10))
            dropDown.Add(new ToolStripLabel($"{item.At:HH:mm}  {TruncateText(item.Message, 72)}") { Enabled = false });
        dropDown.Add(new ToolStripSeparator());
        dropDown.Add(new ToolStripMenuItem("Clear all", null, (_, _) => NotificationCenter.Clear()));
    }

    private static string TruncateText(string text, int max)
        => text.Length <= max ? text : text[..(max - 1)] + "…";

    private sealed class ActivityMessageFilter(Action onActivity) : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0200: // WM_MOUSEMOVE
                case 0x0100: // WM_KEYDOWN
                case 0x0201: // WM_LBUTTONDOWN
                case 0x020A: // WM_MOUSEWHEEL
                case 0x0104: // WM_SYSKEYDOWN
                    onActivity();
                    break;
            }
            return false;
        }
    }

    private void EnsureWatcher()
    {
        if (AppServices.Settings.AutoSync && !string.IsNullOrWhiteSpace(_lastFolder) && Directory.Exists(_lastFolder))
        {
            _watcher ??= new Controller.Sync.DirectoryWatchController();
            _watcher.FileChanged -= OnWatchedFileChanged;
            _watcher.FileChanged += OnWatchedFileChanged;
            _watcher.Start(_lastFolder);
        }
        else
        {
            _watcher?.Stop();
        }
    }

    private void OnWatchedFileChanged(string path)
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired)
        {
            BeginInvoke(new Action<string>(OnWatchedFileChanged), path);
            return;
        }
        _ = AutoSyncFileAsync(path);
    }

    private async Task AutoSyncFileAsync(string path)
    {
        if (_busy) return;
        if (!File.Exists(path)) return;
        if (Controller.Sync.DirectoryScannerController.IsExcluded(path, AppServices.Settings.ExcludePatterns)) return;
        try
        {
            string hash;
            long size;
            await using (FileStream stream = File.OpenRead(path))
            {
                hash = await AppServices.Sha256.ComputeHashAsync(stream);
                size = stream.Length;
            }
            Model.Entities.FileModel? model = _viewModel.Files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase));
            if (model == null)
            {
                model = new Model.Entities.FileModel
                {
                    FileName = Path.GetFileName(path),
                    FilePath = path,
                    SizeBytes = size,
                    Sha256Hash = hash,
                    Status = Core.Enums.SyncStatus.Pending,
                    UploadedAt = File.GetLastWriteTimeUtc(path)
                };
                _viewModel.Files.Add(model);
            }
            else
            {
                if (model.Sha256Hash == hash && model.Status == Core.Enums.SyncStatus.Synced) return;
                model.SizeBytes = size;
                model.Sha256Hash = hash;
            }
            bool ok = await AppServices.SyncEngine.ProcessFileAsync(model, await File.ReadAllBytesAsync(path));
            gridFiles.DataSource = null;
            gridFiles.DataSource = _viewModel.Files;
            UpdateDashboard();
            BuildTree();
            SetStatus(ok ? $"Auto-sync: {model.FileName} uploaded." : $"Auto-sync: {model.FileName} queued for resume.", ok ? ToastKind.Success : ToastKind.Warning);
            if (ok) await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileUpload, $"Auto-sync uploaded {path}.");
        }
        catch
        {
        }
    }

    private async Task RefreshFolderAsync(string path)
    {
        _lastFolder = path;
        SetBusy(true, "Scanning");
        Cursor = Cursors.WaitCursor;
        try
        {
            var scanner = new Controller.Sync.DirectoryScannerController(AppServices.Sha256, AppServices.Files);
            scanner.ExcludePatternsProvider = () => AppServices.Settings.ExcludePatterns;
            await _viewModel.ScanFolderAsync(path, scanner);
            gridFiles.DataSource = null;
            gridFiles.DataSource = _viewModel.Files;
            UpdateDashboard();
            BuildTree();
            EnsureWatcher();
            txtFilter_TextChanged(txtFilter, EventArgs.Empty);
            int synced = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Synced);
            int pending = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Pending);
            SetBusy(false, null);
            SetStatus($"Scanned {_viewModel.Files.Count} files. Synced: {synced}, Pending: {pending}", ToastKind.Info);
        }
        finally
        {
            SetBusy(false, null);
            Cursor = Cursors.Default;
        }
    }

    private void SetBusy(bool busy, string? action)
    {
        _busy = busy;
        if (busy)
        {
            _busyBaseText = action ?? string.Empty;
            _dots = 0;
            if (AppServices.Settings.Animations)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.MarqueeAnimationSpeed = 30;
                _animTimer.Start();
            }
        }
        else
        {
            _animTimer.Stop();
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.MarqueeAnimationSpeed = 0;
        }
    }

    private void AnimTimer_Tick(object? sender, EventArgs e)
    {
        if (!_busy || !AppServices.Settings.Animations) return;
        _dots = (_dots + 1) % 4;
        lblStatus.Text = _busyBaseText + new string('.', _dots);
    }

    private void SetStatus(string message, ToastKind kind)
    {
        lblStatus.Text = message;
        try
        {
            lblStatus.ForeColor = kind switch
            {
                ToastKind.Success => DarkThemeModule.SuccessColor,
                ToastKind.Warning => DarkThemeModule.WarningColor,
                ToastKind.Error => DarkThemeModule.DangerColor,
                _ => DarkThemeModule.TextColor,
            };
        }
        catch
        {
        }
        ToastForm.ShowToast(this, message, kind);
    }

    private void Notify(string title, string message, ToolTipIcon icon)
    {
        NotificationCenter.Push(message, icon switch
        {
            ToolTipIcon.Error => ToastKind.Error,
            ToolTipIcon.Warning => ToastKind.Warning,
            _ => ToastKind.Info,
        });
        if (!AppServices.Settings.Notifications) return;
        try
        {
            notifyIcon.ShowBalloonTip(4000, title, message, icon);
        }
        catch
        {
        }
    }

    private async void MainView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5)
        {
            e.Handled = true;
            if (tabControl.SelectedTab == tabSync && _lastFolder != null)
                await RefreshFolderAsync(_lastFolder);
            else
                await RefreshCurrentTabAsync();
        }
        else if (e.Control && e.KeyCode == Keys.L)
        {
            e.Handled = true;
            LockSession();
        }
        else if (e.Control && e.Shift && e.KeyCode == Keys.E)
        {
            e.Handled = true;
            if (tabControl.TabPages.Contains(tabAudit)) btnExportAudit_Click(this, EventArgs.Empty);
        }
        else if (e.Control && e.Shift && e.KeyCode == Keys.U)
        {
            e.Handled = true;
            if (tabControl.TabPages.Contains(tabUsers)) ExportUsersCsv();
        }
        else if (e.Control && e.KeyCode == Keys.S)
        {
            e.Handled = true;
            btnSync.PerformClick();
        }
        else if (e.KeyCode == Keys.Delete && gridFiles.Focused)
        {
            e.Handled = true;
            btnDelete.PerformClick();
        }
        else if (e.Control && e.KeyCode == Keys.F)
        {
            e.Handled = true;
            tabControl.SelectedTab = tabSync;
            txtFilter.Focus();
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_exitRequested)
        {
            if (!AppServices.Settings.MinimizeToTray)
            {
                _exitRequested = true;
                base.OnFormClosing(e);
                return;
            }
            var choice = MessageBox.Show(
                "Do you want to exit the application or minimize it to the system tray?",
                "DataSyncEngine",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (choice == DialogResult.Yes)
            {
                _exitRequested = true;
                Application.Exit();
                return;
            }
            if (choice == DialogResult.No)
            {
                e.Cancel = true;
                Hide();
                Notify("DataSyncEngine", "Still running in the system tray.", ToolTipIcon.Info);
                return;
            }
            e.Cancel = true;
            return;
        }
        try { _watcher?.Dispose(); } catch { }
        base.OnFormClosing(e);
    }

    public MainView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        if (!DarkThemeModule.IsDesignTime)
        {
            Icon = LogoModule.GetIcon() ?? AppIcon.Create();
            notifyIcon.Icon = LogoModule.GetIcon() ?? AppIcon.Create();
        }
        Image? logo = LogoModule.GetImage(40);
        if (logo != null) picSideLogo.Image = logo;
        _viewModel = new MainViewModel(AppServices.Session.Role ?? Core.Enums.UserRole.Operator);
        ApplyRoleLayout();
        LoadSettingsIntoControls();
        WireEvents();
        UpdateServerInfo();
        AppServices.HotSwap.ServerChanged += _ => UpdateServerInfo();
        _animTimer.Tick += AnimTimer_Tick;
        _lastActivity = DateTime.UtcNow;
        _activityFilter = new ActivityMessageFilter(() => _lastActivity = DateTime.UtcNow);
        Application.AddMessageFilter(_activityFilter);
        _maintenanceTimer.Tick += MaintenanceTimer_Tick;
        _maintenanceTimer.Start();
        NotificationCenter.Changed += RebuildNotifications;
        RebuildNotifications();
    }

    private async void MaintenanceTimer_Tick(object? sender, EventArgs e)
    {
        try
        {
            int idleMinutes = AppServices.Settings.IdleLogoutMinutes;
            if (idleMinutes > 0 && !_exitRequested && (DateTime.UtcNow - _lastActivity).TotalMinutes >= idleMinutes)
            {
                await LogoutAsync($"auto-logout after {idleMinutes} minutes of inactivity");
                return;
            }
            if (AutoBackupDue())
            {
                await RunBackupAsync(ResolveAutoBackupFolder(), manual: false);
            }
        }
        catch
        {
        }
    }

    private static string ResolveAutoBackupFolder()
    {
        string folder = AppServices.Settings.AutoBackupFolder;
        if (string.IsNullOrWhiteSpace(folder))
        {
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataSyncEngineBackup");
            AppServices.Settings.AutoBackupFolder = folder;
            AppServices.SaveSettings();
        }
        Directory.CreateDirectory(folder);
        return folder;
    }

    private bool AutoBackupDue()
    {
        int hours = AppServices.Settings.AutoBackupHours;
        if (hours <= 0) return false;
        if (!DateTime.TryParse(AppServices.Settings.LastBackupAt, out DateTime last)) return true;
        return DateTime.Now - last >= TimeSpan.FromHours(hours);
    }

    private async Task LogoutAsync(string reason)
    {
        if (_lockView is { IsDisposed: false }) _lockView.ForceDismiss();
        string? user = AppServices.Session.Username;
        if (user != null)
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.Logout, $"User {user} logged out ({reason}).");
        AppServices.Auth.Logout();
        _exitRequested = true;
        Close();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _maintenanceTimer.Stop();
        _maintenanceTimer.Dispose();
        NotificationCenter.Changed -= RebuildNotifications;
        if (_activityFilter != null) Application.RemoveMessageFilter(_activityFilter);
        base.OnFormClosed(e);
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await _viewModel.LoadServersAsync();
        UpdateDashboard();
        await LoadUserBadgeAsync();
        ToastForm.ShowToast(this, $"Signed in as {AppServices.Session.Username} ({AppServices.Session.Role}).", ToastKind.Success);
    }

    private async Task LoadUserBadgeAsync()
    {
        try
        {
            string? username = AppServices.Session.Username;
            if (username == null) return;
            string role = AppServices.Session.Role?.ToString() ?? "User";
            lblUserBadge.Text = $"{username}\n{role}";
            var user = await AppServices.Users.GetByUsernameAsync(username);
            if (user?.Avatar is { Length: > 0 })
            {
                using var ms = new MemoryStream(user.Avatar);
                using var img = Image.FromStream(ms);
                Image? old = picUserAvatar.Image;
                picUserAvatar.Image = new Bitmap(img);
                old?.Dispose();
            }
        }
        catch
        {
        }
    }

    private static void HideColumns(DataGridView grid, params string[] names)
    {
        foreach (string name in names)
        {
            if (grid.Columns.Contains(name)) grid.Columns[name]?.Visible = false;
        }
    }

    private static void SetupNavButton(Krypton.Toolkit.KryptonButton button, string name, string text, int y)
    {
        button.Location = new System.Drawing.Point(12, y);
        button.Name = name;
        button.Size = new System.Drawing.Size(216, 36);
        button.Text = text;
        button.ButtonStyle = Krypton.Toolkit.ButtonStyle.ListItem;
    }

    private void UpdateDashboard()
    {
        lblFilesVal.Text = _viewModel.Files.Count.ToString();
        lblSyncedVal.Text = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Synced).ToString();
        lblPendingVal.Text = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Pending).ToString();
        lblServersVal.Text = _viewModel.Servers.Count.ToString();
    }

    private void TickProgress()
    {
        if (progressBar.InvokeRequired)
        {
            progressBar.Invoke(new Action(TickProgress));
            return;
        }
        if (progressBar.Value < progressBar.Maximum) progressBar.Value++;
    }

    private void themeDarkItem_Click(object? sender, EventArgs e)
    {
        KryptonThemeModule.SetTheme(true);
    }

    private void themeLightItem_Click(object? sender, EventArgs e)
    {
        KryptonThemeModule.SetTheme(false);
    }

    private void txtFilter_TextChanged(object? sender, EventArgs e)
    {
        string term = txtFilter.Text.Trim().ToLowerInvariant();
        var filtered = _viewModel.Files
            .Where(f => f.FileName.Contains(term, StringComparison.OrdinalIgnoreCase) || f.Status.ToString().Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
        gridFiles.DataSource = null;
        gridFiles.DataSource = term.Length == 0 ? _viewModel.Files : filtered;
    }

    private void BindAuditGrid()
    {
        string term = txtAuditFilter.Text.Trim();
        IEnumerable<Model.Entities.AuditLogModel> rows = _viewModel.AuditEntries;
        if (term.Length > 0)
        {
            rows = rows.Where(a =>
                (a.Username?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                a.Action.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (a.Details?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        gridAudit.DataSource = null;
        gridAudit.DataSource = rows.ToList();
    }

    private void txtAuditFilter_TextChanged(object? sender, EventArgs e) => BindAuditGrid();

    private void btnExportAudit_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"audit_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
            Title = "Export audit log"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var rows = gridAudit.DataSource as List<Model.Entities.AuditLogModel> ?? _viewModel.AuditEntries;
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Id;Timestamp;User;Action;Details");
            foreach (var a in rows)
                sb.AppendLine($"{a.Id};{a.Timestamp:yyyy-MM-dd HH:mm:ss};{Escape(a.Username)};{a.Action};{Escape(a.Details)}");
            File.WriteAllText(dialog.FileName, sb.ToString(), System.Text.Encoding.UTF8);
            ToastForm.ShowToast(this, $"Exported {rows.Count} entries.", ToastKind.Success);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static string Escape(string? value)
        {
            value ??= string.Empty;
            return value.Contains(';') || value.Contains('"')
                ? "\"" + value.Replace("\"", "\"\"") + "\""
                : value;
        }
    }

    private void trayAbout_Click(object? sender, EventArgs e)
    {
        trayShow_Click(sender, e);
        using var about = new SubViews.AboutView();
        about.ShowDialog(this);
    }

    private void trayChangePassword_Click(object? sender, EventArgs e)
    {
        trayShow_Click(sender, e);
        using var change = new SubViews.ChangePasswordView();
        change.ShowDialog(this);
    }

    private void trayLock_Click(object? sender, EventArgs e)
    {
        trayShow_Click(sender, e);
        LockSession();
    }

    private void LockSession()
    {
        if (_lockView is { IsDisposed: false }) return;
        string? user = AppServices.Session.Username ?? "unknown";
        _ = AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SessionLocked, $"User {user} locked the screen.");
        ToastForm.ShowToast(this, "Screen locked.", ToastKind.Warning);
        _lockView = new SubViews.LockView();
        try
        {
            if (_lockView.ShowDialog(this) == DialogResult.OK)
            {
                _ = AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SessionUnlocked, $"User {user} unlocked the screen.");
                ToastForm.ShowToast(this, "Screen unlocked.", ToastKind.Success);
            }
        }
        finally
        {
            _lockView = null;
        }
    }

    private void btnNavToggle_Click(object? sender, EventArgs e)
    {
        sideBar.Visible = false;
        sideStrip.Visible = true;
    }

    private void btnExpand_Click(object? sender, EventArgs e)
    {
        sideStrip.Visible = false;
        sideBar.Visible = true;
    }

    private void navDashboard_Click(object? sender, EventArgs e) => tabControl.SelectedTab = tabDashboard;
    private void navSync_Click(object? sender, EventArgs e) => tabControl.SelectedTab = tabSync;
    private void navBackup_Click(object? sender, EventArgs e) => tabControl.SelectedTab = tabBackup;
    private void navTrash_Click(object? sender, EventArgs e) => tabControl.SelectedTab = tabTrash;
    private void navServers_Click(object? sender, EventArgs e) { if (tabControl.TabPages.Contains(tabServers)) tabControl.SelectedTab = tabServers; }
    private void navAudit_Click(object? sender, EventArgs e) { if (tabControl.TabPages.Contains(tabAudit)) tabControl.SelectedTab = tabAudit; }
    private void navAnalytics_Click(object? sender, EventArgs e) { if (tabControl.TabPages.Contains(tabAnalytics)) tabControl.SelectedTab = tabAnalytics; }
    private void navUsers_Click(object? sender, EventArgs e) { if (tabControl.TabPages.Contains(tabUsers)) tabControl.SelectedTab = tabUsers; }
    private void navSettings_Click(object? sender, EventArgs e) => tabControl.SelectedTab = tabSettings;

    private void treeFiles_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not Model.Entities.FileModel file) return;
        for (int i = 0; i < gridFiles.Rows.Count; i++)
        {
            if (gridFiles.Rows[i].DataBoundItem is Model.Entities.FileModel row
                && row.FilePath.Equals(file.FilePath, StringComparison.OrdinalIgnoreCase))
            {
                gridFiles.ClearSelection();
                gridFiles.Rows[i].Selected = true;
                gridFiles.CurrentCell = gridFiles.Rows[i].Cells[0];
                break;
            }
        }
    }

    private void treeFiles_DoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node?.Tag is not Model.Entities.FileModel) return;
        treeFiles_AfterSelect(sender, new TreeViewEventArgs(e.Node));
        tabControl.SelectedTab = tabSync;
    }

    private void radDark_CheckedChanged(object? sender, EventArgs e)
    {
        if (!radDark.Checked) return;
        AppServices.Settings.ThemeDark = true;
        AppServices.SaveSettings();
        KryptonThemeModule.SetTheme(true);
    }

    private void radLight_CheckedChanged(object? sender, EventArgs e)
    {
        if (!radLight.Checked) return;
        AppServices.Settings.ThemeDark = false;
        AppServices.SaveSettings();
        KryptonThemeModule.SetTheme(false);
    }

    private void trkFontSize_ValueChanged(object? sender, EventArgs e)
    {
        lblFontSizeVal.Text = trkFontSize.Value.ToString("0.0");
        UpdateFontPreview();
        AppServices.Settings.FontSize = trkFontSize.Value;
        AppServices.SaveSettings();
        KryptonThemeModule.ApplyFontToOpenForms(AppServices.Settings.FontSize, AppServices.Settings.FontBold);
    }

    private void chkBold_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateFontPreview();
        AppServices.Settings.FontBold = chkBold.Checked;
        AppServices.SaveSettings();
        KryptonThemeModule.ApplyFontToOpenForms(AppServices.Settings.FontSize, AppServices.Settings.FontBold);
    }

    private void cmbAccent_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbAccent.SelectedItem is not string name) return;
        AppServices.Settings.Accent = name;
        AppServices.SaveSettings();
        KryptonThemeModule.SetAccent(name);
    }

    private void swAnimations_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.Animations = swAnimations.Checked;
        AppServices.SaveSettings();
    }

    private void swConfirmDelete_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.ConfirmDelete = swConfirmDelete.Checked;
        AppServices.SaveSettings();
    }

    private void swNotifications_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.Notifications = swNotifications.Checked;
        AppServices.SaveSettings();
    }

    private void swColdStorage_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.KeepColdStorage = swColdStorage.Checked;
        AppServices.SaveSettings();
        AppServices.SyncEngine.KeepColdStorage = swColdStorage.Checked;
    }

    private void swMinimizeToTray_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.MinimizeToTray = swMinimizeToTray.Checked;
        AppServices.SaveSettings();
    }

    private void swAutoSync_CheckedChanged(object? sender, EventArgs e)
    {
        AppServices.Settings.AutoSync = swAutoSync.Checked;
        AppServices.SaveSettings();
        EnsureWatcher();
        ToastForm.ShowToast(this, swAutoSync.Checked ? "Auto-sync enabled." : "Auto-sync disabled.", ToastKind.Info);
    }

    private void txtExclude_Leave(object? sender, EventArgs e)
    {
        AppServices.Settings.ExcludePatterns = txtExclude.Text.Trim();
        AppServices.SaveSettings();
        ToastForm.ShowToast(this, "Exclude patterns saved. Rescan to apply.", ToastKind.Info);
    }

    private void btnResetSettings_Click(object? sender, EventArgs e)
    {
        var s = AppServices.Settings;
        s.ThemeDark = true;
        s.Accent = "Blue";
        s.FontSize = 9.5f;
        s.FontBold = false;
        s.Animations = true;
        s.ConfirmDelete = true;
        s.Notifications = true;
        s.KeepColdStorage = true;
        s.MinimizeToTray = true;
        s.AutoSync = false;
        s.ExcludePatterns = "*.tmp;*.log;~$*;*.bak;*.swp";
        s.IdleLogoutMinutes = 15;
        s.AutoBackupHours = 0;
        s.AutoBackupFolder = string.Empty;
        AppServices.SaveSettings();
        KryptonThemeModule.SetAccent("Blue");
        KryptonThemeModule.SetTheme(true);
        AppServices.SyncEngine.KeepColdStorage = true;
        LoadSettingsIntoControls();
        KryptonThemeModule.ApplyFontToOpenForms(s.FontSize, s.FontBold);
        ToastForm.ShowToast(this, "Settings restored to defaults.", ToastKind.Info);
    }

    private async void btnBackupNow_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog { Description = "Select backup destination folder" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        await RunBackupAsync(dialog.SelectedPath, manual: true);
    }

    private async Task RunBackupAsync(string destination, bool manual)
    {
        if (_backupRunning) return;
        _backupRunning = true;
        btnBackupNow.Enabled = false;
        lblBackupInfo.Text = "Backing up...";
        try
        {
            var backup = new Controller.Sync.BackupController(AppServices.Files, AppServices.Aes, AppServices.Compressor,
                () => AppServices.Aes.DeriveKey(AppServices.MasterKey));
            var (ok, fail) = await Task.Run(() => backup.ExportAsync(destination));
            AppServices.Settings.LastBackupAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            AppServices.SaveSettings();
            lblBackupInfo.Text = $"Backup finished: {ok} exported, {fail} failed.";
            lblLastBackup.Text = $"Last backup: {AppServices.Settings.LastBackupAt}";
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncCompleted,
                $"Backup{(manual ? "" : " (automatic)")}: {ok} exported, {fail} failed to {destination}.");
            ToastForm.ShowToast(this, $"Backup finished: {ok} exported.", fail > 0 ? ToastKind.Warning : ToastKind.Success);
        }
        catch (Exception ex)
        {
            lblBackupInfo.Text = $"Backup failed: {ex.Message}";
            ToastForm.ShowToast(this, "Backup failed.", ToastKind.Error);
        }
        finally
        {
            _backupRunning = false;
            btnBackupNow.Enabled = true;
        }
    }

    private async void btnRestore_Click(object? sender, EventArgs e)
    {
        if (gridTrash.CurrentRow?.DataBoundItem is not Model.Entities.TrashEntry entry) return;
        try
        {
            byte[]? blob = await AppServices.Trash.GetTrashBlobAsync(entry.Id);
            if (blob == null || blob.Length == 0)
            {
                ToastForm.ShowToast(this, "No content stored for this file.", ToastKind.Warning);
                return;
            }
            await AppServices.Files.InsertWithBlobAsync(entry.ToFileModel(), blob);
            await AppServices.Trash.DeleteAsync(entry.Id);
            await RefreshTrashAsync();
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileUpload, $"Restored {entry.FilePath} from trash.");
            ToastForm.ShowToast(this, $"{entry.FileName} restored.", ToastKind.Success);
        }
        catch (Exception ex)
        {
            ToastForm.ShowToast(this, $"Restore failed: {ex.Message}", ToastKind.Error);
        }
    }

    private async void btnPurge_Click(object? sender, EventArgs e)
    {
        if (gridTrash.CurrentRow?.DataBoundItem is not Model.Entities.TrashEntry entry) return;
        if (AppServices.Settings.ConfirmDelete
            && MessageBox.Show($"Permanently delete {entry.FileName}? This cannot be undone.", "Trash", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        try
        {
            await AppServices.Trash.DeleteAsync(entry.Id);
            await RefreshTrashAsync();
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileDelete, $"Purged {entry.FilePath} from trash.");
            ToastForm.ShowToast(this, $"{entry.FileName} deleted forever.", ToastKind.Info);
        }
        catch (Exception ex)
        {
            ToastForm.ShowToast(this, $"Purge failed: {ex.Message}", ToastKind.Error);
        }
    }

    private void trayShow_Click(object? sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        BringToFront();
    }

    private void trayExit_Click(object? sender, EventArgs e)
    {
        _exitRequested = true;
        Close();
        Application.Exit();
    }

    private void UpdateServerInfo()
    {
        if (_viewModel.Role == Core.Enums.UserRole.Admin)
        {
            lblServerInfo.Text = $"Server: {AppServices.CurrentServer.Name} ({AppServices.CurrentServer.Host}:{AppServices.CurrentServer.Port}) | User: {AppServices.Session.Username} | Role: {AppServices.Session.Role}";
        }
        else
        {
            lblServerInfo.Text = $"User: {AppServices.Session.Username} | Role: {AppServices.Session.Role}";
        }
    }

    private void GridFiles_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || gridFiles.Rows[e.RowIndex].DataBoundItem is not Model.Entities.FileModel file) return;
        e.CellStyle.ForeColor = file.Status switch
        {
            Core.Enums.SyncStatus.Synced => DarkThemeModule.SuccessColor,
            Core.Enums.SyncStatus.Pending => DarkThemeModule.WarningColor,
            Core.Enums.SyncStatus.Modified => DarkThemeModule.ModifiedColor,
            _ => DarkThemeModule.TextColor,
        };
    }

    private void BuildTree()
    {
        treeFiles.BeginUpdate();
        try
        {
            treeFiles.Nodes.Clear();
            string root = string.IsNullOrWhiteSpace(_lastFolder) ? "Files" : Path.GetFileName(_lastFolder.TrimEnd(Path.DirectorySeparatorChar));
            var rootNode = new TreeNode(string.IsNullOrWhiteSpace(root) ? "Files" : root);
            var dirs = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);
            foreach (Model.Entities.FileModel file in _viewModel.Files.OrderBy(f => f.FilePath))
            {
                string rel = file.FilePath;
                if (!string.IsNullOrWhiteSpace(_lastFolder) && rel.StartsWith(_lastFolder, StringComparison.OrdinalIgnoreCase))
                    rel = rel.Substring(_lastFolder.Length).TrimStart(Path.DirectorySeparatorChar);
                string[] parts = rel.Split(Path.DirectorySeparatorChar);
                TreeNode parent = rootNode;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string key = string.Join("/", parts[..(i + 1)]);
                    if (!dirs.TryGetValue(key, out TreeNode? node))
                    {
                        node = new TreeNode(parts[i]);
                        parent.Nodes.Add(node);
                        dirs[key] = node;
                    }
                    parent = node;
                }
                var fileNode = new TreeNode(file.FileName) { Tag = file };
                parent.Nodes.Add(fileNode);
            }
            treeFiles.Nodes.Add(rootNode);
            rootNode.Expand();
        }
        finally
        {
            treeFiles.EndUpdate();
        }
    }

    private void ApplyRoleLayout()
    {
        if (_viewModel.Role == Core.Enums.UserRole.Operator)
        {
            tabControl.TabPages.Remove(tabAudit);
            tabControl.TabPages.Remove(tabAnalytics);
            navAudit.Visible = false;
            navAnalytics.Visible = false;
        }
        if (_viewModel.Role != Core.Enums.UserRole.Admin)
        {
            btnAddServer.Visible = false;
            btnEditServer.Visible = false;
            btnSetActive.Visible = false;
            btnDeleteServer.Visible = false;
            tabControl.TabPages.Remove(tabUsers);
            tabControl.TabPages.Remove(tabServers);
            navUsers.Visible = false;
            navServers.Visible = false;
        }
    }

    private void WireEvents()
    {
        btnSelectFolder.Click += async (_, _) =>
        {
            using var dialog = new FolderBrowserDialog { Description = "Select folder to scan" };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            await RefreshFolderAsync(dialog.SelectedPath);
        };

        btnAddServer.Click += (_, _) =>
        {
            using var dialog = new SubViews.ServerConfigView();
            dialog.ShowDialog(this);
        };

        btnSync.Click += async (_, _) =>
        {
            var pending = _viewModel.Files.Where(f => f.Status != Core.Enums.SyncStatus.Synced).ToList();
            if (pending.Count == 0)
            {
                lblStatus.Text = "Nothing to sync.";
                return;
            }
            lblStatus.Text = "Syncing...";
            SetBusy(true, "Syncing");
            Cursor = Cursors.WaitCursor;
            progressBar.Value = 0;
            progressBar.Maximum = pending.Count;
            var engine = AppServices.SyncEngine;
            Action<string> tick = _ => TickProgress();
            engine.FileProcessed += tick;
            engine.FileFailed += tick;
            try
            {
                int ok = await Task.Run(() => engine.ProcessBatchAsync(pending, f => File.ReadAllBytes(f.FilePath)));
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                SetBusy(false, null);
                SetStatus($"Synced {ok}/{pending.Count} files.", ok == pending.Count ? ToastKind.Success : ToastKind.Warning);
                Notify("DataSyncEngine", $"Sync completed: {ok}/{pending.Count}.", ToolTipIcon.Info);
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncCompleted, $"Synced {ok}/{pending.Count} files.");
                if (engine.PendingResume.Count > 0)
                {
                    Notify("DataSyncEngine", $"{engine.PendingResume.Count} files marked for resume.", ToolTipIcon.Warning);
                    await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncFailed, $"{engine.PendingResume.Count} files pending resume.");
                }
            }
            catch (Exception ex)
            {
                SetBusy(false, null);
                SetStatus($"Sync failed: {ex.Message}", ToastKind.Error);
                Notify("DataSyncEngine", $"Sync failed: {ex.Message}", ToolTipIcon.Error);
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncFailed, ex.Message);
            }
            finally
            {
                SetBusy(false, null);
                engine.FileProcessed -= tick;
                engine.FileFailed -= tick;
                Cursor = Cursors.Default;
            }
        };

        btnExplorer.Click += (_, _) =>
        {
            using var explorer = new SubViews.FileExplorerView();
            explorer.ShowDialog(this);
        };

        btnResume.Click += async (_, _) =>
        {
            var engine = AppServices.SyncEngine;
            if (engine.PendingResume.Count == 0)
            {
                lblStatus.Text = "No files marked for resume.";
                return;
            }
            lblStatus.Text = "Resuming...";
            Cursor = Cursors.WaitCursor;
            try
            {
                int ok = await Task.Run(() => engine.ResumePendingAsync(
                    f => File.ReadAllBytes(f.FilePath),
                    path => _viewModel.Files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                        ?? new Model.Entities.FileModel { FilePath = path, FileName = Path.GetFileName(path) }));
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                lblStatus.Text = $"Resume completed: {ok} file(s).";
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncCompleted, $"Resume: {ok} files recovered.");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        };

        btnLogout.Click += async (_, _) => await LogoutAsync("manual logout");

        btnDelete.Click += async (_, _) =>
        {
            if (gridFiles.CurrentRow?.DataBoundItem is not Model.Entities.FileModel file) return;

            if (_viewModel.Role == Core.Enums.UserRole.Operator)
            {
                using var masterKey = new SubViews.MasterKeyPopUpView();
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.MasterKeyPrompted, $"Operator requested delete of {file.FilePath}.");
                if (masterKey.ShowDialog(this) != DialogResult.OK || !masterKey.Authorized)
                {
                    await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileDelete, $"Delete denied for {file.FilePath}.");
                    return;
                }
            }

            if (AppServices.Settings.ConfirmDelete
                && MessageBox.Show($"Delete {file.FileName} from the server?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                if (file.Id > 0)
                {
                    await MoveToTrashAsync(file);
                    await AppServices.Files.DeleteAsync(file.Id);
                }
                _viewModel.Files.Remove(file);
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileDelete, $"Deleted {file.FilePath}.");
                SetStatus($"{file.FileName} moved to trash.", ToastKind.Warning);
            }
            catch (Exception ex)
            {
                SetStatus($"Delete failed: {ex.Message}", ToastKind.Error);
                Notify("DataSyncEngine", $"Delete failed: {ex.Message}", ToolTipIcon.Error);
            }
        };

        tabAnalytics.Enter += async (_, _) => await LoadAnalyticsTabAsync();

        tabAudit.Enter += async (_, _) => await LoadAuditTabAsync();

        tabUsers.Enter += async (_, _) => await LoadUsersTabAsync();

        btnExportUsers.Click += (_, _) => ExportUsersCsv();

        btnViewProfile.Click += async (_, _) =>
        {
            if (gridUsers.CurrentRow?.DataBoundItem is not Model.Entities.UserModel user) return;
            var history = await AppServices.Audits.GetByUserAsync(user.Username);
            using var profile = new SubViews.UserProfileView(user, history);
            profile.ShowDialog(this);
        };

        btnResetPassword.Click += (_, _) =>
        {
            if (gridUsers.CurrentRow?.DataBoundItem is not Model.Entities.UserModel user) return;
            if (user.Username.Equals(AppServices.Session.Username, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Use the tray menu (Change Password) to change your own password.",
                    "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var dialog = new SubViews.ChangePasswordView(user.Username);
            dialog.ShowDialog(this);
        };

        btnAddUser.Click += (_, _) =>
        {
            using var dialog = new SubViews.UserConfigView();
            dialog.ShowDialog(this);
        };

        btnDeleteUser.Click += async (_, _) =>
        {
            if (gridUsers.CurrentRow?.DataBoundItem is not Model.Entities.UserModel user) return;
            if (user.Username.Equals(AppServices.Session.Username, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot delete the logged in user.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Delete user {user.Username}?", "Users", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            await AppServices.Users.DeleteAsync(user.Id);
            await _viewModel.LoadUsersAsync();
            gridUsers.DataSource = null;
            gridUsers.DataSource = _viewModel.Users;
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileDelete, $"User {user.Username} deleted.");
        };

        btnEditServer.Click += (_, _) =>
        {
            if (gridServers.CurrentRow?.DataBoundItem is not Model.Entities.ServerModel server) return;
            using var dialog = new SubViews.ServerConfigView(server);
            dialog.ShowDialog(this);
        };

        btnSetActive.Click += async (_, _) =>
        {
            if (gridServers.CurrentRow?.DataBoundItem is not Model.Entities.ServerModel target) return;
            foreach (var s in _viewModel.Servers)
            {
                s.IsActive = s.Id == target.Id;
                await AppServices.Servers.UpdateAsync(s);
            }
            AppServices.HotSwap.SwitchServer(target);
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.ServerSwitch, $"Switched to {target.Name}.");
            gridServers.DataSource = null;
            gridServers.DataSource = _viewModel.Servers;
        };

        btnDeleteServer.Click += async (_, _) =>
        {
            if (gridServers.CurrentRow?.DataBoundItem is not Model.Entities.ServerModel server) return;
            if (MessageBox.Show($"Delete server {server.Name}?", "Servers", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            await AppServices.Servers.DeleteAsync(server.Id);
            await _viewModel.LoadServersAsync();
            gridServers.DataSource = null;
            gridServers.DataSource = _viewModel.Servers;
        };

        tabServers.Enter += async (_, _) => await LoadServersTabAsync();

        gridFiles.CellFormatting += GridFiles_CellFormatting;

        tabBackup.Enter += (_, _) => LoadBackupTab();

        tabTrash.Enter += async (_, _) => await RefreshTrashAsync();

        tabSettings.Enter += (_, _) => LoadSettingsIntoControls();

        gridTrash.CellFormatting += GridTrash_CellFormatting;
    }

    private void GridTrash_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || gridTrash.Rows[e.RowIndex].DataBoundItem is not Model.Entities.TrashEntry) return;
        e.CellStyle.ForeColor = DarkThemeModule.WarningColor;
    }

    private async Task LoadAnalyticsTabAsync()
    {
        lblAnaTotal.Text = $"Total files: {_viewModel.Files.Count}";
        lblAnaSynced.Text = $"Synced: {_viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Synced)}";
        lblAnaPending.Text = $"Pending: {_viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Pending)}";
        lblAnaModified.Text = $"Modified: {_viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Modified)}";
        lblAnaBytes.Text = $"Total size: {_viewModel.Files.Sum(f => f.SizeBytes)} bytes";
        try
        {
            var serverFiles = await AppServices.Files.GetAllAsync();
            lblAnaServer.Text = $"Server files: {serverFiles.Count} ({serverFiles.Sum(f => f.SizeBytes)} bytes)";
        }
        catch
        {
            lblAnaServer.Text = "Server files: unavailable";
        }
    }

    private async Task LoadAuditTabAsync()
    {
        await _viewModel.LoadAuditAsync();
        BindAuditGrid();
    }

    private async Task LoadUsersTabAsync()
    {
        await _viewModel.LoadUsersAsync();
        gridUsers.DataSource = null;
        gridUsers.DataSource = _viewModel.Users;
        HideColumns(gridUsers, "PasswordHash", "Salt", "Avatar");
    }

    private async Task LoadServersTabAsync()
    {
        await _viewModel.LoadServersAsync();
        gridServers.DataSource = null;
        gridServers.DataSource = _viewModel.Servers;
        UpdateDashboard();
        HideColumns(gridServers, "Password");
    }

    private void LoadBackupTab()
    {
        lblLastBackup.Text = string.IsNullOrWhiteSpace(AppServices.Settings.LastBackupAt)
            ? "Last backup: never"
            : $"Last backup: {AppServices.Settings.LastBackupAt}";
    }

    private async Task RefreshCurrentTabAsync()
    {
        var tab = tabControl.SelectedTab;
        if (tab == tabSync && _lastFolder != null) await RefreshFolderAsync(_lastFolder);
        else if (tab == tabAudit) await LoadAuditTabAsync();
        else if (tab == tabUsers) await LoadUsersTabAsync();
        else if (tab == tabServers) await LoadServersTabAsync();
        else if (tab == tabTrash) await RefreshTrashAsync();
        else if (tab == tabAnalytics) await LoadAnalyticsTabAsync();
        else if (tab == tabBackup) LoadBackupTab();
        else if (tab == tabSettings) LoadSettingsIntoControls();
        else UpdateDashboard();
        SetStatus("Refreshed.", ToastKind.Info);
    }

    private void ExportUsersCsv()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"users_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
            Title = "Export users"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Id;Username;Role;Active");
            foreach (var u in _viewModel.Users)
                sb.AppendLine($"{u.Id};{u.Username};{u.Role};{(u.IsActive ? "yes" : "no")}");
            File.WriteAllText(dialog.FileName, sb.ToString(), System.Text.Encoding.UTF8);
            ToastForm.ShowToast(this, $"Exported {_viewModel.Users.Count} users.", ToastKind.Success);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadSettingsIntoControls()
    {
        var s = AppServices.Settings;
        radDark.Checked = s.ThemeDark;
        radLight.Checked = !s.ThemeDark;
        trkFontSize.Value = Math.Clamp((int)Math.Round(s.FontSize), trkFontSize.Minimum, trkFontSize.Maximum);
        lblFontSizeVal.Text = s.FontSize.ToString("0.0");
        chkBold.Checked = s.FontBold;
        UpdateFontPreview();
        if (cmbAccent.Items.Count == 0)
            cmbAccent.Items.AddRange(new object[] { "Blue", "Green", "Red", "Purple" });
        cmbAccent.SelectedItem = s.Accent;
        swAnimations.Checked = s.Animations;
        swConfirmDelete.Checked = s.ConfirmDelete;
        swNotifications.Checked = s.Notifications;
        swColdStorage.Checked = s.KeepColdStorage;
        swMinimizeToTray.Checked = s.MinimizeToTray;
        swAutoSync.Checked = s.AutoSync;
        txtExclude.Text = s.ExcludePatterns;
        if (cmbIdle.Items.Count == 0) cmbIdle.Items.AddRange(new object[] { "0", "5", "15", "30", "60" });
        string idleValue = s.IdleLogoutMinutes.ToString();
        if (!cmbIdle.Items.Contains(idleValue)) cmbIdle.Items.Add(idleValue);
        cmbIdle.SelectedItem = idleValue;
        if (cmbAutoBackup.Items.Count == 0) cmbAutoBackup.Items.AddRange(new object[] { "0", "1", "6", "12", "24" });
        string backupValue = s.AutoBackupHours.ToString();
        if (!cmbAutoBackup.Items.Contains(backupValue)) cmbAutoBackup.Items.Add(backupValue);
        cmbAutoBackup.SelectedItem = backupValue;
    }

    private void cmbIdle_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbIdle.SelectedItem is not string value || !int.TryParse(value, out int minutes)) return;
        if (minutes == AppServices.Settings.IdleLogoutMinutes) return;
        AppServices.Settings.IdleLogoutMinutes = minutes;
        AppServices.SaveSettings();
        ToastForm.ShowToast(this, minutes == 0
            ? "Auto-logout disabled."
            : $"Auto-logout after {minutes} minutes of inactivity.", ToastKind.Info);
    }

    private void cmbAutoBackup_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbAutoBackup.SelectedItem is not string value || !int.TryParse(value, out int hours)) return;
        if (hours == AppServices.Settings.AutoBackupHours) return;
        AppServices.Settings.AutoBackupHours = hours;
        AppServices.SaveSettings();
        ToastForm.ShowToast(this, hours == 0
            ? "Automatic backup disabled."
            : $"Automatic backup every {hours}h to Documents\\DataSyncEngineBackup.", ToastKind.Info);
    }

    private void UpdateFontPreview()
    {
        try
        {
            lblFontPreview.Font = new Font("Segoe UI", trkFontSize.Value, chkBold.Checked ? FontStyle.Bold : FontStyle.Regular);
        }
        catch
        {
        }
    }

    private async Task RefreshTrashAsync()
    {
        await _viewModel.LoadTrashAsync();
        gridTrash.DataSource = null;
        gridTrash.DataSource = _viewModel.TrashEntries;
        lblTrashInfo.Text = $"{_viewModel.TrashEntries.Count} file(s) in trash.";
    }

    private async Task MoveToTrashAsync(Model.Entities.FileModel file)
    {
        try
        {
            byte[]? blob = await AppServices.Files.GetFileBlobAsync(file.Id);
            if (blob == null || blob.Length == 0) return;
            string by = AppServices.Session.Username ?? "unknown";
            await AppServices.Trash.MoveToTrashAsync(file, blob, by);
        }
        catch
        {
        }
    }
}
