# Non-Goals and Hard Blocks

## Non-goals for SYSTEM-RC-001

This package does not attempt to deliver:

```text
production readiness
enterprise IAM runtime
real external tool execution
full cloud deployment
full provider parity across every LLM vendor
frontend redesign
new product semantics
new ontology v1
new Conexus provider routing model
new Platform product abstractions
```

## Hard blocks

### No real external tool execution

Real external tool execution remains blocked. Work may prepare contracts, ledgers, or validators, but must not enable external side effects.

### No feature PR lock bump

Runtime lock updates must happen only in dedicated lock-promotion PRs.

### No stale PASS claims

No doc may claim PASS unless it links an artifact.

### No hidden product semantics in Platform

Platform must remain mechanics-only.

### No raw prompt/completion/secret leakage in default evidence

Evidence/export/observability must stay redacted by default.

### No Kanon model authority leak

Kanon may use Conexus assistance only as non-authoritative draft/review input.

### No Conexus semantic/orchestration leak

Conexus must not own semantic truth, policy decisions, or governed runtime state.

### No Allagma provider routing leak

Allagma owns model purpose; Conexus owns alias/provider routing.

## If scope pressure appears

Prefer to reduce provider breadth, not reduce certification depth.


---

## AG-UI-specific non-goals and hard blocks

- Do not turn this package into a frontend AG-UI implementation sprint.
- Do not duplicate Evidence Spine as a separate AG-UI evidence graph.
- Do not export raw prompts or raw completions in interaction events.
- Do not expose model provider secrets, project keys, service tokens, admin keys, or connection strings.
- Do not change Kanon or Conexus public error contracts for AG-UI convenience.
- Do not add real external tool execution to make AG-UI demos more impressive.
- Do not claim AG-UI certification unless JSONL export, SSE stream, schema validation, redaction, and evidence links are all proven.
