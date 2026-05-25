# Add an evaluation or baseline

> **Audience:** backend developer (Allagma), operator  
> **Applies to:** `allagma-dotnet`, console Evaluations pages  
> **Source of truth:** `allagma-dotnet/docs/evals/`, eval harness tests  
> **Last verified:** 2026-05-25

## Concepts

| Term | Meaning |
| --- | --- |
| **Evaluation run** | Recorded assessment attached to a governed run (not a `RunStatus`) |
| **Baseline** | Comparable expected outcomes for regression |
| **Dataset** | Scenario fixtures (e.g. `docs/evals/datasets/maf-scenario-dataset-v0/`) |
| **Harness** | Dry/local replayer (`EvalHarnessScenarioReplayer`) |

## Checklist

1. **Scenario fixtures** — Add case JSON under `docs/evals/datasets/` or `docs/evals/cases/`.
2. **Harness** — Wire scenario kind in `Allagma.Tests` eval harness if new behavior.
3. **CI script** — e.g. `scripts/run-maf-eval-ci-suite.ps1` for MAF scenarios.
4. **Events** — Use existing vocabulary (`EvaluationRunRecorded`, etc.); update snapshots if new event types.
5. **API** — Expose via `/allagma/v0` eval routes; sync OpenAPI to frontend.
6. **UI** — Evaluations overview + detail pages; signal groups for status.
7. **Evidence** — Document PASS in `docs/evidence/` when promoting baselines.
8. **Honesty** — Do not claim live provider proof unless RP-003A or explicit live run recorded.

## Run locally

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-allagma-eval-suite.ps1
.\scripts\run-maf-eval-ci-suite.ps1
dotnet test --filter FullyQualifiedName~MafEval
```

## References

- `allagma-dotnet/docs/evidence/AGM_MAF_EVAL_SCENARIOS_001.md`
- [03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md)
- [04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md)
