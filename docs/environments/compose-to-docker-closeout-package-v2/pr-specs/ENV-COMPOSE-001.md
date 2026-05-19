# ENV-COMPOSE-001 — Docker Compose orchestration

Repo:

```text
ontogony-platform
```

## Goal

Add actual Docker Compose orchestration for the first Dockerized local working system.

## Add/update

```text
docker/local-working-system/docker-compose.yml
docker/local-working-system/.env.example
docker/local-working-system/README.md
docker/local-working-system/scripts/start-local-working-system.ps1
docker/local-working-system/scripts/stop-local-working-system.ps1
docker/local-working-system/scripts/wait-local-working-system.ps1
docker/local-working-system/scripts/reset-local-working-system.ps1
docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md
```

## Services

```text
postgres
kanon-api
conexus-api
allagma-api
ontogony-frontend optional but preferred if image is ready
```

## Required health behavior

Conexus startup health must use:

```text
/health/live
```

Conexus strict readiness remains:

```text
/ready
```

`/ready` may return 503 before bootstrap and this is expected.

## Acceptance

```text
docker compose config PASS
postgres healthy
Kanon /health PASS
Conexus /health/live PASS
Allagma /health PASS
Conexus /ready strict before bootstrap
no real provider keys
no real external execution
safe reset behavior
```

## Not in scope

```text
full guided main flow
Allagma restart durability proof
post-closeout hardening
real provider mode
```
