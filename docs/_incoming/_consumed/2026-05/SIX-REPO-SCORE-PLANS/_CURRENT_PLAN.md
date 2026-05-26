Below are three execution plans to push the three weaker repos **above 9** without corrupting their boundaries. The key is not to make them “do more product work,” but to make each one **more mechanically authoritative, release-locked, contract-tested, and system-verifiable**.

The earlier uploaded cohesion review correctly identified that the runtime loop was connected but not yet fully production-grade/system-locked . The current Allagma side has since become much stronger with system matrices, runtime lock, route/env/auth/test matrices, trace context, feature connection, error contracts, and acceptance gates.  The next lift is to bring `ontogony-platform`, `ontogony-frontend`, and `ontogony-ui` into that same level of release discipline.

---

# Target scores

| Repo                | Current |   Target | Main lift                                                                        |
| ------------------- | ------: | -------: | -------------------------------------------------------------------------------- |
| `ontogony-platform` |    8.65 | **9.15** | Become the six-repo compatibility and mechanical contract substrate              |
| `ontogony-frontend` |    8.15 | **9.10** | Become a release-grade operator console, not merely a broad live console         |
| `ontogony-ui`       |    8.10 | **9.05** | Become a packed, multi-console UI mechanics foundation with hard consumer proofs |

---

# Plan 1 — `ontogony-platform` above 9

## Strategic objective

`ontogony-platform` should not become a runtime orchestrator. Its score should rise by becoming the **formal mechanical contract authority** for the whole six-repo system.

Its current boundary is already correct: it is a mechanics substrate for tracing, errors, startup guards, HTTP, hashing, idempotency, logging, redaction, secrets, persistence/outbox, actor context, AI telemetry, artifacts, and execution journal facts — while explicitly excluding Kanon semantics, Allagma orchestration, Conexus routing, product workflows, and UI. 

The goal is therefore:

```text
Platform does not own meaning.
Platform does not own orchestration.
Platform does not own UI.

Platform owns the contracts that make the system mechanically coherent.
```

## Target score

| Dimension               | Current |   Target |
| ----------------------- | ------: | -------: |
| System integration      |     8.3 |  **9.1** |
| Tightness / correctness |     9.0 |  **9.2** |
| Overall                 |    8.65 | **9.15** |

## PLAT-9-001 — Six-repo system compatibility gate

**Status (2026-05-26):** **Done** — [`docs/evidence/PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md`](../../../evidence/PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md).

**Purpose:** promote `Ontogony.SystemCompatibility` from four-runtime-repo validation into a six-repo compatibility authority.

Today, the Allagma runtime lock pins only the four runtime repos: `ontogony-platform`, `conexus-dotnet`, `kanon-dotnet`, and `allagma-dotnet`.  That is good, but it leaves frontend/UI outside the same reproducibility unit.

Add a platform-owned six-repo schema:

```text
docs/contracts/ONTOGONY_SIX_REPO_COMPATIBILITY_LOCK.md
schemas/system/ontogony-six-repo-lock.schema.json
src/Ontogony.SystemCompatibility/SixRepoCompatibilityGate.cs
tests/Ontogony.SystemCompatibility.Tests/SixRepoCompatibilityGateTests.cs
```

The lock should cover:

```json
{
  "repos": {
    "ontogony-platform": { "commit": "...", "packageVersion": "0.3.0-alpha.1" },
    "conexus-dotnet": { "commit": "...", "clientVersion": "0.1.0-alpha.1" },
    "kanon-dotnet": { "commit": "...", "clientVersion": "0.1.0-alpha.0" },
    "allagma-dotnet": { "commit": "...", "apiPrefix": "/allagma/v0" },
    "ontogony-ui": { "commit": "...", "packageVersion": "0.1.0-alpha.0", "tarballSha256": "..." },
    "ontogony-frontend": { "commit": "...", "buildProvenanceSha256": "..." }
  },
  "contracts": {
    "openapiSnapshots": {},
    "routeInventories": {},
    "frontendRouteInventory": {},
    "uiPublicSubpaths": {},
    "agentInteractionPackage": {}
  }
}
```

Acceptance:

```powershell
./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev
```

Must validate:

```text
- all six repos exist;
- pinned commits match or post-lock deltas are classified;
- runtime API prefixes match;
- frontend route inventory matches;
- ui package exports match;
- OpenAPI snapshots are current;
- frontend generated clients match snapshots;
- ontogony-ui packed tarball can be consumed by frontend;
- no obsolete product naming in active source/docs;
- build provenance exists for frontend and UI package.
```

