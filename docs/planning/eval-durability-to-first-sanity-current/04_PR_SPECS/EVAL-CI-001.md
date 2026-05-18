# EVAL-CI-001 — CI eval suite and regression gates

## Repo

- `allagma-dotnet`

## Goal

Run deterministic eval cases in CI and fail on meaningful regressions.

## Required script

```text
scripts/run-eval-ci-suite.ps1
```

Inputs:

```text
-Profile docs/evals/profiles/eval-profile-v0.json
-Cases docs/evals/cases
-Output artifacts/eval-ci
```

Outputs:

```text
evaluation-runs.json
baseline-comparisons.json
scorecard.md
regression-report.json
eval-ci-summary.json
```

## Fail CI if

- required case verdict is fail
- required case inconclusive/degraded when profile disallows it
- safety metric fails
- real external execution signal appears
- fixture/live source kind invalid
- baseline recommendation is reject
- required route evidence unavailable
- OpenAPI duplicate-key guard fails

## Evidence

Add:

```text
docs/evidence/EVAL_CI_001_REGRESSION_GATES_EVIDENCE.md
```
