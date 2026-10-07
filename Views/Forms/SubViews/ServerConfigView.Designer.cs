namespace Views.Forms.SubViews;

partial class ServerConfigView : Form
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
        this.lblName = new System.Windows.Forms.Label();
        this.txtName = new System.Windows.Forms.TextBox();
        this.lblHost = new System.Windows.Forms.Label();
        this.txtHost = new System.Windows.Forms.TextBox();
        this.lblPort = new System.Windows.Forms.Label();
        this.txtPort = new System.Windows.Forms.TextBox();
        this.lblDatabase = new System.Windows.Forms.Label();
        this.txtDatabase = new System.Windows.Forms.TextBox();
        this.lblUser = new System.Windows.Forms.Label();
        this.txtUser = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.chkActive = new System.Windows.Forms.CheckBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.SuspendLayout();

        this.lblName.AutoSize = true; this.lblName.Location = new System.Drawing.Point(20, 20); this.lblName.Text = "Name:";
        this.txtName.Location = new System.Drawing.Point(140, 17); this.txtName.Size = new System.Drawing.Size(220, 27); this.txtName.TabIndex = 0;
        this.lblHost.AutoSize = true; this.lblHost.Location = new System.Drawing.Point(20, 60); this.lblHost.Text = "Host:";
        this.txtHost.Location = new System.Drawing.Point(140, 57); this.txtHost.Size = new System.Drawing.Size(220, 27); this.txtHost.TabIndex = 1;
        this.lblPort.AutoSize = true; this.lblPort.Location = new System.Drawing.Point(20, 100); this.lblPort.Text = "Port:";
        this.txtPort.Location = new System.Drawing.Point(140, 97); this.txtPort.Size = new System.Drawing.Size(220, 27); this.txtPort.TabIndex = 2; this.txtPort.Text = "3306";
        this.lblDatabase.AutoSize = true; this.lblDatabase.Location = new System.Drawing.Point(20, 140); this.lblDatabase.Text = "Database:";
        this.txtDatabase.Location = new System.Drawing.Point(140, 137); this.txtDatabase.Size = new System.Drawing.Size(220, 27); this.txtDatabase.TabIndex = 3;
        this.lblUser.AutoSize = true; this.lblUser.Location = new System.Drawing.Point(20, 180); this.lblUser.Text = "User:";
        this.txtUser.Location = new System.Drawing.Point(140, 177); this.txtUser.Size = new System.Drawing.Size(220, 27); this.txtUser.TabIndex = 4;
        this.lblPassword.AutoSize = true; this.lblPassword.Location = new System.Drawing.Point(20, 220); this.lblPassword.Text = "Password:";
        this.txtPassword.Location = new System.Drawing.Point(140, 217); this.txtPassword.Size = new System.Drawing.Size(220, 27); this.txtPassword.TabIndex = 5; this.txtPassword.UseSystemPasswordChar = true;
        this.chkActive.AutoSize = true; this.chkActive.Location = new System.Drawing.Point(140, 260); this.chkActive.Text = "Set as active server"; this.chkActive.TabIndex = 6;
        this.btnTest = new System.Windows.Forms.Button();
        this.btnTest.Location = new System.Drawing.Point(20, 300);
        this.btnTest.Size = new System.Drawing.Size(110, 32);
        this.btnTest.TabIndex = 8;
        this.btnTest.Text = "Test";
        this.btnTest.UseVisualStyleBackColor = false;
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

        this.btnSave.Location = new System.Drawing.Point(140, 300); this.btnSave.Size = new System.Drawing.Size(110, 32); this.btnSave.TabIndex = 7; this.btnSave.Text = "Save"; this.btnSave.UseVisualStyleBackColor = false;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 360);
        this.Controls.Add(this.lblName); this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblHost); this.Controls.Add(this.txtHost);
        this.Controls.Add(this.lblPort); this.Controls.Add(this.txtPort);
        this.Controls.Add(this.lblDatabase); this.Controls.Add(this.txtDatabase);
        this.Controls.Add(this.lblUser); this.Controls.Add(this.txtUser);
        this.Controls.Add(this.lblPassword); this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.chkActive); this.Controls.Add(this.btnSave); this.Controls.Add(this.btnTest);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "ServerConfigView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Server Configuration";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblHost;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Label lblDatabase;
    private System.Windows.Forms.TextBox txtDatabase;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.TextBox txtUser;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.CheckBox chkActive;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnTest;
}
