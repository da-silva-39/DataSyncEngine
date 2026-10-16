namespace Views.Forms.SubViews;

partial class ShortcutsView : Krypton.Toolkit.KryptonForm
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
        this.lblTitle = new Krypton.Toolkit.KryptonLabel();
        this.lblAction1 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey1 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction2 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey2 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction3 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey3 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction4 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey4 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction5 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey5 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction6 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey6 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction7 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey7 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction8 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey8 = new Krypton.Toolkit.KryptonLabel();
        this.lblAction9 = new Krypton.Toolkit.KryptonLabel();
        this.lblKey9 = new Krypton.Toolkit.KryptonLabel();
        this.btnClose = new Krypton.Toolkit.KryptonButton();
        this.SuspendLayout();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Location = new System.Drawing.Point(28, 24);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Keyboard shortcuts";

        this.lblAction1.AutoSize = true;
        this.lblAction1.Location = new System.Drawing.Point(28, 74);
        this.lblAction1.Name = "lblAction1";
        this.lblAction1.TabIndex = 1;
        this.lblAction1.Text = "Select a folder to scan";

        this.lblKey1.AutoSize = false;
        this.lblKey1.Location = new System.Drawing.Point(300, 70);
        this.lblKey1.Name = "lblKey1";
        this.lblKey1.Size = new System.Drawing.Size(150, 32);
        this.lblKey1.TabIndex = 2;
        this.lblKey1.Text = "Ctrl + O";
        this.lblKey1.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction2.AutoSize = true;
        this.lblAction2.Location = new System.Drawing.Point(28, 114);
        this.lblAction2.Name = "lblAction2";
        this.lblAction2.TabIndex = 3;
        this.lblAction2.Text = "Sync pending files";

        this.lblKey2.AutoSize = false;
        this.lblKey2.Location = new System.Drawing.Point(300, 110);
        this.lblKey2.Name = "lblKey2";
        this.lblKey2.Size = new System.Drawing.Size(150, 32);
        this.lblKey2.TabIndex = 4;
        this.lblKey2.Text = "Ctrl + S";
        this.lblKey2.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction3.AutoSize = true;
        this.lblAction3.Location = new System.Drawing.Point(28, 154);
        this.lblAction3.Name = "lblAction3";
        this.lblAction3.TabIndex = 5;
        this.lblAction3.Text = "Refresh the current tab";

        this.lblKey3.AutoSize = false;
        this.lblKey3.Location = new System.Drawing.Point(300, 150);
        this.lblKey3.Name = "lblKey3";
        this.lblKey3.Size = new System.Drawing.Size(150, 32);
        this.lblKey3.TabIndex = 6;
        this.lblKey3.Text = "Ctrl + R / F5";
        this.lblKey3.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction4.AutoSize = true;
        this.lblAction4.Location = new System.Drawing.Point(28, 194);
        this.lblAction4.Name = "lblAction4";
        this.lblAction4.TabIndex = 7;
        this.lblAction4.Text = "Open the file explorer";

        this.lblKey4.AutoSize = false;
        this.lblKey4.Location = new System.Drawing.Point(300, 190);
        this.lblKey4.Name = "lblKey4";
        this.lblKey4.Size = new System.Drawing.Size(150, 32);
        this.lblKey4.TabIndex = 8;
        this.lblKey4.Text = "Ctrl + E";
        this.lblKey4.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction5.AutoSize = true;
        this.lblAction5.Location = new System.Drawing.Point(28, 234);
        this.lblAction5.Name = "lblAction5";
        this.lblAction5.TabIndex = 9;
        this.lblAction5.Text = "Go to the backup tab";

        this.lblKey5.AutoSize = false;
        this.lblKey5.Location = new System.Drawing.Point(300, 230);
        this.lblKey5.Name = "lblKey5";
        this.lblKey5.Size = new System.Drawing.Size(150, 32);
        this.lblKey5.TabIndex = 10;
        this.lblKey5.Text = "Ctrl + B";
        this.lblKey5.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction6.AutoSize = true;
        this.lblAction6.Location = new System.Drawing.Point(28, 274);
        this.lblAction6.Name = "lblAction6";
        this.lblAction6.TabIndex = 11;
        this.lblAction6.Text = "Go to the dashboard";

        this.lblKey6.AutoSize = false;
        this.lblKey6.Location = new System.Drawing.Point(300, 270);
        this.lblKey6.Name = "lblKey6";
        this.lblKey6.Size = new System.Drawing.Size(150, 32);
        this.lblKey6.TabIndex = 12;
        this.lblKey6.Text = "Ctrl + D";
        this.lblKey6.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction7.AutoSize = true;
        this.lblAction7.Location = new System.Drawing.Point(28, 314);
        this.lblAction7.Name = "lblAction7";
        this.lblAction7.TabIndex = 13;
        this.lblAction7.Text = "Show this shortcut guide";

        this.lblKey7.AutoSize = false;
        this.lblKey7.Location = new System.Drawing.Point(300, 310);
        this.lblKey7.Name = "lblKey7";
        this.lblKey7.Size = new System.Drawing.Size(150, 32);
        this.lblKey7.TabIndex = 14;
        this.lblKey7.Text = "Ctrl + K";
        this.lblKey7.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction8.AutoSize = true;
        this.lblAction8.Location = new System.Drawing.Point(28, 354);
        this.lblAction8.Name = "lblAction8";
        this.lblAction8.TabIndex = 15;
        this.lblAction8.Text = "Open the file metadata viewer";

        this.lblKey8.AutoSize = false;
        this.lblKey8.Location = new System.Drawing.Point(300, 350);
        this.lblKey8.Name = "lblKey8";
        this.lblKey8.Size = new System.Drawing.Size(150, 32);
        this.lblKey8.TabIndex = 16;
        this.lblKey8.Text = "Double-click a file";
        this.lblKey8.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.lblAction9.AutoSize = true;
        this.lblAction9.Location = new System.Drawing.Point(28, 394);
        this.lblAction9.Name = "lblAction9";
        this.lblAction9.TabIndex = 17;
        this.lblAction9.Text = "Close this dialog";

        this.lblKey9.AutoSize = false;
        this.lblKey9.Location = new System.Drawing.Point(300, 390);
        this.lblKey9.Name = "lblKey9";
        this.lblKey9.Size = new System.Drawing.Size(150, 32);
        this.lblKey9.TabIndex = 18;
        this.lblKey9.Text = "Esc";
        this.lblKey9.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;

        this.btnClose.AutoSize = false;
        this.btnClose.Location = new System.Drawing.Point(340, 440);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(110, 36);
        this.btnClose.TabIndex = 19;
        this.btnClose.Text = "Close";
        this.btnClose.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnClose;
        this.ClientSize = new System.Drawing.Size(480, 495);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblAction1);
        this.Controls.Add(this.lblKey1);
        this.Controls.Add(this.lblAction2);
        this.Controls.Add(this.lblKey2);
        this.Controls.Add(this.lblAction3);
        this.Controls.Add(this.lblKey3);
        this.Controls.Add(this.lblAction4);
        this.Controls.Add(this.lblKey4);
        this.Controls.Add(this.lblAction5);
        this.Controls.Add(this.lblKey5);
        this.Controls.Add(this.lblAction6);
        this.Controls.Add(this.lblKey6);
        this.Controls.Add(this.lblAction7);
        this.Controls.Add(this.lblKey7);
        this.Controls.Add(this.lblAction8);
        this.Controls.Add(this.lblKey8);
        this.Controls.Add(this.lblAction9);
        this.Controls.Add(this.lblKey9);
        this.Controls.Add(this.btnClose);
        this.KeyPreview = true;
        this.Name = "ShortcutsView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Keyboard shortcuts";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Krypton.Toolkit.KryptonLabel lblTitle;
    private Krypton.Toolkit.KryptonLabel lblAction1;
    private Krypton.Toolkit.KryptonLabel lblKey1;
    private Krypton.Toolkit.KryptonLabel lblAction2;
    private Krypton.Toolkit.KryptonLabel lblKey2;
    private Krypton.Toolkit.KryptonLabel lblAction3;
    private Krypton.Toolkit.KryptonLabel lblKey3;
    private Krypton.Toolkit.KryptonLabel lblAction4;
    private Krypton.Toolkit.KryptonLabel lblKey4;
    private Krypton.Toolkit.KryptonLabel lblAction5;
    private Krypton.Toolkit.KryptonLabel lblKey5;
    private Krypton.Toolkit.KryptonLabel lblAction6;
    private Krypton.Toolkit.KryptonLabel lblKey6;
    private Krypton.Toolkit.KryptonLabel lblAction7;
    private Krypton.Toolkit.KryptonLabel lblKey7;
    private Krypton.Toolkit.KryptonLabel lblAction8;
    private Krypton.Toolkit.KryptonLabel lblKey8;
    private Krypton.Toolkit.KryptonLabel lblAction9;
    private Krypton.Toolkit.KryptonLabel lblKey9;
    private Krypton.Toolkit.KryptonButton btnClose;
}
