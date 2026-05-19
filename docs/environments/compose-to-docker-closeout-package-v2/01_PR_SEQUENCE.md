# PR Sequence

## Docker-local closeout path — CLOSED

| Order | PR | Status | Evidence |
|---:|---|---|---|
| 1 | `ENV-COMPOSE-001` | DONE / PASS | `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md` |
| 2 | `ENV-DOCKER-RUN-001` | DONE / PASS | `docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| 3 | `ENV-DOCKER-FE-001` | DONE / PASS | `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| 4 | `ENV-DOCKER-CLOSEOUT-001` | DONE / PASS | `docs/evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md` |

This closes the first Dockerized local working system. It is not production readiness.

## Post-Docker-local hardening — CLOSED

Items 1–12: see historical table below. Final additions:

| Order | PR | Status | Evidence |
|---:|---|---|---|
| 13 | `CI-COST-001` | DONE / PASS | `docs/evidence/CI_COST_001_COST_CONTROL_EVIDENCE.md` |
| 14 | `POST-DOCKER-HARDENING-CLOSEOUT-001` | DONE / PASS | `docs/evidence/POST_DOCKER_HARDENING_CLOSEOUT_001_EVIDENCE.md` |

Post-closeout hardening package closed 2026-05-19 (includes `CI-COST-001`). Not production readiness.

**Active:** none — choose strategic track in `docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md`.

## Historical — Post-Docker-local hardening sequence

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
| 8 | `FE-AUDIT-FIXTURES-001` | DONE | Fixture/live boundary audit — `ontogony-frontend/docs/evidence/FE_AUDIT_FIXTURES_001_FIXTURE_LIVE_BOUNDARY_EVIDENCE.md` |
| 9 | `FE-TEST-REPLAY-001` | DONE | Replay/test improvements — `ontogony-frontend/docs/evidence/FE_TEST_REPLAY_001_REPLAY_TEST_EVIDENCE.md` |
| 10 | `FE-HYGIENE-CONFIG-001` | DONE | Frontend config hygiene — `ontogony-frontend/docs/evidence/FE_HYGIENE_CONFIG_001_FRONTEND_CONFIG_EVIDENCE.md` |
| 11 | `UI-PACKAGING-STATUS-001` | DONE | `@ontogony/ui` packaging — `ontogony-ui/docs/development/PACKAGING_STATUS.md` |
| 12 | `TERMINOLOGY-CLEANUP-001` | DONE | Glossary — `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md` |

## Strategic next (not part of closed hardening package)

Planning: `docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md`

## Optional later work

| Item | Timing |
|---|---|
| `ENV-REAL-PROVIDER-001` | Optional backend smoke; explicit opt-in only |
| `PROD-READINESS-*` | Separate future program |
| Branch protection | Manual when ready — aggregate checks only (`ci-complete` / `check-full`) |

## Sequence rule (historical)

Do not move broad hardening ahead of `ENV-DOCKER-CLOSEOUT-001` unless a hard blocker prevents the first Dockerized local working system from functioning. **That gate is satisfied; the closeout path is closed.**
