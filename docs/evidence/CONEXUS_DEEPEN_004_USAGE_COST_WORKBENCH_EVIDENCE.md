# CONEXUS-DEEPEN-004 — Usage, cost, and token workbench evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (enriched governance usage summary + observability drill-down UI)  
**Statement:** Operators can move from aggregate usage/cost in the governance window to filtered recent model-call rows without fabricating token usage.

## Scope

`CONEXUS-DEEPEN-004` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`.

## Repos touched

| Repo | Change |
| --- | --- |
| `conexus-dotnet` | `GET /v1/governance/usage` summary adds `byModel`, `byAlias`, failed count, token coverage fields |
| `ontogony-frontend` | Usage/cost workbench card, adapters, drill-down into Recent model calls |
| `ontogony-platform` | This evidence file |

## Backend contract

`GET /v1/governance/usage?from=&to=` (project API key) returns `UsageCostSummary`:

- Successful request totals: `requestCount`, `inputTokens`, `outputTokens`, `totalTokens`, `estimatedCost`, `actualCost`, `currency`
- `failedRequestCount` (excluded from cost/token totals)
- `requestsWithTokenUsage` / `requestsMissingTokenUsage`
- `byModel[]` (`providerKey`, `providerModel`, counts, tokens, cost)
- `byAlias[]` (`modelAlias`, counts, tokens, cost)

## Frontend

- `/conexus/observability` — **Usage / cost window** workbench with provider/alias breakdown tables
- **Show contributing requests** / per-row **Show requests** applies `from`/`to` window + filters on **Recent model calls** (`GET /admin/v0/model-calls`)
- Token coverage copy explains when provider usage was not captured (no fabricated values)

## Validation

```text
conexus-dotnet:
  dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~UsageCost
  dotnet test tests/Conexus.Persistence.Tests --filter FullyQualifiedName~UsageCost
  dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~GovernanceUsage

ontogony-frontend:
  npx vitest run src/conexus/adapters/conexusUsageAdapters.test.ts
  npx vitest run src/conexus/pages/ConexusObservabilityPage.test.tsx
  npm run typecheck
```

## Known limitations

- Usage window is fixed to the last 7 days in the operator hook (same as before); drill-down uses that window only.
- Failed-request cost is not estimated in the usage summary.

## Review polish (004A)

- “Show contributing requests” applies `status=completed` to match successful usage totals.
- “Show requests missing token usage” applies `tokenUsage=missing` via `GET /admin/v0/model-calls` list filter.
- List rows expose `tokenUsageAvailable`.

## Review polish (004B)

- `tokenUsage` query param must be `present` or `missing` (400 otherwise).
- `tokenUsageAvailable` means usage was **captured** (any non-null token field), not “tokens &gt; 0”.

## Next expected item

**CONEXUS-DEEPEN-005** — Cross-service evidence spine (resolver + detail links).
