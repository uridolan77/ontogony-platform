# SYS-TIGHT-006 — Operator failure taxonomy adapter evidence

**Date:** 2026-05-22  
**Baseline:** `SYSTEM-ALPHA-006`  
**Sprint:** SYS-TIGHT-006 (adapter layer; public service error contracts unchanged)

## Scope delivered

- [`docs/operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md`](../operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md) — canonical contract index.
- [`docs/system/operator-failure-taxonomy.matrix.json`](../system/operator-failure-taxonomy.matrix.json) — representative code → taxonomy mappings.
- [`src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs`](../../src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs) — platform adapter.
- [`scripts/validate-system-operator-failure-taxonomy.ps1`](../../scripts/validate-system-operator-failure-taxonomy.ps1) — structural validation.
- Platform tests: `OperatorFailureTaxonomyAdapterTests`.
- Frontend: `ontogony-frontend/src/system/errors/operatorFailureTaxonomy.ts`, `OperatorFailureBanner.tsx`.
- Allagma: additive run failure payload taxonomy fields.

## Non-claims

- Does not change Kanon v0 HTTP error JSON or Conexus OpenAI-shaped gateway errors.
- Does not replace `CrossServiceErrorEnvelope` on Allagma HTTP responses.
- Does not add enterprise IAM or production SLO semantics.

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-operator-failure-taxonomy.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~OperatorFailureTaxonomy"
```

```powershell
cd C:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~OperatorFailureTaxonomy"
```

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/system/errors
```
