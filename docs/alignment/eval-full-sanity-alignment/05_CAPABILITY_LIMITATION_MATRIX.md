# Capability / limitation matrix — eval loop

Legend: **Live** = available in non-fixture operator UI with real API. **Fixture** = labelled fixture only. **Backend** = API/harness only. **Missing** = not implemented.

## Allagma eval HTTP

| Capability | Backend | Frontend | Notes |
| --- | --- | --- | --- |
| List evals per run | Live | Live (run detail + dashboard sample) | Newest-first ordering |
| Get evaluation by id | Live | Live | Detail page |
| Get baseline comparison | Live | Live | Comparison detail page |
| Global eval list | Missing | Missing | Documented in limitations card |
| Cross-run eval trend API | Missing | Fixture/sample | Trend panel uses sample ordering |
| Manual POST evaluation | Backend (gated) | Missing | `ManualWriteEnabled` + non-prod |
| POST baseline comparison | Backend | Missing | Harness/smoke only |
| Durable eval persistence | Live (Postgres mode) | Live | InMemory for local dev |
| Quality score + judges | Live (deterministic) | Live | LLM judge advisory only in profile |
| Scenario dataset v0 | Live (loader/CI) | Fixture banner only | No dataset editor UI |

## Dashboard and operator UX

| Capability | Status | Evidence |
| --- | --- | --- |
| Overview cards (pass/fail/quality) | Live / Fixture | FE-EVAL-002 |
| Scenario matrix | Live / Fixture | `AllagmaEvalScenarioMatrix` |
| Trend sample | Live / Fixture | Not durable time-series |
| Fixture/live banners | Live | FE-POLISH-001 |
| Metric redaction | Live | `safeDisplayText` + `redactOperatorText` |
| Error code display | Live | `ProductLiveQueryState` |
| E2E empty/degraded paths | Live | `allagma-eval-dashboards.spec.ts` |

## Cross-service (full sanity scope)

| Capability | Service | Frontend visibility |
| --- | --- | --- |
| Topology authorization | Kanon | Run eval topology section |
| Route/model call evidence | Conexus | Run events / route evidence panels |
| Governed run execution | Allagma | Run detail, audit |
| Eval harness deterministic output | Allagma (scripts) | Adapter tests + smoke report |

## Safety gates (unchanged)

| Gate | Enforced |
| --- | --- |
| Real external execution disabled | Yes (harness + ops config) |
| No raw prompts/completions in eval DTOs/UI | Yes |
| Fixture never presented as live | Yes (`sourceKind`, banners) |
| Manual eval write gated | Yes |

## What would remove a limitation banner

| Limitation | Required backend change |
| --- | --- |
| No global eval list | New `GET /evaluations` with paging/filter contract |
| Trend is sample only | Durable trend query or export API |
| No UI baseline create | POST comparison operator workflow + auth policy |
| Run topology missing eval ids | Populate `evaluationRunIds` / `baselineComparisonId` on run topology summary |
