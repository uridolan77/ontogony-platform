# PR29 — Security Production Hardening

## Goal

Move `Ontogony.Security` from solid mechanics to production-operational mechanics for service identity.

## Scope

Add:

- key-id header for HMAC signatures
- multi-secret resolver contract
- signing client delegating handler
- verification vectors
- distributed nonce store provider guidance/sample
- middleware-order diagnostics

Suggested APIs:

```csharp
public interface IServiceSigningSecretResolver
{
    ServiceSigningSecretSet ResolveSecrets(string serviceId, string? keyId);
}

public sealed record ServiceSigningSecret(string KeyId, string Secret, bool IsCurrent);
public sealed record ServiceSigningSecretSet(...);
```

Client-side handler:

```csharp
OntogonyServiceIdentitySigningHandler
```

## Must not do

- Do not implement product authorization policy.
- Do not invent business roles.
- Do not store secrets in source code.

## Tests

- Current key validates.
- Previous key validates during rotation window.
- Unknown key-id fails.
- Missing key-id behavior is explicit.
- Signing handler creates correct canonical signature.
- Body hash matches server verification.
- Replay nonce fails.
- Clock skew fails.

## Docs

- `docs/security/service-identity-production.md`
- HMAC signing vector doc
- secret rotation sequence diagram
- production deployment checklist

## Acceptance

Two services can sign/verify a request using current/previous secrets with replay protection and deterministic test vectors.
