# Idempotency Contract

**Owner:** `Ontogony.Idempotency`
**Schema:** `schemas/contracts/idempotency.schema.json`

---

## Headers

| Header | Canonical? | Notes |
| --- | --- | --- |
| `X-Ontogony-Idempotency-Key` | Yes | Preferred header on all unsafe methods that must be idempotent |
| `Idempotency-Key` | Legacy alias | Accepted inbound only; do not emit on outbound calls |

---

## Key structure

Keys are opaque strings. `IdempotencyKeyBuilder` produces deterministic keys from a scope + inputs
hash using `Ontogony.Hashing`.

```text
Format (recommended): {scope}/{sha256-hex-16}
Example: allagma/run-dispatch/3a7f29c1b84e5d20
```

---

## Ledger contract

`IIdempotencyLedger` requires implementors to:

1. Return the original response for a previously processed key.
2. Be atomic — no partial double processing.
3. Expire entries after a configurable TTL (default: 24 h).

---

## Propagation

When an inbound request carries an idempotency key and the service makes downstream calls as part
of the same logical operation, it must propagate `X-Ontogony-Idempotency-Key` (derived or the
same key) via `IntegrationHeadersDelegatingHandler`.

---

## Compliance requirements

- Allagma run dispatch, Kanon canonical-fact mutations, and Conexus provider registration must all
  accept and honour `X-Ontogony-Idempotency-Key`.
- The key must appear in `OntogonyPropagationHeaderContract.FrozenRequired` — any removal is a
  breaking change.
- Frontend must supply an idempotency key when retrying failed mutations via the operator console.
