# SkillEvaluationGate Contract V0

## Purpose

`SkillEvaluationGate` records the held-out selection comparison that accepts or rejects a candidate skill version.

## Required fields

```json
{
  "skillEvaluationGateId": "skillgate_demo_001",
  "skillOptimizationRunId": "skillopt_run_demo_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "incumbentVersionId": "skillver_demo_spreadsheet_v0",
  "candidateVersionId": "skillver_demo_spreadsheet_v1_candidate",
  "selectionTasksetId": "taskset_spreadsheet_selection_demo",
  "policy": {
    "minDelta": 0.0,
    "allowTie": false,
    "minimumTaskCount": 3,
    "confidenceMode": "deterministic-fixture"
  },
  "incumbentScore": 0.66,
  "candidateScore": 0.83,
  "delta": 0.17,
  "status": "accepted",
  "decisionReason": "Candidate strictly improved held-out selection score.",
  "taskResults": [
    {
      "taskId": "selection_task_001",
      "incumbentScore": 0,
      "candidateScore": 1,
      "verifier": "exact-match-fixture"
    }
  ],
  "kanonDecisionId": "kanon_decision_skill_gate_demo_001",
  "createdAt": "2026-05-26T00:00:00Z"
}
```

## Status enum

```text
pending
accepted
rejectedTie
rejectedRegression
rejectedInsufficientSample
rejectedValidationFailure
requiresHumanReview
```

## Acceptance rule

Default:

```text
candidateScore > incumbentScore + minDelta
```

Tie rejection is intentional. It prevents silent drift.

## Rules

- Training split performance must not be used as the acceptance criterion.
- A candidate that improves training but regresses selection must be rejected.
- The gate must preserve per-task results where possible.
- The gate must link to a Kanon decision.
- The gate must be deterministic for fixture mode.
