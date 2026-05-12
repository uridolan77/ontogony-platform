# PR30 — Observability Operations Pack

## Goal

Make observability not just available in code, but operationally usable.

## Scope

Add docs/examples for:

- OpenTelemetry exporter wiring
- local collector docker-compose sample
- dashboard JSON or documented dashboard queries
- alert rules
- log correlation examples
- trace header migration burn-in checks

Add package docs updates for `Ontogony.Observability` and `Ontogony.Http` metrics.

## Metrics catalog

Create:

```text
docs/observability/metrics-catalog.md
```

For each metric:

- name
- type
- dimensions
- when emitted
- example query
- alert idea

## Trace catalog

Create:

```text
docs/observability/trace-attributes.md
```

Document all `OntogonySpanAttributes`.

## Tests

- Existing tests should cover code.
- Add at least one smoke test/example confirming meter names/attribute keys are stable if feasible.

## Acceptance

A new service can follow docs and see traces/metrics locally through an OTel collector.
