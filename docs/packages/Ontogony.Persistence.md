# Ontogony.Persistence — semantic contract

**Status:** Production intent for **mechanical** persistence helpers and contracts. Concrete database behavior ships per hosting service; read contract docs before relying on outbox semantics.

## Guarantees

- Documented **outbox contract** (mechanical fields and lifecycle expectations) in `docs/persistence/`.

## Does not guarantee

- A specific RDBMS schema in this repo.
- Product-specific transaction boundaries.

## Related

- [../persistence/outbox-contract.md](../persistence/outbox-contract.md)
- [../persistence/idempotent-consumer.md](../persistence/idempotent-consumer.md)
