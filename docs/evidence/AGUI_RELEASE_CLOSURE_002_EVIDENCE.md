# AGUI-RELEASE-CLOSURE-002 — Frontend catalog gate (OpenAPI snapshot + workbench honesty)

**Date:** 2026-05-22  
**Verdict:** **IN PROGRESS** — patch pushed; await green `ontogony-frontend` CI on `main`.

## Fix

| Item | Change |
| --- | --- |
| OpenAPI catalog drift | `npm run openapi-catalog:sync` — Kanon operator-expanded paths in `openApiSnapshotCatalog.ts` |
| FE-STUBS-001 | `RouteAwaitingActionPanel` idle surface on agent interaction workbench |
| FE-EVIDENCE-003 | Domain pack row evidence link test aligned to `evidence-spine-open-link-domainPackId` |
| Route coverage test | README static route count 29 (`/system/agent-interaction`) |
| Bundle budget | `maxTotalJsBytes` 1,605,632 (AG-UI + workbench dist ~1,526,267 B) |

## Validation (local)

```powershell
cd C:\dev\ontogony-frontend
npm run openapi-catalog:sync
npm run test -- src/shared/capability/openApiSnapshotCatalog.test.ts
npm run test -- src/app/routeThinPageTruth.test.ts src/kanon/components/KanonDomainPackLifecycleWorkbench.evidenceSpine.test.ts
```

Full gate (operator):

```powershell
npm run check
npm run check:full   # after check green
```

## Closure criteria

- [ ] `ontogony-frontend` CI `check` green on `main`
- [ ] Optional `check:full` green or deferred with reason in this doc
- [ ] `AGUI_RELEASE_CLOSURE_001_EVIDENCE.md` verdict → **CLOSED** when CI green

## Non-claims

- Kanon review-queue in frontend timeline remains **KANON-AGUI-WORKBENCH-001** (next slice).
