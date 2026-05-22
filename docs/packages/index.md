# Ontogony Packages — Reference

This index describes all **27** shipping NuGet packages and when to use each one.

**Pack hygiene:** coordination paths such as `_issue_bodies/` and `.tmp/` must **never** appear inside published `.nupkg` archives. CI runs [`scripts/validate-nupkg-coordination-path-hygiene.ps1`](../../scripts/validate-nupkg-coordination-path-hygiene.ps1) after pack.

---

## Core Foundation

### `Ontogony.Primitives`

**Purpose:** Lowest-level clock and ID primitives shared by all packages.

**Provides:**
- `IClock` / `SystemClock` — Deterministic time abstraction (used everywhere; swap with `FakeClock` in tests)
- `IIdGenerator` / `GuidIdGenerator` — Stable identifier generation

**When to use:** Almost always as a transitive dependency. Directly import when you need to inject `IClock` or `IIdGenerator`.

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
- `ProtocolNames` — Well-known protocol constants (`agentor`, `athanor`, `conexus`, `generic-json`, `cloudevents`, `mcp`, `a2a`, `ag-ui`, etc.)

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
    Protocol = ProtocolNames.GenericJson,  // choose protocol matching your transport
    Payload = myEvent
};
```

---

### `Ontogony.AI.Contracts`

**Purpose:** Mechanical DTOs for LLM requests, responses, stream chunks, usage, cost, provider errors, tool-call facts, and model capability descriptors.

**Provides:**
- `LlmRequestEnvelope` / `LlmResponseEnvelope` / `LlmStreamChunk`
- `LlmUsageRecord` (with `ResolveTotalTokensOrSum()` helper), `LlmCostRecord`, `LlmProviderError`
- `ToolCallRecord`, `ModelCapabilityDescriptor`

**Opaque strings:** `Provider` and `Model` are caller-defined identifiers (no platform enums or provider registry).

**When to use:** Emitting or recording provider-neutral LLM telemetry; wrap in `OntogonyEnvelope<TPayload>` when using the standard envelope pipeline.

**Non-goals:** No routing, ranking, planning, canonization, or KB semantics (see `docs/ai-runtime/boundary-guardrails.md` and [package notes](Ontogony.AI.Contracts.md)).

**Example:**
```csharp
using Ontogony.AI.Contracts;

var usage = new LlmUsageRecord(10, 20, null, "o200k", null);
var total = usage.ResolveTotalTokensOrSum(); // 30
```

---

### `Ontogony.Artifacts`

**Purpose:** Mechanical artifact reference contracts and an in-memory, content-addressed artifact store for large or sensitive payloads.

**Provides:**
- `ArtifactRef` — durable, serialization-friendly reference (id, content hash, size, opaque media/encoding/tier/classification, optional scope and locator URI).
- `ArtifactPutRequest` / `ArtifactPutResult` / `ArtifactContent` — write/read DTOs.
- `IArtifactStore` — mechanical port; `InMemoryArtifactStore` — thread-safe reference implementation that dedupes by `(content hash, tenant, workspace, project, media type, classification)`.
- `ArtifactNotFoundException` — thrown by `GetAsync` on unknown ids.
- `AddOntogonyInMemoryArtifactStore()` — DI registration helper for the in-memory store.

**Opaque conventions:** `MediaType`, `ContentEncoding`, `StorageTier`, and `Classification` are caller-defined strings (no platform enums or registries).

**When to use:** Recording or transferring large/sensitive LLM, tool, document, or replay payloads by reference instead of inline; wrap `ArtifactRef` in `OntogonyEnvelope<TPayload>` for the standard event pipeline.

**Non-goals:** No cloud provider SDK bindings, retention policy, eviction strategy, or product-specific lifecycle (see `docs/ai-runtime/boundary-guardrails.md` and [package notes](Ontogony.Artifacts.md)).

**Example:**
```csharp
services.AddOntogonyInMemoryArtifactStore();

var store = sp.GetRequiredService<IArtifactStore>();
var result = await store.PutAsync(new ArtifactPutRequest
{
    MediaType = "application/json",
    Content = canonicalJsonBytes,
    Classification = "internal",
    TenantId = "tenant-1"
});

