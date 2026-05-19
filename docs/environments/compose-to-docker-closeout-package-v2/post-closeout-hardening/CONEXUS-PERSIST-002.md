# CONEXUS-PERSIST-002 — Conexus migration/bootstrap validation automation

## Purpose

Add repeatable Conexus-focused validation for Docker-local persistence, migrations, dev bootstrap state, key alignment, and route evidence reporting.

Builds on `CONEXUS-PERSIST-001` operator docs.

## Timing

Post-`CONEXUS-PERSIST-001`.

## Boundary

- Validation automation only — **not production readiness**.
- Fake/local provider only; no real provider keys or external execution.
- No broad runtime behavior changes unless a validation gap exposes an obvious defect.

## Repo

`ontogony-platform` (compose-side script + evidence).

## Delivered

```text
docker/local-working-system/scripts/validate-conexus-persistence-bootstrap.ps1
docs/evidence/CONEXUS_PERSIST_002_VALIDATION_EVIDENCE.md
```

## Validation checks

| # | Check | Required for PASS |
| --- | --- | --- |
| 1 | Docker stack reachable (`conexus-api`, `postgres`) | yes |
| 2 | Conexus `GET /health/live` returns 2xx | yes |
| 3 | Conexus `GET /ready` captured (503 OK pre-bootstrap) | informational |
| 4 | Postgres `conexus_local` database exists | yes |
| 5 | EF migrations applied (`__EFMigrationsHistory`, `conexus_model_alias`) | yes |
| 6 | Rendered compose includes `ConnectionStrings__ConexusPostgres` + Allagma project key | yes |
| 7 | `CONEXUS_DEV_PROJECT_API_KEY` aligned with `CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA` | yes |
| 8 | Fake provider + `gpt-4o-mini` alias (detect or invoke bootstrap) | yes |
| 9 | Route/model evidence from guided/seed report | WARN if missing; FAIL with `-RequireRouteEvidence` |
| 10 | JSON report under `artifacts/conexus-persist-002-report.json` | yes |

## Report redaction

The JSON report must not contain raw API keys or connection-string passwords. It records configured booleans, `keysAligned`, and a redacted connection string (`Password=***`). Bootstrap responses omit raw `apiKey` values (`apiKeyIssued` boolean only).

## Script parameters

| Parameter | Default | Meaning |
| --- | --- | --- |
| `-InvokeBootstrap` | auto when state missing | Call `POST /admin/v0/dev/bootstrap` |
| `-SkipBootstrap` | off | Fail if bootstrap state missing |
| `-UseExistingReports` | on | Read guided/seed artifact reports for route IDs |
| `-RequireRouteEvidence` | off | Fail if route IDs not in artifacts |

## Acceptance

- Script **PASS** on current Docker-local stack with running compose.
- Evidence records commands and results.
- No runtime source changes in Conexus/Allagma/Kanon.
- No workflow changes; no secrets committed.

## Follow-up

| PR | Focus |
| --- | --- |
| `CONEXUS-PERSIST-003` | Restart/durability regression checks |
