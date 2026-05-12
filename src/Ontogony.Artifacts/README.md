# Ontogony.Artifacts

Mechanical artifact reference contracts and a reference in-memory, content-addressed artifact store for large or sensitive payloads (prompts, responses, documents, tool outputs, checkpoints, replay bundles).

## Guarantees

- **No cloud provider semantics.** No S3/Azure Blob/GCS bindings, no provider enums, no SDK types on the public surface.
- **No product meaning.** No retention policy, no PII classification rules, no domain-specific lifecycle.
- **`MediaType`, `ContentEncoding`, `StorageTier`, and `Classification` are opaque strings** — the caller decides their meaning (e.g. `application/json`, `gzip`, `local`, `sensitive`). Ontogony does not define a registry.
- **Content addressing is deterministic.** `ArtifactRef.ContentHash` is a lowercase hex SHA-256 of the raw bytes; identical bytes within the same scope (`TenantId`, `WorkspaceId`, `ProjectId`, `MediaType`, `Classification`) dedupe to the same `ArtifactId`.
- **Records are serialization-friendly.** Public DTOs are sealed `record` types with init-only properties and round-trip through `System.Text.Json` and `Ontogony.Hashing.CanonicalJson`.

## Dependencies

- `Ontogony.Contracts` (envelope conventions).
- `Ontogony.Hashing` (`Sha256ContentHashService`, `CanonicalJson`).
- `Ontogony.Primitives` (`IClock`, `IIdGenerator`).
- `Microsoft.Extensions.DependencyInjection.Abstractions` (optional registration helpers).

## Status

Introduced in **PR37** as part of the AI mechanical substrate. The in-memory store is intended for tests, examples, and single-process hosts; durable providers ship in product or future platform packages.
