# AG-UI Compatibility Strategy

## Position

Ontogony should maintain its own internal event contract and expose AG-UI compatibility through a pure adapter.

## Why not adopt AG-UI directly as the internal contract?

- Ontogony needs domain events that AG-UI does not natively own: evidence graph snapshots, Kanon decision/provenance links, Conexus route decisions, Allagma audit bundles.
- .NET AG-UI SDK support should not block backend work.
- Ontogony needs long-term schema drift gates across multiple repos.
- Internal contract can preserve stronger service ownership boundaries.

## Adapter direction

```text
OntogonyAgentEvent[] -> AgUiEvent[]
AgUiRunInput -> Ontogony run/resume request
```

## Mapping table

| Ontogony | AG-UI |
| --- | --- |
| `RUN_STARTED` | `RunStarted` |
| `RUN_FINISHED` | `RunFinished` |
| `RUN_ERROR` | `RunError` |
| `STEP_STARTED` | `StepStarted` |
| `STEP_FINISHED` | `StepFinished` |
| `MESSAGE_STARTED` | `TextMessageStart` |
| `MESSAGE_DELTA` | `TextMessageContent` |
| `MESSAGE_FINISHED` | `TextMessageEnd` |
| `TOOL_CALL_STARTED` | `ToolCallStart` |
| `TOOL_CALL_ARGS_DELTA` | `ToolCallArgs` |
| `TOOL_CALL_READY` | `ToolCallEnd` |
| `STATE_SNAPSHOT` | `StateSnapshot` |
| `STATE_DELTA` | `StateDelta` |
| `INTERRUPT_CREATED` | interrupt outcome or custom event, depending on target client |
| `EVIDENCE_*` | `Custom` with `name: ontogony.evidence.*` |
| `DECISION_*` | `Custom` with `name: ontogony.kanon.*` |
| `MODEL_CALL_*` | `Custom` with `name: ontogony.conexus.*` |
| `UI_INTENT_*` | `Custom` / generative UI extension |

## Compatibility tests

- sample run fixture converts to AG-UI lifecycle/message/tool/state events.
- sample interrupt fixture converts to interrupt-aware run termination.
- custom Ontogony events survive round-trip as custom events.
- unknown AG-UI custom events are preserved, not dropped.
