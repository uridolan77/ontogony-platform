# What improved after PR15

## 1. Envelope validation is now real

You now have:

```text
IEnvelopeValidator
EnvelopeValidationResult
EnvelopeValidationError
DefaultEnvelopeValidator
EnvelopeValidatorOptions
```

The default validator checks required mechanical fields, event type format, source format, trace ID format, schema version, optional payload hash, and optional protocol allowlist.   

This is a big step. It means Athanor can ingest AG-UI/MCP/A2A events through a stable mechanical gate before attaching any domain meaning.

## 2. CloudEvents bridge is much stronger

`CloudEventsExtensions` now includes:

```text
CloudEventTraceIdPolicy
CloudEventConversionOptions
specversion 1.0 validation
schemaVersion extension
protocol extension preservation
traceId generate/reject policy
JsonElement extension handling
```

That is exactly the right direction for protocol ingestion. 

## 3. Service identity moved from “helper” toward serious infrastructure

`ServiceIdentityCurrentActorAccessor` now supports HMAC-SHA256 mode, timestamp validation, nonce requirement, body hash validation, service secret resolver, nonce replay store, and request body hash provider. 

`ServiceIdentityOptions` clearly distinguishes static shared-secret mode from real HMAC mode. 

The HMAC helper has a clean canonical string and base64 signature generation path. 

This is a serious improvement.

## 4. Messaging semantics are now explicit

`InMemoryEventPublisher` now has `PublishWithResultAsync`, handler timing, failure collection, per-handler dispatch results, publish/dispatch metrics, and operation modes. 

The result records are clean:

```text
EventHandlerDispatchResult
EventDispatchFailure
EventPublishResult
```



The operation modes are also clear:

```text
PublishAndDispatch
PublishOnly
CaptureOnly
```



This solves the previous ambiguity around “published” versus “handled.”

## 5. In-memory outbox reference implementation landed

`InMemoryOutboxStore` now implements writer, reader, dispatcher, and processed-message store contracts. It documents itself as thread-safe, in-memory, single-process, not durable/multi-node. 

Dead-letter contracts also exist. 

This is exactly the right step before Postgres.

---

# Main issues to fix before adoption

## P0 — README is now stale and misleading

The README still says:

> “Starter package. The code is intentionally compact and extraction-ready.”

That is no longer true. 

The repo is now a shared infrastructure alpha with HMAC service identity, outbox reference implementation, envelope validation, messaging semantics, testing helpers, docs, and migrations.

Fix the README immediately. It should say something like:

```text
Current status: shared infrastructure alpha after PR15.

Ready for controlled adoption:
- Ontogony.Primitives
- Ontogony.Hashing
- Ontogony.Idempotency
- Ontogony.Contracts
- Ontogony.Observability
- Ontogony.Http
- Ontogony.Errors

Use carefully / not distributed-production complete:
- Ontogony.Messaging in-memory publisher
- Ontogony.Persistence in-memory outbox
- Ontogony.Security HMAC mode until body-hash/nonce-store concerns are hardened
```

This matters because future coding agents will read the README before deeper docs.

---

## P0 — HMAC request-body hashing is synchronous and unbounded

`Sha256RequestBodyHashProvider` calls:

```csharp
request.EnableBuffering();
body.CopyTo(ms);
var bytes = ms.ToArray();
```

inside a synchronous method. 

Problems:

```text
1. It synchronously reads the request body.
2. It copies the whole body into memory.
3. There is no max body-size guard.
4. It lives under CurrentActor access, which is usually expected to be cheap.
```

This is the most important technical issue after PR15.

Recommended fix:

```text
PR16 — Service identity request-body hashing hardening
```

Options:

```csharp
public int MaxSignedBodyBytes { get; set; } = 1_000_000;
public bool AllowUnsignedEmptyBody { get; set; } = true;
```

And change the model to one of these:

1. **Preferred:** service identity verification middleware runs async before `ICurrentActorAccessor`.
2. **Acceptable:** `IRequestBodyHashProvider` gets async API.
3. **Minimal:** refuse body hashing when content length exceeds limit and document it.

