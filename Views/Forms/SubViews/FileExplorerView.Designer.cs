namespace Views.Forms.SubViews;

partial class FileExplorerView : MaterialSkin.Controls.MaterialForm
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
        this.lblFolder = new MaterialSkin.Controls.MaterialLabel();
        this.txtPath = new MaterialSkin.Controls.MaterialTextBox2();
        this.btnBrowse = new MaterialSkin.Controls.MaterialButton();
        this.grid = new System.Windows.Forms.DataGridView();
        this.lblStatus = new MaterialSkin.Controls.MaterialLabel();
        this.ctxSync = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.ctxHash = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.ctxProps = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.ctxResume = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.ctxDelete = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.ctxRefresh = new MaterialSkin.Controls.MaterialToolStripMenuItem();
        this.SuspendLayout();
        this.components = new System.ComponentModel.Container();
        this.ctxMenu = new MaterialSkin.Controls.MaterialContextMenuStrip();
        ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

        this.lblFolder.AutoSize = true;
        this.lblFolder.Location = new System.Drawing.Point(15, 85);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Text = "Folder:";

        this.txtPath.Hint = "Select a folder to explore...";
        this.txtPath.Location = new System.Drawing.Point(85, 75);
        this.txtPath.MaxLength = 32767;
        this.txtPath.Name = "txtPath";
        this.txtPath.ReadOnly = true;
        this.txtPath.Size = new System.Drawing.Size(600, 36);
        this.txtPath.TabIndex = 0;
        this.txtPath.UseTallSize = false;

        this.btnBrowse.AutoSize = false;
        this.btnBrowse.HighEmphasis = true;
        this.btnBrowse.Location = new System.Drawing.Point(700, 76);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(110, 36);
        this.btnBrowse.TabIndex = 1;
        this.btnBrowse.Text = "Browse...";
        this.btnBrowse.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnBrowse.UseAccentColor = false;
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

    private MaterialSkin.Controls.MaterialLabel lblFolder;
    private MaterialSkin.Controls.MaterialTextBox2 txtPath;
    private MaterialSkin.Controls.MaterialButton btnBrowse;
    private System.Windows.Forms.DataGridView grid;
    private MaterialSkin.Controls.MaterialLabel lblStatus;
    private MaterialSkin.Controls.MaterialContextMenuStrip ctxMenu;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxSync;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxHash;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxProps;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxResume;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxDelete;
    private MaterialSkin.Controls.MaterialToolStripMenuItem ctxRefresh;
}
