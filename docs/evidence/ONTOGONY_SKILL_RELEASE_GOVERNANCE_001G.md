# Evidence: ONTOGONY-SKILL-RELEASE-GOVERNANCE-001G (cross-service golden fixture)

**Slice:** 001G — Cross-service golden fixture  
**Date:** 2026-05-27  
**Package:** ONTOGONY-SKILL-RELEASE-GOVERNANCE-001  
**Precondition:** 001A–001F closed (platform contracts, Kanon governance, Allagma lifecycle, frontend Skill Lab)

---

## Goal

Prove the full **manual sandbox** release-governance path end-to-end:

```text
accepted skill optimization candidate
→ sandbox promotion request
→ Kanon promotion evaluation (test stub)
→ approved_for_sandbox
→ sandbox deployment binding
→ activate
→ pause
→ rollback
→ evidence links optimization run + promotion + binding + rollback
```

No production, live, or default-runtime targets. No consumer activation.

---

## Canonical scenario artifact

| Artifact | Path |
| --- | --- |
| Golden path lifecycle | [`docs/schemas/fixtures/skill-release/golden-path.lifecycle.json`](../schemas/fixtures/skill-release/golden-path.lifecycle.json) |
| Approved promotion (reference) | [`promotion-request.approved-sandbox.json`](../schemas/fixtures/skill-release/promotion-request.approved-sandbox.json) |
| Active sandbox binding (reference) | [`deployment-binding.sandbox-active.json`](../schemas/fixtures/skill-release/deployment-binding.sandbox-active.json) |
| Executed rollback (reference) | [`rollback.executed.json`](../schemas/fixtures/skill-release/rollback.executed.json) |

Protocol index: [`docs/protocols/SKILL_RELEASE_GOVERNANCE.md`](../protocols/SKILL_RELEASE_GOVERNANCE.md)

---

## Executable proof (Allagma)

| Test | Repo | Filter |
| --- | --- | --- |
| `SkillReleaseGovernanceGoldenPathTests.Golden_path_proves_sandbox_release_lifecycle` | `allagma-dotnet` | `FullyQualifiedName~SkillReleaseGovernanceGoldenPath` |

Evidence: `allagma-dotnet/docs/evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001G.md`

---

## Acceptance criteria (001G)

| # | Criterion | Proof |
| --- | --- | --- |
| 1 | Optimization run has accepted candidate + promote gate | Golden test step 1 |
| 2 | Promotion request created for candidate | Golden test step 2 |
| 3 | Submit calls Kanon promotion governance | Golden test step 3 (stub) |
| 4 | Promotion becomes `approved_for_sandbox` | Golden test step 3 |
| 5 | Sandbox binding from approved promotion | Golden test step 4 |
| 6 | Binding activates | Golden test step 5 |
| 7 | Binding pauses | Golden test step 6 |
| 8 | Binding rolls back with reason | Golden test step 7 |
| 9 | Rollback links binding, promotion, source run | Golden test evidence refs |
| 10 | No production/live/default-runtime targets | Golden test assertions |
| 11 | Limitations: sandbox-only / no production deployment | Promotion evidence + binding responses |

---

## Platform validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter "FullyQualifiedName~SkillReleaseGovernance"
```

Result: **10 passed** (includes `Golden_path_fixture_has_required_steps_and_sandbox_only_targets`).

```powershell
cd C:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~SkillReleaseGovernanceGoldenPath"
```

Result: **1 passed** (`Golden_path_proves_sandbox_release_lifecycle`).

---

## Frontend note (001F)

Skill Lab / release-governance files typecheck clean.  
Global `npm run typecheck` still reports a **pre-existing** error in `buildTraceReconstructionView.ts`, unrelated to 001F/001G.

---

## Package status after 001G

```text
001A — Platform contracts: closed
001B — Kanon promotion governance: closed
001C — Allagma promotion registry: closed
001D — Sandbox deployment binding: closed
001E — Rollback lifecycle: closed
001F — Frontend release governance UI: closed
001G — Cross-service golden fixture: closed
```

Consumer activation remains a **later** package (`ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001`), not `ontogony-consumers` in this slice.
