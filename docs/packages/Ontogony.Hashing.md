# Ontogony.Hashing — semantic contract

**Status:** Production-safe for deterministic canonical JSON hashing and payload fingerprint helpers.

## Guarantees

- Stable hashing pipeline for **mechanical** fingerprints (`PayloadHasher`, `EnvelopePayloadHasher`, SHA-256 helpers).
- Canonical JSON ordering rules used by idempotency and envelope hashing stay versioned via package semver and changelog.

## Does not guarantee

- That a given hash implies business equivalence of two payloads.
- Secret management or HMAC signing for service identity (see `Ontogony.Security`).

## Related

- [../adr/0006-canonical-json-hashing.md](../adr/0006-canonical-json-hashing.md)
- [./Ontogony.Idempotency.md](./Ontogony.Idempotency.md)
