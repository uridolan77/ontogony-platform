# Refined backlog

## SYS-OBS-004A — Close B-012 Docker OTLP + Grafana readiness quarantine

**Priority:** P0  
**Repo:** ontogony-platform primary; allagma-dotnet evidence consumer  
**Theme:** Observability / quarantine

### Why

SYSTEM-ALPHA-005 is cut complete except B-012. The failed live observability rerun reported Grafana not ready on :3000 within 120s while collector/Prometheus/Jaeger were up.

### Scope

Debug docker/local-working-system observability startup order, Grafana health/readiness, datasource provisioning, and verify-system-observability.ps1 timeout/readiness logic. Produce a PASS artifact for the live Docker observability gate.

### Acceptance criteria

verify-system-observability.ps1 -UseExistingServices returns PASS against Docker-local stack; artifact is committed or indexed under evidence; B-012 is marked cleared in Allagma and platform evidence indexes; no production readiness claim is added.

### Evidence / source anchors

- `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_005_CLOSEOUT.md`
- `allagma-dotnet/docs/evidence/README.md`


## SYS-LOCK-006 — Refresh runtime lock only after moving-main validation passes

**Priority:** P0  
**Repo:** allagma-dotnet primary; all repos referenced  
**Theme:** Runtime lock / baseline governance

### Why

Current main branches are ahead of the SYSTEM-ALPHA-005 lock: platform +8, Conexus +5, Kanon +3, Allagma +7 at review time. The lock is valid as a cut baseline, but it is no longer a full description of moving-main.

### Scope

Run current-main validation across platform, Conexus, Kanon, Allagma, frontend, and UI. If green, cut SYSTEM-ALPHA-006 or equivalent by updating ontogony-runtime.lock.json, closeout evidence, companion repo SHAs, package versions where changed, and required scenario artifacts.

### Acceptance criteria

validate-runtime-lock.ps1 -RequireEvidence -ReleaseMode PASS for new lock; system cohesion summary PASS; restart summary PASS; observability B-012 either PASS or explicitly quarantined with rationale; package/evidence indexes agree on the baseline label.

### Evidence / source anchors

- `allagma-dotnet/docs/system/ontogony-runtime.lock.json`
- `compare lock commits to main results from GitHub review`


## SYS-PROTOCOL-REGISTRY-001 — Create canonical machine-readable system protocol registry

**Priority:** P0  
**Repo:** ontogony-platform primary; all repos consumed  
**Theme:** Protocol registry

### Why

The system now has matrices, route inventories, evidence ledgers, runtime lock, frontend route catalogs, and generated OpenAPI artifacts, but no single registry binds them together and detects drift.

### Scope

Add a JSON registry linking baseline, repos, commits, API prefixes, route inventories, OpenAPI snapshots, env/auth matrices, error contracts, trace headers, E2E artifacts, frontend route catalogs, and known quarantines.

### Acceptance criteria

Registry validates against schema; each referenced file exists; registry can answer: what route/auth/env/error/evidence artifacts define this protocol surface? CI or script fails on missing evidence, stale baseline labels, obsolete package references, or dead paths.

### Evidence / source anchors

- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `allagma-dotnet/docs/system/SYSTEM_TEST_MATRIX.md`
- `kanon-dotnet/docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`
- `ontogony-frontend/src/app/route-workflow-catalog.json`


## SYS-STALE-PACKAGE-GUARD-001 — Add stale incoming-package detector before adopting generated ZIPs

**Priority:** P0  
**Repo:** ontogony-platform  
**Theme:** Planning hygiene

### Why

The older curated review package was directionally useful but became stale after Sprint 3/4/Alpha-005 closures. The repo now contains the old Ontogony_System_Protocols zip as incoming; adopting it without reconciliation would reintroduce closed items.

### Scope

Add a validator for docs/_incoming packages that detects stale baseline names, closed backlog items, obsolete route names, Agentor references, production-readiness overclaims, and tasks superseded by current evidence.

### Acceptance criteria

Validator reports stale items in old packages; new packages must include CURRENT_BASELINE.md, MOVING_MAIN_DELTA.md, and SUPERSEDED_ITEMS.md; CI/documented runbook prevents copying stale package content into canonical docs without reconciliation.

