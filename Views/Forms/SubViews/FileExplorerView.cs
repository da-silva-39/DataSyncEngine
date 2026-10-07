using Views.Components;

namespace Views.Forms.SubViews;

public partial class FileExplorerView : Form
{
    private readonly List<Model.Entities.FileModel> _files = new();

    public FileExplorerView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
    }

    private async void btnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        txtPath.Text = dialog.SelectedPath;
        await LoadFolderAsync(dialog.SelectedPath);
    }

    private async Task LoadFolderAsync(string path)
    {
        var scanner = new Controller.Sync.DirectoryScannerController(AppServices.Sha256, AppServices.Files);
        _files.Clear();
        _files.AddRange(await scanner.ScanAsync(path));
        grid.DataSource = null;
        grid.DataSource = _files;
        lblStatus.Text = $"{_files.Count} item(s)";
    }

    private Model.Entities.FileModel? SelectedFile => grid.CurrentRow?.DataBoundItem as Model.Entities.FileModel;

    private async void ctxSync_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        var engine = AppServices.SyncEngine;
        bool ok = await engine.ProcessFileAsync(file, File.ReadAllBytes(file.FilePath));
        grid.Refresh();
        lblStatus.Text = ok ? $"{file.FileName} synchronized." : $"{file.FileName} marked for resume.";
    }

    private void ctxHash_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is { } file)
        {
            Clipboard.SetText(file.Sha256Hash);
            lblStatus.Text = "Hash copied to clipboard.";
        }
    }

    private void ctxProps_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        MessageBox.Show(
            $"Name: {file.FileName}\nPath: {file.FilePath}\nSize: {file.SizeBytes} bytes\nSHA-256: {file.Sha256Hash}\nStatus: {file.Status}",
            "Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private async void ctxResume_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        var engine = AppServices.SyncEngine;
        engine.MarkForResume(file.FilePath);
        file.Status = Core.Enums.SyncStatus.Pending;
        int ok = await engine.ResumePendingAsync(
            f => File.ReadAllBytes(f.FilePath),
            path => _files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                ?? new Model.Entities.FileModel { FilePath = path, FileName = Path.GetFileName(path) });
        grid.Refresh();
        lblStatus.Text = $"Resume: {ok} file(s) recovered.";
    }

    private async void ctxDelete_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        if (AppServices.Session.Role == Core.Enums.UserRole.Operator)
        {
            using var masterKey = new MasterKeyPopUpView();
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.MasterKeyPrompted, $"Operator requested delete of {file.FilePath}.");
            if (masterKey.ShowDialog(this) != DialogResult.OK || !masterKey.Authorized) return;
        }
        if (MessageBox.Show($"Delete {file.FileName} from server?", "Explorer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (file.Id > 0) await AppServices.Files.DeleteAsync(file.Id);
        _files.Remove(file);
        grid.DataSource = null;
        grid.DataSource = _files;
        lblStatus.Text = "Deleted.";
    }

    private async void ctxRefresh_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPath.Text)) await LoadFolderAsync(txtPath.Text);
    }
}
