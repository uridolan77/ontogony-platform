# `ONTOGONY-SKILL-RELEASE-GOVERNANCE-001`

## Manual Skill Promotion, Sandbox Activation, Rollback, and Release Gates

## Executive goal

After `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001`, the system can produce **candidate skill improvements** from reconstructable traces, evaluate them, govern them through Kanon, store them durably, and expose them in Skill Lab.

The next question is:

```text
How does an accepted skill candidate become safely usable?
```

This package answers that question without jumping to autonomous production deployment.

The goal is to introduce a governed release layer:

```text
accepted candidate skill version
→ manual promotion request
→ Kanon release decision
→ sandbox deployment binding
→ sandbox activation
→ rollback path
→ evidence export
```

No automatic live mutation. No production activation. No self-modifying agents.

---

# 1. Package boundary

## Package name

```text
ONTOGONY-SKILL-RELEASE-GOVERNANCE-001
```

## One-line description

```text
Governed manual promotion and sandbox activation of accepted skill candidate versions, with release evidence, rollback, and no production deployment.
```

## Core rule

This package should move the system from:

```text
“we can generate and inspect candidate skills”
```

to:

```text
“we can safely promote, bind, test, inspect, and roll back candidate skills in sandbox.”
```

It must **not** move the system to:

```text
“agents automatically rewrite their own live behavior.”
```

---

# 2. Preconditions

Do this package only after the Skill Optimization spine has completed:

```text
001H — Durable SkillOptimizationRun persistence
001I — Skill Lab API/error semantics + OpenAPI parity cleanup
001J — Real dry-run optimizer proposal evidence loop
```

Minimum technical prerequisites:

```text
1. Skill optimization runs are durable.
2. Accepted/rejected/deferred candidate edits are persisted.
3. Evidence export includes candidate edits, Kanon decisions, Conexus optimizer evidence, gate results, and limitation flags.
4. Frontend Skill Lab can inspect runs and candidates.
5. Real optimizer calls, if present, remain dry-run only.
6. No skill candidate can affect live execution yet.
```

---

# 3. System roles

## Ontogony Platform

Owns neutral protocol and schema definitions:

```text
SkillVersionReleaseContract
SkillPromotionRequestContract
SkillDeploymentBindingContract
SkillRollbackContract
SkillReleaseEvidenceExportContract
```

## Kanon

Owns semantic/governance authority:

```text
Can this candidate skill version be promoted to sandbox?
Can this sandbox binding be activated?
Should this binding be rolled back?
```

Kanon does **not** orchestrate execution.

## Allagma

Owns lifecycle orchestration:

```text
promotion request
release workflow
sandbox binding creation
activation/deactivation
rollback operation
release evidence export
```

Allagma does **not** decide semantic approval.

## Conexus

Remains model-call/routing/evidence source.

For this package, Conexus should mostly be read-only unless the release workflow needs model-call evidence from optimization runs.

Conexus does **not** deploy skills.

## Frontend

Owns operator experience:

```text
promotion request UI
release evidence view
sandbox binding view
rollback controls
status/limitation badges
```

Frontend must not expose production deployment controls.

## Ontogony UI

Only receives reusable presentational components if needed:

```text
ReleaseStatusBadge
DeploymentBindingCard
RollbackBanner
EvidenceSummaryPanel
```

Do not over-extract too early.

---

# 4. Release lifecycle

Define a clear lifecycle for skill versions.

## Candidate skill version lifecycle

```text
Candidate
→ AcceptedBySkillEditGovernance
→ PromotionRequested
→ ApprovedForSandbox
→ SandboxBound
→ SandboxActive
→ SandboxPaused
→ RolledBack
→ Deprecated
→ RejectedForRelease
```

## Minimal first package lifecycle

For `SKILL-RELEASE-GOVERNANCE-001`, implement only:

```text
AcceptedCandidate
→ PromotionRequested
→ ApprovedForSandbox
→ SandboxBound
→ SandboxActive
→ RolledBack
```

Explicitly defer:

```text
ProductionApproved
ProductionActive
ProgressiveRollout
AutomaticPromotion
ContinuousLearningLoop
```

---

# 5. Core domain concepts

## 5.1 Skill candidate

Already created by Skill Optimization.

Important identifiers:

```text
skillArtifactId
baseVersionId
candidateVersionId
optimizationRunId
candidateEditId
kanonSkillEditDecisionId
evidenceExportId
```

