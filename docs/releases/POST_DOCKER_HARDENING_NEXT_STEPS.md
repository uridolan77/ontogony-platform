# Post-Docker-local hardening — next steps

**Date:** 2026-05-19  
**After:** `POST-DOCKER-HARDENING-CLOSEOUT-001` (hardening package **CLOSED / PASS**)

**Prerequisite closed:** [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md)

**This is strategic planning after local hardening, not production readiness.**

## Current posture

```text
First Dockerized local working system      CLOSED / PASS  (ENV-DOCKER-CLOSEOUT-001)
Post-Docker-local hardening                CLOSED / PASS  (POST-DOCKER-HARDENING-CLOSEOUT-001)
Production readiness                       NOT STARTED
```

## Strategic options (pick based on team priority)

### Option 1 — `ENV-REAL-PROVIDER-001` (optional backend smoke)

**When:** You need confidence that a **real** Conexus provider path works with explicit keys — still not production.

| Aspect | Notes |
| --- | --- |
| Scope | Backend-only; explicit opt-in keys; separate evidence |
| Risk | Cost, credential handling, non-determinism |
| Does not replace | Fake/local default for daily Docker-local dev |

### Option 2 — `CI-COST-001` (optional platform/frontend CI)

**When:** GitHub Actions minutes or `npm run check` duration becomes painful.

| Aspect | Notes |
| --- | --- |
| Scope | Split heavy gates, cache, selective E2E, workflow tuning |
| Touches | `ontogony-frontend` CI, possibly `ontogony-ui` `check:full` |
| Does not reduce | Local quality bar unless explicitly agreed |

### Option 3 — `PROD-READINESS-*` (separate program)

**When:** You are ready to treat deploy, identity, TLS, DR, and provider policy as first-class — **not** an extension of Docker-local.

| Aspect | Notes |
| --- | --- |
| Prerequisite | Closed local + hardening packages (satisfied) |
| Out of scope for ENV | Cloud manifests, staging/prod identity, SLOs |
| Start | New program charter + acceptance matrix — do not relabel ENV closeout as prod |

### Option 4 — Further product hardening

**When:** Operator UX, eval durability, or cross-repo alignment is the bottleneck.

| Track | Example entry points |
| --- | --- |
| Eval / sanity | `docs/planning/eval-durability-to-first-sanity-current/` |
| Backend/frontend alignment | `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/` |
| Allagma eval closeout | `allagma-dotnet/docs/releases/FIRST_FULL_SANITY_CLOSEOUT.md` |
| Frontend | Route coverage gaps, performance budgets, additional E2E |

## Recommended default (no single mandate)

Most teams should **keep using the closed Docker-local stack daily** and choose **one** optional track:

1. **Product velocity** → Option 4 (eval/alignment)  
2. **Provider confidence** → Option 1 (`ENV-REAL-PROVIDER-001`)  
3. **CI pain** → Option 2 (`CI-COST-001`)  
4. **Deploy timeline** → Option 3 (new `PROD-READINESS-*` charter)

## Operator quick reference (unchanged)

| Need | Doc |
| --- | --- |
| Glossary | `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md` |
| Compose stack | `docker/local-working-system/README.md` |
| Hardening closeout | `docs/releases/POST_DOCKER_HARDENING_CLOSEOUT.md` |
| Docker-local closeout | `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md` |
| Frontend `check` | `ontogony-frontend`: `npm run check` |

## Required statement

```text
The first Dockerized local working system and post-Docker-local hardening are closed.
Neither milestone is production readiness.
```
