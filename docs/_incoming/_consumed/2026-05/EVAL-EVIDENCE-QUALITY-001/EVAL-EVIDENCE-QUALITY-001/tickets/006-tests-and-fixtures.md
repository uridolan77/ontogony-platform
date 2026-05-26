# Ticket 006 — Tests and fixtures

## Goal

Add regression coverage for the new evidence-quality behavior.

## Implementation

Add or update tests for:

1. Row with unknown provider.
2. Row with missing dataset metadata.
3. Row with missing comparison ID.
4. Row with run ID and Evidence Spine link.
5. Row without evidence identifiers and disabled link.
6. Summary with current-page-only basis.
7. Metadata coverage counts.
8. Quality preview warning for one scenario/all pass rows.
9. Empty state with active filters.
10. Fixture/demo state labeling.

Update fixtures to include realistic sparse rows, not only ideal rows.

## Acceptance

- Tests fail before implementation and pass after.
- Fixtures include both complete and incomplete evidence rows.
- Snapshot updates are intentional and small.
