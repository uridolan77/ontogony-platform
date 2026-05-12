# Ontogony.Security — semantic contract

**Status:** Production-safe when configured correctly. **Header actor** and **static shared-secret** service identity modes are **not** substitutes for edge authentication.

## Guarantees

- `CurrentActor` projection from claims, headers, or service identity headers using explicit, testable rules.
- Constant-time comparison for static secret and HMAC signature bytes.

## Does not guarantee

- That header-based actor context is authentic unless a **trusted upstream** or verified token flow established trust.
- That static shared-secret signature mode resists disclosure if the secret is observed (use **HMAC** mode for service-to-service requests on untrusted networks).

## Related

- [../security/service-identity.md](../security/service-identity.md)
- [../security/service-identity-production.md](../security/service-identity-production.md)
- [../invariants.md](../invariants.md)
