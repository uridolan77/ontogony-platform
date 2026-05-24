# Ticket 005 — Filter state, empty state, and pagination polish

## Goal

Make filters, empty results, and pagination easier to interpret.

## Problems

The dashboard has many filters. Empty or sparse states need to say whether the backend returned zero rows, filters excluded rows, or metadata is missing.

## Implementation

1. Render active filter chips or a compact active-filter summary.
2. Improve empty state:
   - backend returned zero rows
   - active filters shown
   - clear filters action
   - load fixture/dashboard action, if available and labeled
3. Pagination:
   - show current page size
   - show whether more pages exist
   - avoid aggregate language if only current page is loaded
4. Date range:
   - show normalized ISO dates or local display consistently.
   - prevent invalid ranges if form supports it.

## Acceptance

- Empty states are diagnostic.
- Filter state is visible.
- Pagination does not make current-page data look global.
