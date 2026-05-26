# Contract Drift Tests

## Goal

Prevent the Skill Optimization Spine from drifting across repos.

## Backend drift checks

- JSON schema examples deserialize in .NET DTOs.
- OpenAPI snapshots include new routes where added.
- Route inventories are regenerated.
- Enums match frontend TypeScript unions.
- Fixture ids match across Allagma/Kanon/Conexus/frontend.

## Frontend drift checks

- TypeScript fixtures conform to backend JSON examples.
- UI handles unknown future enum values safely.
- Missing optional fields do not crash pages.
- Skill gate score formatting is stable.

## Suggested manifest

Add a compatibility manifest entry:

```json
{
  "feature": "skill_optimization_spine_v0",
  "contracts": [
    "SkillArtifactContractV0",
    "SkillVersionContractV0",
    "SkillEditContractV0",
    "SkillOptimizationRunContractV0",
    "SkillEvaluationGateContractV0",
    "SkillDeploymentBindingContractV0"
  ],
  "fixtures": ["demo.spreadsheet-analysis"],
  "gate": "strict_held_out_improvement"
}
```
