# Generic secret value resolver abstraction

Priority: High

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Add a neutral `ISecretValueResolver` or equivalent abstraction if missing.
- Provide a safe development resolver that reads from environment/config only by reference.
- Never log raw resolved values.
- Keep cloud-vault SDK integrations out unless explicitly added as separate optional packages later.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No provider credential semantics.
- No OpenAI-specific naming.
- No product policy about which secret is valid.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
