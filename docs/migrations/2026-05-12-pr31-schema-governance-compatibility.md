# Migration Note — PR31 Schema Governance and Compatibility

Date: 2026-05-12
PR: PR31

## Summary

PR31 establishes framework and tests for governing breaking changes to wire contracts, headers, and envelope validation logic. It introduces:

- JSON schema validation tests against golden fixture envelopes
- CloudEvents round-trip compatibility tests
- Header constant snapshot tests to catch accidental modifications
- Formalized breaking-change checklist and compatibility policy
- Documentation on header stability across service boundaries

No runtime API changes or behavior changes were introduced.

## What was added

### Tests

- `tests/Ontogony.Infrastructure.Tests/SchemaFixtureValidationTests.cs` — validates fixture envelopes against JSON schema
- `tests/Ontogony.Infrastructure.Tests/CloudEventsRoundTripTests.cs` — verifies all envelope fields survive CloudEvents conversion
- `tests/Ontogony.Infrastructure.Tests/HeaderConstantsSnapshotTests.cs` — snapshot tests for stable header and protocol constants

### Test fixtures

- `schemas/fixtures/valid/minimal-envelope.json` — minimal valid envelope
- `schemas/fixtures/valid/full-envelope.json` — all optional fields populated
- `schemas/fixtures/valid/ag-ui-protocol.json` — ag-ui protocol example
- `schemas/fixtures/valid/cloudevents-compat.json` — CloudEvents protocol example
- `schemas/fixtures/invalid/missing-eventid.json` — missing required EventId
- `schemas/fixtures/invalid/bad-eventtype-format.json` — invalid EventType pattern
- `schemas/fixtures/invalid/relative-source-uri.json` — Source must be absolute URI
- `schemas/fixtures/invalid/bad-occurred-at.json` — invalid datetime format
- `schemas/fixtures/invalid/additional-properties.json` — schema forbids unknown fields

### Documentation

- `docs/contracts/compatibility-policy.md` — breaking-change definition, testing requirements, PR checklist
- `docs/contracts/header-compatibility-matrix.md` — cross-service header usage, deprecation timeline, adoption guidance

## Dependencies added

- `JsonSchema.Net` (v7.1.0) for JSON schema validation in tests

## Required action for consumers

### Athanor

No immediate action required. Future PRs modifying Athanor's use of Ontogony headers or envelope payloads should:
- Update relevant fixture examples if new optional fields are added
- Run schema validation tests to confirm compatibility
- Document any changes in consuming service migrations

### Agentor

No immediate action required. Same guidance as Athanor.

### Conexus

No immediate action required. Same guidance as Athanor.

### Future services

Adopt the header compatibility matrix and breaking-change checklist when integrating with Ontogony infrastructure.

## Breaking change assessment

- Breaking changes to wire contracts: none in PR31
- API changes: none
- Behavior changes: none

This PR establishes governance for *future* breaking changes, not a breaking change itself.

## How to use the compatibility framework

When a future PR needs to modify:

1. **Envelope fields or validation**: Update fixture envelopes in `schemas/fixtures/`, run `SchemaFixtureValidationTests`, and update `schemas/ontogony-envelope.schema.json`.
2. **Header constants**: Update `HeaderConstantsSnapshotTests` and fill out the breaking-change checklist.
3. **CloudEvents conversion**: Add round-trip tests to `CloudEventsRoundTripTests` and verify backward/forward compatibility.
4. **Header usage**: Consult `docs/contracts/header-compatibility-matrix.md` and document any service-specific adoption steps.

All PRs modifying contracts must include:
- Fixture updates or new fixtures
- Test additions or updates
- CHANGELOG entry
- Migration note (if breaking) or "non-breaking" statement
- Breaking-change checklist (filled or marked N/A)
