# CONEXUS-PERSIST-003 — Durability regression evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**  
**Statement:** Post-Docker-local hardening regression automation; **not production readiness**.

## Scope

Add Conexus **conexus-api** restart/durability regression for Docker-local Postgres persistence. No service `src/` or workflow changes; no secrets committed.

## Delivered

```text
docker/local-working-system/scripts/run-conexus-persistence-durability-regression.ps1
docker/local-working-system/scripts/validate-conexus-persistence-durability-report.ps1
docker/local-working-system/README.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/CONEXUS-PERSIST-003.md
docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md
docs/environments/compose-to-docker-closeout-package-v2/04_STATUS_BOARD.md
docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md
docs/evidence/CONEXUS_PERSIST_003_DURABILITY_REGRESSION_EVIDENCE.md
```

## Prerequisites

- Docker local working system running.
- Route evidence artifacts from prior guided flow or seed (`docker-guided-main-flow-report.json` or `env-seed-001-report.json`).

## Commands run

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-conexus-persistence-durability-regression.ps1 -SkipFrontend

.\docker\local-working-system\scripts\validate-conexus-persistence-durability-report.ps1

Select-String -Path .\docker\local-working-system\artifacts\conexus-persist-003-durability-report.json `
  -Pattern 'cx-dev-key-change-me|cx-conexus-admin-dev|conexus_local_pw|ontogony_admin_pw'
# Expected: no matches
```

## Results

| Step | Result |
| --- | --- |
| Stack healthy | **PASS** |
| CONEXUS-PERSIST-002 before restart | **PASS** |
| Bootstrap + route evidence before restart | **PASS** |
| `docker compose restart conexus-api` | **PASS** |
| `/health/live` after restart | **PASS** (HTTP 200) |
| `/ready` after restart | **503** (informational — strict readiness) |
| Fake provider + alias after restart | **PASS** |
| Route decision admin fetch before/after | **PASS** |
| Key alignment unchanged | **PASS** |
| CONEXUS-PERSIST-002 after restart | **PASS** |
| Durability report validator | **PASS** |
| Raw secrets in durability report | **none** (grep confirmed) |
| Production readiness claimed | **no** |

Artifacts (local, not committed):

```text
docker/local-working-system/artifacts/conexus-persist-003-durability-report.json
docker/local-working-system/artifacts/conexus-persist-002-before-restart-report.json
docker/local-working-system/artifacts/conexus-persist-002-after-restart-report.json
```

## Safety

| Check | Status |
| --- | --- |
| Restarts conexus-api only | **yes** |
| Postgres volume unchanged | **yes** |
| Fake/local provider only | **yes** |
| Report redacts secrets | **yes** |

## Follow-up

After merge, recommended next hardening PR: **`KANON-OP-001`**.