// emit result.Reference inside an OntogonyEnvelope<ArtifactRef>
```

---

### `Ontogony.Execution`

**Purpose:** Mechanical execution journal DTOs (run, step, attempt, transition, checkpoint), an append-only `IExecutionJournal` port, and `InMemoryExecutionJournal` for tests and single-process hosts.

**Provides:**

- `ExecutionRunRecord`, `ExecutionStepRecord`, `ExecutionAttemptRecord`, `ExecutionStateTransitionRecord`, `ExecutionCheckpointRecord` (opaque string statuses/kinds; optional `PayloadArtifactId` on checkpoints for out-of-line bytes).
- `IExecutionJournal` / `InMemoryExecutionJournal` — append lines, `GetRunAsync`, list by `runId` or `stepId`; duplicate primary ids throw `InvalidOperationException`; lists return append order.
- `AddOntogonyInMemoryExecutionJournal()` — DI registration.

**When to use:** Recording execution timelines next to LLM/artifact telemetry; pair checkpoint `PayloadArtifactId` with `Ontogony.Artifacts` when payloads are large. Wrap payloads in `OntogonyEnvelope<TPayload>` when using the standard event pipeline.

**Non-goals:** No workflow engine, scheduler, enforced state machine, lifecycle validation on DTOs, or durable storage provider (see [package notes](Ontogony.Execution.md)).

**Example:**

```csharp
using Ontogony.Execution;

var journal = new InMemoryExecutionJournal();
await journal.AppendRunAsync(new ExecutionRunRecord
{
    RunId = "run-1",
    Status = "running",
    CreatedAt = DateTimeOffset.UtcNow
});
var run = await journal.GetRunAsync("run-1");
```

---

### `Ontogony.Replay.Contracts`

**Purpose:** Replay manifest and replay-bundle DTOs for deterministic debugging and incident reconstruction (contracts only).

**Provides:** Stable records for replay inputs, outputs, environment fingerprints, and determinism hints — **no** replay engine, storage provider, or model mocking.

**When to use:** When you need a shared wire format for debug bundles alongside LLM telemetry; adopt when request replay flows land.

**Non-goals:** No execution engine, no guarantee of deterministic model output (see [package notes](Ontogony.Replay.Contracts.md)).

---

### `Ontogony.Evaluation.Contracts`

**Purpose:** Mechanical DTOs for evaluation runs, per-scenario cases, raw metrics, scored outcomes, baseline comparisons, verdicts, and eval artifact references.

**Provides:** `EvaluationRunRecord`, `EvaluationCaseRecord`, `EvaluationMetricRecord`, `EvaluationScoreRecord`, `BaselineComparisonRecord`, `EvaluationVerdictRecord`, `EvaluationArtifactRef` — opaque string vocabularies only.

**When to use:** Recording or exchanging cross-repo eval evidence (Allagma harness, Conexus route evidence, Kanon policy links) before product-specific rubrics land in consumer repos.

**Non-goals:** No eval harness, scoring policy, topology authorization, or model routing (see [package notes](Ontogony.Evaluation.Contracts.md)).

**Example:**

```csharp
using Ontogony.Evaluation.Contracts;

var run = new EvaluationRunRecord(
    EvaluationRunId: "eval-run-1",
    SubjectRunId: "run-1",
    Verdict: new EvaluationVerdictRecord("pass", QualityScore: 0.92m));
```

---

### `Ontogony.Topology.Contracts`

**Purpose:** Mechanical DTOs for task classification, topology selection, and topology constraints.

**Provides:** `TaskClassificationRecord`, `TopologySelectionRecord`, `TopologyConstraintRecord`, `StandardTopologyModes`, `StandardTaskClassifications` — opaque string vocabularies only.

**When to use:** Recording or exchanging topology metadata (Allagma selection events, Kanon policy links, audit bundles) before product-specific selector or authorization logic lands in consumer repos.

**Non-goals:** No topology planner, Kanon policy evaluation, eval harness, or model routing (see [package notes](Ontogony.Topology.Contracts.md)).

**Example:**

```csharp
using Ontogony.Topology.Contracts;

var selection = new TopologySelectionRecord(
    TopologySelectionId: "toposel-1",
    RunId: "run-1",
    SelectedTopology: StandardTopologyModes.SingleWorkflow,
    SelectorVersion: "topology-selector-v0");
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
});

