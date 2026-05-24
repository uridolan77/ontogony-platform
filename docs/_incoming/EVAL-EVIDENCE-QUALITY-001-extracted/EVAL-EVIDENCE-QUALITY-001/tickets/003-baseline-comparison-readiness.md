# Ticket 003 — Baseline comparison readiness

## Goal

Make baseline comparison absence clear and actionable.

## Problems

The dashboard says no baseline comparison IDs exist in the current list. The operator needs to know whether this is because of sparse metadata, filters, missing backend support, or no comparisons created yet.

## Implementation

1. Update baseline comparison section to show:
   - comparison IDs found on current page
   - current filter scope
   - whether comparison filter is active
   - whether comparison workbench exists
2. If no comparison IDs:
   - show explicit reason based on available data
   - link to comparison workbench
   - explain next action
3. If comparison IDs exist:
   - group by comparison ID
   - link to comparison detail/workbench if supported
   - show count of associated evaluations

## Acceptance

- No comparison ID state is not silent.
- Operator can open comparison workbench.
- The page does not claim regression/baseline confidence when no comparison metadata exists.
