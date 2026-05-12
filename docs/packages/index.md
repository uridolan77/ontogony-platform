# Ontogony Packages — Reference

This index describes all 14 NuGet packages and when to use each one.

---

## Core Foundation

### `Ontogony.Primitives`

**Purpose:** Lowest-level types and helpers.

**Provides:**
- Common markers and traits
- Utility extension methods
- Base type definitions

**When to use:** Almost always as a transitive dependency. Rarely directly imported.

**Example:**
```csharp
using Ontogony.Primitives;
```

---

### `Ontogony.Contracts`

**Purpose:** Event envelope DTOs, protocol names, canonical header constants.

**Provides:**
- `OntogonyEnvelope<TPayload>` — Standard event wrapper
- `OntogonyEventHeaders` — Canonical header names (trace ID, tenant, actor, workspace)
- `ProtocolNames` — Well-known protocol types (internal, cloudevents, generic-json)

**When to use:** Any service that publishes events, consumes from message bus, or needs to validate event format.

**Example:**
```csharp
using Ontogony.Contracts.Events;

var envelope = new OntogonyEnvelope<MyEvent>
{
    EventId = Guid.NewGuid().ToString("n"),
    EventType = "ontogony.myservice.operation.completed",
    Source = "ontogony://myservice/domain",
    OccurredAt = DateTimeOffset.UtcNow,
    TraceId = correlationContext.TraceId,
    Protocol = ProtocolNames.Internal,
    Payload = myEvent
};
```

---

## Observability & Tracing

### `Ontogony.Observability`

**Purpose:** Trace correlation context, request tracing middleware, diagnostic sources.

**Provides:**
- `RequestTracingMiddleware` — Extracts/generates trace IDs, echoes in responses
- `OntogonyCorrelationContext` — Async-local storage for trace, actor, tenant, workspace
- `OntogonyDiagnostics` — Activity source and meter for OpenTelemetry

**When to use:** In any ASP.NET Core service. Use first in middleware pipeline.

**Example:**
```csharp
services.Configure<OntogonyObservabilityOptions>(opts =>
{
    opts.ServiceName = "my-service";
    opts.TraceHeaderName = "X-Ontogony-Trace-Id";
});

app.UseRequestTracingMiddleware();

// Later, in handlers:
var traceId = OntogonyCorrelationContext.TraceId;
var tenant = OntogonyCorrelationContext.Current?.TenantId;
```

---

## Error Handling

### `Ontogony.Errors`

**Purpose:** Exception-to-HTTP status mapping, canonical error JSON shape.

**Provides:**
- `OntogonyExceptionHandlingMiddleware` — Catches exceptions, maps to HTTP response
- `OntogonyExceptionMappingOptions` — Configurable exception-to-status rules
- `OntogonyProblemDetails` — RFC 7807 problem details with trace ID

**When to use:** In error middleware pipeline. Validates all exceptions are either caught or mapped.

**Example:**
```csharp
services.Configure<OntogonyExceptionMappingOptions>(opts =>
{
    opts.MapException<ValidationException>(StatusCodes.Status400BadRequest, "validation_error");
});

app.UseOntogonyExceptionHandling();
```

---

## HTTP Resilience

### `Ontogony.Http`

**Purpose:** Resilient HTTP client integration with retry, circuit-breaker, timeout coordination.

**Provides:**
- `ResilientIntegrationDelegatingHandler` — Retries with jitter, circuit-breaker, budget tracking
- `TransportResilienceOptions` — Timeout, max retries, circuit-breaker thresholds
- `IRetryClassifier` — Determines whether error is retryable
- `DefaultRetryClassifier` — Built-in classification logic

**When to use:** Whenever making outbound HTTP calls to other services.

**Example:**
```csharp
services.AddIntegrationHttpClient<IMyServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://my-service");
})
.ConfigureHttpClientDefaults(opts =>
{
    opts.Timeout = TimeSpan.FromSeconds(10);
    opts.MaxAttempts = 3;
    opts.OpenCircuitAfterConsecutiveFailures = 5;
});
```

