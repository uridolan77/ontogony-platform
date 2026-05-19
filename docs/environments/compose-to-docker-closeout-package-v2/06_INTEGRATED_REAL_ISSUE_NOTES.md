# Integrated Real Issue Notes

## Conexus provider credentials readiness

Problem observed:

```text
conexus-api /health returned 503 due conexus_provider_credentials
```

Correct fix:

```text
Do not weaken provider readiness.
Use lightweight liveness for Docker startup.
Keep /ready strict.
```

Current contract:

```text
/health       liveness
/health/live  liveness
/live         liveness
/ready        strict readiness
```

Package implication:

- Compose healthcheck for Conexus must use `/health/live`.
- Wait script must use `/health/live`.
- Evidence should explicitly prove `/ready` remains strict before bootstrap.
- Seed/guided flow proves fake-provider route usability.

## Allagma connection-string key

Use:

```text
Allagma__Persistence__Mode=Postgres
ConnectionStrings__Allagma=Host=postgres;Port=5432;Database=allagma_local;Username=allagma_local;Password=allagma_local_pw
```

Do not use `ConnectionStrings__AllagmaPostgres` unless the repo explicitly adds that key later.

## Kanon connection-string key

Use:

```text
Kanon__Persistence__Postgres__ConnectionString=Host=postgres;Port=5432;Database=kanon_local;Username=kanon_local;Password=kanon_local_pw
Kanon__Persistence__Postgres__ApplyMigrationsOnStartup=true
```

## Conexus connection-string key

Use:

```text
ConnectionStrings__ConexusPostgres=Host=postgres;Port=5432;Database=conexus_local;Username=conexus_local;Password=conexus_local_pw
```

No `Conexus__Persistence__Mode` toggle is required unless added later.
