# PR27 — Ontogony.Persistence.Postgres outbox provider

## Summary

Adds `Ontogony.Persistence.Postgres`, a durable PostgreSQL provider for outbox and processed-message contracts.

## Consumer actions

### Add package

- Add reference to `Ontogony.Persistence.Postgres`.
- Configure and register with `AddOntogonyPostgresOutbox(...)`.

### Create schema

- Ensure provider schema is applied by calling `EnsureSchemaAsync()` from startup/migration flow.
- The provider owns these mechanical tables:
  - `ontogony_outbox_messages`
  - `ontogony_processed_messages`
  - `ontogony_dead_letter_messages`

### Runtime behavior to note

- `ReadAvailableAsync` now performs an atomic claim lease in PostgreSQL (`FOR UPDATE SKIP LOCKED`) to prevent double-claim across concurrent readers.
- Provider-specific lease controls are available through `IPostgresOutboxClaimStore` (`ClaimAvailableAsync`, `TryClaimAsync`, `RenewClaimAsync`, `ReleaseClaimAsync`).
- `MarkDispatchedAsync` is idempotent.
- `MarkProcessedAsync` is idempotent by key (`consumer_name`, `message_id`).
- Optional dead-letter threshold is configured via `MoveToDeadLetterAfterAttempts`.
- Optional startup schema bootstrap is configured via `EnsureSchemaOnStartup`.

## Repos unchanged

No Athanor, Agentor, or Conexus domain rules were added.
