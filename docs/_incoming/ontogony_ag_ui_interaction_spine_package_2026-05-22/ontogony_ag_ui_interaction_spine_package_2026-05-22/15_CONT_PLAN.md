The next plan should move from **“AG-UI spine exists”** to **“AG-UI is the live operator interaction layer across Allagma/Kanon/Conexus.”**

I would continue in this order.

# Phase 1 — Make AG-UI live in the frontend

## 1. `OFE-AGUI-004 — Allagma interaction-events SSE consumer` ✅ (local)

**Repo:** `ontogony-frontend`  
**Evidence:** `ontogony-frontend/docs/evidence/OFE_AGUI_004_SSE_CONSUMER_EVIDENCE.md`

This is the immediate next PR.

Goal: connect the existing backend stream:

```text
GET /allagma/v0/runs/{runId}/interaction-events/stream
```

into the frontend run detail / workbench UI.

Scope:

```text
- Add EventSource client/hook for Allagma run interaction stream.
- Normalize streamed events into the existing frontend AG-UI event model.
- Preserve current polling / JSONL export fallback.
- Add connection status:
  connected
  reconnecting
  fallbackPolling
  closed
  error
- De-duplicate stream + polling overlap.
- Close stream automatically when run reaches terminal state.
- Add tests and evidence doc.
```

This turns AG-UI from “adapter-ready” into **live operator UX**.

---

# Phase 2 — Close the AG-UI milestone properly

## 2. `AGUI-SPINE-CLOSEOUT-001 — Interaction spine acceptance closeout` ✅ (local)

**Repos:** mostly `ontogony-platform`, possibly `ontogony-frontend`  
**Evidence:** `ontogony-platform/docs/evidence/AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md`

Goal: formally close the 10-PR AG-UI spine sequence.

Scope:

```text
- Check 13_ACCEPTANCE_CHECKLIST.md against actual evidence.
- Mark the original 10 PRs as complete.
- List evidence docs:
  PLAT_AGUI_*
  OUI_AGENT_*
  OFE_AGUI_*
  ALLAGMA_AGUI_*
  KANON_HITL_AGUI_*
  CONEXUS_AGUI_*
- Separate original spine completion from follow-up polish.
- Record known v0 limitations:
  no WebSocket
  Allagma stream is SSE/poll-compatible, not full push bus
  Kanon has HITL mapping, not broad AG-UI HTTP export
  Conexus projection is evidence/adapter layer, not necessarily public export API
```

This gives you a clean milestone story.

---

# Phase 3 — Make the AG-UI workbench useful, not only connected

## 3. `OFE-AGUI-005 — Run interaction workbench polish` ✅ (local)

**Repo:** `ontogony-frontend`  
**Evidence:** `ontogony-frontend/docs/evidence/OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md`

Goal: make the run detail/workbench experience operator-friendly.

Scope:

```text
- Timeline of AG-UI events.
- Filter by event family:
  run
  model
  tool
  human_gate
  policy
  evidence
  error
- Highlight human-action-required events.
- Show linked identifiers:
  allagmaRunId
  kanonDecisionId
  humanGateId
  conexusModelCallId
  traceId
  correlationId
- Add “copy evidence id” actions.
- Add fallback/error state messaging.
```

This is where AG-UI becomes the **interaction spine UI**, not just a protocol layer.

---

# Phase 4 — Kanon human-in-the-loop integration

## 4. `KANON-AGUI-REVIEW-001 — Review queue AG-UI projection` ✅ (local)

**Repo:** `kanon-dotnet`, maybe frontend later  
**Evidence:** `kanon-dotnet/docs/evidence/KANON_AGUI_REVIEW_001_EVIDENCE.md`

Goal: expose Kanon review workflows as AG-UI interaction events.

Use cases:

```text
- source binding requires approval
- Conexus assistance draft needs accept/reject
- canonical fact conflict requires review
- action policy produces human_gate
- domain-pack promotion requires semantic approval
```

Scope:

