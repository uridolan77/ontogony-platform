# Tool and human-gate event contract

## Tool intent

```ts
export interface AgentInteractionToolIntent {
  id: string;
  name?: string;
  status: "proposed" | "evaluating" | "allowed" | "blocked" | "executed" | "failed" | "unknown";
  statusLabel?: string;
  proposedAt?: string;
  evaluatedAt?: string;
  completedAt?: string;
  decisionId?: string;
  policyId?: string;
  reason?: string;
  mode?: "symbolic" | "dry_run" | "local_sandbox" | "real_external";
  evidenceLinks?: AgentInteractionLink[];
}
```

## Human gate

```ts
export interface AgentInteractionHumanGate {
  id: string;
  status: "requested" | "waiting" | "approved" | "denied" | "resolved" | "expired" | "not_required" | "unknown";
  statusLabel?: string;
  requestedAt?: string;
  resolvedAt?: string;
  decisionId?: string;
  policyId?: string;
  reason?: string;
  resolverActorId?: string;
  evidenceLinks?: AgentInteractionLink[];
}
```

## Display rules

- Show tool and gate summaries near the top of the session.
- Also show detailed rows in chronological timeline.
- Show decision/policy IDs when available.
- If not available, display `not_recorded` or `not_required`, not generic `unknown`.

## Safety rule

Real external execution remains blocked unless a separate trust-gated sprint explicitly enables it. This package only displays tool evidence.
