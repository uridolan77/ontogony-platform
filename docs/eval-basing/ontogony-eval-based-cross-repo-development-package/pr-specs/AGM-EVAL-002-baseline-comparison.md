# PR Spec — AGM-EVAL-002 — Baseline Comparison

## Repo

`allagma-dotnet`

## Goal

Compare selected topology against single-workflow baseline safely.

## Safety rule

Baseline mode must not duplicate side effects.

Allowed baseline types:

```text
model_only
simulation_only
dry_run
read_only
```

## Add

```text
BaselineRunner
BaselineComparisonService
Allagma.BaselineRunStarted
Allagma.BaselineRunCompleted
BaselineComparisonRecord persistence
```

## Tests

- no baseline for side-effecting real tool execution.
- simulation baseline can run.
- comparison computes quality/cost/latency deltas.
- promotion recommendation generated.

## Acceptance

- eval summary includes baselineComparison.
- unsafe baseline attempts fail closed.
