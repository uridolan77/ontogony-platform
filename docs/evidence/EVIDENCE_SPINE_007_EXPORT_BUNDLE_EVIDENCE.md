# EVIDENCE-SPINE-007 — Cross-service evidence spine export bundle (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** Canonical JSON Schema and minimal fixture for operator export bundles produced by `ontogony-frontend` evidence spine workbench.

## Delivered

| Artifact | Path |
| --- | --- |
| JSON Schema | `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json` |
| Valid fixture | `docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json` |
| Schema test | `tests/Ontogony.Infrastructure.Tests/EvidenceSpineExportBundleSchemaTests.cs` |
| Frontend evidence | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_007_EXPORT_BUNDLE_EVIDENCE.md` |

## Schema id

```text
ontogony-cross-service-evidence-spine-bundle-v1 (version 1)
```

## Validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter EvidenceSpineExportBundleSchemaTests
```

## Related

- [`docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md)
- [`docs/evidence/EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md`](EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md)

## Next

- EVIDENCE-SPINE-008 — E2E verification of workbench export in browser/Docker
