namespace Views.Forms;

partial class LoginView : MaterialSkin.Controls.MaterialForm
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
        this.lblTitle = new MaterialSkin.Controls.MaterialLabel();
        this.lblSubtitle = new MaterialSkin.Controls.MaterialLabel();
        this.picLogo = new System.Windows.Forms.PictureBox();
        this.txtUsername = new MaterialSkin.Controls.MaterialTextBox2();
        this.txtPassword = new MaterialSkin.Controls.MaterialTextBox2();
        this.btnLogin = new MaterialSkin.Controls.MaterialButton();
        this.lblError = new System.Windows.Forms.Label();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();

        this.picLogo.Location = new System.Drawing.Point(60, 78);
        this.picLogo.Name = "picLogo";
        this.picLogo.Size = new System.Drawing.Size(52, 52);
        this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picLogo.TabIndex = 6;
        this.picLogo.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Location = new System.Drawing.Point(124, 86);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "DataSyncEngine";

        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.Location = new System.Drawing.Point(126, 120);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.Text = "Sign in to continue";

        this.txtUsername.Hint = "Username";
        this.txtUsername.Location = new System.Drawing.Point(60, 155);
        this.txtUsername.MaxLength = 32767;
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(300, 48);
        this.txtUsername.TabIndex = 2;

        this.txtPassword.Hint = "Password";
        this.txtPassword.Location = new System.Drawing.Point(60, 215);
        this.txtPassword.MaxLength = 32767;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(300, 48);
        this.txtPassword.TabIndex = 3;
        this.txtPassword.UseSystemPasswordChar = true;

        this.btnLogin.AutoSize = false;
        this.btnLogin.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
        this.btnLogin.HighEmphasis = true;
        this.btnLogin.Location = new System.Drawing.Point(60, 280);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(300, 36);
        this.btnLogin.TabIndex = 4;
        this.btnLogin.Text = "Login";
        this.btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnLogin.UseAccentColor = false;
        this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

        this.lblError.AutoSize = true;
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(60, 325);
        this.lblError.Name = "lblError";
        this.lblError.TabIndex = 5;
        this.lblError.Text = "";

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(420, 370);
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

    private MaterialSkin.Controls.MaterialLabel lblTitle;
    private MaterialSkin.Controls.MaterialLabel lblSubtitle;
    private System.Windows.Forms.PictureBox picLogo;
    private MaterialSkin.Controls.MaterialTextBox2 txtUsername;
    private MaterialSkin.Controls.MaterialTextBox2 txtPassword;
    private MaterialSkin.Controls.MaterialButton btnLogin;
    private System.Windows.Forms.Label lblError;
}
