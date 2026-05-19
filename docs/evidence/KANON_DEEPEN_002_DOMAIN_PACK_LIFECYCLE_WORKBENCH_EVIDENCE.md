# KANON-DEEPEN-002 — Domain-pack lifecycle workbench

**Status:** Implementation complete — browser walkthrough **not verified in this polish pass**  
**Depends on:** [KANON-DEEPEN-001](KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)  
**Sequence index:** [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)

## Goal

Turn Kanon domain packs from a shallow list into an operator-grade lifecycle workbench: active/current packs, per-version timeline, honest mutation availability, and linked decision IDs.

## Implementation commits

| Repo | Commit | Summary |
|---|---|---|
| `ontogony-frontend` | `5bff111` | Operator domain-pack lifecycle workbench |
| `kanon-dotnet` | `a4eec4a` | OpenAPI schema types for domain-pack lifecycle routes |
| `ontogony-platform` | `f3fea49` | Initial 002 evidence (this doc expanded in polish pass) |

## Repos touched

| Repo | Change |
|---|---|
| `kanon-dotnet` | OpenAPI `Produces<>` for domain-pack list/detail/active/lifecycle/mutation responses; `OpenApiDomainPackSchemaTests` |
| `ontogony-frontend` | Lifecycle workbench (summary cards, table, detail panel, timeline, decision links); typed lifecycle client; action availability from lifecycle + roles |
| `ontogony-platform` | Evidence + sequence index |

## Source files (frontend)

- `src/kanon/components/KanonDomainPackLifecycleWorkbench.tsx`
- `src/kanon/components/KanonDomainPackActions.tsx`
- `src/kanon/components/KanonDecisionIdLink.tsx`
- `src/kanon/lifecycle/domainPackActionAvailability.ts`
- `src/kanon/pages/DomainPacksPage.tsx`
- `src/kanon/api/kanonClient.ts` (lifecycle mutations)

## Backend contracts

Routes unchanged; DTOs already carried lifecycle fields. OpenAPI baseline includes:

- `DomainPackSummaryContract` (`lifecycleStatus`, `isActivePackVersion`, `contentHash`, `lastDecisionId`, …)
- `DomainPackLifecycleListResponse` / `DomainPackLifecycleStatusContract`
- `DomainPackActiveListResponse` / `DomainPackActiveRowContract`

Kanon evidence (OpenAPI only): `kanon-dotnet/docs/evidence/KANON_DEEPEN_002_DOMAIN_PACK_OPENAPI_EVIDENCE.md`

## Closed (002 acceptance)

- Domain-pack lifecycle schemas visible in Kanon OpenAPI
- `/kanon/domain-packs` lifecycle workbench
- Summary cards, active pack summary, lifecycle table
- Active/lifecycle badges, detail panel
- Validate/promote/load decision links to `/kanon/decisions`
- Role/lifecycle-gated actions (Admin/System mutate; Auditor read-only)

## Validation

| Check | Command | Result (2026-05-20 polish pass) |
|---|---|---|
| Kanon OpenAPI schema tests | `dotnet test kanon-dotnet --filter OpenApiDomainPackSchemaTests` | Not re-run (no backend code change) |
| Frontend lifecycle capability tests | `npm test -- src/kanon/lifecycle/` | Not run in this pass |
| Frontend typecheck | `npm run typecheck` | Pre-existing unrelated failures in repo |

## Manual browser verification

**Status: NOT VERIFIED** in this polish pass.

After `docker compose build ontogony-frontend kanon-api` in `docker/local-working-system`:

| Route / action | Expected |
|---|---|
| `/kanon/domain-packs` | Summary cards; table shows packs with lifecycle + active badges |
| Pack **Details** | Timeline lists versions; decision IDs link to decisions workbench |
| Auditor actor | Read works; mutation buttons disabled with role explanation |
| Admin actor | Validate → promote → load when lifecycle allows |

## Known limitations

- Auditor is read-only; validate/load/promote/deprecate require Admin/System.
- HTTP load disabled (`DomainPackHttpLoadDisabled`) surfaces as mutation error, not a pre-flight UI flag.
- Lifecycle history depends on persisted lifecycle rows in Postgres.
- Manifest/signature fields appear on validate response, not on list summary.
- Browser verification requires rebuilt frontend image.

## Follow-up

- **KANON-DEEPEN-003** — decision provenance explorer (done; see 003 evidence).
- **KANON-DEEPEN-006** — full-track browser closeout checklist.
