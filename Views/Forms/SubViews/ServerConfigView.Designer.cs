namespace Views.Forms.SubViews;

partial class ServerConfigView : MaterialSkin.Controls.MaterialForm
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
        this.txtName = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtHost = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtPort = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtDatabase = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtUser = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtPassword = new MaterialSkin.Controls.MaterialTextBox2();
        this.chkActive = new MaterialSkin.Controls.MaterialCheckbox();
        this.btnSave = new MaterialSkin.Controls.MaterialButton();
        this.btnTest = new MaterialSkin.Controls.MaterialButton();
        this.SuspendLayout();

        this.txtName.Hint = "Name";
        this.txtName.Location = new System.Drawing.Point(20, 80);
        this.txtName.MaxLength = 32767;
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(360, 48);
        this.txtName.TabIndex = 0;

        this.txtHost.Hint = "Host";
        this.txtHost.Location = new System.Drawing.Point(20, 140);
        this.txtHost.MaxLength = 32767;
        this.txtHost.Name = "txtHost";
        this.txtHost.Size = new System.Drawing.Size(360, 48);
        this.txtHost.TabIndex = 1;

        this.txtPort.Hint = "Port";
        this.txtPort.Location = new System.Drawing.Point(20, 200);
        this.txtPort.MaxLength = 32767;
        this.txtPort.Name = "txtPort";
        this.txtPort.Size = new System.Drawing.Size(360, 48);
        this.txtPort.TabIndex = 2;
        this.txtPort.Text = "3306";

        this.txtDatabase.Hint = "Database";
        this.txtDatabase.Location = new System.Drawing.Point(20, 260);
        this.txtDatabase.MaxLength = 32767;
        this.txtDatabase.Name = "txtDatabase";
        this.txtDatabase.Size = new System.Drawing.Size(360, 48);
        this.txtDatabase.TabIndex = 3;

        this.txtUser.Hint = "User";
        this.txtUser.Location = new System.Drawing.Point(20, 320);
        this.txtUser.MaxLength = 32767;
        this.txtUser.Name = "txtUser";
        this.txtUser.Size = new System.Drawing.Size(360, 48);
        this.txtUser.TabIndex = 4;

        this.txtPassword.Hint = "Password";
        this.txtPassword.Location = new System.Drawing.Point(20, 380);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(360, 48);
        this.txtPassword.TabIndex = 5;
        this.txtPassword.UseSystemPasswordChar = true;

        this.chkActive.AutoSize = true;
        this.chkActive.Location = new System.Drawing.Point(20, 440);
        this.chkActive.Name = "chkActive";
        this.chkActive.Size = new System.Drawing.Size(170, 37);
        this.chkActive.TabIndex = 6;
        this.chkActive.Text = "Set as active server";

        this.btnTest.AutoSize = false;
        this.btnTest.HighEmphasis = false;
        this.btnTest.Location = new System.Drawing.Point(20, 490);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(165, 36);
        this.btnTest.TabIndex = 7;
        this.btnTest.Text = "Test";
        this.btnTest.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
        this.btnTest.UseAccentColor = false;
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

        this.btnSave.AutoSize = false;
        this.btnSave.HighEmphasis = true;
        this.btnSave.Location = new System.Drawing.Point(215, 490);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(165, 36);
        this.btnSave.TabIndex = 8;
        this.btnSave.Text = "Save";
        this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnSave.UseAccentColor = false;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 545);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.txtHost);
        this.Controls.Add(this.txtPort);
        this.Controls.Add(this.txtDatabase);
        this.Controls.Add(this.txtUser);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.chkActive);
        this.Controls.Add(this.btnTest);
        this.Controls.Add(this.btnSave);
        this.Name = "ServerConfigView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Server Configuration";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private MaterialSkin.Controls.MaterialTextBox2 txtName;
    private MaterialSkin.Controls.MaterialTextBox2 txtHost;
    private MaterialSkin.Controls.MaterialTextBox2 txtPort;
    private MaterialSkin.Controls.MaterialTextBox2 txtDatabase;
    private MaterialSkin.Controls.MaterialTextBox2 txtUser;
    private MaterialSkin.Controls.MaterialTextBox2 txtPassword;
    private MaterialSkin.Controls.MaterialCheckbox chkActive;
    private MaterialSkin.Controls.MaterialButton btnSave;
    private MaterialSkin.Controls.MaterialButton btnTest;
}
