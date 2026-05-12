# Ontogony.Logging — semantic contract

**Status:** Proposed platform module.

## Guarantees

- Stable mechanical field names for structured logs.
- Stable `EventId` identifiers within a major version unless documented.
- Correlation-aware logging scopes derived from `OntogonyCorrelationContext`.
- ASP.NET middleware for request-scoped enrichment.

## Does not guarantee

- Any specific logging provider, sink, exporter, storage engine, or viewer.
- Safe logging of raw request/response bodies.
- Audit-log durability.
- Product-specific meaning.

## Conexus.NET use

Use for gateway request lifecycle logs, provider-call lifecycle logs, quota rejections, idempotency duplicate events, and safe references to artifacts/secrets.
