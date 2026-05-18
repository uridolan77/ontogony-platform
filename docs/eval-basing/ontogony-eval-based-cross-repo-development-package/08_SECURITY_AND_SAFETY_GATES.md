# 08 — Security and Safety Gates

## Non-negotiable safety rules

1. Real external tool execution remains blocked until a dedicated enablement series.
2. Eval harness must not store raw secrets.
3. Eval metrics must not put raw prompts/responses/tool args in labels.
4. High-risk topology choices require Kanon authorization.
5. Human-gate-required decisions must fail closed if gate creation fails.
6. Baseline comparison must not double-execute side effects.
7. Replay must never re-execute completed side-effect scopes.
8. Route decisions must not leak provider secrets.
9. LLM-as-judge output is advisory unless calibrated against deterministic checks.
10. Production config must reject placeholder tokens.

## Baseline safety

Baseline runs must be one of:

```text
simulation-only
model-only
dry-run
read-only
```

Never run two real side-effecting paths just to compare topologies.

## Eval data privacy

Allowed in eval artifacts:

```text
ids
hashes
status codes
decision ids
scores
bounded classifications
cost/latency
redacted snippets only when explicitly enabled
```

Forbidden by default:

```text
raw prompts
raw model responses
secrets
API keys
full tool arguments containing PII
provider raw payloads
unredacted human notes
```

## High-risk topology policy

For `high_risk`, `tool_heavy`, or `human_gate_likely` classifications:

```text
1. Allagma must request Kanon topology authorization.
2. Kanon may require centralized validation.
3. Kanon may require human gate before model/tool continuation.
4. Allagma must record the topology authorization decision.
5. Eval score cannot pass if authorization is missing.
```