This is the single biggest move to lift platform integration above 9.

## PLAT-9-002 — Shared mechanical protocol registry

**Status (2026-05-26):** **Done** — [`docs/contracts/MECHANICAL_PROTOCOL_REGISTRY.md`](../../../contracts/MECHANICAL_PROTOCOL_REGISTRY.md).

Platform already contains protocol-neutral DTOs, typed event envelopes, idempotency primitives, error contracts, HTTP mechanics, and AI telemetry.  The next step is to make these discoverable as a single protocol registry.

Add:

```text
docs/contracts/MECHANICAL_PROTOCOL_REGISTRY.md
docs/contracts/HEADER_PROPAGATION_CONTRACT.md
docs/contracts/CROSS_SERVICE_ERROR_CONTRACT.md
docs/contracts/EVIDENCE_IDENTIFIER_CONTRACT.md
docs/contracts/IDEMPOTENCY_CONTRACT.md
docs/contracts/TRACE_CONTEXT_CONTRACT.md
schemas/contracts/*.schema.json
```

Platform should define the mechanical shape of:

```text
traceparent
X-Correlation-ID
X-Ontogony-Actor-Id
X-Ontogony-Actor-Type
X-Ontogony-Actor-Roles
Idempotency-Key / X-Ontogony-Idempotency-Key
X-Allagma-Run-Id
CrossServiceErrorEnvelope
EvidenceIdentifier
EventEnvelope
ReplayArtifactReference
```

Acceptance:

```text
- Conexus, Kanon, Allagma, frontend, and UI docs link to the registry.
- Runtime repos prove compliance with tests.
- Frontend proves it displays these identifiers consistently.
- UI proves it has neutral rendering contracts for these identifiers.
```

## PLAT-9-003 — Consumer conformance suite

**Status (2026-05-26):** **Done** — [`docs/consumer-blueprints/CONSUMER_CONFORMANCE_SUITE.md`](../../../consumer-blueprints/CONSUMER_CONFORMANCE_SUITE.md), [`docs/evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md`](../../../evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md). Optional follow-on: full-matrix `artifacts/consumer-conformance/` runner.

Platform should stop relying only on examples and package-mode smoke. It already has active alpha consumers and package-mode governance.  Now formalize consumer conformance.

Add:

```text
tests/Ontogony.ConsumerConformance.Tests/
docs/consumer-blueprints/CONSUMER_CONFORMANCE_SUITE.md
scripts/run-consumer-conformance.ps1
```

Conformance matrix:

| Consumer | Required proof                                                            |
| -------- | ------------------------------------------------------------------------- |
| Conexus  | package-mode build, error/trace/idempotency compatibility                 |
| Kanon    | package-mode build, route/error/decision-record compatibility             |
| Allagma  | package-mode build, cross-service propagation, runtime lock compatibility |
| Frontend | generated client snapshot compatibility, route inventory compatibility    |
| UI       | packed package import/export compatibility                                |

Acceptance should produce:

```text
artifacts/consumer-conformance/<timestamp>/summary.json
```

## PLAT-9-004 — Public API hardening pass

**Status (2026-05-26):** **Done** — 25 Tier A packages (`CS1591` enforced); Tier C: `Ontogony.Testing`, `Ontogony.SystemCompatibility` — [`docs/evidence/PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md`](../../../evidence/PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md).

Platform’s correctness is already high, but to get above 9 it needs stricter public API quality.

Do:

```text
- public API snapshot per package;
- breaking-change classifier;
- migration-note required when API surface changes;
- XML-doc Tier A expansion beyond Conexus-facing surface;
- package dependency graph diff;
- no accidental product terms in public names;
- package-level compatibility tests against current Conexus/Kanon/Allagma pins.
```

Acceptance:

```powershell
./scripts/validate-public-api-baseline.ps1
./scripts/validate-package-levels.ps1
./scripts/validate-shipping-inventory.ps1
./scripts/run-consumer-conformance.ps1
```

## PLAT-9-005 — Observability mechanics pack

**Status (2026-05-26):** **Done** — phase 1 + 2 — [`docs/observability/`](../../../observability/), [`docs/evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md`](../../../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md), `scripts/run-observability-mechanics-conformance.ps1`.

Platform should own reusable mechanics, not dashboards for product semantics. But it can own the **mechanical observability contract**.

Add:

