# PLAT-AGUI-ADAPTER-001 — Shared AG-UI compatibility package evidence

**Date:** 2026-05-22  
**Sprint:** ADAPTER-AGUI-001 (platform promotion)  
**Baseline:** `SYSTEM-ALPHA-006`

## Scope delivered

| Artifact | Role |
| --- | --- |
| `packages/ontogony-agent-interaction/` | npm package `@ontogony/agent-interaction` |
| `docs/operators/AG_UI_COMPATIBILITY_ADAPTER.md` | Operator-facing mapping index |
| `src/agUiAdapter.ts` + tests | Pure Ontogony → AG-UI adapter (fixture-driven) |

## Non-claims

- No CopilotKit / `@ag-ui/*` dependency.
- No change to `AgUiProtocolAdapter` ingress DTO semantics.
- No live SSE (`ALLAGMA-AGUI-002` still deferred).

## Validation

```bash
cd packages/ontogony-agent-interaction
npm install
npm run test
npm run typecheck
```

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1
```

## Consumer

- `ontogony-frontend` depends on `file:../ontogony-platform/packages/ontogony-agent-interaction` and re-exports adapter APIs from `src/agent-interaction/`.
