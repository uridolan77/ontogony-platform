# Docker local working system — compose tree

**Boundary:** first Dockerized local working system. Development credentials only — not for staging or production.

## Layout

```text
docker/local-working-system/
  README.md
  postgres/init/          # ENV-DB-001 — first-init bootstrap SQL
  scripts/                # operator helpers (no full compose until ENV-COMPOSE-001)
```

Planned in later PRs:

```text
  docker-compose.yml      # ENV-COMPOSE-001
  .env.example            # ENV-COMPOSE-001
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

## Docs

Canonical plan: `docs/environments/docker-local-working-system/`.
