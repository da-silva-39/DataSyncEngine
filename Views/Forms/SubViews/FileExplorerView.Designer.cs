namespace Views.Forms.SubViews;

partial class FileExplorerView : Form
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
        this.lblFolder = new System.Windows.Forms.Label();
        this.txtPath = new System.Windows.Forms.TextBox();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.grid = new System.Windows.Forms.DataGridView();
        this.lblStatus = new System.Windows.Forms.Label();
        this.ctxMenu = new System.Windows.Forms.ContextMenuStrip();
        this.ctxSync = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxHash = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxProps = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxResume = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxDelete = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxRefresh = new System.Windows.Forms.ToolStripMenuItem();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

        this.lblFolder.AutoSize = true;
        this.lblFolder.Location = new System.Drawing.Point(15, 20);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Text = "Folder:";

        this.txtPath.Location = new System.Drawing.Point(70, 17);
        this.txtPath.Name = "txtPath";
        this.txtPath.Size = new System.Drawing.Size(620, 27);
        this.txtPath.TabIndex = 0;

        this.btnBrowse.Location = new System.Drawing.Point(700, 15);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(110, 30);
        this.btnBrowse.TabIndex = 1;
        this.btnBrowse.Text = "Browse...";
        this.btnBrowse.UseVisualStyleBackColor = false;
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

        this.grid.AllowUserToAddRows = false;
        this.grid.AllowUserToDeleteRows = false;
        this.grid.ReadOnly = true;
        this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.grid.Location = new System.Drawing.Point(15, 60);
        this.grid.Name = "grid";
        this.grid.Size = new System.Drawing.Size(900, 460);
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
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        this.Name = "FileExplorerView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "DataSyncEngine - Explorer";
        ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblFolder;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ContextMenuStrip ctxMenu;
    private System.Windows.Forms.ToolStripMenuItem ctxSync;
    private System.Windows.Forms.ToolStripMenuItem ctxHash;
    private System.Windows.Forms.ToolStripMenuItem ctxProps;
    private System.Windows.Forms.ToolStripMenuItem ctxResume;
    private System.Windows.Forms.ToolStripMenuItem ctxDelete;
    private System.Windows.Forms.ToolStripMenuItem ctxRefresh;
}
