# Proposed PR sequence

## PR1 — Harden HTTP resilience and idempotency safety

**Goal:** Make `Ontogony.Http` safe for real service-to-service traffic before Athanor/Agentor start using it.

This is the most urgent next PR.

### Scope

Enhance:

```text
src/Ontogony.Http/
```

Add explicit safe retry policy:

```text
GET / HEAD / OPTIONS
  retry allowed by default

POST / PUT / PATCH / DELETE
  retry only if Idempotency-Key exists, unless explicitly disabled

multipart / streaming / unknown large content
  no retry in shared handler
```

Add options:

```csharp
public bool RetryUnsafeMethodsOnlyWithIdempotencyKey { get; set; } = true;
public string IdempotencyKeyHeaderName { get; set; } = "Idempotency-Key";
public int MaxBufferedContentBytes { get; set; } = 1_000_000;
public bool RetryMultipartContent { get; set; } = false;
```

Fix circuit failure accounting:

```text
final transient exception should count as circuit failure
final retryable HTTP response should count as circuit failure
success should close/reset circuit
synthetic circuit-open response should be observable
```

Use `IClock` from `Ontogony.Primitives` in `TransportResilienceRegistry` instead of raw `DateTimeOffset.UtcNow`.

### Tests

Add/expand tests for:

```text
GET retries on 500
GET retries on 429
POST without Idempotency-Key does not retry
POST with Idempotency-Key retries
PUT/PATCH/DELETE behavior follows unsafe-method policy
oversized content is not retried
multipart content is not retried
final HttpRequestException increments circuit failure count
circuit opens after configured failures
circuit resets after success
```

### Acceptance

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
```

### Why this matters

This package will be used for calls like:

```text
Agentor → Athanor
Agentor → Conexus
Athanor → recorder ingestion
future protocol adapters → services
```

So retry behavior must be conservative and idempotency-aware.

---

## PR2 — Observability v1: trace, span, headers, and OpenTelemetry integration surface

**Goal:** Make `Ontogony.Observability` the unified correlation layer for Athanor, Agentor, Conexus-facing adapters, and future protocol recorders.

### Scope

Enhance:

```text
src/Ontogony.Observability/
src/Ontogony.Contracts/
tests/Ontogony.Observability.Tests/
```

Add stronger support for:

```text
W3C traceparent / tracestate
X-Ontogony-Trace-Id
X-Athanor-Trace-Id
X-Agentor-Trace-Id
X-Conexus-Request-Id
X-Ontogony-Actor-Id
X-Ontogony-Tenant-Id
X-Ontogony-Workspace-Id
X-Ontogony-Project-Id
X-Ontogony-Session-Id
```

Define canonical constants for span tags:

```csharp
public static class OntogonySpanAttributes
{
    public const string TraceId = "ontogony.trace_id";
    public const string OperationId = "ontogony.operation_id";
    public const string ActorId = "ontogony.actor_id";
    public const string TenantId = "ontogony.tenant_id";
    public const string WorkspaceId = "ontogony.workspace_id";
    public const string ProjectId = "ontogony.project_id";
    public const string SessionId = "ontogony.session_id";
    public const string Protocol = "ontogony.protocol";
    public const string EventType = "ontogony.event_type";
}
```

Add helper APIs:

```csharp
OntogonyCorrelationContext.CurrentOrCreate()
OntogonyCorrelationContext.ToMetadata()
OntogonyCorrelationContext.FromHeaders(...)
OntogonyCorrelationContext.FromEnvelope(...)
```

Add metrics helpers:

```text
HTTP request count
HTTP duration
HTTP error count
integration call count
integration duration
integration error count
event published count
event handled count
```

Do **not** add vendor-specific exporters yet. Keep it vendor-neutral.

### Tests

```text
new trace ID generated when no incoming trace exists
Ontogony header wins over legacy headers
legacy Athanor header accepted
legacy Agentor header accepted
legacy Conexus request ID accepted
traceparent accepted/preserved where possible
response headers are echoed
outgoing HTTP correlation headers are propagated
nested async context restores prior context
background context can be manually pushed
```

### Acceptance

A sample ASP.NET app can do:

```csharp
builder.Services.AddOntogonyObservability(...);
app.UseOntogonyRequestTracing();
```

and any outbound `Ontogony.Http` client propagates the same trace context.

---

## PR3 — Error contracts v1: safe API errors, exception mapping, and ProblemDetails bridge

**Goal:** Make `Ontogony.Errors` safe enough to replace duplicated Athanor/Agentor middleware.

### Scope

Enhance:

```text
src/Ontogony.Errors/
tests/Ontogony.Infrastructure.Tests/
docs/
```

Replace simple exception mapping with richer mapping:

```csharp
public sealed record ExceptionMapping(
    HttpStatusCode StatusCode,
    string ErrorCode,
    string? PublicMessage = null,
    bool IncludeExceptionMessage = false,
    bool LogAsWarning = true,
    bool IncludeDetails = false);
