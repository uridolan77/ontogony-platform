# Platform Invariants Checklist

Use this before merging every PR.

## Boundary

- [ ] Does the PR share mechanics only?
- [ ] Does it avoid Athanor canonization/evidence/snapshot/contradiction semantics?
- [ ] Does it avoid Agentor run/orchestration/tool/policy/human-review semantics?
- [ ] Does it avoid Conexus routing/provider strategy?
- [ ] Does it avoid iGaming/business rules?

## Reliability

- [ ] Time-sensitive behavior uses `IClock` where test determinism matters.
- [ ] IDs use platform ID abstractions where appropriate.
- [ ] Retry behavior is idempotency-aware.
- [ ] In-memory/reference implementations are documented as such.
- [ ] Production caveats are explicit.

## Security

- [ ] No secrets in source.
- [ ] No unsafe defaults in production without explicit opt-in.
- [ ] Header-based actor trust is documented as trusted-upstream only.
- [ ] HMAC/body-hash behavior is bounded.
- [ ] Replay protection caveats are explicit.

## Observability

- [ ] Trace ID is canonicalized as `X-Ontogony-Trace-Id` unless explicitly configured.
- [ ] Legacy headers are aliases, not canonical APIs.
- [ ] Metrics names/dimensions are documented when added.
- [ ] Error payloads do not leak unmapped exception messages.

## Documentation

- [ ] README or relevant docs updated.
- [ ] Package semantic docs updated.
- [ ] Changelog updated.
- [ ] Migration note added for behavior/API changes.
- [ ] Examples/adoption snippets added where useful.

## Tests

- [ ] Unit tests added/updated.
- [ ] Integration tests added when behavior crosses package boundaries.
- [ ] Negative tests included for safety behavior.
- [ ] Pack step passes with explicit `PACKAGE_VERSION`.
