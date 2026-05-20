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
10. `ALLAGMA-SANDBOX-OBS-001` — explicit simulated/local execution labels. **Closed** (PASS backend/audit; frontend labels follow in Sprint 2)

## Sprint 2 — Frontend operator truth

**Status (2026-05-20):** Items 11–14 closed in `ontogony-frontend` (evidence under `docs/evidence/FE_*_001_*` and `FE_FRONTEND_HARDENING_011_014_CLOSEOUT_EVIDENCE.md`).

11. `FE-CLEANUP-001` — resolve duplicate config artifacts. **Closed**
12. `FE-AUTH-001` — route-level operator auth guard. **Closed**
13. `FE-ERRBOUND-001` — route-level error boundaries. **Closed**
14. `FE-FIXTURE-MATRIX-001` — live/fallback/fixture matrix. **Closed**
15. `FE-STUBS-001` — honest states or live wiring for thin pages.
16. `FE-API-ADAPTER-001` — adapter contract tests. **Closed** (`FE_API_ADAPTER_001_CONTRACT_TESTS_EVIDENCE.md`)

## Sprint 3 — Operational maturity

17. `SYSTEM-DASH-001` — dashboard/SLO starter pack.
18. `CONEXUS-IDEMP-001` — durable implicit idempotency.
19. `CONEXUS-STREAM-COST-001` — streaming cost/usage observability.
20. `CONEXUS-ADMIN-SAFETY-001` — admin/diagnostic exposure tests.
21. `KANON-AUTH-LOCAL-001` — service-token local mode.
22. `KANON-API-MODULAR-001` — modular endpoint registration.
23. `FE-DOCKER-001` — Docker-local frontend composition.
24. `FE-TEST-001` — high-value page smoke tests.

## Sprint 4 — Deferred but important

25. `ALLAGMA-EVIDENCE-001` — audit/eval evidence alignment.
26. `ALLAGMA-STREAM-001` — Conexus streaming into Allagma progress events.
27. `CONEXUS-RETENTION-001` — retention evidence and maintenance visibility.
28. `KANON-CONEXUS-ASSIST-001` — assistance disabled/mock/local E2E.
29. `KANON-DOMAINPACK-GOV-001` — lifecycle promotion semantics.
