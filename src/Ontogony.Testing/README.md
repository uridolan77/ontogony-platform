# Ontogony.Testing

Test doubles, harnesses, and conformance assertion kits for validating Ontogony platform integration.

## What this is

- **Fakes** — clocks, IDs, publishers, HTTP stubs, correlation scopes.
- **Conformance kits** — assertions for tracing, errors, envelopes, HMAC, outbox, HTTP resilience.

## What this is not

- Not a test runner or assertion library replacement (bring xUnit/NUnit/etc.).
- Not product demo scenarios or domain fixtures.

## Overview

`Ontogony.Testing` provides:

- **Test doubles** — Fake clocks, ID generators, current actor accessors, event publishers, HTTP handlers
- **Middleware harnesses** — Thin wrappers for testing ASP.NET Core middleware in isolation
- **Conformance kits** — Assertion libraries that verify services correctly implement platform mechanics

## Usage

### Test Doubles

```csharp
var clock = new FakeClock(DateTimeOffset.UtcNow);
clock.Advance(TimeSpan.FromMinutes(5));

var publisher = new FakeEventPublisher();
await publisher.PublishAsync(envelope);
Assert.Contains(envelope, publisher.Published);
```

### Conformance Assertions

```csharp
// Verify trace header echo
await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("trace-123");

// Verify error shape
await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();

// Verify envelope conventions
EnvelopeConformanceAssertions.AssertFullConformance(envelope);

// Verify HMAC signing
HmacConformanceAssertions.AssertSignatureRoundTrip(secret, method, path, timestamp, nonce, bodySha256);
```

## Key Types

### Test Doubles
- `FakeClock` — Deterministic time advancement
- `FakeIdGenerator` — Sequential IDs for testing
- `FakeEventPublisher` — Captures published events
- `StubHttpMessageHandler` — Scripted HTTP responses
- `RecordingHttpMessageHandler` — Records outgoing HTTP requests
- `TestCorrelationScope` — Pushes temporary correlation context

### Conformance Kits
- `TracingConformanceAssertions` — Trace header echo and propagation
- `ErrorShapeConformanceAssertions` — Exception handling and error JSON shape
- `EnvelopeConformanceAssertions` — Event envelope conventions and payload hash
- `HmacConformanceAssertions` — HMAC signature format and round-trips
- `OutboxConformanceHarness` — Outbox write/read/dispatch mechanics
- `HttpResilienceConformanceHarness` — HTTP retry and circuit-breaker behavior

## Design

- **Framework-agnostic.** Conformance assertions throw `InvalidOperationException` and work with any test framework.
- **No domain coupling.** All helpers work against abstract contracts; no Athanor/Agentor/Conexus imports.
- **Deterministic.** Fake clocks and ID generators ensure reproducible test behavior.
- **Inspection-friendly.** Test doubles expose recorded state for assertions.

## License

MIT — see LICENSE in the repository root.
