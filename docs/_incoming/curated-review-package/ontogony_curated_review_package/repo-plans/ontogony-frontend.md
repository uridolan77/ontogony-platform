# ontogony-frontend plan

## Priority

The frontend should become an honest operator console: live where possible, explicit fallback/demo where not.

## Keep

- Domain-segmented architecture.
- Route catalog and route tests.
- OntogonyShell consumption of shared UI.
- Live health/fallback pattern where already documented.
- Evaluation route code-splitting.

## Implement next

**Sprint 2 closeout (2026-05-20):** `FE-CLEANUP-001` through `FE-STUBS-001` and `FE-API-ADAPTER-001` closed.

**Sprint 3 frontend (2026-05-20):** `FE-DOCKER-001`, `FE-TEST-001` closed. Evidence: `docs/evidence/README.md` (Sprint 3 section).

Next platform-wide Sprint 3 items remain in `02_SPRINT_PLAN.md` (SYSTEM-DASH-001, Conexus/Kanon slices).

## Notes

System and Allagma fixture files already label fallback/demo intent in places. The remaining work is route-by-route data-source proof and removal of ambiguous mock operator surfaces, especially in Kanon pages.
