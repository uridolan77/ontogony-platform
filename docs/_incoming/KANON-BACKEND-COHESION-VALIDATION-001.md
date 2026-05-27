Kanon semantic authority / governance cohesion validation
Goal

Prove that kanon-dotnet fully owns and validates the Ontogony semantic authority layer across backend cohesion.

Kanon must prove:

decision reconstructability
semantic governance
skill-edit evaluation
skill promotion governance
provenance
domain packs / ontology versions
missing evidence diagnostics
route/client/OpenAPI discipline
cross-service trace/error contract

Kanon is the semantic authority, not orchestrator, not model gateway, not transformation engine.

Deliverables
docs/evidence/KANON_BACKEND_COHESION_VALIDATION_001.md
docs/reviews/KANON_BACKEND_COHESION_SCORECARD.md
docs/reviews/KANON_BACKEND_COHESION_DEFERRALS.md
tests/Kanon.Tests/BackendCohesion/
Slices
001A — Current-state audit

Deliver:

docs/reviews/KANON_BACKEND_COHESION_CURRENT_STATE_AUDIT.md
docs/evidence/KANON_BACKEND_COHESION_001A_CURRENT_STATE.md

Audit:

route inventory
OpenAPI snapshot
client coverage
decision record types
decision reconstructability
skill-edit governance
skill-release promotion governance
provenance
domain pack / ontology version handling
trace/actor/role headers
known deferrals
001B — Platform manifest alignment

Deliver:

docs/system/KANON_PLATFORM_COHESION_ALIGNMENT.md
tests/Kanon.Tests/BackendCohesion/KanonPlatformCohesionAlignmentTests.cs

Tests:

Kanon owns semantic authority scenario
Kanon owns decision reconstructability scenario
Kanon participates in skill optimization
Kanon participates in skill release governance
Kanon does not own Allagma workflow execution
Kanon does not own Conexus model routing
Kanon does not own Metabole transformation execution
001C — Decision reconstructability golden path

Golden path:

decision record
→ decision event projection
→ reconstructability classifier
→ report grade
→ diagnostics for missing evidence

Deliver:

tests/Kanon.Tests/BackendCohesion/DecisionReconstructabilityGoldenPathTests.cs
docs/evidence/KANON_BACKEND_COHESION_001C_DECISION_RECONSTRUCTABILITY.md

Tests:

full evidence decision = reconstructable/pass
partial evidence decision = warn
missing evidence decision = fail
diagnostics contain owner-service hints
all decision record types project to decision event
no fake labels on unavailable evidence
001D — Skill-edit governance cohesion

Golden path:

skill edit proposal
→ Kanon evaluate
→ accepted / rejected / deferred
→ decision record
→ reconstructability event

Deliver:

tests/Kanon.Tests/BackendCohesion/SkillEditGovernanceCohesionTests.cs
docs/evidence/KANON_BACKEND_COHESION_001D_SKILL_EDIT_GOVERNANCE.md

Tests:

valid bounded edit accepted
unsafe/unbounded edit rejected
missing evidence deferred
decision record persisted/projected
safe rationale has no hidden reasoning
Allagma-facing contract stable
001E — Skill-release promotion governance cohesion

Golden path:

promotion evaluation request
→ approved_for_sandbox / rejected / deferred
→ decision record
→ no production deployment allowed

Deliver:

tests/Kanon.Tests/BackendCohesion/SkillReleasePromotionGovernanceCohesionTests.cs
docs/evidence/KANON_BACKEND_COHESION_001E_SKILL_RELEASE_PROMOTION.md

Tests:

sandbox accepted candidate approved
production/live/default-runtime rejected
missing optimization evidence deferred
rejected source edit rejected
automatic promotion rejected
live deployment allowed flag rejected
targetEnvironment normalized
001F — Provenance / domain pack / ontology cohesion

Deliver:

tests/Kanon.Tests/BackendCohesion/KanonProvenanceOntologyCohesionTests.cs
docs/evidence/KANON_BACKEND_COHESION_001F_PROVENANCE_ONTOLOGY.md

Tests:

active ontology version resolved
domain pack id/version stable
provenance reads require correct roles
operator actor headers are respected
forbidden role produces correct error envelope
001G — Trace/error/header discipline

Deliver:

tests/Kanon.Tests/BackendCohesion/KanonErrorTraceHeaderCohesionTests.cs
docs/evidence/KANON_BACKEND_COHESION_001G_ERROR_TRACE_HEADERS.md

Tests:

X-Ontogony-Actor-* accepted
X-Ontogony-Roles accepted
traceId/correlationId propagated into error envelope
403 authorization not confused with downtime
404 unavailable fake authority does not fabricate labels
001H — Route/OpenAPI/client coverage discipline

Deliver:

docs/evidence/KANON_BACKEND_COHESION_001H_ROUTE_OPENAPI_CLIENT.md
tests/Kanon.Tests/BackendCohesion/KanonRouteOpenApiClientCoverageTests.cs

Tests:

route inventory matches live route catalog
OpenAPI snapshot matches live route surface
Client routes classified correctly
ServerOnly routes justified
104/86/18 or current count is stable and documented
001I — Closeout

Deliver:

docs/evidence/KANON_BACKEND_COHESION_VALIDATION_001_CLOSEOUT.md
docs/reviews/KANON_BACKEND_COHESION_SCORECARD.md
Acceptance
1. Decision reconstructability is fully proven.
2. Skill-edit governance is proven.
3. Skill-release promotion governance is proven.
4. Kanon remains semantic authority only.
5. No execution/orchestration/model routing ownership leaks into Kanon.
6. Route/OpenAPI/client coverage is synchronized.
7. Missing/unavailable evidence is explicit and not fabricated.
Cursor prompt
Implement KANON-BACKEND-COHESION-VALIDATION-001 in kanon-dotnet.

Goal:
Prove Kanon’s semantic authority, decision reconstructability, skill-edit governance, skill-release promotion governance, provenance, route/client/OpenAPI discipline, and error/trace/header behavior against the platform backend cohesion manifest.

Kanon owns:
- semantic authority
- ontology/domain pack/version governance
- decision records
- decision reconstructability
- missing evidence diagnostics
- skill-edit evaluation
- skill-release promotion evaluation
- provenance reads

Kanon does not own:
- Allagma workflow execution
- Conexus model/provider routing
- Metabole transformation execution
- production deployment

Slices:
001A current-state audit
001B platform manifest alignment
001C decision reconstructability golden path
001D skill-edit governance cohesion
001E skill-release promotion governance cohesion
001F provenance/ontology cohesion
001G trace/error/header discipline
001H route/OpenAPI/client coverage
001I closeout

Hard requirements:
- every decision record type projects to decision event
- no fake labels on unavailable evidence
- production/live/default-runtime promotion rejected
- route/OpenAPI/client truth synchronized
- evidence docs for each slice