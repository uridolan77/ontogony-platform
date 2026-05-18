# Known limitations — Docker local working system

**Boundary: first Dockerized local working system, not production readiness.**

Items below are **accepted** for v1. They are not excuses to relax safety gates.

## Program scope

- Single PostgreSQL container with three logical databases (not three Postgres instances).
- Development credentials only (`*_local_pw`) — never use in staging/production.
- Fake/local Conexus provider first; no real provider mode in default compose.
- Manual eval write only in Development with `ManualWriteEnabled`.

## Operations

- No TLS/certificates between services or at ingress.
- No production identity (OIDC, service principals).
- No production observability stack (centralized metrics/tracing beyond dev defaults).
- No backup/restore or disaster-recovery plan.
- No cloud deployment manifest (Kubernetes, Azure Container Apps, etc.).

## Parity gaps vs script-based program

- Script-based stack (closed) used process-per-window on host; Docker uses container lifecycle.
- ENV-PG-001 used port `55432` and database `allagma_e2e`; Docker program uses `5432` (or mapped port) and three DBs.
- Live browser walkthrough remains **manual** until ENV-DOCKER-FE-001.

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

## Next implementation PRs

| PR | Delivers |
| --- | --- |
| ENV-DOCKER-RUN-001 | Dockerized guided main flow |
| ENV-DOCKER-FE-001 | Frontend Docker walkthrough |
| ENV-DOCKER-CLOSEOUT-001 | Docker phase closeout |
