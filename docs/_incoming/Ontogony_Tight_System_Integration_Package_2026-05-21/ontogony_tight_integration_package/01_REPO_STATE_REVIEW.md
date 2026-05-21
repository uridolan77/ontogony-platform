# 01 — Repo state review

## 1. allagma-dotnet

### Current strengths

- Owns the system compatibility matrix and machine-readable runtime lock.
- Coordinates Kanon planning/policy and Conexus model completion.
- Supports model purpose → Conexus alias mapping.
- Has opt-in Conexus SSE streaming per model purpose.
- Records run/event lifecycle, audit bundles, operations, evaluations, retry/cancel/replay endpoints.
- Has Postgres persistence and restart evidence.
- Has feature connection matrix mapping Allagma endpoints to Kanon calls, Conexus calls, indirect data, frontend links, and known gaps.
- Owns current `SYSTEM-ALPHA-006` closeout evidence.

### Current risks

- The feature matrix is good but not yet the universal operator contract.
- Streaming is shipped but canonical cohesion remains non-streaming by default.
- Real tool execution is intentionally blocked; documentation exists, but any implementation pressure must be resisted until safety gates graduate.
- Error contracts remain service-specific, although Allagma has cross-service error envelope concepts.
- Moving-main vs locked-commit policy is present but should become the central release discipline.

### Role in this package

Allagma owns the implementation sequence and receives most of the PRs because tight integration is runtime integration.

## 2. kanon-dotnet

### Current strengths

- v0 frozen for integrators.
- 61 `/ontology/v0` routes, 53 client routes, 8 server-only routes.
- Compatibility manifest v1.1 with OpenAPI baseline SHA.
- Contract freeze doc clearly defines frozen surfaces and breaking-change policy.
- Evidence-spine operator handoff maps decisions, provenance, replay bundles, semantic graph, domain-pack evolution, assistance review, topology decisions, human gates, trace/entity discovery.
- Postgres semantic smoke covers recent semantic authority features after data-source recreation.

### Current risks

- v0 still keeps documented non-ApiError DTO-shaped 400 response families.
- Assistance accept/reject remains HTTP-first server-only.
- Replay execution is not present and should not be smuggled into Kanon.
- Full production auth is not done.

### Role in this package

Kanon should be consumed, not expanded. Only add compatibility/evidence metadata if downstream operator tooling requires it and the change is additive.

## 3. conexus-dotnet

### Current strengths

- Owns model aliases, provider routing, fallback, project keys, quotas, usage/cost, route decisions, and model-call evidence.
- Supports JSON and SSE streaming for OpenAI-compatible chat completions.
- Has durable Postgres mode for project config, aliases, pricing, overrides, replay, telemetry, journals, quota ledger, artifacts, and protocol-neutral idempotency ledger.
- Provides route-preview and quota status endpoints as additive safe APIs.
- Provides model-call evidence-links and evidence-bundle routes.
- Has fake provider system E2E models for Allagma cohesion smoke.

### Current risks

- Route-preview and quota status are not yet fully surfaced in frontend/operator flows.
- Admin/operator contracts remain alpha; enterprise IAM not present.
- Streaming semantics are available, but canonical system evidence should be tightened.
- Full system-wide idempotency classification remains pending even though Conexus durable ledger improved.

### Role in this package

Conexus should expose and document evidence and routing surfaces for the operator; avoid changing provider-routing ownership.

## 4. ontogony-platform

### Current strengths

- Owns reusable mechanics: errors, hosting, tracing, idempotency primitives, HTTP resilience, telemetry DTOs, evidence registry concepts.
- Holds system-level evidence indices and KANON-CONNECT evidence.
- Recently gained protocol/registry validation work.

### Current risks

- The system protocol registry should become a first-class release artifact, not just scattered evidence docs.
- Cross-service error envelope primitives need clearer adoption gates across Allagma/Kanon/Conexus.
- Operator v1 lock and post-lock delta registers need to be aligned with runtime lock discipline.

### Role in this package

Platform owns shared schema, registry, evidence conventions, and CI validators. It should not own product semantics.

## 5. operator/frontend companion layer

### Current strengths

- Evidence exists for route parity, Docker cross-service smoke, evidence-spine semantic graph, and Conexus evidence flow.
- Frontend route parity is already a baseline blocker for Kanon connect evidence.

### Current risks

- Operator journeys are still spread across feature pages rather than a unified audit/evidence flow.
- Some newly safe Conexus endpoints are not wired.
- Streaming and model-purpose evidence visualization is deferred.

### Role in this package

Frontend/operator work should consume existing backend evidence surfaces before requesting new routes.
