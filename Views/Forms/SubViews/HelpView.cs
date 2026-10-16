using Views.Components;

namespace Views.Forms.SubViews;

public partial class HelpView : Krypton.Toolkit.KryptonForm
{
    public HelpView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        txtGuidePt.Text = GuidePt;
        txtGuideEn.Text = GuideEn;
        txtGuidePt.Select(0, 0);
        txtGuideEn.Select(0, 0);
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private const string GuidePt = """
DataSyncEngine — Guia de utilizador

1. PRIMEIROS PASSOS
   1.1. Inicie sessão com o seu utilizador e palavra-passe.
   1.2. No primeiro arranque é criado um cofre AES-256-GCM, onde todos os
        ficheiros ficam encriptados no servidor. A chave mestra é entregue
        através de um ficheiro protegido — guarde-a em local seguro.
   1.3. Configure a ligação ao servidor na página "Servidores".

2. PAINEL (Dashboard)
   - Visão geral: total de ficheiros, sincronizados, pendentes e servidores.
   - Botões rápidos: digitalizar, sincronizar, backup, explorador, atalhos
     e este guia.
   - Lista os oito eventos de auditoria mais recentes. Duplo clique abre o
     registo completo.

3. SINCRONIZAÇÃO
   3.1. Escolha a pasta local com "Selecionar pasta…" (Ctrl+O).
   3.2. A digitalização lista os ficheiros alterados; as alterações são
        detetadas por hash SHA-256.
   3.3. "Sincronizar" (Ctrl+S) envia os ficheiros pendentes. Uma barra de
        progresso mostra o estado e o toast final resume o resultado.
   3.4. Se ativar a verificação de integridade nas Definições, cada envio é
        relido do cofre e comparado com o original.

4. BACKUP
   - "Exportar backup" copia os ficheiros desencriptados do servidor para a
     pasta de destino.
   - "Definir pasta de backup" escolhe o destino do backup automático.
   - O backup automático corre em segundo plano a cada N horas (Definições).
   - "Repor" recupera a versão guardada de um ficheiro selecionado.

5. LIXO
   - Os ficheiros eliminados ficam no lixo até serem repostos ou removidos
     permanentemente.
   - "Eliminar" move para o lixo; "Eliminar permanentemente" apaga o blob do
     cofre e não pode ser anulado.
   - "Restaurar" volta a colocar o ficheiro na lista ativa.

6. SERVIDORES
   - Registe uma ou mais ligações (MySQL/MariaDB) com nome, host, porta,
     base de dados e credenciais.
   - "Testar ligação" valida os dados antes de guardar.
   - O servidor predefinido é usado para sincronizar.

7. AUDITORIA
   - Registo imutável de todas as ações: sessões, sincronizações, backups,
     restauros e purgas.
   - Filtre por texto e exporte para CSV (Ctrl+Shift+E no menu de contexto).
   - Consoante a retenção definida, registos antigos são purgados a cada dia.

8. ESTATÍSTICAS
   - Gráficos agregados: ficheiros por estado, tamanho por tipo e atividade
     diária.

9. UTILIZADORES (apenas Administrador)
   - Criar, editar e desativar contas, com papéis Admin/User.
   - Alterar palavra-passe e avatar na sua conta (canto inferior esquerdo).

10. DEFINIÇÕES
    Aparência
      - Tema escuro (GitHub-inspired) ou claro.
      - Tamanho da letra de toda a aplicação (10–20 pt) e negrito opcional.
      - Cor de destaque usada nos botões e seleções.
    Idioma
      - Português ou English; a mudança é imediata, sem reiniciar.
    Sincronização
      - Auto-sync ao alterar um ficheiro.
      - Verificar integridade depois de sincronizar.
      - Manter cópia local (cold copy).
      - Envios paralelos (1–5), limite de tamanho (MB) e padrões de exclusão
        (ex.: *.tmp;*.log).
    Backup
      - Backup automático a cada N horas (0 = desligado) e pasta de destino.
      - As alterações aplicam-se de imediato, sem reiniciar.
    Segurança e sessão
      - Confirmar antes de eliminar.
      - Minimizar para a bandeja ao fechar.
      - Notificações na bandeja.
      - Terminar sessão por inatividade (0 = nunca).
    Sistema
      - Iniciar com o Windows.
      - Animações.
      - Compressão do cofre (None/GZip/Brotli) para novos envios.
      - Retenção da auditoria em dias (0 = manter para sempre).
    "Repor predefinições" devolve todas as opções ao estado original.

11. SEGURANÇA
    - Ficheiros e backups são cifrados com AES-256-GCM; a chave deriva da
      chave mestra do utilizador.
    - Bloqueio de sessão com Ctrl+L; a conta bloqueia após 5 tentativas
      falhadas.
    - Nenhum dado é enviado para fora do seu servidor.

12. ATALHOS
    Ctrl+O selecionar pasta · Ctrl+S sincronizar · Ctrl+R/F5 atualizar
    Ctrl+E explorador · Ctrl+B backup · Ctrl+D painel · Ctrl+K atalhos
    Ctrl+H guia · Ctrl+L bloquear · Esc fechar diálogos
""";

