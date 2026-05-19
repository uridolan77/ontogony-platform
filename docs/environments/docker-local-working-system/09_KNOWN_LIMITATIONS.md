# Known limitations — Docker local working system

**Boundary: first Dockerized local working system, not production readiness.**

Items below are **accepted** for v1. They are not excuses to relax safety gates.

## Program scope

- Single PostgreSQL container with three logical databases (not three Postgres instances).
- Development credentials only (`*_local_pw`) — never use in staging/production.
- Fake/local Conexus provider first; no real provider mode in default compose.
- Manual eval write only in Development with `ManualWriteEnabled`.

## Operations

- **Host port collisions:** local dev APIs (e.g. `Conexus.Api` on `127.0.0.1:5082`) can shadow Docker port mappings on Windows when probes use `localhost`. Stop local processes on **5081–5083**, **5175**, or override ports in `docker/local-working-system/.env`. See `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md` (port-collision note).
- **Postgres host port:** `.env.example` defaults to **55433**; override `POSTGRES_HOST_PORT` if a local Postgres already uses that port.
- No TLS/certificates between services or at ingress.
- No production identity (OIDC, service principals).
- No production observability stack (centralized metrics/tracing beyond dev defaults).
- No backup/restore or disaster-recovery plan.
- No cloud deployment manifest (Kubernetes, Azure Container Apps, etc.).

## Parity gaps vs script-based program

- Script-based stack (closed) used process-per-window on host; Docker uses container lifecycle.
- ENV-PG-001 used port `55432` and database `allagma_e2e`; Docker program uses host-mapped Postgres (default **55433** in `.env.example`) and three logical DBs inside the container.
- Live browser walkthrough remains **manual** (see `ontogony-frontend/docs/development/DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md`).

## Carried from eval / sanity alignment

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

## Program status

Docker local working system **closed** (ENV-DOCKER-CLOSEOUT-001). Post-closeout hardening: `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md`.
