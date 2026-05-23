# 06 — Backend Plan: Conexus

## Goal

Conexus must provide complete fake-provider model-call evidence, including resolvable route decision and provider attempt.

## Required behavior

For a governed fake run:

1. Conexus receives request with propagated trace/correlation.
2. Alias resolves to fake provider:
   - alias `risk-summary-v0`
   - provider key `fake`
   - provider model `fake.chat`
3. Conexus records route decision.
4. Conexus records model call.
5. Conexus records provider attempt.
6. Evidence endpoints return consistent IDs.

## Critical bug to fix

If model-call evidence returns:

```text
routeDecisionId = rd-...
```

then this must resolve:

```text
GET /admin/v0/route-decisions/rd-... -> 200
```

Do not emit unresolved route ids unless the route decision is resolvable or the endpoint returns a structured reason.

## Candidate code areas

```text
conexus-dotnet/src/Conexus.Api/Endpoints/RouteDecisionAdminEndpoints.cs
conexus-dotnet/src/Conexus.Application/Telemetry/RouteDecisionAdminDetailQuery.cs
conexus-dotnet/src/Conexus.Persistence/EfRouteDecisionStore.cs
conexus-dotnet/src/Conexus.Infrastructure/Telemetry/InMemoryRouteDecisionStore.cs
conexus-dotnet/src/Conexus.Application/Telemetry/ModelCallEvidenceBundleQuery.cs
conexus-dotnet/src/Conexus.Application/Telemetry/*ModelCall*
conexus-dotnet/tests/Conexus.Api.Tests/
conexus-dotnet/tests/Conexus.Infrastructure.Tests/
```

## Regression test

Create a test like:

```text
Fake chat request
  -> model call detail returns modelCallId and routeDecisionId
  -> evidence-links returns routeDecisionId
  -> GET /admin/v0/route-decisions/{routeDecisionId} returns 200
  -> route detail contains providerKey=fake, providerModel=fake.chat
```

## Acceptance

- Evidence Spine resolves `used_route_decision`.
- Provider attempt displays `fake / fake.chat`.
- No generic "unexpected error" for route-decision lookup.
