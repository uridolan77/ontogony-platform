# Target architecture

## Evidence spine layers

```text
Layer 1 — Identifier taxonomy
Canonical ID kinds, regex/prefix hints, source systems, accepted aliases.

Layer 2 — Resolver contract
Given one or more IDs, resolve graph nodes and edges with evidence source metadata.

Layer 3 — Service adapters
Allagma, Conexus, Kanon adapters normalize service-specific DTOs into graph nodes.

Layer 4 — Graph view model
Stable `EvidenceGraph` used by UI, diagnostics, and export.

Layer 5 — Operator workbench
Paste any ID, inspect graph, jump to pages, export evidence.

Layer 6 — E2E proof
Browser test and Docker-local manual QA prove real resolution path.
```

## Recommended frontend module

```text
src/evidence-spine/
  evidenceIdentifierTypes.ts
  parseEvidenceIdentifier.ts
  evidenceGraphTypes.ts
  evidenceResolutionTypes.ts
  resolveEvidenceSpine.ts
  evidenceSpineAdapters/
    allagmaEvidenceAdapter.ts
    conexusEvidenceAdapter.ts
    kanonEvidenceAdapter.ts
  components/
    EvidenceSpineLookupBar.tsx
    EvidenceGraphPanel.tsx
    EvidenceGraphNodeCard.tsx
    EvidenceGraphEdgeList.tsx
    EvidenceSpineExportPanel.tsx
  pages/
    EvidenceSpineWorkbenchPage.tsx
```

## Optional backend evolution

Do not start by forcing a new backend aggregator. Frontend-first aggregation is acceptable for v1 because the current APIs already expose many pieces.

Later, consider a platform-level or Allagma-owned endpoint:

```text
POST /ontogony/v0/evidence/resolve
```

or service-specific:

```text
POST /allagma/v0/evidence/resolve
```

Only add this after the frontend resolver proves which backend gaps are real.
