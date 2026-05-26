# Ticket 001 — Evaluation source and metadata honesty

## Goal

Make the evaluation dashboard clear about what data source and metadata completeness each row and summary represent.

## Problems

The current dashboard can show a high-confidence quality posture while rows have sparse metadata:

- provider is `unknown`
- tokens are `Not applicable`
- dataset metadata is absent
- baseline comparison IDs are absent
- summaries are current-page-only

## Implementation

1. Add a source/scope label to the evaluation dashboard:
   - live list
   - live with fallback
   - fixture/demo
   - imported
   - current-page summary

2. Compute metadata coverage:
   - provider known count
   - token/cost known count
   - dataset metadata count
   - comparison ID count
   - run ID count
   - trace/correlation count, if available

3. Display metadata coverage in summary area.

4. Update row rendering:
   - unknown provider becomes a visible warning/neutral missing-data state.
   - missing dataset metadata is explicit.
   - missing comparison metadata is explicit.
   - token state distinguishes true not-applicable from not-recorded if the data allows it.

## Acceptance

- A page with 25 rows and 0 provider metadata says so.
- Unknown provider is not rendered as ordinary valid data.
- The summary cards reveal the evidence basis.
- No invented values.
