# 10 — Conexus Trace and Model-Call Events Plan

**Repo:** `conexus-dotnet`

## Goal

Project Conexus model-call and route-decision evidence into the Agent Interaction Spine.

## Current foundation

Conexus already defines a deterministic model-call evidence flow with:

- completion response `id` as `modelCallId`
- project-scoped model-call route evidence
- admin model-call detail
- evidence links
- evidence bundle
- route-decision detail
- usage/cost aggregates

## Event mapping

| Conexus source | Interaction events |
| --- | --- |
| chat completion request accepted | `MODEL_CALL_STARTED` |
| route selected | `MODEL_CALL_ROUTE_DECIDED` |
| provider attempt started/ended | `MODEL_CALL_PROVIDER_ATTEMPTED` |
| completion succeeds | `MODEL_CALL_COMPLETED` |
| completion fails | `MODEL_CALL_FAILED` |
| usage/cost persisted | `USAGE_COST_RECORDED` |
| evidence links available | `EVIDENCE_LINK_ADDED` |
| evidence bundle available | `EVIDENCE_GRAPH_SNAPSHOT` |

## Payload sketch

```json
{
  "modelCallId": "chatcmpl-demo-001",
  "routeDecisionId": "route-demo-001",
  "traceId": "trace-demo-001",
  "correlationId": "corr-demo-001",
  "requestedModelAlias": "gpt-4o-mini",
  "selectedProvider": "fake",
  "selectedModel": "gpt-4o-mini",
  "promptTokens": 120,
  "completionTokens": 80,
  "estimatedCostUsd": 0.0003,
  "evidenceCompleteness": "complete",
  "missingReasonCodes": []
}
```

## Redaction

Default event payloads should not include:

- raw prompts
- raw completions
- provider credentials
- project API keys
- internal secrets

Use summary fields, counts, route metadata, redaction flags, and evidence links.

## Tests

- fake provider call projects schema-valid events
- project evidence and admin detail produce consistent IDs
- route decision links to model call
- provider attempts appear in order
- missing evidence links produce warning events
- redaction flags are preserved

## Frontend integration

`conexusModelCallInteractionAdapter.ts` should:

1. Read model-call detail.
2. Read route decision detail when route id exists.
3. Read evidence links/bundle where available.
4. Emit model-call, route, usage, and evidence events.
5. Use trace/correlation resolver to discover Allagma/Kanon links.

## Non-goals

- Do not add mutation routes.
- Do not expose raw prompt/completion data in interaction events.
- Do not make Conexus own semantic decisions or workflow execution state.
