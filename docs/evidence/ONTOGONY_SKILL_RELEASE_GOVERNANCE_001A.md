# Evidence: ONTOGONY-SKILL-RELEASE-GOVERNANCE-001A (platform)

**Slice:** 001A — Platform release-governance contracts, schemas, fixtures  
**Date:** 2026-05-27  
**Repo:** `ontogony-platform`

---

## Summary

001A promotes the neutral release-governance contract family and JSON schemas for sandbox-only skill promotion. No runtime code in product repos for this slice.

---

## Delivered

| Artifact | Path |
| --- | --- |
| Protocol index | `docs/protocols/SKILL_RELEASE_GOVERNANCE.md` |
| Sandbox lifecycle | `docs/protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md` |
| Contracts (6) | `docs/contracts/SKILL_RELEASE_*_V0.md`, `SKILL_ROLLBACK_V0.md`, `SKILL_VERSION_RELEASE_STATE_V0.md` |
| JSON schemas (5) | `docs/schemas/skill-release/*.v0.schema.json` |
| Fixtures (3) | `docs/schemas/fixtures/skill-release/` |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SkillReleaseGovernanceSchemaTests.cs` |

---

## Verification

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillReleaseGovernanceSchemaTests
```

Expected: all tests pass; fixtures use only `sandbox` / allowed enums; no `production` / `live` / `default-runtime` in golden fixtures.

---

## Next

| Slice | Repo | Focus |
| --- | --- | --- |
| **001B** | `kanon-dotnet` | Fake promotion evaluation endpoint + decision types |
| **001C** | `allagma-dotnet` | Promotion request registry (no binding activation yet) |

Do not start deployment activation before promotion governance is in place.
