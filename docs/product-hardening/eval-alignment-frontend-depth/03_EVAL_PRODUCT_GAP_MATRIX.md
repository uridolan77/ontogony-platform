# 03 — Eval Product Gap Matrix

**Audit:** PFH-001 (2026-05-19). Priority: **P0** = blocks FE product depth; **P1** = high; **P2** = medium.

| Area | Current state (audited) | Product gap | Priority | Target PR | Not in scope |
| --- | --- | --- | --- | --- | --- |
| Eval list/query | **Closed (EVAL-PRODUCT-001):** `GET /allagma/v0/evaluations` + summary/page DTOs; per-run list retained | Saved views, export | **P2** | — | Analytics warehouse |
| Eval dashboard | **Closed (FE-PRODUCT-001):** filters, dimensions, comparison entries, results list | Saved views, richer pagination | **P2** | — | Full BI dashboard |
| Eval detail | `GET /evaluations/{id}` + FE detail page | Richer verdict/quality/metric presentation; correlation IDs | **P1** | `FE-PRODUCT-002` | — |
| Baseline comparison | **Closed (EVAL-PRODUCT-002):** list/history route + FE workbench + detail drilldown links | Create flow still harness-only; richer diff visualization | **P2** | — | Enterprise reporting |
| Scenario dataset | **Closed (EVAL-PRODUCT-003):** `GET /allagma/v0/evaluation-datasets`, detail route, FE dataset surface, metadata-backed labels | Saved views/bookmarks, richer dataset analytics | **P2** | — | Full dataset authoring UI (unless scoped) |
| Quality scoring | **Closed (EVAL-PRODUCT-004):** quality/judge metadata contract exposed in eval list/detail; FE detail shows calibration metadata + advisory limits | Calibration history trends and operator tuning workflows | **P2** | — | Training judge models |
| Eval evidence export | Audit export on run; no eval bundle route | Operator export bundle (run/eval/comparison/dataset/trace) | **P2** | `EVAL-PRODUCT-005` | Compliance archive |
| Replay relation | Replay page + Kanon bundles; limitation banners | Cross-links eval ↔ run ↔ comparison ↔ route evidence | **P2** | `FE-PRODUCT-003` | Live replay trigger (no backend route) |
| Persistence | Postgres + in-memory modes; `EVAL_DUR_001` done | Product retention/query semantics for operators | **P2** | (clarify in EVAL-PRODUCT-001) | DR/backup/SLO |
| OpenAPI discipline | `allagma-openapi-v1.snapshot.json` + `openapi:check` | New eval semantics must update snapshot before FE hooks | **P0** | `EVAL-PRODUCT-001`, `ALIGN-PRODUCT-003` | Ad hoc routes |
| Manual eval write | POST gated; baseline POST not gated same way | Documented in alignment matrix; no operator write UI | **P2** | — (closed ALIGN-PRODUCT-001) | — |
| Run ↔ eval linkage | Live run GET may not surface `evaluationRunIds` | Surface ids on run detail or document lookup-only | **P1** | `FE-PRODUCT-002` | — |

## Must-fix before frontend product depth (unchanged, now evidenced)

1. ~~**Decide** global eval list/query~~ — **Done:** Option A (`GET /allagma/v0/evaluations`) in EVAL-PRODUCT-001.
2. ~~**Define** dashboard data model~~ — **Done:** summary DTO + FE `mapEvaluationSummaryDto` / `useAllagmaEvalDashboard`.
3. **Define** fixture vs live behavior per eval surface (already partially in `FRONTEND_FIXTURE_LIVE_BOUNDARY.md`; extend for new fields).
4. **Document** limitations that remain intentionally deferred (`08_KNOWN_LIMITATIONS.md`).

## Product gap priority list (implementation order)

```text
1. EVAL-PRODUCT-001  — list/query contract + dashboard data model
2. ALIGN-PRODUCT-001 — contract matrix refresh (after backend contract lands or limitation locked)
3. FE-PRODUCT-001    — eval dashboard v2
4. EVAL-PRODUCT-002  — baseline comparison depth (**done**)
5. EVAL-PRODUCT-003  — scenario dataset surfaces (**done**)
6. EVAL-PRODUCT-004  — quality scoring / judge calibration UI (**done**)
7. FE-PRODUCT-002    — run detail evidence depth (**next**)
8. FE-PRODUCT-003    — replay evidence workbench
9. EVAL-PRODUCT-005  — eval evidence export bundle
10. FE-PRODUCT-CLOSEOUT-001
```
