namespace Views.Forms.SubViews;

partial class ChangePasswordView : Krypton.Toolkit.KryptonForm
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
        this.txtCurrent = new Krypton.Toolkit.KryptonTextBox();
        this.txtNew = new Krypton.Toolkit.KryptonTextBox();
        this.txtConfirm = new Krypton.Toolkit.KryptonTextBox();
        this.btnSave = new Krypton.Toolkit.KryptonButton();
        this.lblError = new System.Windows.Forms.Label();
        this.SuspendLayout();

        this.txtCurrent.CueHint.CueHintText = "Current password";
        this.txtCurrent.Location = new System.Drawing.Point(20, 80);
        this.txtCurrent.MaxLength = 32767;
        this.txtCurrent.Name = "txtCurrent";
        this.txtCurrent.Size = new System.Drawing.Size(340, 36);
        this.txtCurrent.TabIndex = 0;
        this.txtCurrent.UseSystemPasswordChar = true;

        this.txtNew.CueHint.CueHintText = "New password (min 6 chars)";
        this.txtNew.Location = new System.Drawing.Point(20, 130);
        this.txtNew.MaxLength = 32767;
        this.txtNew.Name = "txtNew";
        this.txtNew.Size = new System.Drawing.Size(340, 36);
        this.txtNew.TabIndex = 1;
        this.txtNew.UseSystemPasswordChar = true;

        this.txtConfirm.CueHint.CueHintText = "Confirm new password";
        this.txtConfirm.Location = new System.Drawing.Point(20, 180);
        this.txtConfirm.MaxLength = 32767;
        this.txtConfirm.Name = "txtConfirm";
        this.txtConfirm.Size = new System.Drawing.Size(340, 36);
        this.txtConfirm.TabIndex = 2;
        this.txtConfirm.UseSystemPasswordChar = true;

        this.lblError.AutoSize = true;
        this.lblError.ForeColor = System.Drawing.Color.OrangeRed;
        this.lblError.Location = new System.Drawing.Point(20, 225);
        this.lblError.Name = "lblError";
        this.lblError.TabIndex = 4;
        this.lblError.Text = "";

        this.btnSave.Location = new System.Drawing.Point(20, 255);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(340, 36);
        this.btnSave.TabIndex = 3;
        this.btnSave.Text = "Change Password";
        this.btnSave.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(380, 315);
        this.Controls.Add(this.txtCurrent);
        this.Controls.Add(this.txtNew);
        this.Controls.Add(this.txtConfirm);
        this.Controls.Add(this.lblError);
        this.Controls.Add(this.btnSave);
        this.Name = "ChangePasswordView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Change Password";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Krypton.Toolkit.KryptonTextBox txtCurrent;
    private Krypton.Toolkit.KryptonTextBox txtNew;
    private Krypton.Toolkit.KryptonTextBox txtConfirm;
    private Krypton.Toolkit.KryptonButton btnSave;
    private System.Windows.Forms.Label lblError;
}
