namespace Views.Forms.SubViews;

partial class UserProfileView : Krypton.Toolkit.KryptonForm
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
        this.picPhoto = new System.Windows.Forms.PictureBox();
        this.lblName = new Krypton.Toolkit.KryptonLabel();
        this.lblRole = new Krypton.Toolkit.KryptonLabel();
        this.lblActive = new Krypton.Toolkit.KryptonLabel();
        this.lblLastSync = new Krypton.Toolkit.KryptonLabel();
        this.lblHistoryCap = new Krypton.Toolkit.KryptonLabel();
        this.lstHistory = new System.Windows.Forms.ListBox();
        this.btnClose = new Krypton.Toolkit.KryptonButton();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).BeginInit();

        this.picPhoto.Location = new System.Drawing.Point(20, 70);
        this.picPhoto.Name = "picPhoto";
        this.picPhoto.Size = new System.Drawing.Size(120, 120);
        this.picPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picPhoto.TabIndex = 0;
        this.picPhoto.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).EndInit();

        this.lblName.AutoSize = true;
        this.lblName.Location = new System.Drawing.Point(155, 75);
        this.lblName.Name = "lblName";
        this.lblName.TabIndex = 1;
        this.lblName.Text = "Name";

        this.lblRole.AutoSize = true;
        this.lblRole.Location = new System.Drawing.Point(155, 105);
        this.lblRole.Name = "lblRole";
        this.lblRole.TabIndex = 2;
        this.lblRole.Text = "Role";

        this.lblActive.AutoSize = true;
        this.lblActive.Location = new System.Drawing.Point(155, 135);
        this.lblActive.Name = "lblActive";
        this.lblActive.TabIndex = 3;
        this.lblActive.Text = "Active";

        this.lblLastSync.AutoSize = true;
        this.lblLastSync.Location = new System.Drawing.Point(155, 165);
        this.lblLastSync.Name = "lblLastSync";
        this.lblLastSync.TabIndex = 4;
        this.lblLastSync.Text = "Last sync";

        this.lblHistoryCap.AutoSize = true;
        this.lblHistoryCap.Location = new System.Drawing.Point(20, 205);
        this.lblHistoryCap.Name = "lblHistoryCap";
        this.lblHistoryCap.TabIndex = 5;
        this.lblHistoryCap.Text = "Recent history";

        this.lstHistory.Location = new System.Drawing.Point(20, 235);
        this.lstHistory.Name = "lstHistory";
        this.lstHistory.Size = new System.Drawing.Size(440, 150);
        this.lstHistory.TabIndex = 6;

        this.btnClose.Location = new System.Drawing.Point(360, 400);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(100, 36);
        this.btnClose.TabIndex = 7;
        this.btnClose.Text = "Close";
        this.btnClose.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(480, 455);
        this.Controls.Add(this.picPhoto);
        this.Controls.Add(this.lblName);
        this.Controls.Add(this.lblRole);
        this.Controls.Add(this.lblActive);
        this.Controls.Add(this.lblLastSync);
        this.Controls.Add(this.lblHistoryCap);
        this.Controls.Add(this.lstHistory);
        this.Controls.Add(this.btnClose);
        this.Name = "UserProfileView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "User Profile";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.PictureBox picPhoto;
    private Krypton.Toolkit.KryptonLabel lblName;
    private Krypton.Toolkit.KryptonLabel lblRole;
    private Krypton.Toolkit.KryptonLabel lblActive;
    private Krypton.Toolkit.KryptonLabel lblLastSync;
    private Krypton.Toolkit.KryptonLabel lblHistoryCap;
    private System.Windows.Forms.ListBox lstHistory;
    private Krypton.Toolkit.KryptonButton btnClose;
}
