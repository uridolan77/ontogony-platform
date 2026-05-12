# Architecture & Design Decisions

This guide documents key architectural decisions and design patterns in Ontogony.

---

## Core Principle

```text
Share mechanics, not meaning.
```

Ontogony is **infrastructure for cross-service mechanics only**.

✅ **In scope:**
- Trace propagation (mechanics of distributed tracing)
- Event envelopes (mechanics of canonical events)
- HTTP retry (mechanics of resilience)
- Error shapes (mechanics of safe error responses)
- HMAC signing (mechanics of service authentication)

❌ **Out of scope:**
- Domain semantics (what canonization means to Athanor)
- Orchestration rules (how Agentor plans tool invocations)
- Product logic (why iGaming refunds matter to Conexus)

---

## Design Decisions

### 1. Async-Local Correlation Context (not HTTP Context)

**Decision:** Use `AsyncLocal<CorrelationState>` instead of `HttpContext.Items`.

**Why:**

- Works for background jobs, service buses, and non-HTTP contexts
- Each async flow has isolated context (no thread-pool leaks)
- Survives across thread pool threads
- Clear ownership: Ontogony owns async-local, services own HTTP context

**Trade-off:**

- Requires explicit middleware to populate
- Not automatically cleared on request end (could leave residual state)
  - **Mitigation:** Use `TestCorrelationScope` in tests

**Alternative considered:**

```csharp
// ❌ Not chosen: tight to HTTP
var traceId = HttpContext.Items["TraceId"];

// ✅ Chosen: works anywhere
var traceId = OntogonyCorrelationContext.TraceId;
```

---

### 2. Exception-to-HTTP Mapping as Middleware (not Filters)

**Decision:** Use ASP.NET Core middleware for exception handling, not `IExceptionFilter`.

**Why:**