```text
docs/observability/OBSERVABILITY_MECHANICS_CONTRACT.md
docs/observability/STANDARD_SPAN_NAMES.md
docs/observability/STANDARD_LOG_FIELDS.md
docs/observability/STANDARD_METRIC_TAGS.md
src/Ontogony.Observability/SystemCorrelationConventions.cs
```

Acceptance:

```text
- Conexus provider route decision has standard trace/correlation fields.
- Kanon decision record has standard trace/correlation fields.
- Allagma run event has standard trace/correlation fields.
- Frontend can deep-link from each surface using the same identifiers.
```

## PLAT-9-006 — Platform “do not own meaning” guard

**Status (2026-05-26):** **Done** — `scripts/check-no-product-semantics.ps1`, `tests/Ontogony.Architecture.Tests/ProductSemanticLeakageTests.cs`.

The boundary is already correct; make it enforceable.

Add static guards:

```text
scripts/check-no-product-semantics.ps1
tests/Ontogony.Architecture.Tests/ProductSemanticLeakageTests.cs
```

Forbidden concepts in platform source:

```text
provider routing policy
ontology truth
canonical fact resolution
agent plan semantics
business approval rules
domain pack promotion logic
operator page behavior
frontend route behavior
```

Allowed concepts:

```text
error envelope
trace propagation
idempotency
event envelope
artifact reference
actor context
hashing
HTTP resilience
redaction mechanics
secret reference resolution
```

## Platform done-definition

`ontogony-platform` is above 9 when:

```text
1. Six-repo compatibility lock schema exists.
2. Six-repo compatibility gate passes locally.
3. Consumer conformance suite covers Conexus, Kanon, Allagma, frontend, UI.
4. Public API baselines and breaking-change classifier are mandatory.
5. Mechanical protocol registry is consumed by all product repos.
6. Observability conventions are shared and tested.
7. Product-semantic leakage guard passes.
8. Platform package-mode consumers remain green.
```

---

# Plan 2 — `ontogony-frontend` above 9

## Strategic objective

`ontogony-frontend` is already broad. It is a unified operator console for Conexus, Kanon, and Allagma, built on `@ontogony/ui`, and its README claims live operator pages, health badges, settings, topology, Conexus routing, Kanon ontology versions, Allagma run list, trace correlation, tests, and OpenAPI snapshots. 

The problem is not breadth. The problem is **release-grade certainty**.

The goal:

```text
Turn the frontend from a broad live console into a locked, reproducible, evidence-backed operator surface.
```

## Target score

| Dimension               | Current |   Target |
| ----------------------- | ------: | -------: |
| System integration      |     8.1 | **9.25** |
| Tightness / correctness |     8.2 |  **9.0** |
| Overall                 |    8.15 | **9.10** |

## FE-9-001 — Six-repo release-lock integration

Frontend must become part of the system release lock, not merely a sibling consumer.

Current dependencies are local file links to `@ontogony/ui` and `@ontogony/agent-interaction`.  That is fine for local development, but insufficient for release reproducibility.

Add:

```text
docs/system/FRONTEND_RELEASE_LOCK.md
docs/generated/frontend-release-lock.json
scripts/sync-frontend-release-lock.mjs
scripts/check-frontend-release-lock.mjs
```

The lock should record:

```json
{
  "ontogonyFrontendCommit": "...",
  "ontogonyUiCommit": "...",
  "ontogonyUiTarballSha256": "...",
  "agentInteractionCommit": "...",
  "agentInteractionTarballSha256": "...",
  "openapiSnapshotHashes": {
    "conexus": "...",
    "kanon": "...",
    "allagma": "..."
  },
  "generatedClientHashes": {},
  "routeInventoryHash": "...",
  "workflowInventoryHash": "...",
  "buildProvenanceHash": "..."
}
```

Acceptance:

```bash
npm run frontend-release-lock:check
npm run check
npm run check:full
```

## FE-9-002 — Backend contract parity gate

The frontend already has many contract scripts: OpenAPI drift, route-client drift, Kanon route parity, live artifact evidence journey, readiness scorecards, fixture/live audits, and Docker-live E2E scripts.  The next step is to turn this into one canonical backend parity gate.

Add:

```text
scripts/run-backend-contract-parity.mjs
docs/contracts/BACKEND_CONTRACT_PARITY.md
docs/generated/backend-contract-parity.summary.json
```

Gate should validate:

