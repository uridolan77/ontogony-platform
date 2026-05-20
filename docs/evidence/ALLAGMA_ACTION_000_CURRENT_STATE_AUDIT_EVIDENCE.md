# ALLAGMA-ACTION-000 — Current state audit evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (audit complete; implementation not started)  
**Statement:** Docs-only actionability baseline for Allagma operator mutations. No `src/` changes in any repo for this step.

## Scope

`ALLAGMA-ACTION-000` from `Ontogony-Allagma-Actionability-Workbench-Package-v1` (intake under `allagma-dotnet/docs/_incoming/`). Delivers backend route inventory, OpenAPI alignment, frontend action inventory, capability matrix, and explicit unsupported-operation list.

## Delivered

| Repo | Artifact |
| --- | --- |
| `ontogony-platform` | [`docs/reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md`](../reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md) — full matrix and sequencing |
| `ontogony-platform` | This evidence file |

## Audit conclusions (summary)

```text
Backend actionable routes (confirmed):  POST runs, POST resume, POST run evaluations (gated),
                                        POST baseline-comparisons, GET audit, GET eval evidence
Frontend wired today:                 resume (run detail + gates), eval evidence export,
                                        audit bundle consumed in evidence sections (no dedicated export CTA)
Frontend missing clients:             start run, create baseline comparison, write evaluation
Unsupported HTTP routes:              retry, cancel, replay trigger, promote, bulk export
Deny human gate:                      Kanon API only; Allagma resume applies deny when Kanon returns deny
Next slice:                           ALLAGMA-ACTION-001 (start run workbench)
```

## Key file references (verification)

| Check | Path |
| --- | --- |
| Endpoint map | `allagma-dotnet/src/Allagma.Api/Program.cs` |
| OpenAPI snapshot | `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` |
| OpenAPI provenance | `allagma-dotnet/docs/api/allagma-openapi-v1.provenance.json` |
| OpenAPI drift tests | `allagma-dotnet/tests/Allagma.Tests/AllagmaOpenApiSnapshotTests.cs` |
| Run status enum | `allagma-dotnet/src/Allagma.Domain/RunStatus.cs` |
| Resume gating | `allagma-dotnet/src/Allagma.Application/ResumeAgentRunService.cs` |
| Manual eval policy | `Program.cs` (`ManualWriteEnabled`, production guard) |
| Frontend HTTP client | `ontogony-frontend/src/allagma/api/allagmaClient.ts` |
| Frontend mutations | `ontogony-frontend/src/allagma/api/allagmaMutations.ts` |
| Run ops capability | `ontogony-frontend/src/allagma/lifecycle/allagmaRunOperationsCapability.ts` |
| Evidence capability | `ontogony-frontend/src/allagma/lifecycle/allagmaEvidenceCapability.ts` |
| Route catalog | `ontogony-frontend/src/app/route-workflow-catalog.json` |
| Stale backend-waiting (start run) | `ontogony-frontend/docs/phase-j-frontend-ui-tightening/backend-waiting/allagma-start-run.md` |
| Package roadmap | `allagma-dotnet/docs/_incoming/.../03_ACTIONABILITY_ROADMAP.md` |

## Confirmed backend state

- **15** operator operations under `/allagma/v0` in `Program.cs` (see review §1 table).
- OpenAPI snapshot lists the same 12 path templates; CI asserts eval, baseline, audit, capabilities schemas.
- `POST /runs` accepts idempotency headers; conflicts return 409 with `CrossServiceErrorEnvelope`.
- `POST /runs/{runId}/resume` returns 409 body for `not_waiting`; requires `RunStatus.WaitingForHumanGate`.
- No `MapPost` for `/retry`, `/cancel`, `/replay` under `/allagma/v0`.

## Confirmed frontend state

| Surface | POST/Action wired? |
| --- | --- |
| `allagmaClient.ts` | `resumeAllagmaRun` only (no `startAllagmaRun`, no baseline POST, no eval POST) |
| `AllagmaRunOperationsPanel` | Resume button when `waiting_for_human` / `blocked` |
| `HumanGatesPage` | Kanon resolve + resume |
| `EvaluationRunDetailPage` | Eval evidence export panel |
| `BaselineComparisonWorkbenchPage` | List/read only; UI states harness POST |
| `AllagmaOverviewPage` | `startRunEnabled: false` |
| `ReplayEvidencePage` | Lookup + client export; no replay trigger API |

## Unsupported operations (explicit)

| Operation | Route searched | Result |
| --- | --- | --- |
| Retry run | `POST /allagma/v0/runs/{runId}/retry` | Not found |
| Cancel run | `POST /allagma/v0/runs/{runId}/cancel` | Not found |
| Replay trigger | `POST /allagma/v0/runs/{runId}/replay` | Not found |
| Deny human gate (Allagma) | Allagma deny endpoint | Not found; use Kanon |
| Bulk export | bulk/export paths | Not found |
| Promote baseline/eval | promote mutation | Not found; `promotionRecommendation` is data-only |

## Validation performed

```text
- Read Allagma.Api Program.cs end-to-end for /allagma/v0 mappings
- Grep OpenAPI snapshot paths vs Program.cs
- Grep allagma-dotnet src for retry/cancel/replay/promote HTTP routes (none)
- Inventoried ontogony-frontend allagma client, mutations, pages, route-workflow-catalog
- Cross-checked package 02_CURRENT_STATE_FINDINGS.md and 05_OPERATION_CONTRACT_MATRIX.md
- Confirmed RunStatus.Cancelled exists without cancel API
```

## Next step

Proceed with **ALLAGMA-ACTION-001** per `allagma-dotnet/docs/_incoming/.../prompts/ALLAGMA-ACTION-001_START_RUN_WORKBENCH.md`: add `startAllagmaRun` to `allagmaClient.ts`, operator form on overview/runs, idempotency key UX, and retire stale backend-waiting copy for start-run.
