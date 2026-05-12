# PostgreSQL Outbox Provider

`Ontogony.Persistence.Postgres` is the durable PostgreSQL provider for `Ontogony.Persistence` outbox contracts.

## Scope

This package provides mechanical storage behavior only:

- `PostgresOutboxStore` implements:
  - `IOutboxWriter`
  - `IOutboxReader`
  - `IOutboxDispatcher`
  - `IProcessedMessageStore`
- `PostgresDeadLetterWriter` implements `IDeadLetterWriter`.
- `AddOntogonyPostgresOutbox(...)` wires the provider into DI.
- `IPostgresOutboxClaimStore` exposes provider-specific claim lease controls.

No Athanor, Agentor, or Conexus semantics are encoded.

## Tables

The provider creates and uses three tables:

- `ontogony_outbox_messages`
- `ontogony_processed_messages`
- `ontogony_dead_letter_messages`

Schema is created idempotently by `EnsureSchemaAsync()`.

## Claim and Dispatch Semantics

`ReadAvailableAsync(asOfUtc, maxBatchSize)` performs an atomic claim step:

- rows must be `available_at <= asOfUtc`
- rows must not be dispatched or dead-lettered
- rows are sorted by `available_at`, then `occurred_at`
- selected rows are claimed using a lease (`claimed_by`, `claimed_until_utc`)
- SQL uses `FOR UPDATE SKIP LOCKED` to avoid double-claim in concurrent readers

`MarkDispatchedAsync` is idempotent. Unknown, already-dispatched, or dead-lettered rows are no-ops.

`MarkDispatchedIfOwnedAsync` is provider-specific and succeeds only when the current worker owns the active claim lease.

`MarkFailedAsync`:

- increments attempt count
- updates `last_error`
- schedules next retry via `available_at`
- optionally dead-letters when `MoveToDeadLetterAfterAttempts` threshold is reached

`MarkFailedIfOwnedAsync` is provider-specific and applies the same failure mechanics only when the current worker owns the active claim lease.

When dead-lettering and `IDeadLetterWriter` is `PostgresDeadLetterWriter`, outbox row update and dead-letter insert are executed in the same PostgreSQL transaction.

When a non-Postgres `IDeadLetterWriter` is used, the outbox row update commits first and the external dead-letter write occurs afterward. If the external write fails, the row may already be marked dead-lettered and excluded from reads.

## Configuration

```csharp
services.AddOntogonyPostgresOutbox(options =>
{
    options.ConnectionString = "Host=localhost;Port=5432;Database=ontogony;Username=postgres;Password=postgres";
    options.SchemaName = "public";
    options.OutboxTableName = "ontogony_outbox_messages";
    options.ProcessedTableName = "ontogony_processed_messages";
    options.DeadLetterTableName = "ontogony_dead_letter_messages";
    options.ClaimLeaseDuration = TimeSpan.FromSeconds(30);
    options.MoveToDeadLetterAfterAttempts = 10;
    options.EnsureSchemaOnStartup = true;
});
```

When `EnsureSchemaOnStartup` is true, the package registers a hosted service that calls `EnsureSchemaAsync()` during host startup.

## Explicit Claim API

`IPostgresOutboxClaimStore` adds explicit lease operations:

- `ClaimAvailableAsync(...)` to atomically claim a batch with optional lease duration override.
- `TryClaimAsync(...)` to claim a specific message when available and unclaimed/expired.
- `RenewClaimAsync(...)` to extend a claim lease owned by the current worker.
- `ReleaseClaimAsync(...)` to release ownership early.
- `MarkDispatchedIfOwnedAsync(...)` to mark dispatch only when current claim ownership is valid.
- `MarkFailedIfOwnedAsync(...)` to mark failure only when current claim ownership is valid.

`IOutboxReader.ReadAvailableAsync(...)` remains supported and delegates to `ClaimAvailableAsync(...)` with default lease duration.

`IOutboxDispatcher.MarkDispatchedAsync(...)` and `IOutboxDispatcher.MarkFailedAsync(...)` remain compatibility methods and do not require claim ownership.

## Local Docker Test Setup

A quick local database for integration tests:

```powershell
docker run --name ontogony-postgres-test -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ontogony -p 5432:5432 -d postgres:16
$env:ONTOGONY_POSTGRES_TEST_CONNECTION="Host=localhost;Port=5432;Database=ontogony;Username=postgres;Password=postgres"
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter PostgresOutboxStoreTests
```

If `ONTOGONY_POSTGRES_TEST_CONNECTION` is not set, PostgreSQL integration tests are skipped.

## Limitations

- The provider does not define product transaction boundaries.
- `WriteAsync` can participate in ambient transactions when the host uses transaction flow, but transaction orchestration remains host-owned.
- This package does not include EF Core mappings by design; it uses lightweight `Npgsql` commands.
- Claim lease timeout tuning (`ClaimLeaseDuration`) is host responsibility and should match worker heartbeat/dispatch timing.
