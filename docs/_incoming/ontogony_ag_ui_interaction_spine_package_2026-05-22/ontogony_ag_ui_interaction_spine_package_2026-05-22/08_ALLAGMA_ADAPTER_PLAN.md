# 08 — Allagma Adapter Plan

**Repo:** `allagma-dotnet`

## Goal

Make Allagma the first canonical backend producer of Ontogony interaction events because it owns the runtime run lifecycle.

## Current foundation

Allagma already exposes run start/get/events/audit/operations/resume/retry/cancel/replay endpoints in its feature connection matrix. That is enough to begin with a projection layer before adding a live stream.

## Phase 1 — Projection only

Add an application service:

```csharp
public interface IAgentInteractionEventProjector
{
    IReadOnlyList<OntogonyAgentEventDto> ProjectRun(AgentRun run, IReadOnlyList<AgentRunEvent> events);
    IReadOnlyList<OntogonyAgentEventDto> ProjectOperations(AgentRun run, IReadOnlyList<RunOperation> operations);
    IReadOnlyList<OntogonyAgentEventDto> ProjectAudit(AgentRun run, AgentRunAuditBundle audit);
}
```

Output must validate against `ontogony-agent-interaction-event-v0.schema.json`.

## Phase 2 — Export route or command

Add either:

```http
GET /allagma/v0/runs/{runId}/interaction-events
```

or a dev/operator export command. The response may be JSON array or JSONL. Prefer JSONL for replay fixtures.

## Phase 3 — Live stream

Add SSE only after UI/reducer/schema are stable:

```http
GET /allagma/v0/runs/{runId}/interaction-events/stream
Accept: text/event-stream
```

Recommended event payload:

```text
event: ontogony.agent-event
id: <eventId>
data: { ...OntogonyAgentEventDto }
```

## Projection mapping

| Allagma fact | Event |
| --- | --- |
| run created/started | `RUN_STARTED` |
| run status completed | `RUN_FINISHED` |
| run failed | `RUN_ERROR` |
| run cancelled | `RUN_CANCELLED` |
| run retry requested | `RUN_RETRIED` |
| replay requested | `RUN_REPLAYED` |
| operation starts | `STEP_STARTED` |
| operation updates | `STEP_PROGRESS` |
| operation completed | `STEP_FINISHED` |
| run event references Kanon decision | `DECISION_RECORDED` / `EVIDENCE_LINK_ADDED` |
| run event references modelCallId | `MODEL_CALL_STARTED` or `MODEL_CALL_COMPLETED` summary |
| run event references humanGateId | `INTERRUPT_CREATED` / `HUMAN_GATE_OPENED` |
| audit bundle loaded | `EVIDENCE_GRAPH_SNAPSHOT` |

## Resume semantics

When Allagma receives resume:

1. Validate resume payload shape at API boundary.
2. If human gate is involved, call Kanon check/resolve according to existing conventions.
3. Emit or project `RESUME_SUBMITTED`.
4. Emit `INTERRUPT_RESOLVED` when accepted.
5. Continue run or emit `RESUME_REJECTED` / `RUN_ERROR` on validation failure.

## Tests

- started/completed run projection
- failed run projection
- run with model-call id projection
- run with Kanon decision id projection
- run with human-gate id projection
- audit bundle evidence event projection
- JSONL fixture validates against schema

## Non-goals

- Do not replace existing run endpoints.
- Do not stream raw model token chunks unless explicitly enabled by a separate output persistence policy.
- Do not make Allagma the owner of Kanon decisions or Conexus model-call details.
