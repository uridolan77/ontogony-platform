# SkillOptimizationRun Contract V0

## Purpose

`SkillOptimizationRun` represents an Allagma-governed optimization process for one skill artifact/version.

## Required fields

```json
{
  "skillOptimizationRunId": "skillopt_run_demo_001",
  "allagmaRunId": "run_skillopt_demo_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "baseVersionId": "skillver_demo_spreadsheet_v0",
  "status": "accepted",
  "createdAt": "2026-05-26T00:00:00Z",
  "createdByActorId": "local-operator",
  "targetModelProfile": "fake-target-local",
  "optimizerModelProfile": "fake-optimizer-local",
  "executionHarness": "direct",
  "dataset": {
    "trainTasksetId": "taskset_spreadsheet_train_demo",
    "selectionTasksetId": "taskset_spreadsheet_selection_demo",
    "testTasksetId": null
  },
  "schedule": {
    "epoch": 1,
    "step": 1,
    "editBudget": 3,
    "learningRateSchedule": "constant"
  },
  "phases": [
    { "phase": "rolloutEvidence", "status": "completed" },
    { "phase": "minibatchReflection", "status": "completed" },
    { "phase": "candidateEditGeneration", "status": "completed" },
    { "phase": "candidateValidation", "status": "completed" },
    { "phase": "heldOutGate", "status": "completed" },
    { "phase": "acceptanceDecision", "status": "completed" }
  ],
  "candidateVersionIds": ["skillver_demo_spreadsheet_v1_candidate"],
  "acceptedVersionId": "skillver_demo_spreadsheet_v1",
  "rejectedEditBufferId": "rejbuf_demo_001",
  "skillEvaluationGateId": "skillgate_demo_001",
  "evidenceRefs": [
    { "type": "trace", "id": "trace_skillopt_demo_001" },
    { "type": "conexusModelCall", "id": "conexus_call_optimizer_001" },
    { "type": "kanonDecision", "id": "kanon_decision_skill_accept_demo_001" }
  ]
}
```

## Status enum

```text
created
running
paused
cancelled
failed
candidateGenerated
gatePending
accepted
rejected
published
deployed
rolledBack
```

## Phase enum

```text
rolloutEvidence
minibatchReflection
candidateEditGeneration
candidateValidation
candidatePatchApplication
heldOutGate
acceptanceDecision
publishDecision
deploymentBinding
rollback
```

## Required operations

```text
start
pause
resume
cancel
validateCandidate
runGate
acceptCandidate
rejectCandidate
publishVersion
deployVersion
rollbackDeployment
```

## Rules

- A run must declare target and optimizer model profiles separately.
- A run must persist optimizer model calls separately from target rollout calls.
- A run may create many candidate versions but should accept at most one per optimization step.
- A run may be replayed deterministically when using fake provider fixtures.
