# AI telemetry sink abstraction

Priority: Medium

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Evaluate a generic sink/recorder abstraction around `LlmRequestEnvelope`, `LlmResponseEnvelope`, and `LlmProviderError`.
- Ship in-memory sink for tests if useful.
- Keep DTOs provider-neutral.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No provider routing.
- No model ranking.
- No content policy.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
