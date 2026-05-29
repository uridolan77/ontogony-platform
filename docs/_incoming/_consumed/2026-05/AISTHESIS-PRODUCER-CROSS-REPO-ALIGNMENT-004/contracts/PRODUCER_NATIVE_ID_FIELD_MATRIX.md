# Producer native ID field matrix

| Producer | Evidence | Required native IDs |
|---|---|---|
| Allagma | run lifecycle | `runId` |
| Allagma | run→Kanon plan | `runId`, `semanticPlanId` |
| Allagma | run→Kanon decision | `runId`, `decisionId` |
| Allagma | run→Conexus model call | `runId`, `modelCallId` |
| Kanon | semantic plan | `semanticPlanId` |
| Kanon | semantic decision | `decisionId`, `semanticPlanId` |
| Kanon | policy evaluation | `policyEvaluationId`, `decisionId` |
| Conexus | route decision | `routeDecisionId` |
| Conexus | model call | `modelCallId`, `routeDecisionId`, `providerAttemptId` |
| Metabole | pipeline | `pipelineRunId` |
| Metabole | data profile | `pipelineRunId`, `dataProfileId` |
| Metabole | mapping candidate | `pipelineRunId`, `mappingCandidateId` |
| Metabole | output artifact / SLOD artifact | `mappingCandidateId`, `artifactId` |
