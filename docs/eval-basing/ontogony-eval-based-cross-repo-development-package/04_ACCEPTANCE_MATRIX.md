# 04 — Acceptance Matrix

## Global acceptance gates

| Gate | Required evidence |
|---|---|
| Build | `dotnet build` passes for changed repo |
| Unit tests | repo default tests pass |
| Contract tests | serialization/openapi/public API tests updated |
| Boundary tests | no forbidden references introduced |
| Smoke | relevant local smoke script passes |
| Evidence | machine-readable JSON summary committed or generated |
| Docs | README/status/changelog updated |
| Security | no raw secrets, prompts, provider payloads in logs/metrics |
| Replay | run evidence is replay-safe where applicable |

## PR-specific acceptance

| PR | Must prove |
|---|---|
| `PLAT-EVAL-001` | neutral eval DTOs, no product dependencies |
| `PLAT-TOPO-001` | neutral topology vocabulary, no Allagma/Kanon semantics |
| `AGM-TOPO-001` | every run emits classification + selected topology |
| `KANON-TOPO-001` | topology request returns allow/deny/human_gate + decision record |
| `CX-ROUTE-EVIDENCE-001` | model call links to routeDecisionId |
| `AGM-EVAL-001` | scenario runner exports eval summary JSON |
| `SYS-EVAL-001` | stack smoke emits trace-linked eval artifact |

## Definition of done for the full program

A single local smoke should produce:

```text
runId
traceId
taskClassificationId
topologySelectionId
kanonPlanningDecisionId
kanonTopologyPolicyDecisionId
conexusRouteDecisionId
modelCallId
evaluationRunId
baselineComparisonId
finalVerdict
```

The final verdict must include:

```text
success
qualityScore
policyCorrect
toolTrajectoryCorrect
costUsd
latencyMs
replayable
baselineDelta
promotionRecommendation
```
