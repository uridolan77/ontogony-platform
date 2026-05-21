# SYS-TIGHT-002 — System evidence spine contract evidence

**Date:** 2026-05-21  
**Baseline:** `SYSTEM-ALPHA-006`  
**Sprint:** SYS-TIGHT-002 (trimmed — contract index; no resolver rewrite)

## Scope delivered

- [`docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](../operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md) — canonical contract index (aliases Evidence Spine 000–009).
- [`docs/system/system-evidence-spine-resolution.matrix.json`](../system/system-evidence-spine-resolution.matrix.json) — machine identifier → route map (admin Conexus paths for operator spine).
- [`scripts/validate-system-evidence-spine-contract.ps1`](../../scripts/validate-system-evidence-spine-contract.ps1) — structural + sibling handoff validation.
- Platform tests: `SystemEvidenceSpineContractTests`.
- Frontend conformance test: `evidenceSpineContractMatrix.test.ts`.
- Taxonomy cross-link to this contract.

## Non-claims

- Does not add a backend unified resolve API (deferred by Evidence Spine v1).
- Does not implement SYS-TIGHT-003 run-centric audit page.
- Does not change Kanon v0 API surface.

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-evidence-spine-contract.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~SystemEvidenceSpineContractTests"
```

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/evidence-spine/evidenceSpineContractMatrix.test.ts
```

## Resolver coverage (existing)

Required roots are already exercised in `ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.test.ts`:

- `allagmaRunId`, `allagmaEvaluationRunId`, `conexusModelCallId`, `kanonDecisionId`
- `correlationId`, `traceId`, `humanGateId`
- Unresolved / missing downstream: explicit graph edges (non-fatal)
