namespace Views.Forms.SubViews;

partial class MasterKeyPopUpView : MaterialSkin.Controls.MaterialForm
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
        this.lblPrompt = new MaterialSkin.Controls.MaterialLabel();
        this.txtMasterKey = new MaterialSkin.Controls.MaterialTextBox2();
        this.lblError = new System.Windows.Forms.Label();
        this.btnOk = new MaterialSkin.Controls.MaterialButton();
        this.btnCancel = new MaterialSkin.Controls.MaterialButton();
        this.SuspendLayout();

        this.lblPrompt.AutoSize = true;
        this.lblPrompt.Location = new System.Drawing.Point(20, 80);
        this.lblPrompt.Name = "lblPrompt";
        this.lblPrompt.TabIndex = 0;
        this.lblPrompt.Text = "Enter the master key to authorize this operation:";

        this.txtMasterKey.Hint = "Master key";
        this.txtMasterKey.Location = new System.Drawing.Point(20, 115);
        this.txtMasterKey.MaxLength = 32767;
        this.txtMasterKey.Name = "txtMasterKey";
        this.txtMasterKey.Size = new System.Drawing.Size(340, 48);
        this.txtMasterKey.TabIndex = 1;
        this.txtMasterKey.UseSystemPasswordChar = true;

        this.lblError.AutoSize = true;
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(20, 175);
        this.lblError.Name = "lblError";
        this.lblError.TabIndex = 2;
        this.lblError.Text = "";

        this.btnOk.AutoSize = false;
        this.btnOk.HighEmphasis = true;
        this.btnOk.Location = new System.Drawing.Point(140, 205);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(100, 36);
        this.btnOk.TabIndex = 3;
        this.btnOk.Text = "OK";
        this.btnOk.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        this.btnOk.UseAccentColor = false;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        this.btnCancel.AutoSize = false;
        this.btnCancel.HighEmphasis = false;
        this.btnCancel.Location = new System.Drawing.Point(260, 205);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(100, 36);
        this.btnCancel.TabIndex = 4;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
        this.btnCancel.UseAccentColor = false;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(380, 260);
        this.Controls.Add(this.lblPrompt);
        this.Controls.Add(this.txtMasterKey);
        this.Controls.Add(this.lblError);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.Name = "MasterKeyPopUpView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Master Key Required";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private MaterialSkin.Controls.MaterialLabel lblPrompt;
    private MaterialSkin.Controls.MaterialTextBox2 txtMasterKey;
    private System.Windows.Forms.Label lblError;
    private MaterialSkin.Controls.MaterialButton btnOk;
    private MaterialSkin.Controls.MaterialButton btnCancel;
}
