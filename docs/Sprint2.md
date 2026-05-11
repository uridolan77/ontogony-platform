I reviewed the repo state after PR10 using the latest head commit I could see, `4ac33a05...`. The repo is now in a much better place: PR7–PR10 added messaging, outbox contracts, testing utilities, adoption docs, and examples, and the changelog records **197 passing tests** after PR10. 

There was one connector weirdness: fetching by `main` briefly showed some stale files, but fetching by the latest commit SHA shows the PR6.1 corrections are actually present. For example, `TransportResilienceOptions` now includes `CountOnlyRetryableResponsesAsCircuitFailures`, unsafe-method idempotency requirements, max buffer size, and idempotency header name. 

## Current verdict

Score: **8.5 / 10**

This is no longer just a starter package. It is now a serious internal platform foundation.

The direction is right:

```text
Ontogony.Platform = shared mechanics
Athanor / Agentor / Conexus = product meaning
```

The platform now covers:

```text
contracts
observability
HTTP resilience
errors
hashing
idempotency
security primitives
messaging
outbox contracts
testing helpers
adoption docs
examples
```

That is a strong base.

But from now on the danger changes. The danger is no longer “not enough infrastructure.” The danger is **ambiguous infrastructure**: helpers that look production-grade but are actually only local/dev-grade, abstractions that look final but lack exact behavioral guarantees, and docs that say “do not do X” but code still makes X too easy.

So the next phase should be:

> No more feature accumulation. Harden semantics, contracts, invariants, and adoption paths.

---

# What looks good now

## 1. HTTP resilience is much better

The latest `TransportResilienceOptions` has the right shape:

```csharp
CountOnlyRetryableResponsesAsCircuitFailures
RetryUnsafeMethodsOnlyWithIdempotencyKey
MaxBufferedContentBytes
IdempotencyKeyHeaderName
```



This is exactly what we needed before Agentor/Athanor start relying on `Ontogony.Http`.

## 2. Security bugs from PR6 were corrected

`ServiceIdentityCurrentActorAccessor` now separates service actor ID from tenant/workspace/project fields, uses `OntogonyActorTypes.Service`, and compares signatures using constant-time equality. 

`HeaderCurrentActorAccessor` now uses `OntogonySecurityHeaders.Roles`, reads actor type from the actor-type header, and falls back to `OntogonyActorTypes.Service`. 

Good.

## 3. Messaging is useful but still cleanly modest

`InMemoryEventPublisher` now does envelope validation, optional payload hash computation, sink capture, handler dispatch, handler fan-out, and aggregate exception handling when continuing after handler failures. 

The options are simple and readable:

```csharp
ContinueOnHandlerException
ComputePayloadHash
ValidateRequiredEnvelopeFields
```



That is the right level for v1.

## 4. Outbox contracts are well scoped

The outbox package defines interfaces and neutral records, not EF entities or service-specific schemas:

```text
IOutboxWriter
IOutboxReader
IOutboxDispatcher
IProcessedMessageStore
IUnitOfWorkBoundary
OutboxMessage
ProcessedMessage
OutboxDispatchResult
```



That is exactly the right boundary. It is persistence mechanics, not Athanor/Agentor domain.

## 5. Adoption docs are steering agents correctly

The Agentor adoption doc explicitly says to adopt shared mechanics — request tracing, correlation propagation, span tags, metrics — while keeping Agentor-specific run/plan/tool semantics inside Agentor. 

That is the right discipline.

---

# Remaining risks

## Risk 1 — Service identity still looks more production-grade than it is

`ServiceIdentityCurrentActorAccessor` now uses constant-time equality, which is good, but it still compares a provided signature directly against a configured secret. The comment itself says production should use HMAC or another cryptographic verification strategy. 

So this must become crystal clear:

```text
Current service identity signature = shared-secret equality helper.
Not full production service authentication.
```

Next step: implement real HMAC mode, or rename the current mode to make its limitations unmistakable.

## Risk 2 — In-memory publisher records before handler success

`InMemoryEventPublisher` appends the event to the sink before dispatching handlers. 

That is fine if the sink means “published/attempted.” It is wrong if the sink means “successfully handled.”

Make this explicit in names/docs:

```text
InMemoryEventSink = published-event capture
not delivery-success ledger
```

Add a separate dispatch result ledger later if needed.

## Risk 3 — Outbox contracts are good, but delivery semantics are not yet exact

The outbox contract has `MarkDispatchedAsync`, `MarkFailedAsync`, `AttemptCount`, `AvailableAt`, `LastError`, and retry schedule calculation. 

But before implementing a real Postgres outbox, you need to define exact semantics:

```text
When is AttemptCount incremented?
Before handler? After failure?
Is dispatch idempotent?
What happens if publish succeeds but MarkDispatched fails?
What lock/lease model prevents double dispatch?
What is the dead-letter threshold?
```

