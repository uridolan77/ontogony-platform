# Executive brief

## Core objective

Build a unified cross-service evidence resolver:

```text
Given any known ID, resolve the whole Ontogony execution chain.
```

Supported input IDs should include:

```text
traceId
correlationId
allagmaRunId
allagmaEvaluationRunId
baselineComparisonId
conexusModelCallId / requestId
conexusRouteDecisionId
kanonDecisionId
planningDecisionId
humanGateId
datasetId / scenarioId
```

## Product target

The operator should be able to paste an ID into one resolver and receive:

```text
- normalized graph nodes
- confidence/completeness indicators
- links to every relevant page
- evidence export actions
- missing-link explanations
- source API calls used to resolve the graph
```

## Why now

The system has enough building blocks:
- frontend trace/correlation resolver exists
- CrossServiceLinksCard exists
- Allagma evidence journeys exist
- Conexus observability workbench exists
- Kanon decision/provenance surfaces exist
- evaluation export bundles exist
- run audit bundles exist

But these are still page-local. The evidence spine should become a platform-level operator pattern.

## Success criterion

From any one meaningful ID, the operator can answer:

```text
What happened?
Which run/eval/request/decision is this?
Which services participated?
Where did routing happen?
Where did semantic authority happen?
What evidence can I export?
What is missing, and why?
```