```

Add a consistent API error shape:

```csharp
public sealed record ApiError(
    string Code,
    string Message,
    string? TraceId = null,
    object? Details = null,
    string? Instance = null);
```

Add optional `ProblemDetails` compatibility:

```text
ApiError → ProblemDetails
ProblemDetails → ApiError
```

Support service-specific mappings without referencing service-specific exception types in platform code:

```csharp
services.AddOntogonyErrors(options =>
{
    options.Map<ValidationException>(HttpStatusCode.BadRequest, "ValidationFailed", "The request is invalid.");
    options.Map<UnauthorizedAccessException>(HttpStatusCode.Forbidden, "Forbidden", "The operation is not allowed.");
});
```

Add redaction discipline:

```text
never expose unmapped exception message
never expose stack trace
mapped exception message only if explicitly allowed
trace ID always included if present
```

### Tests

```text
unmapped exception returns generic 500
mapped exception returns safe public message
mapped exception can opt into exception message
trace ID appears in error response
details are included only when mapping allows it
log level differs between expected and unexpected exceptions
middleware does not write if response already started
```

### Acceptance

Athanor and Agentor can both replace their local exception middleware with service-specific mappings on top of `Ontogony.Errors`.

---

## PR4 — Contracts v1: protocol-neutral event envelope and CloudEvents bridge

**Goal:** Make `Ontogony.Contracts` the stable event vocabulary for AG-UI/MCP/A2A/Conexus/Agentor/Athanor event flows.

### Scope

Enhance:

```text
src/Ontogony.Contracts/
schemas/
docs/04_EVENT_ENVELOPE_STANDARD.md
tests/
```

Stabilize:

```csharp
OntogonyEnvelope<TPayload>
OntogonyEventHeaders
OntogonyEventMetadata
OntogonyActorRef
OntogonySubjectRef
OntogonyArtifactRef
OntogonyTraceRef
```

Add event-type naming helpers:

```text
agui.message.created
agui.state.updated
mcp.tool.invoked
mcp.tool.completed
a2a.task.created
a2a.artifact.generated
llm.request.created
llm.response.completed
agent.run.started
agent.step.completed
athanor.decision.detected
```

Add CloudEvents mapping:

```csharp
CloudEventEnvelope
OntogonyEnvelope.ToCloudEvent()
OntogonyEnvelope.FromCloudEvent()
```

CloudEvents fields should map roughly as:

```text
id              ← eventId
type            ← eventType
source          ← source
time            ← occurredAt
subject         ← subject metadata if available
datacontenttype ← application/json
data            ← payload
extensions      ← trace/session/tenant/protocol metadata
```

Add JSON schema update:

```text
schemas/ontogony-envelope.schema.json
```

### Tests

```text
envelope serializes deterministically
required fields validate
metadata round-trips
CloudEvents conversion preserves eventId/eventType/source/time
trace/tenant/project/session extensions round-trip
payload hash remains untouched
```

### Acceptance

A protocol recorder can receive an AG-UI/MCP/A2A payload and wrap it into a stable `OntogonyEnvelope<TPayload>` without importing Athanor/Agentor domain types.

---

## PR5 — Hashing and idempotency v1: canonical JSON, payload hash, and operation fingerprints

**Goal:** Make event hashes and idempotency keys deterministic enough for protocol recording and retry safety.

### Scope

Enhance:

```text
src/Ontogony.Hashing/
src/Ontogony.Idempotency/
tests/Ontogony.Hashing.Tests/
tests/Ontogony.Infrastructure.Tests/
```

Add/confirm:

```text
canonical JSON
SHA-256 lowercase hex
payload hashing
operation fingerprinting
idempotency key construction
stable prefix/version support
```

Improve `IdempotencyKeyBuilder`:

```csharp
public sealed record IdempotencyKeyOptions
{
    public string Namespace { get; init; } = "ontogony";
    public string Version { get; init; } = "v1";
    public int MaxKeyLength { get; init; } = 256;
}
```

Result format:

```text
ontogony:agentor.run.start:v1:<sha256>
athanor:event.ingest:v1:<sha256>
conexus:model.call:v1:<sha256>
```

Add deterministic event hash helper:

```csharp
public string ComputeEnvelopePayloadHash<TPayload>(OntogonyEnvelope<TPayload> envelope);
public string ComputeOperationFingerprint(string operation, object payload);
```

### Tests

```text
object keys sorted recursively
arrays preserve order
null preserved
missing property differs from null property
unicode stable
dictionary ordering stable
equivalent JSON with different whitespace hashes same
different schema version hashes differently when included
idempotency key has safe characters
idempotency key respects max length
```

### Acceptance

This package can be used by:

```text
Ontogony.Http for Idempotency-Key
Athanor event ingestion deduplication
Agentor run start fingerprinting
Conexus request fingerprinting later
```

---

## PR6 — Security primitives v1: current actor, trusted header mode, claims mode, and startup guard docs

**Goal:** Make `Ontogony.Security` safe and explicit before services adopt it.

### Scope

Enhance:

```text
src/Ontogony.Security/
docs/security/
tests/Ontogony.Infrastructure.Tests/
```

Keep current header mode, but document it clearly:

```text
HeaderCurrentActorAccessor is not authentication.
It trusts upstream middleware/infrastructure.
Do not expose directly to the public internet.
```

Add:

```csharp
ClaimsCurrentActorAccessor
ServiceIdentityCurrentActorAccessor
OntogonySecurityHeaders
OntogonyRoleNames? // only generic roles, not business roles
```

Keep generic roles only:

```text
HumanOperator
Service
Agent
Admin
```

Do **not** include:

```text
GovernanceApprover
Canonizer
SupportManager
RefundApprover
```

Those belong in product services.

Add authentication guard modes:

```text
Disabled
Header
Jwt
```

Validation:

```text
Disabled blocked outside Development/Test unless explicitly allowed.
Header mode warns/docs that it requires trusted upstream.
JWT mode requires authority or explicit unvalidated-token override.
Unvalidated JWT blocked outside Development/Test unless explicitly allowed.
```

### Tests

```text
disabled mode blocked in Production
disabled mode allowed in Development
header mode requires actor header name
JWT requires authority unless unvalidated allowed
unvalidated JWT blocked outside Development/Test
claims accessor extracts actor ID from configured claim priority
roles extracted from configured role claim
```

### Acceptance

Agentor/Athanor can use this for internal-alpha and staging without importing product-specific auth semantics.

---

## PR7 — Messaging v1: in-memory bus, envelope publishing, handler dispatch, and test doubles

**Goal:** Make `Ontogony.Messaging` useful enough for local services and tests before adding outbox/Kafka/NATS.

### Scope

Enhance:

```text
src/Ontogony.Messaging/
src/Ontogony.Testing/
tests/Ontogony.Messaging.Tests/
```

Add:

```csharp
IEventPublisher
IEventHandler<TPayload>
IEventSerializer
InMemoryEventPublisher
InMemoryEventSink
EventDispatchOptions
```

Support:

```text
publish envelope
capture published events in tests
dispatch to registered handlers
preserve correlation metadata
set payload hash optionally
validate required envelope fields
```

Add test helper:

```csharp
PublishedEventRecorder
FakeEventPublisher
EnvelopeAssertions
```

Do **not** add outbox yet. Keep this PR about in-process mechanics.

### Tests

```text
published event captured
handler receives envelope
multiple handlers receive envelope
handler exception behavior is configurable
correlation context can enrich envelope metadata
payload hash is computed when enabled
```

### Acceptance

Future Athanor recorder MVP can use this to publish normalized events in tests without choosing Kafka/NATS/EventHubs yet.

---

## PR8 — Persistence foundations v1: outbox contracts and PostgreSQL-neutral primitives

**Goal:** Prepare for durable events without introducing service-specific repositories or schemas.

### Scope

Enhance:

```text
src/Ontogony.Persistence/
src/Ontogony.Messaging/
docs/
tests/
```

Add contracts only first:

```csharp
IOutboxWriter
IOutboxReader
IOutboxDispatcher
IProcessedMessageStore
IUnitOfWorkBoundary
```

Add neutral records:

```csharp
OutboxMessage
ProcessedMessage
OutboxDispatchResult
```

Fields:

```text
message_id
event_id
event_type
source
trace_id
occurred_at
available_at
attempt_count
last_error
payload_json
payload_hash
metadata_json
```

Add SQL-agnostic documentation:

```text
docs/persistence/outbox-contract.md
docs/persistence/idempotent-consumer.md
```

Do **not** add EF entities for Athanor or Agentor.

Do **not** add generic repositories.

### Tests

For this PR, test contracts/helpers only:

```text
outbox message validation
processed message key construction
retry schedule calculation
metadata serialization
```

### Acceptance

You have a stable persistence contract that can later get:

```text
Ontogony.Persistence.Postgres
Ontogony.Persistence.EfCore
```

without contaminating the core package.

---

## PR9 — Testing package v1: test fixtures for services, middleware, HTTP, messaging, and time

**Goal:** Make `Ontogony.Testing` genuinely useful to Athanor/Agentor migrations.

### Scope

Enhance:

```text
src/Ontogony.Testing/
tests/
```

Add:

```csharp
FakeClock
FakeIdGenerator
FakeCurrentActorAccessor
FakeEventPublisher
StubHttpMessageHandler
RecordingHttpMessageHandler
TestCorrelationScope
EnvelopeFixtureBuilder
ApiErrorAssertions
CanonicalJsonAssertions
```

Add middleware test helpers:

```text
create default HttpContext
run middleware pipeline
assert response JSON
assert response headers
```

Add HTTP testing helpers:

```text
scripted sequence of responses
capture outgoing requests
assert correlation headers
assert retry count
```

### Tests

Self-test all fixtures.

### Acceptance

Future PRs in Agentor/Athanor can depend on `Ontogony.Testing` to simplify migration tests.

---

## PR10 — Documentation and examples v1: service adoption cookbook

**Goal:** Make the repo usable by agents and humans without repeatedly re-explaining the architecture.

### Scope

Add docs:

```text
docs/adoption/agentor-observability-adoption.md
docs/adoption/athanor-observability-adoption.md
docs/adoption/conexus-event-emission-adoption.md
docs/adoption/http-client-adoption.md
docs/adoption/error-middleware-adoption.md
docs/adoption/package-versioning.md
```

Add examples:

```text
examples/MinimalApiWithOntogonyObservability/
examples/IntegrationHttpClientExample/
examples/EventEnvelopeExample/
examples/ErrorMiddlewareExample/
```

Each example should build if included, or be explicitly marked as documentation-only.

Add “do not do this” sections:

```text
Do not share service domain models.
Do not put policy semantics in Ontogony.Platform.
Do not add generic repository abstractions.
Do not retry unsafe HTTP methods without idempotency.
Do not trust header actor mode on public routes.
```

### Acceptance

A new coding agent should be able to implement an Athanor or Agentor adoption PR from docs alone.

---

# After platform hardening: adoption PRs in product repos

Only after PR1–PR5 are stable, start adopting in real repos.

## Agentor PR-A — Adopt Ontogony.Observability and Ontogony.Http

### Scope

Replace local:

```text
RequestTracingMiddleware
AgentorCorrelationContext
AgentorDiagnostics where appropriate
CorrelationHeadersDelegatingHandler
parts of HTTP resilience
```

with:

```text
Ontogony.Observability
Ontogony.Http
```

Keep Agentor-specific semantic events and run traces inside Agentor.

Acceptance:

```text
existing Agentor tests pass
public API trace headers remain backward compatible
X-Agentor-Trace-Id still accepted/emitted if required
```

---

## Athanor PR-A — Adopt Ontogony.Hashing and Ontogony.Primitives

### Scope

Replace:

```text
IClock / SystemClock
IIdGenerator / GuidIdGenerator
IContentHashService / SHA-256 mechanics
```

with platform versions.

Keep Athanor canonical hash policy if it has domain-specific semantics.

Acceptance:

```text
snapshot hashes unchanged where expected
content hashes unchanged
existing Athanor tests pass
```

---

## Athanor PR-B — Adopt Ontogony.Observability and Ontogony.Errors

### Scope

Replace:

```text
TraceIdMiddleware
basic error middleware mechanics
```

with:

```text
Ontogony.Observability
Ontogony.Errors
```

Keep Athanor exception mappings in Athanor.

Acceptance:

```text
X-Athanor-Trace-Id compatibility preserved
Athanor API error contract remains compatible or migration documented
```

---

## Conexus PR-A — Emit Ontogony envelopes from Python

### Scope

No .NET import.

Add small Python client/helper to emit:

```text
llm.request.created
llm.response.completed
llm.provider.failed
llm.cost.recorded
```

using the same envelope schema.

Acceptance:

```text
Conexus request ID maps to X-Conexus-Request-Id
trace ID can be passed downstream
payload hash computed compatibly or deferred with explicit TODO
```

---

# Suggested release grouping

## Release 0.3 — Safe transport and observability

Includes:

```text
PR1 HTTP idempotency-safe resilience
PR2 Observability v1
PR3 Error contracts v1
```

This makes the platform ready for early service adoption.

## Release 0.4 — Event substrate

Includes:

```text
PR4 Contracts + CloudEvents bridge
PR5 Hashing/idempotency v1
PR7 Messaging v1
```

This makes the platform ready for Athanor protocol recorder MVP.

## Release 0.5 — Operational readiness

Includes:

```text
PR6 Security primitives v1
PR8 Persistence/outbox contracts
PR9 Testing package v1
PR10 Docs/examples cookbook
```

This makes the platform mature enough for repeated use across repos.

---

# My recommended immediate sequence

Do these first:

```text
1. PR1 — Harden HTTP resilience and idempotency safety
2. PR2 — Observability v1
3. PR3 — Error contracts v1
4. PR4 — Contracts v1 + CloudEvents bridge
5. PR5 — Hashing and idempotency v1
```

Then pause and adopt into **one** real repo, preferably Agentor for observability/HTTP, or Athanor for hashing/primitives.

The main rule for every PR:

> Comprehensive, but bounded. Each PR should leave the platform more adoptable by real services, not merely more abstract.
