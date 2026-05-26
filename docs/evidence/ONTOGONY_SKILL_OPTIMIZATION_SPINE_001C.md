# Evidence: ONTOGONY-SKILL-OPTIMIZATION-SPINE-001C (platform record)

**Slice:** 001C — Allagma offline optimization run registry  
**Date:** 2026-05-26  
**Lead repo:** `allagma-dotnet`  
**Platform impact:** None (no new platform packages; 001A contracts/docs remain authoritative)

---

## Summary

001C adds the Allagma-side `SkillOptimizationRun` lifecycle, in-memory persistence, a deterministic fake orchestrator, and REST endpoints. It also promotes `POST /ontology/v0/skill-edits/evaluate` from `ServerOnly` to `Client` in `kanon-dotnet`, adding `IKanonSkillGovernanceClient` to the typed client library.

Platform role is observational — all changes are in `allagma-dotnet` and `kanon-dotnet`.

---

## Cross-repo relationship

| Slice | Repo | Status |
|-------|------|--------|
| 001A — Platform contracts/docs/schemas | `ontogony-platform` | **Complete** |
| 001B — Kanon fake governance vertical slice | `kanon-dotnet` | **Complete** |
| 001C — Allagma offline optimization run registry | `allagma-dotnet` | **Complete** |
| 001D — Conexus optimizer/target model evidence | `conexus-dotnet` | **Deferred** |
| 001E — Frontend Skill Lab | `ontogony-frontend` | **Deferred** |

---

## What changed in 001C

### `allagma-dotnet`

- `src/Allagma.Contracts/SkillContracts.cs` — run lifecycle contracts
- `src/Allagma.Application/SkillOptimization/` — record, repository port, in-memory adapter, run service, fake orchestrator
- `src/Allagma.Api/SkillOptimizationEndpoints.cs` — 6 REST endpoints under `/allagma/v0/skill-optimization/runs`
- 18 new tests, all passing

### `kanon-dotnet`

- `src/Kanon.Client/IKanonSkillGovernanceClient.cs` — typed client interface for `EvaluateSkillEditAsync`
- `src/Kanon.Client/KanonSkillGovernanceClient.cs` — implementation binding to `POST /ontology/v0/skill-edits/evaluate`
- Route promoted from `ServerOnly` to `Client`; manifest regenerated (85 Client / 18 ServerOnly)
- Kanon test suite: 744 passing

---

## Hard boundaries verified

All 001C non-negotiables preserved:

| Rule | Verified by |
|------|-------------|
| `IsLiveDeploymentAllowed: false` always | `FakeSkillOptimizationOrchestrator.RunSimulatedGate` hardcode + 2 dedicated test assertions |
| No live optimizer model calls | Fake orchestrator only; no model call integration |
| No Conexus runtime injection | Not present in 001C |
| No fabricated evaluation scores | Gate score is a declared constant (0.85m), not a model output |
| No hidden chain-of-thought storage | No CoT fields in any 001C record types |

---

## Verification

```powershell
# Allagma 001C tests
cd C:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests --filter SkillOptimizationRunTests
# Expected: 18/18 pass

# Kanon tests (post client-route promotion)
cd C:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests
# Expected: 744 passing, 2 skipped
```

---

## Next

001D: Conexus adds `SkillInjectionMetadata` to optimizer/target model-call admin DTOs. Blocked on 001C stability.  
001E: Frontend Skill Lab. Blocked on 001C/001D backend stability.
