# KANON-DEEPEN-002 — Domain-pack lifecycle workbench

**Status:** Implementation complete (browser verification after frontend image rebuild)  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md) (local operator read roles)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)

## Goal

Turn Kanon domain packs from a shallow list into an operator-grade lifecycle workbench: active/current packs, per-version timeline, honest mutation availability, and linked decision IDs.

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | OpenAPI `Produces<>` for domain-pack list/detail/active/lifecycle/mutation responses; `OpenApiDomainPackSchemaTests` |
| `ontogony-frontend` | Lifecycle workbench (summary cards, table, detail panel, timeline, decision links); typed lifecycle client; action availability from lifecycle + roles |
| `ontogony-platform` | This evidence |

## Backend

Routes unchanged; DTOs already carried lifecycle fields. OpenAPI baseline now includes:

- `DomainPackSummaryContract` (`lifecycleStatus`, `isActivePackVersion`, `contentHash`, `lastDecisionId`, `lastLoadedAt`, …)
- `DomainPackLifecycleListResponse` / `DomainPackLifecycleStatusContract`
- `DomainPackActiveListResponse` / `DomainPackActiveRowContract`

Refresh baseline: `./scripts/update-kanon-openapi-baseline.ps1` from `kanon-dotnet` root.

## Frontend

`/kanon/domain-packs` now provides:

1. Summary cards — pack count, active versions (`GET /domain-packs/active`), deprecated count
2. Pack table — lifecycle badge, active badge, ontology version
3. Detail panel — hash, last decision, lifecycle timeline with validate/promote/load decision links to `/kanon/decisions`
4. Actions — validate/load/promote/deprecate gated by **Admin/System** and lifecycle state (Auditor read-only)

## Validation

| Check | Command |
|---|---|
| Kanon OpenAPI schema tests | `dotnet test kanon-dotnet --filter OpenApiDomainPackSchemaTests` |
| Kanon domain-pack API tests | `dotnet test kanon-dotnet --filter DomainPackManagementApiTests` |
| Frontend unit tests | `npm test` in `ontogony-frontend` |
| Frontend typecheck | `npm run typecheck` in `ontogony-frontend` |

## Manual acceptance

After `docker compose build ontogony-frontend kanon-api` in `docker/local-working-system`:

| Route / action | Expected |
|---|---|
| `/kanon/domain-packs` | Summary cards; table shows packs with lifecycle + active badges |
| Pack **Details** | Timeline lists versions; decision IDs link to decisions workbench |
| Auditor actor | Read works; mutation buttons disabled with role explanation |
| Admin actor (settings) | Validate → promote (reviewed → accepted) → load when lifecycle allows |

## Known limitations

- Manifest/signature fields appear on validate response, not on list summary (use validate or lifecycle row hash).
- HTTP load disabled (`DomainPackHttpLoadDisabled`) surfaces as mutation error, not a pre-flight flag on the UI.
- Single pack version per directory on disk; lifecycle API returns multiple versions only when Postgres lifecycle store has history.

## Follow-up

- **KANON-DEEPEN-003** — provenance explorer polish and cross-service links.
