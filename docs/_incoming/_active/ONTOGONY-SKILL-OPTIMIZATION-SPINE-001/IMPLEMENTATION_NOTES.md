# ONTOGONY-SKILL-OPTIMIZATION-SPINE-001 — Implementation notes

## Slice 001A — Contracts and fixtures (in progress)

**Started:** 2026-05-26  
**Repo:** `ontogony-platform` (lead for phase 1)

### Promoted to canonical

| Item | Path |
| --- | --- |
| Protocol index | [`docs/protocols/SKILL_OPTIMIZATION_SPINE.md`](../../protocols/SKILL_OPTIMIZATION_SPINE.md) |
| Lifecycle | [`docs/protocols/SKILL_ARTIFACT_LIFECYCLE.md`](../../protocols/SKILL_ARTIFACT_LIFECYCLE.md) |
| Contracts (8) | [`docs/contracts/SKILL_*_V0.md`](../../contracts/) |
| JSON schemas (7) | [`docs/schemas/skill-optimization/`](../../schemas/skill-optimization/) |
| Example fixture | [`docs/schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json`](../../schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json) |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SkillOptimizationSpineSchemaTests.cs` |

### Next (001B — Kanon fake vertical slice)

Per [`ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/07_rollout/PHASED_EXECUTION_PLAN.md`](./ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/07_rollout/PHASED_EXECUTION_PLAN.md) phase 2:

- Kanon skill validation routes + decision records
- Allagma deterministic optimization run fixture
- Conexus skill injection metadata (fake provider)

### Commands (001A)

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillOptimizationSpineSchemaTests
```

### Intake

Package tree: [`ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/`](./ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/) (nested export layout).
