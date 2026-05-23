# 12 — Non-goals and Risks

## Non-goals

This package does not:

- enable real external tool execution;
- enable OpenAI or any paid provider;
- solve production IAM/security;
- complete all Kanon semantic graph edges;
- implement all domain-pack switching;
- make release readiness green;
- replace fixture replay entirely;
- make historical broken runs retroactively complete.

## Risks

### Risk: Scope creep into all Evidence Spine gaps

Mitigation: prove one live governed fake path first.

### Risk: Treating missing data as resolved

Mitigation: require source attempts and reason codes.

### Risk: Changing domain semantics

Mitigation: use existing `gaming-core@0.1.0` and `summarize-player-risk`.

### Risk: Breaking direct Conexus chat

Mitigation: add direct Conexus regression test and mark Kanon decision not applicable.

### Risk: Store mismatch between EF and in-memory paths

Mitigation: route-decision tests must run against both relevant persistence modes where feasible.

### Risk: Fixture replay still influences readiness

Mitigation: ensure live proof and fixture proof are visually and programmatically separated.
