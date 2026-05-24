# Executive brief

## Problem

The Agent Interaction workbench currently behaves like a fixture/demo replay page even though the local Allagma/Kanon/Conexus loop now produces real runs, events, model calls, decisions, and evidence links.

The operator needs a page that answers:

- What did this run do?
- Which Kanon decisions shaped it?
- Which Conexus model call happened?
- Which provider/route was used?
- Which tools were proposed, blocked, approved, or executed?
- Did a human gate happen?
- What messages were exchanged?
- Which timeline events are unresolved, and why?

## Desired outcome

`ALLAGMA-AGENT-INTERACTION-001` makes Agent Interaction a live interaction inspector with explicit modes:

```text
live_lookup       Real backend/API evidence
fixture_replay    Demo fixture, never readiness evidence
imported_jsonl    Offline/imported replay artifact
```

The workbench should default to `live_lookup` when the live backend exists. Fixture replay must be a deliberate operator choice and carry a strong “Demo fixture — not live evidence” banner.

## Minimum live timeline

A successful governed fake-provider run should render:

```text
RunCreated
TaskClassified
TopologySelected
TopologyAuthorizationRequested
TopologyAuthorizationCompleted
KanonPlanRequested
KanonPlanCompiled
WorkflowCheckpointRecorded
ToolIntentProposed
ToolIntentEvaluationRequested
ToolIntentEvaluationCompleted
ToolIntentBlocked / ToolIntentAllowed / ToolIntentExecuted
ConexusModelRequested
ConexusModelCompleted
RunCompleted
EvaluationRunRecorded / BaselineComparisonRecorded, when present
```

The page should enrich that with linked:

- Kanon planning decision and action decision(s), when present.
- Conexus model call, request ID, execution run, provider attempt, provider posture, tokens, cost, latency, fake/real mode.
- Allagma replay/audit bundle links.
- Evidence Spine links.

## Non-goal

This package does not enable real tools, external I/O, or production sandbox execution. It only renders existing symbolic/dry-run/local-sandbox evidence honestly.
