# Contract and Error Testing

## Why this matters

Ontogony has multiple typed clients and orchestration clients. A route can compile locally and still break the system if its shape changes silently.

Contract tests must check:

- route existence;
- method and path;
- required headers;
- request schema;
- response schema;
- error schema;
- status code semantics;
- idempotency behavior;
- trace/correlation headers.

## Standard error envelope target

```json
{
  "code": "allagma.kanon.plan_unavailable",
  "message": "Unable to compile semantic plan.",
  "stage": "kanon_planning",
  "system": "allagma",
  "downstreamSystem": "kanon",
  "traceId": "...",
  "correlationId": "...",
  "retryable": true,
  "detail": {}
}
```

## Required negative tests per protected route

For every protected route:

1. Missing auth header.
2. Invalid token/key.
3. Expired or revoked token/key where supported.
4. Wrong role/scope.
5. Valid auth but malformed payload.
6. Valid auth but forbidden domain action.
7. Duplicate idempotency key with same payload.
8. Duplicate idempotency key with different payload.
9. Unsupported content type.
10. Oversized payload.

## Error-code naming convention

```text
<system>.<domain>.<condition>
```

Examples:

```text
allagma.run.not_found
allagma.run.idempotency_conflict
allagma.kanon.plan_unavailable
kanon.policy.action_denied
kanon.human_gate.waiting
conexus.provider.unavailable
conexus.idempotency.streaming_not_supported
metabole.transformation.invalid_input
aisthesis.memory.trace_not_found
platform.auth.unauthorized
```

## Contract drift policy

A breaking API change is allowed only if:

- manifest route matrix is updated;
- typed clients are updated;
- frontend callers are updated;
- old behavior is intentionally deprecated or removed;
- tests include old-vs-new migration note.
