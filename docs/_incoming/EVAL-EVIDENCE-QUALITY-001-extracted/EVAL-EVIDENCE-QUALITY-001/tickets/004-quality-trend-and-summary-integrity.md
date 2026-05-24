# Ticket 004 — Quality trend and summary integrity

## Goal

Prevent the quality trend and dashboard summaries from implying durable analytics when only list-page data is available.

## Problems

The current text correctly says quality trend is based on the current list page, but the label can still imply a durable time-series.

## Implementation

1. Rename or annotate:
   - `Quality trend` → `Quality preview (current page)` when no durable analytics API is used.
2. Add basis copy:
   - number of rows
   - current sort direction
   - current filters
   - scenario count
3. Add warning/callout when:
   - only one scenario appears
   - all rows share same suite
   - all rows pass
   - provider metadata is absent
4. Keep average score but qualify it:
   - `Average quality score — current page only`

## Acceptance

- The section cannot be mistaken for a durable time-series.
- Summary cards disclose scope.
- Narrow samples are presented as narrow samples.
