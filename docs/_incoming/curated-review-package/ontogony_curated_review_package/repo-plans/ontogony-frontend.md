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

**Sprint 2 closeout (2026-05-20):** `FE-CLEANUP-001` through `FE-STUBS-001` and `FE-API-ADAPTER-001` closed. Evidence: `ontogony-frontend/docs/evidence/README.md` (Sprint 2 section).

1. `FE-DOCKER-001`
2. `FE-TEST-001`

## Notes

System and Allagma fixture files already label fallback/demo intent in places. The remaining work is route-by-route data-source proof and removal of ambiguous mock operator surfaces, especially in Kanon pages.
