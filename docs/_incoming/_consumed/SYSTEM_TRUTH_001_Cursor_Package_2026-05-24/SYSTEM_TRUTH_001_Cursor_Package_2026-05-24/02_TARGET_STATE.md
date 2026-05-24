# 02 — Target State

## Operator Home target wording

Home should display a structured truth model:

```text
System baseline
SYSTEM-ALPHA-006 · locked · owner: allagma-dotnet

Local stack status
Conexus: live · not ready · health contract valid
Kanon: live · ready · health contract valid
Allagma: live · ready · health contract valid

Compatibility
Overall: warning
Reason: Conexus readiness not ready
Service versions: verified / mismatch / unavailable
Compatibility artifact: present / missing / stale
Data source: live + generated summary

Execution safety
Real external: blocked
Local sandbox: disabled
Default execution: symbolic
Kill switch: not configured · local-alpha warning
```

## Truth dimensions

Use separate dimensions everywhere:

| Dimension | Values | Meaning |
|---|---|---|
| Connectivity | `live`, `degraded`, `offline`, `unknown` | Can the console reach the service? |
| Readiness | `ready`, `not_ready`, `unknown`, `not_applicable` | Can the service perform its expected local-alpha role? |
| Contract health | `valid`, `warning`, `invalid`, `unknown` | Did the payload match the expected schema? |
| Compatibility | `passed`, `warning`, `failed`, `unknown` | Does runtime state match the compatibility manifest? |
| Data source | `live`, `live_with_fallback`, `generated`, `fixture`, `imported`, `unknown` | Where did displayed values come from? |
| Authority | `authoritative`, `advisory`, `demo`, `diagnostic` | How much should the operator trust this data? |
| Release posture | `not_assessed`, `blocked`, `warning`, `candidate`, `passed` | Release-readiness, not ordinary runtime readiness. |

## Required outcome

The console may still show warnings after this workstream. That is acceptable.

It must not show contradictory or overconfident claims.
