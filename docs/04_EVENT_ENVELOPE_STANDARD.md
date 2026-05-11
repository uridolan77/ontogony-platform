# 04 — Event Envelope Standard

## Canonical shape

All protocol-specific payloads should be wrapped in `OntogonyEnvelope<TPayload>`.

```json
{
  "eventId": "evt_...",
  "eventType": "mcp.tool.invoked",
  "source": "agentor://runtime",
  "occurredAt": "2026-05-11T09:00:00Z",
  "traceId": "trace_...",
  "spanId": "span_...",
  "parentSpanId": "span_parent",
  "tenantId": "tenant_...",
  "workspaceId": "workspace_...",
  "projectId": "project_...",
  "actorId": "actor_...",
  "sessionId": "session_...",
  "protocol": "mcp",
  "schemaVersion": "1.0",
  "payloadHash": "sha256...",
  "payload": {}
}
```

## Event naming convention

Use:

```text
protocol.entity.verb
```

Examples:

```text
agui.message.created
agui.state.updated
agui.user.approved
mcp.tool.invoked
mcp.tool.completed
a2a.task.created
a2a.artifact.generated
llm.request.created
llm.response.completed
policy.evaluated
athanor.decision.detected
```

## Payload rule

Keep raw protocol payloads stored separately if needed. The envelope should contain normalized metadata plus enough payload to drive first-class queries.

## Hashing rule

When computing `payloadHash`, use canonical JSON:

- compact JSON
- sorted object keys recursively
- UTF-8 bytes
- SHA-256 lowercase hex

This prevents harmless key ordering differences from producing different fingerprints.
