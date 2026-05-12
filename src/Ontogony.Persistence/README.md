# Ontogony.Persistence

SQL-agnostic **outbox**, **processed-message**, and related persistence **ports** (no ORM, no domain models).

## What this is

- `OutboxMessage` plus `IOutboxWriter` / `IOutboxReader` / `IOutboxDispatcher` — transactional outbox mechanics.
- `IProcessedMessageStore` — idempotent consumer dedupe keys.
- `InMemoryOutboxStore` — thread-safe reference implementation for **tests and single-process** hosts.
- `AddOntogonyPersistencePrimitives()`, `AddOntogonyInMemoryOutboxStore()` — DI helpers.

## What this is not

- Not event publishing or handler wiring (see **`Ontogony.Messaging`**).
- Not a database driver: use **`Ontogony.Persistence.Postgres`** (or your own provider) for durable storage.

## See also

- `docs/persistence/outbox-contract.md`, `docs/packages/Ontogony.Persistence.md`.