Do not build a database implementation until this is written down.

---

# Recommended next phase: “hardening, not expansion”

I would do the next work as **5 robust PRs**, each comprehensive but not bloated.

## PR11 — Platform semantic contract audit

Goal: make every public abstraction impossible to misunderstand.

Scope:

```text
Ontogony.Http
Ontogony.Messaging
Ontogony.Persistence
Ontogony.Security
Ontogony.Contracts
docs/
```

Tasks:

```text
1. Add semantic docs to each major package:
   - what this package guarantees
   - what it does not guarantee
   - production-safe / test-only / alpha-only status

2. Rename or document ambiguous concepts:
   - InMemoryEventSink = published event capture, not delivery ledger
   - ServiceIdentity signature = shared-secret comparison unless HMAC mode enabled
   - Header actor mode = trusted-upstream only

3. Add invariants docs:
   - event envelope required fields
   - idempotency key guarantees
   - retry guarantees
   - outbox dispatch guarantees
   - actor/role separation

4. Add XML comments to important public types.
5. Keep CS1591 suppressed for now, but reduce missing docs on core public contracts.
```

Acceptance:

```text
No behavior change.
Docs and names remove ambiguity.
All tests pass.
```

This PR is not glamorous, but it is exactly how to keep the platform crystal clear.

---

## PR12 — Production-grade service identity mode

Goal: make `Ontogony.Security` honest and usable.

Scope:

```text
src/Ontogony.Security/
tests/Ontogony.Infrastructure.Tests/
docs/security/
```

Add real service-to-service HMAC support:

```text
X-Ontogony-Service-Id
X-Ontogony-Service-Timestamp
X-Ontogony-Service-Nonce
X-Ontogony-Service-Body-Hash
X-Ontogony-Service-Signature
```

Signature:

```text
HMACSHA256(secret, method + "\n" + path + "\n" + timestamp + "\n" + nonce + "\n" + bodyHash)
```

Add options:

```csharp
public bool RequireHmacSignature { get; set; }
public TimeSpan MaxTimestampSkew { get; set; } = TimeSpan.FromMinutes(5);
public bool RequireNonce { get; set; } = true;
```

Add interfaces:

```csharp
public interface IServiceSecretResolver
public interface INonceReplayStore
public interface IRequestBodyHashProvider
```

Keep the current simple shared-secret equality mode, but rename it:

```text
StaticSharedSecretMode
```

and document it as internal/dev only.

Acceptance:

```text
HMAC valid request succeeds.
Invalid signature fails.
Old timestamp fails.
Missing nonce fails when required.
Replay store can reject repeated nonce.
Constant-time comparison used.
```

This turns `Ontogony.Security` from “useful helper” into a serious foundation.

---

## PR13 — Messaging delivery semantics and diagnostics

Goal: clarify what “publish,” “capture,” “dispatch,” and “handled” mean.

Scope:

```text
src/Ontogony.Messaging/
src/Ontogony.Observability/
src/Ontogony.Testing/
tests/
docs/messaging/
```

Add:

```csharp
public sealed record EventPublishResult(...)
public sealed record EventHandlerDispatchResult(...)
public sealed record EventDispatchFailure(...)
```

Clarify modes:

```text
PublishOnly
PublishAndDispatch
CaptureOnly
```

Add metrics:

```text
ontogony.event.publish.count
ontogony.event.dispatch.count
ontogony.event.dispatch.failure.count
ontogony.event.handler.duration.ms
```

Add behavior:

```text
sink captures published attempts
dispatch result captures handler outcomes
exceptions can stop immediately or aggregate
cancellation behavior is explicit
```

Acceptance:

```text
A failed handler does not make the sink lie.
Dispatch result shows which handler failed.
Metrics record publish and dispatch separately.
Existing InMemoryEventBus compatibility remains.
```

This keeps messaging clean before connecting it to outbox.

---

## PR14 — Outbox semantics specification and in-memory reference implementation

Goal: define exact outbox behavior before Postgres.

Scope:

```text
src/Ontogony.Persistence/
tests/
docs/persistence/
```

Do **not** build Postgres yet.

Add an in-memory reference implementation:

```csharp
InMemoryOutboxStore :
    IOutboxWriter,
    IOutboxReader,
    IOutboxDispatcher,
    IProcessedMessageStore
```

Add explicit semantics:

```text
attempt count increments on MarkFailed
ReadAvailable returns messages ordered by AvailableAt then OccurredAt
MarkDispatched is idempotent
MarkFailed records error and next availability
ProcessedMessageStore uses consumerName + messageId
```

Add dead-letter contract, but not implementation-specific storage:

```csharp
public sealed record DeadLetterMessage(...)
public interface IDeadLetterWriter
```

Acceptance:

