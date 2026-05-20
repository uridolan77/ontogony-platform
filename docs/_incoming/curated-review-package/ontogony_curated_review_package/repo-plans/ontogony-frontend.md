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

**Closed (2026-05-20):** `FE-CLEANUP-001`, `FE-AUTH-001`, `FE-ERRBOUND-001`, `FE-FIXTURE-MATRIX-001`, `FE-API-ADAPTER-001` — evidence in `ontogony-frontend/docs/evidence/FE_*_001_*` and `FE_FRONTEND_HARDENING_011_014_CLOSEOUT_EVIDENCE.md`.

1. `FE-STUBS-001`
2. `FE-DOCKER-001`
3. `FE-TEST-001`

## Notes

System and Allagma fixture files already label fallback/demo intent in places. The remaining work is route-by-route data-source proof and removal of ambiguous mock operator surfaces, especially in Kanon pages.
