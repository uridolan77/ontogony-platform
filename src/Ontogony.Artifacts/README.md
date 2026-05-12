# Ontogony.Artifacts

Mechanical artifact reference contracts and a reference in-memory, content-addressed artifact store for large or sensitive payloads (prompts, responses, documents, tool outputs, checkpoints, replay bundles).

## What this is

- **`ArtifactRef`** and store ports for passing large payloads **by reference** instead of inlining them in envelopes.
- **`MediaType`, `ContentEncoding`, `StorageTier`, and `Classification` are opaque strings** — the caller decides their meaning (e.g. `application/json`, `gzip`, `local`, `sensitive`). Ontogony does not define a registry.
- **`InMemoryArtifactStore`** — thread-safe reference store for **tests and single-process** hosts.

## What this is not

- No cloud provider semantics (no S3/Azure Blob/GCS bindings, no provider enums, no SDK types on the public surface).
- No product meaning (no retention policy, no PII classification rules, no domain-specific lifecycle).

## Guarantees

- **Content addressing is deterministic.** `ArtifactRef.ContentHash` is a lowercase hex SHA-256 of the **stored raw bytes**; identical bytes within the same identity tuple (`TenantId`, `WorkspaceId`, `ProjectId`, `MediaType`, `ContentEncoding`, `Classification`) dedupe to the same `ArtifactId`. `StorageTier` and `Uri` are hint metadata and do not affect identity.
- **Stream-based writes are first-class.** `ArtifactStreamPutRequest` lets implementations stream large payloads without buffering; `ExpectedSizeBytes` / `ExpectedContentHash` are verified when supplied.
- **Defensive reads.** The in-memory reference store returns a copy of the stored bytes so consumers cannot mutate the backing array.
- **Records are serialization-friendly.** Public DTOs are sealed `record` types with init-only properties and round-trip through `System.Text.Json` and `Ontogony.Hashing.CanonicalJson`.

## Dependencies

- `Ontogony.Contracts` (envelope conventions).
- `Ontogony.Hashing` (`Sha256ContentHashService`, `CanonicalJson`).
- `Ontogony.Primitives` (`IClock`, `IIdGenerator`).
- `Microsoft.Extensions.DependencyInjection.Abstractions` (optional registration helpers).

## Status

Introduced in **PR37** as part of the AI mechanical substrate. The in-memory store is intended for tests, examples, and single-process hosts; durable providers ship in product or future platform packages.
