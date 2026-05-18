# 07 — Event and Trace Model

## New Allagma event types

```text
Allagma.TaskClassified
Allagma.TopologySelected
Allagma.TopologyAuthorizationRequested
Allagma.TopologyAuthorizationCompleted
Allagma.BaselineRunStarted
Allagma.BaselineRunCompleted
Allagma.EvaluationStarted
Allagma.EvaluationCompleted
Allagma.EvaluationFailed
```

## Event payloads

### `Allagma.TaskClassified`

```json
{
  "taskClassificationId": "taskcls_...",
  "runId": "run_...",
  "classification": "sequential",
  "riskLevel": "low",
  "confidence": 0.82,
  "signals": ["no_consequential_marker", "single_objective"],
  "classifierVersion": "task-topology-v0"
}
```

### `Allagma.TopologySelected`

```json
{
  "topologySelectionId": "toposel_...",
  "runId": "run_...",
  "selectedTopology": "single_workflow",
  "reason": "Default topology for sequential low-risk task",
  "requiresKanonAuthorization": false,
  "selectorVersion": "topology-selector-v0"
}
```

### `Allagma.EvaluationCompleted`

```json
{
  "evaluationRunId": "eval_...",
  "runId": "run_...",
  "scenarioId": "summarize-player-risk-v0",
  "verdict": "pass",
  "qualityScore": 0.91,
  "baselineComparisonId": "basecmp_...",
  "promotionRecommendation": "keep_single_workflow"
}
```

## Cross-service IDs

Every eval-capable run should link:

```text
traceId
correlationId
allagmaRunId
kanonDecisionId(s)
kanonHumanGateId(s)
conexusModelCallId(s)
conexusRouteDecisionId(s)
evaluationRunId
baselineRunId
```

## Query goals

Operators should be able to ask:

```text
Show all runs where topology != single_workflow and score < baseline.
Show all high-risk runs that did not create a human gate.
Show all model aliases with poor eval scores by scenario.
Show all runs where Conexus fallback improved completion but increased latency.
Show all topology selections denied by Kanon.
```
