# Cursor prompt — 01 implement shared taxonomy in @ontogony/ui

Implement the shared operator status taxonomy in `ontogony-ui`.

## Requirements

Add types/helpers/components for:

- connectivity;
- readiness;
- contract health;
- operator usability;
- evidence completeness;
- data source;
- authority;
- topology edge state.

## Important design rule

Do not create one overloaded enum called `Status`. Keep dimensions separate.

## API shape

Adapt to existing repo style, but provide equivalents of:

```ts
OperatorStatusDimension
OperatorStatusSeverity
OperatorStatusViewModel
DataSourceBadge
AuthorityBadge
EvidenceCompletenessBadge
OperatorServiceStatusCard
TopologyEdgeStatusCard
DeveloperDetails
```

## Unknown handling

Expose a helper that requires a subject and reason for unknown states.

## Tests

Add tests for:

- label mapping;
- severity mapping;
- no bare unknown formatting;
- fixture/generated/imported not treated as live;
- not_applicable evidence distinct from unresolved.

## Exports

Ensure the new API is exported through a public subpath and covered by existing export guards.