- Middleware runs early, catching framework exceptions too
- Exception pipeline is unified (no missing cases)
- Deterministic ordering (other middleware can't hide exceptions)
- Works with WebSocket and streaming responses

**Trade-off:**

- Cannot inspect response headers set by endpoint
- Cannot modify response after endpoint runs
  - **Mitigation:** Use `HttpContext.Response.HasStarted` checks

**Pattern:**

```csharp
public class OntogonyExceptionHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

---

### 3. Transient Retry State in Handler (not Polly)

**Decision:** Custom retry logic with budget tracking instead of direct Polly integration.

**Why:**

- Explicit retry budget prevents cascade failures
- Circuit-breaker and retry are coordinated (one shared state)
- Deterministic with `IClock` for testing
- Retry classifier is injectable/replaceable

**Trade-off:**

- Doesn't handle all Polly scenarios (e.g., bulkhead isolation)
- Retry logic duplicated vs Polly library
  - **Mitigation:** Document that Polly can wrap `ResilientIntegrationDelegatingHandler`

**Pattern (illustrative — see shipped code):** `ResilientIntegrationDelegatingHandler` is constructed with a client name, `TransportResilienceRegistry`, `IOptions<TransportResilienceOptions>`, and `IClock`, and it coordinates retries, `IRetryClassifier` (`RetryDecision`), per-attempt and total timeouts, and `RetryBudgetPerMinute`. Register clients with `AddOntogonyIntegrationHttpClient` rather than hand-rolling this pipeline.

```csharp
// Shipped handler loop (simplified; see Ontogony.Http)
for (var attempt = 0; attempt <= maxRetries; attempt++)
{
    var response = await base.SendAsync(request, cancellationToken);
    var decision = _retryClassifier.ShouldRetry(request, response, null);
    if (decision == RetryDecision.DoNotRetry || attempt == maxRetries)
        return response;

    if (decision == RetryDecision.Retry && !_registry.TryConsumeRetryBudget(_clientName, _options))
        return response;

    await Task.Delay(ComputeDelay(attempt, response), cancellationToken);
}
```

---

### 4. Protocol-Neutral Envelopes (not Protocol-Specific)

**Decision:** Single `OntogonyEnvelope<T>` for HTTP, brokers, in-process dispatch, and any adapter you provide.

**Why:**

- Services can switch transports without re-implementing
- Cross-service contract is clear
- Trace ID and context always present
- Payload hashing works the same way

**Trade-off:**

- Envelopes add overhead for high-frequency events
- Protocol-specific optimizations lose out
  - **Mitigation:** Batch events or optimize serialization per protocol

**Pattern:**

```csharp
// Same envelope shape, different transport
var envelope = new OntogonyEnvelope<OrderCreatedEvent> { ... };

// HTTP
await httpClient.PostAsJsonAsync("/events", envelope);

// Message Bus
await serviceBusClient.SendMessageAsync(new ServiceBusMessage(envelope));

// In-process
await publisher.PublishAsync(envelope);

// External wire formats (mechanical adapters in Ontogony.ProtocolIngress — generic-json, CloudEvents, MCP, A2A, AG-UI)
// Example: normalize inbound JSON to OntogonyEnvelope<RawProtocolPayload> (see package docs; no gRPC conversion helper ships today)
```

---

### 5. Payload Hash vs Full Envelope Hash

**Decision:** Hash only the payload; envelope metadata (trace ID, timestamp) is not hashed.

**Why:**

- Payload determines idempotency (same business event = same hash)
- Metadata varies per transport/retry (trace ID regenerated, timestamp updated)
- Hash verifies data integrity, not metadata integrity
- Simpler for consumer: hash is reproducible

**Trade-off:**

- Envelope tampering (e.g., changing trace ID) is undetected
  - **Mitigation:** Use HMAC signing for inter-service messages

**Pattern:**

```csharp
// Envelope structure
{
  "eventId": "abc-123",
  "traceId": "trace-xyz",        // ← Not hashed
  "payload": { "customerId": 1 },
  "payloadHash": "sha256:def..."  // ← Hash of only this
}

// Idempotency key
var idempotencyKey = envelope.PayloadHash;  // Reproducible
```

---

### 6. Test Doubles with Explicit Fakes (not Mocks)

**Decision:** Provide `FakeClock`, `FakeEventPublisher`, `StubHttpMessageHandler` instead of relying on mocking libraries.

**Why:**

- Fakes have controllable behavior (advance time, record calls)
- Easier to test state transitions
- Faster than reflection-based mocks
- Deterministic (no randomness in test doubles)

**Trade-off:**

- More code to write initially
- Not flexible to unexpected changes
  - **Mitigation:** Document what each fake does

**Pattern:**

```csharp
[Fact]
public async Task RetryBudgetResets()
{
    var clock = new FakeClock(DateTimeOffset.UtcNow);
    var handler = new ResilientIntegrationDelegatingHandler(..., clock);

    // Advance time to reset budget window
    clock.Advance(TimeSpan.FromMinutes(1));

    // Now retries are available again
}
```

---

### 7. Conformance Kits as Static Assertion Helpers

**Decision:** Static helper classes that throw `InvalidOperationException` instead of test-framework-specific attributes.

**Why:**

- Work with any test framework (xUnit, NUnit, MSTest)
- Can be used in integration tests or console apps
- Clear assertion semantics
- Reusable across multiple test classes

**Trade-off:**

- Exception-based assertions are less idiomatic
- No specialized assertion messages
  - **Mitigation:** Exception message includes expected vs actual

**Pattern:**

```csharp
public static class TracingConformanceAssertions
{
    public static async Task AssertCanonicalTraceHeaderEchoedAsync(string traceId)
    {
        var context = /* ... */;
        if (!context.Response.Headers.Contains(TRACE_HEADER_NAME))
            throw new InvalidOperationException(
                $"Expected header '{TRACE_HEADER_NAME}' in response");
    }
}

// Usage
try
{
    await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("trace-123");
}
catch (InvalidOperationException ex)
{
    _logger.LogError(ex, "Conformance check failed");
    throw;
}
```

---

## Package Boundaries

```
Ontogony.Primitives (no dependencies)
  ↓
Ontogony.Contracts (depends on Primitives)
  ↓
Ontogony.Observability (depends on Contracts)
Ontogony.Errors (depends on Contracts)
Ontogony.Http (depends on Contracts)
...
  ↓
Ontogony.Testing (depends on most, but only for testing)
```

**Rule:** No circular dependencies. Packages should form a DAG.

---

## Versioning Strategy

**File structure:**

```
global.json          ← Single source of truth for version
CHANGELOG.md         ← Human-readable release notes
scripts/pack-all.ps1 ← Reads global.json, applies to all projects
Directory.Build.props ← Shared build properties (includes version)
```

**Version format:**

```
0.2.0           ← Stable release
0.3.0-alpha.1   ← Pre-release (alpha)
0.3.0-local     ← Local development (never published)
```

**Bump decision:**

- `MAJOR` — Breaking change (e.g., middleware order requirement)
- `MINOR` — Feature (e.g., new conformance kit)
- `PATCH` — Bug fix (e.g., retry timeout logic)
- `-prerelease` — Testing before GA

---

## Testing Strategy

| Type | Where | Example |
|------|-------|---------|
| **Unit** | `tests/Ontogony.*.Tests/` | Individual package tests |
| **Conformance** | `tests/Ontogony.Infrastructure.Tests/` | Cross-package adoption tests |
| **Integration** | `tests/Ontogony.*.Tests/` with PostgreSQL | HTTP client + database |
| **Smoke** | Manual or CI scripts | Full system startup check |

---

## Release Process

```
1. Update global.json version
2. Update CHANGELOG.md with changes
3. Commit and tag: git tag v0.3.0
4. GitHub Actions runs:
   - restore, build, test
   - validate changelog
   - pack (creates .nupkg and .snupkg)
   - generate manifest (SHA256 hashes all packages)
   - upload artifacts
   - create GitHub Release
```

See `.github/workflows/release-packages.yml` for details.

---

## Future Improvements

### Timeout Context for Better Retry Classification

Current issue: `TaskCanceledException` classification is ambiguous.

**Hypothetical API sketch (not implemented today):**

```csharp
public record RetryExceptionContext
{
    public int AttemptNumber { get; init; }
    public int MaxRetries { get; init; }
    public TimeSpan TotalElapsed { get; init; }
    public TimeSpan AttemptTimeout { get; init; }
    public bool IsCallerCancellation { get; init; }
    public bool IsAttemptTimeout { get; init; }
    public bool IsTotalTimeout { get; init; }
}

// Today, IRetryClassifier.ShouldRetry returns RetryDecision and does not take this context.
public interface IRetryClassifierV2
{
    RetryDecision ShouldRetry(
        HttpRequestMessage request,
        HttpResponseMessage? response,
        Exception? exception,
        RetryExceptionContext context);
}
```

### Host-Level Conformance Tests

Current: Direct middleware instance testing.

Recommended: `WebApplicationFactory` integration tests.

```csharp
public class HostConformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task HostEchoesTraceHeaders()
    {
        var client = _factory.CreateClient();
        // Test via real HTTP, not middleware directly
    }
}
```

### Real NuGet Package Publishing

Current: Dry-run only (artifact release).

Recommended: Real `dotnet nuget push` to GitHub Packages or private feed.

---

## ADR (Architecture Decision Records)

Location: `docs/adr/`

Key ADRs (filenames in `docs/adr/`):

- `0001-shared-mechanics-not-shared-meaning.md` — platform boundary
- `0002-trace-correlation-first.md` — correlation defaults
- `0003-cloud-events-compatible-envelope.md` — envelope interoperability
- `0004-postgres-outbox-before-kafka.md` — durable dispatch ordering
- `0005-fake-http-disabled-integration-modes.md` — test vs integration modes
- `0006-canonical-json-hashing.md` — deterministic hashing contracts

---

## Contributing

When proposing changes:

1. **Does this follow "share mechanics, not meaning"?**
   - If not, it belongs in a product repo.

2. **Does this create coupling?**
   - Services should be able to opt out or replace the implementation.

3. **Is this deterministic and testable?**
   - Use `IClock`, avoid random or time-dependent behavior.

4. **Are breaking changes documented?**
   - Add migration note in `docs/migrations/`.

---

**Last Updated:** May 2026  
**Version:** 0.2.0
