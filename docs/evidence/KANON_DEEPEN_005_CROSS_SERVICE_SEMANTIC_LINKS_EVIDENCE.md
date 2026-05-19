# KANON-DEEPEN-005 — Cross-service semantic links

**Status:** Implementation complete (browser verification after frontend image rebuild)  
**Depends on:** [KANON-DEEPEN-003](KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md), [KANON-DEEPEN-004](KANON_DEEPEN_004_FACTS_PLANS_BINDINGS_EXPLORER_EVIDENCE.md)

## Goal

Make Kanon semantic authority visible from Allagma and Conexus operator workflows with honest unavailable copy when identifiers are absent.

## ID inventory (Allagma run GET / correlation)

| Identifier | Typical source | Kanon link target |
|---|---|---|
| `planningDecisionId` | Run GET, correlation | `/kanon/decisions?decisionId=…` |
| `actionEvaluationDecisionId` | Run events / audit `kanonDecisions` (not on run GET) | Same; slot documents gap |
| `topologyAuthorizationDecisionId` | Audit topology evidence | Same |
| `ontologyVersionId` | Run GET | `/kanon/ontologies?versionId=…` |
| `domainPackId` / version | Not on run GET today | `/kanon/domain-packs?packId=…` when present |
| `traceId` | Run GET, correlation | `/kanon/decisions?mode=trace&traceId=…` |
| `humanGateId` | Pause events / correlation | Allagma gates + Kanon decision when linked |
| `modelCallId` | Run GET, eval metrics | Conexus observability (not Kanon) |

Conexus execution-run lookup surfaces `traceId`; correlation resolves Allagma run and Kanon decision when APIs return them.

## Repos touched

| Repo | Change |
|---|---|
| `ontogony-frontend` | Shared link builders, Allagma semantic authority cards, Conexus Kanon guidance, correlation href updates |
| `ontogony-platform` | This evidence |

No backend DTO changes: identifiers already exist on Allagma run GET or audit bundles; gaps are documented in UI copy rather than fabricated links.

## Frontend

| Surface | Component / module |
|---|---|
| Link matrix | `src/shared/navigation/kanonSemanticAuthorityLinks.ts` |
| Allagma run detail | `AllagmaSemanticAuthorityLinksCard` + `buildAllagmaSemanticAuthorityLinkSlots` |
| Allagma eval detail | `Kanon semantic evidence` card (subset of authority slots) |
| Conexus observability | `ConexusObservabilityGuidanceCard` Kanon section; execution-run chips use trace/decision hrefs |
| Correlation spine | `correlationAdapters` — trace and action-evaluation decision slots link to Kanon |

## Validation

| Check | Command |
|---|---|
| Link builder tests | `npm test -- src/shared/navigation/kanonSemanticAuthorityLinks.test.ts` |
| Authority slot tests | `npm test -- src/allagma/adapters/buildAllagmaSemanticAuthorityLinkSlots.test.ts` |
| Full unit suite | `npm test` in `ontogony-frontend` |
| Typecheck | `npm run typecheck` in `ontogony-frontend` |

## Manual acceptance

After `docker compose build ontogony-frontend` in `docker/local-working-system`:

| Action | Expected |
|---|---|
| Allagma run with `planningDecisionId` | Semantic authority card shows planning + ontology links; Open in Kanon works |
| Allagma run without action eval id | Action evaluation slot unavailable with explicit reason |
| Eval run detail | Kanon semantic evidence card mirrors correlation decisions |
| Conexus observability | Guidance explains trace vs decision lookup; execution run links Kanon when trace/decision resolved |

## Known limitations

- `actionEvaluationDecisionId` and `domainPackId` are not on Allagma run GET; operators use audit topology/sandbox sections or Kanon decision lookup.
- Domain pack deep-link uses `packId` query; workbench selection-by-URL is best-effort until pack list filter API exists.

## Follow-up

- **KANON-DEEPEN-006** — closeout manual QA across the full Kanon workbench.
