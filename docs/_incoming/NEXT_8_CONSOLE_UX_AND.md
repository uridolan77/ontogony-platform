Below is a comprehensive roadmap for the next seven packages, ordered so that the console finishes its canonical UX migration before moving back into deeper system functionality.

Current baseline: Batch 2 overview/list pages and Batch 3 workbenches are marked migrated, with Batch 3 explicitly complete in the migration plan.  The remaining routed surfaces are mostly Allagma detail pages, Kanon semantic pages, and Conexus Chat, visible in the app router but not yet fully classified in the adoption matrix.

---

# Roadmap overview

```text
1. CONSOLE-UX-BATCH-4-ALLAGMA-DETAILS-001
   Finish Allagma detail / secondary workbench pages.

2. CONSOLE-UX-BATCH-5-KANON-SEMANTIC-PAGES-001
   Finish Kanon semantic authority pages.

3. CONSOLE-UX-BATCH-6-CONEXUS-CHAT-001
   Clean the remaining Conexus Chat surface.

4. CONSOLE-UX-FINAL-ADOPTION-GATE-001
   Enforce route-level adoption classification for every operator route.

5. REPLAY-RUNTIME-002
   Wire replay shell into frontend clients and UI without building full orchestration yet.

6. RUNTIME-CONFIG-001
   Runtime-configurable frontend and cleaner local/docker/custom stack profiles.

7. SYSTEM-LEARNING-GUIDE-001
   Canonical learning documentation and stale-doc consolidation.
```

The important sequencing principle is:

```text
Finish the console surface first.
Then enforce that no route escapes classification.
Then continue replay/runtime/docs work on top of a stable console.
```

---

# 1. CONSOLE-UX-BATCH-4-ALLAGMA-DETAILS-001

## Goal

Finish canonical UX migration for the remaining Allagma detail and secondary pages that were not part of Batch 2 or Batch 3.

Routes to cover:

```text
/allagma/runs/start
/allagma/audit
/allagma/replay
/allagma/evaluations/datasets
/allagma/evaluations/:evaluationRunId
/allagma/evaluations/baseline-comparisons
/allagma/evaluations/baseline-comparisons/:comparisonId
```

These routes exist in the router but are not yet part of the completed adoption matrix.

## Main UX problem

Allagma detail pages are likely still uneven: some are workbench-like, some are dashboard-like, some are evaluation-specific. The risk is that they each invent their own header, warnings, evidence links, and export patterns.

The goal is not to redesign Allagma. It is to normalize the presentation layer:

```text
OperatorPageFrame
OperatorSignalSummary
OperatorDisclosure
OperatorDataTable where applicable
canonical action slots
canonical evidence/export reachability
```

## Target pages and intended shape

| Route                                                     | Desired default surface                               | Collapse into disclosures                                                 |
| --------------------------------------------------------- | ----------------------------------------------------- | ------------------------------------------------------------------------- |
| `/allagma/runs/start`                                     | Start-run form, selected preset, primary start action | Runtime posture, domain/model-purpose explanation, advanced payload/debug |
| `/allagma/audit`                                          | Audit lookup / entry point                            | Legacy explanation, route relationship to Run Detail/Audit Journey        |
| `/allagma/replay`                                         | Replay request/result shell                           | Raw replay bundle, delta, safety policy details                           |
| `/allagma/evaluations/datasets`                           | Dataset list/table + primary dataset actions          | Dataset metadata caveats, fixture/source limitations                      |
| `/allagma/evaluations/:evaluationRunId`                   | Evaluation summary, row results, evidence links       | Raw metadata, baseline/debug/export                                       |
| `/allagma/evaluations/baseline-comparisons`               | Comparison workbench/list                             | Baseline caveats, advanced filters, source posture                        |
| `/allagma/evaluations/baseline-comparisons/:comparisonId` | Comparison detail summary + deltas                    | Raw comparison payload, export/debug                                      |

## Implementation phases

