# Ontogony Platform — Compatibility Policy

This document establishes how breaking changes to wire contracts and public APIs are managed.

## Core principle

> Envelopes and headers are shared infrastructure. Breaking changes must be documented, tested, and migrated carefully across all consuming services (Allagma, Kanon, Conexus, and others).

## What is a breaking change?

A **breaking change** is any modification that causes existing valid envelopes, headers, or payloads to be rejected or misinterpreted by consuming services.

Examples of breaking changes:

- Tightening `EventType` pattern (e.g., splitting into more segments)
- Adding new required fields to the envelope
- Removing or renaming existing required fields
- Changing the format or validation rules for `TraceId`, `Source`, or `Protocol`
- Modifying the semantics of header constants (e.g., renaming `X-Ontogony-Trace-Id`)
- Altering CloudEvents round-trip field mappings
- Changing `SchemaVersion` default or adding version-gated validation

Non-breaking examples (no action needed):

- Adding optional fields
- Loosening validation (accepting more inputs)
- Adding new protocol names
- Adding new header constants alongside old ones

## Versioning strategy

- **SchemaVersion field**: Tracks the mechanical version of the `OntogonyEnvelope<TPayload>` shape. Default is `1.0`. Increment when the wire format changes.
- **Semver in CHANGELOG**: Breaking changes bump minor or major version depending on scope (e.g., `0.2.0` → `0.3.0` for a breaking change in a pre-1.0 library).
- **Migration notes**: Each breaking change must have a `docs/migrations/*.md` file explaining the change, timeline, and steps for consuming services.

## Testing requirements

Before a breaking change is merged:

1. **Fixture tests**: Valid and invalid JSON fixtures must pass/fail as expected in `schemas/fixtures/valid/*.json` and `schemas/fixtures/invalid/*.json`.
2. **Schema validation**: The JSON schema in `schemas/ontogony-envelope.schema.json` must align with `DefaultEnvelopeValidator` logic.
3. **CloudEvents round-trip**: All optional and required fields must survive `ToCloudEvent()` + `ToOntogonyEnvelope<T>()` conversions.
4. **Header snapshot tests**: `OntogonyEventHeaders` and `ProtocolNames` constants must have snapshot tests in `HeaderConstantsSnapshotTests.cs`.

## Breaking-change checklist

Before merging a PR that modifies contracts, headers, or envelope validation:

- [ ] **What changed?** Describe the modification (e.g., "added required field X", "renamed header Y").
- [ ] **Why?** Document the business or operational reason.
- [ ] **Migration path**: How will consuming services adapt? What is the timeline?
- [ ] **Tests added or updated?**
  - [ ] Schema fixtures updated (valid and invalid examples)?
  - [ ] JSON schema (`ontogony-envelope.schema.json`) updated?
  - [ ] `DefaultEnvelopeValidator` logic updated to match?
  - [ ] CloudEvents round-trip tests updated?
  - [ ] Header snapshot tests updated (if constants changed)?
- [ ] **Documentation updated?**
  - [ ] `CHANGELOG.md` entry added?
  - [ ] Migration note created under `docs/migrations/`?
  - [ ] This policy updated if scope changed?
  - [ ] Header compatibility matrix updated?
- [ ] **Consuming services contacted?**
  - [ ] Allagma aware and prepared?
  - [ ] Kanon aware and prepared?
  - [ ] Conexus aware and prepared?
  - [ ] Any other consumers aware?

## Review gates

All PRs modifying `Ontogony.Contracts` or `schemas/` must include:

1. Clear title indicating "breaking" or "compatible" in the description
2. A filled-out breaking-change checklist (even if all items are "not applicable")
3. Test passes for all fixture and validation tests
4. Documentation proof that migration notes and changelog are in place

## Compatibility matrix

See [header-compatibility-matrix.md](./header-compatibility-matrix.md) for a detailed table of header stability across service boundaries.

## System compatibility gate (PLATFORM-9-001)

Cross-repo mechanical drift detection: [SYSTEM_COMPATIBILITY_GATE.md](./SYSTEM_COMPATIBILITY_GATE.md). Run via `scripts/run-system-compatibility-gate.ps1`.

## Error envelope conformance (PLATFORM-9-002)

Cross-service failure shape enforcement: [CROSS_SERVICE_ERROR_ENVELOPE_GATE.md](./CROSS_SERVICE_ERROR_ENVELOPE_GATE.md). Included in the system compatibility gate; standalone: `scripts/validate-cross-service-error-envelope.ps1`.

## Header propagation contract (PLATFORM-9-003) — done

Frozen outbound header names and reusable conformance helpers: [HEADER_PROPAGATION_CONTRACT.md](./HEADER_PROPAGATION_CONTRACT.md). Matrix: [`docs/system/propagation-header.matrix.json`](../system/propagation-header.matrix.json). Gate checks in `HeaderPropagationConformance`; standalone: `scripts/validate-header-propagation-contract.ps1`.

**Consumer proofs (done):** ALLAGMA-PROP-001, KANON-PROP-001, CONEXUS-PROP-001 — see [PHASE_TIGHT_CLOSEOUT_2026-05-22.md](../planning/PHASE_TIGHT_CLOSEOUT_2026-05-22.md).
