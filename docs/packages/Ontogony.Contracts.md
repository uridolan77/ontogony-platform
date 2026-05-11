# Ontogony.Contracts — semantic contract

**Status:** Production-safe for DTOs and header names. Breaking changes follow semver and migration notes.

## Guarantees

- Stable **protocol-neutral** shapes for correlation, events, and shared identifiers.
- Header name constants (`OntogonyEventHeaders`, etc.) remain stable across minor versions unless documented otherwise.
- Mechanical envelope validation via `IEnvelopeValidator` / `DefaultEnvelopeValidator`, JSON schema (`schemas/ontogony-envelope.schema.json`), and stricter CloudEvents conversion options (`CloudEventConversionOptions`).

## Does not guarantee

- Validation of business payloads.
- Authentication or trust of header values (see `Ontogony.Security`).

## Related

- [../invariants.md](../invariants.md) — envelope fields and validation layers.
- [../contracts/schema-versioning.md](../contracts/schema-versioning.md) — schema and `SchemaVersion` semantics.
- [../04_EVENT_ENVELOPE_STANDARD.md](../04_EVENT_ENVELOPE_STANDARD.md)
