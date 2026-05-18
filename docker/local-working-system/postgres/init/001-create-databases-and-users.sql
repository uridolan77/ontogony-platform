-- ENV-DB-001 — local dev only. Mounted to /docker-entrypoint-initdb.d on first Postgres init.
-- Creates per-service databases and users for the Docker local working system.

CREATE USER allagma_local WITH PASSWORD 'allagma_local_pw';
CREATE USER kanon_local WITH PASSWORD 'kanon_local_pw';
CREATE USER conexus_local WITH PASSWORD 'conexus_local_pw';

CREATE DATABASE allagma_local OWNER allagma_local;
CREATE DATABASE kanon_local OWNER kanon_local;
CREATE DATABASE conexus_local OWNER conexus_local;

GRANT ALL PRIVILEGES ON DATABASE allagma_local TO allagma_local;
GRANT ALL PRIVILEGES ON DATABASE kanon_local TO kanon_local;
GRANT ALL PRIVILEGES ON DATABASE conexus_local TO conexus_local;
