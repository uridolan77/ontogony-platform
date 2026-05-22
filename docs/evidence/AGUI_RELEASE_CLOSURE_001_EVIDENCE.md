# AGUI-RELEASE-CLOSURE-001 — AG-UI continuation phases 1–6 land

**Date:** 2026-05-22  
**Verdict:** **IN PROGRESS** — artifacts on `main`; frontend CI fixes pushed (`ontogony-ui` tsconfig, route/inventory sync).

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

## Validation (local)

```powershell
cd C:\dev\ontogony-platform
powershell -NoProfile -File .\scripts\validate-agent-interaction-spine.ps1
dotnet test tests\Ontogony.Infrastructure.Tests --filter FullyQualifiedName~SystemAgentInteractionSpineContractTests

cd C:\dev\ontogony-frontend
npm run typecheck
npm run test -- src/agent-interaction/
```

## CI status

| Repo | Branch | Notes |
| --- | --- | --- |
| ontogony-platform | `main` | Monitor `ci` + CodeQL after push |
| ontogony-frontend | `main` | Monitor CI after `df7932f` + inventory catalog commits |
| ontogony-ui | `main` | `9731c04` — tsconfig paths for `feedback` / `navigation` subpaths |

## Remaining (post-green CI)

- [ ] Frontend `CI` workflow green on `main`
- [ ] Optional docker-live smoke: `/system/agent-interaction?runId=...`
- [ ] **KANON-AGUI-WORKBENCH-001** — review queue in frontend timeline (next product slice)

## Non-claims

- Kanon review-queue not in frontend workbench timeline (library projection only).
- No server-side evidence aggregation API.
