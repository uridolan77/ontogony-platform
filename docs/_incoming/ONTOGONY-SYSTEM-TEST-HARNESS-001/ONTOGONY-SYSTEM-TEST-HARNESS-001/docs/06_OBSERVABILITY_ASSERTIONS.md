# Observability Assertions

The harness should test observability as a first-class feature, not as a nice-to-have.

## Required correlation chain

```text
Client -> Allagma -> Kanon -> Conexus
```

Required headers/fields:

```text
traceparent
X-Correlation-ID
X-Ontogony-Actor-Id
X-Ontogony-Actor-Type
X-Ontogony-Actor-Roles
Idempotency-Key
X-Allagma-Run-Id
```

## Assertions

For a completed governed run:

- Allagma response includes or exposes run ID.
- Allagma events include correlation ID.
- Kanon decision/provenance includes correlation ID or trace ID.
- Conexus telemetry can be linked to Allagma run ID or correlation ID without storing sensitive payload.
- Error responses include enough diagnostics to debug the failure.

## Evidence bundle format

```json
{
  "testRunId": "...",
  "scenarioId": "E2E-001",
  "startedAtUtc": "...",
  "completedAtUtc": "...",
  "status": "passed",
  "correlationId": "...",
  "requests": [],
  "responses": [],
  "events": [],
  "decisions": [],
  "telemetryHints": [],
  "assertions": []
}
```

## Metrics to assert eventually

- service readiness gauge;
- request count by route/status;
- latency histogram by route;
- Conexus provider fallback count;
- Allagma run state count;
- Kanon policy decision count;
- idempotency replay/conflict count;
- error count by normalized error code.
