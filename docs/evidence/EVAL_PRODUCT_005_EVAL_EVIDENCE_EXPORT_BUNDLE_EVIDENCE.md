# EVAL-PRODUCT-005 — Eval Evidence Export Bundle Evidence

Date: 2026-05-19  
Status: **PASS**

## Decision

`EVAL-PRODUCT-005` is accepted as a completed product-hardening slice.

Operators can export a coherent eval evidence bundle across evaluation run, subject run refs, baseline comparisons, dataset summary, trace/correlation ids, and artifact locators.

## Scope implemented

### Platform (`ontogony-platform`)

- JSON schema: `docs/product-hardening/eval-alignment-frontend-depth/schemas/allagma-eval-evidence-export-bundle-v1.schema.json`
- Valid fixture: `schemas/fixtures/valid/eval-evidence-export-bundle-minimal.json`
- Schema test: `tests/Ontogony.Infrastructure.Tests/EvalEvidenceExportBundleSchemaTests.cs`

### Backend (`allagma-dotnet`)

- Contract: `AllagmaEvalEvidenceExportBundleContract` and related DTOs
- Service: `GetEvalEvidenceExportBundleService`
- Route: `GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
- Metadata redaction helper: `EvalEvidenceExportRedactor`
- OpenAPI snapshot + provenance updated
- Tests: `EvalEvidenceExportBundleTests`, `AllagmaOpenApiSnapshotTests` path/schema assertions

### Frontend (`ontogony-frontend`)

- OpenAPI sync + generated client types
- API client + hook: `getAllagmaEvalEvidenceExportBundle`, `useAllagmaEvalEvidenceExportBundle`
- Fixture builder: `buildAllagmaEvalEvidenceExportFromEvaluation`
- UI: `AllagmaEvalEvidenceExportPanel` on evaluation run detail page
- Tests: `buildAllagmaEvalEvidenceExport.test.ts`

## Validation commands

Backend:

```powershell
cd c:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~EvalEvidenceExportBundleTests|FullyQualifiedName~AllagmaOpenApiSnapshotTests.OpenApi_snapshot"
powershell -NoProfile -ExecutionPolicy Bypass -File ./scripts/update-allagma-openapi-provenance.ps1
```

Platform:

```powershell
cd c:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~EvalEvidenceExportBundleSchemaTests"
```

Frontend:

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:sync:allagma
npm run openapi:gen
npm run openapi:check
npm run typecheck
npm run test -- src/allagma/diagnostics/buildAllagmaEvalEvidenceExport.test.ts
```

## Remaining limitations (intentional)

- Not a compliance archive or long-term retention product.
- Does not embed Kanon decision payloads, Conexus route bodies, or full run audit bundles (locators only).
- Subject run `context` and raw model I/O are omitted by design.
- No bulk/multi-eval export or warehouse integration.

## Boundary statement

This work is **product hardening only** and explicitly **not production readiness**.
