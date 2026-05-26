# CX-CONTRACT-001 to CX-CONTRACT-006 — Conexus contract-discipline slice

## Objective

Raise Conexus contract discipline above 9 by making gateway/admin contracts explicit, snapshotted, and package-mode enforced.

## CX-CONTRACT-001 — OpenAPI snapshot completeness

Ensure gateway and admin path snapshots cover `/v1/chat/completions`, `/v1/models`, governance quota/usage, `/conexus/v0` model-call evidence, `/admin/v0/model-calls`, `/admin/v0/replay/*`, route preview, and alias readiness routes if added.

## CX-CONTRACT-002 — DTO golden snapshots expansion

Add/extend golden snapshots for model call detail, route decision detail, replay dry-run response, provider capability profile, maintenance history, and admin auth errors.

## CX-CONTRACT-003 — Provider capability contract matrix

Capability profile must be generated/validated against provider adapters. Route-preview must expose capability mismatch in stable shape.

## CX-CONTRACT-004 — Error contract map

Conexus may remain OpenAI-shaped for `/v1`, but admin/governance routes need clear error classification. Tests should cover idempotency conflict, streaming idempotency rejection, auth failure, quota exceeded, and provider unavailable.

## CX-CONTRACT-005 — Package-mode contract proof

Ensure `Conexus.Client` / `Conexus.Contracts` package-mode builds remain valid for Allagma. Package version bumps require changelog and downstream matrix update.

## CX-CONTRACT-006 — Compatibility manifest

Maintain a Conexus compatibility manifest with API prefixes, package versions, snapshot hashes, route groups, provider capability artifact, and contract gates.
