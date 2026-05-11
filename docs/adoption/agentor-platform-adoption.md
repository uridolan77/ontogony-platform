# Agentor — Ontogony platform adoption (mechanics)

This document is the **mechanical** adoption map for Agentor against Ontogony.Platform: tracing, outbound HTTP integration, and optional shared exception handling. Run/plan/tool semantics stay in the Agentor repository.

## Packages (in order)

| Concern | Package | Agentor use |
|--------|---------|-------------|
| Request tracing, correlation context, metrics | `Ontogony.Observability` | Replace `RequestTracingMiddleware` with `UseOntogonyRequestTracing`. |
| Named integration clients + correlation on outbound calls | `Ontogony.Http` | Replace ad hoc `HttpClient` wiring with `AddOntogonyIntegrationHttpClient`. |
| Stable API error shape + trace id in ProblemDetails | `Ontogony.Errors` (optional) | Replace local exception middleware; keep mappings in Agentor. |

Supporting references: `Ontogony.Contracts` (header names), `Ontogony.Primitives` (clock and small types used by HTTP resilience).

## Middleware order

When using both observability and errors in the same ASP.NET Core app:

1. `UseOntogonyRequestTracing()`
2. `UseOntogonyExceptionHandling()`
3. Authentication / authorization
4. Endpoints

Details: [`observability-error-ordering.md`](./observability-error-ordering.md).

## Trace headers (compatibility)

- **Canonical:** `X-Ontogony-Trace-Id` (`OntogonyEventHeaders.TraceId`), plus W3C `traceparent` / `tracestate` when present.
- **Legacy:** `X-Agentor-Trace-Id` (`OntogonyEventHeaders.LegacyAgentorTraceId`) is still read when building correlation state from headers (`OntogonyCorrelationContext.FromHeaders`), so Agentor can accept older callers during rollout.

Prefer emitting canonical headers on new integrations; keep reading legacy headers until all producers are migrated.

## Step-by-step guides

- Athanor phased mechanics (primitives first): [`athanor-platform-adoption.md`](./athanor-platform-adoption.md).
- Observability + Http wiring for Agentor: [`agentor-observability-adoption.md`](./agentor-observability-adoption.md).
- Conexus headers and events (including polyglot): [`conexus-platform-adoption.md`](./conexus-platform-adoption.md).
- Http client pattern (any service): [`http-client-adoption.md`](./http-client-adoption.md).
- Errors middleware and keeping mappings local: [`error-middleware-adoption.md`](./error-middleware-adoption.md).

## Donor reference (not production)

The `_donors/agentor/` tree in this repo is historical reference only. Production Agentor changes belong in the Agentor repository; adopt the APIs above from NuGet (or project reference) there.

## Verification (minimal)

1. Inbound request without headers gets a trace id; response exposes canonical trace header per your observability options.
2. Inbound `X-Agentor-Trace-Id` still correlates when `X-Ontogony-Trace-Id` is absent.
3. Outbound named client sends correlation headers aligned with current `CorrelationHeadersDelegatingHandler` behavior.
4. If `Ontogony.Errors` is enabled, thrown failures include the same trace id as logs (see ordering guide).
