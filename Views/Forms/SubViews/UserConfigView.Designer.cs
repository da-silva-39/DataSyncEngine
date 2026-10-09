namespace Views.Forms.SubViews;

partial class UserConfigView : MaterialSkin.Controls.MaterialForm
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
        this.txtUsername = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtPassword = new MaterialSkin.Controls.MaterialTextBox2();
        this.cmbRole = new MaterialSkin.Controls.MaterialComboBox();
        this.chkActive = new MaterialSkin.Controls.MaterialCheckbox();
        this.btnSave = new MaterialSkin.Controls.MaterialButton();
        this.SuspendLayout();

        this.txtUsername.Hint = "Username";
        this.txtUsername.Location = new System.Drawing.Point(20, 80);
        this.txtUsername.MaxLength = 32767;
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(340, 48);
        this.txtUsername.TabIndex = 0;

        this.txtPassword.Hint = "Password";
        this.txtPassword.Location = new System.Drawing.Point(20, 140);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(340, 48);
        this.txtPassword.TabIndex = 1;
        this.txtPassword.UseSystemPasswordChar = true;

        this.cmbRole.AutoResize = false;
        this.cmbRole.Hint = "Role";
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

        this.btnSave.AutoSize = false;
        this.btnSave.HighEmphasis = true;
        this.btnSave.Location = new System.Drawing.Point(20, 310);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(340, 36);
        this.btnSave.TabIndex = 4;
        this.btnSave.Text = "Save";
        this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnSave.UseAccentColor = false;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(380, 370);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.cmbRole);
        this.Controls.Add(this.chkActive);
        this.Controls.Add(this.btnSave);
        this.Name = "UserConfigView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "User Configuration";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private MaterialSkin.Controls.MaterialTextBox2 txtUsername;
    private MaterialSkin.Controls.MaterialTextBox2 txtPassword;
    private MaterialSkin.Controls.MaterialComboBox cmbRole;
    private MaterialSkin.Controls.MaterialCheckbox chkActive;
    private MaterialSkin.Controls.MaterialButton btnSave;
}
