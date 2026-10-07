namespace Views.Forms.SubViews;

partial class UserConfigView : Form
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
        this.lblUsername = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblRole = new System.Windows.Forms.Label();
        this.cmbRole = new System.Windows.Forms.ComboBox();
        this.chkActive = new System.Windows.Forms.CheckBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.SuspendLayout();

        this.lblUsername.AutoSize = true; this.lblUsername.Location = new System.Drawing.Point(20, 20); this.lblUsername.Text = "Username:";
        this.txtUsername.Location = new System.Drawing.Point(140, 17); this.txtUsername.Size = new System.Drawing.Size(200, 27); this.txtUsername.TabIndex = 0;
        this.lblPassword.AutoSize = true; this.lblPassword.Location = new System.Drawing.Point(20, 60); this.lblPassword.Text = "Password:";
        this.txtPassword.Location = new System.Drawing.Point(140, 57); this.txtPassword.Size = new System.Drawing.Size(200, 27); this.txtPassword.TabIndex = 1; this.txtPassword.UseSystemPasswordChar = true;
        this.lblRole.AutoSize = true; this.lblRole.Location = new System.Drawing.Point(20, 100); this.lblRole.Text = "Role:";
        this.cmbRole.Location = new System.Drawing.Point(140, 97); this.cmbRole.Size = new System.Drawing.Size(200, 28); this.cmbRole.TabIndex = 2; this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.chkActive.AutoSize = true; this.chkActive.Location = new System.Drawing.Point(140, 140); this.chkActive.Text = "Active"; this.chkActive.TabIndex = 3; this.chkActive.Checked = true;
        this.btnSave.Location = new System.Drawing.Point(140, 180); this.btnSave.Size = new System.Drawing.Size(110, 32); this.btnSave.TabIndex = 4; this.btnSave.Text = "Save"; this.btnSave.UseVisualStyleBackColor = false;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(380, 240);
        this.Controls.Add(this.lblUsername); this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblPassword); this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblRole); this.Controls.Add(this.cmbRole);
        this.Controls.Add(this.chkActive); this.Controls.Add(this.btnSave);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "UserConfigView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "User Configuration";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblRole;
    private System.Windows.Forms.ComboBox cmbRole;
    private System.Windows.Forms.CheckBox chkActive;
    private System.Windows.Forms.Button btnSave;
}
