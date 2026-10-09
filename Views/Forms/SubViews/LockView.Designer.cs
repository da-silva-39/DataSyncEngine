namespace Views.Forms.SubViews;

partial class LockView : Krypton.Toolkit.KryptonForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cooldownTimer.Dispose();
            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblTitle = new Krypton.Toolkit.KryptonLabel();
        this.picAvatar = new System.Windows.Forms.PictureBox();
        this.lblUser = new Krypton.Toolkit.KryptonLabel();
        this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
        this.btnUnlock = new Krypton.Toolkit.KryptonButton();
        this.lblError = new System.Windows.Forms.Label();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Location = new System.Drawing.Point(20, 18);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Text = "Screen locked";

        this.picAvatar.Location = new System.Drawing.Point(20, 55);
        this.picAvatar.Name = "picAvatar";
        this.picAvatar.Size = new System.Drawing.Size(56, 56);
        this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picAvatar.TabIndex = 1;
        this.picAvatar.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();

        this.lblUser.Location = new System.Drawing.Point(90, 58);
        this.lblUser.Name = "lblUser";
        this.lblUser.Size = new System.Drawing.Size(300, 50);
        this.lblUser.Text = "";

        this.txtPassword.CueHint.CueHintText = "Password";
        this.txtPassword.Location = new System.Drawing.Point(20, 130);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(380, 36);
        this.txtPassword.TabIndex = 0;
        this.txtPassword.UseSystemPasswordChar = true;

        this.btnUnlock.Location = new System.Drawing.Point(20, 180);
        this.btnUnlock.Name = "btnUnlock";
        this.btnUnlock.Size = new System.Drawing.Size(380, 40);
        this.btnUnlock.TabIndex = 1;
        this.btnUnlock.Text = "UNLOCK";
        this.btnUnlock.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);

        this.lblError.AutoSize = false;
        this.lblError.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(20, 232);
        this.lblError.Name = "lblError";
        this.lblError.Size = new System.Drawing.Size(380, 50);
        this.lblError.TabIndex = 5;
        this.lblError.Text = "";

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(420, 295);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.picAvatar);
        this.Controls.Add(this.lblUser);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.btnUnlock);
        this.Controls.Add(this.lblError);
        this.ControlBox = false;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "LockView";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DataSyncEngine - Locked";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Krypton.Toolkit.KryptonLabel lblTitle;
    private System.Windows.Forms.PictureBox picAvatar;
    private Krypton.Toolkit.KryptonLabel lblUser;
    private Krypton.Toolkit.KryptonTextBox txtPassword;
    private Krypton.Toolkit.KryptonButton btnUnlock;
    private System.Windows.Forms.Label lblError;
}
