namespace Views.Forms.SubViews;

partial class HelpView : Krypton.Toolkit.KryptonForm
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
        this.tabGuide = new System.Windows.Forms.TabControl();
        this.tabGuidePt = new System.Windows.Forms.TabPage();
        this.txtGuidePt = new System.Windows.Forms.RichTextBox();
        this.tabGuideEn = new System.Windows.Forms.TabPage();
        this.txtGuideEn = new System.Windows.Forms.RichTextBox();
        this.btnClose = new Krypton.Toolkit.KryptonButton();
        this.tabGuide.SuspendLayout();
        this.tabGuidePt.SuspendLayout();
        this.tabGuideEn.SuspendLayout();
        this.SuspendLayout();

        this.tabGuide.Controls.Add(this.tabGuidePt);
        this.tabGuide.Controls.Add(this.tabGuideEn);
        this.tabGuide.Location = new System.Drawing.Point(14, 14);
        this.tabGuide.Name = "tabGuide";
        this.tabGuide.SelectedIndex = 0;
        this.tabGuide.Size = new System.Drawing.Size(812, 520);
        this.tabGuide.TabIndex = 0;

        this.tabGuidePt.Controls.Add(this.txtGuidePt);
        this.tabGuidePt.Location = new System.Drawing.Point(4, 29);
        this.tabGuidePt.Name = "tabGuidePt";
        this.tabGuidePt.Padding = new System.Windows.Forms.Padding(3);
        this.tabGuidePt.Size = new System.Drawing.Size(804, 487);
        this.tabGuidePt.TabIndex = 0;
        this.tabGuidePt.Text = "Português";

        this.txtGuidePt.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.txtGuidePt.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtGuidePt.Name = "txtGuidePt";
        this.txtGuidePt.ReadOnly = true;
        this.txtGuidePt.TabIndex = 0;

        this.tabGuideEn.Controls.Add(this.txtGuideEn);
        this.tabGuideEn.Location = new System.Drawing.Point(4, 29);
        this.tabGuideEn.Name = "tabGuideEn";
        this.tabGuideEn.Padding = new System.Windows.Forms.Padding(3);
        this.tabGuideEn.Size = new System.Drawing.Size(804, 487);
        this.tabGuideEn.TabIndex = 1;
        this.tabGuideEn.Text = "English";

        this.txtGuideEn.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.txtGuideEn.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtGuideEn.Name = "txtGuideEn";
        this.txtGuideEn.ReadOnly = true;
        this.txtGuideEn.TabIndex = 0;

        this.btnClose.AutoSize = false;
        this.btnClose.Location = new System.Drawing.Point(716, 544);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(110, 36);
        this.btnClose.TabIndex = 1;
        this.btnClose.Text = "Close";
        this.btnClose.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnClose;
        this.ClientSize = new System.Drawing.Size(840, 592);
        this.Controls.Add(this.tabGuide);
        this.Controls.Add(this.btnClose);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "HelpView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "User guide";
        this.tabGuide.ResumeLayout(false);
        this.tabGuidePt.ResumeLayout(false);
        this.tabGuideEn.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TabControl tabGuide;
    private System.Windows.Forms.TabPage tabGuidePt;
    private System.Windows.Forms.RichTextBox txtGuidePt;
    private System.Windows.Forms.TabPage tabGuideEn;
    private System.Windows.Forms.RichTextBox txtGuideEn;
    private Krypton.Toolkit.KryptonButton btnClose;
}
