namespace Views.Forms;

partial class LoginView : Form
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
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblUser = new System.Windows.Forms.Label();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.btnLogin = new System.Windows.Forms.Button();
        this.lblError = new System.Windows.Forms.Label();
        this.SuspendLayout();

        this.lblSubtitle = new System.Windows.Forms.Label();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(110, 30);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(180, 37);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "DataSyncEngine";

        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.Location = new System.Drawing.Point(140, 70);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Text = "Sign in to continue";

        this.lblUser.AutoSize = true;
        this.lblUser.Location = new System.Drawing.Point(60, 100);
        this.lblUser.Name = "lblUser";
        this.lblUser.TabIndex = 1;
        this.lblUser.Text = "Username:";

        this.txtUsername.Location = new System.Drawing.Point(160, 97);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(200, 27);
        this.txtUsername.TabIndex = 2;

        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new System.Drawing.Point(60, 145);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.TabIndex = 3;
        this.lblPassword.Text = "Password:";

        this.txtPassword.Location = new System.Drawing.Point(160, 142);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(200, 27);
        this.txtPassword.TabIndex = 4;

        this.btnLogin.Location = new System.Drawing.Point(160, 190);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(120, 32);
        this.btnLogin.TabIndex = 5;
        this.btnLogin.Text = "Login";
        this.btnLogin.UseVisualStyleBackColor = false;
        this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

        this.lblError.AutoSize = true;
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(60, 240);
        this.lblError.Name = "lblError";
        this.lblError.TabIndex = 6;
        this.lblError.Text = "";

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(420, 290);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblSubtitle);
        this.Controls.Add(this.lblUser);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.btnLogin);
        this.Controls.Add(this.lblError);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "LoginView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DataSyncEngine - Login";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Label lblError;
}