### Phase 1 — Audit and classification

Create a current-state audit:

```text
docs/_incoming_active/CONSOLE-UX-BATCH-4-ALLAGMA-DETAILS-001/IMPLEMENTATION_NOTES.md
```

For each page, record:

```text
current visible cards
current warnings/notices
current actions
current evidence/export links
current data source posture
current local wrappers
canonical primitives already used
migration risk
```

### Phase 2 — Signal builders

Add dedicated builders in:

```text
src/system/operatorConsoleSignals.ts
```

Likely builders:

```text
buildAllagmaStartRunSignalGroups
buildAllagmaReplayEvidenceSignalGroups
buildAllagmaEvaluationDetailSignalGroups
buildAllagmaEvaluationDatasetsSignalGroups
buildAllagmaBaselineComparisonWorkbenchSignalGroups
buildAllagmaBaselineComparisonDetailSignalGroups
buildAllagmaAuditEntrySignalGroups
```

Keep signals limited to three groups:

```text
System
Context
Evidence / Safety / Contract depending on page
```

### Phase 3 — Page migration

For each page:

```text
- Wrap in OperatorPageFrame if not already.
- Add OperatorSignalSummary in statusSlot.
- Move refresh/start/export/primary navigation into actionsSlot when appropriate.
- Keep one primary work surface visible.
- Collapse secondary notes, raw payloads, source limitations, and export/debug blocks.
```

### Phase 4 — Evidence and export preservation

Every page with run/evaluation/replay evidence must retain links to:

```text
Evidence Spine
Agent Interaction where relevant
Run Detail / Audit Journey where relevant
export bundle where relevant
```

### Phase 5 — Adoption matrix and tests

Update:

```text
docs/generated/console-ui-adoption-matrix.json
docs/ux/CONSOLE_PAGE_MIGRATION_PLAN.md
docs/ux/CONSOLE_CANONICAL_UI_SET.md
```

Tests:

```text
npm run typecheck
npm run console-ui:check
vitest for touched pages + signal builders
Playwright for start run / replay / evaluation detail if existing
```

## Acceptance criteria

```text
- All listed Allagma routes appear in the adoption matrix.
- Each route is adoption level 4 or explicitly classified otherwise.
- No new page-local wrapper is introduced.
- Evidence/export/debug details remain reachable.
- Start Run still supports governed fake preset and domain context.
- Replay page remains compatible with REPLAY-RUNTIME-002.
- Evaluation evidence-quality honesty remains intact.
```

---

# 2. CONSOLE-UX-BATCH-5-KANON-SEMANTIC-PAGES-001

## Goal

Finish canonical UX migration for remaining Kanon semantic authority pages.

Routes to cover:

```text
/kanon/source-bindings
/kanon/facts
/kanon/plans
/kanon/decisions
/kanon/domain-packs
/kanon/assistance
/kanon/policies
```

These routes are active in the router, while only Kanon overview, ontology versions, and review queue are already clearly migrated in the current adoption matrix.

## Main UX problem

Kanon pages carry heavy semantic-authority concepts: source bindings, facts, plans, decisions, provenance, domain packs, policies, assistance, and review state. The risk is overexplaining everything all the time.

The target principle:

```text
semantic authority remains explicit,
but raw provenance/expert explanation moves behind disclosure.
```

## Target pages

| Route                    | Desired default surface                           | Collapse into disclosures                                            |
| ------------------------ | ------------------------------------------------- | -------------------------------------------------------------------- |
| `/kanon/source-bindings` | Binding table/workbench, selected binding actions | Lifecycle explanation, source quality detail, raw provenance         |
| `/kanon/domain-packs`    | Pack inventory + active pack actions              | Inventory metrics, generated-looking warnings, validation details    |
| `/kanon/decisions`       | Decision/provenance list + lookup                 | Decision lifecycle docs, raw records, replay/provenance export       |
| `/kanon/assistance`      | Assistance request/result workflow                | Conexus model-call details, safety defaults, raw prompt/context      |
| `/kanon/facts`           | Facts/plans primary table or workbench            | Raw semantic graph/expert notes                                      |
| `/kanon/plans`           | Semantic plan list/detail                         | Plan internals, trace/debug                                          |
| `/kanon/policies`        | Action policy status and review actions           | Capability matrix, forbidden action explanation, raw policy payloads |

