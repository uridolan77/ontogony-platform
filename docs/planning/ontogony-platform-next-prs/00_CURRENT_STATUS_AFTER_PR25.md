# Current Status After PR25

## Executive assessment

`ontogony-platform` is no longer a starter repo. It is now a real shared infrastructure alpha with usable mechanical packages for .NET services.

PR25 closes the earlier release-hardening gap:

- `scripts/pack-all.ps1` now requires `PACKAGE_VERSION`; no more implicit stale starter version.
- CI restores, builds, tests, and packs with explicit package version.
- `AcceptedIncomingTraceHeaders` is now actually used by request tracing.
- `IClock` is injected into security skew checks, outbox dead-letter timestamps, and CloudEvents missing-time fallback.
- HMAC hosts can require preload-only body hashing for body-bearing requests.
- `IEventPublisherWithResult` exists for in-process/diagnostic publishers.
- HTTP resilience has Retry-After, jitter, request option cloning, and clock-aware parsing.

## Strategic state

The platform now has a credible core:

```text
Primitives       = time/id abstractions
Configuration    = startup guards and validation
Contracts        = envelope/header/event contracts
Observability    = trace/correlation/meter/activity mechanics
Errors           = exception mapping and JSON error middleware
Http             = correlation + resilience for outbound HTTP
Hashing          = deterministic hashing/canonical JSON mechanics
Idempotency      = idempotency key construction and hash use
Security         = current actor + service identity/HMAC mechanics
Messaging        = in-process/reference event publisher
Persistence      = outbox contracts + in-memory reference
Testing          = test fixtures and assertions
```

## Remaining architectural gaps

The platform is still not complete as a robust infrastructure foundation. Missing or incomplete areas:

1. No `Ontogony.Hosting` service-defaults package.
2. No durable Postgres outbox provider.
3. No protocol ingress normalization layer for AG-UI/MCP/A2A/CloudEvents/OpenTelemetry events.
4. No production-grade distributed nonce replay store interface/provider guidance beyond the base abstraction.
5. No explicit secret rotation/key-id story for HMAC service identity.
6. No OpenTelemetry exporter/sample dashboard docs.
7. No package compatibility/conformance kit for consumers.
8. No release automation for publishing packages beyond packing.
9. No source-link/package metadata maturity pass.
10. Documentation is good but still scattered; needs a developer path, ops path, and architecture path.

## Boundary warning

Do not use the next PRs to move Agentor run semantics or Athanor canonization semantics into the platform.

The next phase should be infrastructure-depth work: hosting, durable transport, protocol normalization, conformance tests, security operationalization, release hygiene, and documentation.
