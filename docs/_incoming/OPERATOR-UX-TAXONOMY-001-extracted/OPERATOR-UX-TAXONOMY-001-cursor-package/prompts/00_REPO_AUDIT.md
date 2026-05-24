# Cursor prompt — 00 repo audit

Before implementation, audit the actual status vocabulary.

## Search targets

In `ontogony-ui` and `ontogony-frontend`, search for:

```text
unknown
healthy
health payload format warning
not ready
ready
live
fixture
fallback
sample
demo
generated
imported
advisory
authoritative
partial
unresolved
not applicable
Gateway health
Live with fixture fallback
Failed runs (sample)
No kill switch
Backend-waiting list APIs
The parameter already belongs to a collection
```

## Produce an audit note

Create or update a local implementation note, for example:

```text
docs/reviews/OPERATOR_UX_TAXONOMY_001_AUDIT.md
```

For each major finding, record:

- file path;
- rendered phrase/component;
- taxonomy dimension it should map to;
- whether it is a blocker or polish;
- proposed migration.

Do not edit UI until this audit is complete.