```text
Conexus:
- /v1/models used by routing page
- /v1/chat/completions assumptions
- admin route-decision/evidence surfaces
- streaming evidence surfaces

Kanon:
- route inventory parity
- generated client coverage
- server-only routes not imported by product pages
- assistance/provenance/replay surfaces

Allagma:
- run list/detail/resume/audit/replay routes
- evaluation/baseline comparison routes
- evidence journey links
- event vocabulary mapping
```

Acceptance:

```bash
npm run backend-contract-parity:check
```

No page should call an endpoint absent from the current snapshots.

## FE-9-003 — Operator journey acceptance suite

The frontend should not only test pages. It should test **operator journeys**.

Add E2E groups:

```text
e2e/journeys/system-health-to-run-audit.spec.ts
e2e/journeys/conexus-route-to-model-call-evidence.spec.ts
e2e/journeys/kanon-decision-to-provenance-replay.spec.ts
e2e/journeys/allagma-run-to-evidence-graph.spec.ts
e2e/journeys/human-gate-resolution.spec.ts
e2e/journeys/streaming-model-call-evidence.spec.ts
e2e/journeys/six-repo-release-posture.spec.ts
```

Each journey should assert:

```text
- live mode vs fixture mode is explicit;
- degraded states are honest;
- unavailable evidence is not silently hidden;
- trace/correlation/run/model/decision IDs are visible or copyable;
- route links remain valid;
- redaction is applied before shared UI rendering;
- keyboard path works;
- mobile path does not lose critical actions.
```

Acceptance:

```bash
npm run test:e2e:operator-journeys
```

## FE-9-004 — Evidence spine as first-class navigation layer

The frontend already has evidence spine and agent-interaction routes. The README lists `/system/evidence-spine` and `/system/agent-interaction`.  Make the evidence spine the navigation skeleton across all major pages.

Add a reusable frontend service:

```text
src/system/evidence-spine/
  resolveEvidenceJourney.ts
  buildEvidenceLinks.ts
  evidenceSurfaceRegistry.ts
  evidenceCompleteness.ts
```

Every detail page should answer:

```text
What entity am I looking at?
What upstream/downstream evidence exists?
What is missing?
What is unavailable because backend lacks it?
What is unavailable because auth/config is missing?
What link should the operator follow next?
```

Acceptance:

```text
- Run detail links to model call, Kanon decision, replay, audit, trace.
- Evaluation detail links to baseline comparison and dataset evidence.
- Conexus route decision links to model call and provider fallback evidence.
- Kanon decision links to provenance/replay bundle.
- Evidence-spine workbench resolves all supported identifiers.
```

## FE-9-005 — Live/fixture boundary hardening

Frontend score cannot exceed 9 if fixture/demo data can be mistaken for live system state.

Add:

```text
src/system/liveMode/liveBoundary.ts
src/system/liveMode/LiveBoundaryBanner.tsx
docs/architecture/LIVE_FIXTURE_BOUNDARY.md
scripts/check-fixture-leakage.mjs
```

Rules:

```text
- Fixture data must be visually marked.
- Live pages must not silently fall back to fixture data.
- Demo fallback must require explicit mode.
- Backend auth failure must be distinct from backend unavailable.
- Empty live result must be distinct from fixture not loaded.
```

Acceptance:

```bash
npm run fixtures:check
npm run test:e2e -- e2e/live-fixture-boundary.spec.ts
```

## FE-9-006 — Frontend release candidate gate

Create a single command that makes the frontend release-grade:

```bash
npm run rc:check
```

It should run:

```text
- npm run check
- npm run check:full
- backend contract parity
- frontend release lock check
- packed @ontogony/ui consumer check
- Docker-live operator journeys
- axe/accessibility smoke
- performance budgets
- build provenance validation
- no obsolete naming guard
- evidence journey completeness check
```

Produce:

```text
artifacts/frontend-rc/<timestamp>/summary.json
artifacts/frontend-rc/<timestamp>/summary.md
dist/provenance.json
```

## FE-9-007 — Operator readiness scorecard

Add an operator-facing scorecard page and generated artifact:

```text
/system/release-readiness
docs/generated/operator-release-readiness.json
```

Score every route:

```text
route exists
live client wired
fixture mode marked
loading/error/empty states
contract snapshot covered
E2E covered
evidence links present
redaction policy applied
keyboard accessible
mobile usable
```

Acceptance:

```bash
npm run readiness:check
```

## Frontend done-definition

`ontogony-frontend` is above 9 when:

