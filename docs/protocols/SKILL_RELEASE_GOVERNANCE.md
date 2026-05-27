# Skill Release Governance (protocol index)

**Package:** `ONTOGONY-SKILL-RELEASE-GOVERNANCE-001` · **Slice:** `001A` (contracts + schemas + fixtures)  
**Status:** Slices 001A–001G closed (contracts, Kanon/Allagma lifecycle, frontend Skill Lab, golden fixture). Consumer activation is a later package.  
**Precondition:** [Skill Optimization Spine](./SKILL_OPTIMIZATION_SPINE.md) closed (001A–001K).

Governed manual promotion and **sandbox-only** activation of accepted skill candidates. Not autonomous production deployment.

## Boundary

| Repo | Owns |
| --- | --- |
| `ontogony-platform` | Cross-repo contract docs, JSON schemas, fixture shapes |
| `kanon-dotnet` | Promotion/binding/rollback governance decisions |
| `allagma-dotnet` | Promotion registry, binding lifecycle, rollback orchestration, evidence export |
| `conexus-dotnet` | Read-only optimizer evidence refs from source optimization runs |
| `ontogony-frontend` | Skill Lab release views (manual promotion, evidence, rollback) |

## Contract family (v0)

| Contract | Doc | Schema |
| --- | --- | --- |
| Promotion request | [SKILL_RELEASE_PROMOTION_REQUEST_V0.md](../contracts/SKILL_RELEASE_PROMOTION_REQUEST_V0.md) | [skill-promotion-request.v0.schema.json](../schemas/skill-release/skill-promotion-request.v0.schema.json) |
| Release decision | [SKILL_RELEASE_DECISION_V0.md](../contracts/SKILL_RELEASE_DECISION_V0.md) | [skill-release-decision.v0.schema.json](../schemas/skill-release/skill-release-decision.v0.schema.json) |
| Sandbox deployment binding | [SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md](../contracts/SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md) | [skill-deployment-binding.v0.schema.json](../schemas/skill-release/skill-deployment-binding.v0.schema.json) |
| Rollback | [SKILL_ROLLBACK_V0.md](../contracts/SKILL_ROLLBACK_V0.md) | [skill-rollback.v0.schema.json](../schemas/skill-release/skill-rollback.v0.schema.json) |
| Evidence export | [SKILL_RELEASE_EVIDENCE_EXPORT_V0.md](../contracts/SKILL_RELEASE_EVIDENCE_EXPORT_V0.md) | [skill-release-evidence-export.v0.schema.json](../schemas/skill-release/skill-release-evidence-export.v0.schema.json) |
| Version release state | [SKILL_VERSION_RELEASE_STATE_V0.md](../contracts/SKILL_VERSION_RELEASE_STATE_V0.md) | — (lifecycle protocol) |

Lifecycle narrative: [SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md](./SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md)

**Note:** Optimization-spine [SKILL_DEPLOYMENT_BINDING_V0.md](../contracts/SKILL_DEPLOYMENT_BINDING_V0.md) covers Conexus runtime injection metadata; release bindings use the contracts above.

## Fixtures

| Fixture | Path |
| --- | --- |
| Approved sandbox promotion | [`promotion-request.approved-sandbox.json`](../schemas/fixtures/skill-release/promotion-request.approved-sandbox.json) |
| Active sandbox binding | [`deployment-binding.sandbox-active.json`](../schemas/fixtures/skill-release/deployment-binding.sandbox-active.json) |
| Executed rollback | [`rollback.executed.json`](../schemas/fixtures/skill-release/rollback.executed.json) |
| Golden path lifecycle (001G) | [`golden-path.lifecycle.json`](../schemas/fixtures/skill-release/golden-path.lifecycle.json) |

## Phase 1 validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillReleaseGovernanceSchemaTests
```

Intake archive: [`docs/_incoming/_consumed/2026-05/ONTOGONY-SKILL-RELEASE-GOVERNANCE-001/`](../_incoming/_consumed/2026-05/ONTOGONY-SKILL-RELEASE-GOVERNANCE-001/) · Closeout: [`docs/evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md`](../evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md)

## Non-goals (001A and v0 package)

- No production or live `targetEnvironment` values in schemas or golden fixtures.
- No automatic promotion, progressive rollout, or real consumer activation.
- No runtime skill injection into production traffic.
- No Allagma/Kanon/frontend implementation in 001A (contracts and schemas only).

## Recommended slice order

| Slice | Focus |
| --- | --- |
| **001A** | Platform contracts/schemas (this slice) |
| **001B** | Kanon sandbox promotion governance (fake vertical slice) |
| **001C** | Allagma promotion request registry |
| **001D+** | Sandbox binding, rollback, frontend — after promotion is governed |
