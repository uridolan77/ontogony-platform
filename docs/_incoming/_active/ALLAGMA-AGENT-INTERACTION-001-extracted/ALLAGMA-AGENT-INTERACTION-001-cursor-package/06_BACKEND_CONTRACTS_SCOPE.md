# Backend contracts scope

## Principle

Implement frontend-first if existing live APIs expose enough data. Add backend changes only where the live interaction timeline cannot be truthfully reconstructed.

## Existing likely Allagma APIs

Use if available:

```text
GET /allagma/v0/runs
GET /allagma/v0/runs/{runId}
GET /allagma/v0/runs/{runId}/events
GET /allagma/v0/runs/{runId}/audit
GET /allagma/v0/runs/{runId}/evaluations
GET /allagma/v0/evaluations/{evaluationRunId}/evidence
POST /allagma/v0/runs
POST /allagma/v0/runs/{runId}/resume
```

## Add only if missing

If the frontend cannot determine live interaction linkage from existing routes, add one narrow endpoint rather than many page-specific hacks:

```text
GET /allagma/v0/runs/{runId}/interaction
```

Suggested response:

```json
{
  "schemaVersion": "allagma-agent-interaction-v1",
  "run": {},
  "events": [],
  "messages": [],
  "stateSnapshots": [],
  "toolIntents": [],
  "humanGates": [],
  "linkedIdentifiers": {
    "traceId": null,
    "correlationId": null,
    "planningDecisionId": null,
    "actionDecisionIds": [],
    "conexusModelCallId": null,
    "conexusRequestId": null,
    "evaluationRunIds": [],
    "baselineComparisonIds": []
  },
  "evidenceLinks": [],
  "redactions": []
}
```

## Conexus enrichment

Use existing admin routes if available:

```text
GET /admin/v0/model-calls/{modelCallId}
GET /admin/v0/model-calls/{modelCallId}/evidence-links
GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
GET /admin/v0/route-decisions/{routeDecisionId}
```

Add fields only if the current model-call detail cannot support:

- provider mode
- selected provider
- selected provider model
- alias/model purpose
- tokens/cost/latency
- messages/redaction state
- provider attempts
- route decision ID

## Kanon enrichment

Use existing routes if available:

```text
GET /ontology/v0/decision-records/{decisionId}
GET /ontology/v0/decision-records/{decisionId}/provenance
GET /ontology/v0/semantic-graph
```

Add fields only if action decision IDs are produced but not persisted/exposed.

## Required backend behavior if changed

- Preserve stable API prefixes.
- Do not break existing clients.
- Add OpenAPI updates where repo practice requires it.
- Add tests for new DTO fields/routes.
- Use structured error envelopes where already available.
- Do not expose secrets/raw prompts by default.
