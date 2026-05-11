# PR15: Envelope validation and CloudEvents hardening

## Summary

- Runtime validation: `DefaultEnvelopeValidator` implements `IEnvelopeValidator` and returns structured `EnvelopeValidationResult` / `EnvelopeValidationError` entries.
- CloudEvents: `ToOntogonyEnvelope` rejects unsupported `specversion` values; missing `traceId` can be rejected via `CloudEventConversionOptions.TraceIdPolicy`.
- `ToCloudEvent` includes a `schemaVersion` extension for round-trip with `OntogonyEnvelope.SchemaVersion`.

## Test and fixture defaults

`TestEnvelopeFactory` and `EnvelopeFixtureBuilder` now default to `agentor.run.started`, `ProtocolNames.Agentor`, and URI-like sources so samples align with validator rules. Call sites that relied on `test.event` / non-URI sources should update.

## JSON schema

`schemas/ontogony-envelope.schema.json` documents the serialized envelope JSON; keep enum lists aligned with `ProtocolNames` when you extend protocols.
