# Ontogony.Artifacts

Mechanical artifact reference contracts and an in-memory, content-addressed artifact store for large or sensitive payloads (prompts, responses, documents, tool outputs, checkpoints, replay bundles).

## What this package is

- Serialization-friendly `ArtifactRef` records describing **where** an artifact lives and **what** it fingerprints to — never the payload bytes themselves.
- `ArtifactPutRequest` (in-memory) and `ArtifactStreamPutRequest` (stream-based) for write, plus `ArtifactPutResult` and `ArtifactContent` for read.
- `IArtifactStore` mechanical port plus a thread-safe `InMemoryArtifactStore` reference implementation suitable for tests, examples, and single-process hosts.
- Deterministic content addressing: lowercase hex SHA-256 over the **stored raw bytes** via `Ontogony.Hashing.Sha256ContentHashService`. Stream writes verify `ExpectedSizeBytes` and `ExpectedContentHash` when supplied.

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

## Identity vs hint metadata

`ArtifactRef` fields split into two roles:

| Role | Fields | Notes |
|------|--------|-------|
| **Identity (dedupe)** | `ContentHash`, `TenantId`, `WorkspaceId`, `ProjectId`, `MediaType`, `ContentEncoding`, `Classification` | Same bytes labelled with different `ContentEncoding` (e.g. `identity` vs `gzip`) are **different artifacts**, because the encoding tells readers how to interpret those exact bytes. |
| **Hint (locator)** | `StorageTier`, `Uri` | Do not affect dedupe identity. The store may keep the first hint it observed for a given identity. |

`ContentHash` is computed over the **stored raw bytes** — i.e. the bytes the store accepted, whatever encoding `ContentEncoding` labels them as. Callers that pre-compress should pass `ContentEncoding = "gzip"`; callers that store raw bytes should pass `identity` (or omit).

## Deduplication

`InMemoryArtifactStore` dedupes by the identity tuple `(ContentHash, TenantId, WorkspaceId, ProjectId, MediaType, ContentEncoding, Classification)`. Two payloads with the same bytes but different tenants — or different `ContentEncoding` — get distinct `ArtifactId`s; identical bytes within the same identity tuple return the same reference with `ArtifactPutResult.Existed = true`.

## Defensive reads

`InMemoryArtifactStore` returns a **defensive copy** of stored bytes from `GetAsync` / `TryGetAsync`, so consumers cannot mutate the in-process backing array. Likewise, `PutAsync(ArtifactPutRequest)` copies the caller's buffer before storing, so subsequent caller mutation does not change stored content.

## Error surface

`ArtifactNotFoundException.Message` embeds the missing id for diagnostics. Hosts that surface exceptions to external callers should map this through their error middleware (e.g. `Ontogony.Errors`) so artifact ids are not leaked verbatim in public responses.

## See also

- `docs/ai-runtime/boundary-guardrails.md`
- `docs/ai-runtime/implementation-order.md`
- ADR `docs/adr/ADR-0037-artifacts-as-large-payload-boundary.md`
