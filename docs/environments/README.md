# Environment planning docs

This folder holds planning packages and **canonical operator documentation** for local Ontogony environments.

## Package — compose-to-Docker closeout (v2)

**Planning/control package:**

- [`docs/environments/compose-to-docker-closeout-package-v2/`](compose-to-docker-closeout-package-v2/)

**Docker-local closeout path — CLOSED** (2026-05-19, not production readiness):

- **ENV-COMPOSE-001** — DONE / PASS
- **ENV-DOCKER-RUN-001** — DONE / PASS
- **ENV-DOCKER-FE-001** — DONE / PASS
- **ENV-DOCKER-CLOSEOUT-001** — DONE / PASS

**Current phase:** post-Docker-local hardening **CLOSED** (`POST-DOCKER-HARDENING-CLOSEOUT-001`). See [`POST_DOCKER_HARDENING_CLOSEOUT.md`](../releases/POST_DOCKER_HARDENING_CLOSEOUT.md), [`POST_DOCKER_HARDENING_NEXT_STEPS.md`](../releases/POST_DOCKER_HARDENING_NEXT_STEPS.md), and `compose-to-docker-closeout-package-v2/04_STATUS_BOARD.md`.

Hardening specs under `compose-to-docker-closeout-package-v2/post-closeout-hardening/` are **post-closeout** and separate from the closed milestone.

Unpack evidence: `docs/evidence/COMPOSE_TO_DOCKER_CLOSEOUT_PACKAGE_V2_UNPACK_EVIDENCE.md`

## Active program — Docker local working system (ENV-DOCKER-LOCAL)

**Canonical plan (operator-facing):**

- [`docs/environments/docker-local-working-system/`](docker-local-working-system/)

Start with:

- `docker-local-working-system/00_MANIFEST.json`
- `docker-local-working-system/02_TARGET_ARCHITECTURE.md`
- `docker-local-working-system/03_EXACT_SETTINGS.md`
- `docker-local-working-system/05_DOCKER_COMPOSE_PLAN.md`

**Implementation specs and stubs:**

- `allagma-dotnet/docs/environments/working-local-environment-complete-package/`

**Prior program (closed):** script-based local operator sanity — `local-operator-sanity/` + [FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md](../releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md)

## Closed program — Script-based local operator environment

**Canonical docs:**

- `docs/environments/local-operator-sanity/`

**Planning package:**

- `docs/environments/local-operator-sanity-package/`

Incoming ZIP preserved at:

- `docs/_incoming/ontogony-first-working-environment-package.zip`

Expected workspace layout (all repos under `C:\dev\`):

```text
C:\dev\
  ontogony-platform\
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

## PR status

| PR | Status | Repo |
| --- | --- | --- |
| ENV-SETUP-001 | **Done** — workspace/settings docs | `ontogony-platform` |
| ENV-SETUP-002 | **Done** — stack start/check scripts | `allagma-dotnet` |
| ENV-RUN-001 | **Done** — guided main flow | `allagma-dotnet` |
| ENV-FE-001 | **Done** — frontend walkthrough | `ontogony-frontend` |
| ENV-PG-001 | **Done** — optional Postgres durable mode | `allagma-dotnet` |
| ENV-UI-001 | **Done** — ontogony-ui integration readiness | `ontogony-frontend` |
| ENV-CLOSEOUT-001 | **Done** — first working environment closeout | `ontogony-platform` |
| ENV-REAL-PROVIDER-001 | Deferred (optional) | — |
| ENV-DOCKER-001 | **Done** — Docker local working system plan | `ontogony-platform` |
| ENV-DOCKER-002 | **Done** — Dockerfiles + `.dockerignore` | `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend` |
| ENV-DB-001 | **Done** — Postgres DB bootstrap | `ontogony-platform/docker/local-working-system/` |
| ENV-SEED-001 | **Done** — deterministic seed/bootstrap (host-local API verification) | `ontogony-platform/docker/local-working-system/` |
| **ENV-COMPOSE-001** | **Done** — Docker Compose orchestration | `ontogony-platform/docker/local-working-system/` |
| **ENV-DOCKER-RUN-001** | **Done** — Dockerized guided main flow | `ontogony-platform/docker/local-working-system/` |
| **ENV-DOCKER-FE-001** | **Done** — Dockerized frontend walkthrough | `ontogony-frontend/scripts/docker/` |
| ENV-DOCKER-CLOSEOUT-001 | **Done** — first Docker local system closeout | `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md` |

Evidence:

- `docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md`
- `docs/evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md`
- `docs/evidence/ENV_DOCKER_001_PLAN_EVIDENCE.md`
- `docs/evidence/ENV_DB_001_POSTGRES_BOOTSTRAP_EVIDENCE.md`
- `docs/evidence/ENV_SEED_001_DETERMINISTIC_BOOTSTRAP_EVIDENCE.md`
- `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md`
- `docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md`
- `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md`
- `docs/evidence/COMPOSE_TO_DOCKER_CLOSEOUT_PACKAGE_V2_UNPACK_EVIDENCE.md`
- `docs/evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md`
- Closeout (script-based): `docs/releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md`
- Closeout (Docker): `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md`

Scope note: ENV-SEED-001 evidence validates host-local API behavior against running services (`localhost:5081`–`5083`). ENV-COMPOSE-001 adds compose orchestration and container DNS wiring. Restart-survival proof remains gated by ENV-DOCKER-RUN-001.

## Boundary

This program targets a **first working local environment**, not production readiness. Use fake/local Conexus provider first; optional real-provider mode is a later, explicit PR. Do not treat acceptance here as deploy or hardening sign-off.

## Other planning (unchanged)

Eval and platform planning packages remain in their own trees:

- `docs/planning/eval-durability-to-first-sanity-current/` — active eval durability → first sanity program
- `docs/eval-basing/ontogony-eval-based-cross-repo-development-package/` — completed eval-basing baseline
- `docs/planning/` — other non-environment planning packages (see `docs/planning/README.md`)

Notes:

- Implementation PRs land in the repos named in each spec under `local-operator-sanity-package/04_PR_SPECS/`.
- Script stubs under `local-operator-sanity-package/06_SCRIPT_STUBS/` are not executed until copied into `allagma-dotnet` (ENV-SETUP-002 onward).
