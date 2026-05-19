# 05 — Frontend Operator Depth Gap Matrix

**Audit:** PFH-001 (2026-05-19).

| Surface | Current base (audited) | Product-depth gap | Suggested PR |
| --- | --- | --- | --- |
| Eval dashboard | **Closed (FE-PRODUCT-001 + EVAL-PRODUCT-003):** filters, dimensions, comparison entries, results list, cursor hook; metadata-backed dataset/suite/scenario hints | Deeper pagination UX, saved views | follow-ups |
| Eval run detail | `EvaluationRunDetailPage` — quality panel, fixture/live banners, judge metadata card | Correlation links to run/trace/decision and deeper evidence drilldown | `FE-PRODUCT-002` |
| Run detail | `AllagmaRunEvalTopologyEvidenceSection` — eval + topology; link to evaluations | Integrated journey: run → evals → topology → Conexus route → replay | `FE-PRODUCT-002` |
| Replay evidence | `ReplayEvidencePage` + `AllagmaReplayEvidenceWorkbench` — run/trace/decision modes; `@ontogony/ui` panels | Workbench lookup UX, limitation-aware actions, export, cross-links to eval/comparison | `FE-PRODUCT-003` |
| Baseline comparison | **Closed (EVAL-PRODUCT-002):** `BaselineComparisonWorkbenchPage` list/history filters + detail/workbench cross-links | Richer visual diff/side-by-side outcomes; operator create flow still out of scope | follow-up |
| Scenario datasets | **Closed (EVAL-PRODUCT-003):** dataset index page + read-only scenario metadata + links to filtered evaluations | No saved dataset views/bookmarks | follow-up |
| Quality scoring | **Closed (EVAL-PRODUCT-004):** `AllagmaEvalQualityScoreDetail` includes quality/judge metadata, advisory wording, dimension counts | Calibration history/trend workspace | follow-up |
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
