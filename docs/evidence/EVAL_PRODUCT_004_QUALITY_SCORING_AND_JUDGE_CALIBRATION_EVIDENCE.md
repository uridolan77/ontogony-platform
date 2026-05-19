# EVAL-PRODUCT-004 — Quality Scoring and Judge Calibration Surfaces Evidence

Date: 2026-05-19  
Status: **PASS**

## Decision

`EVAL-PRODUCT-004` is accepted as a completed product-hardening slice.

Quality scoring and judge calibration metadata are now explicitly exposed in backend contracts and consumed in frontend operator UI with limitation-aware wording.

## Scope implemented

### Backend (`allagma-dotnet`)

- Added explicit quality metadata contract:
  - `AllagmaEvaluationQualityMetadataResponse`
- Added quality metadata to:
  - `AllagmaEvaluationRunResponse`
  - `AllagmaEvaluationRunSummaryResponse`
- Added metadata parsing in application mapping path:
  - `EvaluationQualityMetadataParser`
  - wiring in run/detail and run/list summary mappers
- Updated OpenAPI snapshot and provenance:
  - `docs/api/allagma-openapi-v1.snapshot.json`
  - `docs/api/allagma-openapi-v1.provenance.json`
- Added/updated tests:
  - `AllagmaEvaluationApiTests` assertions for quality metadata in detail/list
  - `AllagmaOpenApiSnapshotTests` schema assertion for quality metadata contract

### Frontend (`ontogony-frontend`)

- Synced/regenerated Allagma OpenAPI contracts:
  - `openapi/allagma.v0.json`
  - `src/allagma/api/generated/schema.ts`
  - `docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json`
- Extended eval view models/adapters to map quality metadata fields.
- Extended quality breakdown model to include judge metadata fields.
- Updated `AllagmaEvalQualityScoreDetail` with a dedicated "Judge calibration metadata" section.
- Added limitation-aware wording:
  - explicit advisory-only note when `llmJudgeEnabled=true`
  - deterministic baseline note otherwise
- Added/updated adapter tests covering metadata mapping and breakdown propagation.

## Validation commands

Backend (`allagma-dotnet`):

```powershell
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~AllagmaEvaluationApiTests|FullyQualifiedName~AllagmaOpenApiSnapshotTests"
powershell -NoProfile -ExecutionPolicy Bypass -File ./scripts/update-allagma-openapi-provenance.ps1
```

Frontend (`ontogony-frontend`):

```powershell
npm run openapi:sync:allagma
npm run openapi:gen
npm run openapi:check
npm run typecheck
npm run test -- src/allagma/adapters/allagmaEvaluationAdapters.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts src/app/route-coverage.test.ts src/app/routes.test.ts
```

## Remaining limitations (intentional)

- No calibration history/trend timeline or operator-tunable calibration workflows.
- No training or online adjustment of judge behavior.
- No production-readiness claims for judge reliability.
- LLM judge remains advisory-only in current scope when enabled.

## Boundary statement

This work is **product hardening only** and explicitly **not production readiness**.
