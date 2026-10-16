using Controller.Processing;
using Controller.Security;
using Views.Components;

namespace Views.Forms.SubViews;

public partial class FileMetadataView : Krypton.Toolkit.KryptonForm
{
    private readonly FileMetadataController _meta = new();
    private IReadOnlyList<MetadataItem> _items = Array.Empty<MetadataItem>();

    public FileMetadataView(Model.Entities.FileModel file)
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        Text = $"Metadata — {file.FileName}";
        LoadMetadataAsync(file).ConfigureAwait(false);
    }

    private async Task LoadMetadataAsync(Model.Entities.FileModel file)
    {
        _items = await _meta.InspectVaultAsync(file, AppServices.Files, AppServices.Vault);
        lvMeta.BeginUpdate();
        lvMeta.Items.Clear();
        foreach (var g in _items.GroupBy(i => i.Group))
        {
            var group = new ListViewGroup(g.Key, HorizontalAlignment.Left) { Tag = g.Key };
            lvMeta.Groups.Add(group);
            foreach (var it in g)
            {
                var lvi = new ListViewItem(it.Label)
                {
                    SubItems = { it.Value },
                    Group = group,
                    Tag = it.Value
                };
                lvMeta.Items.Add(lvi);
            }
        }
        lvMeta.EndUpdate();
        lblCount.Text = $"{_items.Count} property(ies)";
    }

    private void lvMeta_DoubleClick(object? sender, EventArgs e)
    {
        if (lvMeta.SelectedItems.Count > 0)
        {
            string v = lvMeta.SelectedItems[0].SubItems[1].Text;
            if (!string.IsNullOrEmpty(v))
            {
                Clipboard.SetText(v);
                lblCount.Text = "Copied to clipboard.";
            }
        }
    }

    private void btnCopy_Click(object? sender, EventArgs e)
    {
        if (lvMeta.SelectedItems.Count > 0)
        {
            Clipboard.SetText(lvMeta.SelectedItems[0].SubItems[1].Text);
            lblCount.Text = "Copied to clipboard.";
        }
    }

    private void btnCopyAll_Click(object? sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new();
        foreach (var it in _items)
            sb.AppendLine($"[{it.Group}] {it.Label}: {it.Value}");
        Clipboard.SetText(sb.ToString());
        lblCount.Text = "All metadata copied.";
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
