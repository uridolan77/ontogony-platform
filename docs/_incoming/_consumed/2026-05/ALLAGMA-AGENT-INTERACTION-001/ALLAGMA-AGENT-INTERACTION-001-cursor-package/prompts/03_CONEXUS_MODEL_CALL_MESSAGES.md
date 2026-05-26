# Cursor prompt — Conexus model-call enrichment

Inspect `conexus-dotnet` and the frontend Conexus client for model-call detail availability.

The Agent Interaction workbench needs to render model-call enrichment when linked from Allagma.

Use existing routes first:

```text
GET /admin/v0/model-calls/{modelCallId}
GET /admin/v0/model-calls/{modelCallId}/evidence-links
GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
GET /admin/v0/route-decisions/{routeDecisionId}
```

## Required display data

- modelCallId, preserving `chatcmpl-*` where appropriate
- requestId, separately from modelCallId
- executionRunId, separately from requestId
- model purpose
- model alias / Conexus alias if available
- selected provider
- provider model
- provider mode: fake, real, unknown
- fallback chain and fallback-used status
- token usage
- cost
- latency
- provider attempts
- redacted messages, or explicit message availability status

## If messages are unavailable

Do not invent them. Return/display:

```text
Messages: not recorded
```

or:

```text
Messages: redacted
```

## Tests

Add tests for ID normalization and display:

```text
Model call ID: chatcmpl-...
Request ID: 0HN...
Execution run ID: chat-0HN...
```

Do not collapse these IDs into one field.
