# KANON-CONNECT-005 — Evidence Spine imports Kanon semantic graph

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (unit tests + export bundle schema validation)  
**Statement:** Evidence Spine deep resolution merges Kanon `GET /ontology/v0/semantic-graph` for Kanon anchors alongside existing decision/trace/Conexus graphs. Semantic graph nodes, edges, source attempts, and partial completeness are converted into Evidence Spine format without replacing prior resolvers.

## Summary

Wired `getKanonSemanticGraph` into Evidence Spine deep-graph contributions for `kanonDecisionId`, `planningDecisionId`, and `humanGateId` anchors (plus secondary discovery when identifiers carry decision or gate ids). Kanon semantic graph cross-service placeholders remain honest via `rawPreview.crossServicePlaceholder` and preserved Kanon `sourceAttempts`.

## Repos touched

| Repo | Change |
| --- | --- |
| `ontogony-frontend` | `kanonSemanticGraphEvidenceAdapters`, `resolveKanonSemanticEvidenceGraph`, `resolveEvidenceSpineDeepGraphs` integration, catalog notes, tests |
| `ontogony-platform` | This evidence file |

## Anchor → semantic graph query

| Evidence anchor | Semantic graph query param |
| --- | --- |
| `kanonDecisionId` / `planningDecisionId` | `decisionId` |
| `humanGateId` | `humanGateId` |
| Secondary `kanonDecisionId` on non-decision primary | `decisionId` |
| Secondary `humanGateId` on non-gate primary | `humanGateId` |
| `entityRef` / `ontologyVersionId` / `domainPackId` | Supported by adapter when a single anchor is supplied (future identifier taxonomy) |

## Tests

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/kanon/adapters/kanonSemanticGraphEvidenceAdapters.test.ts src/kanon/adapters/resolveKanonSemanticEvidenceGraph.test.ts src/evidence-spine/resolveEvidenceSpineDeepGraphs.test.ts src/evidence-spine/buildEvidenceSpineExportBundle.test.ts
```

| Check | Result |
| --- | --- |
| Semantic graph → evidence graph conversion | PASS |
| Kanon `sourceAttempts` preserved on evidence resolution | PASS |
| Partial completeness + warnings propagated | PASS |
| Deep resolver loads semantic graph with decision graph | PASS |
| Export bundle schema validation | PASS |

## Acceptance

- [x] Evidence Spine uses Kanon canonical semantic graph for Kanon anchors
- [x] Cross-service placeholder nodes remain honest
- [x] Existing decision/trace behavior still works (additive merge)
- [x] Export bundle schema validation passes

## Related

- [KANON-CONNECT-004](./KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md)
- [KANON-DEEPEN-011](../evidence/KANON_DEEPEN_011_SEMANTIC_EVIDENCE_GRAPH_EVIDENCE.md) (if present) / semantic graph API
- Next: [KANON-CONNECT-006](./KANON_CONNECT_006_ROUTE_PARITY_EVIDENCE.md) — route/OpenAPI/catalog parity gate ✅
