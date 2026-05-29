# Contract — required-edge golden trace v2

## Purpose

Define the minimum evidence shape for a complete live evidence spine trace.

## Required producers

```text
allagma
kanon
conexus
metabole
```

## Required native IDs

```text
runId
semanticPlanId
decisionId
policyEvaluationId
modelCallId
routeDecisionId
providerAttemptId
pipelineRunId
dataProfileId
mappingCandidateId
artifactId
```

## Required edges

```text
allagma.run_to_kanon.semantic_plan
allagma.run_to_kanon.decision
allagma.run_to_conexus.model_call
kanon.decision_to_semantic_plan
kanon.decision_to_policy
conexus.model_call_to_route_decision
conexus.model_call_to_provider_attempt
metabole.pipeline_to_profile
metabole.pipeline_to_mapping_candidate
metabole.mapping_candidate_to_artifact
```

## Required outcome

```text
requiredEdges.missing = 0
blockingFindings = 0
bundleFingerprintPresent = true
```

A trace can be accepted as partial only if required edges are not applicable for a documented workflow profile.
