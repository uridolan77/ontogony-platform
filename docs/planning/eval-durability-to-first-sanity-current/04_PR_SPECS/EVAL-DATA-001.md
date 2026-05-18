# EVAL-DATA-001 — Scenario dataset management

## Repo

- `allagma-dotnet`
- optional schema docs in `ontogony-platform`

## Goal

Promote fixture cases into versioned scenario datasets.

## Structure

```text
docs/evals/datasets/scenario-dataset-v0/
  dataset.json
  cases/
  expected/
  baselines/
  labels/
  README.md
```

## Add

```text
EvaluationScenarioDatasetLoader
EvaluationScenarioDatasetValidator
scripts/validate-eval-dataset.ps1
```

## Evidence

Add:

```text
docs/evidence/EVAL_DATA_001_SCENARIO_DATASET_EVIDENCE.md
```
