# UI Components and State

## Recommended reusable components

In `ontogony-ui` or local frontend components, depending on current architecture:

```text
SkillArtifactCard
SkillVersionBadge
SkillLifecycleBadge
SkillGateScorePanel
SkillEditDiffCard
RejectedEditBufferPanel
SkillDeploymentBindingCard
SkillEvidenceLinksList
SkillOptimizationTimeline
SkillLineageTimeline
```

## State models

Mirror backend contracts but keep UI projections small.

### SkillArtifactSummary

```ts
export interface SkillArtifactSummary {
  skillArtifactId: string;
  name: string;
  domainId: string;
  status: 'draft' | 'active' | 'retired' | 'archived';
  activeVersionId?: string;
  lastGateStatus?: string;
  deploymentStatus?: string;
  tags: string[];
}
```

### SkillGateProjection

```ts
export interface SkillGateProjection {
  skillEvaluationGateId: string;
  incumbentScore: number;
  candidateScore: number;
  delta: number;
  status: string;
  allowTie: boolean;
  minDelta: number;
  decisionReason: string;
}
```

### SkillEditProjection

```ts
export interface SkillEditProjection {
  skillEditId: string;
  operation: 'add' | 'delete' | 'replace' | 'insert' | 'move';
  sectionPath: string;
  riskClass: 'low' | 'medium' | 'high' | 'blocked';
  validationStatus: string;
  decisionStatus?: string;
  rationaleSummary: string;
  expectedEffect: string;
  evidenceCount: number;
}
```

## UX rules

- Do not show full skill markdown on every page. Use preview with open/full view.
- Do not show all evidence links expanded by default.
- Do not hide rejected edits; collapse them under a clear negative-evidence section.
- Use one primary action per state.
- Make deployment status more visually important than optimization internals.

## Frontend tests

Add tests for:

- accepted candidate rendering;
- rejected candidate rendering;
- gate tie rejection;
- deployment binding active/inactive;
- no active skill empty state;
- evidence links rendering;
- diff card for add/delete/replace.
