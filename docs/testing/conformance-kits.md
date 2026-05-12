# Ontogony.Testing — Conformance Kits

Conformance kits let consumer services prove they adopted platform mechanics correctly.
Each kit is a static class of assertion helpers in `Ontogony.Testing`; they throw
`InvalidOperationException` on failure so they work with any test framework.

---

## Quick reference

| Class | What it proves |
|---|---|
| `TracingConformanceAssertions` | `RequestTracingMiddleware` is wired up and echoes canonical trace headers |
| `ErrorShapeConformanceAssertions` | `OntogonyExceptionHandlingMiddleware` maps exceptions, shapes error JSON, and does not leak internals |
| `EnvelopeConformanceAssertions` | `OntogonyEnvelope` source/type conventions and payload hash integrity |
| `HmacConformanceAssertions` | `ServiceIdentityHmacSignatureHelper` canonical string format and signature round-trips |
| `OutboxConformanceHarness` | `IOutboxWriter`/`IOutboxReader`/`IOutboxDispatcher` contract: write, read, dispatch, idempotency |
| `HttpResilienceConformanceHarness` | `ResilientIntegrationDelegatingHandler` retry and circuit-breaker mechanics |

---

## TracingConformanceAssertions

```csharp
// Verify incoming X-Ontogony-Trace-Id is echoed back in the response
await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("trace-abc123");

// Verify a new trace id is generated when none is supplied
await TracingConformanceAssertions.AssertTraceIdGeneratedWhenAbsentAsync();

// Verify tenant id propagates from header into OntogonyCorrelationContext
await TracingConformanceAssertions.AssertTenantIdPropagatedAsync("tenant-acme");
```

All methods spin up `RequestTracingMiddleware` internally — no real HTTP server needed.

---

## ErrorShapeConformanceAssertions

```csharp
// Unmapped exception → 500 with standard JSON shape
await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();

// Internal exception text must not appear in the response body
await ErrorShapeConformanceAssertions.AssertNoUnmappedExceptionMessageLeakAsync();

// Mapped exception → custom status + error code
await ErrorShapeConformanceAssertions.AssertMappedExceptionProducesExpectedShapeAsync(
    new MyDomainException("oops"),
    HttpStatusCode.UnprocessableEntity,
    "MY_DOMAIN_ERROR");

// Inspect a JSON string directly
ErrorShapeConformanceAssertions.AssertErrorJsonShape(
    json,
    expectedCode: "VALIDATION_FAILED",
    requireTraceId: true);
```

---

## EnvelopeConformanceAssertions

```csharp
// Full conformance check in one call
EnvelopeConformanceAssertions.AssertFullConformance(envelope);

// Individual assertions
EnvelopeConformanceAssertions.AssertSourceFollowsScheme(envelope);   // ontogony://<svc>/<domain>
EnvelopeConformanceAssertions.AssertEventTypeIsNamespaced(envelope); // contains a dot
EnvelopeConformanceAssertions.AssertSchemaVersionPresent(envelope);
EnvelopeConformanceAssertions.AssertPayloadHashMatchesContent(envelope); // SHA-256 round-trip
```

The `AssertFullConformance` overload also runs `EnvelopeAssertions.AssertHasRequiredFields`.

---

## HmacConformanceAssertions

```csharp
// Verify sign → verify round-trip
HmacConformanceAssertions.AssertSignatureRoundTrip(
    secret: "signing-secret",
    httpMethod: "POST",
    pathAndQuery: "/api/events",
    timestamp: "2026-05-12T00:00:00Z",
    nonce: "unique-nonce",
    bodySha256HexLower: "<hex-hash>");

// Verify canonical string has five newline-separated components
HmacConformanceAssertions.AssertCanonicalStringFormat(
    "POST", "/api/events", "2026-05-12T00:00:00Z", "nonce", "<hex-hash>");

// Verify body hash is lowercase 64-char hex
HmacConformanceAssertions.AssertBodyHashFormat(requestBodyBytes);

// Verify tampered signature is not accepted
HmacConformanceAssertions.AssertTamperedSignatureIsRejected(
    "secret", "GET", "/path", "2026-05-12T00:00:00Z", "nonce", "<hex-hash>");
```

---

## OutboxConformanceHarness

```csharp
var store = new InMemoryOutboxStore();
var msg = OutboxConformanceHarness.BuildMessage("msg-001");

// Write then read
await OutboxConformanceHarness.AssertWriteThenReadAsync(store, store, msg);

// Mark dispatched, check it disappears from the queue
await OutboxConformanceHarness.AssertMarkDispatchedRemovesFromQueueAsync(store, store, store, msg);

// Duplicate write must throw
await OutboxConformanceHarness.AssertDuplicateWriteThrowsAsync(store, msg);

// MarkFailed must be idempotent
await OutboxConformanceHarness.AssertMarkFailedIsIdempotentAsync(store, store, msg);
```

`BuildMessage` produces a minimal valid `OutboxMessage` ready for use with any implementation of `IOutboxWriter` / `IOutboxReader` / `IOutboxDispatcher` (for example `InMemoryOutboxStore` or `PostgresOutboxStore`).
Override fields with `with` expressions as needed.

---

## HttpResilienceConformanceHarness

```csharp
var stub = new StubHttpMessageHandler();

// Build a resilient HttpClient backed by the stub
var client = HttpResilienceConformanceHarness.BuildResilientClient(
    stub,
    opts => { opts.MaxRetries = 2; });

// Happy path: no retries
await HttpResilienceConformanceHarness.AssertNoRetryOnSuccessAsync(client, stub);

// Retry path: 2 retries = 3 total calls
await HttpResilienceConformanceHarness.AssertRetriesOnTransientFailureAsync(client, stub, expectedTotalAttempts: 3);

// Circuit breaker: opens after N failures, returns 503 without hitting the stub
await HttpResilienceConformanceHarness.AssertCircuitOpensAfterThresholdAsync(stub, failuresToOpen: 3);
```

---

## Writing service-level conformance tests

A typical smoke test class in a consumer service looks like:

```csharp
public sealed class PlatformConformanceSmokeTests
{
    [Fact]
    public async Task Tracing_CanonicalHeaderIsEchoed() =>
        await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("smoke-trace");

    [Fact]
    public async Task Errors_UnmappedExceptionsAreContained() =>
        await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();

    [Fact]
    public void Envelope_PublishedEnvelopesPassConformance()
    {
        var envelope = MyEventFactory.BuildSampleEnvelope();
        EnvelopeConformanceAssertions.AssertFullConformance(envelope);
    }
}
```

These tests live in the consumer repo and import only `Ontogony.Testing`.
No Athanor, Agentor, or Conexus packages are required.
