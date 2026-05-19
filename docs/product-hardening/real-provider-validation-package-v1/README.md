# Ontogony Real Provider Validation Package v1

This package starts the next phase after `PRODUCT-MANUAL-QA-002R1` passed on the Docker-local fake-provider system.

## Goal

Validate a small number of real LLM provider calls through Conexus in a controlled, local, budgeted, redacted flow, while preserving fake provider mode as the default.

## Hard boundary

```text
Fake provider remains default.
Real provider is explicit opt-in.
Real provider is local/manual only.
No secrets in git.
No real provider calls in CI.
No production readiness claim.
```

## Sequence

1. `RP-000` — package setup
2. `RP-001` — secret, budget, and safety gates
3. `RP-002` — Conexus real-provider local mode
4. `RP-003` — Allagma real-provider guided flow
5. `RP-004` — frontend operator visibility
6. `RP-005` — real-provider manual QA execution
7. `RP-CLOSEOUT-001` — validation closeout
