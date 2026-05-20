# KANON-CONNECT-006 — Route/OpenAPI/catalog parity gate

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (automated cross-repo parity check + unit tests)  
**Statement:** Kanon `/ontology/v0` routes are aligned across backend inventory, committed OpenAPI baselines, frontend snapshot catalog, and route-workflow catalog. Conexus/Allagma OpenAPI drift is explicitly out of scope for this gate.

## Summary

Added a cross-repo parity gate in `ontogony-frontend` that compares:

| Artifact | Repo | Role |
| --- | --- | --- |
| `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json` | `kanon-dotnet` | Canonical route signatures (from `OntologyV0RouteCatalog` + live endpoint tests) |
| `docs/api/kanon-openapi-v1.json` | `kanon-dotnet` | Committed OpenAPI baseline (`OpenApiBaselineTests`) |
| `docs/architecture/ONTOLOGY_V0_ROUTE_AUTH_MATRIX.md` | `kanon-dotnet` | Auth class matrix + drift guard references |
| `openapi/kanon.v0.json` | `ontogony-frontend` | Operator-expanded snapshot |
| `src/shared/capability/openApiSnapshotCatalog.ts` | `ontogony-frontend` | Typed catalog for capability/backend-waiting contracts |
| `src/app/route-workflow-catalog.json` | `ontogony-frontend` | Per-page hooks, clients, and `backendRoutes` |
| `docs/generated/ROUTE_WORKFLOW_INVENTORY.md` | `ontogony-frontend` | Generated workflow inventory |

**Drift fixed:** `/kanon/domain-packs` workflow `backendRoutes` incorrectly used `POST …/domain-packs/{id}/load|promote|deprecate`; corrected to match live API (`POST …/domain-packs/load`, etc.) and documented evolution + lifecycle routes used by the workbench.

## Gate commands

```powershell
cd C:\dev\ontogony-frontend
npm run kanon:route-parity:check
npm test -- src/kanon/contracts/kanonRouteParity.test.ts
```

`npm run check` includes `kanon:route-parity:check` after `inventory:check`.

Set `KANON_DOTNET_ROOT` when `kanon-dotnet` is not a sibling of `ontogony-frontend`.

## Quarantine (non-Kanon)

| Service | Gate scope |
| --- | --- |
| Kanon | **In scope** — 59 `/ontology/v0` signatures must match across artifacts above |
| Conexus | **Out of scope** — `openApiSnapshotCatalog` conexus section may drift from `conexus-dotnet`; tracked under SYSTEM-ALPHA / Conexus connect, not misattributed to Kanon |
| Allagma | **Out of scope** — synced via `openapi:sync:allagma` separately |

## Tests

| Check | Result |
| --- | --- |
| Inventory ↔ `kanon-openapi-v1.json` | PASS (59 routes) |
| Backend OpenAPI ↔ `openapi/kanon.v0.json` | PASS |
| `openApiSnapshotCatalog` ↔ `kanon.v0.json` | PASS |
| Workflow catalog `backendRoutes` ⊆ inventory | PASS (after domain-pack path fix) |
| All nine `/kanon/*` pages documented in workflow catalog | PASS |
| Auth matrix references inventory + `OpenApiBaselineTests` | PASS |

## Acceptance

- [x] Kanon route parity is green across listed artifacts
- [x] Known Conexus catalog drift is not misattributed to Kanon
- [x] New Kanon routes require inventory + OpenAPI baseline + frontend snapshot + workflow entry (enforced by existing Kanon tests + this gate)

## Related

- [KANON-CONNECT-001](./KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md)
- [KANON-CONNECT-005](./KANON_CONNECT_005_EVIDENCE_SPINE_SEMANTIC_GRAPH_EVIDENCE.md)
- Next: **KANON-CONNECT-007** — full cross-service Docker smoke
