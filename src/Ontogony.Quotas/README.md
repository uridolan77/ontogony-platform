# Ontogony.Quotas

## What this is

Mechanical quota/rate-limit contracts and an in-memory ledger:

- quota scopes
- quota windows
- quota limits
- consumption requests
- decisions
- consumption records
- in-memory reference ledger

## What this is not

- not Conexus pricing
- not provider routing
- not a distributed rate limiter
- not billing
- not product policy

Production services should back `IQuotaLedger` with durable/distributed storage.
