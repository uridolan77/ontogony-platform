# TASK-002 — Backend Ready V1

## Goal

Make `/ready` return `ready.v1` with detailed checks.

## Steps

1. Implement `ReadyV1Response` and `ReadinessCheck`.
2. Add service-specific readiness contributors.
3. Add stable check IDs.
4. Classify checks as required/optional/informational.
5. Add tests for both ready and not-ready/degraded scenarios.

## Acceptance

- `/ready` explains not-ready state.
- Conexus strict-not-ready no longer appears without reason.
