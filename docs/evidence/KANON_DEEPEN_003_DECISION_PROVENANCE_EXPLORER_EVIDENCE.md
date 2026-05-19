# KANON-DEEPEN-003 — Decision provenance explorer

**Status:** Implementation complete — browser walkthrough **not verified in this polish pass**  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)  
**Sequence index:** [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)

## Goal

Make Kanon decision records and provenance the primary operator explanation surface: lookup by decision id, trace id, or entity ref; fingerprints, verify, replay bundles, export, and cross-links to Allagma/Conexus/Kanon semantic routes.

## Implementation commits

| Repo | Commit | Summary |
|---|---|---|
| `ontogony-frontend` | `0f0bde6d` | By-trace list handling, decision verify/replay client support, health status mapping |
| `ontogony-frontend` | `664eb04` | Provenance authority panel, envelope, verify, replay export, e2e mocks/tests |

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | No route changes; existing provenance/replay/verify/index APIs |
| `ontogony-frontend` | Trace index list response; verify/prepare-replay/export clients; authority panel; workbench integration |
| `ontogony-platform` | Evidence + sequence index |

## Source files (frontend)

- `src/kanon/api/kanonClient.ts` — `listKanonDecisionsByTrace`, `verifyKanonDecision`, prepare-replay, replay export
- `src/kanon/api/useKanonDecisionByTrace.ts` — exposes `decisions[]`; `decision` is first-item convenience only
- `src/kanon/adapters/kanonProvenanceAdapters.ts`
- `src/kanon/components/KanonDecisionsProvenanceWorkbench.tsx`
- `src/kanon/components/KanonDecisionAuthorityPanel.tsx`
- `src/kanon/pages/DecisionsProvenancePage.tsx`
- `e2e/kanon-provenance.spec.ts`, `e2e/helpers/mockData.ts`, `e2e/helpers/mockServices.ts`

## Backend routes (verified)

| Route | Purpose |
|---|---|
| `GET /ontology/v0/decision-records/{decisionId}` | Decision metadata |
| `GET /ontology/v0/decision-records/{decisionId}/provenance` | Provenance envelope |
| `POST /ontology/v0/decision-records/{decisionId}/verify` | Integrity check |
| `POST /ontology/v0/decision-records/{decisionId}/prepare-replay` | Prepare replay bundle |
| `GET /ontology/v0/decision-records/{decisionId}/replay-bundles` | Bundle list |
| `GET /ontology/v0/decision-records/{decisionId}/replay-bundles/{bundleId}/export` | Export |
| `GET /ontology/v0/decision-records/by-trace/{traceId}` | Trace index (**list**) |
| `GET /ontology/v0/decision-records/by-entity/{entityRef}` | Entity index |

Read access requires `ProvenanceReader` (or `Auditor` / `Admin` / `System`).

## Closed (003 acceptance)

- By-trace API response handled as **list** (`decisions` array)
- `useKanonDecisionByTrace` exposes `decisions`; `decision` is compatibility convenience (first item only)
- UI does not imply only one decision exists per trace when index returns multiple
- Decision authority panel with provenance envelope fingerprints
- Decision integrity verify (`POST` verify)
- Replay prepare/export client wiring
- Replay bundle export preview/copy/download in workbench
- E2e mocks updated; provenance workbench tests
- Cross-links best-effort from available identifiers with missing-link reasons

## Validation

| Check | Command | Result (2026-05-20 polish pass) |
|---|---|---|
| Provenance adapter tests | `npm test -- src/kanon/adapters/kanonProvenanceAdapters.test.ts` | **4 passed** |
| Kanon client tests | `npm test -- src/kanon/api/kanonClient.test.ts` | **8/9 passed** — 1 pre-existing failure (`browser_request_blocked` vs expected `not_configured` in ontology list error test; unrelated to 003) |
| Mocked E2E | `npx playwright test e2e/kanon-provenance.spec.ts` | **Not run** in this pass |

## Manual browser verification

**Status: NOT VERIFIED** in this polish pass.

| Action | Expected |
|---|---|
| Paste decision id | Record, envelope, verify, replay panel |
| Paste trace id (`mode=trace`) | **Decision chain** lists multiple when index returns multiple |
| Paste entity ref (`mode=entity`) | Decision chain when multiple indexed |
| Verify | Valid/invalid badge with fingerprint detail |
| Replay export | Download/copy when bundle exists; honest message when not |
| Cross-links | Open when ids exist; unavailable reason when missing |

## Known limitations

- **Not** the full Cross-Service Evidence Spine graph resolver.
- Verify/replay routes depend on current Kanon backend contracts; frontend OpenAPI snapshot may lag typed clients.
- Prepare replay is operator-triggered POST (not automatic on load).
- Cross-links to source bindings/facts are route-level until list APIs support deep-link filters.
- Unknown payload previews use operator redaction (`redactUnknownPayload`).

## Follow-up

- **KANON-DEEPEN-004** — facts/plans/bindings explorer (done).
- **KANON-DEEPEN-005** — cross-service semantic links (done).
- **KANON-DEEPEN-006** — closeout browser checklist.
