# CONEXUS-DEEPEN-002 — Model-call detail and provider attempts evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (backend aggregate + frontend detail workbench)  
**Statement:** Admin model-call detail API with provider attempts from execution journal; observability workbench panel with summary, route, attempts, tokens, errors, and cross-service links.

## Scope

`CONEXUS-DEEPEN-002` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`.

## Repos touched

| Repo | Change |
| --- | --- |
| `conexus-dotnet` | `GET /admin/v0/model-calls/{modelCallId}`, `IModelCallAdminDetailQuery`, EF + in-memory queries, mapping, tests |
| `ontogony-frontend` | OpenAPI snapshot, client/hook, `ConexusModelCallDetailPanel`, workbench wiring |
| `ontogony-platform` | This evidence file |

## Backend contract

```http
GET /admin/v0/model-calls/{modelCallId}
```

- Auth: Conexus admin key (`admin:read`)
- Aggregates: LLM telemetry row + execution journal `provider_call` attempts + redaction warnings
- Provider attempt fields: attempt number, provider/model, status, retryable, error codes, timing, optional per-attempt tokens/cost from intermediate telemetry (`chatcmpl-{requestId}-attempt-{n}`)
- Fallback chain: primary route from journal metadata + per-attempt labels
- No raw prompts/completions in response (`redactionLevel: hash_only`)

## Frontend

- `/conexus/observability` workbench shows **Model-call detail** when `modelCallId` is in the URL or opened from Recent model calls
- Sections: Summary, Route decision, Provider attempts, Fallback chain, Token/cost, Error classification (when failed), Cross-service links, Evidence export placeholder
- Missing journal/attempts/token usage explained via `redaction.warnings` and section copy

## Validation

```text
conexus-dotnet:
  dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~ModelCallDetailMapping
  dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~ModelCallAdminDetail
  → Passed

ontogony-frontend:
  npx vitest run src/conexus/api/conexusModelCallDetail.test.ts
  npm run typecheck
```

## Known limitations

- Artifact refs empty until raw-payload capture links are indexed on detail (future hardening).
- Route decision explorer UI remains CONEXUS-DEEPEN-003 (detail shows id only).
- Conexus-centered evidence bundle export remains CONEXUS-DEEPEN-005/006 (workbench generic export only).

## Next expected item

**CONEXUS-DEEPEN-003** — Route decision explorer (enrich admin GET + UI).
