# First Docker-local working system — next steps

**Date:** 2026-05-19  
**After:** ENV-DOCKER-CLOSEOUT-001 (Docker local working system **CLOSED / PASS**)

**This is the first Dockerized local working system, not production readiness.**

Post-closeout work is **explicitly separate** from the closeout criteria. Do not treat these items as blockers for the closed milestone unless they become direct production blockers for your team.

## Current phase

**Post-Docker-local hardening** — the minimal Docker-local closeout path is closed. The active decision is which hardening PR to run first.

**Active PR:** `KANON-OP-001` — operator-readable Kanon topology/decision evidence.

| Doc | Path |
| --- | --- |
| Inspect script | `docker/local-working-system/scripts/inspect-kanon-topology-evidence.ps1` |
| Acceptance spec | `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/KANON-OP-001.md` |
| Kanon operator doc | `kanon-dotnet/docs/operators/TOPOLOGY_DECISION_EVIDENCE.md` |

**Completed:** `CONEXUS-PERSIST-001` (operator docs — PR #7/#13), `CONEXUS-PERSIST-002` (validation automation — PR #8), `CONEXUS-PERSIST-003` (durability regression — PR #9).

## Post-closeout hardening backlog

Planning package: `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/`

Sequence: `docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md`

| Order | PR | Focus |
|---:|---|---|
| 1 | `CONEXUS-PERSIST-001` | DONE — Conexus persistence/bootstrap/operator visibility |
| 2 | `CONEXUS-PERSIST-002` | DONE — Conexus migration/bootstrap validation |
| 3 | `CONEXUS-PERSIST-003` | DONE — Conexus restart/durability regression |
| 4 | `KANON-OP-001` | **active** — Operator-readable Kanon topology/decision evidence |
| 5 | `KANON-OP-002` | Kanon operational diagnostics |
| 6 | `TRACE-CONTRACT-001` | Cross-service trace/correlation contract |
| 7 | `FE-HARDEN-001` | Frontend hardening beyond walkthrough |
| 8 | `FE-AUDIT-FIXTURES-001` | Fixture/live boundary audit |
| 9 | `FE-TEST-REPLAY-001` | Replay/test improvements |
| 10 | `FE-HYGIENE-CONFIG-001` | Frontend config hygiene |
| 11 | `UI-PACKAGING-STATUS-001` | `@ontogony/ui` packaging status |
| 12 | `TERMINOLOGY-CLEANUP-001` | Operator/doc terminology cleanup |

## Optional (not blocking)

| Item | Notes |
| --- | --- |
| `CI-COST-001` | If GitHub Actions cost is painful, do before the hardening backlog |
| ENV-REAL-PROVIDER-001 | Backend-only real provider smoke; explicit keys; still not production |
| Runtime Vite config injection | Deferred from ENV-DOCKER-002 / compose frontend image |
| Cloud deploy manifests | Out of scope for first Docker-local program |
| `PROD-READINESS-*` | Separate future program |

## Parallel platform work (unchanged)

- `docs/planning/eval-durability-to-first-sanity-current/`
- `docs/alignment/eval-full-sanity-alignment/`
- `allagma-dotnet/docs/releases/FIRST_FULL_SANITY_CLOSEOUT.md` (eval milestone; separate from ENV)

## Operator entry points

| Need | Start here |
| --- | --- |
| Docker compose stack | `docker/local-working-system/README.md` |
| Plan + acceptance | `docs/environments/docker-local-working-system/` |
| Script-based stack (closed) | `docs/releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md` |
| Frontend Docker walkthrough | `ontogony-frontend/docs/development/DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md` |

## Required statement (carry forward)

```text
This package supports the first working local environment and first Dockerized local working system. It is not production readiness.
```
