# CONEXUS-PERSIST-003 — Conexus restart/durability regression

## Purpose

Prove Conexus Docker-local persistence survives a **conexus-api** container restart without losing bootstrap routing state, key alignment, or route evidence.

Builds on `CONEXUS-PERSIST-001` (operator docs) and `CONEXUS-PERSIST-002` (validation automation).

## Timing

Post-`CONEXUS-PERSIST-002`.

## Boundary

- Validation/regression automation only — **not production readiness**.
- Fake/local provider only; no real provider keys or external execution.
- Restarts **conexus-api only** (Postgres volume unchanged).
- No broad runtime behavior changes in service repos.

## Delivered

```text
docker/local-working-system/scripts/run-conexus-persistence-durability-regression.ps1
docker/local-working-system/scripts/validate-conexus-persistence-durability-report.ps1
docs/evidence/CONEXUS_PERSIST_003_DURABILITY_REGRESSION_EVIDENCE.md
```

## Regression steps

| # | Step | Required for PASS |
| --- | --- | --- |
| 1 | Docker stack healthy (`wait-local-working-system.ps1`) | yes |
| 2 | `CONEXUS-PERSIST-002` validator before restart | yes |
| 3 | Bootstrap state (fake provider + alias) before restart | yes |
| 4 | Route evidence in artifacts before restart | yes |
| 5 | Restart **conexus-api** only | yes |
| 6 | `/health/live` 2xx after restart | yes |
| 7 | `/ready` captured after restart | informational |
| 8 | Fake provider exists after restart | yes |
| 9 | `gpt-4o-mini` alias exists after restart | yes |
| 10 | Project API key alignment unchanged | yes |
| 11 | Route decision admin fetch before/after restart | yes |
| 12 | `CONEXUS-PERSIST-002` validator after restart | yes |
| 13 | JSON report under `artifacts/conexus-persist-003-durability-report.json` | yes |
| 14 | Report validator asserts required fields + `verdict=PASS` | yes |

## Prerequisites

Run once before regression (if artifacts missing):

```powershell
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
# or
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Report redaction

Durability report must not contain raw API keys or connection-string passwords (same discipline as `CONEXUS-PERSIST-002`).

## Acceptance

- Regression script **PASS** on current Docker-local stack.
- Report validator **PASS**.
- Evidence records commands and results.
- No service `src/` or workflow changes.

## Follow-up

After merge, next recommended hardening PR: **`KANON-OP-001`**.
