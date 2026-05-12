# Ontogony.AI.Contracts

Mechanical, serialization-friendly DTOs for **recording** LLM interactions: requests, responses, stream chunks, usage, cost, provider errors, tool-call facts, and model capability descriptors.

## What this is

- Provider-neutral telemetry shapes for gateways and workers.
- **`Provider` and `Model` are opaque strings** — no platform enums or registries.

## What this is not

- Not routing, ranking, planning, canonization, retrieval policy, or provider SDK types.

## Guarantees

- Hash fields are opaque string fingerprints (how you compute them is a caller concern).
- Optional fields are explicit (`null` / omitted) so hosts can evolve independently.

## Dependencies

- `Ontogony.Contracts` (for shared envelope/header conventions when wrapping payloads).

## Status

Introduced in **PR36** (contracts only; no execution engine or provider SDK).
