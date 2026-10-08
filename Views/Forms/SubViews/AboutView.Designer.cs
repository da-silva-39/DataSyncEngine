namespace Views.Forms.SubViews;

partial class AboutView : Form
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
        this.lblProduct = new System.Windows.Forms.Label();
        this.lblVersion = new System.Windows.Forms.Label();
        this.lblDescription = new System.Windows.Forms.Label();
        this.btnOk = new System.Windows.Forms.Button();
        this.SuspendLayout();

        this.lblProduct.AutoSize = true;
        this.lblProduct.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblProduct.Location = new System.Drawing.Point(30, 25);
        this.lblProduct.Name = "lblProduct";
        this.lblProduct.TabIndex = 0;
        this.lblProduct.Text = "DataSyncEngine";

        this.lblVersion.AutoSize = true;
        this.lblVersion.Location = new System.Drawing.Point(32, 70);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.TabIndex = 1;
        this.lblVersion.Text = "Version";

        this.lblDescription.AutoSize = true;
        this.lblDescription.Location = new System.Drawing.Point(32, 105);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.TabIndex = 2;
        this.lblDescription.Text = "Corporate file synchronization with AES-256 encryption,\r\ncompression and MariaDB persistence.";

        this.btnOk.Location = new System.Drawing.Point(150, 180);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(100, 32);
        this.btnOk.TabIndex = 3;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = false;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 235);
        this.Controls.Add(this.lblProduct);
        this.Controls.Add(this.lblVersion);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.btnOk);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AboutView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "About DataSyncEngine";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblProduct;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.Button btnOk;
}
