# CONEXUS-DEEPEN-005 — Cross-service evidence spine evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (Conexus evidence-links resolver + observability spine UI; polish fixes for 003/004)  
**Statement:** Operators can navigate Conexus ↔ Allagma ↔ Kanon using explicit per-slot links; missing ids are explained, not hidden.

## Scope

`CONEXUS-DEEPEN-005` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`, including review follow-ups **003** (route decision modelCallId) and **004A** (usage drill-down filter correctness).

## Repos touched

| Repo | Change |
| --- | --- |
| `conexus-dotnet` | `GET /admin/v0/model-calls/{modelCallId}/evidence-links`; list `tokenUsage` filter; `tokenUsageAvailable` on list items; route detail prefers `relatedModelCallIds` |
| `ontogony-frontend` | `ConexusEvidenceSpinePanel`, evidence-links client, usage drill-down fixes |
| `ontogony-platform` | This evidence file |

## Backend

### Evidence resolver

```http
GET /admin/v0/model-calls/{modelCallId}/evidence-links
```

Returns `ModelCallEvidenceLinksResponse` with Conexus-native slots (`modelCallId`, `requestId`, `traceId`, `correlationId`, `routeDecisionId`, `executionRunId`). Missing route decisions are **not** linked with synthetic ids (`routeDecisionInferred` + slot `unavailableReason`).

### Review polish (004A)

- `GET /admin/v0/model-calls?tokenUsage=present|missing` filters on captured token counts.
- `ModelCallListItem.tokenUsageAvailable` exposed for operators.
- Usage workbench drill-down: successful window uses `status=completed`; missing-token drill uses `tokenUsage=missing`.

### Review polish (003)

- `RouteDecisionDetailResponse.modelCallId` prefers first `relatedModelCallIds` telemetry id; falls back to `chatcmpl-{requestId}` only when empty.

## Frontend

- Model-call detail embeds **Cross-service evidence spine** (`ConexusEvidenceSpinePanel`): Conexus slots from evidence-links API + Allagma/Kanon slots from live `useTraceCorrelation`.
- Per-slot unavailable copy when values cannot be resolved.
- Allagma journey links (existing) remain; route decision explorer links use stored `routeDecisionId` when present.

## Validation

```text
conexus-dotnet:
  dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~ModelCallEvidence
  dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~RouteDecisionDetail
  dotnet test tests/Conexus.Api.Tests --filter FullyQualifiedName~ModelCallAdmin

ontogony-frontend:
  npx vitest run src/conexus/adapters/conexusEvidenceSpineAdapters.test.ts
  npx vitest run src/conexus/adapters/conexusUsageAdapters.test.ts
```

## Review polish (005A)

- When no Kanon decision id is resolved but trace id is known, spine shows **Kanon decisions (trace)** with a working trace-scoped search link.

## Known limitations

- Allagma/Kanon ids still require trace correlation (not stored on Conexus telemetry rows).
- Evidence export bundle schema remains **CONEXUS-DEEPEN-006**.
- Full-repo `npm run typecheck` may fail on unrelated Allagma UI variant typing until fixed separately.

## Next expected item

**CONEXUS-DEEPEN-006** — Frontend observability v2 (tabs, exports, empty states).
