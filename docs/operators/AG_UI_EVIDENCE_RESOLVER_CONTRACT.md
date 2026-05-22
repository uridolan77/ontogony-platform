# AG-UI evidence resolver contract

**Sprint:** ADAPTER-AGUI-002 — Cross-system AG-UI evidence resolver  
**Status:** Canonical contract for event-scoped Evidence Spine resolution  
**Baseline:** `SYSTEM-ALPHA-006` · `ontogony-agent-interaction-v0`

## Purpose

From a single **AG-UI interaction event**, operators open the full cross-service evidence chain without a new backend aggregator. The frontend wraps the existing Evidence Spine resolver and operator deeplinks.

```text
OntogonyAgentEvent
  → pick primary anchor (event.evidence, then event.ids, then event family)
  → resolveEvidenceSpine (when operator APIs configured)
  → operator links (Allagma / Kanon / Conexus / Evidence Spine workbench)
  → unresolved reasons for missing or unreachable ids
```

AG-UI events remain **mechanical projections**. Evidence Spine and service pages remain **authoritative**.

## Relationship to other contracts

| Contract | Role |
| --- | --- |
| [`AGENT_INTERACTION_SPINE_CONTRACT.md`](./AGENT_INTERACTION_SPINE_CONTRACT.md) | Event/session shapes |
| [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./SYSTEM_EVIDENCE_SPINE_CONTRACT.md) | Spine resolver, taxonomy, export bundle |
| [`AG_UI_COMPATIBILITY_ADAPTER.md`](./AG_UI_COMPATIBILITY_ADAPTER.md) | Ontogony ↔ AG-UI wire mapping |
| [`ontogony-agent-interaction-evidence-graph-v0.schema.json`](../schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json) | Resolver result JSON Schema |

Do **not** duplicate HTTP routes here. Use [`system-evidence-spine-resolution.matrix.json`](../system/system-evidence-spine-resolution.matrix.json) and service operator docs.

## Authority boundaries

| Layer | Owns | Must not own |
| --- | --- | --- |
| Allagma | Run, audit, replay | Semantic truth |
| Kanon | Decision, provenance, semantic graph, human gate | Model routing |
| Conexus | Model call, route decision, evidence bundle | Ontology meaning |
| Platform | This contract + result schema | Runtime aggregation API (v0) |
| Frontend | `resolveAgentInteractionEvidenceGraph` | New Kanon/Conexus AG-UI export routes |

## Anchor selection (v0)

Priority:

1. `event.evidence[]` — first link with `identifierKind` + `identifierValue`
2. `event.ids` — by event family (see implementation `pickAgentInteractionEvidenceAnchor`)
3. No anchor → links-only mode with explicit unresolved `anchor`

| Event family | Preferred anchor kinds |
| --- | --- |
| `human_gate`, `policy` | `humanGateId`, `kanonDecisionId`, `planningDecisionId`, `allagmaRunId` |
| `model` | `conexusModelCallId`, `conexusRouteDecisionId`, `allagmaRunId` |
| `run`, `tool`, `error` | `allagmaRunId`, `conexusModelCallId` |
| `evidence` | `allagmaRunId`, `traceId`, `kanonDecisionId` |

## Resolve modes

| Mode | When | Spine HTTP |
| --- | --- | --- |
| `live` | Anchor known + operator settings configured + `resolveEvidenceSpine` succeeds | Yes |
| `links-only` | APIs unconfigured, `resolveLiveSpine: false`, or resolver error | No |
| `fixture` | `event.source === fixture` and live spine skipped | No |

## Operator links (always)

Even without live spine, emit static deeplinks when ids are present:

- Allagma: run detail, audit, replay lookup
- Kanon: decision, human-gate policy, semantic graph
- Conexus: observability, route decision
- Evidence Spine workbench (trace / correlation / primary anchor)
- `event.evidence[]` links preserved as-is

## Redaction

- Do not surface raw prompts, completions, hidden reasoning, or provider secrets in the resolver drawer.
- Spine node list uses labels, summaries, and page links only (no `rawPreview` in AG-UI graph result).

## Implementation

| Repo | Path |
| --- | --- |
| `ontogony-frontend` | `src/agent-interaction/evidence/resolveAgentInteractionEvidenceGraph.ts` |
| `ontogony-frontend` | `src/agent-interaction/evidence/pickAgentInteractionEvidenceAnchor.ts` |
| `ontogony-frontend` | `src/agent-interaction/components/AgentInteractionEvidenceGraphAction.tsx` |
| `ontogony-frontend` | `src/agent-interaction/components/AgentInteractionEvidenceGraphDrawer.tsx` |

## Non-goals (v0)

- No WebSocket or reverse AG-UI ingress
- No new Kanon AG-UI stream or Conexus AG-UI export endpoint
- No server-side graph aggregation API
- Kanon review-queue UI synthesis in the workbench (library projection only until a dedicated FE slice)

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1

cd C:\dev\ontogony-frontend
npm run test -- src/agent-interaction/evidence/
```

Evidence: [`PLAT_AGUI_RESOLVER_002_EVIDENCE.md`](../evidence/PLAT_AGUI_RESOLVER_002_EVIDENCE.md), [`OFE_AGUI_RESOLVER_002_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_AGUI_RESOLVER_002_EVIDENCE.md).
