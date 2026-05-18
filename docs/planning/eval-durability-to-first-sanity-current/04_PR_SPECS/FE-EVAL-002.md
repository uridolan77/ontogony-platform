# FE-EVAL-002 — Richer eval dashboards and trend views

## Repo

- `ontogony-frontend`

## Goal

Move from run-level eval visibility to dashboard-level operator visibility.

## Suggested routes

```text
/allagma/evaluations
/allagma/evaluations/:evaluationRunId
/allagma/evaluations/baseline-comparisons/:comparisonId
```

## Views

- eval overview cards
- scenario matrix
- baseline comparison detail
- quality score detail
- dataset/fixture warning

## Rule

Do not fake live dashboards. If list APIs are missing, show honest limitation state or use clearly labelled fixtures.

## Evidence

Add:

```text
docs/evidence/FE_EVAL_002_DASHBOARDS_EVIDENCE.md
```
