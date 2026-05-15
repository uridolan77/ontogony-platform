# Recommended PR Sequence

## PLAT-INT-001 — Service-to-service integration conventions

Define shared mechanics for service-to-service calls:

```text
header names
actor propagation
trace/correlation propagation
idempotency-key convention
typed HTTP client registration helper
mechanical integration error classification
```

No product clients.

## PLAT-OBS-001 — Integration metrics helper

Make outbound integration metrics easy and consistent:

```text
ontogony.integration.call.count
ontogony.integration.error.count
ontogony.integration.duration.ms
```

## PLAT-TEST-001 — Architecture test helpers

Extract reusable forbidden-dependency tests into `Ontogony.Testing`.

Support:

```text
PackageReference
ProjectReference
PackageVersion
Directory.Build.props
Directory.Build.targets
Directory.Packages.props
forbidden using directives
```

## Later extraction PRs

Only after reuse by at least two services:

```text
PLAT-PG-001   Shared Postgres idempotency ledger
PLAT-PG-002   Shared Postgres execution journal
PLAT-ART-001  Durable artifact store
PLAT-HOST-001 Readiness contributor helper
```
