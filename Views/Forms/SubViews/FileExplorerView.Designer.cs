namespace Views.Forms.SubViews;

partial class FileExplorerView : Krypton.Toolkit.KryptonForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.splitMain = new System.Windows.Forms.SplitContainer();
        this.treeDirs = new System.Windows.Forms.TreeView();
        this.pnlMain = new System.Windows.Forms.Panel();
        this.splitGrid = new System.Windows.Forms.SplitContainer();
        this.grid = new System.Windows.Forms.DataGridView();
        this.pnlDetails = new System.Windows.Forms.Panel();
        this.lblDetailsTitle = new Krypton.Toolkit.KryptonLabel();
        this.txtDetails = new System.Windows.Forms.TextBox();
        this.pnlToolbar = new System.Windows.Forms.Panel();
        this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
        this.btnBack = new Krypton.Toolkit.KryptonButton();
        this.btnRefresh = new Krypton.Toolkit.KryptonButton();
        this.btnScan = new Krypton.Toolkit.KryptonButton();
        this.btnSyncAll = new Krypton.Toolkit.KryptonButton();
        this.txtSearch = new Krypton.Toolkit.KryptonTextBox();
        this.btnBrowse = new Krypton.Toolkit.KryptonButton();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
        this.ctxMenu = new System.Windows.Forms.ContextMenuStrip();
        this.ctxSync = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxDownload = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxMetadata = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxHash = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxResume = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.ctxRefresh = new System.Windows.Forms.ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
        this.splitMain.Panel1.SuspendLayout();
        this.splitMain.Panel2.SuspendLayout();
        this.splitMain.SuspendLayout();
        this.pnlMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitGrid)).BeginInit();
        this.splitGrid.Panel1.SuspendLayout();
        this.splitGrid.Panel2.SuspendLayout();
        this.splitGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
        this.pnlDetails.SuspendLayout();
        this.pnlToolbar.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.ctxMenu.SuspendLayout();
        this.SuspendLayout();

        // splitMain
        this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitMain.Location = new System.Drawing.Point(0, 45);
        this.splitMain.Name = "splitMain";
        this.splitMain.SplitterDistance = 250;
        this.splitMain.Panel1.Controls.Add(this.treeDirs);
        this.splitMain.Panel2.Controls.Add(this.pnlMain);

        // treeDirs
        this.treeDirs.Dock = System.Windows.Forms.DockStyle.Fill;
        this.treeDirs.HideSelection = false;
        this.treeDirs.Name = "treeDirs";
        this.treeDirs.TabIndex = 0;
        this.treeDirs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDirs_AfterSelect);
        this.treeDirs.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeDirs_BeforeExpand);

        // pnlMain
        this.pnlMain.Controls.Add(this.splitGrid);
        this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlMain.Name = "pnlMain";

        // splitGrid
        this.splitGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitGrid.Location = new System.Drawing.Point(0, 0);
        this.splitGrid.Orientation = System.Windows.Forms.Orientation.Horizontal;
        this.splitGrid.SplitterDistance = 320;
        this.splitGrid.Panel1.Controls.Add(this.grid);
        this.splitGrid.Panel2.Controls.Add(this.pnlDetails);

        // grid
        this.grid.AllowUserToAddRows = false;
        this.grid.AllowUserToDeleteRows = false;
        this.grid.AllowDrop = true;
        this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        this.grid.ReadOnly = true;
        this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grid.MultiSelect = false;
        this.grid.Name = "grid";
        this.grid.RowHeadersVisible = false;
        this.grid.ContextMenuStrip = this.ctxMenu;
        this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
        this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
        this.grid.DragEnter += new System.Windows.Forms.DragEventHandler(this.grid_DragEnter);
        this.grid.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);

        // pnlDetails
        this.pnlDetails.Controls.Add(this.lblDetailsTitle);
        this.pnlDetails.Controls.Add(this.txtDetails);
        this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;

        this.lblDetailsTitle.Location = new System.Drawing.Point(8, 6);
        this.lblDetailsTitle.Text = "Details";
        this.lblDetailsTitle.AutoSize = true;

        this.txtDetails.Location = new System.Drawing.Point(8, 32);
        this.txtDetails.Multiline = true;
        this.txtDetails.ReadOnly = true;
        this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtDetails.Name = "txtDetails";

        // toolbar
        this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlToolbar.Height = 45;

        this.btnBack.Location = new System.Drawing.Point(5, 6);
        this.btnBack.Size = new System.Drawing.Size(70, 32);
        this.btnBack.Text = "Up";
        this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

        this.btnBrowse.Location = new System.Drawing.Point(80, 6);
        this.btnBrowse.Size = new System.Drawing.Size(90, 32);
        this.btnBrowse.Text = "Browse";
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

        this.btnRefresh.Location = new System.Drawing.Point(175, 6);
        this.btnRefresh.Size = new System.Drawing.Size(80, 32);
        this.btnRefresh.Text = "Refresh";
        this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

        this.btnScan.Location = new System.Drawing.Point(260, 6);
        this.btnScan.Size = new System.Drawing.Size(70, 32);
        this.btnScan.Text = "Scan";
        this.btnScan.Click += new System.EventHandler(this.btnScan_Click);

        this.btnSyncAll.Location = new System.Drawing.Point(335, 6);
        this.btnSyncAll.Size = new System.Drawing.Size(95, 32);
        this.btnSyncAll.Text = "Sync Selected";
        this.btnSyncAll.Click += new System.EventHandler(this.btnSyncAll_Click);

        this.txtAddress.Location = new System.Drawing.Point(435, 6);
        this.txtAddress.Size = new System.Drawing.Size(380, 32);
        this.txtAddress.ReadOnly = true;
        this.txtAddress.CueHint.CueHintText = "Current folder path...";

        this.txtSearch.Location = new System.Drawing.Point(820, 6);
        this.txtSearch.Size = new System.Drawing.Size(200, 32);
        this.txtSearch.CueHint.CueHintText = "Search by name...";
        this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

        this.pnlToolbar.Controls.AddRange(new System.Windows.Forms.Control[] {
            this.btnBack, this.btnBrowse, this.btnRefresh, this.btnScan, this.btnSyncAll,
            this.txtAddress, this.txtSearch
        });

        // status
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus, new System.Windows.Forms.ToolStripSeparator(), this.lblCount
        });
        this.lblStatus.Text = "Ready";
        this.lblCount.Text = "0 item(s)";

        // ctx
        this.ctxSync.Text = "Sync now";
        this.ctxDownload.Text = "Download...";
        this.ctxMetadata.Text = "View metadata";
        this.ctxHash.Text = "Copy SHA-256";
        this.ctxResume.Text = "Mark for resume";
        this.ctxDelete.Text = "Move to trash";
        this.ctxRefresh.Text = "Refresh";
        this.ctxSync.Click += new System.EventHandler(this.ctxSync_Click);
        this.ctxDownload.Click += new System.EventHandler(this.ctxDownload_Click);
        this.ctxMetadata.Click += new System.EventHandler(this.ctxMetadata_Click);
        this.ctxHash.Click += new System.EventHandler(this.ctxHash_Click);
        this.ctxResume.Click += new System.EventHandler(this.ctxResume_Click);
        this.ctxDelete.Click += new System.EventHandler(this.ctxDelete_Click);
        this.ctxRefresh.Click += new System.EventHandler(this.ctxRefresh_Click);
        this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxSync, this.ctxDownload, this.ctxMetadata, this.ctxHash, this.ctxResume, this.ctxDelete,
            this.toolStripSeparator1, this.ctxRefresh
        });

        // form
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1100, 650);
        this.Controls.Add(this.splitMain);
        this.Controls.Add(this.pnlToolbar);
        this.Controls.Add(this.statusStrip);
        this.Name = "FileExplorerView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "DataSyncEngine - IDE Explorer";
        this.AllowDrop = true;
        this.DragEnter += new System.Windows.Forms.DragEventHandler(this.grid_DragEnter);
        this.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
        this.splitMain.Panel1.ResumeLayout(false);
        this.splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
        this.splitMain.ResumeLayout(false);
        this.pnlMain.ResumeLayout(false);
        this.splitGrid.Panel1.ResumeLayout(false);
        this.splitGrid.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitGrid)).EndInit();
        this.splitGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
        this.pnlDetails.ResumeLayout(false);
        this.pnlDetails.PerformLayout();
        this.pnlToolbar.ResumeLayout(false);
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.ctxMenu.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.TreeView treeDirs;
    private System.Windows.Forms.Panel pnlMain;
    private System.Windows.Forms.SplitContainer splitGrid;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.Panel pnlDetails;
    private Krypton.Toolkit.KryptonLabel lblDetailsTitle;
    private System.Windows.Forms.TextBox txtDetails;
    private System.Windows.Forms.Panel pnlToolbar;
    private Krypton.Toolkit.KryptonTextBox txtAddress;
    private Krypton.Toolkit.KryptonButton btnBack;
    private Krypton.Toolkit.KryptonButton btnRefresh;
    private Krypton.Toolkit.KryptonButton btnScan;
    private Krypton.Toolkit.KryptonButton btnSyncAll;
    private Krypton.Toolkit.KryptonTextBox txtSearch;
    private Krypton.Toolkit.KryptonButton btnBrowse;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    private System.Windows.Forms.ToolStripStatusLabel lblCount;
    private System.Windows.Forms.ContextMenuStrip ctxMenu;
    private System.Windows.Forms.ToolStripMenuItem ctxSync;
    private System.Windows.Forms.ToolStripMenuItem ctxDownload;
    private System.Windows.Forms.ToolStripMenuItem ctxMetadata;
    private System.Windows.Forms.ToolStripMenuItem ctxHash;
    private System.Windows.Forms.ToolStripMenuItem ctxResume;
    private System.Windows.Forms.ToolStripMenuItem ctxDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem ctxRefresh;
}
