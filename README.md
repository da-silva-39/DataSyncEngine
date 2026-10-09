# DataSyncEngine

Corporate file synchronization engine with AES-256 encryption, MariaDB persistence and WinForms UI.

## Projects

- Core: contracts, enums and environment directives
- Model: entities, MySqlConnector repositories with 5s timeout
- Controller: crypto, compression, cache, sync and governance engines
- Views: WinForms UI with dark corporate theme

## Database setup

Run schema.sql on MariaDB, then seed.sql for the initial admin user (admin / admin123).

## Run

Build the solution with .NET 10 SDK and run the Views project, or use publish/Views.exe.
Connection settings live in appsettings.json next to the executable.

## Features

- AES-256 file encryption with PBKDF2 password hashing, GZip compression, MariaDB BLOB storage
- Manual, real-time (auto-sync on file change) and trash with restore
- Exclude patterns (e.g. *.tmp;*.log), server backup export, per-file SHA-256 integrity
- Role-based UI (Admin / Operator), silent audit logging, tray notifications and toasts
- Place logo.png next to the executable to brand the login, sidebar and window icons.
