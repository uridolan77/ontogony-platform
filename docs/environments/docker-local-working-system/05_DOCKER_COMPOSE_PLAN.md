# Docker Compose plan — Docker local working system

**Status:** Plan only (ENV-DOCKER-001). Compose file lands in **ENV-COMPOSE-001**.

## Planned location

```text
ontogony-platform/docker/local-working-system/docker-compose.yml
ontogony-platform/docker/local-working-system/.env.example
ontogony-platform/docker/local-working-system/postgres/init/   # ENV-DB-001
```

Reference stub (not executed until COMPOSE PR):

```text
allagma-dotnet/docs/environments/working-local-environment-complete-package/06_SCRIPT_STUBS/docker-compose.yml
```

## Build contexts (relative to compose file)

From `docker/local-working-system/`:

```yaml
kanon-api:
  build:
    context: ../../../kanon-dotnet

conexus-api:
  build:
    context: ../../../conexus-dotnet

allagma-api:
  build:
    context: ../../../allagma-dotnet

ontogony-frontend:
  build:
    context: ../../../ontogony-frontend
```

Requires sibling repos at `C:\dev\` per `01_WORKSPACE_LAYOUT.md`.

## Services (v1)

| Service | Image | Host port | Container port |
| --- | --- | --- | --- |
| `postgres` | `postgres:16` | `5432` (or alternate if conflict) | `5432` |
| `kanon-api` | build `kanon-dotnet` | `5081` | `8080` |
| `conexus-api` | build `conexus-dotnet` | `5082` | `8080` |
| `allagma-api` | build `allagma-dotnet` | `5083` | `8080` |
| `ontogony-frontend` | build `ontogony-frontend` | `5175` or `5080` | `8080` |

Named volume: `ontogony_postgres_data` for Postgres data dir.

## Depends_on and health

```text
postgres        → healthcheck: pg_isready
kanon-api       → depends_on postgres (healthy)
conexus-api     → depends_on postgres (healthy)
allagma-api     → depends_on postgres (healthy), kanon-api, conexus-api (started)
ontogony-frontend → depends_on allagma-api (healthy) — exact gates in ENV-COMPOSE-001
```

Wait for:

```text
postgres healthy
kanon-api     GET /health
conexus-api   GET /health
allagma-api   GET /health
```

## Environment injection

Use compose `environment:` or `.env` file derived from `03_EXACT_SETTINGS.md`. **Do not** commit `.env` with real provider keys.

`.env.example` should list every variable with placeholder dev values and point to `03_EXACT_SETTINGS.md`.

## Operator scripts (later PRs)

Stubs in planning package (implementation not in ENV-DOCKER-001):

```text
06_SCRIPT_STUBS/start-local-working-system.ps1
06_SCRIPT_STUBS/wait-local-working-system.ps1
06_SCRIPT_STUBS/reset-local-working-system.ps1
```

ENV-DOCKER-RUN-001 will wire guided main flow against the Docker stack.

## Prerequisites before `docker compose up`

| PR | Delivers |
| --- | --- |
| ENV-DOCKER-002 | Dockerfiles + `.dockerignore` per repo |
| ENV-DB-001 | `postgres/init` SQL |
| ENV-SEED-001 | Bootstrap rows after migrations |
| ENV-COMPOSE-001 | Working `docker-compose.yml` |
