# Repo task — ontogony-platform

Platform owns neutral contracts and schemas only.

## Required work

Add or update:

- `docs/contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`
- `docs/schemas/ontogony-system-cohesion-summary-v1.schema.json`
- `docs/schemas/ontogony-cross-service-error-classification-v1.schema.json`
- schema fixtures under `docs/schemas/fixtures/valid/` and `invalid/` if the repo convention exists.

## Rules

- No product-specific semantics.
- No gaming-core assumptions.
- No Allagma/Kanon/Conexus runtime logic.
- Schemas should validate artifacts produced by Allagma scripts.

## Tests

Add or update platform schema validation tests/scripts so the new schemas cannot silently drift.

## Done when

Allagma's SYSTEM-COH-001 summary artifact can be validated against a platform-owned schema.
