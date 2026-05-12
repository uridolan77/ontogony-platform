# Ontogony.Persistence.Postgres

Npgsql-backed **durable** implementation of `Ontogony.Persistence` outbox and processed-message contracts.

## What this is

- `PostgresOutboxStore` — write, read, claim-lease dispatch, processed-message tracking aligned with `OutboxContracts`.
- `AddOntogonyPostgresOutbox(Action<PostgresOutboxOptions>)` — register the store and optional schema initializer.

## What this is not

- Not a general-purpose ORM, migrations framework for your domain, or product-specific repository layer.
- Not in-process or multi-primary replication policy (operational choices stay with the host).

## See also

- `docs/persistence/postgres-outbox-provider.md`, `docs/packages/Ontogony.Persistence.Postgres.md`.
