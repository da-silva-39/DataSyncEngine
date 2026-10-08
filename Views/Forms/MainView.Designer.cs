namespace Views.Forms;

partial class MainView : Form
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
        this.tabControl = new System.Windows.Forms.TabControl();
        this.cardFiles = new System.Windows.Forms.Panel();
        this.cardSynced = new System.Windows.Forms.Panel();
        this.cardPending = new System.Windows.Forms.Panel();
        this.cardServers = new System.Windows.Forms.Panel();
        this.lblDashTitle = new System.Windows.Forms.Label();
        this.tabDashboard = new System.Windows.Forms.TabPage();
        this.tabSync = new System.Windows.Forms.TabPage();
        this.tabServers = new System.Windows.Forms.TabPage();
        this.tabAudit = new System.Windows.Forms.TabPage();
        this.tabAnalytics = new System.Windows.Forms.TabPage();
        this.tabUsers = new System.Windows.Forms.TabPage();
        this.btnEditServer = new System.Windows.Forms.Button();
        this.btnSetActive = new System.Windows.Forms.Button();
        this.btnDeleteServer = new System.Windows.Forms.Button();
        this.btnAddUser = new System.Windows.Forms.Button();
        this.btnDeleteUser = new System.Windows.Forms.Button();
        this.gridUsers = new System.Windows.Forms.DataGridView();
        this.btnSelectFolder = new System.Windows.Forms.Button();
        this.btnSync = new System.Windows.Forms.Button();
        this.btnDelete = new System.Windows.Forms.Button();
        this.btnResume = new System.Windows.Forms.Button();
        this.btnLogout = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.gridFiles = new System.Windows.Forms.DataGridView();
        this.notifyIcon = new System.Windows.Forms.NotifyIcon();
        this.components = new System.ComponentModel.Container();
        this.btnAddServer = new System.Windows.Forms.Button();
        this.gridServers = new System.Windows.Forms.DataGridView();
        this.gridAudit = new System.Windows.Forms.DataGridView();
        this.lblAnalytics = new System.Windows.Forms.Label();
        this.tabControl.SuspendLayout();
        this.tabDashboard.SuspendLayout();
        this.tabSync.SuspendLayout();
        this.tabServers.SuspendLayout();
        this.tabAudit.SuspendLayout();
        this.tabAnalytics.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridServers)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridAudit)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
        this.SuspendLayout();

        this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl.Location = new System.Drawing.Point(0, 0);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(980, 620);
        this.tabControl.TabIndex = 0;
        this.tabControl.Controls.Add(this.tabDashboard);
        this.tabControl.Controls.Add(this.tabSync);
        this.tabControl.Controls.Add(this.tabServers);
        this.tabControl.Controls.Add(this.tabAudit);
        this.tabControl.Controls.Add(this.tabAnalytics);
        this.tabControl.Controls.Add(this.tabUsers);

        this.tabDashboard.Controls.Add(this.lblDashTitle);
        this.tabDashboard.Controls.Add(this.cardFiles);
        this.tabDashboard.Controls.Add(this.cardSynced);
        this.tabDashboard.Controls.Add(this.cardPending);
        this.tabDashboard.Controls.Add(this.cardServers);
        this.tabDashboard.Location = new System.Drawing.Point(4, 34);
        this.tabDashboard.Name = "tabDashboard";
        this.tabDashboard.Padding = new System.Windows.Forms.Padding(3);
        this.tabDashboard.Size = new System.Drawing.Size(972, 582);
        this.tabDashboard.TabIndex = 0;
        this.tabDashboard.Text = "Dashboard";

        this.lblDashTitle.AutoSize = true;
        this.lblDashTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
        this.lblDashTitle.Location = new System.Drawing.Point(25, 20);
        this.lblDashTitle.Name = "lblDashTitle";
        this.lblDashTitle.Text = "Overview";

        this.lblFilesCap = new System.Windows.Forms.Label();
        this.lblFilesVal = new System.Windows.Forms.Label();
        this.lblSyncedCap = new System.Windows.Forms.Label();
        this.lblSyncedVal = new System.Windows.Forms.Label();
        this.lblPendingCap = new System.Windows.Forms.Label();
        this.lblPendingVal = new System.Windows.Forms.Label();
        this.lblServersCap = new System.Windows.Forms.Label();
        this.lblServersVal = new System.Windows.Forms.Label();

        this.cardFiles.Location = new System.Drawing.Point(25, 70);
        this.cardFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.cardFiles.Name = "cardFiles";
        this.cardFiles.Size = new System.Drawing.Size(220, 110);
        this.cardFiles.TabIndex = 1;
        this.lblFilesCap.AutoSize = true;
        this.lblFilesCap.Location = new System.Drawing.Point(15, 15);
        this.lblFilesCap.Name = "lblFilesCap";
        this.lblFilesCap.Text = "All files";
        this.lblFilesVal.AutoSize = true;
        this.lblFilesVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
        this.lblFilesVal.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
        this.lblFilesVal.Location = new System.Drawing.Point(15, 50);
        this.lblFilesVal.Name = "lblFilesVal";
        this.lblFilesVal.Text = "0";
        this.cardFiles.Controls.Add(this.lblFilesCap);
        this.cardFiles.Controls.Add(this.lblFilesVal);

        this.cardSynced.Location = new System.Drawing.Point(268, 70);
        this.cardSynced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.cardSynced.Name = "cardSynced";
        this.cardSynced.Size = new System.Drawing.Size(220, 110);
        this.cardSynced.TabIndex = 2;
        this.lblSyncedCap.AutoSize = true;
        this.lblSyncedCap.Location = new System.Drawing.Point(15, 15);
        this.lblSyncedCap.Name = "lblSyncedCap";
        this.lblSyncedCap.Text = "Synced";
        this.lblSyncedVal.AutoSize = true;
        this.lblSyncedVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
        this.lblSyncedVal.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
        this.lblSyncedVal.Location = new System.Drawing.Point(15, 50);
        this.lblSyncedVal.Name = "lblSyncedVal";
        this.lblSyncedVal.Text = "0";
        this.cardSynced.Controls.Add(this.lblSyncedCap);
        this.cardSynced.Controls.Add(this.lblSyncedVal);

        this.cardPending.Location = new System.Drawing.Point(511, 70);
        this.cardPending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.cardPending.Name = "cardPending";
        this.cardPending.Size = new System.Drawing.Size(220, 110);
        this.cardPending.TabIndex = 3;
        this.lblPendingCap.AutoSize = true;
        this.lblPendingCap.Location = new System.Drawing.Point(15, 15);
        this.lblPendingCap.Name = "lblPendingCap";
        this.lblPendingCap.Text = "Pending";
        this.lblPendingVal.AutoSize = true;
        this.lblPendingVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
        this.lblPendingVal.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
        this.lblPendingVal.Location = new System.Drawing.Point(15, 50);
        this.lblPendingVal.Name = "lblPendingVal";
        this.lblPendingVal.Text = "0";
        this.cardPending.Controls.Add(this.lblPendingCap);
        this.cardPending.Controls.Add(this.lblPendingVal);

        this.cardServers.Location = new System.Drawing.Point(754, 70);
        this.cardServers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.cardServers.Name = "cardServers";
        this.cardServers.Size = new System.Drawing.Size(220, 110);
        this.cardServers.TabIndex = 4;
        this.lblServersCap.AutoSize = true;
        this.lblServersCap.Location = new System.Drawing.Point(15, 15);
        this.lblServersCap.Name = "lblServersCap";
        this.lblServersCap.Text = "Servers";
        this.lblServersVal.AutoSize = true;
        this.lblServersVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
        this.lblServersVal.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
        this.lblServersVal.Location = new System.Drawing.Point(15, 50);
        this.lblServersVal.Name = "lblServersVal";
        this.lblServersVal.Text = "0";
        this.cardServers.Controls.Add(this.lblServersCap);
        this.cardServers.Controls.Add(this.lblServersVal);

        this.tabSync.Controls.Add(this.gridFiles);
        this.tabSync.Controls.Add(this.lblStatus);
        this.tabSync.Controls.Add(this.btnSelectFolder);
        this.tabSync.Controls.Add(this.btnSync);
        this.tabSync.Controls.Add(this.btnDelete);
        this.tabSync.Controls.Add(this.btnResume);
        this.tabSync.Controls.Add(this.btnExplorer);
        this.tabSync.Controls.Add(this.btnLogout);
        this.tabSync.Location = new System.Drawing.Point(4, 34);
        this.tabSync.Name = "tabSync";
        this.tabSync.Padding = new System.Windows.Forms.Padding(3);
        this.tabSync.Size = new System.Drawing.Size(972, 582);
        this.tabSync.TabIndex = 0;
        this.tabSync.Text = "Sync";

        this.btnSelectFolder.Location = new System.Drawing.Point(15, 15);
        this.btnSelectFolder.Name = "btnSelectFolder";
        this.btnSelectFolder.Size = new System.Drawing.Size(150, 32);
        this.btnSelectFolder.TabIndex = 0;
        this.btnSelectFolder.Text = "Select Folder";
        this.btnSelectFolder.UseVisualStyleBackColor = false;

        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(510, 62);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.TabIndex = 1;
        this.lblStatus.Text = "Ready.";

        this.btnSync.Location = new System.Drawing.Point(180, 15);
        this.btnSync.Name = "btnSync";
        this.btnSync.Size = new System.Drawing.Size(110, 32);
        this.btnSync.TabIndex = 3;
        this.btnSync.Text = "Sync";
        this.btnSync.UseVisualStyleBackColor = false;

        this.btnDelete.Location = new System.Drawing.Point(305, 15);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new System.Drawing.Size(110, 32);
        this.btnDelete.TabIndex = 4;
        this.btnDelete.Text = "Delete";
        this.btnDelete.UseVisualStyleBackColor = false;

        this.btnResume.Location = new System.Drawing.Point(430, 15);
        this.btnResume.Name = "btnResume";
        this.btnResume.Size = new System.Drawing.Size(110, 32);
        this.btnResume.TabIndex = 5;
        this.btnResume.Text = "Resume";
        this.btnResume.UseVisualStyleBackColor = false;

        this.btnExplorer = new System.Windows.Forms.Button();
        this.btnExplorer.Location = new System.Drawing.Point(555, 15);
        this.btnExplorer.Name = "btnExplorer";
        this.btnExplorer.Size = new System.Drawing.Size(110, 32);
        this.btnExplorer.TabIndex = 9;
        this.btnExplorer.Text = "Explorer";
        this.btnExplorer.UseVisualStyleBackColor = false;

        this.btnLogout.Location = new System.Drawing.Point(850, 15);
        this.btnLogout.Name = "btnLogout";
        this.btnLogout.Size = new System.Drawing.Size(110, 32);
        this.btnLogout.TabIndex = 6;
        this.btnLogout.Text = "Logout";
        this.btnLogout.UseVisualStyleBackColor = false;

        this.notifyIcon.Icon = System.Drawing.SystemIcons.Application;
        this.notifyIcon.Text = "DataSyncEngine";
        this.notifyIcon.Visible = true;

        this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.trayShow = new System.Windows.Forms.ToolStripMenuItem();
        this.trayExit = new System.Windows.Forms.ToolStripMenuItem();
        this.trayMenu.Items.Add(this.trayShow);
        this.trayMenu.Items.Add(this.trayExit);
        this.trayShow.Text = "Show";
        this.trayExit.Text = "Exit";
        this.trayShow.Click += new System.EventHandler(this.trayShow_Click);
        this.trayExit.Click += new System.EventHandler(this.trayExit_Click);
        this.notifyIcon.ContextMenuStrip = this.trayMenu;
        this.notifyIcon.DoubleClick += new System.EventHandler(this.trayShow_Click);

        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.lblServerInfo = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusStrip.Items.Add(this.lblServerInfo);
        this.statusStrip.Location = new System.Drawing.Point(0, 597);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(980, 22);
        this.statusStrip.TabIndex = 1;
        this.lblServerInfo.Name = "lblServerInfo";
        this.lblServerInfo.Text = "";

        this.btnTheme = new System.Windows.Forms.ToolStripDropDownButton();
        this.themeDarkItem = new System.Windows.Forms.ToolStripMenuItem();
        this.themeLightItem = new System.Windows.Forms.ToolStripMenuItem();
        this.btnTheme.Text = "Theme";
        this.themeDarkItem.Text = "Dark";
        this.themeLightItem.Text = "Light";
        this.btnTheme.DropDownItems.Add(this.themeDarkItem);
        this.btnTheme.DropDownItems.Add(this.themeLightItem);
        this.themeDarkItem.Click += new System.EventHandler(this.themeDarkItem_Click);
        this.themeLightItem.Click += new System.EventHandler(this.themeLightItem_Click);
        this.statusStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        this.statusStrip.Items.Add(this.btnTheme);

        this.txtFilter = new System.Windows.Forms.TextBox();
        this.txtFilter.Location = new System.Drawing.Point(15, 58);
        this.txtFilter.Name = "txtFilter";
        this.txtFilter.Size = new System.Drawing.Size(250, 27);
        this.txtFilter.TabIndex = 7;
        this.txtFilter.PlaceholderText = "Type to filter files...";
        this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
        this.tabSync.Controls.Add(this.txtFilter);

        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.progressBar.Location = new System.Drawing.Point(280, 61);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(210, 20);
        this.progressBar.TabIndex = 8;
        this.tabSync.Controls.Add(this.progressBar);

        this.gridFiles.Location = new System.Drawing.Point(15, 95);
        this.gridFiles.Size = new System.Drawing.Size(940, 465);

        this.Controls.Add(this.statusStrip);

        this.gridFiles.AllowUserToAddRows = false;
        this.gridFiles.AllowUserToDeleteRows = false;
        this.gridFiles.ReadOnly = true;
        this.gridFiles.Name = "gridFiles";
        this.gridFiles.TabIndex = 2;

        this.tabServers.Controls.Add(this.gridServers);
        this.tabServers.Controls.Add(this.btnAddServer);
        this.tabServers.Location = new System.Drawing.Point(4, 34);
        this.tabServers.Name = "tabServers";
        this.tabServers.Size = new System.Drawing.Size(972, 582);
        this.tabServers.TabIndex = 1;
        this.tabServers.Text = "Servers";

        this.btnAddServer.Location = new System.Drawing.Point(15, 15);
        this.btnAddServer.Name = "btnAddServer";
        this.btnAddServer.Size = new System.Drawing.Size(150, 32);
        this.btnAddServer.TabIndex = 0;
        this.btnAddServer.Text = "Add Server";
        this.btnAddServer.UseVisualStyleBackColor = false;

        this.btnAddServer.UseVisualStyleBackColor = false;

        this.btnEditServer.Location = new System.Drawing.Point(180, 15);
        this.btnEditServer.Name = "btnEditServer";
        this.btnEditServer.Size = new System.Drawing.Size(140, 32);
        this.btnEditServer.TabIndex = 2;
        this.btnEditServer.Text = "Edit Server";
        this.btnEditServer.UseVisualStyleBackColor = false;

        this.btnSetActive.Location = new System.Drawing.Point(335, 15);
        this.btnSetActive.Name = "btnSetActive";
        this.btnSetActive.Size = new System.Drawing.Size(140, 32);
        this.btnSetActive.TabIndex = 3;
        this.btnSetActive.Text = "Set Active";
        this.btnSetActive.UseVisualStyleBackColor = false;

        this.btnDeleteServer.Location = new System.Drawing.Point(490, 15);
        this.btnDeleteServer.Name = "btnDeleteServer";
        this.btnDeleteServer.Size = new System.Drawing.Size(140, 32);
        this.btnDeleteServer.TabIndex = 4;
        this.btnDeleteServer.Text = "Delete Server";
        this.btnDeleteServer.UseVisualStyleBackColor = false;

        this.tabServers.Controls.Add(this.btnEditServer);
        this.tabServers.Controls.Add(this.btnSetActive);
        this.tabServers.Controls.Add(this.btnDeleteServer);

        this.gridServers.AllowUserToAddRows = false;
        this.gridServers.AllowUserToDeleteRows = false;
        this.gridServers.ReadOnly = true;
        this.gridServers.Location = new System.Drawing.Point(15, 60);
        this.gridServers.Name = "gridServers";
        this.gridServers.Size = new System.Drawing.Size(940, 500);
        this.gridServers.TabIndex = 1;

        this.tabAudit.Controls.Add(this.gridAudit);
        this.tabAudit.Location = new System.Drawing.Point(4, 34);
        this.tabAudit.Name = "tabAudit";
        this.tabAudit.Size = new System.Drawing.Size(972, 582);
        this.tabAudit.TabIndex = 2;
        this.tabAudit.Text = "Audit";

        this.gridAudit.AllowUserToAddRows = false;
        this.gridAudit.AllowUserToDeleteRows = false;
        this.gridAudit.ReadOnly = true;
        this.gridAudit.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridAudit.Name = "gridAudit";
        this.gridAudit.TabIndex = 0;

        this.tabAnalytics.Controls.Add(this.lblAnalytics);
        this.tabAnalytics.Location = new System.Drawing.Point(4, 34);
        this.tabAnalytics.Name = "tabAnalytics";
        this.tabAnalytics.Size = new System.Drawing.Size(972, 582);
        this.tabAnalytics.TabIndex = 3;
        this.tabAnalytics.Text = "Analytics";

        this.lblAnalytics.AutoSize = true;
        this.lblAnalytics.Location = new System.Drawing.Point(20, 20);
        this.lblAnalytics.Name = "lblAnalytics";
        this.lblAnalytics.Text = "Analytics dashboard (Admin only).";

        this.lblAnaTotal = new System.Windows.Forms.Label();
        this.lblAnaSynced = new System.Windows.Forms.Label();
        this.lblAnaPending = new System.Windows.Forms.Label();
        this.lblAnaModified = new System.Windows.Forms.Label();
        this.lblAnaBytes = new System.Windows.Forms.Label();
        this.lblAnaTotal.AutoSize = true;
        this.lblAnaTotal.Location = new System.Drawing.Point(20, 60);
        this.lblAnaTotal.Name = "lblAnaTotal";
        this.lblAnaTotal.Text = "Total files: 0";
        this.lblAnaSynced.AutoSize = true;
        this.lblAnaSynced.Location = new System.Drawing.Point(20, 90);
        this.lblAnaSynced.Name = "lblAnaSynced";
        this.lblAnaSynced.Text = "Synced: 0";
        this.lblAnaPending.AutoSize = true;
        this.lblAnaPending.Location = new System.Drawing.Point(20, 120);
        this.lblAnaPending.Name = "lblAnaPending";
        this.lblAnaPending.Text = "Pending: 0";
        this.lblAnaModified.AutoSize = true;
        this.lblAnaModified.Location = new System.Drawing.Point(20, 150);
        this.lblAnaModified.Name = "lblAnaModified";
        this.lblAnaModified.Text = "Modified: 0";
        this.lblAnaBytes.AutoSize = true;
        this.lblAnaBytes.Location = new System.Drawing.Point(20, 180);
        this.lblAnaBytes.Name = "lblAnaBytes";
        this.lblAnaBytes.Text = "Total size: 0 bytes";
        this.tabAnalytics.Controls.Add(this.lblAnaTotal);
        this.tabAnalytics.Controls.Add(this.lblAnaSynced);
        this.tabAnalytics.Controls.Add(this.lblAnaPending);
        this.tabAnalytics.Controls.Add(this.lblAnaModified);
        this.tabAnalytics.Controls.Add(this.lblAnaBytes);

        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(980, 620);
        this.Controls.Add(this.tabControl);
        this.Name = "MainView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DataSyncEngine";
        this.tabControl.ResumeLayout(false);
        this.tabSync.ResumeLayout(false);
        this.tabSync.PerformLayout();
        this.tabServers.ResumeLayout(false);
        this.tabServers.PerformLayout();
        this.tabAudit.ResumeLayout(false);
        this.tabAnalytics.ResumeLayout(false);
        this.tabAnalytics.PerformLayout();

        this.tabUsers.SuspendLayout();
        this.tabUsers.Controls.Add(this.gridUsers);
        this.tabUsers.Controls.Add(this.btnAddUser);
        this.tabUsers.Controls.Add(this.btnDeleteUser);
        this.tabUsers.Location = new System.Drawing.Point(4, 34);
        this.tabUsers.Name = "tabUsers";
        this.tabUsers.Size = new System.Drawing.Size(972, 582);
        this.tabUsers.TabIndex = 4;
        this.tabUsers.Text = "Users";

        this.btnAddUser.Location = new System.Drawing.Point(15, 15);
        this.btnAddUser.Name = "btnAddUser";
        this.btnAddUser.Size = new System.Drawing.Size(140, 32);
        this.btnAddUser.TabIndex = 0;
        this.btnAddUser.Text = "Add User";
        this.btnAddUser.UseVisualStyleBackColor = false;

        this.btnDeleteUser.Location = new System.Drawing.Point(170, 15);
        this.btnDeleteUser.Name = "btnDeleteUser";
        this.btnDeleteUser.Size = new System.Drawing.Size(140, 32);
        this.btnDeleteUser.TabIndex = 1;
        this.btnDeleteUser.Text = "Delete User";
        this.btnDeleteUser.UseVisualStyleBackColor = false;

        this.gridUsers.AllowUserToAddRows = false;
        this.gridUsers.AllowUserToDeleteRows = false;
        this.gridUsers.ReadOnly = true;
        this.gridUsers.Location = new System.Drawing.Point(15, 60);
        this.gridUsers.Name = "gridUsers";
        this.gridUsers.Size = new System.Drawing.Size(940, 500);
        this.gridUsers.TabIndex = 2;
        this.tabUsers.ResumeLayout(false);

        ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridServers)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gridAudit)).EndInit();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabDashboard;
    private System.Windows.Forms.TabPage tabSync;
    private System.Windows.Forms.Panel cardFiles;
    private System.Windows.Forms.Label lblFilesCap;
    private System.Windows.Forms.Label lblFilesVal;
    private System.Windows.Forms.Panel cardSynced;
    private System.Windows.Forms.Label lblSyncedCap;
    private System.Windows.Forms.Label lblSyncedVal;
    private System.Windows.Forms.Panel cardPending;
    private System.Windows.Forms.Label lblPendingCap;
    private System.Windows.Forms.Label lblPendingVal;
    private System.Windows.Forms.Panel cardServers;
    private System.Windows.Forms.Label lblServersCap;
    private System.Windows.Forms.Label lblServersVal;
    private System.Windows.Forms.Label lblDashTitle;
    private System.Windows.Forms.TabPage tabServers;
    private System.Windows.Forms.TabPage tabAudit;
    private System.Windows.Forms.TabPage tabAnalytics;
    private System.Windows.Forms.Button btnSelectFolder;
    private System.Windows.Forms.Button btnSync;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnResume;
    private System.Windows.Forms.Button btnExplorer;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.Button btnAddUser;
    private System.Windows.Forms.Button btnDeleteUser;
    private System.Windows.Forms.DataGridView gridUsers;
    private System.Windows.Forms.TabPage tabUsers;
    private System.Windows.Forms.Button btnEditServer;
    private System.Windows.Forms.Button btnSetActive;
    private System.Windows.Forms.Button btnDeleteServer;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblServerInfo;
    private System.Windows.Forms.ToolStripDropDownButton btnTheme;
    private System.Windows.Forms.ToolStripMenuItem themeDarkItem;
    private System.Windows.Forms.ToolStripMenuItem themeLightItem;
    private System.Windows.Forms.TextBox txtFilter;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.ContextMenuStrip trayMenu;
    private System.Windows.Forms.ToolStripMenuItem trayShow;
    private System.Windows.Forms.ToolStripMenuItem trayExit;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.DataGridView gridFiles;
    private System.Windows.Forms.Button btnAddServer;
    private System.Windows.Forms.DataGridView gridServers;
    private System.Windows.Forms.DataGridView gridAudit;
    private System.Windows.Forms.Label lblAnalytics;
    private System.Windows.Forms.Label lblAnaTotal;
    private System.Windows.Forms.Label lblAnaSynced;
    private System.Windows.Forms.Label lblAnaPending;
    private System.Windows.Forms.Label lblAnaModified;
    private System.Windows.Forms.Label lblAnaBytes;
}
