# ENV-DB-001 — Postgres DB bootstrap evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** PASS  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` only: init SQL for three logical databases/users and a verification script. No compose file, service seed rows, migrations, or API containers.

## Delivered

```text
docker/local-working-system/
  README.md
  postgres/init/001-create-databases-and-users.sql
  scripts/verify-postgres-bootstrap.ps1
docs/environments/docker-local-working-system/04_DATABASES_AND_SEEDS.md (path + verify command)
docs/environments/docker-local-working-system/README.md
docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md
docs/environments/README.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_DB_001_POSTGRES_BOOTSTRAP_EVIDENCE.md
```

## Bootstrap objects

| Database | User | Dev password |
| --- | --- | --- |
| `allagma_local` | `allagma_local` | `allagma_local_pw` |
| `kanon_local` | `kanon_local` | `kanon_local_pw` |
| `conexus_local` | `conexus_local` | `conexus_local_pw` |

Admin (container bootstrap): `ontogony_admin` / `ontogony_admin_pw`.

## Commands run

```powershell
cd C:\dev\ontogony-platform

# File presence
@(
  "docker\local-working-system\postgres\init\001-create-databases-and-users.sql",
  "docker\local-working-system\scripts\verify-postgres-bootstrap.ps1"
) | ForEach-Object { [PSCustomObject]@{ File = $_; Exists = (Test-Path $_) } }

# Disposable postgres:16 with init mount (host port 55433)
.\docker\local-working-system\scripts\verify-postgres-bootstrap.ps1 -Recreate
```

## Results

| Check | Result |
| --- | --- |
| Init SQL present | **PASS** |
| Verify script present | **PASS** |
| Databases `allagma_local`, `kanon_local`, `conexus_local` | **PASS** |
| Roles `allagma_local`, `kanon_local`, `conexus_local` | **PASS** |
| Service-user login to owned database | **PASS** (each `SELECT 1`) |

Verify script output (2026-05-19):

```text
PASS login: allagma_local -> allagma_local
PASS login: kanon_local -> kanon_local
PASS login: conexus_local -> conexus_local
ENV-DB-001 Postgres bootstrap verification PASS.
```

## Safety

| Check | Status |
| --- | --- |
| No secrets committed beyond documented dev placeholders | **yes** |
| Real external execution | **not in scope** (unchanged) |
| Production readiness claimed | **no** |

## Known limitations

- Init SQL runs only on **empty** Postgres data directory (`/docker-entrypoint-initdb.d` semantics).
- No `docker-compose.yml` until **ENV-COMPOSE-001**.
- No seed rows until **ENV-SEED-001**.
- Verify container uses host port **55433** by default to avoid conflict with local Postgres (`5432`) and ENV-PG-001 (`55432`).

## Next step

**ENV-SEED-001** — deterministic seed/bootstrap rows (Conexus dev project/fake provider, Kanon topology defaults, Allagma eval persistence checks).
