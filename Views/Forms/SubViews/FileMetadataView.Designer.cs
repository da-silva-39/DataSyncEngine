namespace Views.Forms.SubViews;

partial class FileMetadataView : Krypton.Toolkit.KryptonForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lvMeta = new System.Windows.Forms.ListView();
        this.colLabel = new System.Windows.Forms.ColumnHeader();
        this.colValue = new System.Windows.Forms.ColumnHeader();
        this.lblCount = new Krypton.Toolkit.KryptonLabel();
        this.btnCopy = new Krypton.Toolkit.KryptonButton();
        this.btnCopyAll = new Krypton.Toolkit.KryptonButton();
        this.btnClose = new Krypton.Toolkit.KryptonButton();
        this.SuspendLayout();

        this.lvMeta.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colLabel, this.colValue });
        this.lvMeta.FullRowSelect = true;
        this.lvMeta.HideSelection = false;
        this.lvMeta.Location = new System.Drawing.Point(15, 20);
        this.lvMeta.Name = "lvMeta";
        this.lvMeta.Size = new System.Drawing.Size(680, 320);
        this.lvMeta.TabIndex = 0;
        this.lvMeta.UseCompatibleStateImageBehavior = false;
        this.lvMeta.View = System.Windows.Forms.View.Details;
        this.lvMeta.DoubleClick += new System.EventHandler(this.lvMeta_DoubleClick);

        this.colLabel.Text = "Property";
        this.colLabel.Width = 140;
        this.colValue.Text = "Value";
        this.colValue.Width = 500;

        this.lblCount.AutoSize = true;
        this.lblCount.Location = new System.Drawing.Point(15, 355);
        this.lblCount.Name = "lblCount";
        this.lblCount.Text = "Loading...";

        this.btnCopy.Location = new System.Drawing.Point(400, 350);
        this.btnCopy.Name = "btnCopy";
        this.btnCopy.Size = new System.Drawing.Size(90, 30);
        this.btnCopy.Text = "Copy";
        this.btnCopy.TabIndex = 1;
        this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);

        this.btnCopyAll.Location = new System.Drawing.Point(500, 350);
        this.btnCopyAll.Name = "btnCopyAll";
        this.btnCopyAll.Size = new System.Drawing.Size(95, 30);
        this.btnCopyAll.Text = "Copy All";
        this.btnCopyAll.TabIndex = 2;
        this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);

        this.btnClose.Location = new System.Drawing.Point(605, 350);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(90, 30);
        this.btnClose.Text = "Close";
        this.btnClose.TabIndex = 3;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(710, 390);
        this.Controls.Add(this.lblCount);
        this.Controls.Add(this.btnCopy);
        this.Controls.Add(this.btnCopyAll);
        this.Controls.Add(this.btnClose);
        this.Controls.Add(this.lvMeta);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        this.Name = "FileMetadataView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "File Metadata";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.ListView lvMeta;
    private System.Windows.Forms.ColumnHeader colLabel;
    private System.Windows.Forms.ColumnHeader colValue;
    private Krypton.Toolkit.KryptonLabel lblCount;
    private Krypton.Toolkit.KryptonButton btnCopy;
    private Krypton.Toolkit.KryptonButton btnCopyAll;
    private Krypton.Toolkit.KryptonButton btnClose;
}
