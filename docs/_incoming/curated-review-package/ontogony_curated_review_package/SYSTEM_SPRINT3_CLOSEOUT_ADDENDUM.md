# System cohesion — Sprint 3 closeout addendum (items 17–24)

**Date:** 2026-05-20  
**Review method:** GitHub code/docs/test inspection (not local build/run)  
**Sprint plan:** [`02_SPRINT_PLAN.md`](02_SPRINT_PLAN.md) · acceptance: [`03_ACCEPTANCE_MATRIX.md`](03_ACCEPTANCE_MATRIX.md)

## Summary

Sprint 3 operational maturity items **17–24** are **substantially complete**. Do not reopen Sprint 3 wholesale; track maturity gaps as the hardening batch in [`02_SPRINT_PLAN.md`](02_SPRINT_PLAN.md) (Sprint 3.5).

```text
Sprint 3 — closed with precision caveats:
- SYSTEM-DASH-001 — PASS / alpha-ready starter pack (not production SLO governance)
- CONEXUS-IDEMP-001 — PASS; durable implicit ledger + client replay; not cross-request retry dedupe without Idempotency-Key
- CONEXUS-STREAM-COST-001 — PASS
- CONEXUS-ADMIN-SAFETY-001 — PASS
- KANON-AUTH-LOCAL-001 — PASS; landed as KANON-AUTH-001/001A (naming drift)
- KANON-API-MODULAR-001 — PASS / shallow endpoint extraction (not full module framework)
- FE-DOCKER-001 — PASS
- FE-TEST-001 — PASS (mocked Playwright; not live-backend operator tests)
```

## Item status

| # | Id | Verdict | Evidence / notes |
| --- | --- | --- | --- |
| 17 | `SYSTEM-DASH-001` | **PASS / alpha-ready** | `allagma-dotnet/docs/operations/SYSTEM_SLO_STARTER_PACK.md`, `SYSTEM_DASH_PANEL_PACK.md`, `dashboards/grafana/ontogony-alpha-runtime.json`, `docker-compose.observability.yml`, `scripts/verify-system-observability.ps1`, `docs/evidence/SYSTEM_DASH_001_EVIDENCE.md` |
| 18 | `CONEXUS-IDEMP-001` | **PASS** (semantic caveat) | `conexus-dotnet` `EfIdempotencyLedger`, `ChatCompletionIdempotencyCoordinator`, `docs/architecture/IDEMPOTENCY_MODEL.md` — implicit key `request:{requestId}:{payloadFingerprint}` does not dedupe separate client retries without `Idempotency-Key` |
| 19 | `CONEXUS-STREAM-COST-001` | **PASS** | `conexus-dotnet/docs/evidence/CONEXUS_STREAM_COST_001_EVIDENCE.md` |
| 20 | `CONEXUS-ADMIN-SAFETY-001` | **PASS** | `conexus-dotnet/docs/security/ADMIN_ENDPOINT_EXPOSURE.md`, `AdminEndpointExposureIntegrationTests.cs` |
| 21 | `KANON-AUTH-LOCAL-001` | **PASS** (maps to `KANON-AUTH-001/001A`) | `kanon-dotnet` `KanonApiAuthOptions`, `KanonServiceTokenAuthenticationHandler`, `KanonApiAuthTests` |
| 22 | `KANON-API-MODULAR-001` | **PASS / shallow** | `OntologyV0Endpoints.cs` + feature endpoint modules; repeated try/catch mapping remains |
| 23 | `FE-DOCKER-001` | **PASS** | `ontogony-frontend/docs/evidence/FE_DOCKER_001_DOCKER_LOCAL_FRONTEND_COMPOSITION_EVIDENCE.md` |
| 24 | `FE-TEST-001` | **PASS** | `ontogony-frontend/docs/evidence/FE_TEST_001_HIGH_VALUE_PAGE_SMOKE_TESTS_EVIDENCE.md` |

## Status precision (do not over-claim)

| Id | Correct label | Not |
| --- | --- | --- |
| `SYSTEM-DASH-001` | Alpha starter pack / operator discussion targets | Contractual production SLO program |
| `CONEXUS-IDEMP-001` | Durable neutral ledger for implicit request lifecycle + client replay when `Idempotency-Key` set | Automatic cross-request retry idempotency without client key |
| `KANON-AUTH-LOCAL-001` | `KANON-AUTH-001/001A` service-token local mode | Final production identity (JWT/mTLS) |
| `KANON-API-MODULAR-001` | Endpoint grouping via named modules | Full endpoint-module framework with shared filters and generated inventory |

## Recommended hardening batch (Sprint 3.5)

| Id | Repo | Title |
| --- | --- | --- |
| `SYSTEM-DASH-002` | ontogony-platform (+ refs) | Centralize operational dashboard/SLO entrypoint — **closed** |
| `SYSTEM-OBS-METERS-001` | multi | `/ready` SLI, Kanon plan-compile meters, Conexus cost OTEL |
| `CONEXUS-IDEMP-DOC-001` | conexus-dotnet | Clarify implicit idempotency semantics in status docs |
| `CONEXUS-ADMIN-ROUTE-INVENTORY-001` | conexus-dotnet | Route-inventory test for `/admin/**` classification drift |
| `KANON-AUTH-NAMING-001` | kanon-dotnet | Reconcile `KANON-AUTH-LOCAL-001` ↔ `KANON-AUTH-001/001A` labels |
| `KANON-ENDPOINT-FILTERS-001` | kanon-dotnet | Shared endpoint filters + route-module inventory tests |
| `FE-LIVE-SMOKE-001` | ontogony-frontend | One safe Docker-live Playwright smoke (read-only routes) |
