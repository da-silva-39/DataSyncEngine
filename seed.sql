USE datasync_db;

-- Initial bootstrap seed. Default login: admin / admin123 (change after first login).
-- Replace CHANGE_ME_ROOT_PASSWORD with the real database password before running.

INSERT INTO users (username, password_hash, salt, role, is_active) VALUES
('admin', '0v+jBoRRVP05r/yjJM04dCiVLSErydHlkqNVWFFjawI=', 'c3RhdGljc2FsdDEyMzQ1Njc4OTAxMjM0NTY3ODkwYWI=', 'Admin', 1);

INSERT INTO servers (name, host, port, database_name, user_name, password, is_active) VALUES
('local', 'localhost', 3306, 'datasync_db', 'root', 'CHANGE_ME_ROOT_PASSWORD', 1);
