# Docker local working system — compose tree

**Boundary:** first Dockerized local working system. Development credentials only — not for staging or production.

## Layout

```text
docker/local-working-system/
  README.md
  docker-compose.yml
  .env.example
  postgres/init/          # ENV-DB-001 — first-init bootstrap SQL
  scripts/
    start-local-working-system.ps1
    wait-local-working-system.ps1
    reset-local-working-system.ps1
    verify-postgres-bootstrap.ps1
    seed-and-verify-local-working-system.ps1
```

Copy `.env.example` to `.env` to override placeholders locally without committing changes:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
Copy-Item .env.example .env
```

## Compose orchestration (ENV-COMPOSE-001)

Start stack:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
```

Wait-only (if stack already running):

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\wait-local-working-system.ps1
```

Reset stack + volumes:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
```

## Postgres bootstrap (ENV-DB-001)

Init SQL creates logical databases and service users:

| Database | User | Password (dev) |
| --- | --- | --- |
| `allagma_local` | `allagma_local` | `allagma_local_pw` |
| `kanon_local` | `kanon_local` | `kanon_local_pw` |
| `conexus_local` | `conexus_local` | `conexus_local_pw` |

Admin bootstrap (compose / verify script): `ontogony_admin` / `ontogony_admin_pw`.

Mount `postgres/init` read-only to `/docker-entrypoint-initdb.d` (runs only on empty data directory).

Verify without full compose:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-postgres-bootstrap.ps1 -Recreate
```

## Seed/bootstrap (ENV-SEED-001)

`seed-and-verify-local-working-system.ps1` performs deterministic API/bootstrap steps:

- Conexus `POST /admin/v0/dev/bootstrap` (dev project + fake provider alias)
- baseline + subject Allagma runs
- Kanon topology evidence checks (`single_workflow` null baseline auth ID; centralized subject auth decision)
- Conexus route-decision evidence checks
- Allagma eval write/list + baseline comparison create/fetch
- JSON evidence report export under `docker/local-working-system/artifacts/`

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Docs

Canonical plan: `docs/environments/docker-local-working-system/`.
