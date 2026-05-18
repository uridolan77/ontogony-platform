# Docker local working system — canonical plan

Operator-facing plan for the **first Docker/Postgres local working system**. This tree is **documentation only** until later ENV-DOCKER-* implementation PRs land compose, Dockerfiles, and runners.

## Boundary

**This package supports the first working local environment and first Dockerized local working system. It is not production readiness.**

## Start here

| Doc | Purpose |
| --- | --- |
| [00_MANIFEST.json](./00_MANIFEST.json) | Program metadata and PR sequence |
| [01_WORKSPACE_LAYOUT.md](./01_WORKSPACE_LAYOUT.md) | Six-repo layout and compose root |
| [02_TARGET_ARCHITECTURE.md](./02_TARGET_ARCHITECTURE.md) | Service topology |
| [03_EXACT_SETTINGS.md](./03_EXACT_SETTINGS.md) | Container vs host URLs and config keys |
| [04_DATABASES_AND_SEEDS.md](./04_DATABASES_AND_SEEDS.md) | Logical DBs, users, seed outcomes |
| [05_DOCKER_COMPOSE_PLAN.md](./05_DOCKER_COMPOSE_PLAN.md) | Planned compose location and health gates |
| [06_MAIN_USE_FLOW.md](./06_MAIN_USE_FLOW.md) | Twelve-step proof flow |
| [07_ACCEPTANCE_CHECKLIST.md](./07_ACCEPTANCE_CHECKLIST.md) | Closeout gates for Docker phase |
| [08_TROUBLESHOOTING.md](./08_TROUBLESHOOTING.md) | Common Docker networking mistakes |
| [09_KNOWN_LIMITATIONS.md](./09_KNOWN_LIMITATIONS.md) | Accepted v1 limitations |

## Planning package (PR specs, stubs)

Implementation specs and script stubs remain in:

```text
allagma-dotnet/docs/environments/working-local-environment-complete-package/
```

## Prior program (closed)

Script-based local operator sanity: `docs/environments/local-operator-sanity/` — closed by ENV-CLOSEOUT-001.

## Next implementation PR

**ENV-COMPOSE-001** — Docker Compose orchestration.

## Completed

**ENV-DOCKER-002** — Dockerfiles and `.dockerignore` merged in allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend (2026-05-19).

**ENV-DB-001** — Postgres init SQL and verify script in `docker/local-working-system/postgres/init/` (2026-05-19).

**ENV-SEED-001** — deterministic seed/bootstrap script + API verification report in `docker/local-working-system/scripts/` (2026-05-19).
