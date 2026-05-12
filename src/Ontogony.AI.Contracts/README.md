# Ontogony.AI.Contracts

Mechanical, serialization-friendly DTOs for **recording** LLM interactions: requests, responses, stream chunks, usage, cost, provider errors, tool-call facts, and model capability descriptors.

## Guarantees

- No routing, ranking, planning, canonization, or retrieval policy types.
- Hash fields are opaque string fingerprints (how you compute them is a caller concern).
- Optional fields are explicit (`null` / omitted) so hosts can evolve independently.

## Dependencies

- `Ontogony.Contracts` (for shared envelope/header conventions when wrapping payloads).

## Status

Introduced in **PR36** (contracts only; no execution engine or provider SDK).
