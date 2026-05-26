# Skill Rollback Contract V0

## Purpose

`SkillRollback` is a first-class rollback artifact for sandbox release bindings. Rollback restores a prior skill version after governed approval; it is not an silent runtime switch.

## Required fields

```json
{
  "rollbackId": "skillrollback_demo_001",
  "deploymentBindingId": "skillbind_release_sandbox_001",
  "fromSkillVersionId": "skillver_demo_spreadsheet_v1",
  "toSkillVersionId": "skillver_demo_spreadsheet_v0",
  "reason": "Sandbox validation regression; revert to prior known-good version.",
  "requestedByActorId": "local-operator",
  "requestedAtUtc": "2026-05-27T14:00:00Z",
  "executedAtUtc": "2026-05-27T14:05:00Z",
  "kanonRollbackDecisionId": "kanon_decision_skill_rollback_demo_001",
  "rollbackEvidenceRefs": [
    { "type": "skillReleaseDeploymentBinding", "id": "skillbind_release_sandbox_001" },
    { "type": "skillPromotionRequest", "id": "skillprom_demo_sandbox_001" }
  ],
  "status": "executed"
}
```

## `status` enum

```text
requested
approved
executed
rejected
failed
```

## Rules

- `toSkillVersionId` must match or be compatible with the binding's `rollbackTargetVersionId` when policy requires it.
- Kanon evaluates rollback before execution (001B+ / 001E slices).
- Automatic production rollback triggers are out of scope for v0.

## Related

- [SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md](./SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md)
