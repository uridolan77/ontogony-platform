# Ontogony.Http — semantic contract

**Status:** Production-safe for resilient outbound HTTP registration and correlation propagation.

## Guarantees

- Typed registration of named `HttpClient` instances with optional **delegating handlers** for correlation headers and transport policies.
- **Mechanical** error surfaces (`IntegrationHttpError`) for adapters without domain exception types.

## Does not guarantee

- Which HTTP calls are safe to retry (idempotency is a product concern).
- Service discovery, URL templates, or auth token acquisition.

## Related

- [../invariants.md](../invariants.md) — retry scope.
- [../adoption/http-client-adoption.md](../adoption/http-client-adoption.md)
