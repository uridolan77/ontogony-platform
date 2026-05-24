# Backend Follow-ups

These are not required frontend implementation tasks unless current backend contracts already support them.

## Allagma evaluation metadata

Consider ensuring every evaluation row can include:

```json
{
  "evaluationId": "...",
  "runId": "...",
  "traceId": "...",
  "correlationId": "...",
  "scenarioId": "...",
  "datasetId": "...",
  "suiteId": "...",
  "comparisonId": "...",
  "provider": "...",
  "modelAlias": "...",
  "model": "...",
  "modelCallId": "...",
  "routeDecisionId": "...",
  "kanonDecisionId": "...",
  "inputTokenCount": 0,
  "outputTokenCount": 0,
  "totalTokenCount": 0,
  "estimatedCost": 0,
  "completedAt": "...",
  "qualityScore": 0.0,
  "verdict": "pass|fail|inconclusive"
}
```

## Durable analytics endpoint

If desired later:

```text
GET /allagma/v0/evaluations/summary
GET /allagma/v0/evaluations/trends
```

These should be explicitly separate from current-page list summaries.

## Evidence resolution

If Evidence Spine should support evaluation IDs directly, expose or document:

```text
identifier kind: evaluation
lookup target: eval_<id>
resolved links:
- run
- trace/correlation
- model call
- decisions
- comparison
```

## Baseline comparison linkage

Make sure evaluation results created from baseline comparisons include the comparison ID and subject/baseline run IDs.

## Provider metadata

If provider is currently unknown because the evaluation is decoupled from model calls, decide whether to:

1. mark as truly not applicable; or
2. link to the model call and propagate provider metadata.
