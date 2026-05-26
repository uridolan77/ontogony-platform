# Skill Version Release State Contract V0

## Purpose

`SkillVersionReleaseState` names the release-governance phase for a candidate skill version after optimization acceptance. It bridges optimization outcomes and sandbox promotion without conflating optimization run status with release status.

## State enum (v0 package scope)

```text
accepted_candidate
promotion_requested
approved_for_sandbox
sandbox_bound
sandbox_active
sandbox_paused
rolled_back
rejected_for_release
```

## Minimal first-package path

```text
accepted_candidate
  → promotion_requested
  → approved_for_sandbox
  → sandbox_bound
  → sandbox_active
  → rolled_back
```

## Deferred states (later packages)

```text
production_approved
production_active
progressive_rollout
automatic_promotion
continuous_learning_loop
```

## Identifier anchors

A version in release governance is addressed by:

```text
skillArtifactId
candidateVersionId (or bound skillVersionId)
sourceOptimizationRunId
candidateEditId (optional)
kanonSkillEditDecisionId
promotionRequestId (once created)
deploymentBindingId (once bound)
```

## Authority

| Transition | Authority |
| --- | --- |
| `accepted_candidate` | Skill Optimization + Kanon skill-edit governance |
| `promotion_requested` → `approved_for_sandbox` | Kanon release decision |
| `sandbox_bound` / `sandbox_active` | Allagma orchestration after Kanon allow |
| `rolled_back` | Kanon rollback decision + Allagma execution |

## Related

- [SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md](../protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md)
