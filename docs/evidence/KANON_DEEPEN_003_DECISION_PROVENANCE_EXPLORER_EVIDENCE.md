# KANON-DEEPEN-003 — Decision provenance explorer

**Status:** Implementation complete (browser verification after frontend image rebuild)  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md) (local operator read roles)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)

## Goal

Make Kanon decision records and provenance the primary operator explanation surface: lookup by decision id, trace id, or entity ref; show fingerprints, verify, replay bundles, export, and cross-links to Allagma/Conexus/Kanon semantic routes.

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | No route changes; existing provenance/replay/verify/index APIs used as-is |
| `ontogony-frontend` | Trace index fix (`by-trace` list response); verify/prepare-replay/export clients; authority panel; workbench integration |
| `ontogony-platform` | This evidence |

## Backend routes (verified)

| Route | Purpose |
|---|---|
| `GET /ontology/v0/decision-records/{decisionId}` | Decision metadata |
| `GET /ontology/v0/decision-records/{decisionId}/provenance` | Provenance envelope |
| `POST /ontology/v0/decision-records/{decisionId}/verify` | Integrity check |
| `POST /ontology/v0/decision-records/{decisionId}/prepare-replay` | Prepare replay bundle |
| `GET /ontology/v0/decision-records/{decisionId}/replay-bundles` | Bundle list |
| `GET /ontology/v0/decision-records/{decisionId}/replay-bundles/{bundleId}/export` | Signed/hash export |
| `GET /ontology/v0/decision-records/by-trace/{traceId}` | Trace index |
| `GET /ontology/v0/decision-records/by-entity/{entityRef}` | Entity index |

Read access requires `ProvenanceReader` (or `Auditor` / `Admin` / `System`) per Kanon API auth.

## Frontend

`/kanon/decisions` workbench:

1. **Lookup modes** — decision id, trace id, entity ref (URL params: `decisionId`, `traceId`, `entityRef`, `mode`)
2. **Decision chain** — multiple decisions for entity **or trace** index
3. **Authority panel** — provenance envelope fingerprints, verify result, prepare replay, replay export download, semantic cross-links
4. **Provenance trail** — filterable trail + replay bundle summary + operator evidence export (existing)
5. **Correlation** — Allagma/Conexus correlation panel + Allagma replay link by trace

## Validation

| Check | Command |
|---|---|
| Kanon provenance API tests | `dotnet test kanon-dotnet --filter DecisionProvenanceApiTests` |
| Frontend unit tests | `npm test` in `ontogony-frontend` |
| Frontend typecheck | `npm run typecheck` in `ontogony-frontend` |
| Mocked E2E | `npx playwright test e2e/kanon-provenance.spec.ts` in `ontogony-frontend` |

## Manual acceptance

After `docker compose build ontogony-frontend kanon-api` in `docker/local-working-system`:

| Action | Expected |
|---|---|
| Paste decision id | Decision record, envelope, verify, replay panel |
| Paste trace id (`mode=trace`) | Related decisions listed when index returns multiple |
| Paste entity ref (`mode=entity`) | Decision chain when multiple decisions indexed |
| Verify | Valid/invalid badge with fingerprint detail or explicit unavailable |
| Replay export | Download/copy when bundle exists; honest message when not |
| Cross-links | Open links when ids exist; unavailable reason when missing |

## Known limitations

- Frontend OpenAPI snapshot may omit typed verify/export schemas; clients use explicit DTO types until baseline refresh.
- Prepare replay is operator-triggered POST (not automatic on page load).
- Cross-links to source bindings/facts are route-level until list APIs support deep-link filters.

## Follow-up

- **KANON-DEEPEN-004** — facts/plans/bindings explorer graph.
- **KANON-DEEPEN-005** — cross-service semantic links from Allagma/Conexus into Kanon decisions.
