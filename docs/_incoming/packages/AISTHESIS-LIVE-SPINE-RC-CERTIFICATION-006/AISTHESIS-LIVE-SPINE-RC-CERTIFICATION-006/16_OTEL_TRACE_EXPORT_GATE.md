# OpenTelemetry trace export gate

## Goal

Bridge runtime traces and evidence traces:

```text
traceparent -> Aisthesis traceId -> evidence timeline -> graph -> reconstructability -> bundle fingerprint
```

## First implementation slice

Add ActivitySource spans for:

```text
Aisthesis.Evidence.IngestEnvelope
Aisthesis.Evidence.IngestBatch
Aisthesis.Evidence.CreateEdge
Aisthesis.Evidence.Lookup
Aisthesis.Reconstructability.Compute
Aisthesis.Bundle.Export
Aisthesis.Evaluation.Run
```

## Span attributes

No raw payloads.

Allowed attributes:

```text
service.name = aisthesis
aisthesis.trace_id
aisthesis.correlation_id
aisthesis.evidence_id
aisthesis.edge_id
aisthesis.producer_system
aisthesis.evidence_type
aisthesis.required_edges.missing
aisthesis.reconstructability.grade
aisthesis.bundle_fingerprint
```

## Export config

Document OTLP endpoint, sampler, redaction, local disabled default, and ReleaseMode behavior.

## Closeout classification

```yaml
implemented: true|false
metricsOnly: true|false
traceExportReady: true|false
productionBlocker: true|false
```
