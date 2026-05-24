# Agent Interaction session contract

## Purpose

A normalized frontend/operator contract for rendering a live or replayed agent interaction without leaking backend DTO shape into UI components.

## TypeScript target shape

```ts
export type AgentInteractionMode = "live_lookup" | "fixture_replay" | "imported_jsonl";
export type AgentInteractionDataSource = "live" | "fixture" | "imported" | "mixed" | "unknown";

export interface AgentInteractionSession {
  schemaVersion: "agent-interaction-session-v1";
  sessionId: string;
  mode: AgentInteractionMode;
  dataSource: AgentInteractionDataSource;
  isLiveEvidence: boolean;
  title: string;
  status: "completed" | "running" | "waiting" | "failed" | "blocked" | "cancelled" | "unknown";
  statusLabel?: string;
  startedAt?: string;
  completedAt?: string;
  summary: AgentInteractionSummary;
  linkedIdentifiers: AgentInteractionLinkedIdentifiers;
  timeline: AgentInteractionTimelineEvent[];
  messages: AgentInteractionMessage[];
  toolSummary?: AgentInteractionToolSummary;
  humanGateSummary?: AgentInteractionHumanGateSummary;
  providerSummary?: AgentInteractionProviderSummary;
  kanonSummary?: AgentInteractionKanonSummary;
  sourceAttempts: AgentInteractionSourceAttempt[];
  missing: AgentInteractionMissingReason[];
  links: AgentInteractionLink[];
  exportWarnings: string[];
}
```

## Summary

```ts
export interface AgentInteractionSummary {
  objective?: string;
  purpose?: string;
  taskType?: string | null;
  taskTypeLabel?: string;
  actorId?: string;
  historicalActorId?: string;
  ontologyVersionId?: string;
  providerMode?: "fake" | "real" | "mixed" | "unknown";
  providerModeLabel?: string;
}
```

## Linked identifiers

```ts
export interface AgentInteractionLinkedIdentifiers {
  allagmaRunId?: string;
  threadId?: string;
  traceId?: string;
  correlationId?: string;
  planningDecisionId?: string;
  actionDecisionIds?: string[];
  humanGateIds?: string[];
  conexusModelCallId?: string;
  conexusRequestId?: string;
  conexusExecutionRunId?: string;
  routeDecisionId?: string;
  evaluationRunIds?: string[];
  baselineComparisonIds?: string[];
}
```

## Rule

Never display a bare `unknown`. If a field is unknown, label the dimension:

```text
Task type: unknown
Provider mode: unknown
Decision link: not recorded
Messages: redacted
```
