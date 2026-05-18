# PR Spec — AGM-TOPO-002 — Audit Bundle Topology Extension

## Repo

`allagma-dotnet`

## Goal

Make topology evidence queryable by operators.

## Add to audit bundle

```text
taskClassification
topologySelection
topologyAuthorizationDecisionId
baselineComparisonId
evaluationRunIds
```

Fields may be null until later PRs.

## Tests

- audit bundle backward-compatible.
- missing topology rows produce explicit `not_available`, not exceptions.
- event-derived fallback works when persistence row is absent.

## Acceptance

- docs/operations/run-and-event-query-api.md updated.
- sample audit JSON added under docs/evidence or examples.
