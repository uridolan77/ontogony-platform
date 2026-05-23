# Prompt 02 — Conexus Route Decision

```text
Implement the Conexus side of EVIDENCE-SPINE-REAL-001.

Problem:
Evidence links can expose routeDecisionId, but GET /admin/v0/route-decisions/{routeDecisionId} may fail generically or not find detail.

Tasks:
1. Inspect model-call creation, route-decision creation, routing telemetry, evidence-link generation, and admin route-decision endpoint.
2. Determine whether route decisions are intended to be persisted for fake-provider chat calls.
3. If yes, ensure the fake-provider path persists route-decision detail and the admin endpoint returns 200 for route IDs emitted in evidence links.
4. If no, stop emitting routeDecisionId as a resolvable detail link, or make the endpoint return typed 404 with code `conexus.route_decision_not_recorded`.
5. Ensure route decision detail includes routeDecisionId, requestId, modelCallId if known, model alias, selected provider, selected provider model, fallback chain, fallbackUsed, status, traceId/correlationId when available.
6. Add regression test: fake chat -> model-call evidence-links -> routeDecisionId -> route detail 200 or typed 404 by design.
7. Ensure no endpoint returns only generic “unexpected error” for not-found route decisions.

Do not change provider routing semantics except to persist or truthfully report route-decision evidence.
```
