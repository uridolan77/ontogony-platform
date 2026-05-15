# Ontogony.Observability — semantic contract

**Status:** Production-safe for ASP.NET request tracing middleware, correlation context, and OpenTelemetry metric instruments.

## Guarantees

- Propagation of trace and correlation headers and **mechanical** span attributes.
- Stable metric instrument names (`ontogony.*`) within a major version unless documented otherwise.
- Stable activity source and meter identity: `Ontogony.Platform`.

## Metrics and Traces

- Metrics catalog: [../observability/metrics-catalog.md](../observability/metrics-catalog.md)
- Trace attributes catalog: [../observability/trace-attributes.md](../observability/trace-attributes.md)
- Operations guide and local collector setup: [../observability/operations-pack.md](../observability/operations-pack.md)
- Integration metrics adoption: [../adoption/integration-metrics-adoption.md](../adoption/integration-metrics-adoption.md)

## Integration metrics helper

Use `IIntegrationOperationMeter` for consistent outbound service-call metrics:

- `StartCall` — opens a client-side diagnostic `Activity` for tracing (does not emit metrics)
- `RecordSuccess` / `RecordFailure` — emit `ontogony.integration.*` counters and duration histograms

Stable dimension keys live on `IntegrationMetricDimensions`:

```text
source_service
target_service
operation
status
error_code
http_status
```

Register through `AddOntogonyObservability()` (registers `IIntegrationOperationMeter` as a singleton).

Legacy `OntogonyMetrics.RecordIntegrationCall`, `RecordIntegrationDuration`, and `RecordIntegrationError` remain available and delegate to the same recorder with the updated dimension model.

### Cardinality guidance

Do not attach high-cardinality values as metric or activity dimensions, for example:

```text
user_id
request_id
decision_id
trace_id
prompt_id
artifact_id
raw URL paths with identifiers
```

Use trace/activity tags for request-scoped identifiers only when necessary, or prefer logs for high-cardinality identifiers. Reserved dimension keys cannot be overridden by caller-supplied dimensions.

## Does not guarantee

- Full OpenTelemetry exporter configuration or sampling policies (hosting concern).
- Authentication of incoming correlation headers.

## Related

- [../03_TRACE_CORRELATION_STANDARD.md](../03_TRACE_CORRELATION_STANDARD.md)
- [../adoption/observability-error-ordering.md](../adoption/observability-error-ordering.md)