## 5.2 Promotion request

A manual operator request to promote an accepted candidate to sandbox.

Fields:

```text
promotionRequestId
skillArtifactId
candidateVersionId
sourceOptimizationRunId
requestedByActorId
requestedAtUtc
targetEnvironment
reason
evidenceRefs
status
metadata
```

Allowed `targetEnvironment` in this package:

```text
sandbox
local-demo
fixture-only
```

Forbidden:

```text
production
live
default-runtime
```

## 5.3 Release decision

Kanon decision record saying whether promotion is allowed.

Fields:

```text
decisionId
promotionRequestId
skillArtifactId
candidateVersionId
decisionStatus
policyBasis
safeRationaleSummary
blockingReasons
requiredEvidence
createdAtUtc
```

Decision statuses:

```text
approved_for_sandbox
rejected
deferred
```

## 5.4 Deployment binding

A binding between a skill version and a sandbox target.

Fields:

```text
deploymentBindingId
skillArtifactId
skillVersionId
targetConsumerId
targetEnvironment
bindingStatus
createdByActorId
createdAtUtc
activatedByActorId
activatedAtUtc
releaseDecisionId
rollbackTargetVersionId
evidenceExportId
metadata
```

Binding statuses:

```text
created
active
paused
rolled_back
deprecated
```

## 5.5 Rollback record

A first-class rollback artifact.

Fields:

```text
rollbackId
deploymentBindingId
fromSkillVersionId
toSkillVersionId
reason
requestedByActorId
requestedAtUtc
executedAtUtc
kanonRollbackDecisionId
rollbackEvidenceRefs
status
```

Rollback statuses:

```text
requested
approved
executed
rejected
failed
```

---

# 6. Contract plan

## Platform contracts

Add under `ontogony-platform`:

```text
docs/contracts/SKILL_RELEASE_PROMOTION_REQUEST_V0.md
docs/contracts/SKILL_RELEASE_DECISION_V0.md
docs/contracts/SKILL_DEPLOYMENT_BINDING_V0.md
docs/contracts/SKILL_ROLLBACK_V0.md
docs/contracts/SKILL_RELEASE_EVIDENCE_EXPORT_V0.md
docs/protocols/SKILL_RELEASE_GOVERNANCE.md
docs/protocols/SKILL_SANDBOX_ACTIVATION_LIFECYCLE.md
```

JSON schemas:

```text
docs/schemas/skill-release/skill-promotion-request.v0.schema.json
docs/schemas/skill-release/skill-release-decision.v0.schema.json
docs/schemas/skill-release/skill-deployment-binding.v0.schema.json
docs/schemas/skill-release/skill-rollback.v0.schema.json
docs/schemas/skill-release/skill-release-evidence-export.v0.schema.json
```

Fixtures:

```text
docs/schemas/fixtures/skill-release/promotion-request.approved-sandbox.json
docs/schemas/fixtures/skill-release/deployment-binding.sandbox-active.json
docs/schemas/fixtures/skill-release/rollback.executed.json
```

Platform tests:

```text
SkillReleaseGovernanceSchemaTests
```

Test expectations:

```text
schemas load
fixtures validate
lifecycle status enums are stable
no production target environment in first package fixtures
```

---

# 7. Kanon slice

## Goal

Kanon defines and enforces release governance semantics.

## New decision types

Add decision types:

```text
SkillVersionPromotionDecision
SkillDeploymentBindingDecision
SkillRollbackDecision
```

## New endpoint candidates

Preferred endpoint shape:

```text
POST /ontology/v0/skill-releases/promotion/evaluate
POST /ontology/v0/skill-releases/binding/evaluate
POST /ontology/v0/skill-releases/rollback/evaluate
```

For the first slice, you can start with only:

```text
POST /ontology/v0/skill-releases/promotion/evaluate
```

## Promotion evaluation request

```csharp
SkillPromotionEvaluationRequest(
    PromotionRequestId,
    SkillArtifactId,
    CandidateVersionId,
    SourceOptimizationRunId,
    TargetEnvironment,
    EvidenceRefs,
    RequestedByActorId,
    Reason
)
```

## Promotion evaluation result

```csharp
SkillPromotionEvaluationResult(
    DecisionId,
    PromotionRequestId,
    DecisionStatus,
    TargetEnvironment,
    SafeRationaleSummary,
    BlockingReasons,
    RequiredEvidence,
    CreatedAtUtc
)
```

