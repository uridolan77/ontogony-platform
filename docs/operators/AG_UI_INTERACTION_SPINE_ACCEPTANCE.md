# AG-UI interaction spine — signed acceptance checklist

**Milestone:** `ONTOGONY-AGUI-000 — Agent Interaction Spine Baseline`  
**Closeout:** [`AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md`](../evidence/AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md) (2026-05-22)  
**Source:** Package `13_ACCEPTANCE_CHECKLIST.md` — ticks reflect **local workspace evidence**, not CI merge status.

**Legend:** ✅ = evidenced locally · 🔶 = follow-up polish (outside 10-PR spine) · ⏳ = merge/CI only

---

## Platform

- [x] ✅ `AGENT_INTERACTION_SPINE_CONTRACT.md` published. — [`AGENT_INTERACTION_SPINE_CONTRACT.md`](./AGENT_INTERACTION_SPINE_CONTRACT.md)
- [x] ✅ Event and session schemas published. — [`ontogony-agent-interaction-event-v0.schema.json`](../schemas/ontogony-agent-interaction-event-v0.schema.json), [`ontogony-agent-interaction-session-v0.schema.json`](../schemas/ontogony-agent-interaction-session-v0.schema.json)
- [x] ✅ Matrix maps event families to service owners. — [`agent-interaction-event.matrix.json`](../system/agent-interaction-event.matrix.json)
- [x] ✅ Validation scripts pass. — [`validate-agent-interaction-spine.ps1`](../../scripts/validate-agent-interaction-spine.ps1); [`PLAT_AGUI_000_EVIDENCE.md`](../evidence/PLAT_AGUI_000_EVIDENCE.md)
- [x] ✅ Fixtures validate. — [`docs/schemas/fixtures/agent-interaction/`](../schemas/fixtures/agent-interaction/)
- [x] ✅ Redaction / hidden reasoning rules explicit. — contract § redaction; projectors default `raw*Included: false`

## @ontogony/ui

- [x] ✅ `./agent` export added. — `ontogony-ui/package.json` exports `./agent`
- [x] ✅ Timeline component implemented. — `AgentEventTimeline.tsx`
- [x] ✅ Interrupt card implemented. — `AgentInterruptCard.tsx`
- [x] ✅ Tool-call card implemented. — `AgentToolCallCard.tsx`
- [x] ✅ Evidence links panel implemented. — `AgentEvidenceLinksPanel.tsx`
- [x] ✅ State diff panel implemented. — `AgentStateDiffPanel.tsx`
- [x] ✅ Storybook examples added. — `AgentEventTimeline.stories.tsx`
- [x] ✅ Export smoke tests pass. — `npm run check:exports`, `check:smoke-named-exports`; component unit tests under `src/components/agent/`

## ontogony-frontend

- [x] ✅ `src/agent-interaction` module added.
- [x] ✅ JSONL fixture adapter added. — `jsonlFixtureAdapter.ts`
- [x] ✅ Deterministic reducer added. — `interactionReducer.ts` (+ tests)
- [x] ✅ Workbench route added. — `/system/agent-interaction`
- [x] ✅ Evidence Spine adapter added. — `evidenceSpineInteractionAdapter.ts`
- [x] ✅ Allagma/Kanon/Conexus client-side adapters added.
- [x] ✅ Lookup by run/model-call/decision/trace/humanGate works. — `agentInteractionLinks.ts`
- [x] ✅ JSONL import/export works. — workbench + AG-UI JSONL export
- [x] ✅ Live SSE consumer (OFE-AGUI-004). — [`OFE_AGUI_004_SSE_CONSUMER_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_AGUI_004_SSE_CONSUMER_EVIDENCE.md)
- [x] ✅ Workbench polish (OFE-AGUI-005). — [`OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md`](../../../ontogony-frontend/docs/evidence/OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md)

## Allagma

- [x] ✅ Projection service added. — [`ALLAGMA_AGUI_001_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/ALLAGMA_AGUI_001_EVIDENCE.md)
- [x] ✅ Run lifecycle maps to event schema.
- [x] ✅ Run events/operations map to steps/tool/message/events.
- [x] ✅ Human gate references map to interrupts.
- [x] ✅ JSONL export validates. — `GET …/interaction-events`
- [x] ✅ Live stream implemented. — [`ALLAGMA_AGUI_002_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/ALLAGMA_AGUI_002_EVIDENCE.md); frontend consumer OFE-AGUI-004

## Kanon

- [x] ✅ Human gate interrupt mapping documented. — [`KANON_HITL_AGUI_001_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_HITL_AGUI_001_EVIDENCE.md)
- [x] ✅ Response-schema conventions documented.
- [x] ✅ Approval/reject/edit payloads covered by tests.
- [x] ✅ Decision/provenance links emitted as evidence events.
- [x] ✅ Review queue AG-UI projection (KANON-AGUI-REVIEW-001). — [`KANON_AGUI_REVIEW_001_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_AGUI_REVIEW_001_EVIDENCE.md)

## Conexus

- [x] ✅ Model-call projection documented. — [`CONEXUS_AGUI_001_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_AGUI_001_EVIDENCE.md)
- [x] ✅ Fake provider event sequence fixture validates.
- [x] ✅ Route decision and provider attempts mapped.
- [x] ✅ Usage/cost event redacts raw content.
- [x] ✅ Evidence links and bundle mapping covered.
- [x] ✅ Model-call lifecycle timeline (CONEXUS-AGUI-002). — [`CONEXUS_AGUI_002_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_AGUI_002_EVIDENCE.md)

## System-level acceptance

- [x] ✅ Operator can open `/system/agent-interaction?runId=...` and see a timeline.
- [x] ✅ Operator can open by `modelCallId`, `decisionId`, `traceId`, or `humanGateId`.
- [x] ✅ Missing links are displayed as unresolved, not blank. — `missingReasonCode` paths
- [x] ✅ Exported JSONL can be replayed offline.
- [x] ✅ AG-UI adapter tests pass against sample fixtures.
- [x] ✅ Live stream on run detail when `runId` present and run non-terminal (OFE-AGUI-004).

## Merge / CI (out of checklist scope, required for production closeout)

- [ ] ⏳ PRs opened and merged per [`05_REPO_PR_SEQUENCE.md`](../_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/05_REPO_PR_SEQUENCE.md)
- [ ] ⏳ CI green on all affected repos
- [ ] ⏳ Optional docker-live smoke of agent-interaction workbench
