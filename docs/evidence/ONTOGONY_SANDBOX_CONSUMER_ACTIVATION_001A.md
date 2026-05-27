# Evidence: ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001A (platform)

**Slice:** 001A — Platform sandbox consumer activation contracts, schemas, fixtures  
**Date:** 2026-05-27  
**Repo:** `ontogony-platform`

---

## Summary

001A defines the neutral contract family for sandbox consumers resolving active governed skill bindings and recording applied-version evidence. Allagma binding resolution and consumer implementations are deferred to 001B+.

**Prerequisite:** `ONTOGONY-SKILL-RELEASE-GOVERNANCE-001` (001A–001G closed).

---

## Delivered

| Artifact | Path |
| --- | --- |
| Protocol index | `docs/protocols/SANDBOX_CONSUMER_ACTIVATION.md` |
| Contracts (4) | `docs/contracts/SKILL_CONSUMER_*_V0.md`, `SANDBOX_CONSUMER_ACTIVATION_EVENT_V0.md` |
| JSON schemas (4) | `docs/schemas/sandbox-consumer-activation/*.v0.schema.json` |
| Fixtures (7) | `docs/schemas/fixtures/sandbox-consumer-activation/` |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SandboxConsumerActivationSchemaTests.cs` |

---

## Verification

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SandboxConsumerActivationSchemaTests
```

Expected: all tests pass; resolved fixture uses `sandbox` only; paused/rolled_back/no-binding fixtures have `resolved: false`; `production` request fails schema validation.

---

## Package status

**Superseded by package closeout:** [`ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md`](./ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md) — `ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001` closed (001A–001G).