app.UseOntogonyRequestTracing();

// Later, in handlers:
var traceId = OntogonyCorrelationContext.TraceId;
var tenant = OntogonyCorrelationContext.Current?.TenantId;
```

### `Ontogony.Logging`

**Purpose:** Structured logging mechanics aligned with `Ontogony.Observability`: stable field names, event IDs, correlation-aware scopes, and ASP.NET logging-scope middleware.

**Provides:** Provider-neutral constants and helpers; optional `IRedactor` on `BeginOntogonyScope` redacts `additionalFields` by field-name rules when supplied; no sinks, exporters, or log storage.

**When to use:** Gateway and worker services that already use `Ontogony.Observability` and want consistent structured fields before choosing Serilog / OpenTelemetry exporters in the host.

**Non-goals:** No logging vendor bindings or body logging guarantees (see [package notes](Ontogony.Logging.md)).

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
    opts.Map<ValidationException>(HttpStatusCode.BadRequest, "validation_error");
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
// Register resilient HTTP client
services.AddOntogonyIntegrationHttpClient(
    "payment-service",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = configuration["Payment:BaseUrl"],
        TimeoutSeconds = 10
    });

// Configure resilience globally
services.Configure<TransportResilienceOptions>(opts =>
{
    opts.MaxRetries = 3;
    opts.CircuitFailureThreshold = 5;
    opts.CircuitOpenDurationSeconds = 30;
});
```

---

## Redaction & secrets

### `Ontogony.Redaction`

**Purpose:** Deterministic redaction primitives for structured logs, metadata maps, and provider-facing strings.

**Provides:** `IRedactor`, `DefaultRedactor`, field-name rules, and DI registration — no external redaction service.

**When to use:** Before logging or serializing objects that may contain tokens, emails, or provider keys.

**Non-goals:** Not a full PII scanner or compliance framework (see [package notes](Ontogony.Redaction.md)).

### `Ontogony.Secrets`

**Purpose:** Secret references, masking, fingerprints, and a development-only protector — **no** cloud secret store.

**Provides:** `SecretRef`, masking helpers, fingerprint port, and `DevelopmentBase64SecretProtector` for local/dev only.

**When to use:** Typing provider API keys and similar credentials with safe display metadata; pair with host-specific secret resolution in the product repo.

**Non-goals:** No Azure Key Vault / AWS Secrets Manager clients (see [package notes](Ontogony.Secrets.md)).

---

## Security & Authentication

### `Ontogony.Security`

**Purpose:** HMAC-SHA256 signing, actor context, claims validation.

**Provides:**
- `OntogonyServiceIdentitySigningHandler` — `DelegatingHandler` that signs outbound requests with HMAC-SHA256
- `ServiceIdentityOptions` — Configure signing secrets, clock skew, nonce requirements
- `OntogonyAuthenticationOptions` — Configure authentication mode (Disabled / Header / Jwt)
- `ICurrentActorAccessor` — Retrieve authenticated actor from request context
- `IServiceSecretResolver` — Plug-in interface for secrets management

**When to use:** Service-to-service authentication, request signing, actor audit trails.

**Example:**
```csharp
// Outbound: sign HTTP requests from this service
services.AddHttpClient("downstream")
    .AddHttpMessageHandler(sp => new OntogonyServiceIdentitySigningHandler(
        serviceId: "my-service",
        secret: configuration["Ontogony:ServiceSecret"]));

// Inbound: validate HMAC signatures on incoming requests
services.AddOntogonyServiceIdentityActorContext(opts =>
{
    opts.RequireHmacSignature = true;
    opts.ServiceSecrets["caller-service"] = configuration["Ontogony:CallerSecret"];
});
```

---

## Hashing & Determinism

### `Ontogony.Hashing`

**Purpose:** Canonical JSON hashing, payload fingerprints, deterministic serialization.

**Provides:**
- `CanonicalJson` — Static class: deterministic JSON serialization (sorted keys, no whitespace)
- `PayloadHasher` — SHA-256 over canonical JSON; inject to compute envelope payload hashes
- `IContentHashService` / `Sha256ContentHashService` — Low-level byte hashing

**When to use:** Building idempotency keys, payload integrity checks, deterministic fingerprints.