Do not leave production HMAC path reading arbitrary request bodies synchronously.

---

## P1 — In-memory nonce replay store is unbounded

`InMemoryNonceReplayStore` stores all seen nonces forever in a `HashSet<string>`. It is documented as process-local and not suitable for multi-node clusters, which is good. 

But even for single-process hosts, it should have TTL cleanup or max-count protection.

Add:

```csharp
public TimeSpan NonceRetention { get; set; } = TimeSpan.FromMinutes(10);
public int MaxStoredNonces { get; set; } = 100_000;
```

Or make it even clearer:

```text
InMemoryNonceReplayStore is test-only.
Production services must provide distributed INonceReplayStore.
```

Given your “perfect and crystal clear” goal, I would do both.

---

## P1 — Envelope validator source rule text is inconsistent

`DefaultEnvelopeValidator` error text says:

> “Source must be an absolute URI or service-like URI (scheme://authority or path).”

But implementation only does:

```csharp
Uri.TryCreate(source, UriKind.Absolute, out _)
```



So “path” or service-like non-URI strings may fail.

Choose one:

### Option A — strict and clean

Require absolute URI only.

Change message/docs/schema to:

```text
Source must be an absolute URI.
```

### Option B — service-like source support

Actually allow service-like patterns:

```text
agentor://runtime
athanor://ingestion
conexus://gateway
service:agentor
```

I recommend **Option A for now**. Strict is better for ingestion infrastructure.

---

## P1 — CloudEvents payload null path needs clearer failure behavior

`ToOntogonyEnvelope<TPayload>` deserializes `cloudEvent.Data` into `TPayload`, but if data is missing/null and `TPayload` is non-nullable or a value type, failure behavior will be runtime-cast/deserialization dependent. 

Add an explicit check:

```csharp
if (cloudEvent.Data is null)
    throw new InvalidOperationException("CloudEvent data is required for OntogonyEnvelope payload.");
```

or allow null only via option:

```csharp
AllowNullData = false
```

---

## P1 — In-memory outbox uses wall-clock time for dead-letter timestamp

`InMemoryOutboxStore.MarkFailedAsync` uses:

```csharp
var deadLetterAt = DateTimeOffset.UtcNow;
```



Since the repo already has `IClock`, use it. The in-memory outbox is a reference implementation and should be deterministic in tests.

---

## P2 — `IEventPublisher` interface does not expose publish result

`IEventPublisher` still only exposes:

```csharp
Task PublishAsync<TPayload>(...)
```



But the concrete `InMemoryEventPublisher` has `PublishWithResultAsync`. 

That is okay if `IEventPublisher` is intentionally minimal. But if services will depend on result semantics, add a separate interface:

```csharp
public interface IEventPublisherWithResult
{
    Task<EventPublishResult> PublishWithResultAsync<TPayload>(...);
}
```

Do not force all publishers to support rich dispatch results, because Kafka/NATS/EventHubs may not have equivalent semantics.

---

# Updated readiness matrix

| Package                  |                   Readiness | Notes                                                                    |
| ------------------------ | --------------------------: | ------------------------------------------------------------------------ |
| `Ontogony.Primitives`    |                     ✅ Ready | Safe to adopt now                                                        |
| `Ontogony.Configuration` |                     ✅ Ready | Stable mechanics                                                         |
| `Ontogony.Hashing`       |                     ✅ Ready | Good for Athanor/Agentor                                                 |
| `Ontogony.Idempotency`   |                     ✅ Ready | Strong enough now                                                        |
| `Ontogony.Contracts`     |              ✅ Almost ready | Fix source rule / null payload behavior                                  |
| `Ontogony.Observability` |                     ✅ Ready | Suitable for Agentor adoption                                            |
| `Ontogony.Http`          |                     ✅ Ready | Good transport mechanics                                                 |
| `Ontogony.Errors`        |                     ✅ Ready | Suitable for controlled adoption                                         |
| `Ontogony.Messaging`     |       ⚠️ Internal/reference | Good in-memory semantics; not distributed messaging                      |
| `Ontogony.Persistence`   |           ⚠️ Reference only | In-memory outbox is test/single-process only                             |
| `Ontogony.Security`      | ⚠️ Good but needs hardening | HMAC exists, but body hashing and nonce store need production guardrails |
| `Ontogony.Testing`       |                     ✅ Ready | Useful for adoption PRs                                                  |

---

# Recommended next PRs

## PR16 — Security HMAC hardening

Scope:

```text
Ontogony.Security
ServiceIdentityCurrentActorAccessor
Sha256RequestBodyHashProvider
InMemoryNonceReplayStore
docs/security/service-identity.md
```

Tasks:

```text
1. Add max signed body size.
2. Avoid synchronous full-body copy for large requests.
3. Introduce async verification path or middleware-based verifier.
4. Add nonce TTL/max-count cleanup.
5. Clarify production requirement: distributed nonce replay store.
6. Add tests for oversized body, replay expiry, missing body hash, empty body.
```

This is the highest-value hardening PR.

## PR17 — Contracts polish and schema alignment

Scope:

```text
DefaultEnvelopeValidator
CloudEventsExtensions
schemas/ontogony-envelope.schema.json
docs/contracts/schema-versioning.md
```

Tasks:

```text
1. Make Source rule strict and consistent.
2. Add explicit CloudEvent data null behavior.
3. Add validator tests for all schema constraints.
4. Add optional strict protocol allowlist example for AG-UI/MCP/A2A.
```

## PR18 — Adoption pilot: Athanor primitives/hash/contracts

First real consumer adoption.

Scope in Athanor:

```text
Ontogony.Primitives
Ontogony.Hashing
Ontogony.Idempotency
Ontogony.Contracts only if no domain collision
```

Goal:

```text
No behavior change.
Existing hashes unchanged or differences explicitly documented.
No Athanor domain logic moves into platform.
```

## PR19 — Adoption pilot: Agentor observability/http

Scope in Agentor:

```text
Ontogony.Observability
Ontogony.Http
possibly Ontogony.Errors
```

Goal:

```text
X-Ontogony-Trace-Id is canonical on the wire; legacy response echoes are opt-in (EchoLegacyHeaders).
Inbound legacy headers remain accepted for correlation during rollout.
Outbound Athanor/Conexus calls propagate correlation.
No Agentor run semantics move into platform.
```

---

# Bottom line

After PR15, the platform is strong. It now has enough maturity to stop inventing more building blocks and start proving itself through **controlled adoption**.

But before adopting security-sensitive pieces, fix HMAC body hashing and nonce replay semantics.

My recommendation:

```text
1. PR16 — HMAC/body/nonce hardening
2. PR17 — envelope/schema polish
3. Athanor adoption: Primitives + Hashing + Idempotency
4. Agentor adoption: Observability + Http
```

That will keep the platform robust, precise, and grounded in real service usage.

---

# Adoption arc PR21–PR24 (post-PR20 status)

This section tracks **cross-repo adoption** work that landed after the PR15-era notes above.

| Track | Status | Notes |
|-------|--------|--------|
| **PR21 — Hashing** | Stage 1 done | Athanor delegates UTF-8 SHA-256 to `Ontogony.Hashing`; canonical JSON stays Athanor-owned until JSON parity is proven. Migration: `docs/migrations/2026-05-11-pr21-athanor-hashing-stage1.md`. |
| **PR22 — Packaging** | Tightened | Shared NuGet metadata in `Directory.Build.props`; hardened `scripts/pack-all.ps1`; adoption index `docs/adoption/consumer-package-migration.md`. |
| **PR23 — CI templates** | Reference samples | `workflow_dispatch`-only samples under `.github/workflows/samples/` for multi-checkout vs internal feed; linked from `docs/adoption/local-repo-layout-and-ci.md`. |
| **PR24 — Athanor observability** | Host wiring done | Tracing + errors middleware in Athanor production paths (see Athanor `docs/engineering/PR20-athanor-ontogony-errors-tracing.md`). Remaining work is **ops validation** (dashboards, alerts, correlation burn-in), not re-implementing ASP.NET middleware here. |
