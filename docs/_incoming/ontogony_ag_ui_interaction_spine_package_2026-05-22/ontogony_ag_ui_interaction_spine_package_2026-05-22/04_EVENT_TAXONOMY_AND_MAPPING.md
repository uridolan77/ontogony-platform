# 04 — Event Taxonomy and Service Mapping

## Service ownership matrix

| Event family | Producer | Consumer | Notes |
| --- | --- | --- | --- |
| `RUN_*` | Allagma | Frontend, UI, Platform evidence | Can be synthesized from existing run endpoints first |
| `STEP_*` | Allagma | Frontend, UI | Maps to operations or run event phases |
| `MESSAGE_*` | Allagma / Frontend | Frontend, UI | Store redacted content only by default |
| `TOOL_CALL_*` | Allagma / Frontend / Conexus | Frontend, UI, Kanon | A model call is a special tool-call subtype |
| `STATE_*` | Frontend / Allagma | UI, adapter | State snapshots/deltas must be deterministic |
| `INTERRUPT_*` | Kanon / Allagma | Frontend, UI | Represents human gate or approval pause |
| `RESUME_*` | Frontend / user | Allagma / Kanon | POST to Allagma resume after validation |
| `EVIDENCE_*` | Frontend / Platform / services | UI | Derived from Evidence Spine graph and source attempts |
| `DECISION_*` | Kanon | Frontend, UI | Semantic/provenance authority |
| `MODEL_CALL_*` | Conexus | Frontend, UI | Route, provider, cost, attempts |
| `UI_INTENT_*` | Agent runtime / Frontend | UI | Must be registry-validated |

## Mapping from existing APIs

### Allagma

| Existing source | Interaction events |
| --- | --- |
| `GET /allagma/v0/runs/{runId}` | `RUN_STARTED`, `RUN_FINISHED`, `RUN_ERROR`, `STATE_SNAPSHOT` |
| `GET /allagma/v0/runs/{runId}/events` | `STEP_*`, `MESSAGE_*`, `TOOL_CALL_*`, `HUMAN_GATE_*`, `EVIDENCE_*` |
| `GET /allagma/v0/runs/{runId}/audit` | `EVIDENCE_GRAPH_SNAPSHOT`, `DECISION_PROVENANCE_LINKED`, `MODEL_CALL_*` summaries |
| `GET /allagma/v0/runs/{runId}/operations` | `STEP_STARTED`, `STEP_PROGRESS`, `STEP_FINISHED` |
| `POST /allagma/v0/runs/{runId}/resume` | `RESUME_SUBMITTED`, `INTERRUPT_RESOLVED`, later run continuation |
| `POST /allagma/v0/runs/{runId}/cancel` | `RUN_CANCELLED` |
| `POST /allagma/v0/runs/{runId}/retry` | `RUN_RETRIED` |
| `POST /allagma/v0/runs/{runId}/replay` | `RUN_REPLAYED` |

### Kanon

| Existing source | Interaction events |
| --- | --- |
| `GET /ontology/v0/decision-records/{decisionId}` | `DECISION_RECORDED` |
| `GET /ontology/v0/decision-records/{decisionId}/provenance` | `DECISION_PROVENANCE_LINKED` |
| `GET /ontology/v0/semantic-graph?humanGateId=...` | `HUMAN_GATE_OPENED`, `HUMAN_GATE_CHECKED`, `HUMAN_GATE_RESOLVED` |
| `POST /ontology/v0/actions/human-gates/check` | `INTERRUPT_CREATED` or `HUMAN_GATE_CHECKED` |
| `POST /ontology/v0/actions/human-gates/{humanGateId}/resolve` | `INTERRUPT_RESOLVED`, `HUMAN_GATE_RESOLVED` |
| `GET /ontology/v0/decision-records/by-trace/{traceId}` | `DECISION_RECORDED` plus evidence link |

### Conexus

| Existing source | Interaction events |
| --- | --- |
| `POST /v1/chat/completions` | `MODEL_CALL_STARTED`, `MODEL_CALL_COMPLETED` |
| `GET /conexus/v0/model-calls/{modelCallId}` | `MODEL_CALL_ROUTE_DECIDED`, `USAGE_COST_RECORDED` |
| `GET /admin/v0/model-calls/{modelCallId}` | `MODEL_CALL_COMPLETED`, `MODEL_CALL_PROVIDER_ATTEMPTED`, `EVIDENCE_LINK_ADDED` |
| `GET /admin/v0/model-calls/{modelCallId}/evidence-links` | `EVIDENCE_LINK_ADDED` |
| `GET /admin/v0/model-calls/{modelCallId}/evidence-bundle` | `EVIDENCE_GRAPH_SNAPSHOT` |
| `GET /admin/v0/route-decisions/{routeDecisionId}` | `MODEL_CALL_ROUTE_DECIDED` |

### Frontend / Evidence Spine

| Existing source | Interaction events |
| --- | --- |
| `resolveEvidenceSpine` | `EVIDENCE_GRAPH_SNAPSHOT`, `EVIDENCE_SOURCE_ATTEMPTED`, `EVIDENCE_NODE_ADDED`, `EVIDENCE_EDGE_ADDED` |
| `resolveTraceCorrelation` | `EVIDENCE_LINK_ADDED`, `STATE_DELTA` |
| `extractHumanGateIdFromEvents` | `HUMAN_GATE_OPENED` or `INTERRUPT_CREATED` candidate |

## AG-UI adapter mapping

| Ontogony event | AG-UI-style event |
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
| `TOOL_CALL_RESULT` | `ToolCallResult` / tool message |
| `STATE_SNAPSHOT` | `StateSnapshot` |
| `STATE_DELTA` | `StateDelta` |
| `INTERRUPT_CREATED` | `RunFinished` outcome interrupt + metadata |
| `UI_INTENT_*` | Custom event / generative UI event depending on adapter target |
| `EVIDENCE_*`, `DECISION_*`, `MODEL_CALL_*` | Custom events with stable namespacing |

## Namespacing

Custom AG-UI events should use `ontogony.*` names:

```text
ontogony.evidence.graph_snapshot
ontogony.evidence.source_attempted
ontogony.kanon.decision_recorded
ontogony.kanon.human_gate_opened
ontogony.conexus.model_call_completed
ontogony.conexus.route_decided
ontogony.allagma.audit_bundle_linked
ontogony.ui.intent_proposed
```
