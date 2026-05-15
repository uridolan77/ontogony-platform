# Ontogony.Http — semantic contract

**Status:** Production-safe for resilient outbound HTTP registration and correlation propagation.

## Guarantees

- `OntogonyIntegrationHeaders` canonical names for cross-service HTTP calls.
- Named and typed `AddOntogonyIntegrationHttpClient` registration with correlation, actor, and idempotency propagation.
- **Mechanical** error surfaces (`IntegrationHttpError`) for adapters without domain exception types.
- Correlation propagation includes W3C trace context headers (`traceparent`, `tracestate`) and Ontogony correlation headers when present.
- Optional `AddOntogonyOutboundActorPropagation()` in `Ontogony.Security` bridges `ICurrentActorAccessor` to outbound calls.

## Metrics interoperability

`Ontogony.Http` focuses on transport mechanics and propagation.

When services want outbound integration metrics in the shared Ontogony metric namespace, prefer `IIntegrationOperationMeter` in `Ontogony.Observability`. Legacy static helpers on `OntogonyMetrics` remain available for existing HTTP handlers.

- `ontogony.integration.call.count`
- `ontogony.integration.error.count`
- `ontogony.integration.duration.ms`

See [../observability/metrics-catalog.md](../observability/metrics-catalog.md) for dimensions and [../adoption/integration-metrics-adoption.md](../adoption/integration-metrics-adoption.md) for usage.

## Does not guarantee

- Which HTTP calls are safe to retry (idempotency is a product concern).
- Service discovery, URL templates, or auth token acquisition.
- Automatic exporter configuration (hosting concern).

## Related

- [../invariants.md](../invariants.md) — retry scope.
- [../adoption/http-client-adoption.md](../adoption/http-client-adoption.md)
- [../observability/operations-pack.md](../observability/operations-pack.md)
