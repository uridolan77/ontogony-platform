# First Docker-local working system — known limitations

**Date:** 2026-05-19  
**Scope:** ENV-DOCKER-CLOSEOUT-001 closeout (Docker/Postgres local program)

**This is first Dockerized local working system, not production readiness.**

These items are **accepted** for this milestone. They are not blockers for closing ENV-DOCKER-CLOSEOUT-001.

## Program scope

- **Docker Compose only** — not Kubernetes, Azure Container Apps, or cloud IaC.
- **Single PostgreSQL container** with three logical databases (not three Postgres instances).
- **Development credentials only** (`*_local_pw`, `cx-dev-key-change-me`) — never use in staging/production.
- **Fake/local Conexus provider** in default compose; no real provider mode unless an explicit later PR adds it.

## Operations

- **Host port collisions:** local dev APIs on `127.0.0.1:5081`–`5083` can shadow Docker on Windows when probes use `localhost`. Stop host processes or override ports in `.env`.
- **Postgres host port:** `.env.example` defaults to **55433**; override `POSTGRES_HOST_PORT` if taken.
- **Conexus `/ready`:** stricter than liveness; **503** pre-bootstrap is expected; **200** after seed is also valid.
- **Seed idempotency:** re-run seed on a bootstrapped volume may fail key checks; use `reset-local-working-system.ps1 -Force` for clean re-seed.
- No TLS between services or at ingress.
- No production identity (OIDC, service principals).
- No centralized observability stack beyond dev defaults.
- No backup/restore or disaster-recovery plan.

## Frontend

- `VITE_*` values are **compile-time** in the frontend image; runtime config injection deferred.
- Live browser walkthrough (banners, secret hygiene) is **manual** per `DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md`.
- Docker guided runner uses `-SkipFrontend` by default; FE verified separately (ENV-DOCKER-FE-001).

## Parity vs script-based program (closed)

- Prior [FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md](./FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md) used host processes and optional InMemory Allagma; Docker program uses Postgres persistence by default.
- ENV-PG-001 script-mode proof on port `55432` is separate; do not conflate with compose Postgres host port **55433**.

## API and data (carried forward)

- No global eval list API; dashboards sample recent runs.
- No cross-run eval trend API.
- Baseline `topologyAuthorizationDecisionId` null by design on `single_workflow`.
- Run GET may not embed eval/comparison IDs; harness creates comparison separately.

## Safety (non-limitations — do not waive)

| Rule | Status |
| --- | --- |
| Real external execution | Disabled |
| Conexus provider | Fake/local first |
| Manual eval POST | `ManualWriteEnabled` + non-production only |
| Secrets / raw prompts in UI or reports | Forbidden |

## Source alignment

Canonical operator limitations: `docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md`.