## Implementation phases

### Phase 1 — Kanon semantic page audit

For each route:

```text
- What is the operator’s primary job?
- What is semantic-authority state?
- What is evidence/provenance state?
- What is raw/debug/expert state?
- What should remain visible?
- What should become disclosure?
```

### Phase 2 — Signal builders

Add:

```text
buildKanonSourceBindingsSignalGroups
buildKanonDomainPacksSignalGroups
buildKanonDecisionsSignalGroups
buildKanonAssistanceSignalGroups
buildKanonFactsSignalGroups
buildKanonPlansSignalGroups
buildKanonPoliciesSignalGroups
```

Recommended groups:

```text
System
Domain / Context
Authority / Evidence / Safety
```

### Phase 3 — Page migration

Use:

```text
OperatorPageFrame
OperatorSignalSummary
OperatorDisclosure
OperatorDataTable where applicable
DestructiveConfirmDialog for dangerous actions
```

No new Kanon-local wrapper unless classified as `page_specific_allowed`.

### Phase 4 — Preserve parity and source-binding nuance

Do not break the distinction already established:

```text
ServerOnly in Kanon.Client does not necessarily mean SPA cannot call it.
Some operator routes are documented operator-http paths.
```

Update coverage docs if page behavior changes.

### Phase 5 — Tests and adoption matrix

Update:

```text
console-ui-adoption-matrix.json
CONSOLE_PAGE_MIGRATION_PLAN.md
CONSOLE_CANONICAL_UI_SET.md
```

Run:

```text
npm run typecheck
npm run console-ui:check
Kanon page vitest tests
Kanon parity hardening if routes/clients touched
Domain Switcher smoke if domain-pack/source-binding behavior changes
```

## Acceptance criteria

```text
- Every active Kanon route is migrated or explicitly classified.
- Semantic authority is visible without overwhelming the page.
- Source binding/domain pack/decision evidence remains reachable.
- Dangerous actions remain confirmed.
- Domain Switcher compatibility remains intact.
- Kanon route parity and UI/API coverage remain valid.
```

---

# 3. CONSOLE-UX-BATCH-6-CONEXUS-CHAT-001

## Goal

Migrate the remaining Conexus Chat route to the canonical console pattern.

Route:

```text
/conexus/chat
```

This route is active in the router, while the adoption matrix already covers Conexus overview, projects, routing, and observability.

## Main UX problem

Chat pages often become demo-like or raw-provider-like. In Ontogony, Conexus Chat should clearly show:

```text
this is a gateway/operator test surface,
not the main governed execution path.
```

It should distinguish:

```text
fake provider
real OpenAI provider
alias
project key
route decision
model call evidence
token usage
errors
```

## Target shape

Default surface:

```text
- prompt/input
- selected project/alias/model route context
- response/result
- model-call evidence link if available
```

Collapsed disclosures:

```text
- provider posture
- route decision detail
- raw request/response
- usage/token metadata
- fake-vs-real explanation
- limitations
```

## Implementation phases

### Phase 1 — Audit

Inspect:

```text
src/conexus/pages/ConexusChatPage.tsx
related components/hooks
Conexus model-call evidence links
route decision links
provider posture components
```

### Phase 2 — Signal builder

Add:

```text
buildConexusChatSignalGroups
```

Suggested groups:

```text
System:
  configured / not configured / sending / response ready / failed

Context:
  project / alias / provider mode

Evidence:
  model call linked / route decision linked / tokens recorded / no evidence yet
```

### Phase 3 — Page migration

