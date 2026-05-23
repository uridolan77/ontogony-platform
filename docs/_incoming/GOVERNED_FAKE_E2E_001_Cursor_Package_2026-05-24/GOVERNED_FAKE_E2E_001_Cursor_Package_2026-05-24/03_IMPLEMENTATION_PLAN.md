# 03 — Implementation Plan

## Phase 1 — Baseline discovery

1. Start the full local stack.
2. Run one direct Conexus fake chat and confirm the known partial graph.
3. Run or inspect one Allagma governed fake run.
4. Record exact IDs:
   - runId
   - traceId
   - correlationId
   - Kanon decisionId
   - Conexus modelCallId
   - routeDecisionId
5. Identify which IDs are currently missing.

## Phase 2 — Backend propagation

Ensure the live governed path propagates identifiers:

```text
Allagma run:
  runId
  traceId
  correlationId
  planningDecisionId
  actionDecisionId? optional
  modelCallId
  routeDecisionId? optional but useful

Kanon decision:
  decisionId
  traceId
  correlationId
  runId / entityRef where appropriate

Conexus model call:
  modelCallId
  requestId
  traceId
  correlationId
  routeDecisionId
  provider attempts
```

## Phase 3 — Conexus route decision repair

If `routeDecisionId` appears in model-call detail or evidence links, it must resolve:

```text
GET /admin/v0/route-decisions/{routeDecisionId} -> 200
```

If the route decision genuinely cannot be found:

```text
GET ... -> 404 route_decision_not_found
```

No generic "unexpected error."

## Phase 4 — Evidence Spine live graph

Update resolver behavior:

- resolve from `runId`;
- expand to Allagma events;
- extract Kanon decisions from run/audit/events and by trace;
- extract Conexus model call ids from run/audit/events;
- resolve Conexus model call details and evidence links;
- resolve route decision;
- resolve provider attempts;
- merge placeholder and resolved nodes by canonical ID;
- classify non-applicable links.

## Phase 5 — Frontend live affordances

Add operator shortcuts:

- "Use latest completed governed fake run";
- "Start governed fake run and open Evidence Spine";
- "Open Agent Interaction for this live run";
- "Open Conexus model call";
- "Open Kanon decision".

Make fixture replay explicitly non-authoritative.

## Phase 6 — Tests

Add backend integration tests and frontend/e2e tests. See `09_TEST_PLAN.md`.
