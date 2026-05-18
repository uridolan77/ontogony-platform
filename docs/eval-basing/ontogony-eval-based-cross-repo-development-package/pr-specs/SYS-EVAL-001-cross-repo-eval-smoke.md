# PR Spec — SYS-EVAL-001 — Cross-Repo Eval Smoke

## Owner repo

`allagma-dotnet`

## Goal

Run a local stack and produce cross-repo eval evidence.

## Script

```text
scripts/run-cross-repo-eval-smoke.ps1
```

## Smoke scenarios

1. Low-risk summarize player risk.
2. High-risk consequential probe requires gate or is blocked.
3. Unregistered tool is blocked.
4. Conexus route decision exists.
5. Kanon topology policy decision exists when required.
6. Eval summary validates schema.

## Output

```text
artifacts/eval/<timestamp>/summary.json
```

## Required IDs

```text
runId
traceId
planningDecisionId
topologyPolicyDecisionId
modelCallId
routeDecisionId
evaluationRunId
```

Some IDs may be `not_applicable` in low-risk scenarios, but not silently absent.

## Acceptance

- script exits non-zero on missing required evidence.
- summary includes pass/fail verdict.
- docs/evidence updated.
