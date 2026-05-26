# Skill Release Deployment Binding Contract V0

## Purpose

`SkillReleaseDeploymentBinding` binds an approved skill version to a **sandbox** consumer target after promotion governance. This is the release-governance binding lifecycle (create → activate → pause → rollback).

**Distinction:** [SKILL_DEPLOYMENT_BINDING_V0.md](./SKILL_DEPLOYMENT_BINDING_V0.md) (optimization spine) describes Conexus target-model **runtime injection** metadata. Release bindings orchestrate sandbox promotion; runtime injection remains a separate concern until explicitly linked in later slices.

## Required fields

```json
{
  "deploymentBindingId": "skillbind_release_sandbox_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "skillVersionId": "skillver_demo_spreadsheet_v1",
  "targetConsumerId": "sandbox-harness-demo",
  "targetEnvironment": "sandbox",
  "bindingStatus": "active",
  "createdByActorId": "local-operator",
  "createdAtUtc": "2026-05-27T12:10:00Z",
  "activatedByActorId": "local-operator",
  "activatedAtUtc": "2026-05-27T12:15:00Z",
  "releaseDecisionId": "kanon_decision_skill_promote_demo_001",
  "promotionRequestId": "skillprom_demo_sandbox_001",
  "rollbackTargetVersionId": "skillver_demo_spreadsheet_v0",
  "evidenceExportId": "skillrelease_evidence_promo_001",
  "metadata": {
    "sourceOptimizationRunId": "skillopt_run_demo_001"
  }
}
```

## `targetEnvironment` enum (v0)

Same allow/forbid list as [SKILL_RELEASE_PROMOTION_REQUEST_V0.md](./SKILL_RELEASE_PROMOTION_REQUEST_V0.md).

## `bindingStatus` enum

```text
created
active
paused
rolled_back
deprecated
```

## Rules

- Activation requires prior `approved_for_sandbox` release decision (001D Allagma slice).
- `rollbackTargetVersionId` must be set before first activation when rollback is required by policy.
- Production and live targets are forbidden in v0 fixtures and schemas.

## Related

- [SKILL_ROLLBACK_V0.md](./SKILL_ROLLBACK_V0.md)
- [SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md](../protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md)