---

## Security & Authentication

### `Ontogony.Security`

**Purpose:** HMAC-SHA256 signing, actor context, claims validation.

**Provides:**
- `ServiceIdentityHmacSignatureHelper` — Create and verify HMAC signatures
- `CurrentActorAccessor` — Retrieve authenticated actor from request context
- `OntogonyClaimsValidator` — Validate JWT/bearer token claims
- `OntogonySecurityOptions` — Configure identity, signing secrets, clock skew

**When to use:** Service-to-service authentication, request signing, actor audit trails.

**Example:**
```csharp
services.Configure<OntogonySecurityOptions>(opts =>
{
    opts.ServiceIdentity = "ontogony://my-service";
    opts.SharedSecret = Environment.GetEnvironmentVariable("SERVICE_SHARED_SECRET");
});

// Sign outbound request:
var signature = ServiceIdentityHmacSignatureHelper.SignRequest(
    secret: options.SharedSecret,
    method: "POST",
    path: "/api/events",
    timestamp: clock.UtcNow,
    nonce: Guid.NewGuid().ToString("n"),
    bodyHash: "..."
);
```

---

## Hashing & Determinism

### `Ontogony.Hashing`

**Purpose:** Canonical JSON hashing, payload fingerprints, deterministic serialization.

**Provides:**
- `CanonicalJsonHasher` — Deterministic SHA256 of JSON payloads
- `EnvelopePayloadHasher` — Hash event envelope payloads for tamper detection
- `PayloadHasher` — Generic payload hashing

**When to use:** Building idempotency keys, payload integrity checks, deterministic fingerprints.

**Example:**
```csharp
var payloadJson = JsonSerializer.Serialize(myEvent);
var hash = hasher.ComputeHash(payloadJson);
// Use hash in idempotency keys or tamper detection
```

---

## Configuration & Hosting

### `Ontogony.Configuration`

**Purpose:** Reusable configuration helpers, validation, startup guards.

**Provides:**
- Configuration builders and extensions
- Options validation
- Startup guard patterns

**When to use:** Setting up strongly-typed options during service startup.

---

### `Ontogony.Hosting`

**Purpose:** ASP.NET Core service startup patterns, extension methods.

**Provides:**
- Middleware registration helpers
- Service collection extensions
- Structured startup flow

**When to use:** Organizing your `Program.cs` with Ontogony middleware.

---

## Messaging & Events

### `Ontogony.Messaging`

**Purpose:** Protocol-neutral event publishing and consumption abstractions.

**Provides:**
- `IEventPublisher<T>` — Abstract event publishing interface
- `IEventSubscriber<T>` — Abstract event subscription
- Event handler patterns

**When to use:** Publishing domain events, integrating with message buses (Azure Service Bus, RabbitMQ, etc.).

---

### `Ontogony.ProtocolIngress`

**Purpose:** Convert incoming HTTP/gRPC/protocol messages into canonical `OntogonyEnvelope` format.

**Provides:**
- Protocol deserializers (JSON, gRPC)
- Enum and contract validation
- `DefaultEnvelopeValidator` — Verify envelope schema compliance
- Header mapping (protocol-specific → canonical)

**When to use:** API gateway, message adapter, protocol translation layers.

**Example:**
```csharp
var incoming = request.Body;
var envelope = await protocolIngress.DeserializeAsync<MyPayload>(
    incoming,
    protocolName: ProtocolNames.CloudEvents
);
// Envelope now has canonical shape, validated, trace ID propagated
```

---

## Data Persistence

### `Ontogony.Persistence`

**Purpose:** Data access patterns, repository abstractions, migrations.

**Provides:**
- Entity mapping helpers
- Migration patterns
- Audit trail support

**When to use:** Setting up data access layer, managing schema changes.

---

### `Ontogony.Persistence.Postgres`

**Purpose:** PostgreSQL-specific implementations, connection pooling, JSON operators.

**Provides:**
- Postgres-specific EF Core extensions
- JSONB column mappings
- Connection string builders
- Migration runners

