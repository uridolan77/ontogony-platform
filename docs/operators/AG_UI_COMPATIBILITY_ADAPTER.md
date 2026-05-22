# AG-UI compatibility adapter

**Sprint:** ADAPTER-AGUI-001  
**Package:** `@ontogony/agent-interaction` (`packages/ontogony-agent-interaction`)  
**Baseline:** `SYSTEM-ALPHA-006` / `ontogony-agent-interaction-v0`

## Position

Ontogony maintains its **internal** interaction event contract as canonical. AG-UI-compatible streams are produced by a **pure adapter** in `@ontogony/agent-interaction`, not by adopting AG-UI as the internal schema.

```text
OntogonyAgentEvent[]  →  AgUiEvent[]     (export / external clients)
AgUiEvent (CUSTOM)    →  OntogonyAgentEvent   (round-trip / ingress sketches)
```

## Why not AG-UI as internal contract?

- Domain events AG-UI does not own: evidence graph, Kanon decisions, Conexus route decisions, Allagma audit bundles.
- .NET AG-UI SDK maturity must not block backend projection work.
- Cross-repo schema drift gates require an Ontogony-owned contract family.
- Service ownership boundaries stay explicit in [`agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json).

## Mapping table

| Ontogony | AG-UI |
| --- | --- |
| `RUN_STARTED` | `RUN_STARTED` |
| `RUN_FINISHED` | `RUN_FINISHED` (with `outcome` including interrupt variant) |
| `RUN_ERROR` | `RUN_ERROR` |
| `RUN_CANCELLED` | `RUN_CANCELLED` |
| `STEP_STARTED` | `STEP_STARTED` |
| `STEP_FINISHED` | `STEP_FINISHED` |
| `MESSAGE_STARTED` | `TEXT_MESSAGE_START` |
| `MESSAGE_DELTA` | `TEXT_MESSAGE_CONTENT` |
| `MESSAGE_FINISHED` | `TEXT_MESSAGE_END` |
| `TOOL_CALL_STARTED` | `TOOL_CALL_START` |
| `TOOL_CALL_ARGS_DELTA` | `TOOL_CALL_ARGS` |
| `TOOL_CALL_READY` | `TOOL_CALL_END` |
| `TOOL_CALL_RESULT` | `TOOL_CALL_RESULT` |
| `STATE_SNAPSHOT` | `STATE_SNAPSHOT` |
| `STATE_DELTA` | `STATE_DELTA` |
| `INTERRUPT_CREATED` | `CUSTOM` `name: ontogony.interrupt_created` + interrupt `RUN_FINISHED` outcome |
| `EVIDENCE_*`, `DECISION_*`, `MODEL_CALL_*`, `UI_INTENT_*`, … | `CUSTOM` `name: ontogony.<type_lower>` |

Unknown external `CUSTOM` events are preserved as Ontogony `CUSTOM` with `agUiName` / `agUiValue` — not dropped.

## Consumers

| Repo | Usage |
| --- | --- |
| `ontogony-frontend` | Agent Interaction Workbench **Export AG-UI JSONL** |
| Future | CopilotKit / SSE bridges after `ALLAGMA-AGUI-002` |

`Ontogony.ProtocolIngress.AgUiProtocolAdapter` remains the **ingress** normalizer for a separate AG-UI DTO shape; do not conflate with this export adapter.

## Validation

```bash
cd packages/ontogony-agent-interaction
npm install
npm run test
```

Platform spine script (structural):

```powershell
.\scripts\validate-agent-interaction-spine.ps1
```

## Related

- [`AGENT_INTERACTION_SPINE_CONTRACT.md`](./AGENT_INTERACTION_SPINE_CONTRACT.md)
- [`docs/evidence/PLAT_AGUI_ADAPTER_001_EVIDENCE.md`](../evidence/PLAT_AGUI_ADAPTER_001_EVIDENCE.md)
