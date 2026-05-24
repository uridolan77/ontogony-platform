# Copy and operator language rules

## Use operator-grade language

Prefer:

```text
Readiness: not ready — provider route check failed
Evidence: partial — route decision detail was not resolved
Data source: demo fixture — not live evidence
```

Avoid:

```text
Backend-waiting list APIs
sample rows in tests are not mixed into production responses
Route: GET /ontology/v0 (snapshot)
An unexpected error occurred
unknown
```

## API route details

API route details are allowed, but not in primary operator copy. Put them in:

- developer details;
- source attempts;
- diagnostics panels;
- exported evidence bundles.

## Refresh labels

If multiple refresh scopes exist, label them by scope:

- Refresh health
- Refresh bindings
- Refresh page data
- Refresh topology checks
- Refresh evidence graph

## Sample/demo labels

Never write `sample` alone. Use:

- `Demo fixture`
- `Current list sample`
- `Generated artifact`
- `Imported bundle`
- `Last 24h metric`

## Future capability language

Planned/future features must be visibly marked as planned. They must not look like current topology, readiness, or evidence.
