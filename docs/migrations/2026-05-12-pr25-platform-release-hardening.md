# PR25 — Platform release hardening (deterministic mechanics, packaging, observability)

## Summary

Ontogony platform-only hardening: packaging version gate, configurable inbound trace header resolution, injectable time for security/persistence/CloudEvents fallbacks, optional HMAC body-hash preload requirement, richer messaging contract surface, HTTP resilience polish, and documentation alignment.

## Consumer actions

### Packaging / CI

- `scripts/pack-all.ps1` **no longer defaults** a package version. Set `PACKAGE_VERSION` (for example `0.2.0`) before packing.
- CI in this repo sets `PACKAGE_VERSION` when invoking the pack script; consumer pipelines should do the same.

### Observability

- `RequestTracingMiddleware` now passes `OntogonyObservabilityOptions.TraceHeaderName` and `AcceptedIncomingTraceHeaders` into `OntogonyCorrelationContext.FromHeaders`. Custom accepted aliases work as configured; the canonical trace header is always checked first.

### Security (HMAC)

- New `ServiceIdentityOptions.RequirePreloadedBodyHashForHmacBodies` (default `false`). When `true`, HMAC verification for non-empty bodies requires `ServiceIdentityBodyHashPreloadMiddleware` to have stored a precomputed hash on `HttpContext.Items`. Intended for production hosts that always register preload early in the pipeline.

### Persistence

- `InMemoryOutboxStore` accepts optional `Ontogony.Primitives.IClock` for dead-letter timestamps. DI registration resolves `IClock` when registered (for example via `AddOntogonyPersistencePrimitives`).

### Contracts

- `CloudEventConversionOptions.Clock` supplies time when CloudEvent `time` is missing (defaults to `SystemClock` behavior when unset).

### Messaging

- New `IEventPublisherWithResult` for publishers that expose structured dispatch diagnostics. `InMemoryEventPublisher` implements it; `IEventPublisher` stays minimal.

### HTTP

- `TransportResilienceOptions`: `RespectRetryAfterHeader`, `BackoffJitterFraction`; retries clone `HttpRequestMessage.Options`; delays honor `Retry-After` when enabled.

## Repos unchanged

No Athanor or Agentor changes in this PR.
