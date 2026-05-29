# SHARED-ERROR-CONTRACT-001 — closeout (ontogony-platform)

**Date:** 2026-05-29  
**Slice:** ONTOGONY-BACKEND-COORDINATION-002 / Phase 3

## Summary

Promoted cross-service error envelope contract v1, JSON schema, platform wire samples (Metabole, Aisthesis), and extended validation script. System compatibility gate error checks pass.

## Artifacts created/updated

| Path | Action |
| --- | --- |
| `docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md` | Promoted |
| `docs/schemas/ontogony-cross-service-error-envelope-v1.schema.json` | Created |
| `docs/system/cross-service-error-envelope.matrix.json` | Extended (metabole, aisthesis) |
| `docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md` | Cross-ref v1 |
| `scripts/validate-cross-service-error-envelope.ps1` | v1 contract + schema checks |

## Validation

```powershell
pwsh ./scripts/validate-cross-service-error-envelope.ps1 -DevRoot C:\dev
dotnet test tests/Ontogony.SystemCompatibility.Tests -c Release --filter "FullyQualifiedName~CrossServiceError"
```

**Result:** PASS.
