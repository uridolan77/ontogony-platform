# Ontogony.Testing

Test doubles, harnesses, and conformance assertion kits for validating Ontogony platform integration.

## What this is

- **Fakes** — clocks, IDs, publishers, HTTP stubs, correlation scopes.
- **Conformance kits** — assertions for tracing, errors, envelopes, HMAC, outbox, HTTP resilience.
- **Architecture helpers** — forbidden MSBuild reference and `using` directive scanners.

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

### Architecture Helpers
- `ArchitectureReferenceAssertions` — scan `PackageReference`, `ProjectReference`, and `PackageVersion` entries (including multiline `Include`), plus `using` / `using static` / alias directives
- `ArchitectureScanTargets` — collect `*.csproj`, `Directory.Packages.props`, and resolve `src/**/*.cs` globs

See [../docs/adoption/architecture-tests-adoption.md](../docs/adoption/architecture-tests-adoption.md).

### Conformance Kits
- `TracingConformanceAssertions` — Trace header echo and propagation
- `HeaderPropagationConformanceAssertions` — Frozen PLATFORM-9-003 outbound header propagation proofs
- `PropagationHeaderScenario` — Expected values for propagation conformance tests
- `ErrorShapeConformanceAssertions` — Exception handling and error JSON shape
- `EnvelopeConformanceAssertions` — Event envelope conventions and payload hash
- `HmacConformanceAssertions` — HMAC signature format and round-trips
- `OutboxConformanceHarness` — Outbox write/read/dispatch mechanics
- `IdempotencyLedgerConformanceHarness` — `IIdempotencyLedger` begin/succeed/fail semantics
- `ArtifactStoreConformanceHarness` — `IArtifactStore` hash, dedupe, and reference semantics
- `HttpResilienceConformanceHarness` — HTTP retry, Retry-After, total-timeout budget, and circuit-breaker behavior

## Design

- **Framework-agnostic.** Conformance assertions throw `InvalidOperationException` and work with any test framework.
- **No domain coupling.** All helpers work against abstract contracts; no Athanor/Agentor/Conexus imports.
- **Deterministic.** Fake clocks and ID generators ensure reproducible test behavior.
- **Inspection-friendly.** Test doubles expose recorded state for assertions.

## License

MIT — see LICENSE in the repository root.
