# ENV-COMPOSE-001 — Docker Compose orchestration evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **ACCEPTED** — DONE / PASS  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` only:

- add working compose root artifacts (`docker-compose.yml`, `.env.example`)
- add compose operator scripts (`start`, `wait`, `reset`)
- update Docker local working-system docs and sequence status
- record targeted validation output for compose rendering + runtime startup behavior

No production hardening, no real provider keys, no real external execution enablement.

## Delivered

```text
docker/local-working-system/docker-compose.yml
docker/local-working-system/.env.example
docker/local-working-system/scripts/start-local-working-system.ps1
docker/local-working-system/scripts/wait-local-working-system.ps1
docker/local-working-system/scripts/reset-local-working-system.ps1
docker/local-working-system/README.md
docs/environments/docker-local-working-system/00_MANIFEST.json
docs/environments/docker-local-working-system/05_DOCKER_COMPOSE_PLAN.md
docs/environments/docker-local-working-system/06_MAIN_USE_FLOW.md
docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md
docs/environments/docker-local-working-system/README.md
docs/environments/README.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md
```

## Health endpoint contract (Conexus)

Use for Docker startup vs strict readiness:

```text
Use /health or /health/live for Docker startup and wait scripts (liveness).
Use /ready for strict readiness (provider credentials, durable stores).
```

Conexus maps:

```text
/health       → lightweight liveness
/health/live  → lightweight liveness
/live         → lightweight liveness
/ready        → strict ready-tag health checks
```

## Port-collision note (root cause of false 404)

During closeout validation, `/health/live` returned **404** while `docker compose ps` showed `conexus-api` **healthy**. Investigation showed this was **not** a stale Docker image.

**Root cause:** host port collision / `localhost` resolution on Windows.

```text
A stale local Conexus.Api process was bound to 127.0.0.1:5082 while Docker Conexus was bound to 0.0.0.0:5082. On Windows, localhost probes hit the local process first, producing false 404s for /health/live. After stopping the local process, Docker Conexus returned /health/live=200 and /ready=503 pre-bootstrap as expected.
```

**Operator rule:**

```text
Before running Docker-local health checks, stop local services using ports 5081, 5082, 5083, 5175, or override host ports in docker/local-working-system/.env.
```

**Troubleshooting:**

```text
If /health/live returns 404 but docker compose ps shows conexus-api healthy, check for a local process already listening on 5082:

netstat -ano | findstr :5082

Stop the stale local Conexus.Api process or change CONEXUS_HOST_PORT in .env.
```

## Postgres host port default

`.env.example` default:

```text
POSTGRES_HOST_PORT=55433
```

Rationale: fewer collisions with a local Postgres on **5432**. Container-internal Postgres remains **5432**; only the host mapping changes. Override in `.env` if **55433** is taken.

Evidence run below used **5432** on the host (port was free at validation time).

## Closeout commands run

```powershell
cd C:\dev\ontogony-platform

docker compose --env-file .\docker\local-working-system\.env.example `
  -f .\docker\local-working-system\docker-compose.yml config

.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\wait-local-working-system.ps1
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Closeout results (2026-05-19)

| Check | Result |
| --- | --- |
| `docker compose config` | **PASS** |
| postgres healthy | **PASS** |
| kanon-api `/health` | **PASS** (`http://localhost:5081/health`) |
| conexus-api `/health/live` | **PASS** (`http://localhost:5082/health/live`) — after stopping stale local Conexus on 5082 |
| allagma-api `/health` | **PASS** (`http://localhost:5083/health`) |
| ontogony-frontend `/` | **PASS** (`http://localhost:5175/`) |
| Conexus `/ready` pre-bootstrap (strict) | **PASS** (`503` expected before seed/bootstrap) |
| Conexus `/health` liveness | **PASS** (`200`) |
| `seed-and-verify-local-working-system.ps1` | **PASS** (initial run) — report: `docker/local-working-system/artifacts/env-seed-001-report.json`. Re-run after bootstrap may fail on idempotent key checks; use `reset-local-working-system.ps1 -Force` for a clean re-seed. |
| Real provider keys | **no** |
| Real external execution | **no** |
| Production readiness claim | **no** |

### Wait script output (representative)

```text
PASS postgres healthy.
PASS kanon-api healthy: http://localhost:5081/health
PASS conexus-api healthy: http://localhost:5082/health/live
PASS allagma-api healthy: http://localhost:5083/health
PASS ontogony-frontend healthy: http://localhost:5175/
All requested services are healthy.
```

## Safety

| Check | Status |
| --- | --- |
| Real provider keys committed | **no** |
| Dev placeholder credentials only | **yes** |
| Real external execution enabled | **no change** |
| Production readiness claim | **no** |

## Known limitations

- Conexus `/ready` is intentionally stricter than liveness and can remain **503** until provider/alias bootstrap completes; **200** after seed is also valid when strict checks pass.
- ENV-SEED-001 proves fake-provider local usability via host-local API calls; container DNS wiring is compose-backed; restart-survival proof remains **ENV-DOCKER-RUN-001**.
- Host port collisions with locally running APIs are an operator concern (see port-collision note above).

## Next step

**ENV-DOCKER-RUN-001** — Dockerized guided main flow, including Allagma restart and persistence-after-restart verification.
