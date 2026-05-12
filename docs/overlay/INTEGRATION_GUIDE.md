# Integration guide — PR48–PR52 platform modules

## Recommended PR split

Use one PR per module if you want clean review history:

- PR48 — `Ontogony.Logging`
- PR49 — `Ontogony.Redaction`
- PR50 — `Ontogony.Secrets`
- PR51 — `Ontogony.Quotas`
- PR52 — `Ontogony.Replay.Contracts`

Or merge as one platform-hardening PR before Conexus.NET if speed matters.

## Recommended package levels

This overlay assumes the package-level model is corrected as follows:

```text
Level 0 — Pure foundation
  Ontogony.Primitives
  Ontogony.Configuration

Level 0.5 — Shared representation and deterministic mechanics
  Ontogony.Contracts
  Ontogony.Hashing

Level 1 — Service mechanics
  Ontogony.Hosting
  Ontogony.Observability
  Ontogony.Errors
  Ontogony.Http
  Ontogony.Security
  Ontogony.Logging
  Ontogony.Redaction
  Ontogony.Secrets

Level 2 — Event, consistency, and resource-control mechanics
  Ontogony.Messaging
  Ontogony.Idempotency
  Ontogony.Persistence
  Ontogony.Persistence.Postgres
  Ontogony.ProtocolIngress
  Ontogony.Quotas

Level 3 — AI runtime and replay mechanics
  Ontogony.AI.Contracts
  Ontogony.Artifacts
  Ontogony.Execution
  Ontogony.Replay.Contracts

Aggregate / test-only
  Ontogony.Testing
```

## New package dependency edges

```text
Ontogony.Logging -> Ontogony.Observability
Ontogony.Redaction -> no Ontogony package dependencies
Ontogony.Secrets -> Ontogony.Redaction
Ontogony.Quotas -> Ontogony.Primitives
Ontogony.Replay.Contracts -> no Ontogony package dependencies
```

## Conexus.NET usage intent

Conexus.NET should directly reference:

```text
Ontogony.Hosting
Ontogony.Observability
Ontogony.Errors
Ontogony.Http
Ontogony.Security
Ontogony.Logging
Ontogony.Redaction
Ontogony.Secrets
Ontogony.Idempotency
Ontogony.Hashing
Ontogony.Contracts
Ontogony.AI.Contracts
Ontogony.Artifacts
Ontogony.Execution
Ontogony.Quotas
```

`Ontogony.Replay.Contracts` can be adopted once request replay/debug bundles are implemented.
