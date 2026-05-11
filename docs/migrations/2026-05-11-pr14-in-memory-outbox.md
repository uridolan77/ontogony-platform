# PR14: In-memory outbox reference (`InMemoryOutboxStore`)

## Summary

New types live in `Ontogony.Persistence`:

- `InMemoryOutboxStore` — reference implementation for `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher`, `IProcessedMessageStore`.
- `InMemoryOutboxStoreOptions` — optional dead-letter threshold.
- `DeadLetterMessage`, `IDeadLetterWriter`, `InMemoryDeadLetterWriter`.
- `AddOntogonyInMemoryOutboxStore` — registers one shared instance for all four outbox-related interfaces.

## Breaking changes

None to existing interface shapes in `OutboxContracts.cs`.

## Adoption

- Use for **tests and single-process** validation only unless you explicitly accept in-memory durability limits.
- For production durability, implement the same interfaces against your database or broker; follow `docs/persistence/outbox-contract.md`.
