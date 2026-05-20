# Recommended PR sequence

## Sprint 0 — Coordination spine

1. `SYSTEM-COH-001` — compatibility/environment/auth/route/test matrices.
2. `SYSTEM-E2E-001` — full local-stack E2E harness.
3. `SYSTEM-CTX-001` — trace/correlation/actor/idempotency propagation contract.
4. `SYSTEM-ERR-001` — shared cross-service error envelope.

## Sprint 1 — Service hardening

**Status (2026-05-20):** Items 5–10 closed in source. Reconciliation addendum: `allagma-dotnet/docs/reviews/SYSTEM_COHESION_SPRINT1_CLOSEOUT_ADDENDUM.md`.

5. `KANON-POSTGRES-LOCAL-001` — prove durable Kanon local path. **Closed**
6. `KANON-EVIDENCE-001` — evidence packs for major semantic workflows. **Closed**
7. `CONEXUS-TOOLS-001` — tool/function pass-through. **Closed** (PASS; OpenAI-shaped + fake)
8. `CONEXUS-GOV-DRILLDOWN-001` — model-call/usage drill-down contracts. **Closed** (PASS source + Docker-local admin list/detail after rebuild)
9. `ALLAGMA-TOOL-TRUST-001` — real-tool trust model, without enabling real execution. **Closed** (PASS design-only; real execution blocked)
10. `ALLAGMA-SANDBOX-OBS-001` — explicit simulated/local execution labels. **Closed** (PASS backend/audit; FE-SANDBOX-LABELS-001 in `ontogony-frontend`, baseline lock pending SYSTEM-ALPHA-004)

## Sprint 2 — Frontend operator truth

**Status (2026-05-20):** Items **11–16** closed in `ontogony-frontend` (`FE_STUBS_001_THIN_PAGE_TRUTH_EVIDENCE.md`). Evidence index: `ontogony-frontend/docs/evidence/README.md` (Sprint 2 section).

11. `FE-CLEANUP-001` — resolve duplicate config artifacts. **Closed**
12. `FE-AUTH-001` — route-level operator auth guard. **Closed** (local browser session gate; not backend RBAC/SSO)
13. `FE-ERRBOUND-001` — route-level error boundaries. **Closed** (unit/page-level; full Playwright route resilience is Sprint 3 `FE-TEST-001`)
14. `FE-FIXTURE-MATRIX-001` — live/fallback/fixture matrix. **Closed**
15. `FE-STUBS-001` — honest states or live wiring for thin pages. **Closed** (`FE_STUBS_001_THIN_PAGE_TRUTH_EVIDENCE.md`)
16. `FE-API-ADAPTER-001` — adapter contract tests. **Closed** (`FE_API_ADAPTER_001_CONTRACT_TESTS_EVIDENCE.md`)

**Not Sprint 2:** `FE-ROUTE-STATES-001` (route-state/E2E catalog) is a separate slice; it does not claim full repo green or Sprint 2 completion.

## Sprint 3 — Operational maturity

**Status (2026-05-20):** Items **17–24** closed in source (GitHub review). Reconciliation addendum: [`SYSTEM_SPRINT3_CLOSEOUT_ADDENDUM.md`](SYSTEM_SPRINT3_CLOSEOUT_ADDENDUM.md). Evidence index: `ontogony-frontend/docs/evidence/README.md` (Sprint 3 section).

17. `SYSTEM-DASH-001` — dashboard/SLO starter pack. **Closed** (PASS / alpha-ready; hosted primarily in `allagma-dotnet`)
18. `CONEXUS-IDEMP-001` — durable implicit idempotency ledger. **Closed** (PASS; not cross-request retry dedupe without `Idempotency-Key`)
19. `CONEXUS-STREAM-COST-001` — streaming cost/usage observability. **Closed** (PASS)
20. `CONEXUS-ADMIN-SAFETY-001` — admin/diagnostic exposure tests. **Closed** (PASS)
21. `KANON-AUTH-LOCAL-001` — service-token local mode. **Closed** (PASS; landed as `KANON-AUTH-001/001A`)
22. `KANON-API-MODULAR-001` — modular endpoint registration. **Closed** (PASS / shallow modularization)
23. `FE-DOCKER-001` — Docker-local frontend composition. **Closed** (PASS)
24. `FE-TEST-001` — high-value page smoke tests. **Closed** (PASS; mocked Playwright smoke)

## Sprint 3.5 — Operational hardening (follow-ups from Sprint 3 review)

Do not reopen Sprint 3; treat these as precision and drift-prevention work.

- `SYSTEM-DASH-002` — mirror/centralize dashboard/SLO entrypoint in `ontogony-platform`. **Closed** (PASS — [`SYSTEM_DASH_002_EVIDENCE.md`](../../../evidence/SYSTEM_DASH_002_EVIDENCE.md))
- `SYSTEM-OBS-METERS-001` — `/ready` SLI, Kanon plan-compile meters, Conexus cost OTEL counters.
- `CONEXUS-IDEMP-DOC-001` — status docs: implicit ledger ≠ automatic retry dedupe without client key.
- `CONEXUS-ADMIN-ROUTE-INVENTORY-001` — generated `/admin/**` route inventory safety test.
- `KANON-AUTH-NAMING-001` — align sprint labels with `KANON-AUTH-001/001A`.
- `KANON-ENDPOINT-FILTERS-001` — shared filters/helpers + route-module inventory tests.
- `FE-LIVE-SMOKE-001` — one safe Docker-live Playwright smoke on `FE-DOCKER-001` stack. **Closed** (`FE_LIVE_SMOKE_001_DOCKER_LIVE_READONLY_EVIDENCE.md`)

## Sprint 4 — Deferred but important

25. `ALLAGMA-EVIDENCE-001` — audit/eval evidence alignment.
26. `ALLAGMA-STREAM-001` — Conexus streaming into Allagma progress events.
27. `CONEXUS-RETENTION-001` — retention evidence and maintenance visibility.
28. `KANON-CONEXUS-ASSIST-001` — assistance disabled/mock/local E2E.
29. `KANON-DOMAINPACK-GOV-001` — lifecycle promotion semantics.