```text
- Define Kanon review event projection.
- Map existing Kanon decisions/human gates into AG-UI event types.
- Keep Kanon semantic authority; do not make AG-UI authoritative.
- Add tests for:
  assistance accept/reject
  human gate
  source-binding review
  contradiction review
```

This connects directly with the **Kanon semantic product-deepening track**.

---

# Phase 5 — Conexus model-call interaction visibility

## 5. `CONEXUS-AGUI-002 — Model-call AG-UI evidence projection` ✅ (local)

**Repo:** `conexus-dotnet`  
**Evidence:** `conexus-dotnet/docs/evidence/CONEXUS_AGUI_002_EVIDENCE.md`

Goal: make model calls visible in the operator interaction timeline.

Scope:

```text
- Project route decisions into AG-UI-compatible events.
- Project model-call lifecycle:
  requested
  route_decided
  provider_started
  streamed_chunk_metadata
  completed
  failed
  fallback_attempted
  cost_recorded
- Do not expose raw prompt/response by default.
- Link to existing Conexus model-call evidence bundle.
```

This gives the operator a clear model-call story inside the same AG-UI timeline.

---

# Phase 6 — Cross-system AG-UI evidence graph

## 6. `ADAPTER-AGUI-002 — Cross-system AG-UI evidence resolver` ✅ (local)

**Repos:** `ontogony-platform`, `ontogony-frontend`  
**Evidence:** `ontogony-platform/docs/evidence/PLAT_AGUI_RESOLVER_002_EVIDENCE.md`, `ontogony-frontend/docs/evidence/OFE_AGUI_RESOLVER_002_EVIDENCE.md`

Goal: from one AG-UI event, resolve the whole chain:

```text
Allagma run event
  → Kanon decision / human gate / semantic graph
  → Conexus model call / route decision
  → trace / correlation / replay bundle
```

Scope:

```text
- Add resolver helpers for event-linked IDs.
- Add page links.
- Add “open evidence graph” from AG-UI timeline.
- Reuse existing evidence spine logic.
- Avoid new backend aggregation unless necessary.
```

This makes AG-UI the front door to the evidence spine.

---

# Recommended immediate sequence

Do not start all of this at once. I’d do:

```text
1. OFE-AGUI-004 ✅ (local)
   Live frontend SSE consumer.

2. AGUI-SPINE-CLOSEOUT-001 ✅ (local)
   Formal acceptance closeout — see ontogony-platform/docs/evidence/AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md

3. OFE-AGUI-005 ✅ (local)
   Better workbench/timeline UX — see ontogony-frontend/docs/evidence/OFE_AGUI_005_WORKBENCH_POLISH_EVIDENCE.md

4. KANON-AGUI-REVIEW-001 ✅ (local)
   Human review / semantic review event projection — see kanon-dotnet/docs/evidence/KANON_AGUI_REVIEW_001_EVIDENCE.md

5. CONEXUS-AGUI-002 ✅ (local)
   Model-call timeline projection — see conexus-dotnet/docs/evidence/CONEXUS_AGUI_002_EVIDENCE.md

6. ADAPTER-AGUI-002 ✅ (local)
   Cross-system evidence graph from AG-UI events.

7. AGUI-RELEASE-CLOSURE-001 / 002 ✅ (local)
   `npm run check:full` green on `ontogony-frontend` (378 Playwright tests, 2026-05-22).
   Evidence: `ontogony-platform/docs/evidence/AGUI_RELEASE_CLOSURE_001_EVIDENCE.md`,
   `AGUI_RELEASE_CLOSURE_002_EVIDENCE.md`,
   `ontogony-frontend/docs/evidence/AGUI_RELEASE_CLOSURE_E2E_001_EVIDENCE.md`.
   Next: **KANON-AGUI-WORKBENCH-001** (not a release-closure blocker).
```

# Strategic direction

The right end-state is:

```text
AG-UI = operator interaction protocol
Allagma = runtime event source
Kanon = semantic/human-review authority
Conexus = model-call evidence source
Frontend = live AG-UI workbench
Platform = shared event contracts/adapters only
```

So the next move is simple: **wire live SSE in the frontend first**, then close the milestone, then deepen AG-UI into the operator workbench and Kanon review flows.
