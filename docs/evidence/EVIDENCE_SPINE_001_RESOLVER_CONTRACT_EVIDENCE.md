# EVIDENCE-SPINE-001 — Resolver contract and ID taxonomy evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** Canonical evidence-spine types, identifier parser, and graph helpers implemented in `ontogony-frontend`; platform taxonomy doc added. No unified resolver UI (deferred to EVIDENCE-SPINE-002).

## Scope

`EVIDENCE-SPINE-001` from `docs/_incoming/Ontogony-Cross-Service-Evidence-Spine-Package-v1/prompts/EVIDENCE-SPINE-001_RESOLVER_CONTRACT_AND_ID_TAXONOMY.md`.

## Delivered

| Repo | Artifact |
| --- | --- |
| `ontogony-frontend` | `src/evidence-spine/` module (types, parser, graph helpers, barrel export) |
| `ontogony-frontend` | Unit tests: `parseEvidenceIdentifier.test.ts`, `evidenceGraphHelpers.test.ts` |
| `ontogony-platform` | [`docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) |
| `ontogony-platform` | This evidence file |

## Module layout

```text
src/evidence-spine/
  evidenceIdentifierTypes.ts    — EvidenceIdentifierKind, ParsedEvidenceIdentifier
  evidenceResolutionTypes.ts    — EvidenceLookupInput, EvidenceResolutionResult, limits
  evidenceGraphTypes.ts         — EvidenceGraph, EvidenceNode, EvidenceEdge, page links
  parseEvidenceIdentifier.ts    — classify + manual override
  evidenceGraphHelpers.ts       — addNode, addEdge, mergeIdentifiers, markUnresolvedEdge, buildPageLinks
  index.ts                      — public exports
```

## Acceptance mapping

| Criterion | Status |
| --- | --- |
| Canonical ID taxonomy | `EvidenceIdentifierKind` + operator doc |
| Graph model | `EvidenceGraph`, nodes, edges, page links |
| Parser with override | `parseEvidenceIdentifier({ raw, kind? })` |
| Ambiguous classification | Multiple candidates + `ambiguous` flag |
| Graph helpers | `addNode`, `addEdge`, `mergeIdentifiers`, `markUnresolvedEdge`, `buildPageLinks` |
| Unit tests | Parser + graph helper suites |
| No graph UI | None added |

## Validation performed

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/evidence-spine
```

## Next slice

**EVIDENCE-SPINE-002** — `resolveEvidenceSpine.ts`: multi-root API resolution, source-attempt recording, reuse of `resolveTraceCorrelation` where appropriate.
