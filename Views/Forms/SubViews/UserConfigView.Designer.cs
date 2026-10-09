namespace Views.Forms.SubViews;

partial class UserConfigView : Krypton.Toolkit.KryptonForm
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
        this.txtUsername = new Krypton.Toolkit.KryptonTextBox();
        this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
        this.cmbRole = new Krypton.Toolkit.KryptonComboBox();
        this.chkActive = new Krypton.Toolkit.KryptonCheckBox();
        this.picAvatar = new System.Windows.Forms.PictureBox();
        this.btnPhoto = new Krypton.Toolkit.KryptonButton();
        this.btnSave = new Krypton.Toolkit.KryptonButton();
        this.SuspendLayout();

        this.txtUsername.CueHint.CueHintText = "Username";
        this.txtUsername.Location = new System.Drawing.Point(20, 80);
        this.txtUsername.MaxLength = 32767;
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(340, 48);
        this.txtUsername.TabIndex = 0;

        this.txtPassword.CueHint.CueHintText = "Password";
        this.txtPassword.Location = new System.Drawing.Point(20, 140);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(340, 48);
        this.txtPassword.TabIndex = 1;
        this.txtPassword.UseSystemPasswordChar = true;

        this.cmbRole.CueHint.CueHintText = "Role";
        this.cmbRole.Location = new System.Drawing.Point(20, 200);
        this.cmbRole.Name = "cmbRole";
        this.cmbRole.Size = new System.Drawing.Size(340, 48);
        this.cmbRole.TabIndex = 2;
        this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        this.chkActive.AutoSize = true;
        this.chkActive.Location = new System.Drawing.Point(20, 260);
        this.chkActive.Name = "chkActive";
        this.chkActive.Size = new System.Drawing.Size(100, 37);
        this.chkActive.TabIndex = 3;
        this.chkActive.Text = "Active";
        this.chkActive.Checked = true;

        this.picAvatar = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
        this.picAvatar.Location = new System.Drawing.Point(390, 80);
        this.picAvatar.Name = "picAvatar";
        this.picAvatar.Size = new System.Drawing.Size(170, 170);
        this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picAvatar.TabIndex = 5;
        this.picAvatar.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();

        this.btnPhoto.AutoSize = false;
        this.btnPhoto.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
        this.btnPhoto.Location = new System.Drawing.Point(390, 260);
        this.btnPhoto.Name = "btnPhoto";
        this.btnPhoto.Size = new System.Drawing.Size(170, 36);
        this.btnPhoto.TabIndex = 6;
        this.btnPhoto.Text = "Choose Photo...";
        this.btnPhoto.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
        this.btnPhoto.Click += new System.EventHandler(this.btnPhoto_Click);

        this.btnSave.AutoSize = false;
        this.btnSave.Location = new System.Drawing.Point(20, 310);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(340, 36);
        this.btnSave.TabIndex = 4;
        this.btnSave.Text = "Save";
        this.btnSave.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(580, 370);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.cmbRole);
        this.Controls.Add(this.chkActive);
        this.Controls.Add(this.picAvatar);
        this.Controls.Add(this.btnPhoto);
        this.Controls.Add(this.btnSave);
        this.Name = "UserConfigView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "User Configuration";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Krypton.Toolkit.KryptonTextBox txtUsername;
    private Krypton.Toolkit.KryptonTextBox txtPassword;
    private Krypton.Toolkit.KryptonComboBox cmbRole;
    private Krypton.Toolkit.KryptonCheckBox chkActive;
    private System.Windows.Forms.PictureBox picAvatar;
    private Krypton.Toolkit.KryptonButton btnPhoto;
    private Krypton.Toolkit.KryptonButton btnSave;
}
