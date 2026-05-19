# Graph node examples

## Allagma run node

```json
{
  "id": "allagma.run:run_123",
  "kind": "allagma.run",
  "service": "allagma",
  "label": "Run run_123",
  "status": "completed",
  "identifiers": {
    "runId": "run_123",
    "traceId": "trace_abc",
    "modelCallId": "chatcmpl_123",
    "planningDecisionId": "decision_123"
  }
}
```

## Conexus model-call node

```json
{
  "id": "conexus.modelCall:chatcmpl_123",
  "kind": "conexus.modelCall",
  "service": "conexus",
  "label": "Model call chatcmpl_123",
  "identifiers": {
    "modelCallId": "chatcmpl_123",
    "traceId": "trace_abc",
    "routeDecisionId": "route_123"
  }
}
```
