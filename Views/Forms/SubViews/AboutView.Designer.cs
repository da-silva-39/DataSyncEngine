namespace Views.Forms.SubViews;

partial class AboutView : MaterialSkin.Controls.MaterialForm
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
        this.lblProduct = new MaterialSkin.Controls.MaterialLabel();
        this.lblVersion = new MaterialSkin.Controls.MaterialLabel();
        this.lblDescription = new MaterialSkin.Controls.MaterialLabel();
        this.btnOk = new MaterialSkin.Controls.MaterialButton();
        this.SuspendLayout();

        this.lblProduct.AutoSize = true;
        this.lblProduct.Location = new System.Drawing.Point(30, 85);
        this.lblProduct.Name = "lblProduct";
        this.lblProduct.TabIndex = 0;
        this.lblProduct.Text = "DataSyncEngine";

        this.lblVersion.AutoSize = true;
        this.lblVersion.Location = new System.Drawing.Point(32, 125);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.TabIndex = 1;
        this.lblVersion.Text = "Version";

        this.lblDescription.AutoSize = true;
        this.lblDescription.Location = new System.Drawing.Point(32, 160);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.TabIndex = 2;
        this.lblDescription.Text = "Corporate file synchronization with AES-256 encryption,\r\ncompression and MariaDB persistence.";

        this.btnOk.AutoSize = false;
        this.btnOk.HighEmphasis = true;
        this.btnOk.Location = new System.Drawing.Point(150, 230);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(100, 36);
        this.btnOk.TabIndex = 3;
        this.btnOk.Text = "OK";
        this.btnOk.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnOk.UseAccentColor = false;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 285);
        this.Controls.Add(this.lblProduct);
        this.Controls.Add(this.lblVersion);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.btnOk);
        this.Name = "AboutView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "About DataSyncEngine";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private MaterialSkin.Controls.MaterialLabel lblProduct;
    private MaterialSkin.Controls.MaterialLabel lblVersion;
    private MaterialSkin.Controls.MaterialLabel lblDescription;
    private MaterialSkin.Controls.MaterialButton btnOk;
}
