# Standard structured log fields

## Request scope (required)

Every inbound HTTP request handled by Ontogony middleware should emit logs within a scope containing:

| Field | Type | Source |
| --- | --- | --- |
| `traceId` | string | `CorrelationState.TraceId` |
| `operationId` | string | `CorrelationState.OperationId` |
| `service` | string | `OntogonyObservabilityOptions.ServiceName` |
| `method` | string | HTTP method |
| `path` | string | Request path (no query secrets) |

Build scopes with `SystemCorrelationConventions.ToLogScope(state, serviceName)` and merge route-specific fields.

## Optional tenancy / actor fields

| Field | Header |
| --- | --- |
| `actorId` | `X-Ontogony-Actor-Id` |
| `tenantId` | `X-Ontogony-Tenant-Id` |
| `workspaceId` | `X-Ontogony-Workspace-Id` |
| `projectId` | `X-Ontogony-Project-Id` |
| `sessionId` | `X-Ontogony-Session-Id` |

## Forbidden in log fields

```text
raw prompts or completions
API keys, connection strings, bearer tokens
unredacted PII unless explicitly approved for a secure sink
full request/response bodies for LLM calls
```

Use hashes and ids (`modelCallId`, `decisionEventId`, `runId`) for cross-linking.

## Cross-link to operator UI

Log `traceId` values must match:

- Evidence Spine (`/system/evidence-spine?traceId=…`)
- Trace reconstructability (`/system/evidence-spine/:traceId/reconstructability`)
- Kanon `GET /ontology/v0/reconstructability/by-trace/{traceId}`
