# Durable quotas/idempotency adapters

Priority: Later

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Add durable stores only when Conexus needs multi-node behavior.
- Prefer existing persistence/Postgres package layering.
- Document transactional assumptions.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No product quota policy.
- No gateway billing.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
