# CONEXUS-DEEPEN-007 — Closeout and manual QA

**Status:** Closeout documentation complete — **browser walkthrough not executed** in this pass  
**Depends on:** CONEXUS-DEEPEN-000 through CONEXUS-DEEPEN-006 evidence  
**Sequence index:** [CONEXUS_DEEPEN_SEQUENCE_STATUS.md](./CONEXUS_DEEPEN_SEQUENCE_STATUS.md)

## Goal

Close the Conexus deepening track (000–007) with auditable evidence, honest test records, a scorecard, known limitations, and a repeatable manual QA checklist for Docker-local operator verification.

## Deliverables

| Artifact | Path |
| --- | --- |
| Closeout summary | [`docs/releases/CONEXUS_DEEPENING_CLOSEOUT.md`](../releases/CONEXUS_DEEPENING_CLOSEOUT.md) |
| Scorecard | [`docs/releases/CONEXUS_DEEPENING_SCORECARD.md`](../releases/CONEXUS_DEEPENING_SCORECARD.md) |
| Known limitations | [`docs/releases/CONEXUS_DEEPENING_KNOWN_LIMITATIONS.md`](../releases/CONEXUS_DEEPENING_KNOWN_LIMITATIONS.md) |
| Next options | [`docs/releases/CONEXUS_DEEPENING_NEXT_OPTIONS.md`](../releases/CONEXUS_DEEPENING_NEXT_OPTIONS.md) |
| Sequence status | [CONEXUS_DEEPEN_SEQUENCE_STATUS.md](./CONEXUS_DEEPEN_SEQUENCE_STATUS.md) |
| Manual checklist (package) | `conexus-dotnet/docs/_incoming/.../checklists/CONEXUS_DEEPEN_MANUAL_OVERVIEW.md` |

## Sequence closeout table

| Item | Status | Evidence |
| --- | --- | --- |
| CONEXUS-DEEPEN-000 | Done | [000](./CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT_EVIDENCE.md) |
| CONEXUS-DEEPEN-001 | Done | [001](./CONEXUS_DEEPEN_001_REQUEST_LIFECYCLE_LIST_EVIDENCE.md) |
| CONEXUS-DEEPEN-002 | Done | [002](./CONEXUS_DEEPEN_002_MODEL_CALL_DETAIL_EVIDENCE.md) |
| CONEXUS-DEEPEN-003 | Done | [003](./CONEXUS_DEEPEN_003_ROUTE_DECISION_EXPLORER_EVIDENCE.md) |
| CONEXUS-DEEPEN-004 | Done | [004](./CONEXUS_DEEPEN_004_USAGE_COST_WORKBENCH_EVIDENCE.md) |
| CONEXUS-DEEPEN-005 | Done | [005](./CONEXUS_DEEPEN_005_CROSS_SERVICE_EVIDENCE_SPINE_EVIDENCE.md) |
| CONEXUS-DEEPEN-006 | Done | [006](./CONEXUS_DEEPEN_006_FRONTEND_OBSERVABILITY_V2_EVIDENCE.md) |
| CONEXUS-DEEPEN-007 | Done (docs/validation) | This file |

## Validation run (2026-05-20 closeout pass)

### Backend (`conexus-dotnet`)

```text
dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~ModelCall
  → Passed: 12

dotnet test tests/Conexus.Application.Tests \
  --filter "FullyQualifiedName~ModelCallEvidence|FullyQualifiedName~RouteDecisionDetail|FullyQualifiedName~UsageCost"
  → Passed: 8
```

### Frontend (`ontogony-frontend`)

```text
npx vitest run src/conexus --reporter=dot
  → 20 files, 61 tests passed

e2e/conexus-observability.spec.ts
  → Updated for tabbed observability (mocked APIs); not re-run in this pass
```

`npm run typecheck`: **not clean** repo-wide (pre-existing errors outside conexus, e.g. Allagma start-run); conexus-focused vitest suite passes.

## Manual QA checklist

**Browser result for this pass:** NOT EXECUTED — checklist ready for operator.

### Environment setup

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build ontogony-frontend conexus-api
docker compose up -d
.\scripts\seed-and-verify-local-working-system.ps1
```

Configure operator settings (frontend): Conexus base URL, admin API key, project id, project API key (match bootstrap / seed report).

Open: `http://localhost:5175/conexus/observability` (adjust host/port to compose mapping).

### Traffic generation

| Step | Action | Expected |
| --- | --- | --- |
| 1 | Fake-provider chat completion (`/conexus/chat` or bootstrap + completion) | New row in **Recent requests** |
| 2 | Provider error / fallback (if harness available) | Failed or fallback-visible status; attempts in Lookup detail |
| 3 | Real-provider request (only if local keys intentionally enabled) | Row appears; provider key differs from `fake` |

### Workbench verification

| Check | Pass criteria |
| --- | --- |
| Recent requests visible | Default tab shows table after traffic |
| Request filters work | Project/status filters change rows; clear drill-down resets |
| Request detail opens | **Open** on row → **Lookup** tab with model-call detail |
| Route decision opens | **Route** on row → **Route decisions** tab with explorer |
| Provider attempts visible | Lookup detail shows attempt rows or honest missing copy |
| Tokens/cost visible or explained | Summary tokens/cost or `tokenUsageAvailable` / coverage note |
| Usage window links to rows | **Usage / cost** → drill-down → **Recent requests** with banner |
| Cross-service links work | Evidence spine + correlation links when ids exist |
| Evidence export | Model-call bundle copy/download OR clear limitation text |
| Empty states useful | In-memory / no telemetry / no filters banners when applicable |
| Diagnostics still visible | **Diagnostics** tab shows summary + limitations card |

Package checklist mirror:

- [ ] Recent requests visible
- [ ] Request filters work
- [ ] Request detail opens
- [ ] Route decision opens
- [ ] Provider attempts visible
- [ ] Tokens/cost visible or missing explained
- [ ] Usage window links to request rows
- [ ] Cross-service links work
- [ ] Evidence export works or limitation clear
- [ ] Empty states useful
- [ ] Diagnostics still visible

## Score dimensions (summary)

See full table in [`CONEXUS_DEEPENING_SCORECARD.md`](../releases/CONEXUS_DEEPENING_SCORECARD.md). Overall documentation closeout: **4/5** with browser verification pending.

## Follow-up

When browser QA completes:

1. Check all boxes in the manual checklist above.
2. Record date, compose image digests, and any screenshots in a short note (optional: `ontogony-frontend/docs/evidence/CONEXUS_DEEPEN_007_BROWSER_MANUAL_QA_EVIDENCE.md`).
3. Update this file status from **NOT EXECUTED** to **VERIFIED** with UTC date.