### Evidence / source anchors

- `ontogony-platform/docs/_incoming/Ontogony_System_Protocols_Package_2026-05-20.zip`
- `ontogony-platform/docs/_incoming/curated-review-package/...`


## SYS-CONNECT-MATRIX-AUDIT-001 — Audit Allagma feature connection matrix against actual route inventories and clients

**Priority:** P0  
**Repo:** allagma-dotnet primary; kanon-dotnet and ontogony-platform referenced  
**Theme:** Route/feature matrix truthfulness

### Why

A new Allagma feature connection matrix exists and is valuable, but it must be proven against Program.cs, Kanon generated route inventory, Conexus route inventory, and frontend catalogs before becoming canonical.

### Scope

Compare every route string and dependency claim in ALLAGMA_FEATURE_CONNECTION_MATRIX.md with actual API route mappings, typed client calls, generated OpenAPI, and frontend hooks. Fix any route-name drift and mark indirect dependencies explicitly.

### Acceptance criteria

A generated test or script fails if a matrix route is absent from the source inventory. Matrix rows distinguish direct HTTP call, typed client abstraction, indirect data dependency, and frontend-only link. Known gaps remain explicit, not implied connected.

### Evidence / source anchors

- `allagma-dotnet/docs/system/ALLAGMA_FEATURE_CONNECTION_MATRIX.md`
- `kanon-dotnet/README.md route list`
- `allagma-dotnet/src/Allagma.Infrastructure/Kanon/KanonSemanticAuthorityClient.cs`


## SYS-E2E-REVALIDATE-006 — Re-run full cohesion suite against current moving-main before next lock

**Priority:** P0  
**Repo:** allagma-dotnet primary  
**Theme:** System E2E

### Why

SYSTEM-ALPHA-005 evidence is strong, but current main includes additional work in platform, Conexus, Kanon, and Allagma after the cut. Those changes need one integrated proof pass.

### Scope

Run local stack, system cohesion smoke with assistance/fallback/streaming evidence, restart survival, package-mode build, persistence smoke, Kanon route parity, frontend live smoke, and evidence spine Docker-live checks.

### Acceptance criteria

A single evidence bundle records PASS/FAIL for each required scenario, with exact repo SHAs and artifact paths. Any failure becomes an explicit quarantine, not hidden under Alpha-005 evidence.

### Evidence / source anchors

- `allagma-dotnet/docs/e2e/SYSTEM_COHESION_SMOKE.md`
- `allagma-dotnet/docs/system/SYSTEM_TEST_MATRIX.md`


## SYS-EVIDENCE-INDEX-006 — Reconcile evidence indexes to Alpha-005 plus moving-main deltas

**Priority:** P0  
**Repo:** ontogony-platform and allagma-dotnet  
**Theme:** Evidence governance

### Why

Evidence indexes should identify the current cut baseline, cleared quarantines, remaining quarantine, post-cut companion commits, and moving-main deltas without ambiguity.

### Scope

Update platform and Allagma evidence indexes so Alpha-005, B-010/B-011/B-013 cleared, B-012 open, and post-cut Kanon Connect 001-007 evidence are visible and not mixed with older Alpha-004 wording.

### Acceptance criteria

A reader can determine from index alone: current cut baseline, current moving-main status, open quarantines, closed quarantines, companion FE/UI commits, and exact commands to validate.

### Evidence / source anchors

- `allagma-dotnet/docs/evidence/README.md`
- `ontogony-platform/docs/evidence/README.md`
- `ontogony-platform/docs/evidence/KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md`


## KANON-CONNECT-LOCK-001 — Promote KANON-CONNECT 001-007 evidence into the next runtime baseline decision

**Priority:** P1  
**Repo:** ontogony-platform primary; kanon-dotnet and frontend referenced  
**Theme:** Kanon connection closeout

### Why

Kanon connection evidence now maps routes, settings, Allagma links, Conexus assistance observability, semantic graph, route parity, and Docker smoke. It should be recognized in the next lock/evidence package, not left as parallel evidence.

### Scope

Collect KANON-CONNECT 001-007 evidence into runtime baseline criteria. Decide which items are baseline blockers, which are companion frontend evidence, and which remain informational.

