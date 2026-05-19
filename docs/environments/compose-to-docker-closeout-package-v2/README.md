# Ontogony Next Phase — Compose to Docker Closeout Package v2

This is the corrected package for the next phase after:

```text
ENV-CLOSEOUT-001
ENV-DOCKER-001
ENV-DOCKER-002
ENV-DB-001
ENV-SEED-001
```

It is current with the Conexus liveness/readiness separation:

```text
/health       → lightweight liveness
/health/live  → lightweight liveness
/live         → lightweight liveness
/ready        → strict readiness
```

## Boundary

This package targets the **first Dockerized local working system**.

It is **not production readiness**.

Terminology glossary: [`docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](../../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md).

Post-Docker-local hardening closeout: [`docs/releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md`](../../releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md) (**CLOSED**, includes `CI-COST-001`).

## Minimal path to close Docker-local

```text
1. ENV-COMPOSE-001
2. ENV-DOCKER-RUN-001
3. ENV-DOCKER-FE-001
4. ENV-DOCKER-CLOSEOUT-001
```

Broad hardening comes **after** the Docker-local closeout.

## Critical health rule

Use Conexus liveness for startup:

```powershell
curl http://localhost:5082/health/live
```

Use Conexus strict readiness for semantic/provider checks:

```powershell
curl http://localhost:5082/ready
```

Before bootstrap, `/ready` may return `503`; this is expected and must not fail Docker startup. After seed/bootstrap, route evidence and fake-provider usability are proven through the seed/guided flow.

## Package layout

```text
00_MANIFEST.json
01_PR_SEQUENCE.md
02_ROADMAP.md
03_VALIDATION_MATRIX.md
04_STATUS_BOARD.md
05_CURRENT_STATE_BASELINE.md
06_INTEGRATED_REAL_ISSUE_NOTES.md
07_UNPACK_PROMPT.md
08_CURSOR_MASTER_PROMPT.md
pr-specs/
prompts/
script-stubs/
evidence-templates/
post-closeout-hardening/
```
