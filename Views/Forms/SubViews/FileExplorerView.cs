using Views.Components;

namespace Views.Forms.SubViews;

public partial class FileExplorerView : MaterialSkin.Controls.MaterialForm
{
    private readonly List<Model.Entities.FileModel> _files = new();

    public FileExplorerView()
    {
        InitializeComponent();
        MaterialThemeModule.Apply(this);
        grid.CellFormatting += Grid_CellFormatting;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || grid.Rows[e.RowIndex].DataBoundItem is not Model.Entities.FileModel file) return;
        e.CellStyle.ForeColor = file.Status switch
        {
            Core.Enums.SyncStatus.Synced => DarkThemeModule.SuccessColor,
            Core.Enums.SyncStatus.Pending => DarkThemeModule.WarningColor,
            Core.Enums.SyncStatus.Modified => DarkThemeModule.ModifiedColor,
            _ => DarkThemeModule.TextColor,
        };
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
        lblStatus.Text = "Synchronizing...";
        bool ok = await Task.Run(() => engine.ProcessFileAsync(file, File.ReadAllBytes(file.FilePath)));
        grid.Refresh();
        lblStatus.Text = ok ? $"{file.FileName} synchronized." : $"{file.FileName} marked for resume.";
        ToastForm.ShowToast(this, lblStatus.Text, ok ? ToastKind.Success : ToastKind.Warning);
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
        lblStatus.Text = "Resuming...";
        int ok = await Task.Run(() => engine.ResumePendingAsync(
            f => File.ReadAllBytes(f.FilePath),
            path => _files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                ?? new Model.Entities.FileModel { FilePath = path, FileName = Path.GetFileName(path) }));
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
        if (AppServices.Settings.ConfirmDelete
            && MessageBox.Show($"Delete {file.FileName} from server?", "Explorer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (file.Id > 0)
        {
            try
            {
                byte[]? blob = await AppServices.Files.GetFileBlobAsync(file.Id);
                if (blob != null && blob.Length > 0)
                    await AppServices.Trash.MoveToTrashAsync(file, blob, AppServices.Session.Username ?? "unknown");
                await AppServices.Files.DeleteAsync(file.Id);
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Delete failed: {ex.Message}";
                ToastForm.ShowToast(this, "Delete failed.", ToastKind.Error);
                return;
            }
        }
        _files.Remove(file);
        grid.DataSource = null;
        grid.DataSource = _files;
        lblStatus.Text = "Moved to trash.";
        ToastForm.ShowToast(this, $"{file.FileName} moved to trash.", ToastKind.Warning);
    }

    private async void ctxRefresh_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPath.Text)) await LoadFolderAsync(txtPath.Text);
    }
}
