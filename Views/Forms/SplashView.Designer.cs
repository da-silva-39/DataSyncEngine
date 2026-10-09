namespace Views.Forms;

partial class SplashView : Form
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
        this.lblApp = new System.Windows.Forms.Label();
        this.loader = new Views.Components.DotsLoader();
        this.lblPercent = new System.Windows.Forms.Label();
        this.loadTimer = new System.Windows.Forms.Timer();
        this.components = new System.ComponentModel.Container();
        this.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();

        this.picLogo.Location = new System.Drawing.Point(196, 36);
        this.picLogo.Name = "picLogo";
        this.picLogo.Size = new System.Drawing.Size(128, 128);
        this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picLogo.TabIndex = 0;
        this.picLogo.TabStop = false;
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();

        this.lblApp.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
        this.lblApp.ForeColor = System.Drawing.Color.White;
        this.lblApp.Location = new System.Drawing.Point(0, 176);
        this.lblApp.Name = "lblApp";
        this.lblApp.Size = new System.Drawing.Size(520, 48);
        this.lblApp.TabIndex = 1;
        this.lblApp.Text = "DataSyncEngine";
        this.lblApp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        this.loader.Location = new System.Drawing.Point(60, 240);
        this.loader.Name = "loader";
        this.loader.Size = new System.Drawing.Size(400, 60);
        this.loader.TabIndex = 2;

        this.lblPercent.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
        this.lblPercent.ForeColor = System.Drawing.Color.FromArgb(0x40, 0xC4, 0xFF);
        this.lblPercent.Location = new System.Drawing.Point(0, 310);
        this.lblPercent.Name = "lblPercent";
        this.lblPercent.Size = new System.Drawing.Size(520, 30);
        this.lblPercent.TabIndex = 3;
        this.lblPercent.Text = "0%";
        this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        this.loadTimer.Interval = 50;
        this.loadTimer.Tick += new System.EventHandler(this.loadTimer_Tick);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Black;
        this.ClientSize = new System.Drawing.Size(520, 370);
        this.Controls.Add(this.picLogo);
        this.Controls.Add(this.lblApp);
        this.Controls.Add(this.loader);
        this.Controls.Add(this.lblPercent);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "SplashView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DataSyncEngine";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblApp;
    private Views.Components.DotsLoader loader;
    private System.Windows.Forms.Label lblPercent;
    private System.Windows.Forms.Timer loadTimer;
}
