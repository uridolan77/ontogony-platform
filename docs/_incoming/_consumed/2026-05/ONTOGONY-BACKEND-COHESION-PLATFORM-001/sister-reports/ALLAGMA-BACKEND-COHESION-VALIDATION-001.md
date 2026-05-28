Allagma orchestration/runtime cohesion validation
Goal

Prove that allagma-dotnet implements the runtime orchestration parts of the platform backend cohesion manifest.

Allagma must prove:

workflow lifecycle
budget/cost governance
Conexus model-call execution boundary
Kanon governance calls
skill optimization runs
skill release governance
sandbox binding resolution
rollback
evidence export
trace/correlation/idempotency
error envelope behavior

Allagma is the governed orchestration plane, not semantic authority and not model gateway.

Deliverables
docs/evidence/ALLAGMA_BACKEND_COHESION_VALIDATION_001.md
docs/reviews/ALLAGMA_BACKEND_COHESION_SCORECARD.md
docs/reviews/ALLAGMA_BACKEND_COHESION_DEFERRALS.md
docs/system/ALLAGMA_BACKEND_COHESION_SCENARIOS.md
tests/Allagma.Tests/BackendCohesion/
Slices
001A — Current-state audit

Deliver:

docs/reviews/ALLAGMA_BACKEND_COHESION_CURRENT_STATE_AUDIT.md
docs/evidence/ALLAGMA_BACKEND_COHESION_001A_CURRENT_STATE.md

Audit:

route inventory
OpenAPI snapshot state
workflow routes
budget routes
skill optimization routes
skill release routes
sandbox resolver route
evidence export routes
Kanon client boundaries
Conexus client boundaries
Metabole boundary if present
idempotency model
Postgres durability
known failing tests
deferrals
001B — Platform manifest alignment

Allagma should read or mirror the platform backend cohesion manifest and assert which scenarios it owns.

Deliver:

docs/system/ALLAGMA_PLATFORM_COHESION_ALIGNMENT.md
tests/Allagma.Tests/BackendCohesion/AllagmaPlatformCohesionAlignmentTests.cs

Tests:

Allagma declares ownership of workflow lifecycle scenario
Allagma declares ownership of budget/cost scenario
Allagma declares ownership of skill optimization scenario
Allagma declares ownership of skill release governance scenario
Allagma declares ownership of sandbox binding resolver scenario
Allagma does not claim Kanon semantic authority
Allagma does not claim Conexus model routing
Allagma does not claim Metabole transformation ownership
001C — Workflow + budget + Conexus golden path

Golden path:

create/run workflow
→ budget policy applied
→ Conexus model call route evidence collected
→ budget usage recorded
→ run evidence export links model call and cost/usage

Deliver:

tests/Allagma.Tests/BackendCohesion/WorkflowBudgetConexusGoldenPathTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001C_WORKFLOW_BUDGET_CONEXUS.md

Tests:

budget policy blocks before Conexus call when exceeded
budget usage recorded after Conexus call
missing telemetry remains unknown, not fabricated
Conexus route evidence ref appears in run evidence
trace/correlation propagated
001D — Skill optimization golden path

Golden path:

skill optimization run
→ rollout Conexus evidence refs
→ fake or real_dry_run proposal mode
→ Kanon skill-edit evaluation
→ candidate persisted
→ evidence export

Deliver:

tests/Allagma.Tests/BackendCohesion/SkillOptimizationBackendCohesionTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001D_SKILL_OPTIMIZATION.md

Tests:

fake mode remains deterministic
real_dry_run requires budget policy
optimizer evidence refs stored
invalid optimizer output creates failure artifact
all proposals go through Kanon
live deployment remains false
Postgres reload preserves optimizer columns
001E — Skill release governance golden path

Golden path:

accepted candidate
→ promotion request
→ Kanon promotion evaluation
→ approved_for_sandbox
→ sandbox binding
→ activate
→ pause
→ rollback
→ evidence refs

Deliver:

tests/Allagma.Tests/BackendCohesion/SkillReleaseGovernanceBackendCohesionTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001E_SKILL_RELEASE.md

Tests:

