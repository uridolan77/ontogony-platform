Real Optimizer-Call Planning + Evidence Loop, No Deployment
Goal

Introduce a governed path for real optimizer model calls to generate candidate skill edits, while preserving all current safety boundaries:

real optimizer calls may propose
Kanon may evaluate
Allagma may store
frontend may inspect
nothing deploys
nothing mutates live behavior

001J is the first bridge from fake deterministic candidate generation toward evidence-linked model-assisted skill improvement.

Core principle

001J must not become “self-improving agents.”

It should be:

durable offline optimization run
→ real or fake optimizer call
→ bounded candidate edit proposal
→ Conexus model-call evidence captured
→ Kanon governance evaluation
→ stored candidate
→ operator-visible
→ no deployment
Scope
1. Add optimizer mode to Allagma skill optimization runs

Add an explicit mode:

fake
real_dry_run

or:

SkillOptimizationProposalMode.Fake
SkillOptimizationProposalMode.RealDryRun

Default must remain:

fake

Real mode must require explicit request field, config flag, or operator role.

Example request addition:

ProposalMode: "fake" | "real_dry_run"
OptimizerModelPurpose: string?
MaxOptimizerCalls: int?

Hard constraints:

MaxOptimizerCalls defaults to 1
Budget governance applies
Conexus route evidence must be collected
No live deployment
2. Add proposal generator abstraction

Current fake orchestrator probably generates candidate edits directly.

Introduce a port:

ISkillEditProposalGenerator

Implement:

FakeSkillEditProposalGenerator
ConexusSkillEditProposalGenerator

The fake generator preserves existing behavior.

The Conexus generator should call Conexus through existing model-completion infrastructure, not direct provider SDKs.

3. Define optimizer prompt/request contract

Add a safe prompt contract, not free-form magic.

Inputs:

base skill artifact id
base version id
dataset ref
rollout evidence refs
Conexus model-call evidence summary
candidate edit budget
allowed operations: add / replace / delete
allowed section paths
protected section paths
maximum boundedBudgetUnits

Output must be structured JSON matching:

SkillEditProposalContract[]

No chain-of-thought. Allow only:

safe rationale summary
evidence refs
bounded diff summary

The optimizer must not return raw hidden reasoning.

4. Conexus integration

Conexus remains routing/model-call source, not governance owner.

Use existing Conexus routing and telemetry.

001J should produce:

optimizer model call id
route decision id
cost/usage if known
latency if known
provider/model key
redaction status

The resulting model-call id should be stored with the skill optimization run and included in evidence export.

Add to evidence export:

OptimizerModelCallIds
OptimizerEvidenceRefs
OptimizerEvidenceSummary

or generalize existing evidence refs with roles:

rollout_model_call
optimizer_model_call
validation_model_call

Recommended:

Do not create separate parallel evidence structures if roles are enough.
5. Kanon governance stays mandatory

Every model-generated candidate edit must still pass through:

POST /ontology/v0/skill-edits/evaluate

Kanon remains the accept/reject/defer authority.

Allagma must not accept optimizer output directly.

If Kanon is unavailable:

candidate status = deferred
run status = CandidateGenerated or GatePending, not Accepted
6. Budget/cost governance

Before allowing real optimizer calls, check:

model purpose budget policy
max calls
max estimated cost
max tokens if known

Use existing budget/cost governance patterns from MGMT-006.

Missing telemetry:

unknown remains unknown
do not fabricate cost

If no budget policy exists, decide explicitly:

either block real_dry_run
or allow only fake mode

I recommend:

real_dry_run requires an explicit optimizer budget policy
7. Persistence updates

Because 001H will make storage durable, persist:

proposal mode
optimizer model purpose
optimizer model-call ids
optimizer route evidence refs
generated proposals
raw structured response hash
safe rationale summaries
Kanon decision records
budget snapshot at generation time

Do not persist:

hidden chain-of-thought
raw provider secrets
unredacted prompts if policy forbids
8. API changes

Potential request shape:

CreateSkillOptimizationRunRequest(
    OperatorActorId,
    BaseSkillArtifactId,
    BaseVersionId,
    DatasetRef,
    RolloutEvidenceRefs,
    ConexusModelCallIds,
    ProposalMode,
    OptimizerModelPurpose,
    MaxOptimizerCalls
)

Existing endpoints can remain unchanged.

Do not add frontend start controls yet unless still read-only or clearly fake/dev-only.

9. Evidence export update

GET /skill-optimization/runs/{runId}/evidence should show:

proposal mode
optimizer model purpose
optimizer model-call ids
optimizer evidence refs
rollout evidence refs
candidate edits
Kanon decisions
budget evidence
limitation flags

Limitations should now become more nuanced:

StorageIsNonDurable: false after 001H
LiveDeploymentNotAllowed: true
OptimizerModelCallsAreFake: false when real_dry_run
RealOptimizerCallsAreDryRunOnly: true

Maybe add:

DeploymentBindingDisabled: true
10. Tests

Backend tests:

fake mode preserves existing behavior
real_dry_run requires explicit budget/config
real_dry_run calls proposal generator
real_dry_run stores optimizer model-call evidence
optimizer proposals are sent to Kanon
Kanon rejection is persisted
Kanon deferral is persisted
no live deployment remains true
missing Conexus telemetry remains unknown
optimizer response with invalid JSON is rejected/deferred
optimizer response with unbounded edit is rejected/deferred

Frontend tests can wait for 001K unless DTO changes require smoke updates.

11. Evidence docs

Add:

allagma-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001J.md
conexus-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001J.md if Conexus touched
ontogony-frontend/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001J.md if frontend touched

Include:

real optimizer mode status
model purposes
budget policy
evidence refs
known limitations
safety boundaries
test results
Non-goals

Do not implement:

live deployment
automatic skill activation
runtime skill injection
frontend deploy button
background optimizer jobs
multi-generation search loops
self-modifying agents
unbounded prompt rewrites
hidden chain-of-thought storage
Acceptance criteria

001J is closed when:

1. Fake mode still works unchanged.
2. Real dry-run mode can generate candidate proposals through Conexus.
3. Optimizer model-call evidence is stored and exported.
4. Every generated proposal goes through Kanon evaluation.
5. Invalid/unbounded proposals are rejected or deferred.
6. Budget/cost governance gates real optimizer calls.
7. No live deployment is possible.
8. Evidence export makes the dry-run status obvious.
9. Tests prove fake/real-dry-run separation.
Suggested commit title
feat(skill-opt): add real dry-run optimizer proposal evidence loop