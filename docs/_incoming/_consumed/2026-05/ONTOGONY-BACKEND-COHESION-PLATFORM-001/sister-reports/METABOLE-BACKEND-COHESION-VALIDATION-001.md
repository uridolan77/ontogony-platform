Metabole transformation / provenance cohesion validation

Goal

Prove that metabole-dotnet fully owns and validates the Ontogony data transformation plane against the platform backend cohesion manifest.

Metabole must prove:

schema extraction and profiling
SLOD mapping candidate generation
gaming-domain profile inference
lineage and pipeline evidence
operator-review payloads
pipeline run lifecycle and durability
Kanon semantic validation boundary (request only)
Conexus advisory boundary (request only)
trace/correlation/idempotency
error envelope behavior
route inventory discipline
forbidden peer/SDK dependency boundaries

Metabole is the transformation and mapping-candidate plane, not semantic authority, not model gateway, not workflow orchestrator.

Deliverables

docs/evidence/METABOLE_BACKEND_COHESION_VALIDATION_001.md
docs/reviews/METABOLE_BACKEND_COHESION_SCORECARD.md
docs/reviews/METABOLE_BACKEND_COHESION_DEFERRALS.md
tests/Metabole.Tests/BackendCohesion/

Slices

001A — Current-state audit

Deliver:

docs/reviews/METABOLE_BACKEND_COHESION_CURRENT_STATE_AUDIT.md
docs/evidence/METABOLE_BACKEND_COHESION_001A_CURRENT_STATE.md

Audit:

route inventory
OpenAPI snapshot state
pipeline run lifecycle routes
schema-profile and SLOD profiling paths
gaming-domain profile and slod-candidates routes
lineage evidence and replay bundle builder state
Kanon adapter boundaries
Conexus adapter boundaries
Allagma orchestration boundary
idempotency model
Postgres durability
forbidden dependency tests
known deferrals

001B — Platform manifest alignment

Deliver:

docs/system/METABOLE_PLATFORM_COHESION_ALIGNMENT.md
tests/Metabole.Tests/BackendCohesion/MetabolePlatformCohesionAlignmentTests.cs

Tests:

Metabole declares ownership of transformation provenance scenario (golden G)
Metabole participates in error/trace/correlation scenario (golden F)
Metabole does not claim Kanon semantic authority
Metabole does not claim Conexus model routing
Metabole does not claim Allagma workflow orchestration
Metabole does not mark mappings accepted or canonical

001C — Schema profile golden path

Golden path:

POST schema-profile pipeline run
→ extract schema
→ profile schema
→ mapping candidates (heuristic)
→ Conexus advisory (optional)
→ Kanon validation request (optional)
→ lineage evidence
→ durable run/events

Deliver:

tests/Metabole.Tests/BackendCohesion/SchemaProfileGoldenPathTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001C_SCHEMA_PROFILE.md

Tests:

pipeline run reaches completed status
events append in order
evidence retrievable
idempotency replay works
trace/correlation on run and evidence
missing run returns stable error envelope

001D — SLOD profiling golden path

Golden path:

schema-profile with slodProfileMode
→ gaming domain profile
→ SLOD mapping candidates with evidence
→ operator slod-candidates output
→ evidence slodSummary

Deliver:

tests/Metabole.Tests/BackendCohesion/SlodProfilingGoldenPathTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001D_SLOD_PROFILING.md

Tests:

gaming-domain-profile route returns profile
slod-candidates route returns grouped candidates
candidate confidence/evidence preserved
low-confidence candidates not dropped
deterministic generation on fixtures
sensitive field classification surfaced

001E — Kanon validation boundary

Deliver:

tests/Metabole.Tests/BackendCohesion/KanonValidationBoundaryTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001E_KANON_BOUNDARY.md

Tests:

Metabole sends validation request with actor/trace/correlation
payload excludes credentials/connection strings
Metabole does not treat Kanon response as canonical truth without operator review
Metabole does not create Kanon decision records
fake adapter default; HTTP stub smoke optional

001F — Conexus advisory boundary

Deliver:

tests/Metabole.Tests/BackendCohesion/ConexusAdvisoryBoundaryTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001F_CONEXUS_BOUNDARY.md

Tests:

Metabole sends advisory request with source fingerprint context
Conexus output remains advisory only
Metabole does not route models or record model-call evidence
Metabole does not fabricate Conexus cost/token telemetry
fake adapter default; HTTP stub smoke optional

001G — Lineage evidence and replay bundle cohesion

Deliver:

tests/Metabole.Tests/BackendCohesion/LineageReplayCohesionTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001G_LINEAGE_REPLAY.md

Tests:

lineage evidence contract shape stable
replay bundle builder produces metabole-replay-bundle-v0
replay bundle includes run/events/evidence refs
replay HTTP route remains deferred (documented)
Postgres roundtrip preserves extended payloads when enabled

001H — Error/trace/idempotency discipline

Deliver:

tests/Metabole.Tests/BackendCohesion/MetaboleErrorTraceIdempotencyTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001H_ERROR_TRACE_IDEMPOTENCY.md

Tests:

validation/conflict/downstream failures use Metabole error envelope
traceId/correlationId on errors and outbound adapters
idempotency prevents duplicate pipeline runs where expected
in-progress idempotency conflict stable
retryable flag correct

001I — Route inventory and client coverage discipline

Deliver:

docs/generated/METABOLE_ROUTE_INVENTORY.json
tests/Metabole.Tests/BackendCohesion/MetaboleRouteInventoryCohesionTests.cs
docs/evidence/METABOLE_BACKEND_COHESION_001I_ROUTE_INVENTORY.md

Tests:

route catalog matches live Program.cs surface
route inventory doc matches catalog
OpenAPI snapshot gap classified as deferral or closed
client coverage classification exists
future routes documented separately from live surface

001J — Closeout

Deliver:

docs/evidence/METABOLE_BACKEND_COHESION_VALIDATION_001_CLOSEOUT.md
docs/reviews/METABOLE_BACKEND_COHESION_SCORECARD.md

Acceptance

1. Schema profile and SLOD golden paths are proven.
2. Lineage evidence is reconstructable from run APIs.
3. Kanon/Conexus boundaries are proven (advisory/validation request only).
4. Metabole does not claim semantic authority or model routing.
5. Route inventory truth is synchronized or deferrals explicit.
6. Known optional-test skips (Postgres, SQL Server live) are classified.
7. Forbidden dependency boundaries hold.

Cursor prompt

Implement METABOLE-BACKEND-COHESION-VALIDATION-001 in metabole-dotnet.

Goal:
Prove Metabole's transformation plane, SLOD profiling, lineage evidence, Kanon/Conexus boundaries, and route/error/trace discipline against the platform backend cohesion manifest.

Metabole owns:
- schema extraction and profiling
- SLOD mapping candidate generation
- gaming-domain profile inference
- pipeline run lifecycle and evidence
- operator-review payloads
- lineage and replay bundle export shape

Metabole does not own:
- semantic truth / ontology authority
- model/provider routing
- workflow orchestration
- skill-edit or mapping acceptance
- production deployment

Slices:
001A current-state audit
001B platform manifest alignment
001C schema profile golden path
001D SLOD profiling golden path
001E Kanon validation boundary
001F Conexus advisory boundary
001G lineage/replay bundle cohesion
001H error/trace/idempotency discipline
001I route inventory/client coverage
001J closeout

Hard requirements:
- Kanon/Conexus output is advisory until operator/Kanon governance says otherwise
- no provider secrets in evidence or adapter payloads
- no Ontogony.Platform forbidden peer references
- evidence docs for each slice
- route inventory truth aligned or deferrals listed
