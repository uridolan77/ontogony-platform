# Sandbox Consumer Activation (protocol index)

**Package:** `ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001` · **Slice:** `001A` (contracts + schemas + fixtures)  
**Status:** 001A — platform contracts and fixture skeleton  
**Precondition:** [Skill Release Governance](./SKILL_RELEASE_GOVERNANCE.md) closed (001A–001G).

Sandbox consumers resolve **active governed skill bindings** from Allagma and record which skill version was applied. Not production deployment or consumer-side release policy.

## Boundary

| Repo | Owns |
| --- | --- |
| `ontogony-platform` | Cross-repo contract docs, JSON schemas, fixture shapes |
| `allagma-dotnet` | Authoritative binding resolver (`001B`) |
| `ontogony-consumers` | Consumer kit client + demo wiring (`001C`–`001D`) |

**Authority:** Allagma is the release **binding** authority. Consumers ask; they do not decide promotion, pause, or rollback.

## Contract family (v0)

| Contract | Doc | Schema |
| --- | --- | --- |
| Binding resolution | [SKILL_CONSUMER_BINDING_RESOLUTION_V0.md](../contracts/SKILL_CONSUMER_BINDING_RESOLUTION_V0.md) | [skill-consumer-binding-resolution.v0.schema.json](../schemas/sandbox-consumer-activation/skill-consumer-binding-resolution.v0.schema.json) |
| Execution context | [SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md](../contracts/SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md) | [skill-consumer-execution-context.v0.schema.json](../schemas/sandbox-consumer-activation/skill-consumer-execution-context.v0.schema.json) |
| Version applied evidence | [SKILL_VERSION_APPLIED_EVIDENCE_V0.md](../contracts/SKILL_VERSION_APPLIED_EVIDENCE_V0.md) | [skill-version-applied-evidence.v0.schema.json](../schemas/sandbox-consumer-activation/skill-version-applied-evidence.v0.schema.json) |
| Activation event | [SANDBOX_CONSUMER_ACTIVATION_EVENT_V0.md](../contracts/SANDBOX_CONSUMER_ACTIVATION_EVENT_V0.md) | — (envelope; payloads reference schemas above) |

Release bindings: [SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md](../contracts/SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md).

## Fixtures

| Fixture | Path |
| --- | --- |
| Active sandbox binding resolved | [`binding-resolution.sandbox-active.json`](../schemas/fixtures/sandbox-consumer-activation/binding-resolution.sandbox-active.json) |
| No active binding (fallback) | [`binding-resolution.no-active-binding.json`](../schemas/fixtures/sandbox-consumer-activation/binding-resolution.no-active-binding.json) |
| Paused binding (fallback) | [`binding-resolution.paused-binding.json`](../schemas/fixtures/sandbox-consumer-activation/binding-resolution.paused-binding.json) |
| Rolled back binding (fallback) | [`binding-resolution.rolled-back-binding.json`](../schemas/fixtures/sandbox-consumer-activation/binding-resolution.rolled-back-binding.json) |
| Forbidden production request | [`binding-resolution-request.forbidden-production.json`](../schemas/fixtures/sandbox-consumer-activation/binding-resolution-request.forbidden-production.json) |
| Execution context (resolved) | [`execution-context.sandbox-resolved.json`](../schemas/fixtures/sandbox-consumer-activation/execution-context.sandbox-resolved.json) |
| Evidence (applied version) | [`skill-version-applied.sandbox.json`](../schemas/fixtures/sandbox-consumer-activation/skill-version-applied.sandbox.json) |

## Phase 1 validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SandboxConsumerActivationSchemaTests
```

Evidence: [`docs/evidence/ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001A.md`](../evidence/ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001A.md)

## Non-goals (001A)

- No production, live, or `default-runtime` `targetEnvironment` in schemas or golden fixtures.
- No Allagma resolver, consumer client, or anti-sycophancy chat implementation (later slices).
- No automatic promotion, progressive rollout, or background self-improvement.
- No consumer-side release policy or skill mutation.

## Recommended slice order

| Slice | Focus |
| --- | --- |
| **001A** | Platform contracts/schemas (this slice) |
| **001B** | Allagma sandbox binding resolver |
| **001C** | Consumer kit resolution client |
| **001D+** | Anti-sycophancy chat, evidence, pause/rollback proof, golden fixture |
