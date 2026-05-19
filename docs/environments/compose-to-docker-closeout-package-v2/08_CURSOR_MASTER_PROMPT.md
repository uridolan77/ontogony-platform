# Cursor Master Prompt

We are continuing Ontogony Docker-local ENV work.

Current completed work:
- ENV-CLOSEOUT-001 done
- ENV-DOCKER-001 done
- ENV-DOCKER-002 done
- ENV-DB-001 done
- ENV-SEED-001 accepted as host-local API seed verification

Current health contract:
- Conexus `/health`, `/health/live`, `/live` are lightweight liveness.
- Conexus `/ready` is strict readiness.
- Docker startup must use `/health/live`.
- `/ready` may be 503 before bootstrap and that is expected.

Minimal closeout path:
1. ENV-COMPOSE-001
2. ENV-DOCKER-RUN-001
3. ENV-DOCKER-FE-001
4. ENV-DOCKER-CLOSEOUT-001

Rules:
- no real provider keys
- no real external execution
- no committed secrets
- no production-readiness claim
- close Docker-local before broad hardening
