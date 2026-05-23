# Cursor prompt — Allagma live replay/API support

Inspect `allagma-dotnet` and determine whether existing APIs can support the Agent Interaction live workbench.

Use existing routes first:

```text
GET /allagma/v0/runs/{runId}
GET /allagma/v0/runs/{runId}/events
GET /allagma/v0/runs/{runId}/audit
GET /allagma/v0/runs/{runId}/evaluations
GET /allagma/v0/evaluations/{evaluationRunId}/evidence
```

If these routes already expose enough data, do not add backend code.

If essential interaction data is missing, add the smallest additive route or fields, preferably:

```text
GET /allagma/v0/runs/{runId}/interaction
```

The interaction route should include:

- run summary
- ordered events
- state snapshots/deltas if available
- messages or redaction/not-recorded markers
- tool intents
- human gate states
- linked identifiers
- evidence links

## Required fields to expose if currently missing

- planningDecisionId
- actionDecisionIds or explicit absence
- conexusModelCallId
- conexusRequestId
- traceId
- correlationId
- ontologyVersionId
- purpose/model purpose
- provider mode if known

## Tests

Add backend tests only for new routes/fields. Preserve existing routes.

## Safety

Do not enable real external tools. Do not expose secrets or raw prompts beyond existing policy.
