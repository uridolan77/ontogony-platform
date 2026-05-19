# KANON-DEEPEN-004 — Facts, plans, and source bindings explorer

**Status:** Implementation complete (action-result workbenches only) — browser walkthrough **not verified in this polish pass**  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md), [KANON-DEEPEN-003](KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md)  
**Sequence index:** [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)

## Goal

Make the semantic reasoning substrate inspectable: source bindings, canonical facts, compiled semantic plans, and the decisions they produce — with honest API limitations where list/history routes are absent.

**Does not claim:** durable browse/history for facts or plans. Those remain resolve/compile **action-result** workbenches unless new read APIs ship.

## Implementation commits

| Repo | Commit | Summary |
|---|---|---|
| `ontogony-frontend` | `65fe83c` | Canonical fact / semantic plan / source binding depth, review-status filtering, limitation cards, tests |

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | No new routes; existing list/filter/resolve/compile APIs |
| `ontogony-frontend` | Substrate workbenches, review-status filter, nested compile mapper, cross-links, limitation cards |
| `ontogony-platform` | Evidence + sequence index |

## Source files (frontend)

- `src/kanon/components/KanonCanonicalFactResolveWorkbench.tsx`
- `src/kanon/components/KanonSemanticPlanWorkbench.tsx`
- `src/kanon/components/KanonSemanticSubstrateCrossLinks.tsx`
- `src/kanon/components/KanonSemanticSubstrateLimitationsCard.tsx`
- `src/kanon/adapters/kanonCanonicalFactAdapters.ts`
- `src/kanon/adapters/kanonSemanticPlanAdapters.ts`
- `src/kanon/pages/SourceBindingsPage.tsx`, `FactsAndPlansPage.tsx`, `KanonSemanticPlansPage.tsx`
- `e2e/kanon-facts.spec.ts`, `e2e/kanon-plan-workbench.spec.ts`, `e2e/kanon-lifecycle.spec.ts`

## Backend routes (verified)

| Route | Purpose |
|---|---|
| `GET /ontology/v0/source-bindings?ontologyVersionId=&reviewStatus=` | List/filter bindings (**server-side** `reviewStatus` supported) |
| `POST /ontology/v0/source-bindings/{bindingId}/review` | Approve/reject |
| `POST /ontology/v0/source-bindings/{bindingId}/test` | Sample row test |
| `POST /ontology/v0/canonical-facts/resolve` | Resolve canonical fact (+ decision) |
| `POST /ontology/v0/semantic-query-plans/compile` | Compile plan (+ decision) |

**No v0 list/history routes** for canonical facts or semantic plans. UI shows current-action workbenches + limitation cards; audit via decision records and provenance.

## Closed (004 acceptance)

- Canonical fact result maps policy, confidence, value, ontology version
- Selected/rejected sources shown on fact resolve view
- Human gate state shown
- Decision/provenance link on fact result
- Semantic plan maps nested `{ plan, decisionRecord }` API shape
- Binding IDs, policy checks, human gate state, ontology version on compile view
- Source binding `reviewStatus` query param (backend + UI)
- `KanonSemanticSubstrateLimitationsCard` on facts/plans/bindings routes

## UI-HARDEN overlap (documented)

Limitation/empty presentation may use `@ontogony/ui/feedback` primitives adopted during the same period as UI-HARDEN-005. That is **shared UI packaging**, not a change to Kanon semantic contracts.

## Validation

| Check | Command | Result (2026-05-20 polish pass) |
|---|---|---|
| Fact adapters | `npm test -- src/kanon/adapters/kanonCanonicalFactAdapters.test.ts` | **2 passed** |
| Plan adapters | `npm test -- src/kanon/adapters/kanonSemanticPlanAdapters.test.ts` | **5 passed** |
| Mocked E2E facts/plans/bindings | `npx playwright test e2e/kanon-facts.spec.ts` etc. | **Not run** in this pass |

## Manual browser verification

**Status: NOT VERIFIED** in this polish pass.

| Action | Expected |
|---|---|
| Source bindings — filter by review status | List updates; URL reflects `reviewStatus` |
| Facts — resolve | Policy/source explanation, decision link |
| Plans — compile | Plan steps, bindings, policy checks, decision link |
| Limitation cards | State no list/history API for facts/plans |

## Known limitations

- Facts are **resolve-result** workbench only; no fake history from browser state.
- Semantic plans are **compile-result** workbench only.
- Fact/plan history requires new read contracts or decision-index navigation.
- Compile mapper accepts flat mocks and nested Kanon responses.
- Source binding deep-link by `bindingId` is route-level until list API supports `bindingId` filter.

## Follow-up

- **KANON-DEEPEN-005** — cross-service semantic links (done).
- **KANON-DEEPEN-008** (candidate) — facts/plans history APIs if product requires browse.
