# ASP.NET consumer test host fixtures

Priority: Medium

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Provide reusable fixtures for service-default middleware ordering and health endpoints.
- Help consumers test tracing/logging/error handling composition.
- Keep package in `Ontogony.Testing`.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No Conexus endpoint semantics.
- No gateway auth helper.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
