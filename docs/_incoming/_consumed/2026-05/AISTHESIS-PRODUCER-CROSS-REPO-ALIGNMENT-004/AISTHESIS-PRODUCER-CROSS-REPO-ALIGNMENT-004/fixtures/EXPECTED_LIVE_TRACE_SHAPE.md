# Expected live trace shape

A successful live trace must include producer coverage:

```text
allagma
kanon
conexus
metabole
```

Required native IDs:

```text
runId
semanticPlanId
decisionId
policyEvaluationId
routeDecisionId
modelCallId
providerAttemptId
pipelineRunId
dataProfileId
mappingCandidateId
artifactId
```

Required result:

```text
requiredEdges.present = 10
requiredEdges.missing = 0
reconstructabilityGrade = complete
```
