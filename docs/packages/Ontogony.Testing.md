# Ontogony.Testing — semantic contract

**Status:** **Test-only and local diagnostics.** Not a substitute for production middleware, brokers, or persistence.

## Guarantees

- **`TestEnvelopeFactory`**, **`EnvelopeFixtureBuilder`**, **`PublishedEventRecorder`**, and related helpers produce mechanically valid sample envelopes when using current defaults.
- Assertions such as **`EnvelopeAssertions`** encode shared test expectations for correlation and required fields.

## Does not guarantee

- Security, durability, or fidelity to production ingress behavior.
- That `PublishedEventRecorder` or in-memory sinks represent delivery or outbox state.

## Architecture helpers

`Ontogony.Testing.Architecture` provides reusable forbidden-dependency scanners for product test projects:

- MSBuild `PackageReference`, `ProjectReference`, and `PackageVersion` (including multiline `Include`)
- `using`, `global using`, `using static`, and alias directives

See [../adoption/architecture-tests-adoption.md](../adoption/architecture-tests-adoption.md).

## Related

- [../packages/Ontogony.Messaging.md](./Ontogony.Messaging.md)
- [../packages/Ontogony.Contracts.md](./Ontogony.Contracts.md)
- [../adoption/architecture-tests-adoption.md](../adoption/architecture-tests-adoption.md)
