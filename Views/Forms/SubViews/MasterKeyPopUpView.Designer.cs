namespace Views.Forms.SubViews;

partial class MasterKeyPopUpView : Form
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
        this.lblPrompt = new System.Windows.Forms.Label();
        this.txtMasterKey = new System.Windows.Forms.TextBox();
        this.lblError = new System.Windows.Forms.Label();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();

        this.lblPrompt.AutoSize = true;
        this.lblPrompt.Location = new System.Drawing.Point(20, 20);
        this.lblPrompt.Text = "Enter the master key to authorize this operation:";

        this.txtMasterKey.Location = new System.Drawing.Point(20, 55);
        this.txtMasterKey.Size = new System.Drawing.Size(340, 27);
        this.txtMasterKey.TabIndex = 0;
        this.txtMasterKey.UseSystemPasswordChar = true;

        this.lblError.AutoSize = true;
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(20, 95);
        this.lblError.TabIndex = 1;
        this.lblError.Text = "";

        this.btnOk.Location = new System.Drawing.Point(140, 130);
        this.btnOk.Size = new System.Drawing.Size(100, 32);
        this.btnOk.TabIndex = 2;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = false;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        this.btnCancel.Location = new System.Drawing.Point(260, 130);
        this.btnCancel.Size = new System.Drawing.Size(100, 32);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = false;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(380, 180);
        this.Controls.Add(this.lblPrompt);
        this.Controls.Add(this.txtMasterKey);
        this.Controls.Add(this.lblError);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "MasterKeyPopUpView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Master Key Required";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblPrompt;
    private System.Windows.Forms.TextBox txtMasterKey;
    private System.Windows.Forms.Label lblError;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
}