Use canonical primitives:

```text
OperatorPageFrame
OperatorSignalSummary
OperatorDisclosure
Button in actionsSlot if refresh/clear/export exists
```

### Phase 4 — Evidence integration

Ensure chat result can link to:

```text
Conexus Observability
Evidence Spine by modelCallId / traceId if available
route decision explorer if routeDecisionId exists
```

### Phase 5 — Tests

Run:

```text
npm run typecheck
npm run console-ui:check
Conexus Chat unit tests
Conexus observability/evidence tests if touched
```

## Acceptance criteria

```text
- /conexus/chat is in adoption matrix.
- Chat clearly shows fake/real/provider posture.
- Model-call and route-decision evidence remains reachable.
- Raw request/response is not default-visible.
- No new chat-specific layout system is introduced.
```

---

# 4. CONSOLE-UX-FINAL-ADOPTION-GATE-001

## Goal

Prevent the console from fragmenting again.

The adoption matrix currently documents migrated pages, but the next maturity step is to compare the router against the matrix and force every operator route into an explicit classification.

## Why this matters

Right now, route migration has been done by batches. The final gate should make route classification automatic:

```text
New route added?
It must be classified.
```

## Required route classifications

Every route in `src/app/routes.tsx` should be classified as one of:

```text
migrated_level_5
migrated_level_4
intentionally_untracked
legacy_hidden
dev_only
deprecated
external_redirect
non_operator
```

## Implementation phases

### Phase 1 — Route extraction

Create or update script:

```text
scripts/console-ui/extract-operator-routes.mjs
```

Inputs:

```text
src/app/routes.tsx
docs/generated/console-ui-adoption-matrix.json
```

Output:

```text
docs/generated/console-ui-route-classification.json
```

### Phase 2 — Gate script

Update:

```text
npm run console-ui:check
```

to fail if:

```text
- operator route missing from adoption/classification matrix
- route marked migrated but page file missing
- route marked level 4/5 without canonical imports
- route exposes evidence/export/debug but lacks reachability tests
- banned bridge import appears
- root @ontogony/ui imports appear where subpath import required
```

### Phase 3 — Explicit exceptions

Some routes may be intentionally untracked, but must say why.

Example:

```json
{
  "route": "/some/dev/path",
  "classification": "dev_only",
  "reason": "not rendered in operator nav"
}
```

### Phase 4 — CI integration

Add this to local and CI scripts:

```text
console-ui:check
contracts:discipline
```

Do not make it too strict for unrelated backend-only changes, but strict for frontend route changes.

## Acceptance criteria

```text
- Every route in routes.tsx is classified.
- Adding a new operator route without classification fails console-ui:check.
- Adoption matrix and route classification cannot drift silently.
- Existing migrated pages remain green.
- Docs explain how to classify a new route.
```

---

# 5. REPLAY-RUNTIME-002

## Goal

Wire the replay shell into frontend clients and existing UI surfaces without attempting full cross-service replay orchestration yet.

Current REPLAY-RUNTIME-001 status, based on your prior reports:

```text
- replay vocabulary and Allagma-owned replay request shell exist
- Kanon eligibility became repository-backed
- target resolver and safety policy fixes landed locally
- frontend/generated client wiring remained deferred
```

REPLAY-RUNTIME-002 should close that gap.

## Scope

Do:

```text
- OpenAPI/generated client sync for Allagma replay routes.
- OpenAPI/generated client sync for Kanon replay eligibility.
- Frontend client functions.
- Small panel on existing /allagma/replay surface.
- Evidence Spine links where replay target produces identifiers.
- Honest replay mode labels.
```

Do not:

```text
- full cross-service replay orchestration
- Conexus dry-run execution
- real provider/tool calls
- new top-level Replay Workbench beyond existing route
```

## Implementation phases

### Phase 1 — Contract sync

Update:

