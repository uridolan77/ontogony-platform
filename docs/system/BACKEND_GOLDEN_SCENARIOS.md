# Backend golden scenarios

**Manifest:** [`backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json`](./backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json)

Golden scenarios are end-to-end **proof obligations** for runtime backends. Each scenario maps to one or more feature spines and a validation package in the owning repo(s).

## Scenario catalog

### A — Decision reconstructability

| Field | Value |
| --- | --- |
| **Id** | `golden-A-decision-reconstructability` |
| **Owner** | `kanon-dotnet` |
| **Participants** | `allagma-dotnet`, `conexus-dotnet` |
| **Spines** | `decision-reconstructability` |
| **Validation** | `KANON-BACKEND-COHESION-VALIDATION-001` slice 001C |

**Path:** decision record → decision event projection → reconstructability classifier → report grade → diagnostics for missing evidence.

---

### B — Workflow + budget + Conexus model call

| Field | Value |
| --- | --- |
| **Id** | `golden-B-workflow-budget-conexus` |
| **Owner** | `allagma-dotnet` |
| **Participants** | `conexus-dotnet`, `kanon-dotnet` |
| **Spines** | `workflow-lifecycle`, `budget-cost-governance`, `conexus-routing-model-call-evidence` |
| **Validation** | `ALLAGMA-BACKEND-COHESION-VALIDATION-001` slice 001C |

**Path:** create/run workflow → budget policy applied → Conexus model call → route evidence collected → budget usage recorded → run evidence export links model call and cost.

---

### C — Skill optimization

| Field | Value |
| --- | --- |
| **Id** | `golden-C-skill-optimization` |
| **Owner** | `allagma-dotnet` |
| **Participants** | `kanon-dotnet`, `conexus-dotnet` |
| **Spines** | `skill-optimization`, `kanon-semantic-authority` |
| **Validation** | `ALLAGMA-BACKEND-COHESION-VALIDATION-001` slice 001D |

**Path:** skill optimization run → Conexus rollout evidence → fake or `real_dry_run` proposal → Kanon skill-edit evaluation → candidate persisted → evidence export.

---

### D — Skill release governance

| Field | Value |
| --- | --- |
| **Id** | `golden-D-skill-release-governance` |
| **Owner** | `allagma-dotnet`, `kanon-dotnet` |
| **Participants** | `ontogony-platform` |
| **Spines** | `skill-release-governance`, `kanon-semantic-authority` |
| **Validation** | `ALLAGMA-BACKEND-COHESION-VALIDATION-001` slice 001E |

**Path:** promotion request → Kanon governance → sandbox binding → release evidence → operator-visible state (no live deployment).

---

### E — Sandbox consumer activation

| Field | Value |
| --- | --- |
| **Id** | `golden-E-sandbox-consumer-activation` |
| **Owner** | `allagma-dotnet` |
| **Participants** | `ontogony-platform` |
| **Spines** | `sandbox-consumer-activation` |
| **Validation** | `ALLAGMA-BACKEND-COHESION-VALIDATION-001` slice 001F |

**Path:** sandbox binding resolver → consumer activation contract → traceable binding evidence (governed-fake first; live consumer deferred).

---

### F — Error / trace / correlation discipline

| Field | Value |
| --- | --- |
| **Id** | `golden-F-error-trace-correlation` |
| **Owner** | `ontogony-platform` (contract); each runtime proves conformance |
| **Participants** | all four runtime backends |
| **Spines** | `error-envelope`, `trace-correlation-idempotency` |
| **Validation** | Per-repo `*PlatformConformanceTests` + cohesion slice 001G/001H |

**Path:** cross-service calls preserve headers → error envelope shape → idempotency where required → no fabricated telemetry.

---

### G — Metabole transformation provenance

| Field | Value |
| --- | --- |
| **Id** | `golden-G-metabole-transformation-provenance` |
| **Owner** | `metabole-dotnet` |
| **Participants** | `kanon-dotnet`, `conexus-dotnet` |
| **Spines** | `metabole-transformation-evolution` |
| **Validation** | Metabole foundation + future cohesion validation |

**Path:** source schema profile → transformation/run record → provenance and lineage → Kanon validation request (advisory) → operator review payload.

---

## Uniqueness

Scenario ids are unique in the manifest (enforced by `BackendCohesionManifestSchemaTests`).

## Non-claims

Passing a golden scenario in governed-fake mode does **not** imply production readiness or live external provider coverage.
