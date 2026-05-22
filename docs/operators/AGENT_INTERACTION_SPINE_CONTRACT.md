# Agent interaction spine contract

**Sprint:** PLAT-AGUI-000 — Agent Interaction Spine Baseline  
**Status:** Canonical cross-repo contract index (schemas and drift gates; no runtime streaming in this slice)  
**Baseline:** `SYSTEM-ALPHA-006`  
**Contract id:** `ontogony-agent-interaction-v0`

## Purpose

Provide a **replayable, event-sourced** contract for agent–user co-execution across Allagma (governed runtime), Kanon (semantic authority), Conexus (model gateway), `ontogony-frontend`, and `@ontogony/ui`.

This layer adds a **temporal interaction timeline** (lifecycle, messages, tool/model calls, interrupts, evidence links). It does **not** replace the cross-service Evidence Spine graph contract.

## Relationship to Evidence Spine

| Concern | Evidence Spine | Agent Interaction Spine |
| --- | --- | --- |
| Primary view | Static **graph** of nodes/edges across services | **Append-only event log** for live/replay UI |
| Authority for “what happened” in meaning | Kanon decisions, provenance | Same — events **reference** Kanon/Conexus/Allagma ids |
| Operator workbench (v1) | [`/system/evidence-spine`](../../../ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts) | `/system/agent-interaction` (planned — `OFE-AGUI-001`) |
| Export schema | `ontogony-cross-service-evidence-spine-bundle-v1` | `ontogony-agent-interaction-event-v0` (JSONL streams) |

See [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./SYSTEM_EVIDENCE_SPINE_CONTRACT.md). Evidence Spine **000–009** remains closed; do not reopen resolver implementation under AG-UI work.

## Authority boundaries

| Boundary | Owner | Responsibility |
| --- | --- | --- |
| Run lifecycle | Allagma | Run, operation, resume, retry, cancel, replay |
| Semantic authority | Kanon | Decisions, human gates, provenance, graph anchors |
| Model-call authority | Conexus | Model calls, route decisions, provider attempts, usage/cost |
| Cross-service contract | ontogony-platform | Schemas, event matrix, drift tests |
| UI rendering | @ontogony/ui | Trusted UI primitives and view models only |
| Operator orchestration | ontogony-frontend | Event ingestion, replay, adapters, workbench |

**Allagma** is the governed execution owner. Do not introduce legacy “Agentor” naming in new artifacts.

## Canonical artifacts