## Deterministic fake governance rules

For first implementation:

Approve only if:

```text
targetEnvironment is sandbox/local-demo/fixture-only
candidate has accepted Kanon skill-edit decision
optimization evidence export exists
liveDeploymentAllowed is false in source optimization gate
release request has operator actor
rollback target is defined or explicitly not required for first activation
```

Reject if:

```text
targetEnvironment = production/live
missing skill artifact id
missing candidate version id
missing source optimization run id
candidate decision is rejected
request tries to enable automatic deployment
```

Defer if:

```text
evidence export missing
candidate decision unknown
rollback target unknown
Kanon cannot verify source evidence
```

## Kanon tests

```text
Approve_sandbox_promotion_for_accepted_candidate
Reject_production_target
Reject_missing_candidate_version
Reject_rejected_skill_edit_candidate
Defer_missing_optimization_evidence
Defer_missing_rollback_target_when_required
Safe_rationale_contains_no_hidden_reasoning
Route_inventory_and_OpenAPI_are_synchronized
```

## Kanon non-goals

Do not implement:

```text
execution
binding activation
deployment state
frontend
automatic promotion
```

---

# 8. Allagma slice

## Goal

Allagma orchestrates promotion, sandbox binding, activation, rollback, and release evidence.

## New domain objects

```text
SkillPromotionRequestRecord
SkillDeploymentBindingRecord
SkillRollbackRecord
SkillReleaseEvidenceExportRecord
```

## Persistence

Since Skill Optimization 001H should have already added durable storage, release governance should be durable from the beginning.

Tables:

```text
allagma_skill_promotion_requests
allagma_skill_deployment_bindings
allagma_skill_rollbacks
allagma_skill_release_evidence_exports
```

## Repository interfaces

```csharp
ISkillPromotionRequestRepository
ISkillDeploymentBindingRepository
ISkillRollbackRepository
ISkillReleaseEvidenceRepository
```

## Services

```csharp
SkillPromotionService
SkillDeploymentBindingService
SkillRollbackService
SkillReleaseEvidenceExportService
```

## API endpoints

### Promotion

```text
POST /allagma/v0/skill-releases/promotions
GET  /allagma/v0/skill-releases/promotions
GET  /allagma/v0/skill-releases/promotions/{promotionRequestId}
POST /allagma/v0/skill-releases/promotions/{promotionRequestId}/submit
```

### Binding

```text
POST /allagma/v0/skill-releases/bindings
GET  /allagma/v0/skill-releases/bindings
GET  /allagma/v0/skill-releases/bindings/{bindingId}
POST /allagma/v0/skill-releases/bindings/{bindingId}/activate
POST /allagma/v0/skill-releases/bindings/{bindingId}/pause
```

### Rollback

```text
POST /allagma/v0/skill-releases/bindings/{bindingId}/rollback
GET  /allagma/v0/skill-releases/rollbacks/{rollbackId}
```

### Evidence

```text
GET /allagma/v0/skill-releases/promotions/{promotionRequestId}/evidence
GET /allagma/v0/skill-releases/bindings/{bindingId}/evidence
```

## First-slice endpoint subset

To keep the first package sane, implement only:

```text
POST /allagma/v0/skill-releases/promotions
GET  /allagma/v0/skill-releases/promotions
GET  /allagma/v0/skill-releases/promotions/{promotionRequestId}
POST /allagma/v0/skill-releases/promotions/{promotionRequestId}/submit
GET  /allagma/v0/skill-releases/promotions/{promotionRequestId}/evidence
```

Then add binding/rollback in sub-slices.

---

# 9. Allagma lifecycle

## Promotion request lifecycle

```text
Created
→ SubmittedToKanon
→ ApprovedForSandbox
→ Rejected
→ Deferred
→ Cancelled
```

## Binding lifecycle

```text
Created
→ Active
→ Paused
→ RolledBack
```

## Rollback lifecycle

```text
Requested
→ Approved
→ Executed
→ Failed
```

## Required invariants

```text
No binding can be created without approved sandbox promotion.
No binding can target production.
No activation can occur without rollback target or rollback declaration.
No rollback can execute without actor id and reason.
No release operation can erase optimization evidence.
```

---

# 10. Allagma tests

## Promotion tests

