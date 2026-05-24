# Component mapping contract

## Expected shared exports from `@ontogony/ui`

Names may be adapted to repo style, but the responsibilities should exist.

```ts
export type ConnectivityStatus = 'live' | 'degraded' | 'offline' | 'unknown';
export type ReadinessStatus = 'ready' | 'not_ready' | 'unknown';
export type ContractHealthStatus = 'valid' | 'warning' | 'invalid' | 'unknown';
export type OperatorUsabilityStatus = 'usable' | 'degraded' | 'blocked' | 'unknown';
export type EvidenceCompletenessStatus = 'resolved' | 'partial' | 'unresolved' | 'not_applicable' | 'unknown';
export type DataSourceStatus = 'live' | 'live_with_fallback' | 'fixture' | 'generated' | 'imported' | 'mock' | 'unknown';
export type AuthorityStatus = 'authoritative' | 'advisory' | 'demo' | 'inferred' | 'historical' | 'unknown';
export type TopologyEdgeStatus = 'validated' | 'degraded' | 'missing' | 'planned' | 'blocked' | 'unknown';
```

Recommended helpers:

```ts
getOperatorStatusLabel(status)
getOperatorStatusSeverity(status)
formatLabeledUnknown(subject, reason)
buildServiceStatusSummary(card)
buildEvidenceCompletenessSummary(summary)
buildTopologyEdgeSummary(edge)
```

Recommended components:

```tsx
<OperatorStatusBadge />
<OperatorStatusRow />
<OperatorServiceStatusCard />
<DataSourceBadge />
<AuthorityBadge />
<EvidenceCompletenessBadge />
<TopologyEdgeStatusCard />
<DeveloperDetails />
```

## Rendering rule

Components should force callers to provide enough context for unknown, partial, degraded, blocked, and fixture states.
