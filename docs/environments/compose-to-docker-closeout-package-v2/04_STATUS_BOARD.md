# Status Board

## Completed / accepted before this package

| Item | Status |
|---|---|
| ENV-CLOSEOUT-001 | done |
| ENV-DOCKER-001 | done |
| ENV-DOCKER-002 | done / Dockerfile PRs expected merged or mergeable |
| ENV-DB-001 | done |
| ENV-SEED-001 | done for host-local API deterministic seed verification |

## Docker-local closeout path — CLOSED

| Item | Status | Evidence / notes |
|---|---|---|
| ENV-COMPOSE-001 | DONE / PASS | `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md` |
| ENV-DOCKER-RUN-001 | DONE / PASS | `docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| ENV-DOCKER-FE-001 | DONE / PASS | `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| ENV-DOCKER-CLOSEOUT-001 | DONE / PASS | `docs/evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md` — milestone closed 2026-05-19 |

First Dockerized local working system: **CLOSED / PASS**. Not production readiness.

## Current phase — Post-Docker-local hardening

| Item | Status | Notes |
|---|---|---|
| CONEXUS-PERSIST-001 | **DONE** | Operator docs — platform PR #7, conexus PR #13 |
| CONEXUS-PERSIST-002 | **DONE** | Validation automation — platform PR #8 |
| CONEXUS-PERSIST-003 | **DONE** | Durability regression — PR #9 merged |
| **KANON-OP-001** | **active** | `inspect-kanon-topology-evidence.ps1` |
| KANON-OP-002 | backlog | Kanon operational diagnostics |
| TRACE-CONTRACT-001 | backlog | Cross-service trace/correlation contract |
| FE-HARDEN-001 | backlog | Frontend hardening beyond walkthrough |
| FE-AUDIT-FIXTURES-001 | backlog | Fixture/live boundary audit |
| FE-TEST-REPLAY-001 | backlog | Replay/test improvements |
| FE-HYGIENE-CONFIG-001 | backlog | Frontend config hygiene |
| UI-PACKAGING-STATUS-001 | backlog | `@ontogony/ui` packaging status |
| TERMINOLOGY-CLEANUP-001 | backlog | Operator/doc terminology cleanup |

See `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md` and `01_PR_SEQUENCE.md`.

## Boundary notes

- ENV-SEED-001 proves host-local API seed behavior.
- ENV-COMPOSE-001 proves Docker startup and container orchestration.
- ENV-DOCKER-RUN-001 proves the full Dockerized main flow.
- Conexus `/ready` may fail before bootstrap by design.
