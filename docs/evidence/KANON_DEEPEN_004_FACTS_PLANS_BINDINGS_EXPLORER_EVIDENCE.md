# KANON-DEEPEN-004 — Facts, plans, and source bindings explorer

**Status:** Implementation complete (browser verification after frontend image rebuild)  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md), [KANON-DEEPEN-003](KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md)

## Goal

Make the semantic reasoning substrate inspectable: source bindings, canonical facts, compiled semantic plans, and the decisions they produce — with honest API limitations where list/history routes are absent.

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | No new routes; existing list/filter/resolve/compile APIs used as-is |
| `ontogony-frontend` | Substrate workbenches, review-status filter, nested compile mapper, cross-links, limitation cards |
| `ontogony-platform` | This evidence |

## Backend routes (verified)

| Route | Purpose |
|---|---|
| `GET /ontology/v0/source-bindings?ontologyVersionId=&reviewStatus=` | List/filter bindings |
| `POST /ontology/v0/source-bindings/{bindingId}/review` | Approve/reject |
| `POST /ontology/v0/source-bindings/{bindingId}/test` | Sample row test |
| `POST /ontology/v0/canonical-facts/resolve` | Resolve canonical fact (+ decision) |
| `POST /ontology/v0/semantic-query-plans/compile` | Compile plan (+ decision) |

**No v0 list/history routes** for canonical facts or semantic plans (`ICanonicalFactRepository` / `ISemanticQueryPlanRepository` are save-only). UI shows current-action workbenches and explicit limitation copy; audit via decision records and provenance.

## Frontend

| Route | Workbench |
|---|---|
| `/kanon/source-bindings` | Ontology version + review status filter; ontology link; test/approve/reject; limitation card |
| `/kanon/facts` | Resolve workbench; policy/source explanation; human-gate badge; decision + substrate cross-links |
| `/kanon/plans` | Compile workbench; nested API response mapping; policy checks + human gate; bindings used; cross-links |

Cross-links from decisions (`KanonDecisionAuthorityPanel`) now pass ontology version into source bindings and facts routes where available.

## Validation

| Check | Command |
|---|---|
| Frontend unit tests | `npm test` in `ontogony-frontend` |
| Frontend typecheck | `npm run typecheck` in `ontogony-frontend` |
| Mocked E2E — facts | `npx playwright test e2e/kanon-facts.spec.ts` |
| Mocked E2E — plans | `npx playwright test e2e/kanon-plan-workbench.spec.ts` |
| Mocked E2E — bindings | `npx playwright test e2e/kanon-lifecycle.spec.ts` |

## Manual acceptance

After `docker compose build ontogony-frontend kanon-api` in `docker/local-working-system`:

| Action | Expected |
|---|---|
| Source bindings — filter by review status | List updates; URL reflects `reviewStatus` |
| Source bindings — test binding | Test result visible; secrets redacted |
| Facts — resolve | Explanation, policy/status, decision link |
| Plans — compile | Plan steps, bindings, policy checks, decision link |
| Limitation cards | Facts/plans pages state no list/history API |
| Decision provenance | Cross-links open bindings/facts with ontology context |

## Known limitations

- Fact/plan history requires new read contracts or decision-index navigation; not faked from unrelated routes.
- Compile mapper accepts both flat mock payloads and nested `{ plan, decisionRecord }` Kanon responses.
- Source binding deep-link by binding id is route-level until list API supports `bindingId` filter.

## Follow-up

- **KANON-DEEPEN-005** — cross-service semantic links from Allagma/Conexus into Kanon decisions.
- **KANON-DEEPEN-006** — closeout manual QA across the full workbench.
