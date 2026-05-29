# Test Layers and Ownership

## Layer 0 — Static architecture checks

Purpose: prevent boundary drift before runtime.

Examples:

- Allagma core must not reference MAF assemblies except via isolated adapter project.
- Kanon must not reference Conexus provider internals.
- Conexus must not reference Kanon semantics.
- Frontend must depend on generated/shared clients where possible instead of duplicate ad hoc payloads.

Owner: each repo, with system harness collecting results.

## Layer 1 — Unit/domain tests

Purpose: local deterministic correctness.

Examples:

- Kanon policy evaluator.
- Conexus alias routing and fallback selection.
- Allagma run state machine.
- Metabole transformation/evolution rules.
- Aisthesis memory trace scoring/retrieval.
- Platform hashing/idempotency/error helpers.

Owner: each service repo.

## Layer 2 — Service integration tests

Purpose: real HTTP route behavior against local service + persistence.

Examples:

- Protected routes reject missing tokens.
- Route returns stable error envelope.
- API writes create durable records.
- Read APIs return expected state.
- Idempotency keys replay correctly.

Owner: service repo + system harness.

## Layer 3 — Contract tests

Purpose: prevent drift between service APIs, typed clients, UI clients, and orchestration clients.

Examples:

- OpenAPI route exists.
- Request/response schema compatibility.
- Error code taxonomy compatibility.
- Required headers documented and enforced.
- Deprecated route tests remain until removal window closes.

Owner: system harness.

## Layer 4 — Cross-service E2E

Purpose: prove the Ontogony runtime works as a system.

Minimum flows:

1. Allagma run completes through Kanon planning and Conexus model completion.
2. Allagma pauses/resumes through Kanon human gate.
3. Kanon calls Conexus assistance with redaction and `draft_only` output.
4. Conexus fallback is visible through an Allagma-initiated run.
5. Idempotent retry does not duplicate runs, model calls, decisions, or side effects.
6. Restart does not corrupt run/event state.
7. Trace/correlation IDs link the chain.

Owner: system harness, likely hosted under Allagma or a separate test repo.

## Layer 5 — UI E2E

Purpose: prove the console covers backend capabilities and does not silently regress workflows.

Examples:

- Service navigation pages render.
- Data tables load.
- Actions call the correct backend route.
- Error states render with actionable messages.
- Human gate flows are visible and operable.

Owner: frontend repo + system harness.

## Layer 6 — Load, soak, and resilience

Purpose: catch operational degradation early.

Examples:

- Gateway fallback behavior under provider failure.
- Run creation under repeated retries.
- Long-lived event streams.
- Postgres connection interruption recovery.
- Readiness flips false when critical dependency is down.

Owner: system harness.
