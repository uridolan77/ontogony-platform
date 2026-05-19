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
| KANON-OP-001 | **DONE** | Topology evidence — PR #10 / kanon #2 |
| **KANON-OP-002** | **DONE** | `diagnose-kanon-topology-ops.ps1` |
| **TRACE-CONTRACT-001** | **DONE** | `inspect-trace-correlation-evidence.ps1` |
| **FE-HARDEN-001** | **DONE** | Frontend HTTP inspect + Playwright; `ontogony-frontend/docs/evidence/FE_HARDEN_001_FRONTEND_HARDENING_EVIDENCE.md` |
| **FE-AUDIT-FIXTURES-001** | **DONE** | Fixture/live catalog + `fixtures:check` + E2E; `ontogony-frontend/docs/evidence/FE_AUDIT_FIXTURES_001_FIXTURE_LIVE_BOUNDARY_EVIDENCE.md` |
| **FE-TEST-REPLAY-001** | **DONE** | Replay catalog + `replay:check` + E2E; `ontogony-frontend/docs/evidence/FE_TEST_REPLAY_001_REPLAY_TEST_EVIDENCE.md` |
| **FE-HYGIENE-CONFIG-001** | **DONE** | `VITE_*` catalog + `config:check`; `ontogony-frontend/docs/evidence/FE_HYGIENE_CONFIG_001_FRONTEND_CONFIG_EVIDENCE.md` |
| **UI-PACKAGING-STATUS-001** | **DONE** | `@ontogony/ui` packaging status — `ontogony-ui/docs/development/PACKAGING_STATUS.md`, `ontogony-ui/docs/evidence/UI_PACKAGING_STATUS_001_EVIDENCE.md` |
| **TERMINOLOGY-CLEANUP-001** | **DONE** | Glossary + doc cleanup — `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`, `docs/evidence/TERMINOLOGY_CLEANUP_001_EVIDENCE.md` |

See `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md` and `01_PR_SEQUENCE.md`.

## Boundary notes

- ENV-SEED-001 proves host-local API seed behavior.
- ENV-COMPOSE-001 proves Docker startup and container orchestration.
- ENV-DOCKER-RUN-001 proves the full Dockerized main flow.
- Conexus `/ready` may fail before bootstrap by design.
