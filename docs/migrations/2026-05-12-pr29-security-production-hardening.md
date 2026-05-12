# Migration note: PR29 security production hardening

Date: 2026-05-12

## What changed

`Ontogony.Security` adds production hardening mechanics for service identity HMAC verification:

- new key-id header support (`X-Ontogony-Service-Key-Id`)
- multi-secret resolver contract for current/previous key rotation
- outbound request signing handler (`OntogonyServiceIdentitySigningHandler`)
- middleware-order diagnostics for body-hash preload placement

## Required actions for adopters

1. Decide key-id policy:
   - set `RequireKeyIdForHmacSignature = true` to require explicit key-id on all signed calls, or
   - keep `false` and ensure resolver current-key behavior is intentional.
2. Register `IServiceSigningSecretResolver` for rotation windows if using multiple active secrets.
3. For body-bearing signed requests in production:
   - call `UseOntogonyServiceIdentityBodyHashPreload()` before routing/body-consuming middleware
   - set `RequirePreloadedBodyHashForHmacBodies = true`.
4. Use `OntogonyServiceIdentitySigningHandler` on outbound service clients to avoid custom per-client canonicalization code.
5. Use a distributed `INonceReplayStore` in clustered environments.

## Compatibility notes

- Existing `IServiceSecretResolver` integrations continue to work through compatibility fallback (`legacy-current` key-id behavior).
- Static shared-secret mode (`RequireSignatureVerification`) remains unchanged.

## Verification

Run:

- `dotnet restore Ontogony.Platform.sln`
- `dotnet build Ontogony.Platform.sln --no-restore`
- `dotnet test Ontogony.Platform.sln --no-build`
- `$env:PACKAGE_VERSION="0.2.0-local"`
- `./scripts/pack-all.ps1 -NoBuild`
