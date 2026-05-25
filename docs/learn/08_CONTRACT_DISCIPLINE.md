# Contract discipline

> **Audience:** backend developer, frontend developer  
> **Applies to:** all API-owning repos + `ontogony-frontend` generated clients  
> **Source of truth:** [`../CONTRACTS.md`](../CONTRACTS.md), generated inventories, OpenAPI snapshots  
> **Last verified:** 2026-05-25

## Principle

**Generated artifacts are source of truth.** Learning guides and feature docs **link** to them; they do not duplicate route tables or DTO fields by hand.

## When you change an HTTP route

Use the checklist in [11_ADD_OR_CHANGE_AN_API_ROUTE.md](./11_ADD_OR_CHANGE_AN_API_ROUTE.md). Minimum:

| Step | Repo | Action |
| --- | --- | --- |
| 1 | Owning API | Implement route + tests |
| 2 | Owning API | Update OpenAPI snapshot / provenance scripts |
| 3 | `ontogony-frontend` | `npm run openapi:sync:<service>` + `npm run openapi:gen` |
| 4 | `ontogony-frontend` | `npm run client-routes:sync` (route-workflow catalog) |
| 5 | `ontogony-frontend` | `npm run contracts:discipline` |
| 6 | Platform | Update cross-service matrix/registry if identifier semantics changed |

## Generated artifacts (do not hand-edit)

| Artifact | Repo |
| --- | --- |
| `docs/generated/*_ROUTE_INVENTORY.json` | Allagma, Kanon, Conexus |
| `docs/api/*-openapi-v1.snapshot.json` | Product APIs |
| `ontogony-frontend/docs/generated/*` | Console inventories, client route usage |
| `route-workflow-catalog.json` | ontogony-frontend |

## Frontend discipline gate

```powershell
cd C:\dev\ontogony-frontend
npm run contracts:discipline
npm run typecheck
npm run client-routes:sync
```

CI runs `contracts:discipline` on relevant paths.

## Allagma OpenAPI drift (example)

```powershell
cd C:\dev\allagma-dotnet
node ./scripts/patch-allagma-replay-openapi-snapshot.mjs
powershell -NoProfile -File ./scripts/update-allagma-openapi-provenance.ps1 -AlignmentPhase <phase-id>
cd ..\ontogony-frontend
npm run openapi:sync:allagma
npm run openapi:gen
```

Details: `allagma-dotnet/docs/TESTING.md`

## Cross-service contracts (platform)

| Doc | Topic |
| --- | --- |
| `docs/contracts/HEADER_PROPAGATION_CONTRACT.md` | Trace / correlation headers |
| `docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md` | Error JSON shape |
| `docs/contracts/REPLAY_RUNTIME_CONTRACT.md` | Replay modes and safety posture |

## Breaking changes

- Kanon: migration note under `docs/migrations/`, v0 graduation gates in `docs/contracts/`
- Allagma: `docs/migrations/`, runtime lock bump when evidence baselines move

## Next

- API route checklist: [11_ADD_OR_CHANGE_AN_API_ROUTE.md](./11_ADD_OR_CHANGE_AN_API_ROUTE.md)
- UI contracts: [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md)
