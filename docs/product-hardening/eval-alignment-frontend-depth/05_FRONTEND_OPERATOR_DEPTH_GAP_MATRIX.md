# 05 — Frontend Operator Depth Gap Matrix

| Surface | Current base | Product-depth gap | Suggested PR |
|---|---|---|---|
| Eval dashboard | Hardened, fixture/live audited. | Needs filters, dataset/suite dimensions, comparison entry points, clear list semantics. | `FE-PRODUCT-001` |
| Run detail | Trace/correlation visibility added. | Needs integrated run → eval → topology → route → replay evidence journey. | `FE-PRODUCT-002` |
| Replay evidence | Replay catalog/checks exist. | Needs workbench-style lookup, limitation-aware actions, export, cross-links. | `FE-PRODUCT-003` |
| Baseline comparison | Evidence exists. | Needs comparison history, drilldown, score/case differences. | `EVAL-PRODUCT-002` |
| Scenario datasets | Catalog/gates may exist. | Needs dataset index and UI-facing suite/scenario labels. | `EVAL-PRODUCT-003` |
| Quality scoring | Scaffolding exists. | Needs breakdown, confidence, calibration, judge metadata display. | `EVAL-PRODUCT-004` |

## UX rules

- Never silently substitute fixture data for live failure.
- Fixture pages must visibly identify fixture/sample mode.
- Live pages must show live, empty, degraded, or missing-capability states.
- If backend does not expose an action, show a limitation, not a fake button.
- Prefer evidence-first navigation over decorative dashboards.
