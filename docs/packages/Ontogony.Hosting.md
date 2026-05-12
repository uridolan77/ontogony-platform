# Ontogony.Hosting — semantic contract

**Status:** Production-safe for ASP.NET host composition defaults (registration + middleware + health endpoint mapping).

## Guarantees

- Provides a mechanical composition layer for `Ontogony.Observability`, `Ontogony.Errors`, and optional HMAC body-hash preload middleware.
- Keeps middleware ordering stable when using `UseOntogonyServiceDefaults()`:
  - request tracing before exception handling, so trace IDs flow into error payloads.
- Maps health endpoints through explicit opt-in mechanics (`MapOntogonyHealthEndpoints`) with configurable paths.
- Registers JSON defaults in a host-overridable way.

## Does not guarantee

- Any product/domain startup behavior for Athanor, Agentor, or Conexus.
- Authentication scheme choices, authorization policy defaults, or endpoint-level business semantics.
- Service-specific readiness checks (database, broker, external providers). Those remain in service repos.

## Related

- [../03_TRACE_CORRELATION_STANDARD.md](../03_TRACE_CORRELATION_STANDARD.md)
- [../adoption/observability-error-ordering.md](../adoption/observability-error-ordering.md)
