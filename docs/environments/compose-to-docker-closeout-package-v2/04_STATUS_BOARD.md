# Status Board

## Completed / accepted before this package

| Item | Status |
|---|---|
| ENV-CLOSEOUT-001 | done |
| ENV-DOCKER-001 | done |
| ENV-DOCKER-002 | done / Dockerfile PRs expected merged or mergeable |
| ENV-DB-001 | done |
| ENV-SEED-001 | done for host-local API deterministic seed verification |

## Active

| Item | Status | Notes |
|---|---|---|
| ENV-COMPOSE-001 | done | Evidence reaffirmed 2026-05-19 |
| ENV-DOCKER-RUN-001 | done | Guided flow + Allagma restart durability |
| ENV-DOCKER-FE-001 | active / next | Frontend walkthrough |
| ENV-DOCKER-CLOSEOUT-001 | planned | Milestone closeout |

## Boundary notes

- ENV-SEED-001 proves host-local API seed behavior.
- ENV-COMPOSE-001 proves Docker startup and container orchestration.
- ENV-DOCKER-RUN-001 proves the full Dockerized main flow.
- Conexus `/ready` may fail before bootstrap by design.
