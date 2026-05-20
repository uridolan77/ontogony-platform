# CONEXUS-EVIDENCE-FLOW-001 — Model-call evidence operator flow

**Date:** 2026-05-20  
**Issue:** CONEXUS-EVIDENCE-FLOW-001 (Alpha-005 delta package)  
**Runtime baseline:** **SYSTEM-ALPHA-006**  
**Verdict:** **PASS** — backend flow tests + frontend catalog/spine integration + Docker cross-service proof via Kanon connect 007 step 4.

**Non-claims:** Not production readiness. Raw prompts/completions are not exposed via these routes or operator export bundles.

## Problem

Conexus documents a deterministic operator flow from chat completion to route decision, model-call detail, evidence links, bundle, usage/cost, and trace slots. Frontend observability and Evidence Spine must use the same spine without leaking raw LLM payloads.

## Canonical backend flow

| Doc | Path |
| --- | --- |
| Operator sequence | [`conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`](../../../conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md) |
| Integration tests | [`conexus-dotnet/tests/Conexus.Api.Tests/ModelCallEvidenceOperatorFlowTests.cs`](../../../conexus-dotnet/tests/Conexus.Api.Tests/ModelCallEvidenceOperatorFlowTests.cs) |

```powershell
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Api.Tests --filter "FullyQualifiedName~ModelCallEvidenceOperatorFlow"
```

## Frontend integration

| Surface | Role |
| --- | --- |
| `/conexus/observability` | Lookup tab: `ConexusModelCallDetailPanel` (admin detail + evidence-links + route decision + spine panel + redacted export) |
| `/conexus/observability` | Usage tab + Recent: `ConexusUsageCostWorkbench` + list drill-down → modelCallId |
| `/system/evidence-spine` | `resolveConexusModelCallEvidenceGraph` / `resolveConexusRouteDecisionEvidenceGraph` |
| `/conexus/chat` | Creates completion; deep-link `?modelCallId=` → observability |
| `/kanon/assistance` | `modelInvocationId` → Conexus observability + Evidence Spine |
| `/allagma/runs/:runId` | Eval topology `modelCallId` → Conexus observability |

Machine-readable flow map: [`ontogony-frontend/src/conexus/operators/conexusModelCallEvidenceFlow.ts`](../../../ontogony-frontend/src/conexus/operators/conexusModelCallEvidenceFlow.ts)

Route catalog: [`ontogony-frontend/src/app/route-workflow-catalog.json`](../../../ontogony-frontend/src/app/route-workflow-catalog.json) (`/conexus/observability`, `/system/evidence-spine`)

## ID consistency (acceptance)

Given `modelCallId` from Allagma eval or Kanon assistance (`outputRef` / `modelInvocationId`):

| Identifier | Aligns across |
| --- | --- |
| `modelCallId` | Completion `id`, admin detail, evidence-links, export bundle, Evidence Spine graph |
| `routeDecisionId` | Project evidence, admin detail, links, route explorer, spine edges |
| `traceId` / `correlationId` | Detail, evidence-links slots, trace correlation panel |
| Usage/cost | Model-call detail tokens + usage tab + `GET /admin/v0/usage-cost` drill-down |

Redaction: server admin bundle omits raw prompts/completions; FE `buildConexusModelCallEvidence` applies `operatorRedaction` on export.

## Validation

```powershell
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Api.Tests --filter "FullyQualifiedName~ModelCallEvidenceOperatorFlow"

cd C:\dev\ontogony-frontend
npm test -- src/conexus/operators/conexusModelCallEvidenceFlow.catalog.test.ts
npm test -- src/conexus/adapters/conexusEvidenceSpineAdapters.test.ts src/conexus/adapters/resolveConexusEvidenceGraph.test.ts
npm test -- src/evidence-spine/resolveEvidenceSpineDeepGraphs.test.ts
npm test -- src/app/route-workflow-catalog.cross-service-links.test.ts
```

### Docker-local cross-service (companion)

Kanon connect 007 step 4 (assistance → Conexus observability) and step 5 (Evidence Spine) exercise this flow on compose:

- [KANON_CONNECT_007_DOCKER_CROSS_SERVICE_SMOKE_EVIDENCE.md](./KANON_CONNECT_007_DOCKER_CROSS_SERVICE_SMOKE_EVIDENCE.md)
- [KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md](./KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md)

```powershell
cd C:\dev\ontogony-frontend
npm run docker:smoke:kanon-connect-007
```

## Related deepening / spine work

- [CONEXUS_DEEPEN_005_CROSS_SERVICE_EVIDENCE_SPINE_EVIDENCE.md](./CONEXUS_DEEPEN_005_CROSS_SERVICE_EVIDENCE_SPINE_EVIDENCE.md)
- [CONEXUS_DEEPEN_006_FRONTEND_OBSERVABILITY_V2_EVIDENCE.md](./CONEXUS_DEEPEN_006_FRONTEND_OBSERVABILITY_V2_EVIDENCE.md)
- `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_004_CONEXUS_REQUEST_ROUTE_LINKING_EVIDENCE.md`

## Post-lock context

Route preview and quota status APIs landed in the same Conexus window as evidence-flow hardening; classification: [CONEXUS_POSTLOCK_001_CLASSIFICATION.md](../../../conexus-dotnet/docs/evidence/CONEXUS_POSTLOCK_001_CLASSIFICATION.md).

## Issue card

`docs/_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20/issue-cards/CONEXUS-EVIDENCE-FLOW-001.md`