```text
1. It participates in six-repo release lock.
2. All generated clients are snapshot-hash verified.
3. Backend contract parity gate passes.
4. Operator journey E2E covers Conexus, Kanon, Allagma, system, evidence spine.
5. Fixture/live boundary is impossible to confuse.
6. Evidence navigation exists across all major detail surfaces.
7. RC gate emits machine-readable evidence.
8. Release-readiness scorecard passes for all primary routes.
```

---

# Plan 3 — `ontogony-ui` above 9

## Strategic objective

`ontogony-ui` is already clean as a shared UI mechanics package. It explicitly owns reusable operator UI patterns and neutral presentation contracts, while excluding product workflows, generated clients, authentication, backend clients, and domain-specific redaction. 

The problem is not correctness. The problem is indirect system integration.

The goal:

```text
Make @ontogony/ui a hard-proven multi-console mechanics foundation,
not merely a good shared component library used by one product console.
```

## Target score

| Dimension               | Current |   Target |
| ----------------------- | ------: | -------: |
| System integration      |     7.6 |  **9.0** |
| Tightness / correctness |     8.6 |  **9.1** |
| Overall                 |    8.10 | **9.05** |

## UI-9-001 — Packed multi-console consumer gate

The package already has many public subpaths and strong packaging checks: exports, bundle audits, API audits, density, visual/a11y, dry pack, publint, smoke dist, fixture consumer, ATTW, knip, size, Storybook, multi-console docs, consumer checks, and E2E. 

Now make the consumer gate stricter and product-realistic.

Create fixture consumers:

```text
fixtures/multi-console-consumers/conexus-console/
fixtures/multi-console-consumers/kanon-console/
fixtures/multi-console-consumers/allagma-console/
fixtures/multi-console-consumers/system-console/
```

Each must install the **packed tarball**, not use source imports.

Each must prove:

```text
- imports public subpaths only;
- no @ontogony/ui/src imports;
- no product DTOs in shared package;
- no generated clients in shared package;
- shell/nav/status/system/semantic/execution components compile;
- tree-shaking and CSS import work;
- CJS/ESM entrypoints work where expected.
```

Acceptance:

```bash
npm run check:consumers
npm run check:full
```

## UI-9-002 — Neutral contract hardening

The readiness matrix correctly says product apps must map APIs into neutral UI contracts, while generated clients, product DTOs, authorization engines, and domain-specific redaction stay out of UI.  Strengthen that into formal contracts.

Add:

```text
docs/contracts/NEUTRAL_UI_CONTRACTS.md
docs/contracts/SYSTEM_UI_CONTRACTS.md
docs/contracts/SEMANTIC_UI_CONTRACTS.md
docs/contracts/EXECUTION_UI_CONTRACTS.md
docs/contracts/OBSERVABILITY_UI_CONTRACTS.md
src/contracts/
```

Contract families:

```text
SystemNodeStatus
ServiceReadiness
TraceCorrelationView
EvidenceLink
EvidenceCompleteness
SemanticDecisionSummary
ProvenanceTimeline
ReplayBundleSummary
ExecutionRunSummary
ExecutionEventTimeline
HumanGateSummary
ModelCallEvidenceSummary
IncidentBundle
SafeExportPayload
```

Acceptance:

```text
- every exported high-level component consumes neutral contracts;
- no component imports product DTOs;
- adapter examples are marked reference-only;
- contract docs include mapping examples for frontend, not product logic.
```

## UI-9-003 — Evidence and trace UI kit

The full system now revolves around evidence, traceability, and cross-repo audit. UI should own neutral rendering primitives for that.

Add components:

```text
@ontogony/ui/system
  EvidenceJourneyMap
  TraceCorrelationStrip
  EvidenceCompletenessBadge
  RuntimeLockBadge
  ServiceReadinessMatrix

@ontogony/ui/execution
  RunEventTimeline
  HumanGateCard
  ReplayEvidencePanel
  IdempotencyStatusPill
  StreamLifecycleTimeline

@ontogony/ui/semantic
  DecisionProvenancePanel
  CanonicalFactConflictPanel
  DomainPackLifecyclePanel
  AssistanceDraftOnlyBanner

@ontogony/ui/observability
  CrossServiceIncidentPanel
  MetricStatusCard
  FallbackChainView
```

Acceptance:

```text
- Storybook examples for every evidence component.
- Fixture consumers use each family.
- Frontend can replace local bespoke displays with shared neutral components.
- Components degrade honestly when data is partial.
```

## UI-9-004 — Product-boundary static enforcement