**Example:**
```csharp
// Canonical JSON serialization
var canonical = CanonicalJson.Serialize(myPayload);

// Payload hash (inject PayloadHasher via DI)
var hash = payloadHasher.ComputeCanonicalJsonHash(myPayload);
envelope.PayloadHash = hash;
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

**Purpose:** Protocol-neutral **in-process** event publishing and handler dispatch.

**Provides:**
- `IEventPublisher` — Publish `OntogonyEnvelope<T>` events (not generic on event type)
- `IEventPublisherWithResult` — Publish and receive a `EventPublishResult`
- `IEventHandler<TPayload>` — Implement to consume events in-process
- `EventDispatchOptions` — Control hash computation, validation, and operation mode
- `InMemoryEventPublisher` / `InMemoryEventBus` / `InMemoryEventSink` — capture and dispatch for **tests and single-process** diagnostics (not a broker or durable outbox)

**When to use:** Publishing or handling integration events inside a process, or bridging to an external bus from your own adapter.

**Outbox:** Durable outbox contracts (`IOutboxWriter`, `OutboxMessage`, etc.) are in **`Ontogony.Persistence`**, not this package.

---

### `Ontogony.ProtocolIngress`

**Purpose:** Mechanical normalization of external protocol events into `OntogonyEnvelope<RawProtocolPayload>`.

**Current adapters:** `generic-json`, `CloudEvents`, `MCP`, `A2A`, `AG-UI`.

**No gRPC adapter.** No product interpretation.

**Provides:**
- `IProtocolIngressAdapter<TRaw>` — Normalize a raw protocol event into `OntogonyEnvelope`
- `GenericJsonProtocolAdapter`, `CloudEventsProtocolAdapter`, `McpProtocolAdapter`, etc.
- `DefaultEnvelopeValidator` — Verify envelope schema compliance
- `AddOntogonyProtocolIngress()` — Register all adapters

**When to use:** API gateway, message adapter, protocol translation layers.

**Example:**
```csharp
services.AddOntogonyProtocolIngress();

// In controller/handler:
var adapter = sp.GetRequiredService<GenericJsonProtocolAdapter>();
var envelope = adapter.Normalize(rawJson, ingressContext);
// envelope is OntogonyEnvelope<RawProtocolPayload> with trace ID propagated
```

---

## Data Persistence

### `Ontogony.Persistence`

**Purpose:** Outbox contracts, processed-message tracking, dead-letter store, and in-memory test implementations.

**Provides:**
- `IOutboxWriter` / `IOutboxReader` — Write and read outbox messages
- `IDeadLetterWriter` — Move unprocessable messages to dead-letter store
- `InMemoryOutboxStore` — In-process implementation (tests / single-process hosts)
- `AddOntogonyPersistencePrimitives()` — Register `IClock` and `IIdGenerator`
- `AddOntogonyInMemoryOutboxStore()` — Register in-memory outbox for testing

**When to use:** Services that need durable event outbox or processed-message deduplication (transactional outbox pattern).

---

### `Ontogony.Persistence.Postgres`

**Purpose:** PostgreSQL outbox provider — durable outbox messages, processed messages, dead-letter, and claim leasing via Npgsql.

**Provides:**
- `PostgresOutboxOptions` — Connection string, table names, lease duration, schema init
- `AddOntogonyPostgresOutbox(opts => ...)` — Register Postgres outbox writer/reader/dead-letter writer
- `PostgresOutboxSchemaInitializer` — Create tables on startup when `EnsureSchemaOnStartup = true`
- Claim-lease writer for reliable at-least-once dispatch

**When to use:** Production services that need durable outbox with PostgreSQL.

**Example:**
```csharp
services.AddOntogonyPostgresOutbox(opts =>
{
    opts.ConnectionString = configuration.GetConnectionString("Outbox")!;
    opts.EnsureSchemaOnStartup = true;
    opts.ClaimLeaseDuration = TimeSpan.FromSeconds(30);
});
```

---

## Idempotency & Consistency

### `Ontogony.Idempotency`

**Purpose:** Idempotency key generation, at-most-once semantics, request deduplication.

**Provides:**
- `IdempotencyKeyBuilder` — Build stable fingerprint keys from operation name + parts
- `IIdempotencyLedger` — Track `TryBeginAsync` / `MarkSucceededAsync` / `MarkFailedAsync`
- `InMemoryIdempotencyLedger` — In-process implementation (tests / single-process hosts)

**When to use:** APIs that must be safely retried without duplication (payment APIs, order creation, etc.).

**Example:**
```csharp
// Build key
var key = keyBuilder.BuildKey("create-order", customerId, orderId);

