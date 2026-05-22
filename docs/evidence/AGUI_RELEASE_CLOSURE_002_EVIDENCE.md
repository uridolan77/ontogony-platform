# AGUI-RELEASE-CLOSURE-002 — Frontend catalog gate (OpenAPI snapshot + workbench honesty)

**Date:** 2026-05-22  
**Verdict:** **CLOSED** — catalog/OpenAPI/stub fixes on `main`; `npm run check:full` green with AGUI-RELEASE-CLOSURE-001 (378 e2e, local 2026-05-22).

## Fix

| Item | Change |
| --- | --- |
| OpenAPI catalog drift | `npm run openapi-catalog:sync` — Kanon operator-expanded paths in `openApiSnapshotCatalog.ts` |
| FE-STUBS-001 | `RouteAwaitingActionPanel` idle surface on agent interaction workbench |
| FE-EVIDENCE-003 | Domain pack row evidence link test aligned to `evidence-spine-open-link-domainPackId` |
| Route coverage test | README static route count 29 (`/system/agent-interaction`) |
| Bundle budget | `maxTotalJsBytes` 1,605,632 (AG-UI + workbench dist ~1,526,267 B) |
| E2E drift | `operatorUxRoutes`, OpenAPI-capability copy, collapsible capability notes (post-`d473726`) |
| E2E gate (2026-05-22) | Operator home health badges, local access gate copy, `/system/topology` correlation, mock resets — see [`AGUI_RELEASE_CLOSURE_E2E_001_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/AGUI_RELEASE_CLOSURE_E2E_001_EVIDENCE.md) |

## Validation (local)

```powershell
cd C:\dev\ontogony-frontend
npm run openapi-catalog:sync
npm run test -- src/shared/capability/openApiSnapshotCatalog.test.ts
npm run test -- src/app/routeThinPageTruth.test.ts src/kanon/components/KanonDomainPackLifecycleWorkbench.evidenceSpine.test.ts
npm run check
npm run check:full
```

| Command | Result (2026-05-22) |
| --- | --- |
| `npm run check` | pass |
| `npm run check:full` | pass — **378** Playwright tests, ~5.3m |

Frontend SHA: `f83cb58a5333aae50aeea24481a886814b9fef0b`.

## Closure criteria

- [x] `ontogony-frontend` `npm run check` + `check:full` green locally
- [x] [`AGUI_RELEASE_CLOSURE_001_EVIDENCE.md`](./AGUI_RELEASE_CLOSURE_001_EVIDENCE.md) verdict → **CLOSED**

## Non-claims

- Kanon review-queue in frontend timeline remains **KANON-AGUI-WORKBENCH-001** (next slice).
