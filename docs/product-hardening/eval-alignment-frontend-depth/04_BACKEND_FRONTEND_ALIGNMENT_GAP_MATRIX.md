# 04 — Backend / Frontend Alignment Gap Matrix

Use this as the template for `ALIGN-PRODUCT-001`.

| Capability | Backend route | OpenAPI | Generated client | Hook | Adapter | UI route/page | Fixture mode | Tests | Evidence |
|---|---|---|---|---|---|---|---|---|---|
| Eval dashboard list | TBD | TBD | TBD | TBD | TBD | `/allagma/evaluations` | `dashboardFixture=ci-suite` | adapter + Playwright | TBD |
| Eval detail | TBD | TBD | TBD | TBD | TBD | eval detail route | eval fixture if supported | adapter + route | TBD |
| Per-run evals | Existing or TBD | TBD | TBD | TBD | TBD | run detail/evals | fixture topology/eval | adapter + e2e | TBD |
| Baseline comparison | Existing create/fetch | TBD | TBD | TBD | TBD | comparison route | fixture if supported | e2e + unit | TBD |
| Scenario dataset | TBD | TBD | TBD | TBD | TBD | dashboard filters/detail | catalog fixtures | unit + e2e | TBD |
| Quality scoring | Existing DTOs or TBD | TBD | TBD | TBD | TBD | score breakdown panel | fixture | unit | TBD |
| Replay evidence | Existing evidence routes/limitations | TBD | TBD | TBD | TBD | replay evidence page | replay fixtures | replay checks/e2e | TBD |
| Trace correlation | Existing proof scripts/docs | TBD | TBD | TBD | TBD | run/cross-service links | fixture/live | e2e mocked | TRACE evidence |
| Kanon topology decision | Existing decision-record route | TBD | TBD | TBD | TBD | topology panel/run detail | topology fixture | KANON OP scripts | KANON evidence |
| Conexus route evidence | Existing admin evidence | TBD | TBD | TBD | TBD | cross-service links | maybe live only | scripts | CONEXUS evidence |

A surface is aligned only when route, OpenAPI status, frontend wrapper/hook, adapter, UI state, fixture/live behavior, test coverage, and evidence are all mapped.
