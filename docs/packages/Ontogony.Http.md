# Ontogony.Http — semantic contract

**Status:** Production-safe for resilient outbound HTTP registration and correlation propagation.

## Guarantees

- Typed registration of named `HttpClient` instances with optional **delegating handlers** for correlation headers and transport policies.
- **Mechanical** error surfaces (`IntegrationHttpError`) for adapters without domain exception types.
- Correlation propagation includes W3C trace context headers (`traceparent`, `tracestate`) and Ontogony correlation headers when present.

## Metrics interoperability

`Ontogony.Http` focuses on transport mechanics and propagation.

When services want outbound integration metrics in the shared Ontogony metric namespace, record them through `OntogonyMetrics` in `Ontogony.Observability`:

- `ontogony.integration.call.count`
- `ontogony.integration.error.count`
- `ontogony.integration.duration.ms`

See [../observability/metrics-catalog.md](../observability/metrics-catalog.md) for dimensions and query examples.

## Does not guarantee

- Which HTTP calls are safe to retry (idempotency is a product concern).
- Service discovery, URL templates, or auth token acquisition.
- Automatic exporter configuration (hosting concern).

## Related

- [../invariants.md](../invariants.md) — retry scope.
- [../adoption/http-client-adoption.md](../adoption/http-client-adoption.md)
- [../observability/operations-pack.md](../observability/operations-pack.md)
