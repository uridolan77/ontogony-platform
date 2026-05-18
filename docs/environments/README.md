# Environment planning docs

This folder holds planning packages and **canonical operator documentation** for local Ontogony environments.

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
| **ENV-DOCKER-002** | **Next** — Dockerfiles + `.dockerignore` | `allagma-dotnet`, siblings |
| ENV-DOCKER-002 … ENV-DOCKER-CLOSEOUT-001 | Planned | See `docker-local-working-system/00_MANIFEST.json` |

Evidence:

- `docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md`
- `docs/evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md`
- `docs/evidence/ENV_DOCKER_001_PLAN_EVIDENCE.md`
- Closeout (script-based): `docs/releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md`

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
