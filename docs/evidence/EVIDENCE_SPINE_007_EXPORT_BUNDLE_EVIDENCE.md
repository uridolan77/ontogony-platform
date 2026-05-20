# EVIDENCE-SPINE-007 — Cross-service evidence spine export bundle (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (007A contract hardening)  
**Statement:** Canonical JSON Schema and fixtures for operator export bundles produced by `ontogony-frontend` evidence spine workbench. **007A** adds a rich **generated** fixture emitted from the frontend export builder and validated in both repos.

## Delivered

| Artifact | Path |
| --- | --- |
| JSON Schema | `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json` |
| Minimal fixture | `docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json` |
| Generated fixture (frontend builder) | `docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-generated.json` |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/EvidenceSpineExportBundleSchemaTests.cs` |
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
