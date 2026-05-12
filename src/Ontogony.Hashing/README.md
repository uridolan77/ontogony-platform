# Ontogony.Hashing

Deterministic **canonical JSON** and **SHA-256** helpers for envelopes, idempotency, and artifacts.

## What this is

- `CanonicalJson` — sorted-key JSON serialization for stable fingerprints.
- `PayloadHasher`, `IContentHashService` / `Sha256ContentHashService` — byte and object hashing.

## What this is not

- Not semantic “truth” hashing for domain aggregates; callers decide what gets serialized.
