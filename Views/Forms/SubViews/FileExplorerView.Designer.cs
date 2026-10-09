namespace Views.Forms.SubViews;

partial class FileExplorerView : Krypton.Toolkit.KryptonForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblFolder = new Krypton.Toolkit.KryptonLabel();
        this.txtPath = new Krypton.Toolkit.KryptonTextBox();
        this.btnBrowse = new Krypton.Toolkit.KryptonButton();
        this.grid = new System.Windows.Forms.DataGridView();
        this.lblStatus = new Krypton.Toolkit.KryptonLabel();
        this.ctxSync = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxHash = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxProps = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxResume = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.SuspendLayout();
        this.components = new System.ComponentModel.Container();
        this.ctxMenu = new System.Windows.Forms.ContextMenuStrip();
        ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

        this.lblFolder.AutoSize = true;
        this.lblFolder.Location = new System.Drawing.Point(15, 85);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Text = "Folder:";

        this.txtPath.CueHint.CueHintText = "Select a folder to explore...";
        this.txtPath.Location = new System.Drawing.Point(85, 75);
        this.txtPath.MaxLength = 32767;
        this.txtPath.Name = "txtPath";
        this.txtPath.ReadOnly = true;
        this.txtPath.Size = new System.Drawing.Size(600, 36);
        this.txtPath.TabIndex = 0;

        this.btnBrowse.AutoSize = false;
        this.btnBrowse.Location = new System.Drawing.Point(700, 76);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(110, 36);
        this.btnBrowse.TabIndex = 1;
        this.btnBrowse.Text = "Browse...";
        this.btnBrowse.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

        this.grid.AllowUserToAddRows = false;
        this.grid.AllowUserToDeleteRows = false;
        this.grid.ReadOnly = true;
        this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.grid.Location = new System.Drawing.Point(15, 125);
        this.grid.Name = "grid";
        this.grid.Size = new System.Drawing.Size(900, 395);
        this.grid.TabIndex = 2;
        this.grid.ContextMenuStrip = this.ctxMenu;

        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(15, 530);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Text = "Ready.";

        this.ctxSync.Text = "Synchronize Now";
        this.ctxHash.Text = "Copy SHA-256";
        this.ctxProps.Text = "Properties";
        this.ctxResume.Text = "Mark for Resume";
        this.ctxDelete.Text = "Delete (restricted)";
        this.ctxRefresh.Text = "Refresh";
        this.ctxSync.Click += new System.EventHandler(this.ctxSync_Click);
        this.ctxHash.Click += new System.EventHandler(this.ctxHash_Click);
        this.ctxProps.Click += new System.EventHandler(this.ctxProps_Click);
        this.ctxResume.Click += new System.EventHandler(this.ctxResume_Click);
        this.ctxDelete.Click += new System.EventHandler(this.ctxDelete_Click);
        this.ctxRefresh.Click += new System.EventHandler(this.ctxRefresh_Click);
        this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.ctxSync, this.ctxHash, this.ctxProps, this.ctxResume, this.ctxDelete, new System.Windows.Forms.ToolStripSeparator(), this.ctxRefresh });

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(930, 565);
        this.Controls.Add(this.lblFolder);
        this.Controls.Add(this.txtPath);
        this.Controls.Add(this.btnBrowse);
        this.Controls.Add(this.grid);
        this.Controls.Add(this.lblStatus);
        this.Name = "FileExplorerView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "DataSyncEngine - Explorer";
        ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Krypton.Toolkit.KryptonLabel lblFolder;
    private Krypton.Toolkit.KryptonTextBox txtPath;
    private Krypton.Toolkit.KryptonButton btnBrowse;
    private System.Windows.Forms.DataGridView grid;
    private Krypton.Toolkit.KryptonLabel lblStatus;
    private System.Windows.Forms.ContextMenuStrip ctxMenu;
    private System.Windows.Forms.ToolStripMenuItem ctxSync;
    private System.Windows.Forms.ToolStripMenuItem ctxHash;
    private System.Windows.Forms.ToolStripMenuItem ctxProps;
    private System.Windows.Forms.ToolStripMenuItem ctxResume;
    private System.Windows.Forms.ToolStripMenuItem ctxDelete;
    private System.Windows.Forms.ToolStripMenuItem ctxRefresh;
}
