# Contract Requirements

Every backend route exposed to `ontogony-frontend` must have:

1. OpenAPI path and schema.
2. Auth behavior documented.
3. Error envelope shape.
4. Redaction policy.
5. Correlation metadata fields.
6. Integration/unit tests for success, unauthorized, validation failure, not found/empty, and degraded dependency where applicable.
7. CI artifact or contract evidence.

## Shared response metadata

```json
{
  "traceId": "trace-...",
  "correlationId": "corr-...",
  "service": "conexus|kanon|allagma",
  "serviceVersion": "0.x",
  "contractVersion": "v0",
  "generatedAt": "ISO-8601"
}
```

## Cross-service identifier envelope

```json
{
  "runId": "run_...",
  "decisionId": "decision_...",
  "modelCallId": "model_call_...",
  "requestId": "req_...",
  "humanGateId": "gate_...",
  "replayBundleId": "replay_..."
}
```

## Error envelope

```json
{
  "code": "string",
  "message": "operator-safe string",
  "traceId": "string",
  "correlationId": "string",
  "details": {},
  "retryable": false
}
```

Never put secrets, raw prompts, provider headers, bearer tokens, API keys, or raw tool inputs in operator errors.
