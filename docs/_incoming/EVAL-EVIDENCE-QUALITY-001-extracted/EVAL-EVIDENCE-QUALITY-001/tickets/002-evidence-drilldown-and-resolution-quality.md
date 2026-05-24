# Ticket 002 — Evidence drilldown and resolution quality

## Goal

Make Evidence Spine navigation from evaluation rows trustworthy.

## Problems

The page has Evidence Spine links, but the operator cannot tell whether each link resolves complete evidence or merely opens a generic workbench.

## Implementation

1. Inspect current evidence link helper(s).
2. Identify supported identifier kinds:
   - evaluation ID
   - run ID
   - trace ID
   - correlation ID
   - model call ID
   - Kanon decision ID
   - baseline comparison ID
3. For each evaluation row, compute evidence targets from actual fields.
4. Choose the strongest available target:
   - trace/correlation if present
   - run ID if present
   - evaluation ID if supported
   - otherwise disabled/unavailable
5. Render row-level evidence quality:
   - `complete`
   - `partial`
   - `unresolved`
   - `not available`
6. Add missing reason text:
   - no run id
   - no trace id
   - evaluation lookup unsupported
   - comparison absent
   - model call absent
   - backend route missing

## Acceptance

- Rows with no evidence identifiers do not show normal working links.
- Rows with run/trace IDs link correctly.
- Partial evidence is visibly partial.
- Missing evidence reason is visible or accessible.
