# 08 — Known Limitations

**Audit:** PFH-001 (2026-05-19). Updated with audited facts.

## Program boundary

- Not production readiness.
- Not real provider mode.
- Not cloud deployment.
- Not identity/TLS/secrets/DR.
- Not a replacement for staging or load testing.

## Eval/product limitations (confirmed)

| Limitation | Detail | PFH track |
| --- | --- | --- |
| Global eval list filters | `datasetId` / `baselineComparisonId` depend on evaluation metadata (sparse for manual writes) | Documented in filter UI (`FE-PRODUCT-001`) |
| Dashboard list limit | Live dashboard requests default limit 100; cursor via URL `cursor` param | `Load next page` in FE-PRODUCT-001; full history still deferred |
| Baseline comparison history depth | List/history route exists with cursor + filters; no saved views or pinned workbench queries | follow-up |
| Baseline create in UI | POST exists; harness/smoke only — no operator form | Documented (`ALIGN-PRODUCT-001`) |
| Scenario datasets | Read-only dataset index/detail now available; no dataset authoring workflow | keep read-only by design |
| Judge/scoring depth | Quality metadata surfaced, but no calibration history/trend controls | follow-up after `EVAL-PRODUCT-004` |
| Eval export bundle | No coherent product artifact | `EVAL-PRODUCT-005` |
| Dataset views/bookmarks | No saved workbench or per-operator dataset view presets | follow-up |
| Semantic diff visualization | Baseline comparison remains summary/detail cards; no rich side-by-side semantic diff viewer | follow-up |
| Replay trigger | Allagma retry/cancel/replay POST not in OpenAPI; FE limitation UI | Out of PFH unless backend adds routes |
| Replay page fixtures | No `?replayFixture=`; E2E uses service mocks | follow-up |
| Manual eval write gate | POST eval gated; baseline POST not gated identically | Documented (`ALIGN-PRODUCT-001`, matrix § manual write) |
| Run ↔ eval ids on run GET | May not list `evaluationRunIds` on live run response | `FE-PRODUCT-002` |
| Param routes in release catalog | `/allagma/evaluations/:id` not in `release-route-catalog.json` | `ALIGN-PRODUCT-002` |
| InMemory persistence | Eval data not durable without Postgres mode | Operator docs only |
| Eval artifacts | `artifacts/eval*` gitignored locally | Not a product gap |

## Accepted stance

Document limitations honestly. Do not mask missing capabilities with fake success states.

## EVAL-POLISH-001 outcomes (2026-05-19)

- Baseline list query semantics are aligned across persistence modes for `promotionRecommendation`.
- Baseline list invalid/continuation cursor behavior is explicitly covered in backend API tests.
- Frontend evaluation/baseline/dataset detail routes now preserve degraded/auth error states instead of collapsing all non-OK responses into null/not-found.
- Eval quality detail now uses tri-state copy for judge metadata (`true`/`false`/unknown).

## Separate from PFH (production-readiness)

- Real Conexus provider keys and cost
- Cloud manifests and runtime Vite injection
- Staging/prod identity and SLOs
- Branch protection requiring aggregate checks (optional manual follow-up from `CI-COST-001`)
