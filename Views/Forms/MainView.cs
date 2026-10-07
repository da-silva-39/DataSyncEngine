using Views.Components;
using Views.ViewModels;

namespace Views.Forms;

public partial class MainView : Form
{
    private readonly MainViewModel _viewModel;
    private bool _exitRequested;

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_exitRequested)
        {
            e.Cancel = true;
            Hide();
            notifyIcon.ShowBalloonTip(2000, "DataSyncEngine", "Still running in the system tray.", ToolTipIcon.Info);
        }
        base.OnFormClosing(e);
    }

    public MainView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
        Icon = AppIcon.Create();
        notifyIcon.Icon = AppIcon.Create();
        _viewModel = new MainViewModel(AppServices.Session.Role ?? Core.Enums.UserRole.Operator);
        ApplyRoleLayout();
        WireEvents();
        UpdateServerInfo();
        AppServices.HotSwap.ServerChanged += _ => UpdateServerInfo();
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await _viewModel.LoadServersAsync();
        UpdateDashboard();
    }

    private void UpdateDashboard()
    {
        SetCard(cardFiles, _viewModel.Files.Count.ToString());
        SetCard(cardSynced, _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Synced).ToString());
        SetCard(cardPending, _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Pending).ToString());
        SetCard(cardServers, _viewModel.Servers.Count.ToString());
    }

    private static void SetCard(System.Windows.Forms.Panel card, string value)
    {
        foreach (Control c in card.Controls)
        {
            if (c.Name == "lblValue") c.Text = value;
        }
    }

    private void themeDarkItem_Click(object? sender, EventArgs e)
    {
        DarkThemeModule.SetTheme(true);
        foreach (Form f in Application.OpenForms) DarkThemeModule.Apply(f);
    }

    private void themeLightItem_Click(object? sender, EventArgs e)
    {
        DarkThemeModule.SetTheme(false);
        foreach (Form f in Application.OpenForms) DarkThemeModule.Apply(f);
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
        lblServerInfo.Text = $"Server: {AppServices.CurrentServer.Name} ({AppServices.CurrentServer.Host}:{AppServices.CurrentServer.Port}) | User: {AppServices.Session.Username} | Role: {AppServices.Session.Role}";
    }

    private void ApplyRoleLayout()
    {
        if (_viewModel.Role == Core.Enums.UserRole.Operator)
        {
            tabControl.TabPages.Remove(tabAudit);
            tabControl.TabPages.Remove(tabAnalytics);
        }
        if (_viewModel.Role != Core.Enums.UserRole.Admin)
        {
            btnAddServer.Visible = false;
            btnEditServer.Visible = false;
            btnSetActive.Visible = false;
            btnDeleteServer.Visible = false;
            tabControl.TabPages.Remove(tabUsers);
        }
    }

    private void WireEvents()
    {
        btnSelectFolder.Click += async (_, _) =>
        {
            using var dialog = new FolderBrowserDialog { Description = "Select folder to scan" };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            lblStatus.Text = "Scanning...";
            Cursor = Cursors.WaitCursor;
            try
            {
                var scanner = new Controller.Sync.DirectoryScannerController(AppServices.Sha256, AppServices.Files);
                await _viewModel.ScanFolderAsync(dialog.SelectedPath, scanner);
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                int synced = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Synced);
                int pending = _viewModel.Files.Count(f => f.Status == Core.Enums.SyncStatus.Pending);
                lblStatus.Text = $"Scanned {_viewModel.Files.Count} files. Synced: {synced}, Pending: {pending}";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
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
            Cursor = Cursors.WaitCursor;
            progressBar.Value = 0;
            progressBar.Maximum = pending.Count;
            var engine = AppServices.SyncEngine;
            Action<string> tick = _ => { if (progressBar.Value < progressBar.Maximum) progressBar.Value++; };
            engine.FileProcessed += tick;
            engine.FileFailed += tick;
            try
            {
                int ok = await engine.ProcessBatchAsync(pending, f => File.ReadAllBytes(f.FilePath));
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                lblStatus.Text = $"Synced {ok}/{pending.Count} files.";
                notifyIcon.ShowBalloonTip(3000, "DataSyncEngine", $"Sync completed: {ok}/{pending.Count}.", ToolTipIcon.Info);
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncCompleted, $"Synced {ok}/{pending.Count} files.");
                if (engine.PendingResume.Count > 0)
                {
                    notifyIcon.ShowBalloonTip(5000, "DataSyncEngine", $"{engine.PendingResume.Count} files marked for resume.", ToolTipIcon.Warning);
                    await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncFailed, $"{engine.PendingResume.Count} files pending resume.");
                }
            }
            catch (Exception ex)
            {
                notifyIcon.ShowBalloonTip(5000, "DataSyncEngine", $"Sync failed: {ex.Message}", ToolTipIcon.Error);
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.SyncFailed, ex.Message);
            }
            finally
            {
                engine.FileProcessed -= tick;
                engine.FileFailed -= tick;
                Cursor = Cursors.Default;
            }
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
            try
            {
                int ok = await engine.ResumePendingAsync(
                    f => File.ReadAllBytes(f.FilePath),
                    path => _viewModel.Files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                        ?? new Model.Entities.FileModel { FilePath = path, FileName = Path.GetFileName(path) });
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

        btnLogout.Click += async (_, _) =>
        {
            string? user = AppServices.Session.Username;
            AppServices.Auth.Logout();
            if (user != null) await AppServices.AuditLogger.LogLogoutAsync(user);
            _exitRequested = true;
            Close();
        };

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

            try
            {
                if (file.Id > 0) await AppServices.Files.DeleteAsync(file.Id);
                _viewModel.Files.Remove(file);
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
                UpdateDashboard();
                await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FileDelete, $"Deleted {file.FilePath}.");
            }
            catch (Exception ex)
            {
                notifyIcon.ShowBalloonTip(5000, "DataSyncEngine", $"Delete failed: {ex.Message}", ToolTipIcon.Error);
            }
        };

        tabAudit.Enter += async (_, _) =>
        {
            await _viewModel.LoadAuditAsync();
            gridAudit.DataSource = null;
            gridAudit.DataSource = _viewModel.AuditEntries;
        };

        tabUsers.Enter += async (_, _) =>
        {
            await _viewModel.LoadUsersAsync();
            gridUsers.DataSource = null;
            gridUsers.DataSource = _viewModel.Users;
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
            AppServices.CurrentServer = target;
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

        tabServers.Enter += async (_, _) =>
        {
            await _viewModel.LoadServersAsync();
            gridServers.DataSource = null;
            gridServers.DataSource = _viewModel.Servers;
        };
    }
}
