# Acceptance Gates

## Package-level acceptance

The package is complete when all modified repos satisfy their own tests and the cross-service fixture proves:

```text
base skill -> rollout -> candidate edits -> gate -> accepted version -> deployment -> skill-injected call
```

and:

```text
bad candidate -> gate rejection -> rejected-edit buffer
```

## Non-negotiable gates

### Gate 1: strict held-out improvement

A candidate must not be accepted unless:

```text
candidateScore > incumbentScore + minDelta
```

### Gate 2: tie rejection

A candidate with equal score must be rejected unless an explicit policy allows ties. Default is reject.

### Gate 3: validation before evaluation

Invalid edits must not reach the held-out gate.

### Gate 4: publish != deploy

Acceptance does not imply deployment. Publish/deploy are separate governed actions.

### Gate 5: zero optimizer calls during normal inference

Skill injection is allowed during normal inference. Optimizer model calls are not.

### Gate 6: immutable versions

A published/deployed skill version's content hash must not change.

### Gate 7: rollback path

A deployed skill version must have a clear rollback target or explicitly state that rollback is unavailable.

## Suggested test names

```text
SkillEvaluationGateRejectsTieByDefault
SkillEvaluationGateRejectsRegression
SkillEvaluationGateAcceptsStrictImprovement
SkillEditValidatorRejectsProtectedSectionMutation
SkillEditValidatorRejectsBudgetExceeded
SkillOptimizationRunPersistsRejectedEditBuffer
SkillDeploymentRequiresPublishedVersion
SkillInjectedTargetCallRecordsSkillVersionMetadata
NormalInferenceDoesNotInvokeOptimizerModel
SkillEvidenceGraphContainsRunGateDecisionAndDeployment
```
