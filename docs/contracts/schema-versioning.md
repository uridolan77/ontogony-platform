# Ontogony envelope schema versioning

## Artifacts

| Artifact | Role |
|----------|------|
| `schemas/ontogony-envelope.schema.json` | JSON Schema (draft 2020-12) describing the **serialized** `OntogonyEnvelope<TPayload>` shape for documentation and offline validation. |
| `DefaultEnvelopeValidator` | Runtime mechanical checks aligned with the schema’s intent (required fields, `EventType` shape, URI `Source`, optional `PayloadHash` as 64 lowercase hex). |

The schema cannot fully describe generic `Payload`; product repos attach payload-specific schemas separately.

## `SchemaVersion` field

- `OntogonyEnvelope.SchemaVersion` is a **mechanical** version string for the envelope wrapper (default `1.0`).
- It is **not** automatically the same as a business payload schema version inside `Payload`.
- CloudEvents conversion stores `SchemaVersion` in the `schemaVersion` extension for round-trip fidelity.

## Validator vs schema

- **Runtime:** Use `IEnvelopeValidator` / `DefaultEnvelopeValidator` at ingress boundaries where you need clear, structured errors (`EnvelopeValidationResult`).
- **Offline:** Use the JSON Schema in CI or editors; keep enum lists in sync with `ProtocolNames` and `DefaultEnvelopeValidator.SuggestedProtocolIdentifiers` when you tighten protocol allowlists.

## CloudEvents

- Only CloudEvents **`specversion` 1.0** is accepted when deserializing to `OntogonyEnvelope` (`ToOntogonyEnvelope`).
- Missing `traceId` extension: configurable via `CloudEventConversionOptions.TraceIdPolicy` (`GenerateWhenMissing` vs `RejectWhenMissing`).

## Breaking changes policy

Any tightening of `EventType` patterns, `Protocol` enums, or required fields is a **semver minor or major** change and must be listed in `CHANGELOG.md` with a migration note under `docs/migrations/`.
