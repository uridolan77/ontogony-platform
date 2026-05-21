# FIRST-VERSION-RC-001 Evidence

**Date:** 2026-05-21  
**Repos:** allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-platform, ontogony-frontend, ontogony-ui  
**Verdict:** PASS

## Summary

Cut **OPERATOR-V1-001** product lock on top of **SYSTEM-ALPHA-006** runtime baseline. Validated focused tests, protocol/stale/real-tools guards, feature-connection matrix, frontend `npm run check`, and **8/8** mocked operator demo Playwright flows.

## Files changed

| Repo | Files |
| --- | --- |
| `ontogony-platform` | `docs/system/ontogony-operator-v1.lock.json`, `scripts/validate-operator-v1-lock.ps1`, `scripts/run-operator-v1-validation.ps1`, `docs/evidence/FIRST_VERSION_RC_001_EVIDENCE.md`, `docs/releases/FIRST_VERSION_RC_001_CLOSEOUT.md`, `docs/_incoming/.../STALE_PACKAGE_QUARANTINE.json`, `tests/.../FirstVersionRc001Tests.cs`, `.github/workflows/ci.yml` |
| `allagma-dotnet` | `docs/api/allagma-openapi-v1.provenance.json`, `tests/.../NotImplementedKanon*.cs`, test stub updates |
| `ontogony-frontend` | `openapi/allagma.v0.json`, `openapi-catalog:sync`, `scripts/lib/prettier-markdown.mjs`, `.prettierignore`, `scripts/sync-openapi-snapshot-catalog.mjs`, `src/shared/capability/openApiSnapshotCatalog.ts`, `src/shared/performance/performanceBudgets.json`, `vitest.config.ts`, route/test fixes, `e2e/operator-v1-demo-flows.spec.ts`, `e2e/helpers/mockServices.ts` |
| `ontogony-ui` | repack consumed via `npm pack` (no source change required for RC) |

## Validation

| Gate | Command | Result |
| --- | --- | --- |
| Platform tests | `dotnet test Ontogony.Platform.sln -c Release` | **PASS** |
| Stale package guard | `validate-stale-incoming-package.ps1` | **PASS** (Operator V1 package quarantined) |
| Real tools block | `validate-real-tools-block.ps1` | **PASS** |
| Protocol registry | `validate-system-protocol-registry.ps1` | **PASS** |
| Post-lock register | `validate-post-lock-delta-register.ps1` | **PASS** |
| Operator v1 lock | `validate-operator-v1-lock.ps1 -RequireEvidence` | **PASS** |
| Allagma tests | `dotnet test Allagma.sln -c Release` | **PASS** (753) |
| Kanon tests | `dotnet test -c Release` | **PASS** (312) |
| Conexus tests | `dotnet test -c Release` | **PASS** |
| Feature matrix | `validate-feature-connection-matrix.ps1` | **PASS** (after `openapi:sync:allagma`) |
| Frontend check | `npm run check` | **PASS** (after UI repack + route allowlist) |
| Demo Playwright | `npm run test:e2e:demo-flows` | **8/8 PASS** |
| Docker cohesion / restart | ALPHA-006 artifacts | **PASS** (unchanged runtime lock evidence) |

## Operator behavior

| Capability | Status | Evidence |
| --- | --- | --- |
| Operator home | PASS | `SYS_OPERATOR_HOME_001` |
| Guided run workbench | PASS | `ALLAGMA_OPERATOR_RUNS_001` |
| Human gates | PASS | demo flows approve/deny |
| Conexus operator | PASS | `CONEXUS_OPERATOR_001` |
| Evidence Spine | PASS | `EVIDENCE_SPINE_OPERATOR_001` |
| Sandbox workbench | PASS | `ALLAGMA_SANDBOX_WORKBENCH_001` |
| Demo flows | PASS | `SYSTEM_DEMO_FLOWS_001` |

## Safety statement

- Real external tool execution remains blocked.
- Model assistance remains draft-only/non-authoritative where applicable.
- This is Docker-local/operator scope, not production readiness.
- Kanon remains the only semantic authority.

## Known limitations

- **OPERATOR-V1-001** pins companion SHAs; **SYSTEM-ALPHA-006** runtime lock SHAs for the four backend repos remain the governed-runtime floor until a future SYS-LOCK promotion.
- Docker-live demo prep is optional; Playwright uses mocked APIs.
- Frontend Conexus OpenAPI snapshot refresh for route-preview/quota routes is deferred (client-route audit allowlist until next Conexus OpenAPI sync).
- **ReleaseMode** lock SHA check passes at pinned HEADs; commit local RC fixes before promoting lock SHAs on moving branches.
- `*.live.test.ts` files are excluded from default `npm run check` (run with docker-local stack when needed).
- Playwright requires port **5176** free (stop stray `vite` before `test:e2e:demo-flows`).

## Next step

Promote moving-main backend deltas into **SYSTEM-ALPHA-007** when cohesion/restart revalidation is due; keep **OPERATOR-V1-001** as the operator product RC reference.
