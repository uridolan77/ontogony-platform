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
| Eval export bundle | Single-eval operator export; no bulk/compliance archive | follow-up |
| Dataset views/bookmarks | No saved workbench or per-operator dataset view presets | follow-up |
| Semantic diff visualization | Baseline comparison remains summary/detail cards; no rich side-by-side semantic diff viewer | follow-up |
| Replay trigger | No live replay trigger until Allagma OpenAPI documents `POST /allagma/v0/runs/{runId}/replay`; FE shows limitation banner (no fake button) | Out of PFH unless backend adds route |
| Replay page fixtures | No `?replayFixture=` catalog on `/allagma/replay`; E2E uses service mocks | follow-up |
| Manual eval write gate | POST eval gated; baseline POST not gated identically | Documented (`ALIGN-PRODUCT-001`, matrix § manual write) |
| Run ↔ eval ids on run GET | May not list `evaluationRunIds` on live run response; run-scoped evaluation list + journey links are the source of truth (`FE-PRODUCT-002`) | follow-up |
| Param routes in release catalog | `/allagma/evaluations/:id` not in `release-route-catalog.json` | `ALIGN-PRODUCT-002` |
| InMemory persistence | Eval data not durable without Postgres mode | Operator docs only |
| Eval artifacts | `artifacts/eval*` gitignored locally | Not a product gap |

## Accepted stance

Document limitations honestly. Do not mask missing capabilities with fake success states.

## FE-PRODUCT-003 outcomes (2026-05-19)

- Replay workbench supports run/trace/decision lookup, evidence export, and evidence journey cross-links to eval/baseline/dataset/Conexus/Kanon/run surfaces.
- Replay trigger remains limitation-only until Allagma OpenAPI documents `POST /allagma/v0/runs/{runId}/replay`.
- No `?replayFixture=` on the replay route; E2E continues to use service mocks.

## EVAL-PRODUCT-005 outcomes (2026-05-19)

- Per-evaluation evidence export via `GET /evaluations/{id}/evidence` with JSON schema validation and FE export panel on eval detail.
- Export is operator-facing locators and summaries — not a compliance archive, bulk export, or embedded Kanon/Conexus payloads.

## FE-PRODUCT-CLOSEOUT-001 outcomes (2026-05-19)

- Product hardening v1 package **closed** — see `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`.
- Scorecard, limitations, and next options published under `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_*`.
- Optional `ALIGN-PRODUCT-002`–`004` pr-specs remain registered but were not in the v1 execution sequence.

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