| Artifact | Repo | Role |
| --- | --- | --- |
| **This document** | ontogony-platform | Contract index and acceptance rules |
| [`agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json) | Platform | Event family → service owner → existing HTTP/sources |
| [`ontogony-agent-interaction-event-v0.schema.json`](../schemas/ontogony-agent-interaction-event-v0.schema.json) | Platform | Single interaction event JSON Schema |
| [`ontogony-agent-interaction-session-v0.schema.json`](../schemas/ontogony-agent-interaction-session-v0.schema.json) | Platform | Derived session snapshot (reducer output shape) |
| [`docs/schemas/fixtures/agent-interaction/*.jsonl`](../schemas/fixtures/agent-interaction/) | Platform | Deterministic replay fixtures |
| [`@ontogony/agent-interaction`](../../packages/ontogony-agent-interaction/) | Platform | TypeScript types + AG-UI export adapter ([`AG_UI_COMPATIBILITY_ADAPTER.md`](./AG_UI_COMPATIBILITY_ADAPTER.md)) |
| [`allagma-feature-connection.matrix.json`](../../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json) | Allagma | Run/events/audit/operations source routes |
| [`KANON_EVIDENCE_SPINE_HANDOFF.md`](../../../kanon-dotnet/docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md) | Kanon | Decision / human-gate routes |
| [`MODEL_CALL_EVIDENCE_FLOW.md`](../../../conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md) | Conexus | Model-call evidence sequence |

## Core identifiers

Every event should carry as many of these as are known:

```ts
interface OntogonyInteractionIds {
  threadId?: string;
  runId?: string;
  parentRunId?: string;
  stepId?: string;
  operationId?: string;
  traceId?: string;
  correlationId?: string;
  modelCallId?: string;
  routeDecisionId?: string;
  kanonDecisionId?: string;
  planningDecisionId?: string;
  humanGateId?: string;
  evidenceNodeId?: string;
  evidenceEdgeId?: string;
}
```

## Base event shape

```ts
interface OntogonyAgentEventBase {
  schema: "ontogony-agent-interaction-event-v0";
  eventId: string;
  type: OntogonyAgentEventType;
  timestamp: string; // ISO-8601 date-time
  source:
    | "allagma"
    | "kanon"
    | "conexus"
    | "ontogony-frontend"
    | "ontogony-ui"
    | "ontogony-platform"
    | "adapter"
    | "fixture";
  severity?: "debug" | "info" | "warning" | "error";
  ids: OntogonyInteractionIds;
  redaction?: {
    containsRedactedFields?: boolean;
    redactionReasonCodes?: string[];
  };
  evidence?: OntogonyEvidenceLink[];
  payload: unknown;
}
```

Event types and families are enumerated in [`agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json) and the event JSON Schema.

## Event stream semantics

### Replayable append-only log

An interaction session is an append-only event stream. Derived UI state must be reconstructible from events and **deterministic** reducers.

### Terminal lifecycle

A run terminates with one of:

- `RUN_FINISHED` with success outcome
- `RUN_ERROR`
- `RUN_CANCELLED`
- `RUN_FINISHED` with interrupt outcome, paired with one or more `INTERRUPT_CREATED` events

### Interrupt / resume

A pending interrupt blocks new normal user input for the same `threadId` unless the input includes a `resume` payload addressing all open interrupts. Resume flows use existing Allagma `POST /allagma/v0/runs/{runId}/resume` and Kanon human-gate resolve routes.

### Evidence rule

Material state transitions should carry at least one `evidence` link or document why the link is unavailable (`missingReasonCode` on the link object).

### Redaction and non-goals (required)

Events **must not** include by default:

- Raw secrets or credentials
- Hidden chain-of-thought or private reasoning traces
- Unredacted model prompts or completions

Allowed in v0: summarized payloads, redaction flags, decision **effects**, provenance references, route/provider metadata, token counts, and cost summaries aligned with Conexus evidence policy.

“Reasoning” in AG-UI terms maps to **sanitized milestones** (decision recorded, evidence attempt, step progress) — not private model internals.

### Generative UI

`UI_INTENT_*` events require a **registry-validated** intent schema in `@ontogony/ui`. Arbitrary runtime component execution is out of scope for v0.

## Transport (v0)

1. **Fixture JSONL** — deterministic tests and design (`docs/schemas/fixtures/agent-interaction/`).
2. **Client-synthesized** — project interaction events from existing Allagma/Kanon/Conexus HTTP responses and `resolveEvidenceSpine` (`OFE-AGUI-002`).
3. **SSE / WebSocket** — deferred (`ALLAGMA-AGUI-002`) until reducers and UI primitives stabilize.

Prefer **SSE + POST** resume/cancel/retry over WebSocket for the first backend stream.

## Service mapping (summary)

| Service | Primary event families | Existing sources |
| --- | --- | --- |
| Allagma | `RUN_*`, `STEP_*`, `MESSAGE_*`, `TOOL_CALL_*` | `/allagma/v0/runs/{runId}`, `/events`, `/operations`, `/audit`, resume/retry/cancel/replay |
| Kanon | `DECISION_*`, `HUMAN_GATE_*`, `INTERRUPT_*` | Decision records, provenance, human-gate check/resolve, semantic graph |
| Conexus | `MODEL_CALL_*`, `USAGE_COST_*` | `/v1/chat/completions`, project + **admin** model-call evidence routes |
| Frontend | `EVIDENCE_*`, synthesized cross-service | `resolveEvidenceSpine`, `resolveTraceCorrelation` |
| UI | `UI_INTENT_*` (optional baseline) | `@ontogony/ui` `./agent` slice (planned) |

Full matrix: [`agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json).

## Acceptance (PLAT-AGUI-000)

- [x] Contract index published (this document).
- [x] Event and session JSON Schemas published.
- [x] Event-family matrix published with Allagma/Kanon/Conexus/frontend/UI owners.
- [x] JSONL fixtures validate against event schema.
- [x] Redaction / hidden-reasoning rules explicit.
- [x] Drift script and platform tests gate artifacts.
- [ ] `@ontogony/ui` `./agent` export (`OUI-AGENT-001`) — downstream.
- [ ] Frontend replay workbench (`OFE-AGUI-001`) — downstream.
- [ ] Backend projection / SSE — deferred.

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~AgentInteraction"
```

## Related

- [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./SYSTEM_EVIDENCE_SPINE_CONTRACT.md)
- [`TRACE_CORRELATION_CONTRACT.md`](./TRACE_CORRELATION_CONTRACT.md)
- [`ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md`](../reviews/ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md)
- [`PLAT_AGUI_000_EVIDENCE.md`](../evidence/PLAT_AGUI_000_EVIDENCE.md)
- [`docs/migrations/2026-05-22-agent-interaction-spine-v0.md`](../migrations/2026-05-22-agent-interaction-spine-v0.md)
- Intake archive: `docs/_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/`
