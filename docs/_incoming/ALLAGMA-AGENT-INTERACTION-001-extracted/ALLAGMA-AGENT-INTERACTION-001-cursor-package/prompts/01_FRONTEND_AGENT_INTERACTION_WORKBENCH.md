# Cursor prompt — frontend Agent Interaction workbench

Implement the frontend part of `ALLAGMA-AGENT-INTERACTION-001`.

## Requirements

- Add explicit mode switch: live lookup, fixture replay, imported JSONL.
- Default to live lookup when Allagma is reachable.
- Show `Demo fixture — not live evidence` in fixture mode.
- Show imported/offline badge in imported JSONL mode.
- Add typed interaction session adapter between backend DTOs and UI components.
- Render session summary, linked identifiers, timeline, messages, tools/gates, provider, Kanon decisions, source attempts, missing reasons.
- Ensure no duplicate Agent Interaction sections appear in UI/export.

## Components to prefer

Use existing `@ontogony/ui` primitives for cards, badges, tabs, tables, status pills, copyable IDs, and expandable raw details.

## Output shape

Do not pass raw Allagma/Kanon/Conexus DTOs directly to display components. Normalize into:

```ts
AgentInteractionSession
AgentInteractionTimelineEvent
AgentInteractionMessage
AgentInteractionSourceAttempt
```

## Data source rules

- `live_lookup`: authoritative live evidence if loaded from backend.
- `fixture_replay`: demo only.
- `imported_jsonl`: offline/imported artifact only.

## Tests

Add component/unit tests for:

- default live mode when reachable
- fixture badge
- imported badge
- session summary rendering
- mapped Allagma event rendering
- unresolved reason rendering
- no raw fixture/default leakage in live mode
