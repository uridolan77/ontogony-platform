# Ontogony.Idempotency — semantic contract

**Status:** Production-safe for key construction and in-memory test ledgers; durable ledgers live in hosting services.

## Guarantees

- **`IdempotencyKeyBuilder`** produces deterministic keys from canonical JSON of operation inputs (see XML on type).
- **`InMemoryIdempotencyLedger`** (and similar) provide **test and single-process** deduplication semantics.

## Does not guarantee

- Cross-process or cross-host deduplication for in-memory implementations.
- Business-level idempotency rules (which operations are safe to repeat).

## Related

- [../invariants.md](../invariants.md)
- [../packages/Ontogony.Hashing.md](./Ontogony.Hashing.md)
