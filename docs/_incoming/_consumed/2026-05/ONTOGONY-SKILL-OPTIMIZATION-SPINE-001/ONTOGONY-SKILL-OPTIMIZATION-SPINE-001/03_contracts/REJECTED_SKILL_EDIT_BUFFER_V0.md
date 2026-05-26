# RejectedSkillEditBuffer Contract V0

## Purpose

The rejected-edit buffer stores negative evidence from failed skill updates. It prevents repeated bad proposals and gives future optimizers context on what not to do.

## Required fields

```json
{
  "rejectedEditBufferId": "rejbuf_demo_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "baseVersionId": "skillver_demo_spreadsheet_v0",
  "skillOptimizationRunId": "skillopt_run_demo_001",
  "entries": [
    {
      "skillEditId": "skilledit_bad_001",
      "candidateVersionId": "skillver_demo_bad_candidate_001",
      "rejectionType": "rejectedRegression",
      "rejectionReason": "Candidate improved train fixture but reduced selection score from 0.66 to 0.33.",
      "gateId": "skillgate_bad_001",
      "evidenceRefs": [
        { "type": "evaluationTask", "id": "selection_task_002" }
      ],
      "futureOptimizerHint": "Do not remove workbook inventory step; it caused hidden-sheet failures.",
      "createdAt": "2026-05-26T00:00:00Z"
    }
  ],
  "summary": {
    "totalRejected": 1,
    "byReason": { "rejectedRegression": 1 },
    "lastUpdatedAt": "2026-05-26T00:00:00Z"
  }
}
```

## Rejection type enum

```text
rejectedTie
rejectedRegression
rejectedValidationFailure
rejectedPolicyViolation
rejectedBudgetExceeded
rejectedProtectedSectionMutation
rejectedHumanReview
```

## Rules

- Do not delete rejected edit data during normal cleanup.
- Rejected edits may be summarized, but raw entries must remain accessible in development/local mode.
- Rejected edits should be visible in Skill Lab as negative evidence, not as errors to hide.
- Future optimizer prompts may include compact rejected-edit summaries, never hidden chain-of-thought.
