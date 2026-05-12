# Migration: PR21 — Athanor hashing stage 1 (SHA-256 delegation, canonical JSON unchanged)

## Summary

Athanor delegates the **UTF-8 → SHA-256 (lowercase hex)** step to `Ontogony.Hashing.Sha256ContentHashService` while keeping **Athanor-owned canonical JSON** (`Athanor.Infrastructure.Hashing.CanonicalJson`) for object serialization and `CanonicalizeJson`.

## Parity probe result

**First mismatch category:** string escaping in canonical JSON for object payloads containing control characters or quotes that require escaping.

- Athanor uses `System.Text.Json` nodes with `JavaScriptEncoder.UnsafeRelaxedJsonEscaping`.
- Ontogony uses `Utf8JsonWriter` defaults (stricter Unicode escapes).

Nested anonymous objects and arrays **matched** in the parity probe; the divergent case was `{ text = "line1\n..." }`.

## Consumer impact

- **Fingerprints and content hashes produced by Athanor’s `IContentHashService` are unchanged** for the current canonicalization path.
- Services that **replace** Athanor canonical JSON with `Ontogony.Hashing.CanonicalJson` without a coordinated migration **will change digests** for payloads that hit the escaping divergence.

## Future alignment (optional)

To reach byte-identical JSON between stacks (full Ontogony delegation), either:

- **A — Platform:** align `Ontogony.Hashing.CanonicalJson` writer options with Athanor’s relaxed escaping (mechanical change; requires golden vectors in platform tests and coordinated consumer rollout), or  
- **C — Consumer:** keep Athanor JSON until **A** is proven on all required vectors.

Stage 1 chose **B — adapter:** shared digest primitive only.

## References

- Athanor: `GoldenHashVectorsTests`, `OntogonyCanonicalJsonParityProbeTests`, `Sha256ContentHashService`.
- Platform: `Ontogony.Hashing.CanonicalJson`, `Ontogony.Hashing.Sha256ContentHashService`.
