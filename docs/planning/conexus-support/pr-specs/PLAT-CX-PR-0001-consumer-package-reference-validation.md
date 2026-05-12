# Consumer package/reference validation

Priority: High

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Add a script or docs-assisted validation that a consumer starter matches SDK, TFM, and central package assumptions.
- Validate sibling project reference paths for known consumer workspaces.
- Produce clear errors rather than silently drifting.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- Does not know Conexus domain entities.
- May mention Conexus only as an example consumer.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
