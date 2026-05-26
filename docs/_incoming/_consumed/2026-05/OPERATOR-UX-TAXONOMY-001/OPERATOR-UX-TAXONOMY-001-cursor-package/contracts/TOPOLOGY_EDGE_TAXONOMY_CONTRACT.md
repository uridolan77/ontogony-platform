# Topology edge taxonomy contract

## Edge states

| State | Meaning |
|---|---|
| `validated` | Expected route or evidence link has been successfully verified live. |
| `degraded` | Edge exists but has warnings, partial readiness, stale data, or contract mismatch. |
| `missing` | Expected edge/route/link is absent or failed. |
| `planned` | Intended future edge; not implemented or not validated. |
| `blocked` | Edge cannot be used because of auth, safety gate, config, or policy. |
| `unknown` | Edge has not been checked. |

## Required fields

Every topology edge card should support:

```ts
{
  from: string;
  to: string;
  state: TopologyEdgeState;
  expectedRoutes?: string[];
  lastAttempt?: {
    endpoint: string;
    status: 'success' | 'error' | 'skipped';
    checkedAt?: string;
    latencyMs?: number;
    reasonCode?: string;
  };
  lastSuccessAt?: string;
  traceId?: string;
  correlationId?: string;
  evidenceIds?: string[];
  reason?: string;
  nextAction?: string;
}
```

## Rule

A planned edge must not have the same visual weight as a validated edge.
