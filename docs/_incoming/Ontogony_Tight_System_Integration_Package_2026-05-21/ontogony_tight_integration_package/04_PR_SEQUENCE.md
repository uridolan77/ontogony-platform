# 04 — Proposed PR sequence

## Sprint: SYS-TIGHT-001 — Ontogony Tight Integration Baseline

### Guiding rule

Prefer integration glue, manifests, evidence, test gates, operator journeys, and contract adapters. Avoid expanding backend product capability unless a gate requires it.

---

## SYS-TIGHT-001 — Runtime lock + delta register hardening

**Owner:** allagma-dotnet + ontogony-platform  
**Type:** docs + validators + CI  
**Goal:** Make `SYSTEM-ALPHA-006` style runtime lock discipline repeatable and hard to misread.

### Scope

- Add `docs/system/POST_LOCK_DELTA_REGISTER.md` in Allagma or Platform as canonical for moving-main deltas after a lock cut.
- Add machine JSON: `docs/system/post-lock-deltas.json`.
- Require every post-lock repo delta to be classified: `api`, `runtime`, `docs-only`, `test-only`, `package-impacting`, `operator-impacting`, `security-impacting`.
- Extend lock validator to fail release mode if unclassified deltas exist.

### Acceptance

- `validate-runtime-lock.ps1 -ReleaseMode -RequireEvidence` checks delta register.
- Delta register names all four locked repos.
- Docs distinguish expected refs from pinned commits.
- No runtime code changes.

---

## SYS-TIGHT-002 — Unified cross-service evidence spine resolver contract

**Owner:** ontogony-platform + ontogony-frontend + allagma-dotnet  
**Type:** contract + frontend adapter + tests  
**Goal:** One resolver contract for runId, decisionId, modelCallId, routeDecisionId, traceId, correlationId, humanGateId, domainPackId.

### Scope

- Define `SYSTEM_EVIDENCE_SPINE_CONTRACT.md` in Platform.
- Create or update machine index mapping identifiers to service routes.
- Frontend resolver consumes:
  - Allagma run events/audit,
  - Kanon evidence-spine entrypoints,
  - Conexus model-call evidence routes,
  - trace/correlation discovery.
- Add adapter tests for each identifier kind.

### Acceptance

- Given an Allagma runId, resolver returns graph nodes for Allagma, Kanon, and Conexus where ids exist.
- Given a Kanon decisionId, resolver returns decision, provenance, semantic graph links.
- Given a Conexus modelCallId, resolver returns model-call detail/evidence links/evidence bundle links.
- Missing downstream data is represented as unresolved edge, not a crash.

---

## SYS-TIGHT-003 — Operator audit journey v1

**Owner:** ontogony-frontend + ontogony-ui  
**Type:** UI/operator workflow  
**Goal:** Provide one operator journey for a governed run.

### Scope

- Add an audit page/flow centered on `runId` and `traceId`.
- Display:
  - run state and timeline,
  - Kanon decisions/provenance/human gates,
  - Conexus model-call/route/provider attempts/quota/cost,
  - streaming lifecycle events,
  - retry/cancel/replay availability,
  - unresolved/missing evidence edges.
- Use existing backend routes before requesting new backend endpoints.

### Acceptance

- Playwright Docker-local smoke covers completed run, human gate, Conexus fallback, Kanon assistance, evidence spine, and stream lifecycle metadata.
- UI clearly marks `draft_only` model assistance.
- UI clearly distinguishes semantic authority from model/provider evidence.

---

## SYS-TIGHT-004 — Conexus route-preview and quota operator consumption

**Owner:** conexus-dotnet + ontogony-frontend  
**Type:** frontend + optional client docs/tests  
**Goal:** Wire already-safe Conexus endpoints into operator workflows.

### Scope

- Add UI panels for:
  - `POST /v1/models/{modelAlias}/route-preview`,
  - `GET /v1/governance/quota`.
- Add frontend client/hook tests.
- Add operator docs cross-linking route-preview output to route decisions and model-call evidence.

### Acceptance

- Operator can preview route for `risk-summary-v0` and `risk-summary-stream-v0` without provider call.
- Operator can see quota status before/after a cohesion smoke run.
- Evidence spine can link route-preview context when present.

---

## SYS-TIGHT-005 — Streaming evidence integration

**Owner:** allagma-dotnet + conexus-dotnet + frontend  
**Type:** integration tests + operator UI  
**Goal:** Make opt-in streaming understandable and auditable without storing streamed content by default.

### Scope

- Confirm Conexus streaming contract and Allagma `StreamAsync` compile-time contract.
- Extend audit bundle with stream lifecycle summary if not already present.
- Frontend stream panel displays started/chunk count/completed/interrupted/metadata.
- Keep `PersistStreamedOutput=false` default.

### Acceptance

- Smoke produces stream lifecycle events.
- Operator page shows chunk count and completion state.
- Idempotency-key rejection for streaming remains documented.
- No streamed payload stored unless explicitly configured.

---

## SYS-TIGHT-006 — Cross-service failure taxonomy adapter

**Owner:** ontogony-platform + allagma-dotnet  
**Type:** shared contract + adapter tests  
**Goal:** Normalize operator-facing failures without breaking public service-specific error contracts.

### Scope

- Keep Conexus OpenAI-compatible errors, Kanon v0 documented DTO exceptions, and Allagma CrossServiceErrorEnvelope public shape.
- Add adapter-level taxonomy:
  - `auth_failed`, `forbidden`, `validation_failed`, `not_found`, `conflict`, `idempotency_conflict`, `downstream_unavailable`, `provider_failed_retryable`, `provider_failed_terminal`, `quota_exceeded`, `timeout`, `unknown`.
- Implement adapter tests mapping representative service errors.

### Acceptance

- Allagma can expose normalized failure stage, retryability, downstream service, trace/correlation ids.
- Frontend shows consistent failure banners and recommended operator actions.
- No breaking public contract changes in Kanon/Conexus.

---

## SYS-TIGHT-007 — Runtime package-mode and sibling-source parity gate

**Owner:** allagma-dotnet  
**Type:** CI/release gate  
**Goal:** Ensure package mode and sibling-source mode agree before lock promotion.

### Scope

- Release-mode CI runs:
  - sibling-source cohesion smoke,
  - full package-mode build/test,
  - runtime lock validation,
  - Kanon manifest conformance,
  - Conexus client contract compatibility,
  - frontend route parity where companion pins changed.

### Acceptance

- A release cannot be cut if package versions mismatch runtime lock.
- Allagma package mode fails if Conexus streaming contract or Kanon client contract is incompatible.
- Evidence artifacts are copied/indexed for release closeout.

---

## SYS-TIGHT-008 — Production-readiness separation package

**Owner:** all repos, led by Platform  
**Type:** planning only  
**Goal:** Prevent alpha integration from being confused with production readiness.

### Scope

- Create production-readiness backlog split into:
  - identity/auth,
  - secrets/rotation,
  - durable artifacts,
  - managed observability,
  - data retention,
  - real tool execution safety,
  - deployment hardening.

### Acceptance

- All alpha closeouts link to production-readiness non-claims.
- Real tool execution remains blocked.
- Enterprise IAM is not implied by service-token/project-key alpha flows.
