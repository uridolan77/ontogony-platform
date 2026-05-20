# CONEXUS-DEEPEN-000 — Current state audit evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (audit complete; implementation not started)  
**Statement:** Docs-only audit of Conexus observability APIs, telemetry persistence, frontend consumers, and cross-service trace/correlation. No platform `src/` changes in this step.

## Scope

Platform coordination record for `CONEXUS-DEEPEN-000` from `Ontogony-Conexus-Deep-Enhancement-Package-v1` (intake in `conexus-dotnet/docs/_incoming/`). Primary backend audit lives in `conexus-dotnet`; platform cross-cut covers trace/correlation and local stack only.

## Delivered

| Repo | Artifact |
| --- | --- |
| `conexus-dotnet` | [`docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md`](https://github.com/uridolan77/conexus-dotnet/blob/main/docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md) — full API, persistence, frontend, gap matrix |
| `ontogony-platform` | [`docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md) — cross-service spine + platform touchpoints |
| `ontogony-platform` | This evidence file |

Package intake: `conexus-dotnet/docs/_incoming/Ontogony-Conexus-Deep-Enhancement-Package-v1/`

## Audit conclusions (summary)

```text
Foundation:     LLM request/response telemetry, route decisions, execution journal, diagnostics admin APIs
Missing:        Admin list-first model-call lifecycle — no GET /admin/v0/model-calls
Partial:        GET /conexus/v0/model-calls/{modelCallId} (project API key, thin DTO)
                GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
Frontend:       /conexus/observability is ID-first; backend-waiting gate for request list
Next slice:     CONEXUS-DEEPEN-001 — GET /admin/v0/model-calls + recent requests UI
```

**CONEXUS-DEEPEN-001 decision:** **GO** — add new admin list contract; do not reuse project-scoped evidence route alone.

## Key file references (verification)

| Check | Path |
| --- | --- |
| Program endpoint map | `conexus-dotnet/src/Conexus.Api/Program.cs` |
| Project-scoped evidence | `conexus-dotnet/src/Conexus.Api/Endpoints/ModelCallEvidenceEndpoints.cs` |
| Admin diagnostics | `conexus-dotnet/src/Conexus.Api/Endpoints/DiagnosticsAdminEndpoints.cs` |
| Route decision admin | `conexus-dotnet/src/Conexus.Api/Endpoints/RouteDecisionAdminEndpoints.cs` |
| Persistence tables | `conexus-dotnet/src/Conexus.Persistence/ConexusDbContext.cs` |
| EF route evidence query | `conexus-dotnet/src/Conexus.Persistence/EfModelCallRouteEvidenceQuery.cs` |
| Frontend guidance (no list) | `ontogony-frontend/src/conexus/components/ConexusObservabilityGuidanceCard.tsx` |
| Backend-waiting contract | `ontogony-frontend/docs/phase-j-frontend-ui-tightening/backend-waiting/conexus-request-list.md` |
| ID-first workbench | `ontogony-frontend/src/conexus/components/ConexusRequestObservabilityWorkbench.tsx` |
| Trace/correlation contract | `ontogony-platform/docs/operators/TRACE_CORRELATION_CONTRACT.md` |
| Trace evidence (prior) | `ontogony-platform/docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md` |

## Confirmed backend state

- Admin diagnostics: `GET /admin/v0/diagnostics/summary`, `GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}` — **no model-call list**.
- Startup maps admin, routing, route-decision, diagnostics, DB viewer, retention, chat, governance, and **`GET /conexus/v0/model-calls/{modelCallId}`** only.
- `ConexusDbContext` includes `LlmRequests`, `LlmResponses`, `RouteDecisions`, `ExecutionRuns`, `ExecutionAttempts`, artifacts, quota usage, admin audit, idempotency ledger.
- `EfModelCallRouteEvidenceQuery` joins response/request for thin evidence (modelCallId, routeDecisionId, traceId, provider, tokens, cost, latency) — seed for list rows, not sufficient for operator list/detail alone.

## Confirmed frontend state

- `ConexusObservabilityGuidanceCard` states Conexus does not expose a recent-request listing API.
- Backend-waiting expects `GET /admin/v0/diagnostics/requests` or equivalent with filters + cursor pagination.
- Workbench shows execution-run detail only when `modelCallId` (or equivalent request id) is present in lookup params.

## CONEXUS-DEEPEN-001 target (from audit)

```http
GET /admin/v0/model-calls
```

Suggested read port: `IModelCallAdminListQuery` with `EfModelCallAdminListQuery` and `InMemoryModelCallAdminListQuery`. Primary row source: `conexus_llm_response`, join `conexus_llm_request` for parameters (`requested_model`, `routeDecisionId`, fallback, correlation, Allagma metadata). No raw prompts/completions in list DTOs.

Minimum filters and row fields: see `conexus-dotnet/docs/_incoming/.../prompts/CONEXUS-DEEPEN-001_REQUEST_LIFECYCLE_LIST_CONTRACT.md`.

## Validation performed

```text
- Inventoried /admin/v0 routes from Conexus.Api Endpoints/*.cs and Program.cs
- Confirmed absence of admin model-call list mapping
- Reviewed ConexusDbContext, EfModelCallRouteEvidenceQuery, Infrastructure/Persistence DI
- Reviewed frontend observability page, guidance card, backend-waiting contract, workbench lookup
- Cross-checked trace/correlation docs and TRACE_CONTRACT_001 evidence
- No CONEXUS-DEEPEN-001 implementation in this step
```

## Next expected item

**CONEXUS-DEEPEN-001** — `GET /admin/v0/model-calls` (conexus-dotnet) + recent requests section on `/conexus/observability` (ontogony-frontend) + evidence file `CONEXUS_DEEPEN_001_REQUEST_LIFECYCLE_LIST_CONTRACT_EVIDENCE.md` (platform index).

Platform work for 001: lift or retarget frontend backend-waiting gate; optional trace doc addendum after route ships. No new `Ontogony.*` package required unless a shared pagination primitive is later extracted for multiple services.
