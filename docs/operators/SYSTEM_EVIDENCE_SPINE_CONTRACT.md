# System evidence spine contract

**Sprint:** SYS-TIGHT-002 — Ontogony Tight Integration Baseline  
**Status:** Canonical cross-repo contract index (does not replace shipped Evidence Spine implementation)  
**Baseline:** `SYSTEM-ALPHA-006`

## Purpose

Operators paste one identifier and expect a **single governed execution graph** across Allagma (runtime), Kanon (semantic authority), and Conexus (model gateway). This document is the **system-level contract index** for that behavior. Implementation is **client-side** in the operator console; there is no backend unified resolve API in v1.

## Authority boundaries

| Service | Owns | Must not own |
| --- | --- | --- |
| Allagma | Run lifecycle, audit bundle, eval/baseline ids on runs | Semantic truth, provider routing |
| Kanon | Decision records, provenance, semantic graph, human-gate decisions | Model calls, workflow execution |
| Conexus | Model-call evidence, route decisions, quota/usage | Ontology meaning |
| Ontogony.Platform | Taxonomy, export bundle schema, resolution matrix | Product runtime state |
| Frontend | `resolveEvidenceSpine`, workbench UI | Backend authority |

## Canonical artifacts

| Artifact | Repo | Role |
| --- | --- | --- |
| **This document** | `ontogony-platform` | Contract index and acceptance rules |
| [`EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](./EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) | Platform | Identifier kinds, parser rules, missing-edge codes |
| [`system-evidence-spine-resolution.matrix.json`](../system/system-evidence-spine-resolution.matrix.json) | Platform | Machine-readable identifier → HTTP route map |
| [`ontogony-cross-service-evidence-spine-bundle-v1.schema.json`](../schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json) | Platform | Export bundle JSON Schema |
| [`resolveEvidenceSpine.ts`](../../../ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts) | Frontend | Unified resolver (v1) |
| [`KANON_EVIDENCE_SPINE_ENTRYPOINTS.json`](../../../kanon-dotnet/docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json) | Kanon | Kanon handoff routes (additive v0) |
| [`MODEL_CALL_EVIDENCE_FLOW.md`](../../../conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md) | Conexus | Conexus model-call evidence sequence |

## Program history (do not redo)

Cross-service Evidence Spine **000–009** closed 2026-05-20 (evidence in owning repos). SYS-TIGHT-002 **indexes and gates** that work; it does not reopen EVIDENCE-SPINE implementation slices.

## Required identifier roots (SYS-TIGHT-002)

These kinds must resolve without throwing; missing downstream data becomes **explicit unresolved edges**:

| Kind | Owner | Primary expansion |
| --- | --- | --- |
| `allagmaRunId` | Allagma | Run → events/audit → Kanon decisions → Conexus model calls |
| `kanonDecisionId` | Kanon | Decision → provenance → semantic graph |
| `conexusModelCallId` | Conexus | Admin detail → evidence-links → evidence-bundle → route decision |
| `conexusRouteDecisionId` | Conexus | Route decision detail |
| `traceId` | Cross-service | Per-service trace discovery + spine merge |
| `correlationId` | Cross-service | Allagma/Conexus list filters |
| `humanGateId` | Kanon / Allagma | Run events + semantic graph |
| `domainPackId` | Kanon | Semantic graph + pack evolution decisions |

Additional supported roots (Evidence Spine v1): `allagmaEvaluationRunId`, `baselineComparisonId`, `planningDecisionId`, `sourceBindingId`, `datasetId`, `scenarioId`.

## Resolution algorithm (summary)

1. **Parse** pasted value → `ParsedEvidenceIdentifier` (never prefix-only).
2. **Record** every HTTP attempt as `EvidenceSourceAttempt` (`success` | `not_found` | `error` | `skipped`).
3. **Build graph** with depth/node/API limits (see taxonomy).
4. **Deep graphs** optional via `resolveEvidenceSpineDeepGraphs` for audit bundle and Conexus admin surfaces.
5. **Never crash** on missing downstream ids — emit `unresolved_expected_link` edges with stable reason codes.

Full algorithm: Evidence Spine package `05_RESOLUTION_ALGORITHM.md` (intake archive) and frontend source.

## Operator surface

| Surface | Location |
| --- | --- |
| Workbench | `/system/evidence-spine` |
| Embedded panels | Allagma run/eval detail, Conexus observability, Kanon provenance |
| Export | Redacted bundle validated against platform schema |

## Acceptance (SYS-TIGHT-002)

- [x] Contract index published (this document).
- [x] Machine resolution matrix published and validated.
- [x] Taxonomy cross-linked; Kanon handoff + Conexus evidence flow referenced.
- [x] Frontend resolver tests cover required identifier roots (see `resolveEvidenceSpine.test.ts`).
- [x] Missing data is non-fatal (unresolved edges + source attempts).
- [ ] Unified **run-centric audit page** (SYS-TIGHT-003) — out of scope here.

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-evidence-spine-contract.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~SystemEvidenceSpineContractTests"
```

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/evidence-spine/evidenceSpineContractMatrix.test.ts
```

## Related

- [`AGENT_INTERACTION_SPINE_CONTRACT.md`](./AGENT_INTERACTION_SPINE_CONTRACT.md) — temporal interaction event log (PLAT-AGUI-000); complements this graph contract
- [`TRACE_CORRELATION_CONTRACT.md`](./TRACE_CORRELATION_CONTRACT.md) — legacy trace resolver; subsumed for spine roots
- [`allagma-dotnet/docs/system/allagma-feature-connection.matrix.json`](../../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json) — runtime endpoint catalog
- [`SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md`](../evidence/SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md)
