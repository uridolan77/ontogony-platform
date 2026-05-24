# 04 — Frontend Implementation Plan

## A. Health parser

Update the frontend health client to parse `health.v1`.

When a payload does not match:

- do not show only "health payload format warning";
- show `contract: warning` with exact missing fields;
- preserve liveness if HTTP call succeeded;
- show remediation in detail.

Example:

```text
Connectivity: live
Contract: warning
Missing fields: schemaVersion, version
Readiness: not checked
```

## B. Ready parser

Parse `ready.v1`.

Render readiness as a separate card/table:

```text
Readiness: not ready
Failing required checks:
- conexus.routing.aliases: alias risk-summary-v0 points to missing provider
Warnings:
- openai credentials not configured
```

## C. Operator Home

Replace broad claims with truth dimensions:

- System baseline
- Runtime lock
- Service cards:
  - Connectivity
  - Readiness
  - Contract
  - Version
  - Last checked
  - Data source
- Compatibility card
- Execution safety card
- Primary actions

Do not say "All services are healthy" unless:
- all services are live,
- all required readiness checks are ready,
- all health contracts are valid.

## D. Settings

Settings should show:

- current configured URLs,
- current credential source,
- live connectivity,
- readiness aggregate,
- exact contract warnings,
- exact Conexus not-ready reason,
- role profile clarity.

## E. Topology readiness

Topology should separate:

```text
Current implemented edges
Planned/future edges
Fixture/demo edges
```

Every edge should show:

- expected route(s),
- last attempted call,
- last success,
- data source,
- proof identifier if available,
- status.

## F. Release readiness

If source is generated route inventory only:

- rename to "Generated route readiness artifact";
- show release candidate posture as `not_assessed` or `warning`;
- fixture-only routes cannot be counted as fully ready;
- route-level reasons must explain partial status.

## G. Status taxonomy from `@ontogony/ui`

If `@ontogony/ui` already has status primitives, extend them. Otherwise add a small product-neutral taxonomy:

```ts
type ConnectivityState = "live" | "degraded" | "offline" | "unknown";
type ReadinessState = "ready" | "not_ready" | "degraded" | "unknown" | "not_applicable";
type ContractState = "valid" | "warning" | "invalid" | "unknown";
type DataSourceState = "live" | "live_with_fallback" | "generated" | "fixture" | "imported" | "unknown";
type AuthorityState = "authoritative" | "advisory" | "demo" | "diagnostic";
```

## H. Copy rules

Remove or demote:

```text
Live with fixture fallback
health payload format warning
all services healthy
versions appear aligned
unknown
```

Replace with explicit structured text:

```text
Data source: live with fallback
Contract: warning — missing version field
Compatibility: cannot verify — no service version metadata
Unknown provider — model call did not record provider metadata
```
