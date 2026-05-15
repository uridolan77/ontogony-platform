# PR-PLAT-ART-001 — Durable Artifact Store Provider

## Status

Later / conditional.

## Candidate providers

```text
Ontogony.Artifacts.FileSystem
Ontogony.Artifacts.Postgres
```

## Requirements

```text
content hash verification
idempotent put by content/scope
stream support
classification metadata
size limit enforcement
readiness check if required
```

## Non-goals

No semantic replay engine. No document indexing. No product payload meaning.
