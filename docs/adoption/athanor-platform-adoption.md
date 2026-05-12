# Athanor — Ontogony platform adoption (mechanics)

This document is the **mechanical** adoption map for Athanor: low-risk shared primitives first, then HTTP-facing tracing and errors when you touch the API surface. Canonization, contradiction handling, and review semantics stay in the Athanor repository.

## Phase 1 — Minimal adoption (no API behavior change)

Adopt in this order and **avoid** messaging/outbox until contracts are stable in consuming services:

| Package | Athanor use |
|---------|-------------|
| `Ontogony.Primitives` | Shared clock and small mechanical types where donor duplicates existed. |
| `Ontogony.Hashing` | Canonical JSON and content hashing aligned with platform fingerprint policy. |
| `Ontogony.Contracts` | Envelope/header vocabulary shared with recorders and other services. |

Goal: byte-identical or policy-identical hashes where you claim parity; no new distributed behavior.

Per-package guarantees: [`docs/packages/`](../packages/).

## CI and sibling project references

If Athanor uses `ProjectReference` to this repository, document the checkout layout or use versioned packages from an internal feed. See [local-repo-layout-and-ci.md](./local-repo-layout-and-ci.md) and [private-nuget-feed.md](./private-nuget-feed.md).

## Phase 2 — API and integration (when you change the ASP.NET host)

| Concern | Package |
|---------|---------|
| Replace `TraceIdMiddleware` | `Ontogony.Observability` — `UseOntogonyRequestTracing()` |
| Shared ProblemDetails + trace id | `Ontogony.Errors` — keep exception **mappings** in Athanor |
| Named outbound clients + correlation | `Ontogony.Http` — `AddOntogonyIntegrationHttpClient` |

Middleware order when using both observability and errors: [`observability-error-ordering.md`](./observability-error-ordering.md).

## Trace headers (compatibility)

- **Canonical:** `X-Ontogony-Trace-Id` (`OntogonyEventHeaders.TraceId`), plus W3C `traceparent` / `tracestate` when present.
- **Legacy (inbound):** `X-Athanor-Trace-Id` (`OntogonyEventHeaders.LegacyAthanorTraceId`) is still read when resolving correlation from headers (`OntogonyCorrelationContext.FromHeaders`), so existing clients keep correlating during rollout.
- **Legacy (response echo):** Off by default (`EchoLegacyHeaders = false`). Set `EchoLegacyHeaders = true` in `AddOntogonyObservability` if a service must still emit legacy trace aliases on responses until callers migrate.

Prefer emitting canonical headers on new integrations.

## Step-by-step guides

- Tracing + errors wiring (detailed sample): [`athanor-observability-adoption.md`](./athanor-observability-adoption.md).
- Http client pattern: [`http-client-adoption.md`](./http-client-adoption.md).
- Errors middleware and local mappings: [`error-middleware-adoption.md`](./error-middleware-adoption.md).

## Donor reference (not production)

The `_donors/athanor/` tree in this repo is historical reference only (for example `TraceIdMiddleware`, hashing helpers). Production Athanor changes belong in the Athanor repository.

## Verification (minimal)

**Phase 1**

1. Golden hash or fingerprint tests still pass where you assert parity with platform helpers.
2. No accidental dependency on messaging/outbox packages for core read paths.

**Phase 2**

1. Tracing runs before exception middleware when both are enabled.
2. Trace id matches across logs, response headers, and ProblemDetails extensions for thrown failures.
3. Outbound named clients propagate correlation headers per [`http-client-adoption.md`](./http-client-adoption.md).
