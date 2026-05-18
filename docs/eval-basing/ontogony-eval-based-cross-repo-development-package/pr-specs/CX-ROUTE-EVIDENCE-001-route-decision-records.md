# PR Spec — CX-ROUTE-EVIDENCE-001 — Route Decision Records

## Repo

`conexus-dotnet`

## Goal

Make model/provider route decisions first-class evidence.

## Add

```text
RouteDecisionRecord
IRouteDecisionRecorder
GET /admin/v0/route-decisions/{routeDecisionId}
routeDecisionId on chat telemetry/model call metadata
```

## Fields

```text
routeDecisionId
requestId
traceId
projectId
requestedModelAlias
resolvedModelAlias
providerKey
providerModel
fallbackChain
priceCatalogVersion
capabilityProfileVersion
constraintsApplied
selectionReason
createdAtUtc
```

## Behavior

When `ChatCompletionService` resolves a route, record a route decision before provider call.

## Tests

- non-streaming records route decision.
- streaming records route decision.
- fallback chain recorded.
- project override recorded.
- routeDecisionId appears in telemetry/journal metadata.
- no raw prompt/response stored.

## Acceptance

- no semantic policy in Conexus.
- admin auth applies to query endpoint.
