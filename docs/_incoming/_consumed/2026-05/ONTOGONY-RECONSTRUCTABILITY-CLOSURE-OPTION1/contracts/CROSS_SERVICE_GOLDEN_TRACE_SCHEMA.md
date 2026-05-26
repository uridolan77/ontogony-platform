# Cross-Service Golden Trace Schema

## File

```text
artifacts/reconstructability/golden-trace/<timestamp>/golden-trace.json
```

## Schema

```json
{
  "schema": "ontogony-cross-service-reconstructability-golden-trace-v1",
  "generatedAtUtc": "2026-05-26T00:00:00Z",
  "mode": "fake-provider-local",
  "traceId": "trace-...",
  "correlationId": "corr-...",
  "allagma": {
    "runId": "run-...",
    "decisionEventsEndpoint": "GET /allagma/v0/runs/{runId}/decision-events",
    "decisionEvents": []
  },
  "kanon": {
    "planningDecisionIds": [],
    "humanGateDecisionIds": [],
    "classificationEndpoint": "POST /ontology/v0/reconstructability/classify-batch",
    "classificationResults": []
  },
  "conexus": {
    "modelCallIds": [],
    "routeDecisionIds": [],
    "decisionEvents": []
  },
  "classificationSummary": {
    "totalEvents": 0,
    "criticalEvents": 0,
    "highEvents": 0,
    "failures": 0,
    "warnings": 0,
    "passes": 0,
    "highCriticalFailures": 0
  },
  "evidence": {
    "files": [],
    "commands": [],
    "repoShas": {}
  }
}
```

## Closure rule

```text
classificationSummary.highCriticalFailures == 0
```

If this is not true, the package is not closed.
