# Ontogony.Secrets — semantic contract

**Status:** Proposed platform module.

## Guarantees

- Provider-neutral secret references and metadata.
- Safe display/masking helpers.
- Stable fingerprinting helper.
- Development-only protector for tests/local usage.

## Does not guarantee

- Production encryption.
- Vault/KMS integration.
- Authorization.
- Secret rotation business policy.

## Conexus.NET use

Use for provider credential references, safe BO display, secret fingerprints, and a port for production secret protection.