### Acceptance criteria

Next closeout file includes KANON-CONNECT 001-007 status table, route parity status, Docker cross-service smoke status, and evidence spine semantic graph status.

### Evidence / source anchors

- `ontogony-platform/docs/evidence/KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md`
- `KANON_CONNECT_002..007 evidence files`


## CONEXUS-EVIDENCE-FLOW-001 — Integrate Conexus model-call evidence operator flow into system evidence spine and frontend route catalog

**Priority:** P1  
**Repo:** conexus-dotnet and ontogony-frontend  
**Theme:** Operator evidence

### Why

Conexus now documents a deterministic operator flow from chat completion to route decision, model-call detail, evidence links, bundle, usage/cost, and trace slots.

### Scope

Ensure frontend observability pages and Evidence Spine use the documented Conexus flow. Verify modelCallId, routeDecisionId, traceId, correlationId, and evidence bundle links align in live Docker-local runs.

### Acceptance criteria

Given a modelCallId from Allagma or Kanon assistance, operator UI can resolve route evidence, admin detail, evidence links, evidence bundle, route decision detail, and usage/cost without raw prompt/completion leakage.

### Evidence / source anchors

- `conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`
- `conexus-dotnet/tests/Conexus.Api.Tests/ModelCallEvidenceOperatorFlowTests.cs`


## CONEXUS-POSTLOCK-001 — Classify post-Alpha-005 Conexus route preview, quota status, streaming contract, and evidence-flow additions

**Priority:** P1  
**Repo:** conexus-dotnet  
**Theme:** Post-lock API deltas

### Why

Conexus main has meaningful additions after the Alpha-005 lock: route preview endpoints, quota status contracts, streaming contract docs/tests, model-call evidence operator flow, and store-registration cleanup.

### Scope

Create a short post-lock classification doc: additive safe API, test-only, operator-doc-only, runtime behavior change, lock-impacting package contract, or follow-up needed.

### Acceptance criteria

Each changed file group is classified; lock-impacting changes are either validated into Alpha-006 or left as moving-main deltas. Frontend/API clients know which new endpoints are safe to consume.

### Evidence / source anchors

- `compare Conexus Alpha-005 lock SHA 9847f9... to main`
- `docs/contracts/STREAMING_CHAT_COMPLETION_CONTRACT.md`
- `docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`


## ALLAGMA-RUNTIME-POSTURE-001 — Expose and consume Allagma runtime posture as operator truth source

**Priority:** P1  
**Repo:** allagma-dotnet and ontogony-frontend  
**Theme:** Runtime posture

### Why

Allagma now has runtime posture contracts describing persistence mode, model purposes, tool execution posture, evaluation posture, downstream config, and contract versions. This should become the operator-facing source of what Allagma can do right now.

### Scope

Finalize posture endpoint docs, route matrix entry, frontend consumption, tests, and evidence. Ensure no secrets are exposed and that real external tool execution remains visibly disabled.

### Acceptance criteria

Frontend/system dashboard can show Allagma posture from live API: persistence, model aliases, streaming flags, real execution disabled, sandbox posture, downstream configured flags, and service/package version info.

### Evidence / source anchors

- `allagma-dotnet/src/Allagma.Contracts/AllagmaRuntimePostureContracts.cs`
- `allagma-dotnet/tests/Allagma.Tests/GetAllagmaRuntimePostureServiceTests.cs`


## SYS-REAL-TOOLS-BLOCK-VERIFY-001 — Add explicit recurring verification that real external tool execution is still blocked

**Priority:** P1  
**Repo:** allagma-dotnet and ontogony-platform  
**Theme:** Safety

### Why

The system is growing execution surfaces. Alpha-005 explicitly says real external tool execution remains blocked; future packages should keep proving that invariant.

### Scope

Add or centralize tests/docs verifying RealExecutionEnabled=false by default, kill switch behavior, sandbox/local marker labeling, no provider SDKs in Allagma core, and no real_external execution mode unless SANDBOX-003 is complete.

### Acceptance criteria

Default CI has a named safety test that fails if real execution becomes enabled by default or if docs overclaim real side-effect readiness.

### Evidence / source anchors

- `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_005_CLOSEOUT.md`
- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
