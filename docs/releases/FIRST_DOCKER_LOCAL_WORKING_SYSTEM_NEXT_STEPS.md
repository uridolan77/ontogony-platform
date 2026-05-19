# First Docker-local working system — next steps

**Date:** 2026-05-19  
**After:** ENV-DOCKER-CLOSEOUT-001 (Docker local working system **CLOSED / PASS**)

**This is the first Dockerized local working system, not production readiness.**

Post-closeout work is **explicitly separate** from the closeout criteria. Do not treat these items as blockers for the closed milestone unless they become direct production blockers for your team.

## Current phase

**Post-Docker-local hardening — CLOSED** (`POST-DOCKER-HARDENING-CLOSEOUT-001`, 2026-05-19).

For strategic options after hardening, see **[POST_DOCKER_HARDENING_NEXT_STEPS.md](./POST_DOCKER_HARDENING_NEXT_STEPS.md)**.

| Closeout | Path |
| --- | --- |
| Hardening closeout | `docs/releases/POST_DOCKER_HARDENING_CLOSEOUT.md` |
| Hardening scorecard | `docs/releases/POST_DOCKER_HARDENING_SCORECARD.md` |
| Terminology glossary | `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md` |

## Post-closeout hardening backlog (complete)

Planning package: `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/`

Sequence: `docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md`

| Order | PR | Focus |
|---:|---|---|
| 1 | `CONEXUS-PERSIST-001` | DONE — Conexus persistence/bootstrap/operator visibility |
| 2 | `CONEXUS-PERSIST-002` | DONE — Conexus migration/bootstrap validation |
| 3 | `CONEXUS-PERSIST-003` | DONE — Conexus restart/durability regression |
| 4 | `KANON-OP-001` | DONE — Operator-readable Kanon topology/decision evidence |
| 5 | `KANON-OP-002` | DONE — Kanon operational diagnostics |
| 6 | `TRACE-CONTRACT-001` | DONE — Cross-service trace/correlation contract |
| 7 | `FE-HARDEN-001` | DONE — Frontend hardening beyond walkthrough |
| 8 | `FE-AUDIT-FIXTURES-001` | DONE — Fixture/live boundary audit |
| 9 | `FE-TEST-REPLAY-001` | DONE — Replay/test improvements |
| 10 | `FE-HYGIENE-CONFIG-001` | DONE — Frontend config hygiene |
| 11 | `UI-PACKAGING-STATUS-001` | DONE — `@ontogony/ui` packaging status |
| 12 | `TERMINOLOGY-CLEANUP-001` | DONE — operator/doc terminology glossary |

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
