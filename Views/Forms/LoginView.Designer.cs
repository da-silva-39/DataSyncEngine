namespace Views.Forms;

partial class LoginView : Krypton.Toolkit.KryptonForm
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
        this.picLogo = new System.Windows.Forms.PictureBox();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblSubtitle = new System.Windows.Forms.Label();
        this.txtUsername = new Krypton.Toolkit.KryptonTextBox();
        this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
        this.btnLogin = new Krypton.Toolkit.KryptonButton();
        this.lblError = new System.Windows.Forms.Label();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();

        this.picLogo.Location = new System.Drawing.Point(60, 78);
        this.picLogo.Name = "picLogo";
        this.picLogo.Size = new System.Drawing.Size(96, 96);
        this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picLogo.TabIndex = 6;
        this.picLogo.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
        this.lblTitle.ForeColor = System.Drawing.Color.White;
        this.lblTitle.Location = new System.Drawing.Point(168, 92);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "DataSyncEngine";

        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
        this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(0xB0, 0xB0, 0xB0);
        this.lblSubtitle.Location = new System.Drawing.Point(170, 138);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Sign in to continue";

        this.txtUsername.CueHint.CueHintText = "Username";
        this.txtUsername.Location = new System.Drawing.Point(60, 190);
        this.txtUsername.MaxLength = 32767;
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(360, 50);
        this.txtUsername.TabIndex = 2;

        this.txtPassword.CueHint.CueHintText = "Password";
        this.txtPassword.Location = new System.Drawing.Point(60, 252);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(360, 50);
        this.txtPassword.TabIndex = 3;
        this.txtPassword.UseSystemPasswordChar = true;

        this.btnLogin.AutoSize = false;
        this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.btnLogin.Location = new System.Drawing.Point(60, 320);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(360, 42);
        this.btnLogin.TabIndex = 4;
        this.btnLogin.Text = "LOGIN";
        this.btnLogin.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

        this.lblError.AutoSize = true;
        this.lblError.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(60, 372);
        this.lblError.Name = "lblError";
        this.lblError.TabIndex = 5;
        this.lblError.Text = "";

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(480, 415);
        this.Controls.Add(this.picLogo);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblSubtitle);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.btnLogin);
        this.Controls.Add(this.lblError);
        this.Name = "LoginView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DataSyncEngine - Login";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblSubtitle;
    private Krypton.Toolkit.KryptonTextBox txtUsername;
    private Krypton.Toolkit.KryptonTextBox txtPassword;
    private Krypton.Toolkit.KryptonButton btnLogin;
    private System.Windows.Forms.Label lblError;
}
