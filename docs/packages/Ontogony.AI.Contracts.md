# Ontogony.AI.Contracts

Mechanical DTOs for LLM **telemetry and interchange**: requests, responses, stream chunks, usage, cost, provider errors, tool-call facts, and model capability descriptors.

## What this package is

- Serialization-friendly `record` types for cross-service emission and recorders.
- **`Provider` and `Model` are opaque strings** — Ontogony does not define provider registries, ranking, or model-selection policy (avoid adding platform enums such as `OpenAI` / `Anthropic` here).
- Hash fields are opaque fingerprints (callers define how they are computed).
- Wrap payloads in `OntogonyEnvelope<TPayload>` when participating in the standard event pipeline; validate with `DefaultEnvelopeValidator`.

## What this package is not

- No model routing, ranking, fallback, or cost-optimization policy.
- No agent planning, tool-selection, or execution orchestration.
- No Athanor-style canonization, epistemics, or KB meaning.

## See also

- `docs/ai-runtime/boundary-guardrails.md`
- `docs/ai-runtime/implementation-order.md`
- ADR `docs/adr/ADR-0036-ai-runtime-contracts.md`
