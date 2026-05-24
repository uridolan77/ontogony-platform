Yes. The right next meta-sprint is a **cross-repo contract discipline program**, not another feature page.

The goal is to stop relying on “it works locally” and make every API/UI relationship explicit, generated, classified, checked, and documented.

# CONTRACT-DISCIPLINE-001 — Comprehensive plan

## Current diagnosis

Kanon is now the best model: it has route-parity scripts and operator UI coverage checks exposed in frontend package scripts. The frontend package includes `kanon:route-parity`, `kanon:operator-ui-coverage:sync/check`, `route-client-drift:check`, and broader route/readiness checks. 

Allagma has broad UI/API coverage, but `allagmaClient.ts` still contains handwritten DTOs and comments such as “Not yet in committed OpenAPI snapshot,” especially around runtime posture.  The backend exposes a larger `/allagma/v0` surface, including runs, interaction events, audit, capabilities, model purposes, runtime posture, operations, evaluations, datasets, and baseline comparisons.  

Conexus is operationally aligned for model calls, route decisions, provider inventory, route preview, usage/quota, diagnostics, and chat completions, but it does not yet appear to have the same Kanon-style parity guard. The frontend has a Conexus client and OpenAPI snapshot, but there is no clear `conexus:route-parity` equivalent. The Conexus API maps the major endpoint families through backend endpoint groups. 

So the program should standardize **Kanon-level discipline across Allagma and Conexus**, then add one cross-system gate over all three.

---

# Target end state

After this work, every backend route should fall into one of these states:

```text
operator_ui_used
generated_client_used
backend_only
internal_test_only
dev_only
deprecated
planned_not_implemented
```

Every frontend page should declare:

```text
backend routes used
client functions used
generated schema types used
manual DTO adapters, if any
fixture/demo fallback rules
live/fixture/imported data source status
evidence sensitivity / redaction posture
```

Every repo should answer:

```text
Did the backend API change?
Was OpenAPI regenerated?
Was frontend generated schema updated?
Does the route-workflow catalog still match?
Does the UI use only declared routes?
Are manual DTO shims intentional and tracked?
```

---

# Workstream 1 — Establish the shared contract taxonomy

## Repos

```text
ontogony-platform
ontogony-frontend
allagma-dotnet
conexus-dotnet
kanon-dotnet
```

## Deliverables

Create a shared document:

```text
ontogony-platform/docs/contracts/CONTRACT_DISCIPLINE_STANDARD.md
```

Define the canonical classifications:

```text
Route exposure:
  public_operator
  admin_operator
  cross_service
  internal_backend
  dev_only
  deprecated
  planned

Frontend usage:
  ui_page
  nested_component
  evidence_resolver
  agent_interaction
  smoke_only
  unused

Data source posture:
  live
  live_with_fallback
  fixture_only
  generated_only
  imported
  unknown

Contract state:
  generated_schema
  handwritten_adapter
  transitional_shim
  stale_snapshot
  intentionally_backend_only
```

## Acceptance

A developer can classify any endpoint and know whether the UI is allowed to call it, whether it needs generated types, and whether it must appear in route-workflow inventory.

---

# Workstream 2 — Backend route inventories for all services

## Goal

Each backend repo emits a stable route inventory artifact, not just OpenAPI.

## Existing model

Kanon already has generated route inventory / route coverage discipline. Keep it as the pattern.

## Add or normalize

```text
kanon-dotnet:
  docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json
  already largely present

allagma-dotnet:
  docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json

conexus-dotnet:
  docs/generated/CONEXUS_ROUTE_INVENTORY.json
```

Each route inventory row should include:

```json
{
  "method": "GET",
  "path": "/allagma/v0/runs/{runId}",
  "operationId": "getRun",
  "service": "allagma",
  "apiFamily": "runs",
  "auth": "service_token",
  "routeClass": "public_operator",
  "stability": "alpha",
  "requestSchema": null,
  "responseSchema": "RunDetail",
  "errorSchemas": ["CrossServiceErrorEnvelope"],
  "sourceFile": "src/Allagma.Api/Program.cs"
}
```

## Acceptance

Each backend can run:

```powershell
dotnet test
```

or a script such as:

```powershell
./scripts/sync-route-inventory.ps1
./scripts/check-route-inventory.ps1
```

and produce/check the inventory.

---

