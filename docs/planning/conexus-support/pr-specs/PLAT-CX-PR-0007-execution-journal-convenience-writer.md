# Execution journal convenience writer

Priority: Low/Medium

## Problem

Conexus.NET may need this reusable infrastructure while building the gateway. This PR should exist only if the capability is generic enough for other Ontogony services.

## Scope

- Add optional helper to append run/step/checkpoint records with consistent ids/timestamps.
- Keep lifecycle words opaque strings.
- Do not add workflow engine, scheduler, DAG, or planner semantics.

## Acceptance criteria

- Public APIs are provider-neutral and product-neutral.
- Tests cover the mechanical behavior.
- Documentation states non-goals clearly.
- Conexus can consume the capability without duplicating it locally.

## Non-goals

- No Agentor orchestration.
- No Conexus request lifecycle policy beyond examples.

## Boundary checklist

- [ ] No provider routing policy.
- [ ] No model preference logic.
- [ ] No price or cost calculation.
- [ ] No OpenAI-compatible endpoint behavior.
- [ ] No content policy.
- [ ] No UI workflow logic.
