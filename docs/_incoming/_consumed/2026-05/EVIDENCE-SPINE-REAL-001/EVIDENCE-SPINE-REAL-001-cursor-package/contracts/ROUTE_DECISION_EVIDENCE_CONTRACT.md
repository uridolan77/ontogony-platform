# Route Decision Evidence Contract

## Current defect to fix

Evidence links can expose:

```text
conexusRouteDecisionId = rd-...
```

but detail lookup can fail at:

```text
GET /admin/v0/route-decisions/{routeDecisionId}
```

The operator then sees a generic source failure.

## Desired backend contract

When Conexus returns `routeDecisionId` from model-call evidence links, one of these must be true:

### Option A — Preferred

`GET /admin/v0/route-decisions/{routeDecisionId}` returns `200` with route decision detail.

Minimum detail:

```json
{
  "routeDecisionId": "rd-...",
  "requestId": "0HN...",
  "modelCallId": "chatcmpl-...",
  "projectId": "...",
  "purpose": "summarize-player-risk",
  "modelAlias": "risk-summary-v0",
  "selectedProvider": "fake",
  "selectedProviderModel": "fake.chat",
  "fallbackUsed": false,
  "fallbackChain": [],
  "status": "selected",
  "createdAt": "...",
  "traceId": "...",
  "correlationId": "..."
}
```

### Option B — Acceptable transitional behavior

If Conexus does not persist route decisions yet, the detail endpoint returns typed 404:

```json
{
  "code": "conexus.route_decision_not_recorded",
  "message": "Route decision detail is not persisted for this model call.",
  "routeDecisionId": "rd-...",
  "requestId": "0HN...",
  "modelCallId": "chatcmpl-...",
  "retryable": false
}
```

Then Evidence Spine maps it to:

```text
reasonCode = not_recorded
```

### Option C — Backend inconsistency

If evidence links emit an ID that should exist but lookup returns 404 unexpectedly:

```text
reasonCode = backend_missing
```

## Frontend resolver behavior

Pseudo-code:

```ts
if (evidenceLinks.routeDecisionId) {
  const attempt = await tryGetRouteDecision(evidenceLinks.routeDecisionId);

  if (attempt.success) {
    addRouteDecisionNode(attempt.body);
    addEdge(modelCall, routeDecision, 'used_route_decision', 'direct');
  } else {
    addMissingLink({
      relationship: 'used_route_decision',
      applicability: isGovernedContext(root) ? 'required' : 'optional',
      reasonCode: mapRouteDecisionFailure(attempt),
      message: describeRouteDecisionFailure(attempt),
      sourceAttemptIds: [attempt.id],
    });
  }
}
```

## Tests

Conexus tests:

- fake chat produces model call evidence links with routeDecisionId;
- routeDecisionId resolves to detail; or typed 404 is returned by design;
- route detail includes alias/provider/fallback metadata.

Frontend tests:

- route decision 200 adds node + edge;
- typed 404 adds structured missing link;
- untyped exception maps to `lookup_failed` and does not render generic error.
