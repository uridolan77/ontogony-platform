# Current State Baseline

## Current known truths

- The script-based local operator program is closed.
- Dockerfile readiness exists across service repos.
- PostgreSQL DB bootstrap is done.
- ENV-SEED-001 provides a deterministic seed/verify script using API/domain paths, not raw schema SQL.
- Conexus now separates liveness from strict readiness:
  - `/health`, `/health/live`, `/live` are lightweight liveness.
  - `/ready` remains strict readiness.
- Platform wait script should use Conexus `/health/live`.

## Local workspace

```text
C:\dev\
  ontogony-platform\
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

## Ports

```text
Postgres host: 55433 → container 5432
Kanon host: 5081 → container 8080
Conexus host: 5082 → container 8080
Allagma host: 5083 → container 8080
Frontend host: 5175 → container 8080
```
