# PR27 — Ontogony.Persistence.Postgres Outbox Provider

## Goal

Add a provider package implementing `Ontogony.Persistence` outbox contracts over PostgreSQL.

## Why

The current `Ontogony.Persistence` package has sound SQL-agnostic contracts and an in-memory reference store. The next infrastructure milestone is a real durable provider without moving product repositories into the platform.

## Scope

Add package:

```text
src/Ontogony.Persistence.Postgres/Ontogony.Persistence.Postgres.csproj
```

Implement:

```csharp
PostgresOutboxStore : IOutboxWriter, IOutboxReader, IOutboxDispatcher, IProcessedMessageStore
PostgresDeadLetterWriter : IDeadLetterWriter
AddOntogonyPostgresOutbox(...)
```

Schema:

```sql
ontogony_outbox_messages
ontogony_processed_messages
ontogony_dead_letter_messages
```

## Required mechanics

- Insert outbox row atomically when called by host transaction where possible.
- Read available messages with concurrency-safe claim semantics.
- Preserve contract ordering: `AvailableAt`, then `OccurredAt`.
- Support retries and dead-letter threshold.
- Idempotent `MarkDispatchedAsync`.
- Idempotent processed-message marking.
- Use `IClock` only where current time is needed.

## Open design decision

Choose one:

A. Reader returns unclaimed available messages and host claims separately.
B. Reader claims messages atomically.

Recommendation: add explicit claim model rather than overloading existing `ReadAvailableAsync`. Keep existing contracts, but add provider-specific batch claim API if needed.

## Must not do

- Do not define Agentor/Athanor tables.
- Do not define product transaction boundaries.
- Do not require EF Core if a lightweight Npgsql implementation is cleaner.

## Tests

Preferred: Testcontainers PostgreSQL integration tests.

Minimum:

- SQL schema applies cleanly.
- Write/read ordering.
- Duplicate message handling.
- Concurrent readers do not double-claim.
- Retry schedule updates.
- Dead-letter threshold moves message.
- Processed-message dedup works.

## Docs

- `docs/persistence/postgres-outbox-provider.md`
- schema migration file docs
- local Docker test instructions
- provider limitations

## Acceptance

The provider works independently of Agentor/Athanor and can be used by a sample worker.
