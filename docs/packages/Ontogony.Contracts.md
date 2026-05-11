# Ontogony.Contracts — semantic contract

**Status:** Production-safe for DTOs and header names. Breaking changes follow semver and migration notes.

## Guarantees

- Stable **protocol-neutral** shapes for correlation, events, and shared identifiers.
- Header name constants (`OntogonyEventHeaders`, etc.) remain stable across minor versions unless documented otherwise.

## Does not guarantee

- Validation of business payloads.
- Authentication or trust of header values (see `Ontogony.Security`).

## Related

- [../invariants.md](../invariants.md) — envelope required fields when validation is enabled.
- [../04_EVENT_ENVELOPE_STANDARD.md](../04_EVENT_ENVELOPE_STANDARD.md)
