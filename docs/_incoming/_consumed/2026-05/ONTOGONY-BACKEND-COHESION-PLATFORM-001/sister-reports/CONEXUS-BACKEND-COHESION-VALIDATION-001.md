Conexus model gateway / routing / model-call evidence cohesion validation
Goal

Prove that conexus-dotnet fully owns and validates the Ontogony model gateway layer.

Conexus must prove:

model routing
provider/fake abstraction
route decision evidence
model-call evidence
cost/latency/token telemetry
alias/config discipline
optimizer dry-run evidence support
trace/correlation/idempotency
error envelope behavior
no semantic authority leakage

Conexus is the model gateway, not semantic authority, not workflow orchestrator, not transformation engine.

Deliverables
docs/evidence/CONEXUS_BACKEND_COHESION_VALIDATION_001.md
docs/reviews/CONEXUS_BACKEND_COHESION_SCORECARD.md
docs/reviews/CONEXUS_BACKEND_COHESION_DEFERRALS.md
tests/Conexus.Tests/BackendCohesion/
Slices
001A — Current-state audit

Deliver:

docs/reviews/CONEXUS_BACKEND_COHESION_CURRENT_STATE_AUDIT.md
docs/evidence/CONEXUS_BACKEND_COHESION_001A_CURRENT_STATE.md

Audit:

route inventory
OpenAPI snapshot
provider registry
fake provider
OpenAI/real provider if configured
model aliases
route decisions
model-call evidence
cost/latency/token telemetry
idempotency
trace/correlation
Allagma client dependency
Metabole advisory boundary
Kanon non-authority boundary
known deferrals
001B — Platform manifest alignment

Deliver:

docs/system/CONEXUS_PLATFORM_COHESION_ALIGNMENT.md
tests/Conexus.Tests/BackendCohesion/ConexusPlatformCohesionAlignmentTests.cs

Tests:

Conexus owns routing/model-call evidence scenario
Conexus participates in workflow-budget scenario
Conexus participates in skill optimization optimizer evidence
Conexus participates in Metabole advisory generation
Conexus does not own semantic validation
Conexus does not own workflow orchestration
Conexus does not own artifact evolution truth
001C — Model alias and routing cohesion

Deliver:

tests/Conexus.Tests/BackendCohesion/ModelAliasRoutingCohesionTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001C_MODEL_ALIAS_ROUTING.md

Tests:

configured alias resolves to provider/model
unknown alias returns stable error envelope
fake provider alias works in local/dev
routing decision recorded
route decision includes provider/model/mode
no provider secret appears in response/evidence
001D — Model-call evidence cohesion

Golden path:

model completion request
→ route decision
→ provider/fake call
→ model-call evidence
→ cost/latency/token fields
→ retrievable route evidence

Deliver:

tests/Conexus.Tests/BackendCohesion/ModelCallEvidenceCohesionTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001D_MODEL_CALL_EVIDENCE.md

Tests:

model call id generated
route decision id generated
provider key recorded safely
latency recorded
tokens/cost recorded when known
tokens/cost unknown remains unknown
retrieval endpoint returns evidence
trace/correlation preserved
001E — Allagma integration evidence path

Prove Allagma can consume Conexus evidence.

Deliver:

tests/Conexus.Tests/BackendCohesion/AllagmaEvidenceConsumerCompatibilityTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001E_ALLAGMA_COMPATIBILITY.md

Tests:

Conexus route evidence DTO matches Allagma client expectation
modelCallId lookup returns stable shape
missing modelCallId returns not found envelope
cost unknown is nullable/unknown, not zero-fabricated
latency unknown is nullable/unknown
001F — Optimizer dry-run evidence support

This supports Skill Optimization 001J.

Deliver:

tests/Conexus.Tests/BackendCohesion/OptimizerDryRunEvidenceCohesionTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001F_OPTIMIZER_DRY_RUN.md

Tests:

optimizer model purpose can route through configured alias
dry-run optimizer call records modelCallId
structured response metadata can be referenced by Allagma
Conexus does not evaluate skill edit safety
Conexus does not mark candidate accepted
001G — Metabole advisory boundary

This supports Metabole later.

Deliver:

tests/Conexus.Tests/BackendCohesion/MetaboleAdvisoryBoundaryTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001G_METABOLE_ADVISORY.md

Tests:

Metabole advisory generation can use model-call route evidence
Conexus returns advisory output only
Conexus does not validate semantic mapping
Conexus does not create artifact evolution records

If Metabole integration does not exist yet, classify this slice as a documented deferral with contract tests.

001H — Error/trace/idempotency discipline

Deliver:

tests/Conexus.Tests/BackendCohesion/ConexusErrorTraceIdempotencyTests.cs
docs/evidence/CONEXUS_BACKEND_COHESION_001H_ERROR_TRACE_IDEMPOTENCY.md

Tests:

provider failure maps to CrossServiceErrorEnvelope
trace/correlation included in evidence
idempotency prevents duplicate model-call records if supported
timeout/cancellation handled consistently
retryable flag correct
001I — Route/OpenAPI/client coverage discipline

Deliver:

docs/evidence/CONEXUS_BACKEND_COHESION_001I_ROUTE_OPENAPI_CLIENT.md
tests/Conexus.Tests/BackendCohesion/ConexusRouteOpenApiCoverageTests.cs

Tests:

route inventory matches live routes
OpenAPI snapshot matches current surface
client coverage classification exists
server-only routes justified
provider secrets absent from /api/config-style responses
001J — Closeout

Deliver:

docs/evidence/CONEXUS_BACKEND_COHESION_VALIDATION_001_CLOSEOUT.md
docs/reviews/CONEXUS_BACKEND_COHESION_SCORECARD.md
Acceptance
1. Model routing is proven.
2. Model-call evidence is reconstructable.
3. Cost/latency/token telemetry is honest.
4. Allagma can consume Conexus route evidence.
5. Optimizer dry-run evidence path is proven.
6. Metabole advisory boundary is documented or tested.
7. Conexus does not claim semantic authority.
8. Route/OpenAPI/client truth is synchronized.
Cursor prompt
Implement CONEXUS-BACKEND-COHESION-VALIDATION-001 in conexus-dotnet.

Goal:
Prove Conexus’s model gateway, routing, provider abstraction, model-call evidence, cost/latency telemetry, optimizer dry-run support, Allagma evidence compatibility, Metabole advisory boundary, and route/OpenAPI discipline against the platform backend cohesion manifest.

Conexus owns:
- model/provider routing
- model aliases
- provider/fake execution
- route decisions
- model-call evidence
- cost/latency/token telemetry
- model-call evidence retrieval

Conexus does not own:
- semantic authority
- skill-edit acceptance
- workflow orchestration
- artifact evolution truth
- production deployment

Slices:
001A current-state audit
001B platform manifest alignment
001C model alias/routing cohesion
001D model-call evidence cohesion
001E Allagma evidence consumer compatibility
001F optimizer dry-run evidence support
001G Metabole advisory boundary
001H error/trace/idempotency discipline
001I route/OpenAPI/client coverage
001J closeout

Hard requirements:
- cost unknown remains unknown, never fabricated
- provider secrets never appear in evidence/config responses
- optimizer output is advisory only
- Conexus never marks skill edits/mappings accepted
- evidence docs for each slice