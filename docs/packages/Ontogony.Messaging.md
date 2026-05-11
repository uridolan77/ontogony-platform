# Ontogony.Messaging — semantic contract

**Status:** Production-safe for serialization and envelope helpers. In-memory publisher/sink are **test and single-process diagnostics** unless you explicitly accept their semantics.

## Guarantees

- `OntogonyEnvelope<TPayload>` flows through `IEventPublisher` without adding domain meaning.
- Optional validation of **mechanical** required envelope fields.
- Deterministic optional payload hashing when enabled (via `Ontogony.Hashing`).

## Does not guarantee

- Durable publish, at-least-once delivery, or consumer offset management.
- That `InMemoryEventSink` reflects handler success; it captures **published** envelopes only.

## Related

- [../messaging/delivery-semantics.md](../messaging/delivery-semantics.md)
- [../invariants.md](../invariants.md)
