# AGUI-SPINE-CLOSEOUT-001 — Interaction spine acceptance closeout

**Date:** 2026-05-22  
**Milestone:** `ONTOGONY-AGUI-000 — Agent Interaction Spine Baseline`  
**Verdict:** **ACCEPTED (local implementation)** — evidence-backed; **not merged/CI-closed** until PRs land green.

## What this closeout covers

Formal sign-off of package acceptance criteria ([`13_ACCEPTANCE_CHECKLIST.md`](../operators/AG_UI_INTERACTION_SPINE_ACCEPTANCE.md)) against workspace evidence. This is a **docs-only** slice; no runtime code changes.

## Scope boundaries

| Layer | Status | Notes |
| --- | --- | --- |
| **Original 10-PR spine** (`05_REPO_PR_SEQUENCE.md`) | Complete (local) | PLAT-AGUI-000 through ADAPTER-AGUI-001 |
| **Follow-up polish** | Complete (local) | OFE-AGUI-004 live SSE consumer — **not** in the 10-PR sequence |
| **Intentional v0 limitations** | Documented | See [limitations](#intentional-v0-limitations) |
| **Remaining work** | PR / CI / merge only | No open spine architecture items |

## PR spine map (10 + follow-up)

| # | ID | Repo | Evidence |
| --- | --- | --- | --- |
| 1 | PLAT-AGUI-000 | ontogony-platform | [`PLAT_AGUI_000_EVIDENCE.md`](./PLAT_AGUI_000_EVIDENCE.md) |
| 2 | OUI-AGENT-001 | ontogony-ui | [`05_REPO_PR_SEQUENCE.md`](../_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/05_REPO_PR_SEQUENCE.md) §2; `ontogony-ui` `./agent` export |
| 3 | OFE-AGUI-001 | ontogony-frontend | `src/agent-interaction/` module + fixtures (this closeout § Frontend) |
| 4 | OFE-AGUI-002 | ontogony-frontend | `synthesizeInteractionEvents` + adapters (this closeout § Frontend) |
| 5 | ALLAGMA-AGUI-001 | allagma-dotnet | [`ALLAGMA_AGUI_001_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/ALLAGMA_AGUI_001_EVIDENCE.md) |
| 6 | KANON-HITL-AGUI-001 | kanon-dotnet | [`KANON_HITL_AGUI_001_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_HITL_AGUI_001_EVIDENCE.md) |
| 7 | CONEXUS-AGUI-001 | conexus-dotnet | [`CONEXUS_AGUI_001_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_AGUI_001_EVIDENCE.md) |
| 8 | OFE-AGUI-003 | ontogony-frontend | `AgentInteractionWorkbench` + embedded panels (this closeout § Frontend) |
| 9 | ALLAGMA-AGUI-002 | allagma-dotnet | [`ALLAGMA_AGUI_002_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/ALLAGMA_AGUI_002_EVIDENCE.md) |
| 10 | ADAPTER-AGUI-001 | platform + frontend | [`PLAT_AGUI_ADAPTER_001_EVIDENCE.md`](./PLAT_AGUI_ADAPTER_001_EVIDENCE.md), [`OFE_ADAPTER_AGUI_001_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_ADAPTER_AGUI_001_EVIDENCE.md) |
| — | **OFE-AGUI-004** (follow-up) | ontogony-frontend | [`OFE_AGUI_004_SSE_CONSUMER_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_AGUI_004_SSE_CONSUMER_EVIDENCE.md) |
| — | **OFE-AGUI-005** (follow-up) | ontogony-frontend | [`OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md) |

## Continuation plan (`15_CONT_PLAN.md`)

| # | ID | Status | Evidence |
| --- | --- | --- | --- |
| 4 | KANON-AGUI-REVIEW-001 | ✅ Complete (local) | [`KANON_AGUI_REVIEW_001_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_AGUI_REVIEW_001_EVIDENCE.md) |
| 5 | CONEXUS-AGUI-002 | ✅ Complete (local) | [`CONEXUS_AGUI_002_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_AGUI_002_EVIDENCE.md) |
| 6 | ADAPTER-AGUI-002 | ⏳ Not started | Cross-system evidence resolver from AG-UI events |

## Acceptance checklist summary

Full ticked checklist: [`docs/operators/AG_UI_INTERACTION_SPINE_ACCEPTANCE.md`](../operators/AG_UI_INTERACTION_SPINE_ACCEPTANCE.md).

All sections **PASS** against local evidence except merge/CI gates (explicitly out of scope for this doc).

## Cross-repo validation (local)

```powershell
# Platform contract + schemas
cd C:\dev\ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1

# Platform adapter package
cd packages\ontogony-agent-interaction
npm run test

# Allagma projection + SSE
cd C:\dev\allagma-dotnet
dotnet test tests\Allagma.Tests --filter FullyQualifiedName~AgentRunInteraction

# Kanon HITL + review queue projection
cd C:\dev\kanon-dotnet
dotnet test tests\Kanon.Tests --filter "FullyQualifiedName~HumanGateInteraction|FullyQualifiedName~OperatorReviewQueueInteraction"

# Conexus model-call lifecycle projection
cd C:\dev\conexus-dotnet
dotnet test tests\Conexus.Application.Tests --filter FullyQualifiedName~ModelCallInteractionEventProjector
dotnet test tests\Conexus.Api.Tests --filter FullyQualifiedName~ModelCallInteractionEventExport

# Frontend module + SSE consumer
cd C:\dev\ontogony-frontend
npm run typecheck
npm run test -- src/agent-interaction/ src/allagma/api/parseAllagmaInteractionSse.test.ts src/allagma/api/streamAllagmaRunInteractionEvents.test.ts

# UI agent export
cd C:\dev\ontogony-ui
npm run check:exports
npm run test:run -- src/components/agent/
```

## System-level acceptance (operator journeys)

| Journey | Evidence |
| --- | --- |
| `/system/agent-interaction?runId=…` timeline | `AgentInteractionWorkbenchPage`, live stream via OFE-AGUI-004 |
| Lookup by `modelCallId`, `decisionId`, `traceId`, `humanGateId` | `agentInteractionLinks.ts`, `AgentInteractionLookupBar` |
| Missing links shown as unresolved | `missingReasonCode` on evidence links; `AgentInteractionCrossLinksPanel` |
| JSONL offline replay | `jsonlFixtureAdapter`, import/export on workbench |
| AG-UI adapter tests | `packages/ontogony-agent-interaction/src/agUiAdapter.test.ts` |
| Live stream on run detail | `RunDetailPage` → `AgentInteractionCompactPanel` + SSE hook |

## Intentional v0 limitations

Documented in package [`14_BACKLOG_RISKS_AND_NON_GOALS.md`](../_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/14_BACKLOG_RISKS_AND_NON_GOALS.md). Still true after closeout:

- Allagma SSE is **poll-based** server-side (not a push bus).
- Frontend uses **fetch + SSE framing** (not `EventSource`) for Bearer auth.
- No WebSocket, CopilotKit SDK, or reverse AG-UI ingress in this milestone.
- No new Conexus HTTP export endpoint (client-side synthesis only).
- Hidden reasoning / raw prompts excluded by contract and projectors.
- Additional JSONL fixture categories from package testing doc remain optional backlog.

## Remaining work (merge only)

```text
- [ ] Open/stack PRs per 05_REPO_PR_SEQUENCE.md (+ OFE-AGUI-004 as separate frontend PR)
- [ ] CI green on each repo (unit + contract gates)
- [ ] Merge to moving-main / release branches per team process
- [ ] Optional: docker-live operator smoke of /system/agent-interaction?runId=…
```

No further spine **architecture** work is required for milestone closure after merge.

## Related docs

- Contract: [`AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md)
- AG-UI mapping: [`AG_UI_COMPATIBILITY_ADAPTER.md`](../operators/AG_UI_COMPATIBILITY_ADAPTER.md)
- PR sequence: [`05_REPO_PR_SEQUENCE.md`](../_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/05_REPO_PR_SEQUENCE.md)
- Continuation plan: [`15_CONT_PLAN.md`](../_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/15_CONT_PLAN.md)
