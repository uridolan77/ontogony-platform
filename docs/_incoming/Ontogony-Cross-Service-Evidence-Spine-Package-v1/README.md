# Ontogony — Cross-Service Evidence Spine Package v1

This package plans a deep cross-service enhancement:

```text
Paste any known ID → resolve the whole execution graph.
```

The target is a unified evidence spine that connects:

```text
Allagma run
Allagma evaluation
Allagma replay/evidence/audit bundles
Conexus model call
Conexus route decision
Kanon planning/provenance decision
Trace/correlation IDs
Human gate IDs
Baseline comparison IDs
Dataset/scenario IDs
```

## Why this matters

Today, Ontogony has many strong evidence surfaces, but the operator still has to mentally stitch together IDs across modules. A run page may know one part of the chain, a Conexus observability page another, and a Kanon decision page another.

The evidence spine turns this into one pattern:

```text
Known ID in → resolved graph out.
```

## Package contents

```text
00_MANIFEST.json
01_EXECUTIVE_BRIEF.md
02_CURRENT_STATE_FINDINGS.md
03_TARGET_ARCHITECTURE.md
04_IDENTIFIER_TAXONOMY.md
05_RESOLUTION_ALGORITHM.md
06_API_CONTRACT_GAP_MATRIX.md
07_OPERATOR_UX_SPEC.md
08_EVIDENCE_GRAPH_DATA_MODEL.md
09_EXPORT_AND_REDACTION_BOUNDARY.md
10_TESTING_AND_MANUAL_QA.md
11_REPO_QUALITY_GATE.md

prompts/
  00_UNPACK_AND_START.md
  EVIDENCE-SPINE-000_CURRENT_STATE_AUDIT.md
  EVIDENCE-SPINE-001_RESOLVER_CONTRACT_AND_ID_TAXONOMY.md
  EVIDENCE-SPINE-002_FRONTEND_UNIFIED_RESOLVER_V1.md
  EVIDENCE-SPINE-003_ALLAGMA_EVIDENCE_NORMALIZATION.md
  EVIDENCE-SPINE-004_CONEXUS_REQUEST_AND_ROUTE_LINKING.md
  EVIDENCE-SPINE-005_KANON_DECISION_PROVENANCE_LINKING.md
  EVIDENCE-SPINE-006_GRAPH_WORKBENCH_UI.md
  EVIDENCE-SPINE-007_EXPORT_BUNDLE_AND_DIAGNOSTIC_PACK.md
  EVIDENCE-SPINE-008_E2E_BROWSER_AND_DOCKER_VERIFICATION.md
  EVIDENCE-SPINE-009_CLOSEOUT.md

templates/
checklists/
appendices/
```

## Recommended sequence

Start with the audit. Do not immediately build a graph visualization. First make the identifier taxonomy and resolver contract explicit.

```text
1. EVIDENCE-SPINE-000 — audit existing resolver/linking state
2. EVIDENCE-SPINE-001 — canonical ID taxonomy + resolver contract
3. EVIDENCE-SPINE-002 — frontend unified resolver v1
4. EVIDENCE-SPINE-003/004/005 — normalize Allagma/Conexus/Kanon evidence adapters
5. EVIDENCE-SPINE-006 — graph workbench UI
6. EVIDENCE-SPINE-007 — evidence export bundle
7. EVIDENCE-SPINE-008 — e2e/browser verification
8. EVIDENCE-SPINE-009 — closeout
```
