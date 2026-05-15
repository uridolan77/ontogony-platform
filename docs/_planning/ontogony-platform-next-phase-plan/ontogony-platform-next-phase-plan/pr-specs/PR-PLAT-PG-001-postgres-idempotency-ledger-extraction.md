# PR-PLAT-PG-001 — Shared Postgres Idempotency Ledger Provider

## Status

Later / conditional.

## Trigger

Do only when Conexus, Agentor, or another service needs the same durable provider.

## Scope

Extract generic durable idempotency ledger mechanics into platform.

## API sketch

```csharp
services.AddOntogonyPostgresIdempotencyLedger(options =>
{
    options.Schema = "ontogony_platform";
    options.Table = "idempotency_ledger";
});
```

## Non-goals

No Kanon operation names. No product semantic payload logic.