Add a strict source guard:

```text
scripts/check-no-product-meaning.mjs
scripts/check-no-generated-client-imports.mjs
scripts/check-public-subpath-contracts.mjs
```

Forbidden in `src/`:

```text
Conexus provider routing semantics
Kanon canonical truth rules
Allagma run-state authority logic
generated OpenAPI client imports
product API URLs
product auth implementation
domain-specific redaction policy
backend-specific DTO names in public contracts
```

Allowed:

```text
neutral status labels
neutral evidence links
neutral timeline rendering
safe export mechanics
generic redaction helpers
reference adapters clearly marked as non-authoritative
```

Acceptance:

```bash
npm run boundary:check
```

Add this to `npm run check`.

## UI-9-005 — Accessibility and operator-grade interaction hardening

The package already includes accessibility helpers and Playwright/axe/forced-colors coverage.  To get above 9, make accessibility part of component contract quality, not a generic smoke.

For each major family:

```text
layout
navigation
system
semantic
execution
observability
forms
dialogs
data
chat
```

Require:

```text
- keyboard path story;
- loading state;
- error state;
- empty state;
- reduced-motion state where relevant;
- forced-colors screenshot;
- mobile viewport story;
- density variant;
- copy/export affordance where IDs are shown.
```

Acceptance:

```bash
npm run visual:check
npm run test:e2e
npm run check:storybook
```

## UI-9-006 — Versioned design-token and visual governance baseline

UI should own visual mechanics. Add a versioned token baseline:

```text
docs/visual/DESIGN_TOKEN_CONTRACT.md
docs/visual/OPERATOR_VISUAL_LANGUAGE.md
src/theme/tokenManifest.ts
scripts/check-design-token-manifest.mjs
```

Rules:

```text
- no raw ad-hoc colors in components;
- all status colors use semantic tokens;
- all density/spacing primitives use tokenized values;
- charts/evidence/timelines use neutral semantic categories;
- product apps can theme but not fork component internals.
```

Acceptance:

```bash
npm run visual:check
npm run bundle:check
npm run api:check
```

## UI-9-007 — Release candidate package gate

Create:

```bash
npm run rc:check
```

It should run:

```text
- npm run check:full
- packed tarball consumer builds
- storybook static build
- visual/a11y checks
- public API inventory check
- design-token manifest check
- no product-meaning guard
- package export hash generation
```

Produce:

```text
artifacts/ui-rc/<timestamp>/summary.json
artifacts/ui-rc/<timestamp>/package-manifest.json
artifacts/ui-rc/<timestamp>/tarball-sha256.txt
```

The frontend six-repo lock should consume this `tarball-sha256`.

## UI done-definition

`ontogony-ui` is above 9 when:

```text
1. Packed tarball consumer tests pass for Conexus-like, Kanon-like, Allagma-like, and system console fixtures.
2. Neutral contracts are documented and exported.
3. Evidence, trace, semantic, execution, and observability UI kits exist.
4. Product-meaning/static import guards pass.
5. Accessibility/keyboard/mobile/error/empty/loading states are contractually tested.
6. Design-token governance is enforced.
7. RC package gate emits tarball hash and manifest.
8. ontogony-frontend consumes the packed package in release-lock mode.
```

---

# Recommended execution order

Do not run the three plans independently. Use this order:

## Wave 1 — Lock and package foundations

```text
PLAT-9-001  Six-repo compatibility gate
UI-9-001    Packed multi-console consumer gate
FE-9-001    Frontend release-lock integration
```

This creates the reproducibility spine.

## Wave 2 — Contract hardening

```text
PLAT-9-002  Mechanical protocol registry
UI-9-002    Neutral UI contract hardening
FE-9-002    Backend contract parity gate
```

This creates the contract spine.

## Wave 3 — Evidence and operator journeys

```text
UI-9-003    Evidence and trace UI kit
FE-9-003    Operator journey acceptance suite
FE-9-004    Evidence spine as navigation layer
```

This creates the operator-evidence spine.

## Wave 4 — Boundary and correctness enforcement

```text
PLAT-9-006  No-meaning guard                    [platform: Done 2026-05-26]
UI-9-004    No-product-meaning guard
FE-9-005    Live/fixture boundary hardening
```

This prevents architectural drift.

## Wave 5 — Release candidate gates

