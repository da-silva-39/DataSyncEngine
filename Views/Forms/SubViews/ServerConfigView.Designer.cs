namespace Views.Forms.SubViews;

partial class ServerConfigView : Krypton.Toolkit.KryptonForm
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
        this.txtName = new Krypton.Toolkit.KryptonTextBox();
        this.txtHost = new Krypton.Toolkit.KryptonTextBox();
        this.txtPort = new Krypton.Toolkit.KryptonTextBox();
        this.txtDatabase = new Krypton.Toolkit.KryptonTextBox();
        this.txtUser = new Krypton.Toolkit.KryptonTextBox();
        this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
        this.chkActive = new Krypton.Toolkit.KryptonCheckBox();
        this.btnSave = new Krypton.Toolkit.KryptonButton();
        this.btnTest = new Krypton.Toolkit.KryptonButton();
        this.SuspendLayout();

        this.txtName.CueHint.CueHintText = "Name";
        this.txtName.Location = new System.Drawing.Point(20, 80);
        this.txtName.MaxLength = 32767;
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(360, 48);
        this.txtName.TabIndex = 0;

        this.txtHost.CueHint.CueHintText = "Host";
        this.txtHost.Location = new System.Drawing.Point(20, 140);
        this.txtHost.MaxLength = 32767;
        this.txtHost.Name = "txtHost";
        this.txtHost.Size = new System.Drawing.Size(360, 48);
        this.txtHost.TabIndex = 1;

        this.txtPort.CueHint.CueHintText = "Port";
        this.txtPort.Location = new System.Drawing.Point(20, 200);
        this.txtPort.MaxLength = 32767;
        this.txtPort.Name = "txtPort";
        this.txtPort.Size = new System.Drawing.Size(360, 48);
        this.txtPort.TabIndex = 2;
        this.txtPort.Text = "3306";

        this.txtDatabase.CueHint.CueHintText = "Database";
        this.txtDatabase.Location = new System.Drawing.Point(20, 260);
        this.txtDatabase.MaxLength = 32767;
        this.txtDatabase.Name = "txtDatabase";
        this.txtDatabase.Size = new System.Drawing.Size(360, 48);
        this.txtDatabase.TabIndex = 3;

        this.txtUser.CueHint.CueHintText = "User";
        this.txtUser.Location = new System.Drawing.Point(20, 320);
        this.txtUser.MaxLength = 32767;
        this.txtUser.Name = "txtUser";
        this.txtUser.Size = new System.Drawing.Size(360, 48);
        this.txtUser.TabIndex = 4;

        this.txtPassword.CueHint.CueHintText = "Password";
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
        this.btnTest.Location = new System.Drawing.Point(20, 490);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(165, 36);
        this.btnTest.TabIndex = 7;
        this.btnTest.Text = "Test";
        this.btnTest.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

        this.btnSave.AutoSize = false;
        this.btnSave.Location = new System.Drawing.Point(215, 490);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(165, 36);
        this.btnSave.TabIndex = 8;
        this.btnSave.Text = "Save";
        this.btnSave.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
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

    private Krypton.Toolkit.KryptonTextBox txtName;
    private Krypton.Toolkit.KryptonTextBox txtHost;
    private Krypton.Toolkit.KryptonTextBox txtPort;
    private Krypton.Toolkit.KryptonTextBox txtDatabase;
    private Krypton.Toolkit.KryptonTextBox txtUser;
    private Krypton.Toolkit.KryptonTextBox txtPassword;
    private Krypton.Toolkit.KryptonCheckBox chkActive;
    private Krypton.Toolkit.KryptonButton btnSave;
    private Krypton.Toolkit.KryptonButton btnTest;
}
