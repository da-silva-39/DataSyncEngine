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

        tabAudit.Enter += async (_, _) =>
        {
            await _viewModel.LoadAuditAsync();
            gridAudit.DataSource = null;
            gridAudit.DataSource = _viewModel.AuditEntries;
        };
    }
}
