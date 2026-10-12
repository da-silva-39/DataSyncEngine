using Krypton.Toolkit;
using Views.Components;

namespace Views.Forms;

/// <summary>
/// GitHub-web inspired chrome for the main window: sidebar, content header,
/// cards, navigation state and full-width data surfaces.
/// </summary>
public partial class MainView
{
    private void WireShell()
    {
        HideTabHeaders();
        tabControl.SelectedIndexChanged += (_, _) => UpdateNavSelection();
        ClientSizeChanged += (_, _) => LayoutContent();
        KryptonThemeModule.ThemeChanged += OnThemeChanged;
        ApplyShellTheme();
        LayoutContent();
    }

    private void OnThemeChanged() => ApplyShellTheme();

    private void UnwireShell() => KryptonThemeModule.ThemeChanged -= OnThemeChanged;

    private void ApplyShellTheme()
    {        GitHubThemeModule.StyleSurface(sideBar, DarkThemeModule.SidebarColor, right: true);
        GitHubThemeModule.StyleSurface(sideStrip, DarkThemeModule.SidebarColor, right: true);
        GitHubThemeModule.StyleSurface(sideFooter, DarkThemeModule.SidebarColor, top: true);
        GitHubThemeModule.StyleSurface(topSpacer, DarkThemeModule.HeaderColor, bottom: true);

        GitHubThemeModule.StyleCard(cardFiles);
        GitHubThemeModule.StyleCard(cardSynced);
        GitHubThemeModule.StyleCard(cardPending);
        GitHubThemeModule.StyleCard(cardServers);

        GitHubThemeModule.StyleHeading(lblSideTitle, DarkThemeModule.TextColor, 12F, FontStyle.Bold, "Segoe UI Semibold");
        GitHubThemeModule.StyleHeading(lblNavCaption, DarkThemeModule.MutedTextColor, 7.5F, FontStyle.Bold);
        GitHubThemeModule.StyleHeading(lblTreeCap, DarkThemeModule.MutedTextColor, 7.5F, FontStyle.Bold);
        GitHubThemeModule.StyleHeading(lblPageTitle, DarkThemeModule.TextColor, 13F, FontStyle.Bold, "Segoe UI Semibold");
        GitHubThemeModule.StyleHeading(lblPageSub, DarkThemeModule.MutedTextColor, 9F);
        GitHubThemeModule.StyleHeading(lblUserBadge, DarkThemeModule.TextColor, 9F);
        lblServerInfo.ForeColor = DarkThemeModule.MutedTextColor;
        GitHubThemeModule.StyleHeading(lblFilesCap, DarkThemeModule.MutedTextColor, 9F);
        GitHubThemeModule.StyleHeading(lblSyncedCap, DarkThemeModule.MutedTextColor, 9F);
        GitHubThemeModule.StyleHeading(lblPendingCap, DarkThemeModule.MutedTextColor, 9F);
        GitHubThemeModule.StyleHeading(lblServersCap, DarkThemeModule.MutedTextColor, 9F);

        lblDashTitle.Visible = false;

        GitHubThemeModule.StyleTree(treeFiles);
        GitHubThemeModule.StyleGrid(gridFiles);
        GitHubThemeModule.StyleGrid(gridServers);
        GitHubThemeModule.StyleGrid(gridAudit);
        GitHubThemeModule.StyleGrid(gridUsers);
        GitHubThemeModule.StyleGrid(gridTrash);

        lblFilesVal.ForeColor = DarkThemeModule.AccentColor;
        lblSyncedVal.ForeColor = DarkThemeModule.SuccessColor;
        lblPendingVal.ForeColor = DarkThemeModule.WarningColor;
        lblServersVal.ForeColor = DarkThemeModule.AccentColor;

        BackColor = DarkThemeModule.CanvasColor;

        UpdateNavSelection();
    }

    private void HideTabHeaders()
    {
        tabControl.Appearance = TabAppearance.FlatButtons;
        tabControl.SizeMode = TabSizeMode.Fixed;
        tabControl.ItemSize = new Size(0, 1);
        tabControl.Padding = new Point(0, 0);
        tabControl.Multiline = false;
    }

    private void LayoutContent()
    {
        FillGrid(gridFiles, tabSync, 95);
        FillGrid(gridServers, tabServers, 60);
        FillGrid(gridAudit, tabAudit, 60);
        FillGrid(gridUsers, tabUsers, 60);
        FillGrid(gridTrash, tabTrash, 60);
    }

    private static void FillGrid(DataGridView grid, Control parent, int top)
    {
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        int width = Math.Max(240, parent.ClientSize.Width - 30);
        int height = Math.Max(120, parent.ClientSize.Height - top - 15);
        grid.Location = new Point(15, top);
        grid.Size = new Size(width, height);
    }

    private void UpdateNavSelection()
    {
        var tab = tabControl.SelectedTab;
        GitHubThemeModule.StyleNav(navDashboard, tab == tabDashboard);
        GitHubThemeModule.StyleNav(navSync, tab == tabSync);
        GitHubThemeModule.StyleNav(navBackup, tab == tabBackup);
        GitHubThemeModule.StyleNav(navTrash, tab == tabTrash);
        GitHubThemeModule.StyleNav(navServers, tab == tabServers);
        GitHubThemeModule.StyleNav(navAudit, tab == tabAudit);
        GitHubThemeModule.StyleNav(navAnalytics, tab == tabAnalytics);
        GitHubThemeModule.StyleNav(navUsers, tab == tabUsers);
        GitHubThemeModule.StyleNav(navSettings, tab == tabSettings);

        (lblPageTitle.Text, lblPageSub.Text) = tab == tabDashboard ? ("Dashboard", "Synchronization workspace overview")
            : tab == tabSync ? ("Sync", "Scan a local folder and upload changed files")
            : tab == tabBackup ? ("Backup", "Export decrypted files from the server")
            : tab == tabTrash ? ("Trash", "Restore or permanently remove deleted files")
            : tab == tabServers ? ("Servers", "Manage database connections")
            : tab == tabAudit ? ("Audit log", "Traceability of every user action")
            : tab == tabAnalytics ? ("Analytics", "Aggregated synchronization metrics")
            : tab == tabUsers ? ("Users", "Accounts and access control")
            : ("Settings", "Appearance, behaviour and automation");
    }
}
