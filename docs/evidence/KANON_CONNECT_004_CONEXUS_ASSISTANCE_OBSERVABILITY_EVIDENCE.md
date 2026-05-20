# KANON-CONNECT-004 — Conexus assistance observability proof

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (unit + catalog contract tests; Docker Playwright optional on operator machine)  
**Statement:** Kanon Conexus assistance decisions retain model-call correlation fields; the operator workbench links to Conexus observability and Evidence Spine; Evidence Spine resolves `kanonDecisionId` with a Conexus model-call placeholder and partial graph when Conexus detail is unavailable. Draft-only / non-authoritative boundaries unchanged.

## Summary

Proved end-to-end observability from Kanon `conexus_assistance` decision records (`outputRef` = `modelInvocationId`) through frontend drilldown links and Evidence Spine deep-graph resolution, including honest unavailable states when `modelInvocationId` is absent and partial graphs when Conexus returns 404.

## Repos touched

| Repo | Change |
| --- | --- |
| `kanon-dotnet` | Strengthened `ConexusAssistanceApiTests` decision-record assertions (outputRef, assistance type, fingerprints, trace/correlation) |
| `ontogony-frontend` | Assistance adapters + workbench links; `extractConexusModelCallIdFromDecision`; Evidence Spine partial Conexus resolve; catalog cross-service links |
| `ontogony-platform` | This evidence file |

## Correlation chain

```text
POST /ontology/v0/conexus/assistance/*
  → modelInvocationId on response
  → conexus_assistance decision record (outputRef, policyIds, fingerprints, trace/correlation)
  → /kanon/assistance workbench links
       → /conexus/observability?modelCallId=…
       → /system/evidence-spine?kind=conexusModelCallId&id=…
       → /kanon/decisions?decisionId=…
  → Evidence Spine resolve by kanonDecisionId
       → conexus.modelCall placeholder from outputRef
       → optional deep Conexus graph (partial if admin model-call 404)
```

## Tests

```powershell
cd C:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj --filter "FullyQualifiedName~ConexusAssistanceApiTests"

cd C:\dev\ontogony-frontend
npm test -- src/kanon/adapters/kanonConexusAssistanceAdapters.test.ts src/kanon/adapters/kanonEvidenceAdapters.test.ts src/conexus/adapters/resolveConexusEvidenceGraph.test.ts src/evidence-spine/resolveEvidenceSpineDeepGraphs.test.ts src/app/route-workflow-catalog.cross-service-links.test.ts
```

| Check | Result |
| --- | --- |
| Decision record retains modelInvocationId, assistance type, ontology version, fingerprints, trace/correlation | PASS (Kanon API tests) |
| `modelInvocationId` present → Conexus + Evidence Spine links | PASS (assistance adapter test) |
| `modelInvocationId` absent → unavailable reason | PASS (assistance adapter test) |
| `conexus_assistance` decision → `conexusModelCallId` in Evidence Spine graph | PASS (kanon evidence adapter test) |
| Conexus model-call 404 with `allowPartial` → placeholder graph, not throw | PASS (Conexus + deep-graph tests) |
| Route catalog cross-service contract for `/kanon/assistance` | PASS |

## Acceptance

- [x] Kanon assistance traceable from semantic decision to Conexus model call when `modelInvocationId` is recorded
- [x] Frontend links to Conexus observability and Evidence Spine
- [x] Evidence Spine includes Conexus model-call node from Kanon decision `outputRef`; partial graph when Conexus detail unavailable
- [x] Remains draft-only; no Conexus output treated as semantic truth
- [ ] Live Docker Playwright assistance → observability hop (operator machine)

## Related

- [KANON-CONNECT-001](./KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md)
- [KANON-DEEPEN-007](./KANON_DEEPEN_007_CONEXUS_ASSISTANCE_WORKBENCH_EVIDENCE.md)
- [KANON-CONNECT-005](./KANON_CONNECT_005_EVIDENCE_SPINE_SEMANTIC_GRAPH_EVIDENCE.md) — Evidence Spine imports Kanon semantic graph
- Next: **KANON-CONNECT-006** — route/OpenAPI/catalog parity gate
