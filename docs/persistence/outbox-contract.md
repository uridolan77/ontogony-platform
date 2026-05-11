# Outbox Contract

This document defines SQL-agnostic outbox contracts in `Ontogony.Persistence` and the semantics of the **in-memory reference implementation** (`InMemoryOutboxStore`).

## Purpose

Provide stable, mechanical contracts for durable event dispatch without introducing service-specific schemas or repositories.

## Interfaces

- **`IOutboxWriter`** — append new outbox rows.
- **`IOutboxReader`** — read rows that are eligible for dispatch at a point in time.
- **`IOutboxDispatcher`** — mark success (`MarkDispatchedAsync`) or failure (`MarkFailedAsync`) and schedule retries.
- **`IProcessedMessageStore`** — idempotent consumer deduplication by `consumerName` + `messageId` (see `OutboxContracts.BuildProcessedMessageKey`).
- **`IDeadLetterWriter`** — optional sink for messages moved to dead-letter state (storage is host-specific).

## Core record (`OutboxMessage`)

Mechanical fields (see `OutboxContracts.Validate`):

- `MessageId`, `EventId`, `EventType`, `Source`, `TraceId`, `OccurredAt`, `AvailableAt`
- `AttemptCount`, `LastError`
- `PayloadJson`, `PayloadHash`, `MetadataJson`

## In-memory reference semantics (`InMemoryOutboxStore`)

These rules are what Postgres (or other) providers should preserve unless a migration explicitly changes them.

### `WriteAsync`

- Validates via `OutboxContracts.Validate`.
- Rejects duplicate `MessageId` with `InvalidOperationException` (reference store is strict for tests).

### `ReadAvailableAsync(asOfUtc, maxBatchSize)`

- Returns messages that are **not** dispatched, **not** dead-lettered, and `AvailableAt <= asOfUtc`.
- **Ordering:** ascending by `AvailableAt`, then ascending by `OccurredAt` (stable tie-break).
- Respects `maxBatchSize` (non-negative); `0` returns an empty batch.

### `MarkDispatchedAsync(messageId, dispatchedAtUtc)`

- **Idempotent:** if the message is unknown, already dispatched, or dead-lettered, completes without error.
- Otherwise marks dispatched; subsequent reads exclude the message.

### `MarkFailedAsync(messageId, lastError, nextAvailableAtUtc)`

- **Attempt count:** increments `AttemptCount` by **one** on each successful failure mark (not on no-ops).
- Updates `LastError` and sets `AvailableAt` to `nextAvailableAtUtc` so the reader can reschedule.
- No-op if the message is unknown, already dispatched, or already dead-lettered.

### Dead-letter threshold (optional)

When `InMemoryOutboxStoreOptions.MoveToDeadLetterAfterAttempts` is set **and** an `IDeadLetterWriter` is supplied to the store:

- After `MarkFailedAsync` increments `AttemptCount`, if `AttemptCount >= MoveToDeadLetterAfterAttempts`, the store emits a `DeadLetterMessage` to the writer and marks the row **dead-lettered** (excluded from `ReadAvailableAsync` forever in this reference implementation).

**Product responsibility:** choosing the threshold, whether to dead-letter vs infinite retry, and how `IDeadLetterWriter` maps to durable storage. This repo documents the hook only.

### `IProcessedMessageStore`

- Keys are `consumerName` + `messageId` via `OutboxContracts.BuildProcessedMessageKey`.
- `MarkProcessedAsync` is **add-only** (duplicate marks are ignored via `TryAdd` semantics).

## Retry spacing

Mechanical backoff for suggested `nextAvailableAtUtc` values remains `OutboxContracts.CalculateNextAvailableAt` (deterministic, capped exponential).

## Constraints

- Contracts are persistence-provider neutral.
- No service/domain semantics are part of these contracts.
- No generic repository abstractions are required.

## Notes

Provider-specific implementations (for example PostgreSQL) should live in dedicated packages and map these contracts to storage models.

`Ontogony.Messaging` defines a separate, minimal `IOutboxStore` for in-process scenarios; the **canonical** durable outbox contract for shared platform work is this persistence surface.
