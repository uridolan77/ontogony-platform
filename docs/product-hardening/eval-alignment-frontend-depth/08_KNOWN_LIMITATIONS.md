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
| Global eval list filters | `datasetId` / `baselineComparisonId` depend on evaluation metadata (sparse for manual writes) | Clarify in `FE-PRODUCT-001` |
| Dashboard list limit | Live dashboard requests default limit 100; not full history | `FE-PRODUCT-001` |
| Baseline comparison history | GET-by-id only in UI; no list/filter route | `EVAL-PRODUCT-002` |
| Baseline create in UI | POST exists; harness/smoke only — no operator form | Document or defer in `ALIGN-PRODUCT-001` |
| Scenario datasets | Strong in `docs/evals/datasets/`; weak HTTP/UI index | `EVAL-PRODUCT-003` |
| Judge/scoring depth | Scaffold exists; shallow calibration/confidence in UI | `EVAL-PRODUCT-004` |
| Eval export bundle | No coherent product artifact | `EVAL-PRODUCT-005` |
| Replay trigger | Allagma retry/cancel/replay POST not in OpenAPI; FE limitation UI | Out of PFH unless backend adds routes |
| Replay page fixtures | No `?replayFixture=`; E2E uses service mocks | `FE-PRODUCT-003` may add catalog |
| Manual eval write gate | POST eval gated; baseline POST not gated identically | Document in `ALIGN-PRODUCT-001` |
| Run ↔ eval ids on run GET | May not list `evaluationRunIds` on live run response | `FE-PRODUCT-002` |
| Param routes in release catalog | `/allagma/evaluations/:id` not in `release-route-catalog.json` | `ALIGN-PRODUCT-002` |
| InMemory persistence | Eval data not durable without Postgres mode | Operator docs only |
| Eval artifacts | `artifacts/eval*` gitignored locally | Not a product gap |

## Accepted stance

Document limitations honestly. Do not mask missing capabilities with fake success states.

## Separate from PFH (production-readiness)

- Real Conexus provider keys and cost
- Cloud manifests and runtime Vite injection
- Staging/prod identity and SLOs
- Branch protection requiring aggregate checks (optional manual follow-up from `CI-COST-001`)