# Workstream 3 — OpenAPI snapshot authority

## Goal

Make OpenAPI snapshots reproducible and current.

## Current problem

Allagma frontend snapshot exists, but frontend code still has handwritten runtime posture types because the committed OpenAPI snapshot is incomplete/stale. 

## Required commands

In frontend:

```json
{
  "openapi:sync:allagma": "node ./scripts/sync-allagma-openapi-from-backend.mjs",
  "openapi:sync:conexus": "node ./scripts/sync-conexus-openapi-from-backend.mjs",
  "openapi:sync:kanon": "node ./scripts/sync-kanon-openapi-from-backend.mjs",
  "openapi:check": "node ./scripts/openapi-check.mjs"
}
```

Current frontend already has `openapi:sync:allagma`, `openapi:gen`, `openapi:check`, and Kanon expansion scripts.  Add Conexus symmetry and make Allagma stricter.

## Artifacts

```text
ontogony-frontend/openapi/allagma.v0.json
ontogony-frontend/openapi/conexus.v0.json
ontogony-frontend/openapi/kanon.v0.json
ontogony-frontend/src/allagma/api/generated/schema.ts
ontogony-frontend/src/conexus/api/generated/schema.ts
ontogony-frontend/src/kanon/api/generated/schema.ts
```

## Acceptance

No backend route added to `allagma-dotnet`, `conexus-dotnet`, or `kanon-dotnet` can be used by the frontend unless it appears in the relevant OpenAPI snapshot or is explicitly marked as a temporary handwritten shim.

---

# Workstream 4 — Service-specific route parity checks

## Goal

Give Allagma and Conexus the same discipline Kanon now has.

## Existing Kanon scripts

Kanon already has scripts such as:

```text
kanon:route-parity
kanon:operator-ui-coverage:sync
kanon:operator-ui-coverage:check
route-client-drift:check
```

in `ontogony-frontend/package.json`. 

## Add scripts

```json
{
  "allagma:route-parity": "node ./scripts/check-allagma-route-parity.mjs",
  "allagma:operator-ui-coverage:sync": "node ./scripts/sync-allagma-operator-ui-coverage.mjs",
  "allagma:operator-ui-coverage:check": "node ./scripts/check-allagma-operator-ui-coverage.mjs",

  "conexus:route-parity": "node ./scripts/check-conexus-route-parity.mjs",
  "conexus:operator-ui-coverage:sync": "node ./scripts/sync-conexus-operator-ui-coverage.mjs",
  "conexus:operator-ui-coverage:check": "node ./scripts/check-conexus-operator-ui-coverage.mjs",

  "contracts:service-parity": "npm run kanon:route-parity && npm run allagma:route-parity && npm run conexus:route-parity"
}
```

Each parity script should compare:

```text
backend route inventory
backend OpenAPI snapshot
frontend openapi/*.json
frontend generated schema
frontend route-workflow-catalog.json
frontend client functions
```

## Acceptance

A route is flagged if:

```text
backend exposes route but OpenAPI misses it
frontend calls route but OpenAPI misses it
route-workflow catalog references route but client does not
client function exists but no page/workflow uses it
route exists in snapshot but backend removed it
```

---

# Workstream 5 — Client function inventory

## Goal

Generate a machine-readable list of what each frontend API client actually calls.

## Files to scan

```text
src/allagma/api/allagmaClient.ts
src/conexus/api/conexusClient.ts
src/kanon/api/kanonClient.ts
src/kanon/api/*Client.ts
```

## Artifact

```text
ontogony-frontend/docs/generated/API_CLIENT_ROUTE_USAGE.json
```

Example:

```json
{
  "service": "allagma",
  "client": "allagmaClient.getAllagmaRun",
  "method": "GET",
  "path": "/allagma/v0/runs/{runId}",
  "usesGeneratedSchema": true,
  "requestType": null,
  "responseType": "RunDetail",
  "manualDto": false
}
```

## Acceptance

All client route strings must be discoverable. No hidden route calls embedded in random components.

---

# Workstream 6 — Manual DTO shim register

## Goal

Make every handwritten DTO intentional, temporary, and tracked.

## Current issue

Allagma has handwritten runtime posture DTOs because the OpenAPI snapshot lags.  Kanon also has some manually typed source-binding/provenance shapes. Conexus normalizes some unknown/camel/snake response shapes.

## Add file