promotion requires accepted source candidate
Kanon approval maps to approved_for_sandbox
binding requires approved promotion
activation requires rollback target unless waived
pause changes status
rollback changes binding to rolled_back
evidence links optimization run + promotion + binding + rollback
production/live/default-runtime rejected
001F — Sandbox consumer binding resolver cohesion

Golden path:

active sandbox binding
→ resolve returns skill version
paused binding
→ resolve=false binding_paused
rolled_back binding
→ resolve=false binding_rolled_back
multiple active bindings
→ 409 conflict

Deliver:

tests/Allagma.Tests/BackendCohesion/SandboxConsumerBindingResolverCohesionTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001F_SANDBOX_RESOLVER.md

Tests:

active binding resolves
paused does not resolve
rolled_back does not resolve
no binding returns base fallback
multiple active returns 409
limitations deny production/live
evidence refs include binding/promotion/optimization run
001G — Error/trace/idempotency discipline

Deliver:

tests/Allagma.Tests/BackendCohesion/AllagmaErrorTraceIdempotencyCohesionTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001G_ERROR_TRACE_IDEMPOTENCY.md

Tests:

CrossServiceErrorEnvelope for validation/conflict/downstream failures
traceId/correlationId propagated into outbound Kanon calls
traceId/correlationId propagated into outbound Conexus calls
idempotency prevents duplicate workflow/run/action where expected
duplicate idempotency conflict behavior is stable
001H — OpenAPI/route inventory sync

This should close the known deferral:

ALLAGMA-SKILL-RELEASE-OPENAPI-SYNC-001

Deliver:

docs/api/allagma-openapi-v1.snapshot.json
docs/generated/ALLAGMA_OPENAPI_PROVENANCE.json
tests/Allagma.Tests/OpenApiSnapshotTests.cs
docs/evidence/ALLAGMA_BACKEND_COHESION_001H_OPENAPI_SYNC.md

Acceptance:

skill-release routes are in OpenAPI snapshot
skill-optimization routes are in OpenAPI snapshot
sandbox resolver route is in OpenAPI snapshot
route inventory and OpenAPI no longer disagree for new spines
001I — Closeout

Deliver:

docs/evidence/ALLAGMA_BACKEND_COHESION_VALIDATION_001_CLOSEOUT.md
docs/reviews/ALLAGMA_BACKEND_COHESION_SCORECARD.md
Acceptance
1. Allagma-owned platform scenarios have executable tests.
2. Skill optimization/release/consumer activation paths are proven.
3. Budget/Conexus/Kanon boundaries are proven.
4. OpenAPI/route inventory drift is resolved or explicitly deferred.
5. Known full-suite failures are classified.
6. Allagma does not claim semantic authority or model-routing ownership.
Suggested commit sequence
docs(allagma): audit backend cohesion state
test(allagma): align with platform backend cohesion manifest
test(allagma): prove workflow budget Conexus golden path
test(allagma): prove skill optimization backend cohesion
test(allagma): prove skill release governance cohesion
test(allagma): prove sandbox binding resolver cohesion
test(allagma): prove error trace idempotency discipline
test(allagma): sync skill release routes into OpenAPI snapshot
docs(allagma): close backend cohesion validation
Cursor prompt
Implement ALLAGMA-BACKEND-COHESION-VALIDATION-001 in allagma-dotnet.

Goal:
Prove Allagma’s runtime orchestration paths against the platform backend cohesion manifest.

Allagma owns:
- governed workflow orchestration
- run lifecycle
- budget/cost governance
- skill optimization orchestration
- skill release governance lifecycle
- sandbox binding resolver
- evidence exports
- idempotency / trace / error discipline

Allagma does not own:
- semantic truth
- ontology governance
- model/provider routing
- transformation/evolution provenance
- production deployment

Slices:
001A current-state audit
001B platform manifest alignment
001C workflow + budget + Conexus golden path
001D skill optimization golden path
001E skill release governance golden path
001F sandbox consumer binding resolver cohesion
001G error/trace/idempotency discipline
001H OpenAPI/route inventory sync
001I closeout

Hard requirements:
- tests for each owned golden scenario
- evidence docs for each slice
- route inventory/OpenAPI truth aligned
- all known deferrals listed
- no production deployment
- no semantic authority claimed by Allagma