# OpenTelemetry distributed trace export gate

## Current state

Aisthesis has metrics meters, but not a full distributed trace export pipeline.

## Desired bridge

```text
traceparent / correlation ID
→ Aisthesis trace ID
→ evidence timeline
→ evidence graph
→ reconstructability report
→ trace bundle
```

## RC-readiness minimum

Document:

```yaml
traceContextFields:
correlationIdPolicy:
traceparentPropagation:
exportTarget:
otelCollector:
sampling:
redaction:
productionBlocker:
```

## Implementation options

1. OTel activity source inside ingestion/query/export.
2. Export reconstructability evaluations as spans/events.
3. Link evidence IDs to span attributes.
4. Export bundle fingerprint as span attribute.
5. Never export raw payloads by default.
