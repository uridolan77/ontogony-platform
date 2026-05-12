# Ontogony.Messaging

In-process **event publishing** and **handler dispatch** for `OntogonyEnvelope<TPayload>` (no durable outbox here).

## What this is

- `IEventPublisher` / `IEventPublisherWithResult` — publish envelopes with optional mechanical validation and payload hashing.
- `IEventHandler<TPayload>` — register in-process consumers.
- `InMemoryEventPublisher`, `InMemoryEventBus`, `InMemoryEventSink` — **tests and single-process** diagnostics (not a broker).

## What this is not

- Not transactional outbox rows, claim leasing, or processed-message stores (see **`Ontogony.Persistence`** and **`Ontogony.Persistence.Postgres`**).
- Not domain event semantics or workflow orchestration.

## See also

- `docs/packages/Ontogony.Messaging.md`, `docs/messaging/delivery-semantics.md`.
