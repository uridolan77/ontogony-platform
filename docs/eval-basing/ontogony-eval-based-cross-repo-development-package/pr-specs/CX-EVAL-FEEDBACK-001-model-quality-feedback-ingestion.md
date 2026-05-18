# PR Spec — CX-EVAL-FEEDBACK-001 — Model Quality Feedback Ingestion

## Repo

`conexus-dotnet`

## Goal

Allow eval systems to submit model-call quality feedback linked to route/model decisions.

## Endpoint

```text
POST /admin/v0/model-quality-feedback
```

## Request

```json
{
  "modelCallId": "modelcall_...",
  "routeDecisionId": "route_...",
  "evaluationRunId": "eval_...",
  "scenarioId": "summarize-player-risk-v0",
  "metricName": "task_success",
  "score": 1.0,
  "passed": true
}
```

## Behavior

Store as feedback. Do not automatically alter routing in this PR.

## Tests

- admin auth required.
- unknown model call accepted as external ref? choose explicit policy and document.
- aggregation query by model alias/provider.

## Acceptance

- feedback is queryable.
- no routing mutation yet.
