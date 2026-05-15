# Ontogony.Platform — Next Phase Plan Package

This package defines the next infrastructure phase for `ontogony-platform`.

Core rule:

```text
Ontogony.Platform owns mechanics.
Kanon owns meaning.
Agentor owns execution.
Conexus owns model access.
```

## Recommended immediate PRs

```text
PLAT-INT-001   Service-to-service integration conventions
PLAT-OBS-001   Integration metrics helper
PLAT-TEST-001  Architecture test helpers
```

## Later / conditional PRs

```text
PLAT-PG-001    Shared Postgres idempotency ledger provider
PLAT-PG-002    Shared Postgres execution journal provider
PLAT-ART-001   Durable artifact store provider
PLAT-HOST-001  Readiness contributor helper
```

## Non-goals

Do not add Kanon semantics, Agentor execution semantics, Conexus provider-routing semantics, provider SDKs, business approval rules, domain-pack logic, or UI code.
