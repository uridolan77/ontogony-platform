# Aisthesis ReleaseMode runbook

## Goal

Validate Aisthesis under deployment-like assumptions.

## Steps

1. Stop all local debug API processes.
2. Configure Postgres or explicitly record in-memory waiver.
3. Configure producer-token auth.
4. Start Aisthesis in Release.
5. Run readiness checks.
6. Run fixture smoke with token headers.
7. Run live smoke if producer services are available.
8. Record redacted artifact.

## Required decision

At the end, classify ReleaseMode as:

```text
PASS
PARTIAL
FAIL
NOT_RUN
```

Explain all PARTIAL/NOT_RUN results.
