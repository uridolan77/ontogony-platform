# PR Spec — AGM-EVAL-001 — Eval Harness and Baseline Runner

## Repo

`allagma-dotnet`

## Goal

Create deterministic eval harness for existing scenarios.

## Add

```text
docs/evals/
examples/evals/
scripts/run-allagma-eval-suite.ps1
src/Allagma.Application/Evaluation/
tests/Allagma.Tests/Evaluation/
```

## Initial scenarios

```text
summarize-player-risk-baseline
unregistered-tool-blocked
human-gate-required
provider-fallback-recorded
restart-replay-safe
```

## Eval output

Produce:

```text
artifacts/eval/<timestamp>/eval-summary.json
```

following `schemas/eval-run-summary.schema.json`.

## Baseline

For now, baseline comparison may be same-run structural baseline:

```text
selectedTopology = single_workflow
expectedTopology = single_workflow
```

A later PR can run a second baseline execution.

## Tests

- eval summary validates against schema.
- unsafe tool scenario fails if tool is allowed.
- human gate scenario fails if no gate id.
- route-decision metric remains pending until Conexus PR lands.

## Acceptance

- no real side effects.
- deterministic pass/fail.
- evidence JSON generated.
