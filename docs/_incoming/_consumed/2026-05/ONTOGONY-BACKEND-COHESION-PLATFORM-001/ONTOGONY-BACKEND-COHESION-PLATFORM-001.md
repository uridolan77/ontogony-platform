Platform-owned backend cohesion truth layer
Goal

Create the canonical backend cohesion manifest that all runtime backend repos validate against.

ontogony-platform should define the shared truth for:

backend repo roles
feature spines
golden scenarios
known deferrals
evidence index
schema/manifest validation
acceptance gates

This package does not implement runtime behavior.

Repos covered
Platform truth repo:
- ontogony-platform

Runtime backend repos:
- allagma-dotnet
- kanon-dotnet
- conexus-dotnet
- metabole-dotnet
Deliverables
docs/protocols/BACKEND_SYSTEM_COHESION.md
docs/system/BACKEND_FEATURE_COHESION_MATRIX.md
docs/system/BACKEND_GOLDEN_SCENARIOS.md
docs/system/BACKEND_COHESION_DEFERRALS.md
docs/system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json
docs/schemas/backend-cohesion/backend-cohesion-manifest.v0.schema.json
docs/schemas/fixtures/backend-cohesion/backend-cohesion-manifest.example.json
docs/evidence/BACKEND_COHESION_EVIDENCE_INDEX.md
docs/evidence/ONTOGONY_BACKEND_COHESION_PLATFORM_001.md
tests/Ontogony.Infrastructure.Tests/BackendCohesionManifestSchemaTests.cs
Feature spines to include
Decision reconstructability spine
Workflow lifecycle spine
Budget/cost governance spine
Skill optimization spine
Skill release governance spine
Sandbox consumer activation spine
Kanon semantic authority spine
Conexus routing/model-call evidence spine
Metabole transformation/evolution spine
Cross-service error envelope spine
Trace/correlation/idempotency spine
OpenAPI/route inventory discipline spine
Evidence export/read-model spine
Golden scenarios
A. Decision reconstructability
B. Workflow + budget + Conexus model call
C. Skill optimization
D. Skill release governance
E. Sandbox consumer activation
F. Error / trace / correlation discipline
G. Metabole transformation provenance
Required deferrals registry

Include known deferrals explicitly:

Live Allagma → Candor Chat consumer E2E
Allagma skill-release OpenAPI snapshot sync
Production deployment controls
Kanon rollback governance evaluation
Kanon binding governance evaluation
Conexus real provider coverage beyond fake/local safe paths
Metabole role/evolution integration validation
Frontend global typecheck unrelated error if still present
Known unrelated backend full-suite failures if still present
Tests
schema loads
manifest fixture validates
platformTruthRepo = ontogony-platform
runtimeBackendRepos includes:
  allagma-dotnet
  kanon-dotnet
  conexus-dotnet
  metabole-dotnet
golden scenario ids are unique
deferral ids are unique
each feature spine has status
closed feature spines have evidence docs
production deployment is not marked complete
Acceptance
1. Backend cohesion protocol exists.
2. Backend feature matrix exists.
3. Golden scenario catalog exists.
4. Machine-readable manifest exists.
5. Manifest schema + fixture validate.
6. Deferrals are explicit.
7. Evidence index exists.
8. Tests pass.
9. Runtime backend packages can validate against this manifest.
Suggested commit
docs(platform): add backend cohesion manifest for runtime backends
Cursor prompt
Implement ONTOGONY-BACKEND-COHESION-PLATFORM-001 in ontogony-platform.

Goal:
Create the canonical platform-owned backend cohesion truth layer for:
- allagma-dotnet
- kanon-dotnet
- conexus-dotnet
- metabole-dotnet

ontogony-platform is the contract/fixture/manifest authority, not a runtime backend.

Deliver:
1. docs/protocols/BACKEND_SYSTEM_COHESION.md
2. docs/system/BACKEND_FEATURE_COHESION_MATRIX.md
3. docs/system/BACKEND_GOLDEN_SCENARIOS.md
4. docs/system/BACKEND_COHESION_DEFERRALS.md
5. docs/system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json
6. docs/schemas/backend-cohesion/backend-cohesion-manifest.v0.schema.json
7. docs/schemas/fixtures/backend-cohesion/backend-cohesion-manifest.example.json
8. docs/evidence/BACKEND_COHESION_EVIDENCE_INDEX.md
9. docs/evidence/ONTOGONY_BACKEND_COHESION_PLATFORM_001.md
10. BackendCohesionManifestSchemaTests

Include feature spines:
- decision reconstructability
- workflow lifecycle
- budget/cost governance
- skill optimization
- skill release governance
- sandbox consumer activation
- Kanon semantic authority
- Conexus routing/model-call evidence
- Metabole transformation/evolution
- error envelope
- trace/correlation/idempotency
- OpenAPI/route inventory discipline
- evidence export/read-model

Golden scenarios:
A. decision reconstructability
B. workflow + budget + Conexus call
C. skill optimization
D. skill release governance
E. sandbox consumer activation
F. error/trace/correlation discipline
G. Metabole transformation provenance

Hard boundaries:
- no runtime code
- no service endpoints
- no production deployment
- no live consumer E2E
- no frontend work