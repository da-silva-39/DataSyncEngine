using System.Windows.Forms;

namespace Views.Components;

/// <summary>
/// Lightweight bilingual UI layer (English / Português). Texts live in a flat
/// table keyed by "FormName/ControlName" so the designer files keep readable
/// English defaults while the runtime swaps them for the selected language.
/// <see cref="Apply(Form)"/> walks every control of a form (including tab pages,
/// tool strips and context menus) and replaces the Text property when an entry
/// exists. Tooltips are keyed as "MainView/controlName.tip" and fetched through
/// <see cref="Tip"/>.
/// </summary>
public static class LocalizationModule
{
    public static readonly string[] Languages = { "English", "Português" };

    public static event Action? LanguageChanged;

    private static readonly Dictionary<string, (string En, string Pt)> Map = new();

    public static string Current => AppServices.Settings.Language;

    public static string T(string key)
    {
        if (!Map.TryGetValue(key, out var v)) return key;
        return Current == "Português" ? v.Pt : v.En;
    }

    public static string Tip(string controlName) => T($"MainView/{controlName}.tip");

    /// <summary>Persists the choice, retranslates every open form and raises
    /// <see cref="LanguageChanged"/> so custom panels can refresh dynamic texts.</summary>
    public static void SetLanguage(string language)
    {
        AppServices.Settings.Language = language;
        AppServices.SaveSettings();
        foreach (Form form in Application.OpenForms) Apply(form);
        LanguageChanged?.Invoke();
    }

