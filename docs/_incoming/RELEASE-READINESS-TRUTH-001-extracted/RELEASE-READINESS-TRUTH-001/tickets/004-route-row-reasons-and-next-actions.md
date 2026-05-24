# Ticket 004 — Route row reasons and next actions

## Goal

Every partial, fixture-only, fallback, or unknown row should explain what is missing and what the operator/developer should do next.

## Problem

Rows currently show route, area, dataSource, and readiness, but not enough actionable explanation.

## Implementation

Extend route table rows with:

- reason
- next action
- release impact
- maybe a compact details disclosure if table width is constrained

Example rows:

```text
/system/release-readiness
fixture_only / ready
Impact: demo only
Reason: Generated route artifact page; no live backend release validation.
Next: Keep excluded from RC posture or add live validation.
```

```text
/allagma/runs/:runId/audit
unknown / partial
Impact: unresolved
Reason: Data source is not classified.
Next: classify as live, fallback, fixture, generated, unsupported, or remove from scorecard.
```

## Acceptance

- [ ] All non-clean rows have reason text.
- [ ] All non-clean rows have next action text.
- [ ] Clean live rows can have concise reason text.
- [ ] Table remains readable.
- [ ] Tests assert reason/next-action presence for partial/unknown/fixture cases.
