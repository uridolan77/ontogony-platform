# First Docker-local working system scorecard

**Date:** 2026-05-19  
**Gate:** ENV-DOCKER-CLOSEOUT-001  
**Evidence:** [ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md](../evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md)

**This is first Dockerized local working system, not production readiness.**

Scores reflect what was **demonstrated across the Docker ENV program**, not production SLOs or deploy readiness.

| Area | Score | Notes |
| --- | ---: | --- |
| Docker Compose orchestration | 9.4/10 | Config render, wait/reset, liveness vs `/ready` boundary |
| Postgres bootstrap (3 DBs) | 9.3/10 | Single container; dev credentials only |
| Seed/bootstrap (ENV-SEED-001) | 9.2/10 | Host-local proof + compose seed script |
| Dockerized guided main flow | 9.5/10 | Machine report + validator PASS |
| Governed runs (baseline + subject) | 9.5/10 | Topology auth on subject; route decisions both |
| Restart durability | 9.4/10 | Evals + comparison survive `allagma-api` restart |
| Inter-service wiring | 9.3/10 | Container DNS + host-mapped browser APIs |
| Frontend operator surfaces | 8.9/10 | Compose SPA routes; manual banner/secret check |
| Operator scripts/docs | 9.2/10 | `docker/local-working-system/scripts/` + plan tree |
| Safety boundaries | 9.8/10 | Fake provider; real execution disabled; no secrets in reports |
| **Overall Docker-local health** | **9.3/10** | First compose working system closed |

## ENV PR acceptance (summary)

| PR | Verdict | Primary evidence |
| --- | --- | --- |
| ENV-DOCKER-001 | PASS | `docs/evidence/ENV_DOCKER_001_PLAN_EVIDENCE.md` |
| ENV-DB-001 | PASS | `docs/evidence/ENV_DB_001_POSTGRES_BOOTSTRAP_EVIDENCE.md` |
| ENV-SEED-001 | PASS | `docs/evidence/ENV_SEED_001_DETERMINISTIC_BOOTSTRAP_EVIDENCE.md` |
| ENV-COMPOSE-001 | PASS | `docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md` |
| ENV-DOCKER-RUN-001 | PASS | `docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| ENV-DOCKER-FE-001 | PASS | `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| ENV-DOCKER-CLOSEOUT-001 | PASS | `docs/evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md` |

## What the score does not mean

- Not production deploy readiness, TLS, identity, or DR
- Not real provider keys or external tool execution by default
- Not automated live-browser walkthrough in the docker guided runner
- Not post-closeout hardening (now closed separately — see [POST_DOCKER_HARDENING_SCORECARD.md](./POST_DOCKER_HARDENING_SCORECARD.md))

See [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md).
