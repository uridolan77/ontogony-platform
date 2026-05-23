# 01 — Master Cursor Prompt

Copy this prompt into Cursor at the root of the relevant repo or workspace.

```text
You are working on EVIDENCE-SPINE-REAL-001 for the Ontogony system.

Goal:
Make the Evidence Spine resolver truthful and live-system-grade. Fix route-decision resolution, Kanon applicability semantics, graph node canonicalization, stable missing reason codes, source attempt display, route-template normalization, and governed fake-provider evidence proof.

Important context:
- The system has Conexus as model gateway, Kanon as semantic authority, Allagma as governed execution runtime, and ontogony-frontend as operator console.
- Evidence Spine currently resolves many nodes but shows partial graph, missing route decision detail, generic source errors, placeholder duplicate nodes, and ambiguous IDs.
- Direct Conexus model calls are not governed Allagma/Kanon flows. Do not require Kanon decision links for direct Conexus roots.
- Governed Allagma runs should require Kanon planning decision and Conexus model-call evidence.
- The existing code may already implement parts of this; inspect before changing.

First step:
Search the repo for Evidence Spine resolver code, graph node types, source attempt types, missing-link handling, route-decision client calls, Kanon semantic graph expansion, Allagma run lookup routes, Conexus model-call/evidence-link handling, and tests.

Implementation rules:
1. Preserve existing public behavior unless it is false or ambiguous.
2. Replace generic source errors with stable missing/failure reason codes.
3. Add applicability classification: required, optional, not_applicable.
4. Canonicalize graph nodes by kind + canonical ID; merge placeholder nodes into authoritative nodes.
5. Distinguish modelCallId, requestId, and executionRunId in data model and UI labels.
6. Normalize Allagma route templates to /allagma/v0/runs/{runId} and related v0 routes.
7. Ensure routeDecisionId returned from Conexus evidence links resolves to detail or a structured backend_missing/not_recorded reason.
8. Add regression tests for direct Conexus root, governed Allagma root, baseline comparison root, Kanon decision root, and unresolved route decision.
9. Update export bundle schema only if required and keep backward compatibility where practical.
10. Do not count fixture-only evidence as live evidence.

Deliverables:
- Code changes.
- Unit tests and integration/contract tests where feasible.
- Updated docs or inline operator copy for missing reason semantics.
- A concise final report listing files changed, behavior before/after, and test commands run.

Acceptance criteria:
- No generic “An unexpected error occurred” appears in Evidence Spine source attempts.
- Direct Conexus model call no longer reports missing Kanon decision as a hard missing link.
- Governed Allagma run resolves Allagma -> Kanon -> Conexus chain with required links.
- Duplicate placeholder/resolved nodes merge into one canonical node.
- Route decision evidence is either loaded or structurally explained.
- Model call, request, and execution run IDs are separately displayed.
- Tests cover the above.
```
