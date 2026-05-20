# EVIDENCE-SPINE-000 — Current state audit evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (audit complete; implementation not started)  
**Statement:** Docs-only cross-repo audit of frontend correlation resolver, evidence journeys, and Allagma/Conexus/Kanon lookup APIs. No `src/` changes in this step.

## Scope

`EVIDENCE-SPINE-000` from `docs/_incoming/Ontogony-Cross-Service-Evidence-Spine-Package-v1/prompts/EVIDENCE-SPINE-000_CURRENT_STATE_AUDIT.md`.

Repos reviewed:

- `ontogony-frontend` — resolver, UI, clients, tests, e2e
- `allagma-dotnet` — `Allagma.Api/Program.cs`, query parameters
- `conexus-dotnet` — admin model-call, route-decision, diagnostics endpoints
- `kanon-dotnet` — `Kanon.Api/Program.cs` decision/provenance routes
- `ontogony-platform` — correlation/trace mechanics (no unified resolve API)

## Delivered

| Artifact | Path |
| --- | --- |
| Audit report | [`docs/reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md`](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md) |
| This evidence file | `docs/evidence/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md` |

## Audit conclusions (summary)

```text
Exists:     resolveTraceCorrelation (trace, runId, decisionId, modelCallId)
            CrossServiceLinksCard, TraceCorrelationLookupBar, URL sync
            Page-local evidence journeys (Allagma, Conexus, Kanon)
            Backend GET routes for run, eval, baseline, model-call, route-decision, decision

Missing:    Unified paste-any-ID resolver and EvidenceGraph
            Resolver roots: evalId, baselineComparisonId, routeDecisionId, correlationId, humanGateId
            Recorded source attempts on every lookup (partial: buildCorrelationSourceEvidence on replay only)
            Unified cross-service export bundle

Next slice: EVIDENCE-SPINE-001 (taxonomy + graph types + parser)
            EVIDENCE-SPINE-002 (resolveEvidenceSpine.ts)
```

## Key file references (verification)

| Check | Path |
| --- | --- |
| Trace resolver | `ontogony-frontend/src/system/correlation/resolveTraceCorrelation.ts` |
| Lookup hook | `ontogony-frontend/src/system/correlation/useTraceCorrelation.ts` |
| Types / inputs | `ontogony-frontend/src/system/correlation/correlationTypes.ts` |
| Page links | `ontogony-frontend/src/system/correlation/correlationAdapters.ts` |
| Hint merge | `ontogony-frontend/src/system/correlation/enrichTraceCorrelationView.ts` |
| Cross-service card | `ontogony-frontend/src/shared/components/CrossServiceLinksCard.tsx` |
| URL lookup bar | `ontogony-frontend/src/shared/components/TraceCorrelationLookupBar.tsx` |
| Run evidence journey | `ontogony-frontend/src/allagma/adapters/buildRunEvidenceJourneyLinks.ts` |
| Conexus workbench | `ontogony-frontend/src/conexus/components/ConexusRequestObservabilityWorkbench.tsx` |
| Allagma client | `ontogony-frontend/src/allagma/api/allagmaClient.ts` |
| Conexus client | `ontogony-frontend/src/conexus/api/conexusClient.ts` |
| Kanon client | `ontogony-frontend/src/kanon/api/kanonClient.ts` |
| Allagma routes | `allagma-dotnet/src/Allagma.Api/Program.cs` |
| Allagma run query | `allagma-dotnet/src/Allagma.Api/RunQueryParameters.cs` |
| Conexus model calls | `conexus-dotnet/src/Conexus.Api/Endpoints/ModelCallAdminEndpoints.cs` |
| Conexus route decisions | `conexus-dotnet/src/Conexus.Api/Endpoints/RouteDecisionAdminEndpoints.cs` |
| Conexus execution run | `conexus-dotnet/src/Conexus.Api/Endpoints/DiagnosticsAdminEndpoints.cs` |
| Kanon decisions | `kanon-dotnet/src/Kanon.Api/Program.cs` |
| Resolver unit tests | `ontogony-frontend/src/system/correlation/resolveTraceCorrelation.test.ts` |
| E2E correlation | `ontogony-frontend/e2e/correlation.spec.ts` |
| Package taxonomy (intake) | `ontogony-platform/docs/_incoming/.../04_IDENTIFIER_TAXONOMY.md` |

## Confirmed resolver behavior

- **Inputs:** `traceId`, `runId`, `decisionId`, `modelCallId` only.
- **APIs called:** Allagma run/events/list-by-trace; Kanon decision + by-trace (first match); Conexus execution-run by request id.
- **404:** Treated as unresolved inside `safe()` for Allagma/Kanon/Conexus execution-run lookup.
- **humanGateId:** Extracted from run events after run resolution; no GET-by-gate-id.
- **correlationId:** Set from Kanon decision when present; no correlation-first path.
- **eval / baseline / route decision:** Not handled by trace resolver (handled on individual pages or journey builders only).

## Confirmed backend lookup (direct GET)

| Service | Route pattern |
| --- | --- |
| Allagma | `/allagma/v0/runs/{runId}`, `/runs?traceId&correlationId&waitingForHumanGate`, `/evaluations/{id}`, `/evaluations/{id}/evidence`, `/evaluations/baseline-comparisons/{id}` |
| Conexus | `/admin/v0/model-calls/{id}`, `/model-calls?filters`, `/route-decisions/{id}`, `/diagnostics/execution-runs/by-request-id/{requestId}` |
| Kanon | `/ontology/v0/decision-records/{id}`, `/by-trace/{traceId}`, `/provenance`, `/replay-bundles` |

## Test inventory

| Layer | Present | Gap |
| --- | --- | --- |
| Unit — resolver | 8 cases in `resolveTraceCorrelation.test.ts` | No eval/baseline/route-decision roots |
| Unit — journey | `buildRunEvidenceJourneyLinks.test.ts` | Not integrated with unified resolver |
| E2E — correlation | `correlation.spec.ts`, `trace-url-sync.spec.ts` | No eval-id / baseline-id / correlation-id lookup |
| E2E — journeys | `allagma-run-detail-evidence-journey.spec.ts`, `allagma-replay-evidence.spec.ts` | Mocked APIs only |

## First implementation slice (GO)

1. **EVIDENCE-SPINE-001** — Types, parser, graph helpers, platform taxonomy doc.
2. **EVIDENCE-SPINE-002** — `resolveEvidenceSpine.ts` with multi-root resolution and source-attempt log.

No backend route required for 002 start unless implementation discovers a new blocker.

## Validation performed

```text
- Read frontend correlation module and shared components
- Read Allagma/Conexus/Kanon API clients and journey builders
- Inventoried Allagma.Api and Kanon.Api MapGet routes from Program.cs
- Inventoried Conexus admin endpoints (ModelCall, RouteDecision, Diagnostics)
- Grepped unit and e2e tests for correlation, journey, export
- Compared findings to package 04_IDENTIFIER_TAXONOMY and 06_API_CONTRACT_GAP_MATRIX
- Wrote audit + evidence markdown (docs-only)
```

## Commands not run

No `dotnet build`, `npm test`, or Playwright execution in this audit step (docs-only per prompt). Implementation PRs should run focused tests per slice.