```text
allagma OpenAPI snapshot
kanon OpenAPI snapshot
frontend generated schemas
allagmaClient.ts
kanonClient.ts
route-workflow-catalog.json
API_CLIENT_ROUTE_USAGE.json
manual DTO shim registry if needed
```

### Phase 2 — Frontend clients

Add functions such as:

```text
createReplayRequest
getReplayRequest
getReplayBundle
getReplayDelta
checkKanonReplayEligibility
```

### Phase 3 — Replay page panel

On `/allagma/replay`, add canonical UI:

```text
OperatorPageFrame
OperatorSignalSummary
OperatorDisclosure
Replay eligibility panel
Replay request status
Bundle/delta preview in disclosure
Safety policy disclosure
Evidence Spine link if target resolves
```

### Phase 4 — Honest state model

Labels:

```text
deterministic_simulation
evidence_only
reconstructed
unavailable
not_supported
missing_source_data
```

### Phase 5 — Tests

Run:

```text
npm run typecheck
npm run console-ui:check
replay page vitest
contracts:discipline
Allagma route parity
Kanon route parity
```

## Acceptance criteria

```text
- Frontend can create/read replay requests.
- Frontend can display bundle and delta from existing backend shell.
- Kanon eligibility states are visible and honest.
- No real provider/tool execution is possible.
- Replay UI remains on existing /allagma/replay surface.
- contracts:discipline passes.
```

---

# 6. RUNTIME-CONFIG-001

## Goal

Make the static/operator frontend runtime-configurable and clean up local/docker/custom environment profiles.

This should not become production security. It is about endpoint/profile clarity.

## Problem

Today, the frontend likely mixes:

```text
VITE_* build-time values
browser-local settings
test seeding
Docker assumptions
localhost ports
operator overrides
```

The goal is:

```text
A Docker-served frontend can point to different backend service URLs without rebuild.
```

## Runtime config contract

Proposed file:

```text
/operator-runtime-config.json
```

or:

```text
/config/operator-runtime.json
```

after auditing nginx/static serving.

Minimum fields:

```json
{
  "schema": "ontogony-operator-runtime-config-v1",
  "environmentName": "docker-local",
  "profileName": "local-working-system",
  "services": {
    "conexus": { "baseUrl": "http://localhost:5081" },
    "kanon": { "baseUrl": "http://localhost:5082" },
    "allagma": { "baseUrl": "http://localhost:5083" }
  },
  "frontend": {
    "enableFixtureRoutes": true,
    "enableExpertModeDefault": false
  },
  "localAlpha": {
    "allowBrowserCredentialStorage": true,
    "showLocalCredentialWarnings": true
  }
}
```

No secrets.

## Precedence

```text
1. hardcoded safe fallback
2. runtime config defaults
3. browser-local persisted operator settings
4. session-only overrides
5. explicit URL/test overrides
6. page-local form overrides
```

## Implementation phases

### Phase 1 — Audit

Find all:

```text
VITE_* usage
hardcoded service URLs
settings defaults
Docker frontend env handling
Playwright seed helpers
system truth smoke assumptions
```

### Phase 2 — Loader

Add:

```text
runtime config schema/type
runtime config loader
bootstrap integration
validation
fallback behavior
```

### Phase 3 — Settings provenance

Settings page should show:

```text
runtime default
local override
session override
test override
missing
legacy unknown
```

Use progressive disclosure, not more warning cards.

### Phase 4 — Docker/local-working-system

Add generation/mounting of runtime config so nginx/static frontend can change endpoints without rebuild.

### Phase 5 — Tests

```text
typecheck
runtime config unit tests
settings provenance tests
system truth tests
docker-live smoke
governed fake E2E if URLs touched
```

## Acceptance criteria

```text
- Docker frontend can change service URLs without rebuild.
- Dev mode still works if runtime config is missing.
- Settings distinguishes defaults from overrides.
- System Truth / Evidence Spine / Domain Switcher / Agent Interaction still use correct URLs.
- No secrets in runtime config.
```

---

