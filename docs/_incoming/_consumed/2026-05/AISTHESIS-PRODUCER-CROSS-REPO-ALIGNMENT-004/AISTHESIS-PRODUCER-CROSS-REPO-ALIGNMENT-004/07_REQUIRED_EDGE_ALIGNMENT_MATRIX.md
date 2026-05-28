# Required edge alignment matrix

| # | Required edge ID | Owning emitter | From | To | Relation | Native IDs |
|---:|---|---|---|---|---|---|
| 1 | `allagma.run_to_kanon.semantic_plan` | Allagma | Allagma run | Kanon semantic plan | `requested` / `used` | `runId`, `semanticPlanId` |
| 2 | `allagma.run_to_kanon.decision` | Allagma | Allagma run | Kanon decision | `evaluated_by` | `runId`, `decisionId` |
| 3 | `allagma.run_to_conexus.model_call` | Allagma | Allagma run | Conexus model call | `requested` | `runId`, `modelCallId` |
| 4 | `kanon.decision_to_semantic_plan` | Kanon | Kanon decision | Kanon semantic plan | `derived_from` | `decisionId`, `semanticPlanId` |
| 5 | `kanon.decision_to_policy` | Kanon | Kanon decision | Kanon policy evaluation | `evaluated` | `decisionId`, `policyEvaluationId` |
| 6 | `conexus.model_call_to_route_decision` | Conexus | Conexus model call | Conexus route decision | `routed_to` | `modelCallId`, `routeDecisionId` |
| 7 | `conexus.model_call_to_provider_attempt` | Conexus | Conexus model call | Provider attempt or same envelope | `used` or `providerAttemptId` | `modelCallId`, `providerAttemptId` |
| 8 | `metabole.pipeline_to_profile` | Metabole | Pipeline | Data profile | `produced` | `pipelineRunId`, `dataProfileId` |
| 9 | `metabole.pipeline_to_mapping_candidate` | Metabole | Pipeline | Mapping candidate | `produced` | `pipelineRunId`, `mappingCandidateId` |
| 10 | `metabole.mapping_candidate_to_artifact` | Metabole | Mapping candidate | Output/SLOD artifact | `materialized_as` | `mappingCandidateId`, `artifactId` |

## Grade-critical non-matrix relation

Kanon decision → Conexus model call authorization edge should exist when a model call is authorized:

```text
relation = authorized / approved_by
```
