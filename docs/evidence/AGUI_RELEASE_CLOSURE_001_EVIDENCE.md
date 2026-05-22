# AGUI-RELEASE-CLOSURE-001 — AG-UI continuation phases 1–6 land

**Date:** 2026-05-22  
**Verdict:** **CLOSED** — phases 1–6 on `main`; `ontogony-frontend` `npm run check:full` green (378 Playwright tests, local 2026-05-22).

## Scope

Land AG-UI continuation plan phases 1–6 without splitting PRs:

| Phase | ID | Repo |
| --- | --- | --- |
| 1 | OFE-AGUI-004 | ontogony-frontend |
| 2 | AGUI-SPINE-CLOSEOUT-001 | ontogony-platform |
| 3 | OFE-AGUI-005 | ontogony-frontend |
| 4 | KANON-AGUI-REVIEW-001 | kanon-dotnet |
| 5 | CONEXUS-AGUI-002 | conexus-dotnet |
| 6 | ADAPTER-AGUI-002 | ontogony-platform + ontogony-frontend |

## Landed on `main` (2026-05-22)

### ontogony-platform

- `docs/operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md`
- `docs/schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json`
- `docs/evidence/PLAT_AGUI_RESOLVER_002_EVIDENCE.md`
- Closeout + continuation plan + acceptance (incl. ADAPTER-AGUI-002 tick)

### ontogony-frontend

- `src/agent-interaction/evidence/*` resolver + tests
- `AgentInteractionEvidenceGraphAction` / `Drawer`
- `docs/evidence/OFE_AGUI_RESOLVER_002_EVIDENCE.md`
- Workbench adapter alignment (same release commit family)
- E2E alignment for operator home, access gate, topology correlation, Conexus diagnostics errors, Kanon plan/bindings (`f83cb58` family)

## Validation (local)

```powershell
cd C:\dev\ontogony-platform
powershell -NoProfile -File .\scripts\validate-agent-interaction-spine.ps1
dotnet test tests\Ontogony.Infrastructure.Tests --filter FullyQualifiedName~SystemAgentInteractionSpineContractTests

cd C:\dev\ontogony-frontend
npm run typecheck
npm run test -- src/agent-interaction/
npm run check:full
```

### `check:full` result (2026-05-22, operator machine)

| Step | Result | Notes |
| --- | --- | --- |
| `openapi:check` through `performance:check` | pass | 21-step `run-check.mjs --e2e` pipeline |
| `test:e2e` | pass | **378 passed** (5.3m, 2 workers) |
| Frontend SHA | `f83cb58a5333aae50aeea24481a886814b9fef0b` | `main`, synced with `origin/main` |

E2E detail: [`ontogony-frontend/docs/evidence/AGUI_RELEASE_CLOSURE_E2E_001_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/AGUI_RELEASE_CLOSURE_E2E_001_EVIDENCE.md).

## CI status

| Repo | Branch | Notes |
| --- | --- | --- |
| ontogony-platform | `main` | Docs/evidence; monitor `ci` after evidence push |
| ontogony-frontend | `main` | `check:full` green locally; e2e fixes on `main` (`f83cb58`) |
| ontogony-ui | `main` | `9731c04` — tsconfig paths for `feedback` / `navigation` subpaths |

## Closure criteria

- [x] Frontend `npm run check:full` green locally (378 Playwright tests)
- [x] AG-UI continuation phases 1–6 landed on `main`
- [x] [`AGUI_RELEASE_CLOSURE_002_EVIDENCE.md`](./AGUI_RELEASE_CLOSURE_002_EVIDENCE.md) catalog gate closed
- [ ] Optional docker-live smoke: `/system/agent-interaction?runId=...`
- [ ] **KANON-AGUI-WORKBENCH-001** — review queue in frontend timeline (**next** product slice; not a release-closure blocker)

## Non-claims

- Kanon review-queue not in frontend workbench timeline (library projection only).
- No server-side evidence aggregation API.
- Local `check:full` ≠ GitHub Actions green until CI run recorded on the same SHA.