```text
ontogony-frontend/docs/generated/MANUAL_DTO_SHIMS.md
```

Each entry:

```text
Service: allagma
File: src/allagma/api/allagmaClient.ts
Type: AllagmaRuntimePostureDto
Reason: OpenAPI snapshot does not yet contain full posture contract
Owner: allagma
Target removal: after allagma.v0.json sync includes AllagmaRuntimePostureContract fully
Risk: medium
```

## Add check

```json
{
  "manual-dto-shims:check": "node ./scripts/check-manual-dto-shims.mjs"
}
```

The check fails if a file contains markers like:

```text
Not yet in committed OpenAPI snapshot
manual shim
OpenAPI components omit
unknown normalization
```

without a registered entry.

## Acceptance

No untracked manual DTOs.

---

# Workstream 7 — Route-workflow catalog normalization

## Goal

The route-workflow catalog becomes the canonical map from UI page to backend contract.

The catalog already includes rich Allagma pages, including run audit, run detail, evaluations, human gates, replay, Agent Interaction, and Evidence Spine.    

But it has at least one visible path drift issue: it references `GET /allagma/v0/runtime-posture`, while the actual backend route is `GET /allagma/v0/runtime/posture`. The backend route is `/runtime/posture`. 

## Required cleanup

Normalize route strings in `route-workflow-catalog.json`:

```text
/allagma/v0/runtime-posture
→ /allagma/v0/runtime/posture
```

Then add checks so this cannot recur.

## Acceptance

Every `backendRoutes[]` entry in route-workflow catalog must match a backend inventory route, after normalizing path params.

---

# Workstream 8 — Allagma parity slice

## Package

```text
ALLAGMA-UI-API-PARITY-001
```

## Scope

1. Refresh `openapi/allagma.v0.json` from `allagma-dotnet`.
2. Ensure `/allagma/v0/model-purposes` is represented.
3. Decide whether UI directly uses `/model-purposes` or officially uses `/runtime/posture`.
4. Remove handwritten runtime posture DTOs if schemas exist.
5. Add `allagma:route-parity`.
6. Add `allagma:operator-ui-coverage:sync/check`.
7. Fix route catalog drift for `/runtime/posture`.
8. Add coverage doc:

```text
docs/generated/ALLAGMA_UI_API_COVERAGE.md
```

## Acceptance

All Allagma frontend client functions map to backend inventory and OpenAPI. Any intentionally unused backend route is classified.

---

# Workstream 9 — Conexus parity slice

## Package

```text
CONEXUS-UI-API-PARITY-001
```

## Scope

1. Generate or refresh backend route inventory from `conexus-dotnet`.
2. Refresh `openapi/conexus.v0.json`.
3. Add `conexus:route-parity`.
4. Add `conexus:operator-ui-coverage:sync/check`.
5. Classify all Conexus routes:

```text
model gateway
admin routing
provider posture
model-call evidence
route decision evidence
diagnostics
governance/usage/quota
project/key management
chat completion
```

6. Reduce manual normalization in `conexusClient.ts` where stable schemas now exist.
7. Add explicit parity coverage for:

```text
/admin/v0/model-calls
/admin/v0/model-calls/{modelCallId}
/admin/v0/model-calls/{modelCallId}/evidence-links
/admin/v0/route-decisions/{routeDecisionId}
/admin/v0/providers/inventory
/v1/models/{alias}/route-preview
/v1/chat/completions
/v1/governance/quota
/v1/governance/usage
```

## Acceptance

Conexus reaches Kanon-level route parity discipline.

---

# Workstream 10 — Kanon parity hardening

## Package

```text
KANON-UI-API-PARITY-001A
```

Not a big sprint. Just hardening.

## Scope

1. Ensure all new Domain Switcher routes are included in coverage:

   * `/ontology/v0/domain-packs`
   * `/ontology/v0/domain-packs/active`
2. Ensure Source Bindings routes are not ambiguously marked `ServerOnly` if operator UI calls them.
3. Clarify route coverage taxonomy:

   * backend route
   * generated backend client
   * operator frontend route
   * internal-only
4. Keep Docker-live Domain Switcher smoke in the standard parity bundle.

The Docker-live Domain Switcher spec is already present and verifies Kanon preflight, domain switch, downstream sync, and unchanged Conexus alias. 

## Acceptance

Kanon remains the reference implementation.

---

