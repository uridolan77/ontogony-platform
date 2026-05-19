# 05 — Frontend Operator Depth Gap Matrix

**Audit:** PFH-001 (2026-05-19).

| Surface | Current base (audited) | Product-depth gap | Suggested PR |
| --- | --- | --- | --- |
| Eval dashboard | `EvaluationsOverviewPage` — cards, scenario matrix, trend panel, limitations card, live sample banner; fixture `ci-suite` | Filters (status, verdict, date), suite/dataset dimensions, comparison entry points, clear list semantics after EVAL-PRODUCT-001 | `FE-PRODUCT-001` (after `EVAL-PRODUCT-001`) |
| Eval run detail | `EvaluationRunDetailPage` — quality panel, fixture/live banners | Deeper metric/score breakdown, correlation links to run/trace/decision | `FE-PRODUCT-002` |
| Run detail | `AllagmaRunEvalTopologyEvidenceSection` — eval + topology; link to evaluations | Integrated journey: run → evals → topology → Conexus route → replay | `FE-PRODUCT-002` |
| Replay evidence | `ReplayEvidencePage` + `AllagmaReplayEvidenceWorkbench` — run/trace/decision modes; `@ontogony/ui` panels | Workbench lookup UX, limitation-aware actions, export, cross-links to eval/comparison | `FE-PRODUCT-003` |
| Baseline comparison | `BaselineComparisonDetailPage` — GET detail only; fixture baseline | History, drilldown, diff emphasis, create flow or explicit “harness only” | `EVAL-PRODUCT-002` + FE |
| Scenario datasets | Dashboard matrix from fixture; CI dataset in repo | Dataset index labels, suite membership visible in live mode | `EVAL-PRODUCT-003` |
| Quality scoring | `AllagmaEvalQualityScoreDetail` on detail page | Confidence, calibration, judge metadata, limitation wording | `EVAL-PRODUCT-004` |
| Route catalogs | `release-route-catalog.json` — static `/allagma/evaluations`, `/allagma/replay` only | Add param routes or document intentional omission | `ALIGN-PRODUCT-002` |
| Config / fixture ops | `FRONTEND_FIXTURE_LIVE_BOUNDARY.md`, `fixtures:check`, `config:check` | Extend catalogs when new eval query params land | `FE-HYGIENE` follow-ups |

## UX rules (unchanged)

- Never silently substitute fixture data for live failure.
- Fixture pages must visibly identify fixture/sample mode (`eval-dashboard-fixture-banner`, `eval-live-sample-banner`, etc.).
- Live pages must show live, empty, degraded, or missing-capability states (`ProductLiveQueryState`).
- If backend does not expose an action, show a limitation, not a fake button (`backendWaitingContracts`, replay mutations).
- Prefer evidence-first navigation over decorative dashboards.

## Operator doc anchors

| Doc | Path |
| --- | --- |
| Fixture/live boundary | `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md` |
| Config contract | `ontogony-frontend/docs/operators/FRONTEND_CONFIG_OPERATOR_CONTRACT.md` |
| Replay contract | `ontogony-frontend/docs/operators/FRONTEND_REPLAY_OPERATOR_CONTRACT.md` |
