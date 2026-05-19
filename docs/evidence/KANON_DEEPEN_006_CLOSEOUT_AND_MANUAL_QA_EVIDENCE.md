# KANON-DEEPEN-006 — Closeout and manual QA

**Status:** Closeout documentation complete — **browser walkthrough not executed** in this polish pass  
**Depends on:** KANON-DEEPEN-001 through KANON-DEEPEN-005 evidence  
**Sequence index:** [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)

## Goal

Close the Kanon deepening track (000–006) with auditable evidence, honest test records, a scorecard, known limitations, and a repeatable manual QA checklist for Docker-local operator verification.

## Deliverables

| Artifact | Path |
|---|---|
| Closeout summary | [`docs/releases/KANON_DEEPENING_CLOSEOUT.md`](../releases/KANON_DEEPENING_CLOSEOUT.md) |
| Scorecard | [`docs/releases/KANON_DEEPENING_SCORECARD.md`](../releases/KANON_DEEPENING_SCORECARD.md) |
| Known limitations | [`docs/releases/KANON_DEEPENING_KNOWN_LIMITATIONS.md`](../releases/KANON_DEEPENING_KNOWN_LIMITATIONS.md) |
| Next options | [`docs/releases/KANON_DEEPENING_NEXT_OPTIONS.md`](../releases/KANON_DEEPENING_NEXT_OPTIONS.md) |
| Sequence status | [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md) |
| Frontend checklist copy | `ontogony-frontend/docs/evidence/KANON_DEEPEN_006_BROWSER_MANUAL_QA_EVIDENCE.md` |

## Sequence closeout table

| Item | Status | Evidence |
|---|---|---|
| KANON-DEEPEN-000 | Done | Platform audit |
| KANON-DEEPEN-001 | Done | [001](./KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md) |
| KANON-DEEPEN-002 | Done | [002](./KANON_DEEPEN_002_DOMAIN_PACK_LIFECYCLE_WORKBENCH_EVIDENCE.md) |
| KANON-DEEPEN-003 | Done | [003](./KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md) |
| KANON-DEEPEN-004 | Done (no durable fact/plan history) | [004](./KANON_DEEPEN_004_FACTS_PLANS_BINDINGS_EXPLORER_EVIDENCE.md) |
| KANON-DEEPEN-005 | Done | [005](./KANON_DEEPEN_005_CROSS_SERVICE_SEMANTIC_LINKS_EVIDENCE.md) |
| KANON-DEEPEN-006 | Done (docs/validation) | This file |

## Validation run (2026-05-20 polish pass)

Focused frontend unit tests (Kanon deepening surface):

```text
npm test -- src/kanon/adapters/kanonProvenanceAdapters.test.ts          → 4 passed
npm test -- src/kanon/adapters/kanonCanonicalFactAdapters.test.ts       → 2 passed
npm test -- src/kanon/adapters/kanonSemanticPlanAdapters.test.ts        → 5 passed
npm test -- src/shared/navigation/kanonSemanticAuthorityLinks.test.ts     → 3 passed
npm test -- src/allagma/adapters/buildAllagmaSemanticAuthorityLinkSlots.test.ts → 2 passed
npm test -- src/conexus/components/ConexusObservabilityGuidanceCard.test.ts   → 1 passed
npm test -- src/kanon/api/kanonClient.test.ts                             → 8 passed, 1 failed (pre-existing)
```

E2E (`e2e/kanon-*.spec.ts`): **not run** in this pass (documented in per-item evidence).

`npm run typecheck`: **not clean** in repo (pre-existing unrelated errors); not blocking evidence closeout for docs-only polish.

## Browser verification procedure

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build ontogony-frontend kanon-api
docker compose up -d
.\scripts\verify-frontend-browser-provenance.ps1 -Build   # optional automated probe
```

Manual routes (see frontend 006 checklist):

- `http://localhost:5175/kanon`
- `http://localhost:5175/kanon/domain-packs`
- `http://localhost:5175/kanon/decisions`
- `http://localhost:5175/kanon/facts`
- `http://localhost:5175/kanon/source-bindings`
- `http://localhost:5175/allagma/runs` (run with planning decision)
- `http://localhost:5175/conexus/observability`

**Browser QA result for this pass:** NOT EXECUTED — checklist ready for operator.

## Authorization regression checks (manual)

- [ ] `local-operator` with Auditor/ProvenanceReader can read domain packs and provenance
- [ ] Auditor cannot validate/promote/load domain packs (buttons disabled + explanation)
- [ ] Kanon 403 on domain packs remains role-specific, not mislabeled as “service down”

## Follow-up

Record browser QA results in `KANON_DEEPEN_006_BROWSER_MANUAL_QA_EVIDENCE.md` (frontend) when walkthrough completes; update this file status from **NOT VERIFIED** to **VERIFIED** with date and image digest.
