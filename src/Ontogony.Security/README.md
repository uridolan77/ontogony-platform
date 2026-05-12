# Ontogony.Security

**Service identity** HMAC signing, **current actor** accessors, and related ASP.NET integration hooks.

## What this is

- `OntogonyServiceIdentitySigningHandler`, `ServiceIdentityOptions`, header-based and claims-based actor context.
- `AddOntogonyHeaderActorContext`, `AddOntogonyServiceIdentityActorContext`, and related registration helpers.

## What this is not

- Not final authorization policy, tenant provisioning, or identity provider implementations.

## See also

- `docs/packages/Ontogony.Security.md`, `docs/security/index.md`.
