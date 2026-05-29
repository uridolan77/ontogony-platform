# ALLAGMA-PHENOMENOLOGICAL-BRIDGE-CONTRACT-SYNC-001

## Owner repo

- `allagma-dotnet`

## Problem

Full `dotnet test Allagma.sln` fails on known contract drift: `phenomenological-projection` route in feature matrix but not OpenAPI snapshot; route inventory / OpenAPI provenance hash drift; phenomenological bridge event types missing from vocabulary snapshot; run audit evidence docs out of sync.

## Required implementation

1. Identify actual implemented route(s).
2. Regenerate OpenAPI snapshot.
3. Regenerate route inventory.
4. Update feature connection matrix.
5. Update event vocabulary snapshot.
6. Update run audit evidence docs.
7. Re-run full suite.

## Acceptance

```powershell
cd C:\devllagma-dotnet
dotnet test Allagma.sln -c Release --filter "FullyQualifiedName~OpenApiRouteInventoryCohesionTests|FullyQualifiedName~FeatureConnectionMatrixAuditTests|FullyQualifiedName~AllagmaV0RouteInventoryTests|FullyQualifiedName~RunAuditEvidenceDocsTests|FullyQualifiedName~FirstWorkingSystemEventVocabularyTests"
dotnet test Allagma.sln -c Release
```