**When to use:** Using PostgreSQL as your primary data store.

**Example:**
```csharp
services.AddPostgresContext<MyDbContext>(options =>
{
    options.UseNpgsql(
        connectionString,
        postgresOptions => postgresOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    );
});
```

---

## Idempotency & Consistency

### `Ontogony.Idempotency`

**Purpose:** Idempotency keys, request deduplication, at-most-once semantics.

**Provides:**
- `IdempotencyKeyGenerator` — Create stable fingerprint keys
- `IdempotencyStore` — Track processed requests
- Idempotency middleware

**When to use:** APIs that must be safely retried without duplication (payment APIs, order creation, etc.).

---

## Testing

### `Ontogony.Testing`

**Purpose:** Test doubles, middleware harnesses, conformance assertion kits.

**Provides:**

**Test Doubles:**
- `FakeClock` — Deterministic time for testing
- `FakeIdGenerator` — Sequential IDs
- `FakeEventPublisher` — Capture published events
- `StubHttpMessageHandler` — Scripted HTTP responses
- `TestCorrelationScope` — Push temporary correlation context

**Conformance Kits:**
- `TracingConformanceAssertions` — Verify trace header echo and propagation
- `ErrorShapeConformanceAssertions` — Verify error JSON shape and codes
- `EnvelopeConformanceAssertions` — Verify event format compliance
- `HmacConformanceAssertions` — Verify HMAC signing round-trips
- `OutboxConformanceHarness` — Verify outbox write/read/dispatch
- `HttpResilienceConformanceHarness` — Verify retry and circuit-breaker behavior

**When to use:** Writing unit/integration tests that validate Ontogony adoption.

**Example:**
```csharp
// In your test:
await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("trace-123");
EnvelopeConformanceAssertions.AssertFullConformance(envelope);
```

---

## Dependency Graph

```
Ontogony.Primitives
├── Ontogony.Configuration
├── Ontogony.Contracts
├── Ontogony.Errors
├── Ontogony.Hashing
├── Ontogony.Hosting
├── Ontogony.Http
├── Ontogony.Idempotency
├── Ontogony.Messaging
├── Ontogony.Observability → Ontogony.Contracts
├── Ontogony.ProtocolIngress → Ontogony.Contracts
├── Ontogony.Persistence
├── Ontogony.Persistence.Postgres → Ontogony.Persistence
├── Ontogony.Security
└── Ontogony.Testing
    ├── Ontogony.Observability
    ├── Ontogony.Errors
    ├── Ontogony.Http
    ├── Ontogony.Hashing
    ├── Ontogony.Messaging
    └── Ontogony.Persistence
```

---

## Quick Selection Matrix

| Use Case | Package | Alternative |
|----------|---------|-------------|
| Publish events | `Ontogony.Messaging` | `Ontogony.Contracts` (if hand-crafting) |
| Trace requests | `Ontogony.Observability` | — (required) |
| Handle errors | `Ontogony.Errors` | — (required) |
| Call other services | `Ontogony.Http` | `HttpClient` (not recommended) |
| Authenticate | `Ontogony.Security` | — (required) |
| Store data | `Ontogony.Persistence.Postgres` | Any ORM |
| Idempotent APIs | `Ontogony.Idempotency` | Custom deduplication |
| Integration tests | `Ontogony.Testing` | Any test framework + helpers |
| Convert protocols | `Ontogony.ProtocolIngress` | Manual deserialization |

---

## Installation

All packages are available on GitHub Packages (or your internal NuGet feed).

```bash
dotnet add package Ontogony.Observability
dotnet add package Ontogony.Http
dotnet add package Ontogony.Errors
# ... others as needed
```

---

## Next Steps

- **Start integration:** [Adoption Path](../adoption/)
- **Understand design:** [Architecture Decisions](../architecture/)
- **See examples:** [Examples](../examples/)
- **Run conformance tests:** [Testing](../adoption/conformance-testing.md)

---

**Last Updated:** May 2026  
**Version:** 0.2.0
