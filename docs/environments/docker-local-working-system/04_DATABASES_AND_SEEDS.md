# Databases and seeds — Docker local working system

**Boundary:** first Dockerized local working system. Use **migrations** for schemas; do not apply raw schema SQL except bootstrap users/DBs.

## Logical databases

```text
allagma_local
kanon_local
conexus_local
```

## Service users (development only)

```text
allagma_local  / allagma_local_pw
kanon_local    / kanon_local_pw
conexus_local  / conexus_local_pw
```

Bootstrap SQL stub (implementation: ENV-DB-001):

```text
allagma-dotnet/docs/environments/working-local-environment-complete-package/06_SCRIPT_STUBS/001-create-databases-and-users.sql
```

Planned mount: `docker/local-working-system/postgres/init/` → `/docker-entrypoint-initdb.d`.

## Migration responsibility

| Service | Mechanism |
| --- | --- |
| Kanon | `Kanon__Persistence__Postgres__ApplyMigrationsOnStartup=true` + shipped SQL migrations |
| Conexus | EF migrations on startup when `ConnectionStrings:ConexusPostgres` is set |
| Allagma | EF migrations on startup when `Allagma:Persistence:Mode=Postgres` |

Do not bypass migrations with hand-written schema dumps.

## Seed outcomes (ENV-SEED-001)

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

## Parity with script-based program

The closed **ENV-PG-001** path proved Allagma eval durability on a single Postgres database (`allagma_e2e` on port `55432`). The Docker program extends that model to **three logical DBs** on one container with full cross-service compose networking.

## Not in scope for DB bootstrap PR

- Production backup/restore
- Connection pooling tuning
- Read replicas
- Rotating credentials
