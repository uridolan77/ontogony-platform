# AG-UI Backend Operator Surface Matrix

## Purpose

Map the backend surfaces required for AG-UI / Agent Interaction integration.

| Capability | Owner repo | Backend surface | RC requirement |
| --- | --- | --- | --- |
| Canonical event schema | `ontogony-platform` | `ontogony-agent-interaction-event-v0` schema + validator | Must validate Allagma and Conexus fixtures. |
| Run interaction export | `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/interaction-events` | Must return deterministic JSONL. |
| Live interaction stream | `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/interaction-events/stream` | Must stream SSE and support resume. |
| Run audit bundle | `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/audit` | Must link events to evidence/audit objects. |
| Run event list | `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/events` | Must provide source material for projection. |
| Human gate state | `allagma-dotnet` + `kanon-dotnet` | run events + Kanon human gate decisions | Must map to interrupt/resume/deny. |
| Semantic decisions | `kanon-dotnet` | decision/provenance/replay routes | Must be referenced by `kanonDecisionId`. |
| Review queue | `kanon-dotnet` | review queue / assistance accept/reject routes | Must map to review-request/review-completed events where present. |
| Model-call lifecycle | `conexus-dotnet` | model-call interaction projector | Must emit requested/routed/completed/failed/stream phases. |
| Model-call evidence | `conexus-dotnet` | model-call detail / evidence links / evidence bundle | Must be linkable from AG-UI events. |
| Usage/cost evidence | `conexus-dotnet` | admin usage-cost drilldown | Must link from model-call events where available. |
| Cross-service trace | all repos via Platform | trace/correlation headers and evidence spine | Must allow trace/correlation lookup from event stream. |

## Minimum release scenarios

| Scenario | Must prove |
| --- | --- |
| Completed non-streaming run | JSONL export and SSE stream have equivalent lifecycle. |
| Streaming run | Stream lifecycle phases are metadata-only and redacted. |
| Human gate interrupted run | Interrupt event is renderable and linked to Kanon gate decision. |
| Human gate denied run | Denial/terminal event is explicit. |
| Conexus fallback | Route/fallback evidence appears as linked model-call metadata. |
| Missing evidence | Missing route/model/decision is structured, not silent. |
| Unauthorized evidence lookup | Event stream does not leak protected evidence to unauthorized caller. |

## Do not conflate

- AG-UI backend stream is not the same as raw Conexus model streaming.
- AG-UI event export is not raw audit export.
- Evidence Spine is the object graph; AG-UI is the interaction narrative over that graph.
- Frontend components can consume this contract, but this package certifies backend surfaces only.
