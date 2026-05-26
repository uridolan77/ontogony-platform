# Skill Release Promotion Request Contract V0

## Purpose

`SkillPromotionRequest` is a manual operator request to promote an **accepted** skill candidate version into a **sandbox-only** target environment. Promotion is never implied by skill-edit acceptance or optimization run completion.

## Required fields

```json
{
  "promotionRequestId": "skillprom_demo_sandbox_001",
  "skillArtifactId": "demo.spreadsheet-analysis",
  "candidateVersionId": "skillver_demo_spreadsheet_v1",
  "sourceOptimizationRunId": "skillopt_run_demo_001",
  "requestedByActorId": "local-operator",
  "requestedAtUtc": "2026-05-27T12:00:00Z",
  "targetEnvironment": "sandbox",
  "reason": "Operator review complete; promote accepted candidate to sandbox for harness validation.",
  "status": "approved_for_sandbox",
  "evidenceRefs": [
    { "type": "skillOptimizationRun", "id": "skillopt_run_demo_001" },
    { "type": "kanonDecision", "id": "kanon_decision_skill_accept_demo_001" },
    { "type": "skillReleaseDecision", "id": "kanon_decision_skill_promote_demo_001" }
  ],
  "metadata": {
    "candidateEditId": "skilledit_demo_001",
    "kanonSkillEditDecisionId": "kanon_decision_skill_accept_demo_001"
  }
}
```

## `targetEnvironment` enum (v0 package)

Allowed:

```text
sandbox
local-demo
fixture-only
```

Forbidden in this package:

```text
production
live
default-runtime
```

## `status` enum

```text
created
submitted
pending_decision
approved_for_sandbox
rejected
deferred
cancelled
```

## Rules

- One active promotion per `(skillArtifactId, candidateVersionId, targetEnvironment)` unless superseded with a new reason (enforced in Allagma 001C).
- `sourceOptimizationRunId` must reference a durable optimization run with accepted candidate evidence.
- Submission triggers Kanon `POST /ontology/v0/skill-releases/promotion/evaluate` (001B).
- No binding activation or deployment is performed by this artifact alone.

## Related

- [SKILL_RELEASE_DECISION_V0.md](./SKILL_RELEASE_DECISION_V0.md)
- [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md)
