# ALLAGMA-ACTION-007 — Closeout and manual QA

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PARTIAL PASS** — implementation + Docker API verified; **browser walkthrough** still pending  
**Package:** `allagma-dotnet/docs/_incoming/Ontogony-Allagma-Actionability-Workbench-Package-v1/`

## Goal

Close the Allagma actionability workbench package (ACTION-000 through ACTION-006) with auditable evidence, honest limitation records, and a repeatable manual QA checklist.

## Deliverables

| Artifact | Path |
| --- | --- |
| Closeout summary | [`docs/releases/ALLAGMA_ACTIONABILITY_WORKBENCH_CLOSEOUT.md`](../releases/ALLAGMA_ACTIONABILITY_WORKBENCH_CLOSEOUT.md) |
| Scorecard | [`docs/releases/ALLAGMA_ACTIONABILITY_WORKBENCH_SCORECARD.md`](../releases/ALLAGMA_ACTIONABILITY_WORKBENCH_SCORECARD.md) |
| Known limitations | [`docs/releases/ALLAGMA_ACTIONABILITY_WORKBENCH_KNOWN_LIMITATIONS.md`](../releases/ALLAGMA_ACTIONABILITY_WORKBENCH_KNOWN_LIMITATIONS.md) |
| Next options | [`docs/releases/ALLAGMA_ACTIONABILITY_WORKBENCH_NEXT_OPTIONS.md`](../releases/ALLAGMA_ACTIONABILITY_WORKBENCH_NEXT_OPTIONS.md) |
| Sequence status | [ALLAGMA_ACTION_SEQUENCE_STATUS.md](./ALLAGMA_ACTION_SEQUENCE_STATUS.md) |
| Browser checklist | `ontogony-frontend/docs/evidence/ALLAGMA_ACTION_007_BROWSER_MANUAL_QA_EVIDENCE.md` |

## Sequence closeout table

| Item | Status | Evidence |
| --- | --- | --- |
| ACTION-000 audit | Done | [000 evidence](./ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT_EVIDENCE.md), [review](../reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md) |
| ACTION-001 start run | Done | [001 evidence](./ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md), [001A](./ALLAGMA_ACTION_001A_START_RUN_CLEANUP_EVIDENCE.md) |
| ACTION-002 human-gate resume | Done | [002 evidence](./ALLAGMA_ACTION_002_HUMAN_GATE_RESUME_WORKBENCH_EVIDENCE.md) |
| ACTION-003 baseline compare | Done | [003 evidence](./ALLAGMA_ACTION_003_BASELINE_COMPARE_ACTIONS_EVIDENCE.md) |
| ACTION-004 audit/evidence export | Done | [004 evidence](./ALLAGMA_ACTION_004_RUN_AUDIT_AND_EVIDENCE_ACTIONS_EVIDENCE.md) |
| ACTION-005 operations design | Done | [005 evidence](./ALLAGMA_ACTION_005_RUN_OPERATIONS_CONTRACT_DESIGN_EVIDENCE.md) |
| ACTION-006 operations v2 | Done (code + unit tests) | [006 evidence](./ALLAGMA_ACTION_006_RUN_OPERATIONS_V2_EVIDENCE.md) |
| ACTION-007 closeout | Done (this file) | — |

## Validation run (2026-05-20)

### Backend (`allagma-dotnet`)

```text
dotnet test --filter RunOperationsServiceTests|AllagmaOpenApiSnapshotTests → 10 passed
```

OpenAPI provenance refreshed: `docs/api/allagma-openapi-v1.provenance.json` (`snapshotSha256=3c9808a0…`).

### Frontend (`ontogony-frontend`)

```text
npx vitest run src/allagma → 172 passed, 1 failed → fixed buildAllagmaReplayEvidence.test.ts (replay now OpenAPI-backed)
npx vitest run src/allagma/lifecycle/allagmaRunOperationsCapability.test.ts \
  src/allagma/components/AllagmaRunOperationsPanel.test.tsx → 5 passed
npx playwright test e2e/allagma-run-operations.spec.ts → 2 passed (mocked APIs)
```

`openApiSnapshotCatalog.test.ts` (Conexus admin paths): **not run as gate** — pre-existing Conexus snapshot drift vs committed catalog.

### Docker-local API manual QA (host → `http://localhost:5083`)

| Step | Result |
| --- | --- |
| Health | PASS |
| Start run (`POST /allagma/v0/runs`) | PASS — `Completed` |
| Inspect run (`GET /runs/{id}`) | PASS |
| Audit export (`GET /runs/{id}/audit`) | PASS |
| Operations contract (`GET /runs/{id}/operations`) | **FAIL** (first pass) — stale pre–006 image |
| Retry/cancel/replay mutations | **FAIL** (first pass) — same |

**Rebuild (2026-05-20, second pass):** `docker compose build allagma-api --no-cache` succeeded (NuGet TLS OK without extra CA on this run). Container recreated.

| Step (post-rebuild) | Result |
| --- | --- |
| `GET /runs/{id}/operations` on failed run | **PASS** — `ontogony-allagma-run-operations-v1`; retry + replay `available=true` |
| ENV-SEED-001 | **PASS** — bootstrap + start runs + baseline compare |
| Start run (before seed) | 500 — unknown Conexus alias until bootstrap |

**Remediation for fresh stacks:** run `seed-and-verify-local-working-system.ps1` before operator QA if Conexus aliases are missing.

### Browser manual QA

**NOT EXECUTED** on this pass. Checklist: `ontogony-frontend/docs/evidence/ALLAGMA_ACTION_007_BROWSER_MANUAL_QA_EVIDENCE.md`.

Frontend provenance probe (prior session): `DOCKER-LOCAL-VERIFY-001` PASS at `http://localhost:5175/`.

## Unsupported operations honesty

UI and capability layer document unavailable/unsupported mutations (no fake buttons):

- `AllagmaRunOperationsPanel` lists unsupported/unavailable ops with reasons.
- `getAllagmaRunOperationsCapability` + `GET …/operations` merge when backend contract is present.
- Deny human gate remains **Kanon-only** (not an Allagma route).
- Promote baseline/eval, bulk export, manual eval write (production) remain absent by design.

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| Actionability v1 PASS or PARTIAL with blockers | **PARTIAL** — blocker: browser QA pending |
| Manual QA recorded | API partial + checklist written |
| Unsupported ops not hidden | PASS (capability + panel patterns) |

## Sign-off criteria

- [x] ACTION-000–006 evidence indexed
- [x] Closeout / scorecard / limitations / next-options docs
- [x] Automated backend + targeted frontend tests pass
- [x] Docker `allagma-api` image includes ACTION-006 routes (rebuild 2026-05-20)
- [ ] Browser walkthrough executed and recorded
