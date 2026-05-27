# CONSUMED — ONTOGONY-SKILL-RELEASE-GOVERNANCE-001

**Archived:** 2026-05-27  
**Reason:** ONTOGONY-SKILL-RELEASE-GOVERNANCE-001 closed on 2026-05-27. Scope completed: manual sandbox promotion, Kanon governance, Allagma promotion/binding/rollback lifecycle, frontend Skill Lab release UI, and cross-service golden fixture. Production deployment, live consumer activation, and autonomous promotion remain out of scope.

## Canonical promotion

| Artifact | Location |
| --- | --- |
| Protocol index | [`docs/protocols/SKILL_RELEASE_GOVERNANCE.md`](../../../protocols/SKILL_RELEASE_GOVERNANCE.md) |
| Sandbox activation lifecycle | [`docs/protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md`](../../../protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md) |
| Contract family (v0) | [`docs/contracts/`](../../../contracts/) (`SKILL_RELEASE_*_V0.md`, `SKILL_ROLLBACK_V0.md`) |
| JSON schemas | [`docs/schemas/skill-release/`](../../../schemas/skill-release/) |
| Fixtures | [`docs/schemas/fixtures/skill-release/`](../../../schemas/fixtures/skill-release/) |
| Program closeout | [`docs/evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md`](../../../evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md) |
| Golden path evidence (001G) | [`docs/evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001G.md`](../../../evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001G.md) |

## Sister repos

Implementation and per-slice evidence live in `kanon-dotnet`, `allagma-dotnet`, and `ontogony-frontend` (`docs/evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001*.md`).

Final reviewed commits: `ontogony-platform` `c39e951` (001G fixture + evidence), `allagma-dotnet` `b621da9` (golden path integration test).

## Follow-on

**`ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001`** — connect a real sandbox consumer to sandbox-bound skill versions. Do not jump to production deployment yet.