```text
Read available respects AvailableAt.
MarkDispatched removes/excludes from future batches.
MarkFailed reschedules.
Processed message store prevents duplicate processing.
Dead-letter threshold behavior documented.
```

This makes the later Postgres implementation much easier and safer.

---

## PR15 — Contract validation and schema discipline

Goal: make envelopes and CloudEvents conversion strict enough for external protocol recorders.

Scope:

```text
src/Ontogony.Contracts/
schemas/
tests/
docs/contracts/
```

Add:

```csharp
public interface IEnvelopeValidator
public sealed class EnvelopeValidationResult
public sealed class EnvelopeValidationError
```

Rules:

```text
EventId required and safe
EventType follows protocol.entity.verb
Source required and URI-like or service-like
TraceId required
Protocol required and known/allowed optionally
OccurredAt not default
SchemaVersion required
Payload not null
PayloadHash optional but validated if present
```

CloudEvents hardening:

```text
ToCloudEvent preserves protocol extension.
FromCloudEvent validates specversion.
JsonElement extension extraction tested.
Missing trace ID behavior explicit: generate or reject, based on option.
```

Add schema version doc:

```text
schemas/ontogony-envelope.schema.json
docs/contracts/schema-versioning.md
```

Acceptance:

```text
Invalid envelope fails with clear validation errors.
CloudEvents round-trip preserves protocol/trace/metadata/payloadHash.
Schema docs match runtime validator.
```

This is essential before Athanor ingests AG-UI/MCP/A2A events.

---

# After PR15: only then build implementation adapters

After those hardening PRs, the platform is ready for real integration.

Then do adoption in this order:

## Adoption A — Athanor minimal adoption

Start with the safest packages:

```text
Ontogony.Primitives
Ontogony.Hashing
Ontogony.Contracts
```

Do not start with messaging/outbox.

Goal:

```text
no behavior change
hashes unchanged
trace/canonical logic untouched
```

## Adoption B — Agentor observability + HTTP

Adopt:

```text
Ontogony.Observability
Ontogony.Http
Ontogony.Errors optionally
```

Goal:

```text
same public behavior
legacy Agentor trace headers preserved
outbound Athanor/Conexus calls use shared correlation
```

## Adoption C — Athanor protocol recorder MVP

Only after contracts/validation/messaging are solid:

```text
AG-UI event → OntogonyEnvelope → validate → hash → in-memory/event-store sink
```

---

# My recommended immediate next prompt

Use this for the next coding agent:

```text
Make PR11: Platform semantic contract audit.

Do not add major new runtime features.

Goal:
Make Ontogony.Platform crystal clear and safe to adopt.

Tasks:
1. Add docs/package-contracts/ with one contract file per package:
   - Ontogony.Contracts
   - Ontogony.Observability
   - Ontogony.Http
   - Ontogony.Errors
   - Ontogony.Hashing
   - Ontogony.Idempotency
   - Ontogony.Messaging
   - Ontogony.Persistence
   - Ontogony.Security
   - Ontogony.Testing

Each file must state:
   - What this package guarantees
   - What it explicitly does not guarantee
   - Production-safe vs test-only APIs
   - Known non-goals
   - Adoption status

2. Add or improve XML comments on the most important public types:
   - OntogonyEnvelope
   - CloudEventEnvelope
   - InMemoryEventPublisher
   - EventDispatchOptions
   - OutboxMessage
   - OutboxContracts
   - ServiceIdentityCurrentActorAccessor
   - HeaderCurrentActorAccessor
   - IdempotencyKeyBuilder
   - TransportResilienceOptions

3. Clarify in docs:
   - InMemoryEventSink captures published events, not successful delivery.
   - Header actor mode trusts upstream and is not authentication.
   - Current service identity signature mode is not full HMAC production auth.
   - Outbox contracts are contracts only, not a persistence implementation.
   - Messaging v1 is in-memory/reference mechanics, not distributed messaging.

4. Do not add domain logic.
5. Do not add Athanor/Agentor/Conexus-specific semantics.
6. Do not add database implementation.
7. Run:
   dotnet restore Ontogony.Platform.sln
   dotnet build Ontogony.Platform.sln --no-restore
   dotnet test Ontogony.Platform.sln --no-build

Acceptance:
The public API and docs should make it impossible for future agents to confuse test helpers, trusted-internal helpers, and production-safe mechanisms.
```

---

# Bottom line

The repo after PR10 is strong. It has moved from “starter zip” to “real internal platform.”

But now the correct move is **not more breadth**. It is precision:

```text
PR11 — semantic contract audit
PR12 — production-grade service identity
PR13 — messaging delivery semantics
PR14 — outbox reference implementation
PR15 — envelope validation and schema discipline
```

That sequence will keep the infrastructure robust, explicit, and clean enough to become the common substrate for Athanor, Agentor, Conexus, and eventually the AG-UI/MCP/A2A protocol recorder.
