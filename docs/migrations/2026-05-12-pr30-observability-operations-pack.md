# Migration Note — PR30 Observability Operations Pack

Date: 2026-05-12
PR: PR30

## Summary

PR30 adds operational observability documentation and local collector assets so services can verify traces and metrics end-to-end in development and burn-in environments.

No runtime package API or behavior changes were introduced.

## What was added

- `docs/observability/operations-pack.md`
- `docs/observability/metrics-catalog.md`
- `docs/observability/trace-attributes.md`
- `docs/observability/dashboard-queries.md`
- `docs/observability/alerts.prometheus.rules.yml`
- `docs/observability/trace-header-burn-in-checks.md`
- `docs/observability/local-collector/docker-compose.yml`
- `docs/observability/local-collector/otel-collector-config.yaml`
- `docs/observability/local-collector/prometheus.yml`

## Package docs updated

- `docs/packages/Ontogony.Observability.md`
- `docs/packages/Ontogony.Http.md`

## Test coverage added

- `tests/Ontogony.Observability.Tests/DiagnosticsContractSmokeTests.cs` to pin stable:
  - activity source and meter identity (`Ontogony.Platform`)
  - metric instrument names (`ontogony.*`)
  - span attribute keys (`ontogony.*`)

## Required action for consumers

- No mandatory code change.
- Recommended: adopt the local collector stack and queries for pre-production observability checks.
- Recommended: use the trace header burn-in checklist before disabling legacy header echoes in services that still depend on them.

## Breaking change assessment

- Breaking changes: none.
- Behavior changes: none in runtime libraries.
- Documentation and operational guidance only.
