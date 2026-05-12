# Idempotency result metadata extension

Priority: Medium

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Evaluate richer result references or metadata for duplicate request handling.
- Keep storage and state transitions generic.
- Document multi-node limitations of in-memory implementation.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No OpenAI response semantics.
- No gateway-specific replay behavior.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
