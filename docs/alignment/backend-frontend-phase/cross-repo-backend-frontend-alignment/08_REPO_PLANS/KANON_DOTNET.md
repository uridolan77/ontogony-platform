# kanon-dotnet Alignment Plan

## Objective

Make Kanon the stable typed semantic/provenance/replay authority consumed by the frontend.

## PRs

### KANON-BFA-001 — Replay bundle schema hardening
Ensure typed responses include replayBundleId, decisionId, traceId, entity references, provenance items, source bindings, semantic plan references, and lifecycle decision IDs.

### KANON-BFA-002 — Provenance filter/query semantics
Clarify server-side vs frontend-local filters. Add server-side filters only where needed.

### KANON-BFA-003 — Semantic plan response schema stabilization
Ensure compile responses include stable plan, steps, assumptions, source bindings, decision record, trace metadata, and explain/debug fields.

### KANON-BFA-004 — Domain pack lifecycle contract
Ensure lifecycle endpoints include decision IDs, lifecycle history schema, signature status, and redacted error metadata.

### KANON-BFA-005 — Frontend snapshot refresh
Refresh OpenAPI and reduce flexible adapters where typed schemas exist.

## Acceptance

- Decision IDs connect domain-pack lifecycle to provenance.
- Replay/provenance evidence has stable typed source fields.
