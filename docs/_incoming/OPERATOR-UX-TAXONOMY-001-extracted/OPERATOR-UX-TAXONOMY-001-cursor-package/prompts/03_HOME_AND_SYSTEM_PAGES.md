# Cursor prompt — 03 migrate Home/System pages

Migrate operator Home, protocol truth cockpit, system compatibility, and topology readiness to the shared taxonomy.

## Fix these patterns

- `All services are healthy` when one service is not ready or contract-warning.
- `Service versions appear aligned` at the same level as `compatibility unknown`.
- `Live with fixture fallback` as page headline.
- `health payload format warning` without field-level reason.
- planned topology links that look implemented.

## Required result

Home/System should show:

```text
Connectivity
Readiness
Contract health
Operator usability
Compatibility verification
Data source
```

System compatibility should separate:

- protocol registry state;
- runtime compatibility artifact state;
- service version metadata state;
- overall compatibility state.

Topology edges should show expected route(s), last attempt, last success, trace/correlation/evidence when available, and state.
