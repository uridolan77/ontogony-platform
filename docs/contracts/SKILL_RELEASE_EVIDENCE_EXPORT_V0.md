# Skill Release Evidence Export Contract V0

## Purpose

`SkillReleaseEvidenceExport` consolidates reconstructable release-governance evidence for operator audit: promotion request, source optimization evidence, Kanon decisions, binding/rollback state, and explicit limitation flags.

## Required fields

```json
{
  "evidenceExportId": "skillrelease_evidence_promo_001",
  "promotionRequestId": "skillprom_demo_sandbox_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "candidateVersionId": "skillver_demo_spreadsheet_v1",
  "exportedAtUtc": "2026-05-27T12:20:00Z",
  "promotionRequest": { "promotionRequestId": "skillprom_demo_sandbox_001", "status": "approved_for_sandbox" },
  "sourceOptimizationRunId": "skillopt_run_demo_001",
  "kanonSkillEditDecisionId": "kanon_decision_skill_accept_demo_001",
  "kanonReleaseDecisionId": "kanon_decision_skill_promote_demo_001",
  "deploymentBindingId": "skillbind_release_sandbox_001",
  "rollbackTargetVersionId": "skillver_demo_spreadsheet_v0",
  "evidenceRefs": [
    { "type": "skillOptimizationRun", "id": "skillopt_run_demo_001" },
    { "type": "skillPromotionRequest", "id": "skillprom_demo_sandbox_001" },
    { "type": "skillReleaseDecision", "id": "kanon_decision_skill_promote_demo_001" }
  ],
  "limitations": {
    "productionDeploymentAllowed": false,
    "sandboxOnly": true,
    "automaticPromotionAllowed": false,
    "rollbackRequired": true,
    "liveDeploymentAllowed": false
  }
}
```

## Limitation flags (v0)

| Flag | v0 value | Meaning |
| --- | --- | --- |
| `productionDeploymentAllowed` | `false` | No production binding in this package |
| `sandboxOnly` | `true` | Targets limited to sandbox/local-demo/fixture-only |
| `automaticPromotionAllowed` | `false` | Manual operator promotion only |
| `rollbackRequired` | `true` | Rollback target must be defined before activation when policy requires |
| `liveDeploymentAllowed` | `false` | No live consumer activation |

## Evidence reference types (release slice)

```text
skillOptimizationRun
skillPromotionRequest
skillReleaseDecision
skillReleaseDeploymentBinding
skillRollback
kanonDecision
conexusModelCall
```

## Related

- [SKILL_EVIDENCE_LINKS_V0.md](./SKILL_EVIDENCE_LINKS_V0.md) (optimization spine links)
- [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md)
