# Ontogony.Artifacts

Mechanical artifact reference contracts and an in-memory, content-addressed artifact store for large or sensitive payloads (prompts, responses, documents, tool outputs, checkpoints, replay bundles).

## What this package is

- Serialization-friendly `ArtifactRef` records describing **where** an artifact lives and **what** it fingerprints to — never the payload bytes themselves.
- `ArtifactPutRequest` / `ArtifactPutResult` / `ArtifactContent` for in-process write/read of bytes.
- `IArtifactStore` mechanical port plus a thread-safe `InMemoryArtifactStore` reference implementation suitable for tests, examples, and single-process hosts.
- Deterministic content addressing: lowercase hex SHA-256 over the raw bytes via `Ontogony.Hashing.Sha256ContentHashService`.

## What this package is not

- No cloud provider bindings (no S3, Azure Blob, GCS types in public API).
- No retention, eviction, or replication policy — those belong in the host.
- No PII or sensitivity policy: `Classification`, `MediaType`, `StorageTier`, `ContentEncoding` are **opaque strings** that the caller defines.
- No model routing, agent planning, canonization, or KB meaning.

## Opaque conventions

The platform does not enforce vocabulary; common values you may choose to standardize within a service:

- `MediaType`: `application/json`, `text/plain`, `application/octet-stream`, `text/markdown` …
- `ContentEncoding`: `identity`, `gzip`, `br`, `zstd`.
- `StorageTier`: `inline`, `local`, `remote`.
- `Classification`: `public`, `internal`, `sensitive`, `redacted`.

## Deduplication

`InMemoryArtifactStore` dedupes by the tuple `(ContentHash, TenantId, WorkspaceId, ProjectId, MediaType, Classification)`. Two payloads with the same bytes but different tenants get distinct `ArtifactId`s; identical bytes within the same scope return the same reference with `ArtifactPutResult.Existed = true`.

## See also

- `docs/ai-runtime/boundary-guardrails.md`
- `docs/ai-runtime/implementation-order.md`
- ADR `docs/adr/ADR-0037-artifacts-as-large-payload-boundary.md`
