# SYS-PROTOCOL-RUNTIME-001 Evidence

**Date:** 2026-05-21  
**Repos:** `ontogony-platform`  
**Verdict:** PASS

Issue:
SYS-PROTOCOL-RUNTIME-001 — Evidence-first runtime protocol identity.

Repos changed:
- ontogony-platform

Files changed:
- docs/system/system-protocol-registry.json
- docs/system/schemas/system-protocol-registry.schema.json
- scripts/validate-system-protocol-registry.ps1
- docs/system/README.md
- docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json
- docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json
- docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-generated.json
- docs/product-hardening/eval-alignment-frontend-depth/schemas/allagma-eval-evidence-export-bundle-v1.schema.json
- docs/product-hardening/eval-alignment-frontend-depth/schemas/fixtures/valid/eval-evidence-export-bundle-minimal.json
- schemas/ontogony-envelope.schema.json
- schemas/fixtures/valid/full-envelope.json
- src/Ontogony.Contracts/Events/OntogonyEnvelope.cs
- src/Ontogony.Contracts/Events/OntogonyEventMetadata.cs
- src/Ontogony.Contracts/Events/OntogonyEventHeaders.cs
- src/Ontogony.Contracts/Events/DefaultEnvelopeValidator.cs
- src/Ontogony.Contracts/Events/CloudEventsExtensions.cs
- src/Ontogony.Contracts/README.md
- src/Ontogony.Evaluation.Contracts/EvaluationArtifactRef.cs
- tests/Ontogony.Evaluation.Contracts.Tests/EvaluationContractsTests.cs
- tests/Ontogony.Infrastructure.Tests/EvidenceSpineExportBundleSchemaTests.cs
- tests/Ontogony.Infrastructure.Tests/EvalEvidenceExportBundleSchemaTests.cs
- tests/Ontogony.Messaging.Tests/OntogonyEnvelopeValidatorTests.cs
- tests/Ontogony.Messaging.Tests/OntogonyContractsPr4Tests.cs
- tests/Ontogony.Infrastructure.Tests/SchemaFixtureValidationTests.cs
- tests/Ontogony.Infrastructure.Tests/HeaderConstantsSnapshotTests.cs
- tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.Contracts.verified.txt
- tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.Evaluation.Contracts.verified.txt
- tests/Ontogony.Infrastructure.Tests/SysProtocolRuntime001Tests.cs
- docs/evidence/SYS_PROTOCOL_RUNTIME_001_EVIDENCE.md

Tests run:
- `.\scripts\validate-system-protocol-registry.ps1` (PASS)
- `dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~SysProtocolRuntime001Tests|FullyQualifiedName~SysPostLockDeltaRegister001Tests"` (PASS)
- `dotnet test tests/Ontogony.Evaluation.Contracts.Tests/Ontogony.Evaluation.Contracts.Tests.csproj` (PASS)
- `dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~EvidenceSpineExportBundleSchemaTests|FullyQualifiedName~EvalEvidenceExportBundleSchemaTests|FullyQualifiedName~SysProtocolRuntime001Tests"` (PASS)
- `dotnet test tests/Ontogony.Messaging.Tests/Ontogony.Messaging.Tests.csproj` (PASS)
- `dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~SchemaFixtureValidationTests|FullyQualifiedName~HeaderConstantsSnapshotTests|FullyQualifiedName~CloudEventsRoundTripTests"` (PASS)
- `dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj` (PASS)

Docker/browser validation:
- Not run (this change is registry/schema/validator/test coverage only).

Known limitations:
- Metadata is currently curated in the registry document; it is not yet auto-derived from runtime events.
- Evidence payload schemas and evaluation DTO references now carry the metadata, but producer services still need to populate values end-to-end on live responses.

Safety statement:
- Real external tool execution remains blocked.
- This is Docker-local/operator scope, not production readiness.
- Model assistance remains draft-only/non-authoritative where applicable.
- No semantic authority was moved out of Kanon.

Verdict:
- PASS — protocol surfaces now include `protocolId`, `authorityMode`, and `sideEffectLevel`, with validator and test enforcement.
