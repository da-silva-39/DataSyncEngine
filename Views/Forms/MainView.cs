using Views.Components;
using Views.ViewModels;

namespace Views.Forms;

public partial class MainView : Form
{
    private readonly MainViewModel _viewModel;

    public MainView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
        _viewModel = new MainViewModel(AppServices.Session.Role ?? Core.Enums.UserRole.Operator);
        ApplyRoleLayout();
        WireEvents();
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
            try
            {
                var engine = AppServices.SyncEngine;
                int ok = await engine.ProcessBatchAsync(pending, f => File.ReadAllBytes(f.FilePath));
                gridFiles.DataSource = null;
                gridFiles.DataSource = _viewModel.Files;
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

        tabServers.Enter += async (_, _) =>
        {
            await _viewModel.LoadServersAsync();
            gridServers.DataSource = null;
            gridServers.DataSource = _viewModel.Servers;
        };
    }
}
