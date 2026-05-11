# Platform invariants (mechanical contracts)

This document states **what the shared libraries guarantee** so product repos can rely on stable behavior. It does not define business rules.

## Event envelope (`Ontogony.Contracts`)

When validation is enabled (`Ontogony.Messaging` `EventDispatchOptions.ValidateRequiredEnvelopeFields`), the following fields are **required** on `OntogonyEnvelope<TPayload>`:

- `EventId`, `EventType`, `Source`, `OccurredAt`
- `TraceId`, `Protocol`
- `Payload` (non-null)

For **strict mechanical** ingress (recorders, external bridges), use `IEnvelopeValidator` / `DefaultEnvelopeValidator` in `Ontogony.Contracts` and see `schemas/ontogony-envelope.schema.json` plus [contracts/schema-versioning.md](./contracts/schema-versioning.md).

Tenant, workspace, project, actor, and session identifiers are **optional** unless your ingress layer enforces them.

See also: [04_EVENT_ENVELOPE_STANDARD.md](./04_EVENT_ENVELOPE_STANDARD.md).

## Idempotency (`Ontogony.Idempotency`)

- An idempotency key identifies a **single logical operation** from the caller’s perspective.
- Ledger entries are **mechanical** (accepted / duplicate / conflict); they do not interpret payload meaning.
- Retention and cleanup policies are owned by the hosting service and storage implementation.

## Retries and HTTP resilience (`Ontogony.Http`)

- Delegating handlers apply **transport-level** policies (timeouts, retries where configured). They do not infer idempotency of HTTP verbs for you.
- Safe retry semantics for a given API remain a **product concern**; this repo supplies hooks and classification helpers only.

## Outbox and dispatch (`Ontogony.Persistence` / docs)

Exact dispatch ordering, leases, and idempotency of `MarkDispatched` for **relational** implementations are specified in:

- [persistence/outbox-contract.md](./persistence/outbox-contract.md) (includes `InMemoryOutboxStore` reference semantics: read ordering, `MarkFailed` attempt increments, optional dead-letter threshold).

Until a database-backed implementation ships in this repo, use the contract plus the in-memory store for mechanical integration tests.

## Actor and role separation (`Ontogony.Security`)

- **Service identity** (`ServiceIdentityCurrentActorAccessor`) derives the actor from service identity headers and optional signature verification. It is meant for **service-to-service** calls.
- **Header actor mode** (`HeaderCurrentActorAccessor`) reads actor id, roles, and type from HTTP headers. It assumes a **trusted upstream** has already authenticated the caller; it performs no cryptographic proof.
- **Claims mode** maps OIDC / JWT claims to `CurrentActor` using configured claim types.

Do not mix modes on the same endpoint without an explicit security design.

## Messaging: publish vs capture vs dispatch

See [messaging/delivery-semantics.md](./messaging/delivery-semantics.md).

- **Publish** (in memory): acceptance of an envelope into the publisher pipeline (including append to an in-memory capture sink when enabled).
- **Capture**: the in-memory sink retains a copy of what was published; it is **not** a delivery ledger and not an outbox.
- **Dispatch**: invocation of registered in-process handlers after capture.
