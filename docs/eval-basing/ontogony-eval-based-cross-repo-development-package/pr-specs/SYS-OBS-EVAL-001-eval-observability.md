# PR Spec — SYS-OBS-EVAL-001 — Eval Observability

## Owner repo

`allagma-dotnet`

## Goal

Expose eval/topology metrics across the local observability pack.

## Add metrics

```text
allagma.topology.selected.count
allagma.topology.authorization.count
allagma.eval.run.count
allagma.eval.score
allagma.baseline.comparison.count
kanon.topology_policy.evaluation.count
conexus.route_decision.count
conexus.model_quality.feedback.count
```

## Labels

Bounded only:

```text
topology_mode
task_classification
risk_level
outcome
scenario_id
model_alias
provider_key
```

## Tests

- metric instrument names documented.
- no raw objective/prompt/response/tool args in labels.
- dashboard JSON updated if applicable.

## Acceptance

- observability docs updated.
- local verification script checks at least one eval metric.
