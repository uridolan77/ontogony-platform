# CONEXUS-DEEPEN-003 — Route decision explorer evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (enriched admin route-decision detail + observability explorer UI)  
**Statement:** Operators can explain why a request used a given provider/model via route decision explorer linked from model-call list/detail and Allagma journey links.

## Scope

`CONEXUS-DEEPEN-003` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`.

## Repos touched

| Repo | Change |
| --- | --- |
| `conexus-dotnet` | `GET /admin/v0/route-decisions/{id}` returns `RouteDecisionDetailResponse` (enriched aggregate) |
| `ontogony-frontend` | Route decision explorer panel, adapters, OpenAPI, links from list/detail/Allagma |
| `ontogony-platform` | This evidence file |

## Backend contract

```http
GET /admin/v0/route-decisions/{routeDecisionId}
```

Enrichment over stored `RouteDecisionRecord`:

- `modelCallId` (`chatcmpl-{requestId}`), optional `correlationId` from execution journal
- `overrideSource` (`project_alias_override` | `global_model_alias`)
- `selectedProvider` + `fallbackProviders` readiness (current gateway config; no secrets)
- `relatedModelCallIds` from admin model-call list filter on `routeDecisionId`
- `warnings` (in-memory retention, provider not ready, etc.)

## Frontend

- `/conexus/observability?routeDecisionId=…` — **Route decision explorer** (alias, override, fallback chain, provider readiness, linkage, related model calls)
- Links from Recent model calls **Route**, model-call detail **Open route decision explorer**, Allagma evidence journey **Conexus route decision** (when model call id is `chatcmpl-{requestId}`)

## Validation

```text
conexus-dotnet:
  dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~RouteDecisionDetail
  dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~RouteDecisionAdmin

ontogony-frontend:
  npx vitest run src/conexus/adapters/conexusRouteDecisionAdapters.test.ts
  npx vitest run src/conexus/api/conexusRouteDecisionDetail.test.ts
  npm run typecheck
```

## Known limitations

- Provider readiness on detail is **current config**, not point-in-time at request (called out in UI).
- Related model calls depend on telemetry list filter window and persistence mode.
- Allagma route-decision link requires `chatcmpl-{requestId}` model call id shape to infer `rd-{requestId}`.

## Next expected item

**CONEXUS-DEEPEN-004** — Usage/cost workbench drill-down from usage window to model-call rows.