```text
PLAT-9-003  Consumer conformance suite          [platform: Done 2026-05-26]
PLAT-9-004  Public API hardening                [platform: Done 2026-05-26]
PLAT-9-005  Observability mechanics pack        [platform: Done 2026-05-26]
UI-9-007    UI RC package gate
FE-9-006    Frontend RC gate
FE-9-007    Operator readiness scorecard
```

Platform slices above are complete; remaining wave-5 lift is frontend/UI (and optional PLAT-9-003 artifact runner).

---

# Final target state

After these plans, the system should be assessable like this:

```text
ontogony-platform
  from “tight substrate” 
  to “six-repo mechanical compatibility authority”

ontogony-ui
  from “clean shared UI package”
  to “hard-proven multi-console UI mechanics foundation”

ontogony-frontend
  from “broad live operator console”
  to “release-locked, evidence-backed system operator console”
```

That is the correct way to push all three above 9 without violating the core Ontogony boundary rule: **shared mechanics, not shared meaning.**





After Wave 5 is fully clean, the next move should **not** be more repo-internal tightening. The three repos will already be above 9. The next phase should shift from **repo quality** to **system release posture**.

I would do this in three layers.

---

# First: close Wave 5 completely

Before starting the next phase, finish the three remaining RC blockers:

```text
1. ontogony-ui
   Push/merge UI-9-007 so rc:check is visible on main.

2. ontogony-platform
   Fix validate-package-levels.ps1 / ProjectReference strict-mode so:
   run-consumer-conformance.ps1 -DevRoot C:\dev
   passes end-to-end.

3. ontogony-frontend
   Remove or justify the one readiness gap in /system/release-readiness.
```

Right now, platform has the consumer conformance gate and CI fixture wiring, but your own note says the full runner is blocked by `validate-package-levels.ps1`; that means the full RC chain is not yet green. The runner itself calls public API baseline, package levels, shipping inventory, package smoke, and consumer conformance tests. 

Frontend has the RC gate and readiness scorecard, but the generated scorecard still shows 1 route gap. 

UI’s RC gate was not visible on `main` in my last check, so that must be pushed/merged.

---

# Then: move to Phase 2 — System RC

## Goal

Turn the six repos from:

```text
high-quality integrated alpha repos
```

into:

```text
one reproducible local system release candidate
```

The key artifact should be:

```text
ONTOGONY-SYSTEM-RC-002
```

or similar.

---

# Wave 6 — Six-repo RC promotion

## Objective

Create one promotion workflow that proves all six repos are locked, compatible, built, and operator-usable.

## Deliverables

### 1. System RC lock promotion

Create or promote:

```text
ontogony-platform/docs/system/ontogony-system-rc-002.lock.json
```

It should include:

```text
- all six repo SHAs
- backend OpenAPI hashes
- frontend generated-client hashes
- @ontogony/ui tarball SHA
- @ontogony/agent-interaction package hash
- frontend build provenance hash
- UI RC tarball hash
- frontend RC artifact hash
- platform consumer-conformance artifact hash
```

### 2. System RC promotion script

In `ontogony-platform`:

```powershell
scripts/promote-system-rc.ps1 -DevRoot C:\dev -RcId ONTOGONY-SYSTEM-RC-002
```

It should run:

```text
platform:
  run-consumer-conformance.ps1 -DevRoot C:\dev -ReleaseMode

ui:
  npm run rc:check

frontend:
  npm run rc:check

runtime repos:
  existing system cohesion / Docker-local smoke
```

### 3. One RC evidence folder

Create:

```text
ontogony-platform/docs/evidence/ONTOGONY_SYSTEM_RC_002/
```

With:

```text
system-rc-summary.json
system-rc-summary.md
platform-consumer-conformance-summary.json
ui-rc-summary.json
ui-tarball-sha256.txt
frontend-rc-summary.json
frontend-provenance.json
six-repo-lock.json
post-lock-deltas.json
```

## Acceptance

```text
One command promotes the release candidate.
One evidence folder proves it.
One lock file records it.
```

This is the natural continuation of Wave 5.

---

# Wave 7 — Real local operator system

## Objective

Make the local Docker/dev system behave like a small real product, not just a set of passing checks.

## Deliverables

### 1. Canonical local launch

One command from `ontogony-platform`:

```powershell
scripts/start-local-ontogony-system.ps1
```

Should start:

```text
Conexus
Kanon
Allagma
ontogony-frontend
Postgres / infra dependencies
```

### 2. Canonical local validation

```powershell
scripts/validate-local-ontogony-system.ps1
```

Should prove:

