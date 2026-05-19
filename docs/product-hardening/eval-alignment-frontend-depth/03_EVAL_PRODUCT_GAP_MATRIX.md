# 03 — Eval Product Gap Matrix

| Area | Current likely state | Product gap | Priority | Not in scope |
|---|---|---|---|---|
| Eval routes | Existing Allagma eval routes and API tests exist. | Need current route inventory and formal product semantics. | Must audit | Production API gateway/security. |
| Eval list/query | Frontend has used per-run list + sampling for dashboard. | Need explicit dashboard/list contract or documented limitation. | Must fix | Real analytics warehouse. |
| Eval dashboard | Hardening exists; product depth limited. | Filters, status, suite/dataset dimensions, comparison entry points. | High | Full BI dashboard. |
| Baseline comparison | Create/fetch evidence exists. | History, filters, drilldown, compare semantics, exported result model. | High | Enterprise reporting. |
| Scenario dataset | Dataset/CI matrix exists historically. | Dataset index, scenario labels, suite membership, fixture/live parity. | High | Full dataset authoring UI unless scoped. |
| Quality scoring | Judge/scoring scaffolding exists. | Score breakdown, confidence, calibration metadata, limitation language. | High | Training new judge models. |
| Eval evidence | Individual evidence exists. | Operator export bundle across run/eval/comparison/dataset. | Medium | Compliance archive system. |
| Replay relation | Replay evidence and limitation banners exist. | Clear relation between replay, eval, run, comparison, and route evidence. | Medium | Live replay trigger if backend does not support it. |
| Persistence | Docker-local durable evidence exists. | Product-level retention/query behavior needs clarity. | Medium | DR/backup/SLO. |
| OpenAPI | Snapshots/generation discipline exists. | New eval/product semantics must be contract-first. | Must | Ad hoc undocumented routes. |

## Must-fix before frontend product depth

1. Determine whether a global eval list/query route exists or must be added/formalized.
2. Define dashboard data model.
3. Define live vs fixture behavior for every eval surface.
4. Document limitations that remain intentionally deferred.
