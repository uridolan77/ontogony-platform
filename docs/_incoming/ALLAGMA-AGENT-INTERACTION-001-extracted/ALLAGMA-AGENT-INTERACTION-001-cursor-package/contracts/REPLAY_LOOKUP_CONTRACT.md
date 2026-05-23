# Replay lookup contract

## Purpose

Make Allagma replay lookup real and useful from live identifiers.

## Inputs

At minimum:

```text
runId
```

Optional roots if existing resolver supports them:

```text
traceId
correlationId
planningDecisionId
actionDecisionId
conexusModelCallId
baselineComparisonId
evaluationRunId
```

## Required lookup result

```ts
export interface ReplayLookupResult {
  rootIdentifier: string;
  rootKind: string;
  mode: "live_lookup" | "fixture_replay" | "imported_jsonl";
  session?: AgentInteractionSession;
  sourceAttempts: AgentInteractionSourceAttempt[];
  missing: AgentInteractionMissingReason[];
}
```

## Source attempts

Every API attempt must include:

- system
- endpoint
- identifier kind/value
- status
- latencyMs
- error code/message if failed

## Buttons

- `Use latest completed run`
- `Use latest failed run`
- `Open in Evidence Spine`
- `Export interaction bundle`

## Disable/export rule

If no evidence is resolved, either disable export or export a diagnostic bundle that says no live evidence was resolved.