    public static void Apply(Form form)
    {
        if (Map.TryGetValue(form.Name + "/", out var title) && !string.IsNullOrEmpty(form.Text))
            form.Text = Current == "Português" ? title.Pt : title.En;
        Localize(form, form.Name, form);
        foreach (Control control in Descendants(form)) Localize(form, form.Name, control);
    }

    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (Control nested in Descendants(child)) yield return nested;
        }
    }

    private static void Localize(Form form, string formName, Control control)
    {
        ApplyText(formName, control);
        if (control is ToolStrip strip)
            foreach (ToolStripItem item in strip.Items) LocalizeItem(formName, item);
        if (control is ContextMenuStrip menu)
            foreach (ToolStripItem item in menu.Items) LocalizeItem(formName, item);
        if (control.ContextMenuStrip != null)
            foreach (ToolStripItem item in control.ContextMenuStrip.Items) LocalizeItem(formName, item);
    }

    private static void ApplyText(string formName, Control control)
    {
        if (Map.TryGetValue($"{formName}/{control.Name}", out var v) && !string.IsNullOrEmpty(control.Text))
            control.Text = Current == "Português" ? v.Pt : v.En;
    }

    private static void LocalizeItem(string formName, ToolStripItem item)
    {
        if (Map.TryGetValue($"{formName}/{item.Name}", out var v) && !string.IsNullOrEmpty(item.Text))
            item.Text = Current == "Português" ? v.Pt : v.En;
        if (item is ToolStripDropDownItem drop)
            foreach (ToolStripItem child in drop.DropDownItems) LocalizeItem(formName, child);
    }

    private static void Add(string form, string control, string en, string pt) =>
        Map[$"{form}/{control}"] = (en, pt);

    private static void Tip(string control, string en, string pt) =>
        Map[$"MainView/{control}.tip"] = (en, pt);

    static LocalizationModule()
    {
        Add("MainView", "", "DataSyncEngine", "DataSyncEngine");

        Add("MainView", "lblNavCaption", "NAVIGATION", "NAVEGAÇÃO");
        Add("MainView", "lblTreeCap", "FILES", "FICHEIROS");
        Add("MainView", "navDashboard", "Dashboard", "Painel");
        Add("MainView", "navSync", "Sync", "Sincronização");
        Add("MainView", "navBackup", "Backup", "Backup");
        Add("MainView", "navTrash", "Trash", "Lixo");
        Add("MainView", "navServers", "Servers", "Servidores");
        Add("MainView", "navAudit", "Audit", "Auditoria");
        Add("MainView", "navAnalytics", "Analytics", "Estatísticas");
        Add("MainView", "navUsers", "Users", "Utilizadores");
        Add("MainView", "navSettings", "Settings", "Definições");

        Add("MainView", "tabDashboard", "Dashboard", "Painel");
        Add("MainView", "tabSync", "Sync", "Sincronização");
        Add("MainView", "tabBackup", "Backup", "Backup");
        Add("MainView", "tabTrash", "Trash", "Lixo");
        Add("MainView", "tabServers", "Servers", "Servidores");
        Add("MainView", "tabAudit", "Audit", "Auditoria");
        Add("MainView", "tabAnalytics", "Analytics", "Estatísticas");
        Add("MainView", "tabUsers", "Users", "Utilizadores");
        Add("MainView", "tabSettings", "Settings", "Definições");

        Add("MainView", "lblDashRecent", "RECENT ACTIVITY", "ATIVIDADE RECENTE");
        Add("MainView", "lblDashQuick", "QUICK ACTIONS", "AÇÕES RÁPIDAS");
        Add("MainView", "btnDashScan", "Scan a folder…", "Digitalizar uma pasta…");
        Add("MainView", "btnDashSync", "Sync pending files", "Sincronizar ficheiros pendentes");
        Add("MainView", "btnDashBackup", "Backup server files", "Exportar ficheiros do servidor");
        Add("MainView", "btnDashExplorer", "Open file explorer", "Abrir explorador de ficheiros");
        Add("MainView", "btnDashShortcuts", "Keyboard shortcuts", "Atalhos de teclado");
        Add("MainView", "btnDashDocs", "Read the user guide", "Ler o guia de utilizador");
        Add("MainView", "lblDashHint",
            "Tip: F5 refresh · Ctrl+F search · Ctrl+S sync · Ctrl+L lock · Ctrl+H user guide",
            "Dica: F5 atualizar · Ctrl+F procurar · Ctrl+S sincronizar · Ctrl+L bloquear · Ctrl+H guia de utilizador");

        Add("MainView", "lblFilesCap", "All files", "Todos os ficheiros");
        Add("MainView", "lblSyncedCap", "Synced", "Sincronizados");
        Add("MainView", "lblPendingCap", "Pending", "Pendentes");
        Add("MainView", "lblServersCap", "Servers", "Servidores");

        Add("MainView", "btnSelectFolder", "Select Folder", "Selecionar Pasta");
        Add("MainView", "btnSync", "Sync", "Sincronizar");
        Add("MainView", "btnDelete", "Delete", "Eliminar");
        Add("MainView", "btnResume", "Resume", "Retomar");
        Add("MainView", "btnExplorer", "Explorer", "Explorador");
        Add("MainView", "btnLogout", "Logout", "Terminar sessão");

        Add("MainView", "trayShow", "Show", "Mostrar");
        Add("MainView", "trayExit", "Exit", "Sair");
        Add("MainView", "trayAbout", "About", "Acerca");
        Add("MainView", "trayChangePassword", "Change Password", "Alterar palavra-passe");
        Add("MainView", "trayLock", "Lock Screen", "Bloquear ecrã");
        Add("MainView", "btnTheme", "Theme", "Tema");
        Add("MainView", "themeDarkItem", "Dark", "Escuro");
        Add("MainView", "themeLightItem", "Light", "Claro");
        Add("MainView", "btnNotifications", "Notifications", "Notificações");
        Add("MainView", "btnShortcuts", "Shortcuts", "Atalhos");
        Add("MainView", "btnDocs", "User guide", "Guia de utilizador");

        Add("MainView", "lblBackupTitle", "Server backup (export decrypted files)", "Backup do servidor (exporta ficheiros desencriptados)");
        Add("MainView", "btnBackupNow", "Backup Now", "Fazer backup agora");
        Add("MainView", "lblBackupFolderCap", "Automatic backup folder", "Pasta do backup automático");
        Add("MainView", "btnPickBackupFolder", "Change folder…", "Alterar pasta…");

        Add("MainView", "btnRestore", "Restore", "Restaurar");
        Add("MainView", "btnPurge", "Delete Forever", "Eliminar Definitivamente");
        Add("MainView", "lblTrashInfo", "Deleted files can be restored here.", "Os ficheiros eliminados podem ser restaurados aqui.");

        Add("MainView", "btnAddServer", "Add Server", "Adicionar Servidor");
        Add("MainView", "btnEditServer", "Edit Server", "Editar Servidor");
        Add("MainView", "btnSetActive", "Set Active", "Definir Ativo");
        Add("MainView", "btnDeleteServer", "Delete Server", "Eliminar Servidor");

        Add("MainView", "btnExportAudit", "Export CSV", "Exportar CSV");
        Add("MainView", "lblAnalytics", "Analytics dashboard (Admin only).", "Painel de estatísticas (apenas Admin).");

        Add("MainView", "btnAddUser", "Add User", "Adicionar Utilizador");
        Add("MainView", "btnDeleteUser", "Delete User", "Eliminar Utilizador");
        Add("MainView", "btnViewProfile", "View Profile", "Ver Perfil");
        Add("MainView", "btnResetPassword", "Reset Password", "Redefinir Palavra-passe");
        Add("MainView", "btnExportUsers", "Export CSV", "Exportar CSV");

        Add("MainView", "gbAppearance", "Appearance", "Aparência");
        Add("MainView", "gbLanguage", "Language", "Idioma");
        Add("MainView", "gbSync", "Synchronization", "Sincronização");
        Add("MainView", "gbBackup", "Backup", "Backup");
        Add("MainView", "gbSecurity", "Security and session", "Segurança e sessão");
        Add("MainView", "gbSystem", "System", "Sistema");

        Add("MainView", "lblSetTheme", "Theme", "Tema");
        Add("MainView", "radDark", "Dark", "Escuro");
        Add("MainView", "radLight", "Light", "Claro");
        Add("MainView", "lblSetFont", "Interface font size", "Tamanho da letra da interface");
        Add("MainView", "chkBold", "Bold text", "Texto a negrito");
        Add("MainView", "lblSetAccent", "Accent color", "Cor de destaque");
        Add("MainView", "btnResetSettings", "Reset Defaults", "Restaurar padrões");
        Add("MainView", "lblSetLanguage", "Interface language", "Idioma da interface");
        Add("MainView", "swAnimations", "Animations", "Animações");
        Add("MainView", "swConfirmDelete", "Confirm before delete", "Confirmar antes de eliminar");
        Add("MainView", "swNotifications", "Tray notifications", "Notificações na bandeja");
        Add("MainView", "swColdStorage", "Keep local cold copy", "Manter cópia local (cold)");
        Add("MainView", "swMinimizeToTray", "Minimize to tray", "Minimizar para a bandeja");
        Add("MainView", "swAutoSync", "Auto-sync on change", "Sincronizar ao detetar alterações");
        Add("MainView", "swStartWithWindows", "Start with Windows", "Iniciar com o Windows");
        Add("MainView", "lblSetIdle", "Auto-logout when idle (minutes)", "Terminar sessão por inatividade (minutos)");
        Add("MainView", "lblSetAutoBackup", "Automatic backup (hours, 0 = off)", "Backup automático (horas, 0 = desligado)");
        Add("MainView", "lblSetBackupFolderSettings", "Automatic backup folder", "Pasta do backup automático");
        Add("MainView", "btnSetBackupFolder", "Change folder…", "Alterar pasta…");
        Add("MainView", "lblExcludeCap", "Exclude patterns (separated by ;)", "Padrões a excluir (separados por ;)");
        Add("MainView", "lblSetCompression", "Vault compression (new uploads)", "Compressão do cofre (novos envios)");
        Add("MainView", "lblSetParallel", "Parallel uploads (1 = sequential)", "Envios em paralelo (1 = sequencial)");
        Add("MainView", "swVerifyAfterSync", "Verify integrity after sync", "Verificar integridade após sincronizar");
        Add("MainView", "lblSetMaxSize", "Skip files larger than (MB, 0 = off)", "Ignorar ficheiros maiores que (MB, 0 = desligado)");
        Add("MainView", "lblSetRetention", "Audit retention (days, 0 = keep)", "Retenção da auditoria (dias, 0 = manter)");

        Tip("btnSelectFolder", "Choose a folder to scan", "Escolha uma pasta para digitalizar");
        Tip("btnSync", "Upload pending files to the server (Ctrl+S)", "Envia os ficheiros pendentes para o servidor (Ctrl+S)");
        Tip("btnDelete", "Delete the selected file (Del)", "Elimina o ficheiro selecionado (Del)");
        Tip("btnResume", "Retry the uploads that failed", "Tenta novamente os envios que falharam");
        Tip("btnExplorer", "Browse files with hashes and metadata", "Navega pelos ficheiros com hashes e metadados");
        Tip("btnLogout", "Log out of the application", "Termina a sessão na aplicação");
        Tip("txtFilter", "Type to filter the file list (Ctrl+F)", "Escreva para filtrar a lista de ficheiros (Ctrl+F)");
        Tip("progressBar", "Synchronization progress", "Progresso da sincronização");
        Tip("btnBackupNow", "Export all server files to a folder", "Exporta todos os ficheiros do servidor para uma pasta");
        Tip("btnRestore", "Restore the selected deleted file", "Restaura o ficheiro eliminado selecionado");
        Tip("btnPurge", "Permanently delete — cannot be undone", "Elimina definitivamente — não pode ser desfeito");
        Tip("btnDashScan", "Scan a local folder and list its files", "Digitaliza uma pasta local e lista os ficheiros");
        Tip("btnDashSync", "Upload all pending files to the server", "Envia todos os ficheiros pendentes para o servidor");
        Tip("btnDashBackup", "Export decrypted server files to a folder", "Exporta os ficheiros desencriptados para uma pasta");
        Tip("btnDashExplorer", "Browse server files with hashes and metadata", "Navega pelos ficheiros com hashes e metadados");
        Tip("btnDashShortcuts", "Show all keyboard shortcuts", "Mostra todos os atalhos de teclado");
        Tip("btnDashDocs", "Open the step-by-step user guide", "Abre o guia de utilizador passo a passo");
        Tip("gridDashAudit", "Latest actions performed in the application", "Últimas ações realizadas na aplicação");
        Tip("btnShortcuts", "Show all keyboard shortcuts (Ctrl+K)", "Mostra todos os atalhos de teclado (Ctrl+K)");
        Tip("btnDocs", "Open the step-by-step user guide (Ctrl+H)", "Abre o guia de utilizador passo a passo (Ctrl+H)");
        Tip("btnTheme", "Switch between dark and light appearance", "Alterna entre aparência escura e clara");
        Tip("btnNotifications", "Recent notifications", "Notificações recentes");
        Tip("swAutoSync", "Uploads a file automatically as soon as it changes on disk", "Envia um ficheiro automaticamente assim que muda no disco");
        Tip("swColdStorage", "Keeps a local decrypted copy so backups and previews are instant", "Mantém uma cópia local desencriptada para backups e pré-visualização instantâneos");
        Tip("swVerifyAfterSync", "Re-opens each stored vault file and compares it with the source", "Reabre cada ficheiro guardado no cofre e compara-o com a origem");
        Tip("swStartWithWindows", "Launch DataSyncEngine automatically at sign-in", "Inicia o DataSyncEngine automaticamente ao entrar no Windows");
        Tip("swConfirmDelete", "Ask for confirmation before deleting or purging files", "Pede confirmação antes de eliminar ou limpar ficheiros");
        Tip("swMinimizeToTray", "Closing the window keeps the application running in the tray", "Fechar a janela mantém a aplicação a correr na bandeja");
        Tip("swNotifications", "Show tray balloon tips for sync and backup events", "Mostra balões na bandeja para eventos de sincronização e backup");
        Tip("swAnimations", "Animated progress and toast transitions", "Progresso animado e transições das notificações");
        Tip("cmbIdle", "Locks the session after this idle period (0 = never)", "Bloqueia a sessão após este período sem atividade (0 = nunca)");
        Tip("cmbAutoBackup", "Runs an automatic backup every N hours (0 = off)", "Faz backup automático a cada N horas (0 = desligado)");
        Tip("btnSetBackupFolder", "Folder used by the scheduled automatic backup", "Pasta usada pelo backup automático agendado");
        Tip("txtExclude", "File masks ignored by scans, separated by ;", "Máscaras de ficheiros ignoradas nas digitalizações, separadas por ;");
        Tip("cmbCompression", "Compression applied before sealing new uploads in the vault", "Compressão aplicada antes de selar novos envios no cofre");
        Tip("cmbParallel", "Number of files uploaded at the same time (1 = sequential)", "Número de ficheiros enviados ao mesmo tempo (1 = sequencial)");
        Tip("cmbMaxSize", "Files above this size are skipped during scan (0 = no limit)", "Ficheiros acima deste tamanho são ignorados na digitalização (0 = sem limite)");
        Tip("cmbRetention", "Audit entries older than this are purged automatically (0 = keep forever)", "Registos de auditoria mais antigos são apagados automaticamente (0 = manter)");
        Tip("trkFontSize", "Changes the font size of the whole application", "Altera o tamanho da letra de toda a aplicação");
        Tip("cmbAccent", "Highlight color used by buttons and selections", "Cor de destaque usada nos botões e seleções");
        Tip("radDark", "GitHub-inspired dark theme", "Tema escuro inspirado no GitHub");
        Tip("radLight", "Bright theme for well-lit environments", "Tema claro para ambientes bem iluminados");
        Tip("cmbLanguage", "Switches the interface between English and Portuguese", "Alterna a interface entre inglês e português");
        Tip("btnResetSettings", "Restores every setting to its default value", "Repõe todas as definições no valor original");

        Add("LoginView", "", "DataSyncEngine - Login", "DataSyncEngine - Iniciar sessão");
        Add("LoginView", "lblSubtitle", "Sign in to continue", "Inicie sessão para continuar");
        Add("LoginView", "chkShowPassword", "Show password", "Mostrar palavra-passe");
        Add("LoginView", "btnLogin", "LOGIN", "ENTRAR");

        Add("SplashView", "", "DataSyncEngine", "DataSyncEngine");

        Add("AboutView", "", "About DataSyncEngine", "Acerca do DataSyncEngine");
        Add("AboutView", "lblVersion", "Version", "Versão");
        Add("AboutView", "lblDescription",
            "Corporate file synchronization with AES-256 encryption,\r\ncompression and MariaDB persistence.",
            "Sincronização de ficheiros empresarial com encriptação AES-256,\r\ncompressão e persistência em MariaDB.");

        Add("ShortcutsView", "", "Keyboard shortcuts", "Atalhos de teclado");
        Add("ShortcutsView", "lblTitle", "Keyboard shortcuts", "Atalhos de teclado");
        Add("ShortcutsView", "lblAction1", "Select a folder to scan", "Selecionar uma pasta para digitalizar");
        Add("ShortcutsView", "lblAction2", "Sync pending files", "Sincronizar ficheiros pendentes");
        Add("ShortcutsView", "lblAction3", "Refresh the current tab", "Atualizar o separador atual");
        Add("ShortcutsView", "lblAction4", "Open the file explorer", "Abrir o explorador de ficheiros");
        Add("ShortcutsView", "lblAction5", "Go to the backup tab", "Ir para o separador de backup");
        Add("ShortcutsView", "lblAction6", "Go to the dashboard", "Ir para o painel");
        Add("ShortcutsView", "lblAction7", "Show this shortcut guide", "Mostrar este guia de atalhos");
        Add("ShortcutsView", "lblAction8", "Open the file metadata viewer", "Abrir o visualizador de metadados");
        Add("ShortcutsView", "lblAction9", "Open the user guide", "Abrir o guia de utilizador");
        Add("ShortcutsView", "lblAction10", "Close this dialog", "Fechar esta janela");
        Add("ShortcutsView", "btnClose", "Close", "Fechar");

        Add("LockView", "", "DataSyncEngine - Locked", "DataSyncEngine - Bloqueado");
        Add("LockView", "lblTitle", "Screen locked", "Ecrã bloqueado");
        Add("LockView", "btnUnlock", "UNLOCK", "DESBLOQUEAR");

        Add("ChangePasswordView", "", "Change Password", "Alterar palavra-passe");
        Add("ChangePasswordView", "btnSave", "Change Password", "Alterar palavra-passe");

        Add("MasterKeyPopUpView", "", "Master Key Required", "Chave mestra necessária");
        Add("MasterKeyPopUpView", "lblPrompt", "Enter the master key to authorize this operation:",
            "Introduza a chave mestra para autorizar esta operação:");
        Add("MasterKeyPopUpView", "btnCancel", "Cancel", "Cancelar");

        Add("FileMetadataView", "", "File Metadata", "Metadados do ficheiro");
        Add("FileMetadataView", "colLabel", "Property", "Propriedade");
        Add("FileMetadataView", "colValue", "Value", "Valor");
        Add("FileMetadataView", "btnCopy", "Copy", "Copiar");
        Add("FileMetadataView", "btnCopyAll", "Copy All", "Copiar tudo");
        Add("FileMetadataView", "btnClose", "Close", "Fechar");

        Add("FileExplorerView", "", "DataSyncEngine - File Explorer", "DataSyncEngine - Explorador de ficheiros");
        Add("FileExplorerView", "lblDetailsTitle", "Details", "Detalhes");
        Add("FileExplorerView", "btnBack", "Up", "Acima");
        Add("FileExplorerView", "btnBrowse", "Browse", "Procurar");
        Add("FileExplorerView", "btnRefresh", "Refresh", "Atualizar");
        Add("FileExplorerView", "btnScan", "Scan", "Digitalizar");
        Add("FileExplorerView", "btnSyncAll", "Sync Selected", "Sincronizar seleção");
        Add("FileExplorerView", "ctxSync", "Sync now", "Sincronizar agora");
        Add("FileExplorerView", "ctxDownload", "Download...", "Transferir...");
        Add("FileExplorerView", "ctxMetadata", "View metadata", "Ver metadados");
        Add("FileExplorerView", "ctxHash", "Copy SHA-256", "Copiar SHA-256");
        Add("FileExplorerView", "ctxResume", "Mark for resume", "Marcar para retomar");
        Add("FileExplorerView", "ctxDelete", "Move to trash", "Mover para o lixo");
        Add("FileExplorerView", "ctxRefresh", "Refresh", "Atualizar");

        Add("UserProfileView", "", "User Profile", "Perfil do utilizador");
        Add("UserProfileView", "lblName", "Name", "Nome");
        Add("UserProfileView", "lblRole", "Role", "Função");
        Add("UserProfileView", "lblActive", "Active", "Ativo");
        Add("UserProfileView", "lblLastSync", "Last sync", "Última sincronização");
        Add("UserProfileView", "lblHistoryCap", "Recent history", "Histórico recente");
        Add("UserProfileView", "btnClose", "Close", "Fechar");

        Add("UserConfigView", "", "User Configuration", "Configuração do utilizador");
        Add("UserConfigView", "chkActive", "Active", "Ativo");
        Add("UserConfigView", "btnPhoto", "Choose Photo...", "Escolher foto...");
        Add("UserConfigView", "btnSave", "Save", "Guardar");

        Add("ServerConfigView", "", "Server Configuration", "Configuração do servidor");
        Add("ServerConfigView", "chkActive", "Set as active server", "Definir como servidor ativo");
        Add("ServerConfigView", "btnTest", "Test", "Testar");
        Add("ServerConfigView", "btnSave", "Save", "Guardar");

        Add("MainView", "lblStatus", "Ready.", "Pronto.");
        Add("MainView", "lblBackupInfo", "No backup yet.", "Ainda sem backup.");
        Add("MainView", "btnNotificationsCount", "Notifications ({0})", "Notificações ({0})");
        Add("MainView", "noNotifications", "No notifications yet", "Ainda sem notificações");
        Add("MainView", "clearAll", "Clear all", "Limpar tudo");
        Add("MainView", "signedInFormat", "Signed in as {0} ({1}).", "Sessão iniciada como {0} ({1}).");
        Add("MainView", "lastBackupNever", "Last backup: never", "Último backup: nunca");
        Add("MainView", "lastBackupFormat", "Last backup: {0}", "Último backup: {0}");
        Add("MainView", "backupFinishedFormat", "Backup finished: {0} exported, {1} failed.", "Backup concluído: {0} exportados, {1} falhados.");
        Add("MainView", "backupFolderDefault", @"(not set — defaults to Documents\DataSyncEngineBackup)", @"(não definida — por omissão Documentos\DataSyncEngineBackup)");
        Add("MainView", "themeDarkApplied", "Dark theme applied.", "Tema escuro aplicado.");
        Add("MainView", "themeLightApplied", "Light theme applied.", "Tema claro aplicado.");
        Add("MainView", "autoSyncOn", "Auto-sync enabled.", "Sincronização automática ligada.");
        Add("MainView", "autoSyncOff", "Auto-sync disabled.", "Sincronização automática desligada.");
        Add("MainView", "excludeSaved", "Exclude patterns saved. Rescan to apply.", "Padrões de exclusão guardados. Digitalize novamente para aplicar.");
        Add("MainView", "settingsReset", "Settings restored to defaults.", "Definições repostas nos valores originais.");
        Add("MainView", "startWithWinOn", "DataSyncEngine will start with Windows.", "O DataSyncEngine vai iniciar com o Windows.");
        Add("MainView", "startWithWinOff", "Auto-start disabled.", "Início automático desligado.");
        Add("MainView", "startWithWinFail", "Could not update the Windows startup entry.", "Não foi possível atualizar a entrada de início do Windows.");
        Add("MainView", "compressionSet", "Vault compression set to {0}.", "Compressão do cofre definida para {0}.");
        Add("MainView", "parallelSequential", "Uploads run sequentially.", "Os envios são feitos sequencialmente.");
        Add("MainView", "parallelSet", "Up to {0} uploads at the same time.", "Até {0} envios ao mesmo tempo.");
        Add("MainView", "verifyOn", "Integrity check enabled after every upload.", "Verificação de integridade ligada em cada envio.");
        Add("MainView", "verifyOff", "Integrity check disabled.", "Verificação de integridade desligada.");
        Add("MainView", "maxSizeOff", "No file size limit. Rescan to apply.", "Sem limite de tamanho. Digitalize novamente para aplicar.");
        Add("MainView", "maxSizeLimit", "Files larger than {0} MB are skipped. Rescan to apply.", "Ficheiros maiores que {0} MB são ignorados. Digitalize novamente para aplicar.");
        Add("MainView", "retentionOff", "Audit entries are kept forever.", "Os registos de auditoria são mantidos para sempre.");
        Add("MainView", "retentionLimit", "Audit entries older than {0} days will be purged daily.", "Registos de auditoria com mais de {0} dias serão apagados diariamente.");
        Add("MainView", "backupFolderSet", "Backup folder updated.", "Pasta de backup atualizada.");
        Add("MainView", "languageSet", "Language changed to {0}.", "Idioma alterado para {0}.");
        Add("MainView", "idleOff", "Auto-logout disabled.", "Terminar sessão por inatividade desligado.");
        Add("MainView", "idleSet", "Auto-logout after {0} minutes of inactivity.", "Sessão termina após {0} minutos sem atividade.");
        Add("MainView", "autoBackupOff", "Automatic backup disabled.", "Backup automático desligado.");
        Add("MainView", "autoBackupSet", "Automatic backup every {0}h.", "Backup automático a cada {0}h.");

        Add("MainView", "pageDashboard", "Dashboard", "Painel");
        Add("MainView", "subDashboard", "Synchronization workspace overview", "Visão geral do espaço de sincronização");
        Add("MainView", "pageSync", "Sync", "Sincronização");
        Add("MainView", "subSync", "Scan a local folder and upload changed files", "Digitalize uma pasta local e envie os ficheiros alterados");
        Add("MainView", "pageBackup", "Backup", "Backup");
        Add("MainView", "subBackup", "Export decrypted files from the server", "Exporte ficheiros desencriptados do servidor");
        Add("MainView", "pageTrash", "Trash", "Lixo");
        Add("MainView", "subTrash", "Restore or permanently remove deleted files", "Restaure ou remova definitivamente ficheiros eliminados");
        Add("MainView", "pageServers", "Servers", "Servidores");
        Add("MainView", "subServers", "Manage database connections", "Gerir ligações à base de dados");
        Add("MainView", "pageAudit", "Audit log", "Registo de auditoria");
        Add("MainView", "subAudit", "Traceability of every user action", "Rastreabilidade de todas as ações dos utilizadores");
        Add("MainView", "pageAnalytics", "Analytics", "Estatísticas");
        Add("MainView", "subAnalytics", "Aggregated synchronization metrics", "Métricas agregadas de sincronização");
        Add("MainView", "pageUsers", "Users", "Utilizadores");
        Add("MainView", "subUsers", "Accounts and access control", "Contas e controlo de acesso");
        Add("MainView", "pageSettings", "Settings", "Definições");
        Add("MainView", "subSettings", "Appearance, behaviour and automation", "Aparência, comportamento e automatização");

        Add("HelpView", "", "User guide", "Guia de utilizador");
        Add("HelpView", "tabGuidePt", "Português", "Português");
        Add("HelpView", "tabGuideEn", "English", "English");
        Add("HelpView", "btnClose", "Close", "Fechar");
    }
}
