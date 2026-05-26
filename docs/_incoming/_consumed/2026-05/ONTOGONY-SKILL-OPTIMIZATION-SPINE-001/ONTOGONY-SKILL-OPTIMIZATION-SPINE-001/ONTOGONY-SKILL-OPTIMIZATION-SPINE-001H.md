Durable SkillOptimizationRun Storage in Allagma
Goal

Turn Skill Optimization from an in-memory fake/offline console artifact into a durable, replayable, inspectable operator artifact.

Current weakness:

SkillOptimizationRun exists only in memory.
If Allagma restarts, the Skill Lab state disappears.

001H should fix that without adding live deployment, real optimizer calls, or production skill mutation.

Scope
1. Add persistent domain/storage model

Add EF/Postgres persistence for:

SkillOptimizationRun
SkillOptimizationCandidateEdit
SkillOptimizationEvidenceRef
SkillOptimizationEvidenceSummary
SkillOptimizationGateResult

Recommended storage shape:

allagma_skill_optimization_runs
allagma_skill_optimization_candidate_edits
allagma_skill_optimization_evidence_refs
allagma_skill_optimization_gate_results

Keep this explicitly separate from real skill deployment.

2. Replace in-memory repository

Current:

InMemorySkillOptimizationRunRepository

Add:

EfSkillOptimizationRunRepository

It should support:

CreateAsync
GetAsync
ListAsync
UpdateAsync / SaveAsync

Keep the in-memory repo only for tests or fake-mode fallback.

3. Add migration

Add migration:

SkillOptimizationRunPersistence001

or similar repo-convention name.

The migration should include:

run id
status
operator actor id
base skill artifact id
base version id
dataset ref
rollout evidence refs
Conexus model call ids
candidate edits
resolved evidence refs
evidence summary
gate result
metadata
created / updated timestamps

For flexible fields, JSONB is acceptable if Allagma already uses that style.

4. Preserve existing API shape

Do not change frontend contracts unless necessary.

Existing endpoints should continue to work:

POST /allagma/v0/skill-optimization/runs
GET  /allagma/v0/skill-optimization/runs
GET  /allagma/v0/skill-optimization/runs/{runId}
GET  /allagma/v0/skill-optimization/runs/{runId}/operations
POST /allagma/v0/skill-optimization/runs/{runId}/start
POST /allagma/v0/skill-optimization/runs/{runId}/cancel
GET  /allagma/v0/skill-optimization/runs/{runId}/evidence

The only intended behavioral change:

runs survive process restart
5. Update limitation flags

Once persistence is real, this must change:

StorageIsNonDurable: true

to:

StorageIsNonDurable: false

But only when using the EF repository.

If you keep fake/in-memory mode, expose the truth per runtime mode:

StorageIsNonDurable = repositoryMode == "in-memory"
6. Add persistence tests

Minimum test set:

Create_run_persists_to_repository
Started_run_persists_candidate_edits
Started_run_persists_conexus_evidence_refs
Started_run_persists_evidence_summary
Started_run_persists_gate_result
Evidence_export_survives_repository_reload
Cancelled_run_persists_cancelled_status
List_runs_returns_persisted_runs
Storage_limitation_flag_false_when_using_durable_repository

If Allagma has Postgres smoke tests, add one targeted persistence smoke.

7. Route/OpenAPI discipline

No new routes required unless you add repository-mode diagnostics.

Still update evidence docs and generated truth if contract shape changes.

8. Evidence doc

Add:

docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001H.md

Include:

scope
schema/migration
repository mode
tests
known limitations
explicit non-goals
Non-goals

Do not implement:

live skill deployment
real optimizer model calls
automatic skill mutation
Conexus skill injection
frontend mutation controls
production skill registry binding
Acceptance criteria

001H is closed when:

1. Skill optimization runs are durable.
2. Candidate edits survive restart/reload.
3. Evidence refs survive restart/reload.
4. Evidence export returns persisted state.
5. StorageIsNonDurable reflects actual runtime mode.
6. Existing Skill Lab works unchanged.
7. Tests prove persistence.
8. Evidence docs are updated.
Recommended commit title
feat(skill-opt): persist offline skill optimization runs