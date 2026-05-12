# Ontogony.Persistence.Postgres — semantic contract

**Status:** Durable PostgreSQL provider for `Ontogony.Persistence` contracts.

## Guarantees

- PostgreSQL implementation of `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher`, and `IProcessedMessageStore`.
- Provider-specific claim controls via `IPostgresOutboxClaimStore` (`ClaimAvailableAsync`, `TryClaimAsync`, `RenewClaimAsync`, `ReleaseClaimAsync`, `MarkDispatchedIfOwnedAsync`, `MarkFailedIfOwnedAsync`).
- PostgreSQL dead-letter implementation for `IDeadLetterWriter`.
- Atomic claim semantics for available outbox rows with lease fields and `FOR UPDATE SKIP LOCKED`.
- Idempotent `MarkDispatchedAsync` and idempotent processed-message insert.
- Optional schema bootstrap on host startup via `PostgresOutboxOptions.EnsureSchemaOnStartup`.
- Mechanical schema creation for:
  - `ontogony_outbox_messages`
  - `ontogony_processed_messages`
  - `ontogony_dead_letter_messages`
- Atomic dead-letter persistence is guaranteed only when using `PostgresDeadLetterWriter`.

## Does not guarantee

- Product transaction boundaries.
- Product-specific retry policy decisions.
- Athanor, Agentor, or Conexus table semantics.

## Related

- [../persistence/outbox-contract.md](../persistence/outbox-contract.md)
- [../persistence/postgres-outbox-provider.md](../persistence/postgres-outbox-provider.md)
- [../migrations/2026-05-12-pr27-postgres-outbox-provider.md](../migrations/2026-05-12-pr27-postgres-outbox-provider.md)
- [../migrations/2026-05-12-pr27-5-postgres-provider-hardening.md](../migrations/2026-05-12-pr27-5-postgres-provider-hardening.md)
