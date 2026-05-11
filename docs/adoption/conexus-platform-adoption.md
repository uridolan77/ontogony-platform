# Conexus — Ontogony platform adoption (mechanics)

Conexus is often **polyglot** (for example Python). This guide splits what applies to any runtime from what applies only when a **.NET** host uses the shared ASP.NET packages.

## Shared vocabulary (any runtime)

Use the same correlation and envelope surfaces as the rest of the platform:

- **Trace id:** prefer emitting `X-Ontogony-Trace-Id` on HTTP responses and downstream calls. Incoming correlation parsing in .NET services also recognizes `X-Conexus-Request-Id` (`OntogonyEventHeaders.ConexusRequestId`) and other legacy headers when resolving trace context (`OntogonyCorrelationContext.FromHeaders`).
- **Request identity:** when Conexus issues an outbound request id that should participate in correlation, expose it as `X-Conexus-Request-Id` so gateways and recorders can line up logs and events.
- **Events:** emit protocol-neutral envelopes; field rules and checklists live in [`conexus-event-emission-adoption.md`](./conexus-event-emission-adoption.md). Validate against [`schemas/ontogony-envelope.schema.json`](../../schemas/ontogony-envelope.schema.json) where practical.

Provider routing, pricing, and model policy remain in the Conexus product repository.

## .NET-hosted Conexus (optional)

If a Conexus edge or admin API is built on ASP.NET Core, adopt the same **mechanical** stack as other services:

| Concern | Package |
|--------|---------|
| Request tracing, correlation, metrics | `Ontogony.Observability` |
| Named integration clients + outbound correlation | `Ontogony.Http` |
| Stable ProblemDetails + trace id extension | `Ontogony.Errors` (optional) |

Middleware order when using both observability and errors: [`observability-error-ordering.md`](./observability-error-ordering.md).

Http client registration pattern: [`http-client-adoption.md`](./http-client-adoption.md).

## Polyglot hosts (typical)

- Set canonical and legacy trace headers on outbound HTTP calls to match what your callers already send (`X-Ontogony-Trace-Id`, W3C `traceparent` / `tracestate` when you participate in distributed tracing).
- Carry `traceId` inside emitted envelopes consistently with HTTP (see event emission guide).
- Do not embed provider decision logic in shared schemas or this repo.

## Step-by-step guides

- Event envelope emission from Conexus: [`conexus-event-emission-adoption.md`](./conexus-event-emission-adoption.md).
- Http integration pattern (named clients, resilience): [`http-client-adoption.md`](./http-client-adoption.md).
- Errors middleware (if using .NET): [`error-middleware-adoption.md`](./error-middleware-adoption.md).

## Verification (minimal)

1. HTTP: responses or downstream calls expose trace correlation consistently with headers you document for Conexus.
2. Events: payloads validate against the shared envelope schema; `eventType` and `source` follow team conventions.
3. .NET only: tracing runs before exception middleware; outbound named clients propagate correlation headers.
