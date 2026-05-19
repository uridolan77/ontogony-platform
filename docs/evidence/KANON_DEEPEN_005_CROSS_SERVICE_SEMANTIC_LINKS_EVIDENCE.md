# KANON-DEEPEN-005 — Cross-service semantic links

**Status:** Implementation complete — browser walkthrough **not verified in this polish pass**  
**Depends on:** [KANON-DEEPEN-003](KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md), [KANON-DEEPEN-004](KANON_DEEPEN_004_FACTS_PLANS_BINDINGS_EXPLORER_EVIDENCE.md)  
**Sequence index:** [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)

## Scope boundary

This item is a **Kanon semantic-authority bridge** from Allagma/Conexus operator surfaces into Kanon decision/provenance routes. It is **not**:

- The full **Cross-Service Evidence Spine** package (`EVIDENCE-SPINE-*`)
- A backend graph resolver or unified evidence export spine
- A substitute for Allagma audit bundle or Conexus telemetry listing APIs

## Implementation commits

| Repo | Commit | Summary |
|---|---|---|
| `ontogony-frontend` | `c5c4bfc` | Semantic authority link helpers, Allagma cards, Conexus guidance, correlation href updates |
| `ontogony-platform` | `68f6e90` | Initial 005 evidence + partial README index (expanded in polish pass) |

## ID inventory (Allagma run GET / correlation)

| Identifier | Typical source | Kanon link target |
|---|---|---|
| `planningDecisionId` | Run GET, correlation | `/kanon/decisions?decisionId=…` |
| `actionEvaluationDecisionId` | Run events / audit (not run GET) | Same; slot documents gap |
| `topologyAuthorizationDecisionId` | Audit topology evidence | Same |
| `ontologyVersionId` | Run GET | `/kanon/ontologies?versionId=…` |
| `domainPackId` / version | Not on run GET today | `/kanon/domain-packs?packId=…` when present |
| `traceId` | Run GET, correlation | `/kanon/decisions?mode=trace&traceId=…` |
| `humanGateId` | Pause events / correlation | Allagma gates (+ Kanon when decision linked) |
| `modelCallId` | Run GET, eval metrics | Conexus observability (not Kanon) |

## Repos touched

| Repo | Change |
|---|---|
| `ontogony-frontend` | Shared link builders, Allagma semantic authority cards, Conexus Kanon guidance, correlation href updates |
| `ontogony-platform` | Evidence + sequence index |

No backend DTO changes in 005.

## Source files (frontend)

- `src/shared/navigation/kanonSemanticAuthorityLinks.ts`
- `src/allagma/adapters/buildAllagmaSemanticAuthorityLinkSlots.ts`
- `src/allagma/components/AllagmaSemanticAuthorityLinksCard.tsx`
- `src/allagma/pages/RunDetailPage.tsx`, `EvaluationRunDetailPage.tsx`
- `src/allagma/adapters/buildRunEvidenceJourneyLinks.ts`
- `src/conexus/components/ConexusObservabilityGuidanceCard.tsx`
- `src/conexus/components/ConexusExecutionRunDetailCard.tsx`
- `src/system/correlation/correlationAdapters.ts`

Allagma/Conexus surfaces use href helpers only — **no Kanon DTO imports** in Allagma/Conexus modules.

## Closed (005 acceptance)

- Central Kanon semantic link helpers
- Allagma run detail semantic authority card
- Allagma evaluation detail Kanon semantic evidence card
- Run/eval evidence journeys use shared Kanon link helpers
- Conexus observability guidance includes Kanon semantic decisions section
- Conexus execution-run detail links to Kanon by decision or trace
- Unavailable links show explicit per-slot reasons; no fabricated hrefs

## Validation

| Check | Command | Result (2026-05-20 polish pass) |
|---|---|---|
| Link builder tests | `npm test -- src/shared/navigation/kanonSemanticAuthorityLinks.test.ts` | **3 passed** |
| Authority slot tests | `npm test -- src/allagma/adapters/buildAllagmaSemanticAuthorityLinkSlots.test.ts` | **2 passed** |
| Conexus guidance test | `npm test -- src/conexus/components/ConexusObservabilityGuidanceCard.test.ts` | **1 passed** |

## Manual browser verification

**Status: NOT VERIFIED** in this polish pass.

| Action | Expected |
|---|---|
| Allagma run with `planningDecisionId` | Semantic authority card links; Open in Kanon works |
| Missing `actionEvaluationDecisionId` | Slot unavailable with explicit reason |
| Eval run detail | Kanon semantic evidence card |
| Conexus observability | Kanon guidance + execution-run Kanon chip |

## Known limitations

- `actionEvaluationDecisionId` and `domainPackId` not on Allagma run GET.
- Domain pack deep-link `packId` query is best-effort until workbench supports selection-by-URL.
- Cross-service links are identifier-driven, not a full evidence graph.
- Browser verification requires rebuilt frontend image.

## Follow-up

- **KANON-DEEPEN-006** — closeout manual QA (done as checklist; browser pending).
- **EVIDENCE-SPINE-001** — unified cross-service evidence resolver (separate package).