# 7. SYSTEM-LEARNING-GUIDE-001

## Goal

Create a canonical learning path for understanding, running, debugging, and extending Ontogony.

This should not add more random docs. It should consolidate.

## Canonical guide structure

Create under platform:

```text
docs/learn/INDEX.md
docs/learn/00_START_HERE.md
docs/learn/01_ARCHITECTURE_MAP.md
docs/learn/02_RUN_LOCAL_SYSTEM.md
docs/learn/03_GOVERNED_FAKE_E2E.md
docs/learn/04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md
docs/learn/05_EVIDENCE_SPINE.md
docs/learn/06_AGENT_INTERACTION.md
docs/learn/07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md
docs/learn/08_CONTRACT_DISCIPLINE.md
docs/learn/09_ADD_A_DOMAIN.md
docs/learn/10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md
docs/learn/11_ADD_OR_CHANGE_AN_API_ROUTE.md
docs/learn/12_ADD_A_FRONTEND_PAGE.md
docs/learn/13_ADD_AN_EVALUATION_OR_BASELINE.md
docs/learn/14_DEBUGGING_PLAYBOOK.md
docs/learn/15_UI_CANONICALIZATION_AND_CONSOLE_UX.md
docs/learn/GLOSSARY.md
```

## Implementation phases

### Phase 1 — Docs audit

Matrix:

```text
path
repo
audience
current / stale / duplicate / generated / reference
canonical replacement
action: keep / update / archive / delete_candidate / generated_do_not_edit
```

### Phase 2 — Write first guides

Highest priority:

```text
00_START_HERE
01_ARCHITECTURE_MAP
02_RUN_LOCAL_SYSTEM
03_GOVERNED_FAKE_E2E
05_EVIDENCE_SPINE
07_DOMAIN_MODEL_ROUTING_BOUNDARIES
08_CONTRACT_DISCIPLINE
15_UI_CANONICALIZATION_AND_CONSOLE_UX
GLOSSARY
```

### Phase 3 — Extension guides

Then:

```text
ADD_A_DOMAIN
ADD_A_PROVIDER_OR_MODEL_ALIAS
ADD_OR_CHANGE_AN_API_ROUTE
ADD_A_FRONTEND_PAGE
ADD_AN_EVALUATION_OR_BASELINE
DEBUGGING_PLAYBOOK
```

### Phase 4 — Stale docs cleanup

Do not delete blindly. Mark first:

```text
This document is historical. Canonical guide: ...
```

Then archive/delete in a later cleanup.

### Phase 5 — Validation

Add or run checks:

```text
broken links
referenced script exists
generated artifact exists
canonical index completeness
glossary term coverage
```

## Acceptance criteria

```text
- New developer can start local system from docs alone.
- Developer can run governed fake E2E from docs alone.
- Developer understands domain vs model purpose vs Conexus alias vs provider route.
- Developer can change an API route and know every artifact/check to update.
- Stale docs are identified and not left as parallel truth.
- Generated docs are linked, not copied.
```

---

# Recommended exact execution order

```text
1. CONSOLE-UX-BATCH-4-ALLAGMA-DETAILS-001
2. CONSOLE-UX-BATCH-5-KANON-SEMANTIC-PAGES-001
3. CONSOLE-UX-BATCH-6-CONEXUS-CHAT-001
4. CONSOLE-UX-FINAL-ADOPTION-GATE-001
5. REPLAY-RUNTIME-002
6. RUNTIME-CONFIG-001
7. SYSTEM-LEARNING-GUIDE-001
```

## Why this order

The remaining console pages should be cleaned before the final adoption gate. The final adoption gate should land before runtime/replay/docs work, because those packages will touch routes, settings, and guides. Then Replay Runtime 002 can use the newly migrated `/allagma/replay` surface. Runtime Config should follow because it will affect Settings/System Truth/Evidence pages. The learning guide should come last so it documents the system after the console and runtime shape stabilize.
