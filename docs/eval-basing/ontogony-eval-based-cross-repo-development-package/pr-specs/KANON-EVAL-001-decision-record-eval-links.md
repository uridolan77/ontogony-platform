# PR Spec — KANON-EVAL-001 — Decision Record Eval Links

## Repo

`kanon-dotnet`

## Goal

Allow decision records to carry optional eval/topology link refs.

## Add optional fields

```text
taskClassificationId
topologySelectionId
topologyPolicyDecisionId
evaluationRunId
baselineRunId
routeDecisionIds
```

## Important

These are references only. Kanon does not score evals.

## Tests

- existing decision records remain backward-compatible.
- provenance envelope includes refs when present.
- replay bundle export includes refs.
- missing refs do not fail verification.

## Acceptance

- OpenAPI baseline updated.
- migration note if persisted shape changes.
