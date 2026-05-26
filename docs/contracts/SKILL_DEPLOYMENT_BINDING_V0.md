# SkillDeploymentBinding Contract V0

## Purpose

`SkillDeploymentBinding` controls where an accepted/published skill version is active for **Conexus target-model runtime injection** (optimization spine).

For **sandbox release promotion** bindings (manual promotion → Kanon release decision → activate/pause/rollback), see [SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md](./SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md) and [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md).

## Required fields

```json
{
  "skillDeploymentBindingId": "skillbind_demo_local_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "skillVersionId": "skillver_demo_spreadsheet_v1",
  "status": "active",
  "environment": "local",
  "scope": {
    "agentProfileId": "local-fixture-agent",
    "domainId": "demo",
    "harness": "direct",
    "modelRouteProfile": "fake-target-local",
    "taskKinds": ["spreadsheet-qa"]
  },
  "precedence": 100,
  "activatedAt": "2026-05-26T00:00:00Z",
  "activatedByActorId": "local-operator",
  "authorizedByKanonDecisionId": "kanon_decision_skill_deploy_demo_001",
  "rollback": {
    "previousBindingId": null,
    "previousSkillVersionId": "skillver_demo_spreadsheet_v0",
    "rollbackAllowed": true
  }
}
```

## Status enum

```text
pending
active
inactive
superseded
rolledBack
retired
```

## Rules

- Deployment is never implied by acceptance.
- Deployment requires a publish/deploy decision.
- Deployment scope must be explicit.
- If multiple bindings match, deterministic precedence rules must select one active version.
- Conexus model-call metadata must record the resolved binding id and skill version id.