# Workstream 11 — Cross-system contract gate

## Goal

One command validates all service/API/UI contract discipline.

## Add command

```json
{
  "contracts:discipline": "npm run openapi:check && npm run contracts:service-parity && npm run route-client-drift:check && npm run inventory:check && npm run readiness:check && npm run manual-dto-shims:check"
}
```

Then platform-level wrapper:

```powershell
ontogony-platform/scripts/check/check-contract-discipline.ps1
```

Runs:

```text
ontogony-frontend contract checks
backend route inventory checks
runtime lock validation
governed fake summary validation, optional
domain switcher smoke, optional
```

## Stage policy

```text
Stage 1:
  local advisory

Stage 2:
  manual CI workflow

Stage 3:
  runtime-lock pre-release check

Stage 4:
  required PR gate, later only
```

Do not jump to Stage 4 yet.

---

# Workstream 12 — Contract documentation index

## Add docs

```text
ontogony-platform/docs/contracts/CONTRACT_DISCIPLINE_STANDARD.md
ontogony-platform/docs/contracts/API_CONTRACT_SOURCE_OF_TRUTH.md
ontogony-platform/docs/contracts/UI_API_COVERAGE_MATRIX.md
ontogony-platform/docs/contracts/MANUAL_DTO_SHIM_POLICY.md
```

## Generated docs

```text
ontogony-frontend/docs/generated/ALLAGMA_UI_API_COVERAGE.md
ontogony-frontend/docs/generated/CONEXUS_UI_API_COVERAGE.md
ontogony-frontend/docs/generated/KANON_UI_API_COVERAGE.md
ontogony-frontend/docs/generated/API_CLIENT_ROUTE_USAGE.json
ontogony-frontend/docs/generated/MANUAL_DTO_SHIMS.md
```

---

# Implementation order

## Phase A — Foundation

```text
1. Contract discipline standard
2. Manual DTO shim register
3. Client route usage extractor
4. Route-workflow catalog exact-match checker
```

## Phase B — Allagma

```text
5. ALLAGMA-UI-API-PARITY-001
```

Do Allagma first because it has the most visible drift: broad UI coverage, but handwritten posture DTOs and likely OpenAPI lag.

## Phase C — Conexus

```text
6. CONEXUS-UI-API-PARITY-001
```

Do Conexus second because it is operationally aligned but lacks formal parity discipline.

## Phase D — Kanon hardening

```text
7. KANON-UI-API-PARITY-001A
```

Small hardening only.

## Phase E — Cross-system gate

```text
8. CONTRACT-DISCIPLINE-GATE-001
```

Wrap all checks under one local/manual CI gate.

---

# Concrete acceptance checklist

The whole program is done when:

```text
Backend inventories:
  allagma-dotnet emits ALLAGMA_V0_ROUTE_INVENTORY.json
  conexus-dotnet emits CONEXUS_ROUTE_INVENTORY.json
  kanon-dotnet inventory remains current

OpenAPI:
  allagma.v0.json current
  conexus.v0.json current
  kanon.v0.json current

Generated clients:
  all generated schema files updated
  no untracked handwritten DTOs

Frontend:
  all API client route calls are inventoried
  all route-workflow backendRoutes match backend inventory
  all UI pages declare live/fixture/generated/imported data source posture

Checks:
  kanon:route-parity passes
  allagma:route-parity passes
  conexus:route-parity passes
  route-client-drift:check passes
  manual-dto-shims:check passes
  contracts:discipline passes

Docs:
  service coverage docs generated
  manual DTO shim register generated
  source-of-truth doc written
```

---

# Recommended Cursor package name

Use one umbrella package with explicit slices:

```text
CONTRACT-DISCIPLINE-001
```

Slices:

```text
CONTRACT-DISCIPLINE-001A — standard + taxonomy
CONTRACT-DISCIPLINE-001B — client route usage + manual DTO shim register
CONTRACT-DISCIPLINE-001C — Allagma route parity
CONTRACT-DISCIPLINE-001D — Conexus route parity
CONTRACT-DISCIPLINE-001E — Kanon hardening
CONTRACT-DISCIPLINE-001F — cross-system gate + docs
```

Bottom line: **the product is now functionally strong; contract discipline is the next maturity layer.** The goal is to make Allagma and Conexus as contract-tight as Kanon, then enforce the whole system through one coherent cross-repo gate.
