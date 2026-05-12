# PR27.5 — PostgreSQL provider hardening

## Summary

Stabilization pass for `Ontogony.Persistence.Postgres` after initial PR27 delivery.

## Changes

- Centralized PostgreSQL connection pool lifecycle:
  - `AddOntogonyPostgresOutbox(...)` now registers one shared `NpgsqlDataSource` singleton.
  - `PostgresOutboxStore` and `PostgresDeadLetterWriter` consume the shared data source from DI.
- Hardened schema/index naming for configurable table names:
  - outbox indexes now use table-derived generated names to avoid collisions across multiple configured outbox tables.
- Added ownership-enforced provider methods:
  - `MarkDispatchedIfOwnedAsync(...)`
  - `MarkFailedIfOwnedAsync(...)`
  - Existing `IOutboxDispatcher` methods remain compatibility methods and are intentionally looser.
- Clarified dead-letter behavior:
  - atomic outbox + dead-letter write is guaranteed only for `PostgresDeadLetterWriter`.
  - external `IDeadLetterWriter` may observe partial success if external write fails after outbox update commit.
- Updated README resilience note to reflect Retry-After and jitter support.

## Consumer actions

- No source break for existing `IOutboxDispatcher` usage.
- For strict worker ownership semantics, use `IPostgresOutboxClaimStore` owned methods.
- If using custom table names, no additional action is required; index names are now scoped.
