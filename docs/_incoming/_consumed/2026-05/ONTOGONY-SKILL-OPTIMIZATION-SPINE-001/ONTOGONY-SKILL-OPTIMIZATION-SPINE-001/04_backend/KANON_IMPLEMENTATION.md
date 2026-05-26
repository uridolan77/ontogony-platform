# Kanon Implementation Plan

## Role

Kanon owns semantic authority, skill-edit governance, policy validation, decision records, and evidence graph integration.

## New semantic objects

Add ontology/domain-pack concepts for:

```text
SkillArtifact
SkillVersion
SkillEdit
SkillOptimizationRun
SkillEvaluationGate
RejectedSkillEditBuffer
SkillDeploymentBinding
```

Do not duplicate Allagma orchestration state. Kanon should classify, decide, validate, and connect evidence.

## Decision lifecycle

Create decision records for:

```text
skill.edit.validate
skill.candidate.accept
skill.candidate.reject
skill.version.publish
skill.version.deploy
skill.deployment.rollback
skill.version.retire
```

Each decision should include:

```text
subjectRef
policyBasis
authorityEffect
operatorActorId
inputEvidenceRefs
outputAction
postConditionSummary
safeRationaleSummary
```

## Validation service

Suggested service:

```text
SkillEditGovernanceService
```

Responsibilities:

- Validate known skill artifact/version ids.
- Validate operation type.
- Validate section path.
- Enforce edit budget.
- Reject protected-section mutation unless epoch/meta update.
- Risk-classify delete/replace operations.
- Detect obvious contradictions with domain pack rules.
- Emit missing evidence diagnostics.

## Skill authority effects

Recommended enum:

```text
NoAuthorityEffect
CandidateOnly
ValidatedImprovement
PublishedProcedure
ActiveDeployment
SupersededProcedure
RolledBackProcedure
RetiredProcedure
```

## Evidence graph

Add enriched graph roots for:

```text
skillArtifactId
skillVersionId
skillOptimizationRunId
skillEvaluationGateId
skillDeploymentBindingId
```

Graph edges:

```text
artifact -> version
version -> baseVersion
version -> acceptedEdits
edit -> sourceEvidence
edit -> optimizerCall
edit -> kanonDecision
run -> rolloutEvidence
run -> gate
gate -> incumbentVersion
gate -> candidateVersion
gate -> kanonDecision
binding -> skillVersion
binding -> modelRouteProfile
binding -> deployDecision
```

## Read endpoints

Adapt to Kanon route conventions. Suggested routes:

```text
GET /ontology/v0/skills/{skillArtifactId}
GET /ontology/v0/skills/{skillArtifactId}/versions
GET /ontology/v0/skill-versions/{skillVersionId}
GET /ontology/v0/skill-versions/{skillVersionId}/evidence-graph
GET /ontology/v0/skill-edits/{skillEditId}/governance
GET /ontology/v0/skill-evaluation-gates/{gateId}
GET /ontology/v0/skill-deployments/{bindingId}/authority
```

## Write/decision endpoints

Only if existing architecture allows Kanon write routes. Otherwise keep decisions created by internal services.

```text
POST /ontology/v0/skill-edits/{skillEditId}/validate
POST /ontology/v0/skill-versions/{skillVersionId}/publish-decision
POST /ontology/v0/skill-deployments/{bindingId}/authorize
```

## Tests

Add tests for:

- edit validation pass/fail;
- protected slow/meta update rejection;
- budget exceeded rejection;
- delete operation risk class;
- candidate acceptance decision;
- candidate rejection decision;
- publish/deploy authority effect;
- evidence graph fan-out;
- route inventory/OpenAPI drift.

## Docs

Update:

- ontology route inventory;
- decision lifecycle docs;
- evidence spine docs;
- domain pack/operator guide.
