CREATE DATABASE IF NOT EXISTS datasync_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE datasync_db;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(64) NOT NULL,
    password_hash VARCHAR(128) NOT NULL,
    salt VARCHAR(128) NOT NULL,
    role VARCHAR(16) NOT NULL DEFAULT 'Operator',
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    UNIQUE KEY ux_users_username (username)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS servers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(128) NOT NULL,
    host VARCHAR(255) NOT NULL,
    port INT NOT NULL DEFAULT 3306,
    database_name VARCHAR(128) NOT NULL,
    user_name VARCHAR(128) NOT NULL,
    password VARCHAR(255) NOT NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 0,
    KEY ix_servers_active (is_active)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS files (
    id INT AUTO_INCREMENT PRIMARY KEY,
    file_name VARCHAR(512) NOT NULL,
    file_path VARCHAR(1024) NOT NULL,
    size_bytes BIGINT NOT NULL DEFAULT 0,
    sha256_hash VARCHAR(64) NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'Pending',
    uploaded_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    content_blob LONGBLOB,
    KEY ix_files_status (status),
    KEY ix_files_hash (sha256_hash)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS audit_logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(64) NOT NULL,
    action VARCHAR(32) NOT NULL,
    details VARCHAR(1024) NOT NULL DEFAULT '',
    timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY ix_audit_username (username),
    KEY ix_audit_timestamp (timestamp)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS trash_files (
    id INT AUTO_INCREMENT PRIMARY KEY,
    file_name VARCHAR(512) NOT NULL,
    file_path VARCHAR(1024) NOT NULL,
    size_bytes BIGINT NOT NULL DEFAULT 0,
    sha256_hash VARCHAR(64) NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'Pending',
    uploaded_at DATETIME NOT NULL,
    content_blob LONGBLOB,
    deleted_by VARCHAR(64) NOT NULL DEFAULT '',
    deleted_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY ix_trash_deleted_at (deleted_at)
) ENGINE=InnoDB;
