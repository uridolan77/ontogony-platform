# PLAT-AGUI-000 — Agent interaction spine contract evidence

**Date:** 2026-05-22  
**Baseline:** `SYSTEM-ALPHA-006`  
**Sprint:** PLAT-AGUI-000 (platform contract + schemas + drift gates only)

## Scope delivered

- [`docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md) — contract index; cross-links Evidence Spine; redaction and hidden-reasoning rules.
- [`docs/system/agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json) — event family ownership and existing source routes.
- [`docs/schemas/ontogony-agent-interaction-event-v0.schema.json`](../schemas/ontogony-agent-interaction-event-v0.schema.json)
- [`docs/schemas/ontogony-agent-interaction-session-v0.schema.json`](../schemas/ontogony-agent-interaction-session-v0.schema.json)
- [`docs/schemas/fixtures/agent-interaction/`](../schemas/fixtures/agent-interaction/) — `sample-run.jsonl`, `sample-human-gate-interrupt.jsonl`
- [`scripts/validate-agent-interaction-spine.ps1`](../../scripts/validate-agent-interaction-spine.ps1) — structural + JSONL smoke validation.
- [`scripts/validate-agent-interaction-spine.sh`](../../scripts/validate-agent-interaction-spine.sh) — POSIX wrapper.
- Platform tests: `SystemAgentInteractionSpineContractTests`, `AgentInteractionEventSchemaTests`.
- [`system-protocol-registry.json`](../system/system-protocol-registry.json) — `agent-interaction-spine-contract` protocol entry.

## Non-claims

- No Allagma/Kanon/Conexus runtime or HTTP changes.
- No `ontogony-frontend` workbench or `@ontogony/ui` `./agent` export (downstream PRs).
- SSE stream shipped in `ALLAGMA-AGUI-002` + frontend `OFE-AGUI-004` (see [`AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md`](./AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md)); no WebSocket.
- External AG-UI export adapter shipped in `@ontogony/agent-interaction` (`ADAPTER-AGUI-001` — see [`PLAT_AGUI_ADAPTER_001_EVIDENCE.md`](./PLAT_AGUI_ADAPTER_001_EVIDENCE.md)).

## Fixture deferrals

Package testing doc names additional JSONL categories (`sample-model-call-route`, `sample-missing-evidence`, `sample-replay`). Only two baseline fixtures ship in PLAT-AGUI-000; add more when `OFE-AGUI-001` needs them.

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~AgentInteraction"
```

Optional combined gate:

```powershell
.\scripts\validate-agent-interaction-spine.ps1 -RunSchemaTests
```

## Related intake

- [`ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md`](../reviews/ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md)
