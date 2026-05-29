# ONTOGONY-SYSTEM-TEST-HARNESS-001 — evidence

**Date:** 2026-05-30  
**Status:** promoted to standalone repo

## Outcome

First implementation pass completed; harness promoted from `ontogony-platform/docs/_incoming` to:

`C:\dev\ontogony-system-test-harness` (git init; CI validates build on PR, optional live E2E on `workflow_dispatch`).

## Delivered

- Calibrated `manifests/routes.yml` against Kanon/Allagma/Conexus source (2026-05-30)
- .NET `Ontogony.SystemTests` — compiles; E2E-001 through E2E-005, E2E-010, readiness
- Evidence bundles per E2E (`ontogony-system-test-harness-evidence-v1`)
- [Manual regression retirement report](https://github.com/uridolan77/ontogony-system-test-harness/blob/main/docs/generated/MANUAL_REGRESSION_RETIREMENT_REPORT.md) (local copy under harness `docs/generated/`)

## CI

| Repo | Workflow | PR | Live E2E |
|---|---|---|---|
| `ontogony-system-test-harness` | `ontogony-system-test-harness.yml` | build + verify-package | `workflow_dispatch` + `run_live_stack_tests` |

## Canonical doc

[`docs/packages/ONTOGONY_SYSTEM_TEST_HARNESS.md`](../packages/ONTOGONY_SYSTEM_TEST_HARNESS.md)

## Not in scope (v1)

- Harness does not start the five-service stack
- UI Playwright journeys, restart survival, Metabole/Aisthesis functional E2E (readiness only)
- External LLM / real tool execution (explicitly off)
