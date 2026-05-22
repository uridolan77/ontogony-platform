# Acceptance Checklist

## Runtime lock

- [ ] Lock SHAs match selected repo heads.
- [ ] Package versions match lock.
- [ ] Post-lock deltas are empty or classified.
- [ ] Release-mode lock validation passes.
- [ ] Lock promotion evidence is indexed.

## Full cohesion

- [ ] Completed run scenario passes.
- [ ] Idempotent start scenario passes.
- [ ] Human gate waiting/approved/denied scenarios pass.
- [ ] Kanon-Conexus assistance scenario passes.
- [ ] Conexus fallback scenario passes.
- [ ] Streaming evidence scenario passes.
- [ ] Restart survival is linked or run in same gate.
- [ ] Cohesion summary JSON is indexed.

## Package mode

- [ ] Ontogony packages restored from pack feed.
- [ ] Kanon packages restored from pack feed.
- [ ] Conexus packages restored from pack feed.
- [ ] Allagma builds/tests in package mode.
- [ ] Streaming contract compiles in package mode.
- [ ] Summary JSON is indexed.

## Observability

- [ ] `observability-summary.json` exists.
- [ ] Final verdict is PASS.
- [ ] Allagma metrics/traces PASS.
- [ ] Kanon metrics/traces PASS.
- [ ] Conexus metrics/traces PASS.
- [ ] Cross-service trace chain PASS.
- [ ] Correlation propagation PASS.
- [ ] Grafana/Prometheus/Jaeger status classified.
- [ ] No docs claim PASS without artifact link.

## Evidence spine

- [ ] Golden evidence bundle exists.
- [ ] Golden evidence graph exists.
- [ ] Redaction report exists and passes.
- [ ] Negative missing-evidence cases pass.
- [ ] Model-call evidence bundle links resolve.
- [ ] Usage/cost links resolve.
- [ ] Kanon replay bundle links resolve.
- [ ] Evidence artifacts are indexed.

## Conexus

- [ ] Gateway certification matrix exists.
- [ ] Fallback tests pass.
- [ ] Quota tests pass.
- [ ] Idempotency tests pass.
- [ ] Streaming boundary tests pass.
- [ ] Evidence bundle redaction tests pass.
- [ ] Usage-cost drilldown tests pass.
- [ ] Project/admin isolation tests pass.
- [ ] Certified provider matrix is explicit.

## Kanon

- [ ] Semantic authority certification matrix exists.
- [ ] Compatibility manifest tests pass.
- [ ] Route inventory tests pass.
- [ ] OpenAPI baseline tests pass.
- [ ] v1 graduation guard passes.
- [ ] Negative semantic authority cases pass.
- [ ] Postgres semantic smoke passes.
- [ ] Assistance remains non-authoritative.

## Platform

- [ ] Platform RC contract exists.
- [ ] Protocol registry reflects current runtime lock.
- [ ] Public API checks pass.
- [ ] Package level validation passes.
- [ ] Evidence spine validator passes.
- [ ] Operator failure taxonomy validator passes.
- [ ] No product semantics were added.

## Documentation

- [ ] Stale Conexus evidence-bundle/usage-drilldown comments fixed.
- [ ] Old incoming packages quarantined or marked historical.
- [ ] Admin route prefix wording normalized.
- [ ] Release evidence index points to latest artifacts.
- [ ] Closeout doc added.

## Final sign-off

- [ ] `validate-system-tight-rc-prep.ps1` passes.
- [ ] `validate-system-tight-rc-readiness.ps1` passes.
- [ ] `validate-system-tight-rc-evidence.ps1` passes.


---

## AG-UI / Agent Interaction Spine acceptance

- [ ] `SYSTEM-RC-001F-agent-interaction-agui-spine-certification.md` executed.
- [ ] Platform agent-interaction schema validator passes.
- [ ] Allagma JSONL interaction export passes.
- [ ] Allagma SSE interaction stream passes.
- [ ] SSE resume / `Last-Event-ID` behavior is tested or explicitly recorded as a gap.
- [ ] Conexus model-call interaction projector passes.
- [ ] Conexus model-call events include evidence-bundle/detail/route-decision links.
- [ ] Kanon human-gate and review semantics are representable as interaction interrupts/reviews.
- [ ] Golden AG-UI JSONL artifact exists.
- [ ] Golden SSE transcript artifact exists.
- [ ] Redaction report passes: no raw prompts, completions, tokens, secrets, provider keys, or connection strings.
- [ ] Missing evidence is explicit and structured.
- [ ] AG-UI closeout evidence doc is added and linked from release evidence index.
