using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Views.Components;

namespace Views.Forms.SubViews;

public partial class FileExplorerView : Krypton.Toolkit.KryptonForm
{
    private readonly List<Model.Entities.FileModel> _files = new();
    private readonly List<Model.Entities.FileModel> _filtered = new();
    private string _root = string.Empty;
    private string _current = string.Empty;
    private readonly Controller.Processing.FileMetadataController _meta = new();

    public FileExplorerView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        grid.CellFormatting += Grid_CellFormatting;
        this.AllowDrop = true;
        this.DragEnter += grid_DragEnter;
        this.DragDrop += grid_DragDrop;
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

    private void btnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        _current = _root = dialog.SelectedPath;
        txtAddress.Text = _current;
        BuildTree(_root);
        _ = LoadFolderAsync(_current);
    }

    private void btnBack_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_current)) return;
        DirectoryInfo di = new DirectoryInfo(_current);
        if (di.Parent == null) return;
        _current = di.Parent.FullName;
        txtAddress.Text = _current;
        _ = LoadFolderAsync(_current);
    }

    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_current)) _ = LoadFolderAsync(_current);
    }

    private async void btnScan_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_current)) return;
        lblStatus.Text = "Scanning...";
        var items = await ScanFolder(_current);
        _files.Clear();
        _files.AddRange(items);
        ApplyFilter();
        lblStatus.Text = "Scan complete.";
        await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.FolderScanned, _current);
    }

    private async void btnSyncAll_Click(object? sender, EventArgs e)
    {
        if (grid.SelectedRows.Count == 0) return;
        var engine = AppServices.SyncEngine;
        int ok = 0;
        foreach (DataGridViewRow r in grid.SelectedRows)
        {
            if (r.DataBoundItem is Model.Entities.FileModel file)
            {
                try
                {
                    bool res = await Task.Run(() => engine.ProcessFileAsync(file, System.IO.File.ReadAllBytes(file.FilePath)));
                    if (res) ok++;
                }
                catch
                {
                }
            }
        }
        grid.Refresh();
        lblStatus.Text = $"{ok} file(s) synchronized.";
    }

    private async Task LoadFolderAsync(string path)
    {
        _files.Clear();
        var items = await ScanFolder(path);
        _files.AddRange(items);
        ApplyFilter();
        txtAddress.Text = path;
        _current = path;
    }

    private async Task<IReadOnlyList<Model.Entities.FileModel>> ScanFolder(string path)
    {
        var scanner = new Controller.Sync.DirectoryScannerController(AppServices.Sha256, AppServices.Files);
        scanner.ExcludePatternsProvider = () => AppServices.Settings.ExcludePatterns;
        scanner.MaxFileSizeMbProvider = () => AppServices.Settings.MaxFileSizeMb;
        return await scanner.ScanAsync(path);
    }

    private void BuildTree(string root)
    {
        treeDirs.Nodes.Clear();
        if (string.IsNullOrEmpty(root)) return;
        TreeNode rootNode = new TreeNode(Path.GetFileName(root) == "" ? root : Path.GetFileName(root));
        rootNode.Tag = root;
        treeDirs.Nodes.Add(rootNode);
        AddNodes(rootNode, root);
    }

    private void AddNodes(TreeNode parentNode, string path)
    {
        try
        {
            foreach (string dir in System.IO.Directory.GetDirectories(path))
            {
                TreeNode node = new TreeNode(Path.GetFileName(dir));
                node.Tag = dir;
                parentNode.Nodes.Add(node);
            }
        }
        catch
        {
        }
    }

    private void treeDirs_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is string path && System.IO.Directory.Exists(path))
        {
            _ = LoadFolderAsync(path);
        }
    }

    private void treeDirs_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
    {
        if (e.Node == null) return;
        foreach (TreeNode n in e.Node.Nodes)
        {
            if (n.Nodes.Count == 0)
                AddNodes(n, n.Tag?.ToString() ?? "");
        }
    }

    private void ApplyFilter()
    {
        string q = txtSearch.Text?.Trim() ?? "";
        _filtered.Clear();
        var src = string.IsNullOrEmpty(q) ? _files : _files.Where(f => f.FileName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        _filtered.AddRange(src);
        grid.DataSource = null;
        grid.DataSource = _filtered;
        lblCount.Text = $"{_filtered.Count}/{_files.Count} item(s)";
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        ApplyFilter();
    }

    private Model.Entities.FileModel? SelectedFile => grid.CurrentRow?.DataBoundItem as Model.Entities.FileModel;

    private void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (SelectedFile == null) return;
        var items = _meta.Inspect(SelectedFile);
        var sb = new StringBuilder();
        foreach (var g in items.GroupBy(i => i.Group))
        {
            sb.AppendLine($"=== {g.Key} ===");
            foreach (var it in g)
                sb.AppendLine($"{it.Label}: {it.Value}");
            sb.AppendLine();
        }
        txtDetails.Text = sb.ToString();
    }

    private void grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (SelectedFile != null) ctxMetadata_Click(sender, e);
    }

    private async void ctxSync_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        var engine = AppServices.SyncEngine;
        lblStatus.Text = "Synchronizing...";
        bool ok = await Task.Run(() => engine.ProcessFileAsync(file, System.IO.File.ReadAllBytes(file.FilePath)));
        grid.Refresh();
        lblStatus.Text = ok ? $"{file.FileName} synchronized." : $"{file.FileName} marked for resume.";
        ToastForm.ShowToast(this, lblStatus.Text, ok ? ToastKind.Success : ToastKind.Warning);
    }

    private async void ctxDownload_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        if (file.Id <= 0)
        {
            lblStatus.Text = "File is not on the server yet.";
            ToastForm.ShowToast(this, "Sync first.", ToastKind.Warning);
            return;
        }
        using var dialog = new SaveFileDialog { FileName = file.FileName };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await Task.Run(() => AppServices.Backup.ExportSingleAsync(file, dialog.FileName));
            lblStatus.Text = $"Downloaded to {dialog.FileName}.";
            ToastForm.ShowToast(this, "Downloaded.", ToastKind.Success);
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Download failed: {ex.Message}";
            ToastForm.ShowToast(this, "Download failed.", ToastKind.Error);
        }
    }

    private void ctxMetadata_Click(object? sender, EventArgs e)
    {
        if (SelectedFile != null)
        {
            using var dlg = new FileMetadataView(SelectedFile);
            dlg.ShowDialog(this);
        }
    }

    private void ctxHash_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is { } file)
        {
            Clipboard.SetText(file.Sha256Hash);
            lblStatus.Text = "Hash copied.";
        }
    }

    private async void ctxResume_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        var engine = AppServices.SyncEngine;
        engine.MarkForResume(file.FilePath);
        file.Status = Core.Enums.SyncStatus.Pending;
        grid.Refresh();
        int ok = await Task.Run(() => engine.ResumePendingAsync(
            f => System.IO.File.ReadAllBytes(f.FilePath),
            path => _files.FirstOrDefault(f => f.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                ?? new Model.Entities.FileModel { FilePath = path, FileName = System.IO.Path.GetFileName(path) }));
        lblStatus.Text = $"Resume: {ok} file(s) recovered.";
    }

    private async void ctxDelete_Click(object? sender, EventArgs e)
    {
        if (SelectedFile is not { } file) return;
        if (AppServices.Session.Role == Core.Enums.UserRole.Operator)
        {
            using var masterKey = new MasterKeyPopUpView();
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.MasterKeyPrompted, $"Operator delete {file.FilePath}.");
            if (masterKey.ShowDialog(this) != DialogResult.OK || !masterKey.Authorized) return;
        }
        if (MessageBox.Show($"Move {file.FileName} to trash?", "Explorer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
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
        ApplyFilter();
        lblStatus.Text = "Moved to trash.";
    }

    private void ctxRefresh_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_current)) _ = LoadFolderAsync(_current);
    }

    private void grid_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            e.Effect = DragDropEffects.Copy;
    }

    private async void grid_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 0) return;
            string folder = files.Length == 1 && System.IO.Directory.Exists(files[0]) ? files[0] : System.IO.Path.GetDirectoryName(files[0]) ?? _current;
            if (!string.IsNullOrEmpty(folder)) _current = folder;
            txtAddress.Text = _current;
            await LoadFolderAsync(_current);
        }
    }
}
