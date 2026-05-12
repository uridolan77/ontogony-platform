# Migration: PR41 — Remove duplicate outbox types from Ontogony.Messaging

## Summary

`Ontogony.Messaging` no longer defines `IOutboxStore`, `OutboxMessage`, or `NoOpOutboxStore`. The **only** shared outbox row shape and ports are in **`Ontogony.Persistence`** (`OutboxMessage`, `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher`).

## Who is affected

Any code that referenced `Ontogony.Messaging.IOutboxStore` or `Ontogony.Messaging.OutboxMessage` (this surface was redundant and differed from the persistence contract).

## What to do

1. Add a project reference to `Ontogony.Persistence` (and optionally `Ontogony.Persistence.Postgres` for production).
2. Replace `IOutboxStore` usage with `IOutboxWriter` / `IOutboxReader` / `IOutboxDispatcher` as appropriate, and map call sites to the persistence `OutboxMessage` constructor (see `OutboxContracts.Validate` and `docs/persistence/outbox-contract.md`).
3. For tests or single-process hosts, register `AddOntogonyInMemoryOutboxStore()` from `Ontogony.Persistence` instead of any messaging-local outbox stub.

## Rationale

One mechanical outbox model avoids competing types and keeps **messaging** limited to publish/dispatch mechanics while **persistence** owns durable outbox and processed-message behavior.
