# PR Spec — AGM-TOPO-003 — Kanon Topology Authorization Client

## Repo

`allagma-dotnet`

## Depends on

`KANON-TOPO-001`

## Goal

Call Kanon topology-policy evaluation for high-risk classifications or non-default topology overrides.

## Behavior

Allagma must request topology authorization when:

```text
classification includes high_risk
classification includes tool_heavy
classification includes human_gate_likely
requested topology != single_workflow
actor lacks explicit low-risk/system exemption
```

## Outcomes

| Kanon result | Allagma behavior |
|---|---|
| `allow` | proceed |
| `deny` | fail closed |
| `human_gate` | pause before execution |
| transport failure | fail closed for high-risk, retryable error for low-risk override |

## Tests

- high-risk run calls Kanon topology endpoint.
- low-risk default run does not call endpoint.
- deny fails before model call.
- human gate pauses before model call.
- decision id stored and visible in audit bundle.

## Acceptance

- no bypass for high-risk topology choices.
- cross-service trace headers propagated.
