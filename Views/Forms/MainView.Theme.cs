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
        tabControl.SelectedIndexChanged += (_, _) => UpdateNavSelection();
        KryptonThemeModule.ThemeChanged += OnThemeChanged;
        ApplyShellTheme();
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

        lblDashHint.ForeColor = DarkThemeModule.MutedTextColor;

        GitHubThemeModule.StyleTree(treeFiles);
        GitHubThemeModule.StyleGrid(gridFiles);
        GitHubThemeModule.StyleGrid(gridServers);
        GitHubThemeModule.StyleGrid(gridAudit);
        GitHubThemeModule.StyleGrid(gridUsers);
        GitHubThemeModule.StyleGrid(gridTrash);
        GitHubThemeModule.StyleGrid(gridDashAudit);

        lblFilesVal.ForeColor = DarkThemeModule.AccentColor;
        lblSyncedVal.ForeColor = DarkThemeModule.SuccessColor;
        lblPendingVal.ForeColor = DarkThemeModule.WarningColor;
        lblServersVal.ForeColor = DarkThemeModule.AccentColor;

        BackColor = DarkThemeModule.CanvasColor;

        ApplyButtonSemantics();
        UpdateNavSelection();
    }

    private void ApplyButtonSemantics()
    {
        bool dark = DarkThemeModule.IsDark;
        Color success = ColorTranslator.FromHtml(dark ? "#238636" : "#1F883D");
        Color info = ColorTranslator.FromHtml(dark ? "#1F6FEB" : "#0969DA");
        Color danger = ColorTranslator.FromHtml(dark ? "#DA3633" : "#CF222E");
        Color warning = ColorTranslator.FromHtml(dark ? "#9E6A03" : "#BF8700");

        StyleAction(btnSync, success, dark);
        StyleAction(btnDashSync, success, dark);
        StyleAction(btnBackupNow, success, dark);
        StyleAction(btnDashBackup, success, dark);

        StyleAction(btnSelectFolder, info, dark);
        StyleAction(btnDashScan, info, dark);
        StyleAction(btnResume, info, dark);
        StyleAction(btnExplorer, info, dark);
        StyleAction(btnDashExplorer, info, dark);
        StyleAction(btnDashShortcuts, info, dark);
        StyleAction(btnDashDocs, info, dark);

        StyleAction(btnDelete, danger, dark);
        StyleAction(btnPurge, danger, dark);
        StyleAction(btnDeleteServer, danger, dark);
        StyleAction(btnDeleteUser, danger, dark);

        StyleAction(btnResetSettings, warning, dark);
    }

    private static void StyleAction(KryptonButton button, Color color, bool dark)
        => GitHubThemeModule.StyleAction(button, color, Color.White, dark ? ControlPaint.Light(color, 0.18f) : ControlPaint.Dark(color, 0.08f));

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

        (lblPageTitle.Text, lblPageSub.Text) = tab == tabDashboard ? (T("pageDashboard"), T("subDashboard"))
            : tab == tabSync ? (T("pageSync"), T("subSync"))
            : tab == tabBackup ? (T("pageBackup"), T("subBackup"))
            : tab == tabTrash ? (T("pageTrash"), T("subTrash"))
            : tab == tabServers ? (T("pageServers"), T("subServers"))
            : tab == tabAudit ? (T("pageAudit"), T("subAudit"))
            : tab == tabAnalytics ? (T("pageAnalytics"), T("subAnalytics"))
            : tab == tabUsers ? (T("pageUsers"), T("subUsers"))
            : (T("pageSettings"), T("subSettings"));
    }

    private static string T(string control) => LocalizationModule.T("MainView/" + control);
}