// Check and claim
if (!await ledger.TryBeginAsync(key)) { return Conflict(); }
try { ... await ledger.MarkSucceededAsync(key); }
catch { await ledger.MarkFailedAsync(key); throw; }
```

---

## Quotas & resource control

### `Ontogony.Quotas`

**Purpose:** Mechanical quota scopes, windows, limits, consumption records, decisions, and an in-memory `IQuotaLedger` for tests and single-process hosts.

**Provides:** Provider-neutral numeric metrics (requests, tokens, cost units as opaque counters) — **no** distributed rate limiter, pricing, or routing policy.

**When to use:** Enforcing mechanical budgets next to idempotency or gateway code; product repos own plan tiers and durable ledgers.

**Non-goals:** No billing, pricing tables, or Conexus plan semantics (see [package notes](Ontogony.Quotas.md)).

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

## Dependency graph

Do not rely on an informal ASCII tree here: it drifts from the real graph. **Authoritative allowed edges** (every shipping `Ontogony.*` → `Ontogony.*` `ProjectReference`) live in:

- [`docs/architecture/package-levels.md`](../architecture/package-levels.md) — full matrix, forbidden edges, and documentation levels (0, 0.5, 1–3).
- [`scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1) — CI-enforced golden map.

---

## Quick Selection Matrix

| Use Case | Package | Alternative |
|----------|---------|-------------|
| Publish events | `Ontogony.Messaging` | `Ontogony.Contracts` (if hand-crafting) |
| Trace requests | `Ontogony.Observability` | — (required) |
| Structured log fields / scopes | `Ontogony.Logging` | Ad hoc string keys only |
| Redact sensitive values | `Ontogony.Redaction` | Custom per-field scrubbers |
| Secret refs / masks (no vault SDK) | `Ontogony.Secrets` | Host-specific secret stores |
| Mechanical quotas / in-memory ledger | `Ontogony.Quotas` | External rate-limit appliances only |
| Replay bundle DTOs (no engine) | `Ontogony.Replay.Contracts` | Product-specific debug formats |
| Eval run / baseline DTOs (no harness) | `Ontogony.Evaluation.Contracts` | Product-specific eval JSON only |
| Task classification / topology DTOs (no planner) | `Ontogony.Topology.Contracts` | Product-specific topology JSON only |
| Handle errors | `Ontogony.Errors` | — (required) |
| Call other services | `Ontogony.Http` | `HttpClient` (not recommended) |
| Authenticate | `Ontogony.Security` | — (required) |
| Store data | `Ontogony.Persistence.Postgres` | Any ORM |
| Idempotent APIs | `Ontogony.Idempotency` | Custom deduplication |
| Integration tests | `Ontogony.Testing` | Any test framework + helpers |
| LLM telemetry contracts | `Ontogony.AI.Contracts` | Product routing/orchestration repos |
| Large/sensitive payload by-reference | `Ontogony.Artifacts` | Inline payloads in envelopes (only for small DTOs) |
| Execution journal (runs / steps / attempts) | `Ontogony.Execution` | Product workflow engines or durable journal DBs |

---

## Installation

**GitHub Packages:** pushing a **version tag** (for example `v0.3.0`) runs [.github/workflows/release-packages.yml](../../.github/workflows/release-packages.yml), which packs and publishes to `https://nuget.pkg.github.com/<owner>/index.json`. See [Private NuGet feed](../adoption/private-nuget-feed.md).

Packages are also attached to **GitHub Releases** as downloadable `.nupkg` files. You can additionally mirror to Azure Artifacts or another NuGet-compatible registry with `dotnet nuget push` if your org requires it.

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
- **Run conformance tests:** [Testing conformance kits](../testing/conformance-kits.md)

---

**Last Updated:** May 2026  
**Version:** 0.3.0-alpha.1
