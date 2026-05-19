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
    run-docker-guided-main-flow.ps1
    validate-docker-guided-main-flow.ps1
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

## Dockerized guided main flow (ENV-DOCKER-RUN-001)

Runs wait → seed/bootstrap → `allagma-api` restart → persistence re-fetch → machine report validation.

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
```

Report: `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`

## Troubleshooting

### Conexus `/health/live` returns 404 but container is healthy

Usually **not** a stale Docker image. On Windows, a local `Conexus.Api` dev process may bind `127.0.0.1:5082` while Docker binds `0.0.0.0:5082`. `localhost` probes then hit the wrong process.

```powershell
netstat -ano | findstr :5082
```

Stop the stale local process, or set `CONEXUS_HOST_PORT` in `.env` to an unused port.

**Operator rule:** before Docker-local health checks, stop local services on **5081**, **5082**, **5083**, **5175**, or override host ports in `.env`.

### Health probes

```text
/health, /health/live, /live  → liveness (Docker startup / wait scripts)
/ready                        → strict readiness (may be 503 before bootstrap)
```

### Postgres host port

Default in `.env.example` is **55433** (avoids collision with local Postgres on **5432**). Change `POSTGRES_HOST_PORT` in `.env` if needed.

## Docs

Canonical plan: `docs/environments/docker-local-working-system/`.
