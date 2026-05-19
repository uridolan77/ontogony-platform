# Product feature hardening v1 — known limitations

**Date:** 2026-05-19  
**Scope:** `FE-PRODUCT-CLOSEOUT-001` (package sequence `PFH-000` … `EVAL-PRODUCT-005`)

**This is product hardening on the closed Docker-local stack, not production readiness.**

Canonical matrix: [`08_KNOWN_LIMITATIONS.md`](../product-hardening/eval-alignment-frontend-depth/08_KNOWN_LIMITATIONS.md)

These items are **accepted** for closing product hardening v1. They are not blockers for daily Docker-local operator use unless you are targeting production deploy or compliance archive workflows.

## Program boundary

- Not production readiness.
- Not real provider mode.
- Not cloud deployment.
- Not identity/TLS/secrets/DR.
- Not a replacement for staging or load testing.

## Inherited from Docker-local + post-hardening

Prior programs still apply. See:

- [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md)
- [POST_DOCKER_LOCAL_HARDENING_KNOWN_LIMITATIONS.md](./POST_DOCKER_LOCAL_HARDENING_KNOWN_LIMITATIONS.md)

Including: fake/local Conexus provider by default, development credentials, `VITE_*` compile-time injection, and replay catalog without live trigger when OpenAPI omits the route.

## Product-hardening limitations (intentionally deferred)

| Limitation | Detail |
| --- | --- |
| Dashboard pagination | Default limit 100; cursor via URL; full history UX deferred |
| Global eval filters | `datasetId` / `baselineComparisonId` depend on sparse evaluation metadata |
| Baseline create in UI | POST exists; harness/smoke only — no operator form |
| Scenario datasets | Read-only index/detail; no authoring workflow |
| Judge calibration depth | Metadata surfaced; no calibration history/trend workspace |
| Eval export bundle | Single-eval operator export; locators only; no bulk/compliance archive |
| Semantic diff visualization | Baseline comparison is summary/detail cards; no rich side-by-side diff viewer |
| Replay trigger | No live replay until `POST /allagma/v0/runs/{runId}/replay` in OpenAPI; FE shows limitation banner |
| Replay fixtures | No `?replayFixture=` on `/allagma/replay`; E2E uses mocks |
| Run GET eval ids | Live run GET may omit `evaluationRunIds`; run-scoped eval list + journey links are source of truth |
| Param routes in release catalog | `/allagma/evaluations/:id` not in `release-route-catalog.json` (`ALIGN-PRODUCT-002` follow-up) |
| InMemory persistence | Eval data not durable without Postgres mode |
| Optional ALIGN pr-specs | `ALIGN-PRODUCT-002`–`004` registered but not executed in v1 sequence |

## Accepted stance

Document limitations honestly. Do not mask missing capabilities with fake success states or buttons.

## Safety (non-limitations — do not waive)

| Rule | Status |
| --- | --- |
| Real external execution | Disabled by default |
| Secrets in committed reports or `VITE_*` | Forbidden |
| Production readiness claim | Not made |
| Execute consequential tools before Kanon evaluation | Forbidden (Allagma boundary) |

## Separate from PFH (production-readiness)

- Real Conexus provider keys and cost
- Cloud manifests and runtime Vite injection
- Staging/prod identity and SLOs
- Branch protection requiring aggregate checks (optional manual follow-up from `CI-COST-001`)
