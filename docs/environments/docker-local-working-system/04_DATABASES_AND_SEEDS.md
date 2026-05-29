# Databases and seeds — Docker local working system

**Boundary:** first Dockerized local working system. Use **migrations** for schemas; do not apply raw schema SQL except bootstrap users/DBs.

## Logical databases

```text
allagma_local
kanon_local
conexus_local
aisthesis_local
metabole_local
```

## Service users (development only)

```text
allagma_local   / allagma_local_pw
kanon_local     / kanon_local_pw
conexus_local   / conexus_local_pw
aisthesis_local / aisthesis_local_pw
metabole_local  / metabole_local_pw
```

Bootstrap SQL (ENV-DB-001):

```text
ontogony-platform/docker/local-working-system/postgres/init/001-create-databases-and-users.sql
```

Mount `postgres/init` read-only → `/docker-entrypoint-initdb.d` (first init only).

Verify: `docker/local-working-system/scripts/verify-postgres-bootstrap.ps1 -Recreate`

## Migration responsibility

| Service | Mechanism |
| --- | --- |
| Kanon | `Kanon__Persistence__Postgres__ApplyMigrationsOnStartup=true` + shipped SQL migrations |
| Conexus | EF migrations on startup when `ConnectionStrings:ConexusPostgres` is set |
| Allagma | EF migrations on startup when `Allagma:Persistence:Mode=Postgres` |
| Aisthesis | EF migrations on startup when `ConnectionStrings:Aisthesis` is set |
| Metabole | Foundation schema migrator on startup when `Metabole:Storage=Postgres` |

Do not bypass migrations with hand-written schema dumps.

## Seed outcomes (ENV-SEED-001)

Seed/bootstrap script (runtime API path):

```text
ontogony-platform/docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1
```

Runs after APIs are healthy and migrations are applied.

### Conexus

```text
dev project exists
project API key cx-dev-key-change-me works
fake provider route is available
route-decision evidence is emitted on model calls
```

### Kanon

```text
topology policy defaults exist
single_workflow low-risk path does not require topology authorization decision (null by design)
centralized_orchestrator override path creates topology authorization decision
```

### Allagma

```text
Postgres mode active
evaluation persistence active
manual eval write enabled in Development only
real external execution blocked
```

Evidence report output:

```text
ontogony-platform/docker/local-working-system/artifacts/env-seed-001-report.json
```

## Parity with script-based program

The closed **ENV-PG-001** path proved Allagma eval durability on a single Postgres database (`allagma_e2e` on port `55432`). The Docker program extends that model to **five logical DBs** on one container with full cross-service compose networking.

## Not in scope for DB bootstrap PR

- Production backup/restore
- Connection pooling tuning
- Read replicas
- Rotating credentials
