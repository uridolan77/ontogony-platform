# Contract Drift Tests

## Backend

Where the repos already use contract drift guards, add the new contracts to the existing mechanism.

Suggested assertions:

- `DecisionEvent.schemaVersion == "decision-event.v0"`
- `ReconstructabilityReport.schemaVersion == "reconstructability-report.v0"`
- classification enum values are exactly `F`, `P`, `S`, `O`
- governance statuses are exactly `PASS`, `WARN`, `FAIL`
- hidden CoT capture flag is false by default and absent from raw evidence fragments

## OpenAPI / route inventory

If Kanon adds routes, regenerate:

- OpenAPI baseline;
- route inventory;
- any frontend client generated from OpenAPI.

Do not leave stale route-count comments in docs.

## Frontend type drift

Add TypeScript fixtures and compile checks for:

- property result shape;
- diagnostic shape;
- evidence fragment refs;
- status rendering.

## Cross-repo drift

If possible, add a simple schema copy check or shared JSON-schema check in `ontogony-platform` so backend and frontend use the same enum/property names.