```text
- all health endpoints live
- frontend can reach all services
- operator settings valid
- evidence spine resolves known fixture/live run
- Conexus model call appears
- Kanon decision/provenance appears
- Allagma run/audit appears
- trace/correlation IDs are visible
```

### 3. Operator “golden journey”

Define one canonical journey:

```text
Start Allagma run
→ Kanon plan/decision
→ Conexus model call
→ human gate if needed
→ run audit
→ evidence spine graph
→ replay/export bundle
```

This should become the system’s primary alpha demo.

## Acceptance

```text
A developer can clone all repos, run one command, and see the governed loop in the frontend.
```

---

# Wave 8 — Replace mock confidence with live confidence

Wave 3 and Wave 5 added many mocked/operator journeys. That is good. The next maturity step is to distinguish:

```text
mocked E2E
Docker-local E2E
live-provider optional E2E
```

## Deliverables

### 1. E2E tier matrix

In `ontogony-frontend`:

```text
docs/testing/E2E_TIER_MATRIX.md
```

With:

```text
Tier 1 — fixture/mock, always required
Tier 2 — Docker-local, required for RC
Tier 3 — live provider, manual/secret-gated
```

### 2. Promote key journeys to Docker-local

The operator journey suite already covers system health, Conexus, Kanon, Allagma, human gates, streaming evidence, and six-repo posture. Those should now be split into:

```text
mocked baseline
docker-local acceptance
```

The frontend already has several Docker-local scripts; the next step is to make the RC decide which ones are mandatory.

### 3. Real-provider optional smoke

For Conexus only:

```text
CONEXUS_RUN_LIVE_PROVIDER_SMOKE=true
```

Then prove:

```text
- provider route decision
- model call
- cost/usage telemetry
- no leaked secret
- evidence link visible
```

## Acceptance

```text
The system knows the difference between “UI mocked journey works” and “local real service journey works.”
```

---

# Wave 9 — Operational alpha readiness

After system RC works, start making it operable.

## Deliverables

### 1. Operator runbook

In `ontogony-platform`:

```text
docs/operator/ALPHA_OPERATOR_RUNBOOK.md
```

Cover:

```text
- how to start system
- how to validate health
- how to inspect evidence spine
- how to debug failed Conexus model calls
- how to debug Kanon decision gaps
- how to debug Allagma run stalls
- how to refresh locks
- how to promote RC
```

### 2. Failure-mode catalog

```text
docs/operator/FAILURE_MODE_CATALOG.md
```

Classify:

```text
backend unavailable
auth failure
OpenAPI drift
route inventory drift
UI package hash drift
frontend build provenance drift
Kanon route mismatch
Allagma audit missing
Conexus model-call missing
evidence partial
fixture/live leakage
```

### 3. Troubleshooting links in frontend

The frontend already has readiness and evidence pages. Next, each degraded state should link to a fix path:

```text
Open Settings
Open route readiness
Open evidence spine
Open run audit
Open model-call observability
Open compatibility panel
```

## Acceptance

```text
A failure in the operator console tells the developer what to fix next.
```

---

# Wave 10 — Product surface expansion

Only after Waves 6–9, start expanding functionality.

Good candidates:

## A. Allagma deeper operator execution

```text
- better run resume
- better human-gate resolution
- replay bundle UI
- evaluation/baseline comparison
- audit export
```

## B. Kanon semantic operations

```text
- decision inspector
- canonical fact conflict workflow
- provenance/replay workbench
- domain pack lifecycle
- source binding test UI
```

## C. Conexus gateway operations

```text
- model-call list/detail
- route decision explorer
- provider fallback chain
- streaming transcript evidence
- usage/cost drilldown
```

## D. System evidence spine

```text
- evidence graph visualization
- missing evidence explanation
- direct deep-link resolver
- exported evidence bundle
```

This should be product work, not more infrastructure hardening.

---

# My recommended sequence

Do this:

```text
0. Finish Wave 5 blockers.
1. Wave 6 — Six-repo RC promotion.
2. Wave 7 — Real local operator system.
3. Wave 8 — Docker-local/live confidence tiers.
4. Wave 9 — Operator runbook + failure-mode catalog.
5. Wave 10 — Product-surface expansion.
```

The most important next step is **Wave 6**.

Not another isolated repo plan. Not more component cleanup. The next real milestone should be:

```text
One command.
Six repos.
One lock.
One evidence folder.
One local operator system.
```

That is how Ontogony moves from “very well integrated alpha repos” to a credible **system release candidate**.
