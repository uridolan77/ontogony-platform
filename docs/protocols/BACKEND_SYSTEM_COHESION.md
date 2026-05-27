# Backend system cohesion (protocol index)

**Package:** `ONTOGONY-BACKEND-COHESION-PLATFORM-001`  
**Status:** **Closed** (platform truth layer, 2026-05-27)  
**Closeout:** [`docs/evidence/ONTOGONY_BACKEND_COHESION_PLATFORM_001.md`](../evidence/ONTOGONY_BACKEND_COHESION_PLATFORM_001.md)

`ontogony-platform` owns the **canonical backend cohesion manifest** that runtime backends validate against. This package defines contract truth only — no runtime behavior, endpoints, or production deployment.

## Boundary

| Repo | Role |
| --- | --- |
| `ontogony-platform` | Manifest, JSON schema, fixtures, feature matrix, golden scenarios, deferrals registry, evidence index |
| `allagma-dotnet` | Governed execution — workflow, budget, skill loops, sandbox resolver, evidence export |
| `kanon-dotnet` | Semantic authority — decisions, governance, provenance, ontology |
| `conexus-dotnet` | Model gateway — routing, aliases, model-call evidence |
| `metabole-dotnet` | Data transformation — schema profiling, SLOD candidates, lineage |

```text
Kanon owns meaning.
Allagma owns governed execution.
Conexus owns model access.
Metabole owns transformation candidates.
Ontogony.Platform owns shared mechanics and backend cohesion truth.
```

## Canonical artifacts

| Artifact | Path |
| --- | --- |
| Machine-readable manifest | [`docs/system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json`](../system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json) |
| JSON Schema | [`docs/schemas/backend-cohesion/backend-cohesion-manifest.v0.schema.json`](../schemas/backend-cohesion/backend-cohesion-manifest.v0.schema.json) |
| Example fixture | [`docs/schemas/fixtures/backend-cohesion/backend-cohesion-manifest.example.json`](../schemas/fixtures/backend-cohesion/backend-cohesion-manifest.example.json) |
| Feature matrix | [`docs/system/BACKEND_FEATURE_COHESION_MATRIX.md`](../system/BACKEND_FEATURE_COHESION_MATRIX.md) |
| Golden scenarios | [`docs/system/BACKEND_GOLDEN_SCENARIOS.md`](../system/BACKEND_GOLDEN_SCENARIOS.md) |
| Deferrals registry | [`docs/system/BACKEND_COHESION_DEFERRALS.md`](../system/BACKEND_COHESION_DEFERRALS.md) |
| Evidence index | [`docs/evidence/BACKEND_COHESION_EVIDENCE_INDEX.md`](../evidence/BACKEND_COHESION_EVIDENCE_INDEX.md) |

Runtime repos consume the manifest via alignment tests in their own validation packages:

- `ALLAGMA-BACKEND-COHESION-VALIDATION-001`
- `KANON-BACKEND-COHESION-VALIDATION-001`
- `CONEXUS-BACKEND-COHESION-VALIDATION-001`
- Metabole follow-on: `METABOLE-BACKEND-COHESION-VALIDATION-001` (001A–001G complete; 001H–001J in progress, 2026-05-27)

## Feature spines (v0)

Thirteen cross-cutting spines are indexed in the manifest. Platform-closed spines have contract evidence; runtime-owned spines are `in_progress` until sibling validation packages close.

| Spine | Primary owner |
| --- | --- |
| Decision reconstructability | Kanon |
| Workflow lifecycle | Allagma |
| Budget / cost governance | Allagma |
| Skill optimization | Allagma (+ Kanon, Conexus) |
| Skill release governance | Allagma + Kanon |
| Sandbox consumer activation | Allagma |
| Kanon semantic authority | Kanon |
| Conexus routing / model-call evidence | Conexus |
| Metabole transformation / evolution | Metabole |
| Cross-service error envelope | Platform |
| Trace / correlation / idempotency | Platform |
| OpenAPI / route inventory discipline | Platform + all runtimes |
| Evidence export / read-model | Allagma (+ platform spine contract) |

## Golden scenarios (A–G)

| Id | Scenario |
| --- | --- |
| A | Decision reconstructability |
| B | Workflow + budget + Conexus model call |
| C | Skill optimization |
| D | Skill release governance |
| E | Sandbox consumer activation |
| F | Error / trace / correlation discipline |
| G | Metabole transformation provenance |

See [`BACKEND_GOLDEN_SCENARIOS.md`](../system/BACKEND_GOLDEN_SCENARIOS.md) for ownership and validation package mapping.

## Validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~BackendCohesionManifestSchemaTests
```

## Non-goals

- No runtime code or service endpoints in this package.
- No production deployment charter.
- No live consumer E2E (Candor Chat, etc.) — explicit deferrals only.
- No frontend implementation.
