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

**Active:** none — post-closeout hardening in progress; next backlog item **`FE-AUDIT-FIXTURES-001`**.

Planning detail: `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md`

Recommended order:

| Order | PR | Status | Focus |
|---:|---|---|---|
| 1 | `CONEXUS-PERSIST-001` | DONE | Conexus persistence/bootstrap/operator visibility |
| 2 | `CONEXUS-PERSIST-002` | DONE | Conexus migration/bootstrap validation |
| 3 | `CONEXUS-PERSIST-003` | DONE | Conexus restart/durability regression checks |
| 4 | `KANON-OP-001` | DONE | Operator-readable Kanon topology/decision evidence |
| 5 | `KANON-OP-002` | DONE | Kanon operational diagnostics |
| 6 | `TRACE-CONTRACT-001` | DONE | Cross-service trace/correlation contract |
| 7 | `FE-HARDEN-001` | DONE | Frontend hardening beyond walkthrough |
| 8 | `FE-AUDIT-FIXTURES-001` | backlog | Fixture/live boundary audit |
| 9 | `FE-TEST-REPLAY-001` | backlog | Replay/test improvements |
| 10 | `FE-HYGIENE-CONFIG-001` | backlog | Frontend config hygiene |
| 11 | `UI-PACKAGING-STATUS-001` | backlog | `@ontogony/ui` packaging status |
| 12 | `TERMINOLOGY-CLEANUP-001` | backlog | Operator/doc terminology cleanup |

## Optional later work

| Item | Timing |
|---|---|
| `ENV-REAL-PROVIDER-001` | After local Docker hardening; explicit opt-in only |
| `PROD-READINESS-*` | Separate future program |
| `CI-COST-001` | Optional; if GitHub Actions cost is painful, run before the hardening backlog |

## Sequence rule (historical)

Do not move broad hardening ahead of `ENV-DOCKER-CLOSEOUT-001` unless a hard blocker prevents the first Dockerized local working system from functioning. **That gate is satisfied; the closeout path is closed.**
