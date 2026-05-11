# Ontogony.Primitives — semantic contract

**Status:** Production-safe for small abstractions (`IClock`, `IIdGenerator`) used across packages.

## Guarantees

- **Time:** `IClock` / `SystemClock` provide testable UTC access without hidden globals.
- **Identifiers:** `IIdGenerator` / `GuidIdGenerator` supply opaque unique ids without embedding product meaning.

## Does not guarantee

- Distributed uniqueness across hosts without coordination.
- Cryptographic strength beyond what `Guid` provides.

## Related

- [../invariants.md](../invariants.md)