```text
Create_promotion_request_for_accepted_candidate
Create_rejects_missing_candidate_version
Submit_promotion_calls_Kanon_release_evaluation
Submit_approved_promotion_sets_ApprovedForSandbox
Submit_rejected_promotion_sets_Rejected
Submit_deferred_promotion_sets_Deferred
Promotion_evidence_export_contains_optimization_run_refs
Promotion_cannot_target_production
Promotion_is_durable_after_repository_reload
```

## Binding tests

```text
Create_binding_requires_approved_promotion
Create_binding_rejects_production_target
Activate_binding_sets_active_status
Activate_binding_preserves_no_production_boundary
Pause_binding_sets_paused_status
```

## Rollback tests

```text
Rollback_requires_active_binding
Rollback_requires_reason
Rollback_records_from_and_to_versions
Rollback_sets_binding_rolled_back
Rollback_evidence_export_contains_prior_binding_state
```

## Route/OpenAPI tests

```text
SkillReleaseRoutes_in_inventory
SkillReleaseRoutes_in_OpenAPI
ServerOnly_or_ClientCoverage_classification_is_explicit
Error_envelopes_are_consistent
```

---

# 11. Frontend slice

## Goal

Extend Skill Lab from “optimization inspection” to “release governance inspection and manual sandbox promotion.”

## New pages

```text
/allagma/skill-lab/releases
/allagma/skill-lab/releases/:promotionRequestId
/allagma/skill-lab/bindings
/allagma/skill-lab/bindings/:bindingId
```

## Add to existing Skill Optimization detail page

For an accepted candidate edit, show:

```text
Request sandbox promotion
```

But only if:

```text
candidate decisionStatus = accepted
run gate result exists
liveDeploymentAllowed = false
target = sandbox only
```

Button label must be explicit:

```text
Request sandbox promotion
```

Not:

```text
Deploy
Activate
Use skill
```

## Promotion request page

Show:

```text
promotion status
skill artifact
candidate version
source optimization run
target environment
Kanon release decision
blocking reasons
evidence refs
operator actor
created/submitted timestamps
```

## Binding page

Show:

```text
binding status
skill artifact/version
target consumer
target environment
activation actor
rollback target
release evidence
```

## Rollback UI

Allow only:

```text
Rollback sandbox binding
```

Require:

```text
reason text
confirmation
```

No production wording.

## UI warnings

Always show banners:

```text
Sandbox only
No production deployment
Manual release governance
Rollback required
```

---

# 12. Frontend tests

## Unit tests

```text
ReleaseStatusBadge maps statuses correctly
PromotionButton hidden for non-accepted candidates
PromotionButton disabled without evidence export
PromotionButton labels sandbox explicitly
Rollback form requires reason
```

## Playwright tests

```text
Skill Lab accepted candidate shows Request sandbox promotion
Promotion request can be created in mocked API
Promotion detail shows Kanon decision
Rejected promotion shows blocking reasons
Sandbox binding detail shows no production deployment
Rollback requires reason
```

## Route catalog

Add coverage for:

```text
/allagma/skill-lab/releases
/allagma/skill-lab/releases/:promotionRequestId
/allagma/skill-lab/bindings
/allagma/skill-lab/bindings/:bindingId
```

---

# 13. Evidence export design

Release evidence export should consolidate:

```text
promotion request
source optimization run evidence export
candidate edit summary
Kanon skill-edit decision
Kanon release decision
Conexus optimizer evidence refs
evaluation/gate result
deployment binding if created
rollback plan
limitation flags
```

Example limitation flags:

```csharp
SkillReleaseLimitations(
    ProductionDeploymentAllowed: false,
    SandboxOnly: true,
    AutomaticPromotionAllowed: false,
    RollbackRequired: true
)
```

Evidence endpoint:

```text
GET /allagma/v0/skill-releases/promotions/{promotionRequestId}/evidence
```

Later:

```text
GET /allagma/v0/skill-releases/bindings/{bindingId}/evidence
```

---

# 14. Security and governance boundaries

## Required auth classes

Read endpoints:

```text
OperatorRead
Auditor
Admin
```

Mutation endpoints:

```text
OperatorMutate
Admin
System
```

Promotion/activation should require stronger role than read.

## Header propagation

Preserve:

```text
X-Ontogony-Actor-Id
X-Ontogony-Roles
X-Ontogony-Correlation-Id
X-Ontogony-Trace-Id
```

## Idempotency

All mutation endpoints should use idempotency keys:

