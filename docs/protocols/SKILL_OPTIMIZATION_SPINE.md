# Skill Optimization Spine (protocol index)

**Package:** `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001` · **Status:** **Closed** (slices 001A–001K, 2026-05-27)  
**Closeout:** [`docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md`](../evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md) · **Archive:** [`docs/_incoming/_consumed/2026-05/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/`](../_incoming/_consumed/2026-05/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/)

Ontogony-native governed skill lifecycle: versioned procedural knowledge optimized under Kanon authority, executed via Allagma, with model access through Conexus. Not a standalone prompt toy.

## Boundary

| Repo | Owns |
| --- | --- |
| `ontogony-platform` | Cross-repo contract docs, JSON schemas, fixture shapes, RC checklist |
| `kanon-dotnet` | Skill governance semantics, validation decisions, evidence graph roots |
| `allagma-dotnet` | `SkillOptimization` run orchestration, operations, replay fixtures |
| `conexus-dotnet` | Target-model skill injection metadata; optimizer route (fixture first) |
| `ontogony-frontend` / `ontogony-ui` | Skill Lab operator surfaces |

## Contract family (v0)

| Contract | Doc | Schema |
| --- | --- | --- |
| Skill artifact identity | [SKILL_ARTIFACT_CONTRACT_V0.md](../contracts/SKILL_ARTIFACT_CONTRACT_V0.md) | [skill-artifact.schema.json](../schemas/skill-optimization/skill-artifact.schema.json) |
| Immutable version | [SKILL_VERSION_CONTRACT_V0.md](../contracts/SKILL_VERSION_CONTRACT_V0.md) | [skill-version.schema.json](../schemas/skill-optimization/skill-version.schema.json) |
| Atomic patch | [SKILL_EDIT_CONTRACT_V0.md](../contracts/SKILL_EDIT_CONTRACT_V0.md) | [skill-edit.schema.json](../schemas/skill-optimization/skill-edit.schema.json) |
| Optimization run | [SKILL_OPTIMIZATION_RUN_CONTRACT_V0.md](../contracts/SKILL_OPTIMIZATION_RUN_CONTRACT_V0.md) | [skill-optimization-run.schema.json](../schemas/skill-optimization/skill-optimization-run.schema.json) |
| Held-out gate | [SKILL_EVALUATION_GATE_CONTRACT_V0.md](../contracts/SKILL_EVALUATION_GATE_CONTRACT_V0.md) | [skill-evaluation-gate.schema.json](../schemas/skill-optimization/skill-evaluation-gate.schema.json) |
| Rejected edits (negative evidence) | [REJECTED_SKILL_EDIT_BUFFER_V0.md](../contracts/REJECTED_SKILL_EDIT_BUFFER_V0.md) | [rejected-skill-edit-buffer.schema.json](../schemas/skill-optimization/rejected-skill-edit-buffer.schema.json) |
| Runtime binding | [SKILL_DEPLOYMENT_BINDING_V0.md](../contracts/SKILL_DEPLOYMENT_BINDING_V0.md) | [skill-deployment-binding.schema.json](../schemas/skill-optimization/skill-deployment-binding.schema.json) |
| Evidence links | [SKILL_EVIDENCE_LINKS_V0.md](../contracts/SKILL_EVIDENCE_LINKS_V0.md) | — |

Lifecycle narrative: [SKILL_ARTIFACT_LIFECYCLE.md](./SKILL_ARTIFACT_LIFECYCLE.md)

## Fixtures

| Fixture | Path |
| --- | --- |
| Example optimization run | [`docs/schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json`](../schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json) |

## Phase 1 validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillOptimizationSpineSchemaTests
```

Intake archive notes: [`docs/_incoming/_consumed/2026-05/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/IMPLEMENTATION_NOTES.md`](../_incoming/_consumed/2026-05/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/IMPLEMENTATION_NOTES.md)

**Follow-on (closed):** [`ONTOGONY-SKILL-RELEASE-GOVERNANCE-001`](../_incoming/_consumed/2026-05/ONTOGONY-SKILL-RELEASE-GOVERNANCE-001/) — promotion and sandbox release (001A–001G). Next: **`ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001`** (real sandbox consumer wiring; not yet opened).

## Non-goals (001A)

- No live optimizer provider calls in normal inference paths.
- No autonomous publish without Kanon decision + operator gate where required.
- No duplicate mechanical protocols (trace, errors, idempotency remain in [MECHANICAL_PROTOCOL_REGISTRY.md](../contracts/MECHANICAL_PROTOCOL_REGISTRY.md)).
