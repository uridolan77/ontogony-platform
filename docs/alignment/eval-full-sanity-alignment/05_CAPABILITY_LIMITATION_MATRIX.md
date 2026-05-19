# Capability / limitation matrix — eval loop

Legend: **Live** = available in non-fixture operator UI with real API. **Fixture** = labelled fixture only. **Backend** = API/harness only. **Missing** = not implemented.

**Refreshed:** ALIGN-PRODUCT-001 (2026-05-19).

## Allagma eval HTTP

| Capability | Backend | Frontend | Notes |
| --- | --- | --- | --- |
| Global eval list | Live | Live | `GET /allagma/v0/evaluations`; default limit 100 |
| List evals per run | Live | Live (run detail) | Newest-first ordering |
| Get evaluation by id | Live | Live | Detail page |
| Get baseline comparison | Live | Live | Comparison detail page |
| Baseline comparison list/history | Missing | Missing | GET-by-id only → `EVAL-PRODUCT-002` |
| Cross-run eval trend API | Missing | List-page ordering only | Trend panel uses current list page, not time-series |
| Manual POST evaluation | Backend (gated) | Missing | `ManualWriteEnabled` + non-prod |
| POST baseline comparison | Backend (ungated) | Missing | Harness/smoke only; asymmetry documented |
| Durable eval persistence | Live (Postgres mode) | Live | InMemory for local dev |
| Quality score + judges | Live (deterministic) | Live | LLM judge advisory only in profile |
| Scenario dataset v0 | Live (loader/CI) | Fixture banner only | No dataset editor UI |
| Eval export bundle | Missing | Missing | `EVAL-PRODUCT-005` |

## Dashboard and operator UX

| Capability | Status | Evidence |
| --- | --- | --- |
| Overview cards (pass/fail/quality) | Live / Fixture | FE-EVAL-002, EVAL-PRODUCT-001 |
| Scenario matrix | Live / Fixture | `AllagmaEvalScenarioMatrix` |
| Trend (list page) | Live / Fixture | Not durable time-series |
| Fixture/live banners | Live | FE-POLISH-001, ALIGN-PRODUCT-001 |
| Metric redaction | Live | `safeDisplayText` + `redactOperatorText` |
| Error code display | Live | `ProductLiveQueryState` |
| E2E empty/degraded paths | Live | `allagma-eval-dashboards.spec.ts` |

## Cross-service (full sanity scope)

| Capability | Service | Frontend visibility |
| --- | --- | --- |
| Topology authorization | Kanon | Run eval topology section |
| Route/model call evidence | Conexus | Run events / route evidence panels |
| Governed run execution | Allagma | Run detail, audit |
| Replay bundles | Kanon | `/allagma/replay`, provenance workbench |
| Trace correlation | Platform headers | Run detail, replay, system overview |
| Eval harness deterministic output | Allagma (scripts) | Adapter tests + smoke report |

## Safety gates (unchanged)

| Gate | Enforced |
| --- | --- |
| Real external execution disabled | Yes (harness + ops config) |
| No raw prompts/completions in eval DTOs/UI | Yes |
| Fixture never presented as live | Yes (`sourceKind`, banners) |
| Manual eval write gated | Yes |

## What would remove a limitation banner

| Limitation | Required change |
| --- | --- |
| List page not full history | Higher limit, cursor UX, or export (`FE-PRODUCT-001`, `EVAL-PRODUCT-005`) |
| Trend is list-page only | Durable trend query or export API |
| No UI baseline create | POST comparison operator workflow + auth policy |
| No baseline list | `GET /evaluations/baseline-comparisons` or documented deferral (`EVAL-PRODUCT-002`) |
| Run topology missing eval ids | Populate `evaluationRunIds` on run topology summary (`FE-PRODUCT-002`) |
