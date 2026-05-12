# Ontogony.Idempotency

Mechanical **idempotency ledger** contracts and fingerprint helpers for safe retries.

## What this is

- `IIdempotencyLedger` — try begin / mark succeeded / mark failed.
- `IdempotencyKeyBuilder` — deterministic keys from operation name and opaque parts.
- `InMemoryIdempotencyLedger` — **tests and single-process** implementation.

## What this is not

- Not business deduplication rules (“same order twice” semantics); hosts define key composition and TTL policy.

## See also

- `docs/packages/Ontogony.Idempotency.md`.
