# Skill Release Decision Contract V0

## Purpose

`SkillReleaseDecision` records Kanon semantic authority for whether a promotion request may proceed to sandbox binding. Kanon owns meaning; Allagma records and orchestrates lifecycle only after allow outcomes.

## Required fields

```json
{
  "decisionId": "kanon_decision_skill_promote_demo_001",
  "promotionRequestId": "skillprom_demo_sandbox_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "candidateVersionId": "skillver_demo_spreadsheet_v1",
  "decisionStatus": "approved_for_sandbox",
  "targetEnvironment": "sandbox",
  "policyBasis": "skill-release-governance-fake-v0",
  "safeRationaleSummary": "Accepted candidate with optimization evidence; sandbox target only; production deployment not requested.",
  "blockingReasons": [],
  "requiredEvidence": [],
  "createdAtUtc": "2026-05-27T12:05:00Z"
}
```

## `decisionStatus` enum

```text
approved_for_sandbox
rejected
deferred
```

## Rules

- `safeRationaleSummary` must be operator-safe (no hidden chain-of-thought storage).
- `blockingReasons` and `requiredEvidence` are populated on `rejected` or `deferred`.
- Binding and rollback decisions use separate decision types in later slices (`SkillDeploymentBindingDecision`, `SkillRollbackDecision`).

## Related

- [SKILL_RELEASE_PROMOTION_REQUEST_V0.md](./SKILL_RELEASE_PROMOTION_REQUEST_V0.md)
