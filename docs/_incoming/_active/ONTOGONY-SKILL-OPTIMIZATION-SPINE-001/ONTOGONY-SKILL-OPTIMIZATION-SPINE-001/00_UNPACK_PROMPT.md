# Generic Cursor Unpack Prompt

You are working on the Ontogony multi-repo system. Unpack and implement the package `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001`.

## Objective

Add a governed, cross-service **Skill Optimization Spine**. The goal is to make agent procedural knowledge trainable as versioned external state while preserving Ontogony's governance, evidence, replay, model routing, and operator UX principles.

This is inspired by SkillOpt-style skill optimization, but the implementation must be Ontogony-native. Do not create a standalone prompt-optimizer toy. Build a governed skill lifecycle integrated with Allagma, Kanon, Conexus, the Evidence Spine, and the frontend operator console.

## Repositories

Review and update only what is necessary in these repos:

- `ontogony-platform`
- `allagma-dotnet`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

## First pass: inspect before editing

Before making changes:

1. Inspect current docs, route inventories, OpenAPI baselines, fixtures, Evidence Spine implementations, run lifecycle docs, model routing docs, and frontend console pages.
2. Identify existing objects that already represent runs, traces, model calls, route decisions, human gates, evidence graphs, domain packs, decision records, quality snapshots, and replay fixtures.
3. Reuse existing contracts and naming patterns wherever possible.
4. Do not invent parallel abstractions where an existing Ontogony object can be extended cleanly.
5. Write working notes in:

```text
docs/_incoming_active/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/IMPLEMENTATION_NOTES.md
```

## Required implementation shape

Implement the smallest strong vertical slice:

1. A canonical `SkillArtifact` contract.
2. A canonical `SkillVersion` contract.
3. A canonical `SkillEdit` contract supporting `add`, `delete`, and `replace` operations.
4. A canonical `SkillOptimizationRun` contract with rollout, minibatch reflection, candidate generation, validation, gate, acceptance/rejection, and publish phases.
5. A canonical `SkillEvaluationGate` contract with strict held-out comparison.
6. A `RejectedSkillEditBuffer` or equivalent object that preserves rejected edits as negative evidence.
7. A `SkillDeploymentBinding` contract that controls which agent/domain/harness/profile receives which skill version.
8. At least one Allagma optimization run reconstructed end-to-end.
9. At least one Kanon decision record for accepted/rejected/published skill versions.
10. At least one Conexus target-model call showing active skill injection metadata.
11. A frontend Skill Lab page or panel rendering inventory, lineage, optimization run status, edit diffs, gate results, rejected edit buffer, and deployment binding.
12. Golden tests and fixtures proving accept, reject, rollback, and no-runtime-optimizer behavior.

## Lifecycle semantics

Use this lifecycle as the implementation guide. Names may be adapted to existing conventions, but semantics must remain explicit.

```text
DraftSkill
  -> CandidateSkillVersion
  -> UnderOptimization
  -> GatePending
  -> AcceptedValidated
  -> Published
  -> Deployed
  -> Superseded | Retired | RolledBack
```

Rejected candidate versions are not deleted. They become evidence:

```text
CandidateSkillVersion
  -> GateRejected
  -> RejectedEditBuffer
```

## Skill edit semantics

A skill edit is an atomic patch against a versioned skill document.

Required fields:

```text
editId
skillArtifactId
baseVersionId
candidateVersionId
operation: add | delete | replace
sectionPath
beforeTextHash
proposedTextHash
rationaleSummary
sourceEvidenceRefs
expectedEffect
riskClass
boundedBudgetUnits
generatedByModelCallId
validationStatus
kanonDecisionId
```

Rules:

- Step-level edits must be bounded.
- Candidate changes must preserve section structure unless an explicit migration is declared.
- Slow/meta updates must be protected from step-level edits.
- Deletions require stronger evidence than additions.
- Replacements must include before/after hashes.
- Every accepted edit must be traceable to rollout evidence and a Kanon decision.
- Every rejected edit must preserve its rejection reason and associated gate result.

## Evaluation gate semantics

Use a held-out gate. The minimum rule is strict improvement:

```text
candidateScore > incumbentScore + minDelta
```

Default local fixture values:

```text
minDelta = 0.0
allowTie = false
requiredSampleCount >= 3 for fake fixture
confidenceMode = deterministic-fixture
```

Do not publish a candidate that only improves the train batch. Train evidence is for candidate generation; selection evidence is for acceptance. Test evidence, if present, is for reporting only.

## Governance and evidence rules

Every optimization step should link to evidence fragments:

- Allagma run id.
- Allagma trace/correlation id.
- Conexus optimizer model call id.
- Conexus target rollout model call ids.
- Kanon decision id.
- Evaluation taskset id.
- Evaluation result id.
- Source skill version id.
- Candidate skill version id.

Do not store hidden model chain-of-thought. Use safe reasoning surrogates only: explicit rationale summaries, verifier feedback, failure labels, policy classification, operator notes, and model-visible output rationales when they are part of the external artifact.

## Backend implementation requirements

### Allagma

- Add or extend run type: `SkillOptimization`.
- Add lifecycle operations for start, pause, resume, cancel, validate, accept, reject, publish, deploy, rollback.
- Persist optimization run evidence and state transitions.
- Expose read endpoints for run timeline, candidate versions, edit list, gate results, and replay fixture.
- Integrate with existing replay/runtime operation semantics.

### Kanon

- Add semantic model for skill artifacts and skill-edit governance.
- Validate candidate edits against ontology/domain/policy rules.
- Create decision records for candidate acceptance, rejection, publish, deploy, rollback.
- Add evidence graph roots/edges for skill artifact -> version -> edit -> run -> gate -> deployment.
- Add quality snapshot support for skill versions where appropriate.

### Conexus

- Distinguish `targetModelProfile` from `optimizerModelProfile`.
- Capture optimizer calls separately from target rollout calls.
- Support skill injection metadata on model calls.
- Guarantee normal inference does not call the optimizer.
- Add route/policy metadata for skill-enabled calls.

### Frontend/UI

- Add Skill Lab inventory and detail view.
- Show skill version lineage and status.
- Show candidate edit diffs.
- Show held-out gate comparison.
- Show rejected edit buffer as useful negative evidence, not clutter.
- Show deployment binding and rollback controls.
- Link all major objects into the Evidence Spine.

## Testing requirement

Add tests for:

- contract serialization/deserialization;
- edit operation validation;
- edit budget enforcement;
- acceptance gate strict improvement;
- tie rejection;
- regressing candidate rejection;
- rejected-edit buffer persistence;
- skill deployment binding selection;
- normal inference having zero optimizer calls;
- frontend rendering of accepted, rejected, pending, deployed, and rolled-back states;
- cross-service evidence links.

## Documentation requirement

Update repo docs to explain:

- what a skill is in Ontogony;
- the difference between prompt, skill, policy, route rule, and domain pack;
- how skill optimization uses rollout evidence;
- how candidates are accepted/rejected;
- how skills are deployed and rolled back;
- how the operator reads Skill Lab;
- why hidden chain-of-thought is not persisted.

## Finish condition

The package is complete only when:

- tests pass in modified repos;
- route inventories/OpenAPI snapshots are regenerated where required;
- docs reflect the implemented state, not merely this proposal;
- local fixture data is clearly labeled;
- frontend pages are not crowded with raw debug text;
- a local operator can inspect one deployed skill version and see why it was accepted, where it is deployed, what evidence supports it, and how to roll it back.
