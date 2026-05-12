# Quota reservation/refund extension

Priority: Medium

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Evaluate whether existing `IQuotaLedger.TryConsumeAsync` is sufficient.
- If not, add mechanical reserve/commit/refund semantics.
- Keep amount/unit/window semantics generic.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No project plan tiers.
- No provider/model price logic.
- No Conexus quota policy.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
