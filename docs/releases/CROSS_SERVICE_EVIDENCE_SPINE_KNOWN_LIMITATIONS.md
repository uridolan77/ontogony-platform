# Cross-service evidence spine — known limitations

Consolidated from EVIDENCE-SPINE-000 through EVIDENCE-SPINE-009. Not a production readiness statement.

## Resolver scope (v1)

- **Client-side only** — `resolveEvidenceSpine` orchestrates existing service GET routes; there is no platform or Allagma “resolve graph” aggregator API.
- **Expansion limits:** `maxDepth: 3`, `maxNodes: 50`, `maxApiCalls: 30` — very large runs may truncate with partial completeness.
- **`datasetId` / `scenarioId`:** Parsed and documented in taxonomy; **not** resolved in v1 (`resolveEvidenceSpine` has no cases for these kinds).
- **`includeExports` / `includeEvents` / `includeRelated`:** Present on `EvidenceLookupInput` for forward compatibility; v1 does not automatically attach audit/evidence-export nodes unless expanded in a follow-up.
- **Ambiguous UUID-shaped IDs:** Parser marks ambiguity; resolver tries priority-ordered safe lookups and may emit `ambiguous_identifier` warnings.

## Identifier-specific gaps

| Kind | Limitation |
| --- | --- |
| `humanGateId` | No direct GET; resolver scans Allagma runs by trace/correlation heuristics—may be slow or empty if gate is orphaned |
| `correlationId` | Correlation-first expansion is limited compared to trace/run roots |
| `conexusModelCallId` | Model-call-only root may not reach run/decision without execution-run `traceId` bridge |
| `actionEvaluationDecisionId` | Slot exists in legacy correlation types; no OpenAPI lookup—always empty |
| Kanon provenance | `403` on provenance → `forbidden` attempt, not service-down; graph may lack provenance edges |

## UI and visualization

- Workbench is **structured lists** (nodes, edges, attempts)—not an interactive force-directed graph canvas.
- Legacy **trace correlation** bar (four fields) remains on System overview and Conexus observability; operators may use either surface until consolidation is prioritized.
- Page-local evidence panels (Allagma/Conexus/Kanon) show **scoped** subgraphs; full cross-service graph requires the workbench route.

## Export and redaction

- Default export **excludes** raw prompts, completions, API keys, and oversized DTOs.
- Optional “include raw previews” is operator-opt-in; still subject to client-side redaction helpers—not a guarantee of full payload fidelity.
- Bundle validates against platform schema; **runtime export from browser** is not re-validated server-side on download.

## Operator environment

- **Conexus model-call admin on Docker:** ENV-SEED-001 records model-call IDs from Allagma, but `GET /admin/v0/model-calls/{id}` may return **404** if `conexus-api` image predates CONEXUS-DEEPEN admin routes or persistence is misaligned (see [009A evidence](../evidence/EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md)).
- Docker-local tokens and fake providers are **local posture** only.
- Live graph quality requires seeded Allagma runs, Conexus model calls, and Kanon decisions with consistent trace/correlation IDs.
- Frontend image must be rebuilt with provenance env args before trusting manual browser checks (`verify-frontend-browser-provenance.ps1 -Build`).
- Stale `ontogony-frontend` or backend images may show 404 on newer admin routes even when source and unit tests pass.

## Cross-program boundaries

- Evidence spine does **not** execute Allagma retry/cancel/replay (see ALLAGMA-ACTION package).
- Conexus provider inventory is not a per-request graph node in v1.
- Kanon semantic authority rules unchanged—spine displays decisions, does not evaluate actions.
