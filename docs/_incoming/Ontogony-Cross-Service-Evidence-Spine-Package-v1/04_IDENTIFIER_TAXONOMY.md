# Identifier taxonomy

## Canonical ID kinds

| Kind | Examples | Owner | Primary lookup |
|---|---|---|---|
| `traceId` | `trace_...` or raw trace string | Platform/cross-service | All services where supported |
| `correlationId` | `corr_...` | Platform/cross-service | correlation metadata |
| `allagmaRunId` | `run_...` | Allagma | `GET /runs/{runId}` |
| `allagmaEvaluationRunId` | `eval_...` | Allagma | `GET /evaluations/{evaluationRunId}` |
| `baselineComparisonId` | `baseline_...` / `cmp_...` | Allagma | `GET /evaluations/baseline-comparisons/{id}` |
| `conexusModelCallId` | `chatcmpl_...` / request id | Conexus | execution run/request lookup |
| `conexusRouteDecisionId` | route decision id | Conexus | route decision lookup |
| `kanonDecisionId` | decision id | Kanon | `GET decision` |
| `planningDecisionId` | planning decision id | Kanon/Allagma | Kanon decision lookup |
| `humanGateId` | gate/pause id | Allagma/Kanon | Allagma events |
| `datasetId` | `scenario-dataset-v0` | Allagma eval | dataset endpoint |
| `scenarioId` | `scenario-risk-summary-v0` | Allagma eval | eval/dataset filters |

## Rules

1. Never rely only on prefix. Allow manual type override.
2. When type is ambiguous, run safe lookups in priority order.
3. Track every lookup attempted.
4. Missing result is not failure; it is an unresolved edge.
5. Hard service errors must be shown as source failures.
