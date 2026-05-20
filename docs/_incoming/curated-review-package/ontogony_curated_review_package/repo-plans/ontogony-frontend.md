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

1. `FE-CLEANUP-001`
2. `FE-AUTH-001`
3. `FE-ERRBOUND-001`
4. `FE-FIXTURE-MATRIX-001`
5. `FE-STUBS-001`
6. `FE-API-ADAPTER-001`
7. `FE-DOCKER-001`
8. `FE-TEST-001`

## Notes

System and Allagma fixture files already label fallback/demo intent in places. The remaining work is route-by-route data-source proof and removal of ambiguous mock operator surfaces, especially in Kanon pages.
