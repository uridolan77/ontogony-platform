# Cursor prompt — tool and human-gate timeline

Implement first-class rendering for Allagma tool and human-gate events.

## Tool events to classify

At minimum:

```text
Allagma.ToolIntentProposed
Allagma.ToolIntentEvaluationRequested
Allagma.ToolIntentEvaluationCompleted
Allagma.ToolIntentBlocked
Allagma.ToolIntentAllowed
Allagma.ToolIntentExecuted
Allagma.ToolIntentFailed
```

If exact names differ, map actual names from the repo.

## Human gate events to classify

At minimum:

```text
HumanGateRequested
HumanGateWaiting
HumanGateApproved
HumanGateDenied
HumanGateResolved
Allagma.RunWaitingForHuman
Allagma.RunResumed
```

If exact names differ, map actual names from the repo.

## UI rules

- Tool/gate events are not generic raw JSON.
- Show policy/action reason if present.
- Show related Kanon decision ID if present.
- Show status: proposed, evaluating, allowed, blocked, waiting, approved, denied, executed, failed.
- Provide evidence/source endpoint.

## Missing data

If a tool event lacks a decision ID, show:

```text
Decision link: not recorded by current event payload
```

not `unknown`.

## Tests

Add event-mapping tests for each known event name and at least one unmapped event fallback.
