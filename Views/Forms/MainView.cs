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
                byte[] Key() => AppServices.Aes.DeriveKey(AppServices.MasterKey);
                var engine = new Controller.Sync.SyncEngineController(AppServices.Aes, AppServices.Compressor, AppServices.Files, Key);
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
    }
}
