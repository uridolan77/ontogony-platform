# Allagma Implementation Plan

## Role

Allagma owns the operational lifecycle of skill optimization runs.

## Add / extend concepts

### Run type

Add or extend a run type:

```text
SkillOptimization
```

### Run statuses

Map to existing `RunStatus` if possible. Otherwise add explicit status metadata:

```text
created
running
paused
candidateGenerated
gatePending
accepted
rejected
published
deployed
rolledBack
failed
cancelled
```

### Operations matrix

Add operation availability rules:

| Status | Primary operation | Secondary operations |
|---|---|---|
| created | start | cancel |
| running | pause | cancel |
| candidateGenerated | validateCandidate | cancel |
| gatePending | runGate | rejectCandidate |
| accepted | publishVersion | rollback |
| published | deployVersion | rollback |
| deployed | rollbackDeployment | retireBinding |
| rejected | inspectRejectedBuffer | retryFromBase |
| failed | replay | inspectDiagnostics |

## Endpoints / services

Adapt names to existing routing conventions.

```text
GET  /workflow/v0/skill-optimization/runs
POST /workflow/v0/skill-optimization/runs
GET  /workflow/v0/skill-optimization/runs/{runId}
GET  /workflow/v0/skill-optimization/runs/{runId}/timeline
GET  /workflow/v0/skill-optimization/runs/{runId}/candidate-versions
GET  /workflow/v0/skill-optimization/runs/{runId}/edits
GET  /workflow/v0/skill-optimization/runs/{runId}/gate
POST /workflow/v0/skill-optimization/runs/{runId}/operations/{operation}
GET  /workflow/v0/skill-optimization/runs/{runId}/replay-fixture
```

## Orchestration responsibilities

1. Create run with base skill version and dataset refs.
2. Execute rollout evidence using target model/harness.
3. Request optimizer edits through Conexus optimizer route.
4. Store candidate edit list and candidate version ref.
5. Ask Kanon to validate edit semantics/policy.
6. Run held-out selection gate.
7. Persist accept/reject state.
8. Create human gate where publishing/deployment requires operator review.
9. Expose replay fixture for local reproducibility.

## Fake-provider first slice

Use deterministic fixture services for first implementation:

```text
FakeSkillRolloutRunner
FakeSkillOptimizer
FakeSkillEvaluationGateRunner
```

The first slice should not depend on live OpenAI calls.

## Integration with existing run/evidence spine

Every Allagma skill optimization run should emit evidence fragments:

```text
run-created
rollout-started
rollout-completed
optimizer-call-requested
candidate-edits-generated
candidate-validated
gate-completed
candidate-accepted/rejected
published/deployed/rolled-back
```

## Tests

Add tests for:

- status/operation availability matrix;
- deterministic fixture run;
- accepted candidate path;
- rejected candidate path;
- replay fixture equality;
- evidence fragments present;
- no optimizer call on normal inference path.

## Docs

Update:

- run lifecycle docs;
- workflow operation semantics;
- route inventory/OpenAPI baseline;
- local fixture guide.
