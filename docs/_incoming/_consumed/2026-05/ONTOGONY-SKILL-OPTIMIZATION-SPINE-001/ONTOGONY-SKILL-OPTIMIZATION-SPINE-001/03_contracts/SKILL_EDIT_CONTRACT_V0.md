# SkillEdit Contract V0

## Purpose

`SkillEdit` is an atomic bounded proposal against a base skill version.

## Required fields

```json
{
  "skillEditId": "skilledit_demo_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "baseVersionId": "skillver_demo_spreadsheet_v0",
  "candidateVersionId": "skillver_demo_spreadsheet_v1_candidate",
  "operation": "add",
  "sectionPath": "/Procedure",
  "beforeTextHash": null,
  "proposedTextHash": "sha256:...",
  "boundedBudgetUnits": 1,
  "rationaleSummary": "Rollout failures repeatedly skipped checking hidden sheets before answering.",
  "expectedEffect": "Improve spreadsheet QA accuracy by forcing workbook inventory before calculation.",
  "riskClass": "low",
  "sourceEvidenceRefs": [
    { "type": "allagmaRun", "id": "run_demo_rollout_001" },
    { "type": "evaluationTask", "id": "task_spreadsheet_hidden_sheet_001" }
  ],
  "generatedBy": {
    "optimizerModelCallId": "conexus_call_optimizer_001",
    "optimizerModelProfile": "fake-optimizer-local"
  },
  "validation": {
    "status": "passed",
    "validatorVersion": "skill-edit-validator-v0",
    "messages": []
  },
  "decision": {
    "kanonDecisionId": "kanon_decision_skill_edit_accept_001",
    "decisionStatus": "accepted"
  }
}
```

## Operation enum

```text
add
insert
replace
delete
move
```

Initial slice may implement only:

```text
add
delete
replace
```

## Risk class enum

```text
low
medium
high
blocked
```

## Validation status enum

```text
pending
passed
failed
requiresHumanReview
```

## Rules

- Every edit must target a known section path.
- Every edit must consume bounded budget units.
- Delete operations require explicit evidence and should default to `medium` risk or higher.
- Replace operations require before and after hashes.
- Step-level edits cannot target protected sections.
- All accepted edits must have a Kanon decision.
- Rejected edits must preserve rejection reason and should remain visible in the rejected buffer.
