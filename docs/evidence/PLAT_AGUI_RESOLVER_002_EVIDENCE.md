# PLAT / ADAPTER-AGUI-002 — AG-UI evidence resolver contract

**Date:** 2026-05-22  
**Verdict:** **PASS (local)** — contract + schema + validation gates; frontend implementation in `ontogony-frontend`.

## Scope

Platform contract for resolving a single AG-UI interaction event into operator evidence links and (when configured) an Evidence Spine subgraph. No backend aggregation API.

## Deliverables

| Path | Role |
| --- | --- |
| [`docs/operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md`](../operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md) | Anchor selection, modes, boundaries, non-goals |
| [`docs/schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json`](../schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json) | Resolver result JSON Schema |
| [`docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md) | Cross-link to resolver contract |
| [`scripts/validate-agent-interaction-spine.ps1`](../../scripts/validate-agent-interaction-spine.ps1) | Validates new artifacts exist |
| `tests/Ontogony.Infrastructure.Tests/SystemAgentInteractionSpineContractTests.cs` | Artifact existence gate |

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1
dotnet test tests\Ontogony.Infrastructure.Tests --filter FullyQualifiedName~SystemAgentInteractionSpineContractTests
```

## Non-claims

- No new Kanon or Conexus AG-UI HTTP export routes.
- No server-side graph aggregator.
- CI/merge closure remains separate from this local evidence slice.