```text
POST /promotions
POST /promotions/{id}/submit
POST /bindings
POST /bindings/{id}/activate
POST /bindings/{id}/rollback
```

No duplicated promotion requests for the same:

```text
skillArtifactId + candidateVersionId + targetEnvironment
```

unless explicitly allowed with a new request reason.

---

# 15. Route/OpenAPI discipline

For every backend repo touched:

```text
route catalog updated
route inventory regenerated
OpenAPI baseline regenerated
compatibility manifest updated
client coverage or ServerOnly status explicit
docs generated
tests updated
```

For frontend:

```text
OpenAPI snapshots synced
route parity gaps updated/removed
release-route-catalog updated
coverage-catalog updated
route smoke coverage updated
```

No hand-edited generated truth unless repo convention allows it.

---

# 16. Non-goals

This package must not implement:

```text
production deployment
automatic deployment
runtime skill injection into real traffic
background optimization cycles
continuous self-improvement
unbounded skill rewrites
deployment to external customers
Conexus provider config mutation
hidden chain-of-thought storage
```

Also avoid:

```text
multi-environment rollout percentages
A/B production testing
automatic rollback triggers
real consumer activation
```

Those are later packages.

---

# 17. Recommended sub-slices

## `SKILL-RELEASE-GOVERNANCE-001A`

### Platform contracts and protocol docs

Deliver:

```text
contracts
schemas
fixtures
schema tests
lifecycle protocol
non-goals
```

No runtime code yet.

---

## `001B`

### Kanon release governance fake vertical slice

Deliver:

```text
promotion evaluation endpoint
decision type
deterministic fake policy
tests
route/OpenAPI updates
evidence doc
```

Only sandbox approval/reject/defer.

---

## `001C`

### Allagma promotion request registry

Deliver:

```text
promotion request domain
persistence
create/list/get/submit endpoints
Kanon client integration
evidence export
tests
```

No binding activation yet.

---

## `001D`

### Allagma sandbox deployment binding

Deliver:

```text
binding domain
create/list/get endpoints
activate/pause
sandbox-only enforcement
rollback target field
tests
```

No production.

---

## `001E`

### Rollback lifecycle

Deliver:

```text
rollback request
rollback evaluation or direct governed rollback
rollback execution
binding status rolled_back
rollback evidence export
tests
```

Rollback should be implemented before any more realistic activation.

---

## `001F`

### Frontend release governance UI

Deliver:

```text
promotion request UI
release detail page
binding detail page
rollback form
limitation badges
Playwright smoke
```

No production controls.

---

## `001G`

### Cross-service release golden fixture

Deliver a fixture proving:

```text
optimization run accepted candidate
→ promotion request
→ Kanon approval
→ sandbox binding
→ activation
→ rollback
→ release evidence export
```

This should become the canonical release-governance golden trace.

---

# 18. Acceptance criteria for full package

The package is complete when:

```text
1. Accepted skill candidates can be promoted manually to sandbox.
2. Kanon governs promotion approval/rejection/defer.
3. Allagma persists promotion and binding lifecycle.
4. Release evidence export links back to optimization evidence.
5. Sandbox binding can be activated.
6. Rollback exists and is tested.
7. No production deployment path exists.
8. Frontend exposes release governance clearly and safely.
9. Route/OpenAPI/client truth is synchronized.
10. Cross-service golden fixture proves the lifecycle.
```

---

# 19. Suggested package prompt title

```text
ONTOGONY-SKILL-RELEASE-GOVERNANCE-001
Manual skill promotion, sandbox activation, rollback, and release evidence
```

Suggested first commit title:

```text
docs(skill-release): add skill release governance protocol and contracts
```

Suggested runtime commit titles:

```text
feat(kanon): add sandbox skill promotion governance evaluation
feat(allagma): add durable skill promotion request lifecycle
feat(allagma): add sandbox skill deployment bindings
feat(allagma): add governed skill rollback lifecycle
feat(frontend): add Skill Lab release governance views
test(system): add skill release governance golden fixture
```

---

# 20. Strategic position

After this package, the system maturity becomes:

```text
trace reconstruction
→ skill optimization
→ release governance
→ sandbox activation
→ rollback
```

Only then should you consider:

```text
ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001
```

That next package can connect a real consumer, such as the anti-sycophancy chat, to sandbox-bound skill versions.

But do **not** connect consumers before release governance exists. Otherwise the system has optimized skills but no safe binding/rollback discipline.
