# Ontogony.Quotas — semantic contract

**Status:** Proposed platform module.

## Guarantees

- Mechanical quota scopes, windows, limits, and decisions.
- In-memory ledger for tests/single-process hosts.
- Provider-neutral metrics such as requests/tokens/cost.

## Does not guarantee

- Distributed rate limiting.
- Pricing logic.
- Billing.
- Provider routing.
- Product-specific quota policy.

## Conexus.NET use

Use for project-level RPM/TPM/spend/concurrency limits; Conexus owns policy and persistence.
