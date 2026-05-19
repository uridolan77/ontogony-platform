# First Docker-local working system — next steps

**Date:** 2026-05-19  
**After:** ENV-DOCKER-CLOSEOUT-001 (Docker local working system **CLOSED**)

**This is first Dockerized local working system, not production readiness.**

Post-closeout work is **explicitly separate** from the closeout criteria. Do not treat these items as blockers for the closed milestone unless they become direct production blockers for your team.

## Post-closeout hardening backlog

Planning package: `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/`

| PR | Focus |
| --- | --- |
| CONEXUS-PERSIST-001 / 002 / 003 | Conexus persistence and bootstrap hardening |
| KANON-OP-001 / 002 | Operator-readable Kanon topology/decision surfaces |
| TRACE-CONTRACT-001 | Trace/correlation contract alignment |
| FE-HARDEN-001 | Frontend hardening beyond walkthrough |
| FE-HYGIENE-CONFIG-001 | Config hygiene |
| FE-AUDIT-FIXTURES-001 | Fixture audit coverage |
| FE-TEST-REPLAY-001 | Test replay improvements |
| TERMINOLOGY-CLEANUP-001 | Operator/doc terminology |
| UI-PACKAGING-STATUS-001 | `@ontogony/ui` packaging status |

## Optional (not blocking)

| Item | Notes |
| --- | --- |
| ENV-REAL-PROVIDER-001 | Backend-only real provider smoke; explicit keys; still not production |
| Runtime Vite config injection | Deferred from ENV-DOCKER-002 / compose frontend image |
| Cloud deploy manifests | Out of scope for first Docker-local program |

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
