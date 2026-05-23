# Cursor prompt — repo audit

Audit the current repos for `ALLAGMA-AGENT-INTERACTION-001`. Do not edit code yet.

Find and summarize:

1. Agent Interaction page/component files.
2. Fixture replay data and fixture IDs.
3. Current mode/data-source logic, if any.
4. Allagma client functions for run detail/events/audit/evaluations/replay.
5. Conexus client functions for model calls/evidence links/execution runs/route decisions.
6. Kanon client functions for decision records/provenance/semantic graph.
7. Existing Evidence Spine resolver functions that can be reused.
8. Allagma overview/run-list component files.
9. Start Run page/workbench files.
10. Tests that currently cover these surfaces.

Produce an audit note with:

```text
Current behavior
Available APIs
Missing data
Proposed smallest implementation path
Files likely to change
Tests to add/update
```

Then continue to implementation only after the audit is complete.
