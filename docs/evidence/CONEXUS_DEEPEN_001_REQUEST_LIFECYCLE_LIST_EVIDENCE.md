# CONEXUS-DEEPEN-001 — Request lifecycle list contract evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (backend + frontend list contract shipped)  
**Statement:** Admin model-call list API and observability recent-requests UI. Redacted rows only; no raw prompts/completions in list DTO.

## Scope

`CONEXUS-DEEPEN-001` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`.

## Repos touched

| Repo | Change |
| --- | --- |
| `conexus-dotnet` | `GET /admin/v0/model-calls`, `IModelCallAdminListQuery`, EF + in-memory queries, integration tests |
| `ontogony-frontend` | OpenAPI snapshot, client/hook, `ConexusRecentModelCallsSection`, backend-waiting activation |
| `ontogony-platform` | This evidence file |

## Backend contract

```http
GET /admin/v0/model-calls
```

- Auth: Conexus admin key (`admin:read`)
- Pagination: cursor, default limit 50, max 100, newest-first
- Persistence modes: `postgres` | `in_memory` (with restart warning in response)
- Primary store: `conexus_llm_response` joined with `conexus_llm_request`; correlation from execution run metadata when present

## Frontend

- `/conexus/observability` — **Recent model calls** table with project/status filters, load more, row actions (open workbench, copy id, route link)
- Backend-waiting `conexus-request-list` activated via `openapi/conexus.v0.json`
- Guidance/limitations cards updated when list route is documented

## Validation

```text
conexus-dotnet:
  dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~ModelCallAdminList
  → Passed (4 tests)

ontogony-frontend:
  npm run typecheck (or project equivalent)
  vitest src/conexus/api/conexusModelCallList.test.ts
```

## Known limitations

- Post-filters (correlation, route decision, model alias, status/outcome) may scan a bounded batch in EF when combined with SQL filters.
- Load-more pagination appends pages client-side; detail workbench remains CONEXUS-DEEPEN-002.
- Route link from table pre-fills observability query params; dedicated route explorer is CONEXUS-DEEPEN-003.

## Next expected item

**CONEXUS-DEEPEN-002** — `GET /admin/v0/model-calls/{modelCallId}` admin detail aggregate with provider attempts.
