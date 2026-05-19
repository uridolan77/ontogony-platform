# PR Sequence

## Docker-local closeout path — CLOSED

| Order | PR | Status | Evidence |
|---:|---|---|---|
| 1 | `ENV-COMPOSE-001` | DONE / PASS | `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md` |
| 2 | `ENV-DOCKER-RUN-001` | DONE / PASS | `docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| 3 | `ENV-DOCKER-FE-001` | DONE / PASS | `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| 4 | `ENV-DOCKER-CLOSEOUT-001` | DONE / PASS | `docs/evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md` |

This closes the first Dockerized local working system. It is not production readiness.

## Current phase — Post-Docker-local hardening

Post-closeout hardening is separate from the closed Docker-local milestone.

**Next decision:** choose first post-closeout hardening PR — recommended: **`CONEXUS-PERSIST-001`**.

Planning detail: `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md`

Recommended order:

| Order | PR | Focus |
|---:|---|---|
| 1 | `CONEXUS-PERSIST-001` | Conexus persistence/bootstrap/operator visibility |
| 2 | `KANON-OP-001` | Operator-readable Kanon topology/decision evidence |
| 3 | `KANON-OP-002` | Kanon operational diagnostics |
| 4 | `CONEXUS-PERSIST-002` | Conexus migration/bootstrap validation |
| 5 | `CONEXUS-PERSIST-003` | Conexus restart/durability regression checks |
| 6 | `TRACE-CONTRACT-001` | Cross-service trace/correlation contract |
| 7 | `FE-HARDEN-001` | Frontend hardening beyond walkthrough |
| 8 | `FE-AUDIT-FIXTURES-001` | Fixture/live boundary audit |
| 9 | `FE-TEST-REPLAY-001` | Replay/test improvements |
| 10 | `FE-HYGIENE-CONFIG-001` | Frontend config hygiene |
| 11 | `UI-PACKAGING-STATUS-001` | `@ontogony/ui` packaging status |
| 12 | `TERMINOLOGY-CLEANUP-001` | Operator/doc terminology cleanup |

## Optional later work

| Item | Timing |
|---|---|
| `ENV-REAL-PROVIDER-001` | After local Docker hardening; explicit opt-in only |
| `PROD-READINESS-*` | Separate future program |
| `CI-COST-001` | Optional; if GitHub Actions cost is painful, run before the hardening backlog |

## Sequence rule (historical)

Do not move broad hardening ahead of `ENV-DOCKER-CLOSEOUT-001` unless a hard blocker prevents the first Dockerized local working system from functioning. **That gate is satisfied; the closeout path is closed.**
