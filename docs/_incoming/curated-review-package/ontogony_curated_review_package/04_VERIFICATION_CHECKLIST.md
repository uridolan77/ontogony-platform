# Verification checklist

## Before implementation

- Confirm all four repos are on the expected branch/ref.
- Confirm no stale review-only claim is being used as an implementation requirement.
- Confirm package names and route prefixes in the system matrix.
- Confirm local ports: Kanon 5081, Conexus 5082, Allagma 5083, frontend as configured.

## Per-service checks

### Conexus

- `/health` and `/ready` return expected state.
- `/v1/chat/completions` works non-streaming through fake/local provider.
- Streaming works in its own smoke test.
- Admin model-call, route-decision, governance usage, diagnostics, retention, and evidence routes are auth-protected.
- Tool/function pass-through tests exist once `CONEXUS-TOOLS-001` lands.

### Kanon

- `/health` and `/ready` return expected state.
- Postgres mode runs migrations.
- Ontology lifecycle, source binding, semantic plan, canonical fact, action policy, human gate, decision/provenance/replay, and domain-pack flows persist across restart.
- ServiceToken mode works in local stack.
- Conexus assistance disabled/mock/local behavior is deterministic.

### Allagma

- `/allagma/v0/runs` starts a governed run.
- `/allagma/v0/runs/{runId}` and `/events` return run state and event timeline.
- `/allagma/v0/runs/{runId}/resume` handles waiting, not-waiting, approved, denied, and Kanon unavailable outcomes.
- `/capabilities` exposes execution mode and supported runtime features.
- Real external tools remain unavailable until the trust model and tests are complete.

### Frontend

- Every route declares data source status: live, live_with_fallback, fixture_only, or not_implemented.
- Route-level auth guard is present.
- Error boundaries render safe per-page failures.
- Docker-local env maps all service URLs.
- High-value pages have render/loading/error/success tests.

## Full-stack acceptance

- One command runs frontend + Kanon + Conexus + Allagma + Postgres.
- Completed run records Allagma event timeline, Kanon planning/action/human-gate decisions, and Conexus model call.
- Retry with same idempotency key does not duplicate side effects.
- Human gate approve and deny paths are visible.
- Conexus fallback path is exercised from an Allagma-initiated run.
- Restart E2E preserves run/event/provenance continuity.
- Trace/correlation IDs join Allagma, Kanon, and Conexus records.
