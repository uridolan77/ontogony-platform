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

## Does not guarantee

- Full OpenTelemetry exporter configuration or sampling policies (hosting concern).
- Authentication of incoming correlation headers.

## Related

- [../03_TRACE_CORRELATION_STANDARD.md](../03_TRACE_CORRELATION_STANDARD.md)
- [../adoption/observability-error-ordering.md](../adoption/observability-error-ordering.md)
