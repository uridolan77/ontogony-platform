# SkillVersion Contract V0

## Purpose

`SkillVersion` is an immutable version of a skill document.

## Required fields

```json
{
  "skillVersionId": "skillver_demo_spreadsheet_v1",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "semanticVersion": "0.1.1",
  "status": "acceptedValidated",
  "contentUri": "skills/demo.spreadsheet-analysis/0.1.1/best_skill.md",
  "contentHash": "sha256:...",
  "tokenEstimate": 742,
  "baseVersionId": "skillver_demo_spreadsheet_v0",
  "createdAt": "2026-05-26T00:00:00Z",
  "createdBy": {
    "type": "optimizationRun",
    "skillOptimizationRunId": "skillopt_run_demo_001"
  },
  "lineage": {
    "acceptedEditIds": ["skilledit_demo_001", "skilledit_demo_002"],
    "rejectedEditBufferId": "rejbuf_demo_001",
    "kanonDecisionIds": ["kanon_decision_skill_accept_demo_001"]
  },
  "evaluationSummary": {
    "incumbentScore": 0.66,
    "candidateScore": 0.83,
    "delta": 0.17,
    "gateStatus": "accepted",
    "skillEvaluationGateId": "skillgate_demo_001"
  },
  "deployment": {
    "isDeployable": true,
    "activeBindingIds": ["skillbind_demo_local_001"]
  },
  "protectedSections": ["slowMetaUpdate"],
  "notes": "Accepted by deterministic fixture gate."
}
```

## Status enum

```text
draft
candidate
underOptimization
gatePending
acceptedValidated
gateRejected
published
deployed
superseded
retired
rolledBack
```

## Invariants

- `contentHash` is immutable.
- `baseVersionId` must be present for derived versions.
- `status=deployed` requires at least one active deployment binding.
- `status=published` requires a Kanon publish decision.
- `status=acceptedValidated` requires a passing evaluation gate.
- `status=gateRejected` requires a gate result with rejection reason.

## Skill document format

Recommended markdown structure:

```markdown
# Skill: <name>

## Applicability

## Procedure

## Tool-use Rules

## Output Contract

## Failure Modes

## Slow/Meta Update
```

`Slow/Meta Update` is protected from normal step-level edits unless an epoch-level update explicitly targets it.