    private const string GuideEn = """
DataSyncEngine — User guide

1. GETTING STARTED
   1.1. Sign in with your username and password.
   1.2. On first run an AES-256-GCM vault is created; every file is stored
        encrypted on the server. The master key is delivered through a
        protected file — keep it somewhere safe.
   1.3. Configure the server connection on the "Servers" page.

2. DASHBOARD
   - Overview: total files, synced, pending and servers.
   - Quick actions: scan, sync, backup, explorer, shortcuts and this guide.
   - Lists the eight most recent audit events. Double-click opens the full
     log.

3. SYNC
   3.1. Pick the local folder with "Select folder…" (Ctrl+O).
   3.2. The scan lists changed files; changes are detected by SHA-256 hash.
   3.3. "Sync" (Ctrl+S) uploads pending files. A progress bar shows the
        state and a final toast summarizes the result.
   3.4. With integrity checking enabled in Settings, every upload is re-read
        from the vault and compared with the original.

4. BACKUP
   - "Export backup" copies the decrypted files from the server to the
     destination folder.
   - "Set backup folder" chooses the destination for automatic backups.
   - Automatic backup runs in the background every N hours (Settings).
   - "Restore" brings back the stored version of a selected file.

5. TRASH
   - Deleted files stay in the trash until restored or removed permanently.
   - "Delete" moves to trash; "Delete permanently" wipes the vault blob and
     cannot be undone.
   - "Restore" puts the file back into the active list.

6. SERVERS
   - Register one or more connections (MySQL/MariaDB) with name, host, port,
     database and credentials.
   - "Test connection" validates the data before saving.
   - The default server is used for synchronization.

7. AUDIT
   - Immutable record of every action: sessions, syncs, backups, restores
     and purges.
   - Filter by text and export to CSV (Ctrl+Shift+E in the context menu).
   - Depending on the configured retention, old entries are purged daily.

8. ANALYTICS
   - Aggregated charts: files by status, size by type and daily activity.

9. USERS (Administrator only)
   - Create, edit and disable accounts with Admin/User roles.
   - Change your password and avatar in your account (bottom-left corner).

10. SETTINGS
    Appearance
      - Dark (GitHub-inspired) or light theme.
      - Font size for the whole application (10–20 pt) and optional bold.
      - Accent colour used by buttons and selections.
    Language
      - Portuguese or English; the change is immediate, no restart needed.
    Synchronization
      - Auto-sync when a file changes.
      - Verify integrity after syncing.
      - Keep local cold copy.
      - Parallel uploads (1–5), size limit (MB) and exclude patterns
        (e.g. *.tmp;*.log).
    Backup
      - Automatic backup every N hours (0 = off) and destination folder.
      - Changes apply immediately, no restart needed.
    Security and session
      - Confirm before deleting.
      - Minimize to tray on close.
      - Tray notifications.
      - Auto-logout when idle (0 = never).
    System
      - Start with Windows.
      - Animations.
      - Vault compression (None/GZip/Brotli) for new uploads.
      - Audit retention in days (0 = keep forever).
    "Reset Defaults" restores every option to its original value.

11. SECURITY
    - Files and backups are encrypted with AES-256-GCM; the key derives from
      the user's master key.
    - Lock the session with Ctrl+L; the account locks after 5 failed
      attempts.
    - No data ever leaves your server.

12. SHORTCUTS
    Ctrl+O select folder · Ctrl+S sync · Ctrl+R/F5 refresh
    Ctrl+E explorer · Ctrl+B backup · Ctrl+D dashboard · Ctrl+K shortcuts
    Ctrl+H guide · Ctrl+L lock · Esc close dialogs
""";
}
