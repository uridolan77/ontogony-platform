# Generic secret-value resolver

## Goal

Add reusable secret value resolution abstraction.

## Acceptance criteria

Conexus can resolve provider secrets through platform abstraction.

## Boundary checklist

- [x] Reusable platform mechanics only.
- [x] No Conexus routing/provider/model semantics.
- [x] CI/build/package validation updated where relevant.

## Implementation notes

`ISecretValueResolver` with `SecretValueReference` / `SecretValueResolveResult`, `EnvironmentVariableSecretValueResolver` (scheme `env`), `CompositeSecretValueResolver`, and `AddOntogonyEnvironmentSecretValueResolver()`. Conexus registers additional resolvers or composes `CompositeSecretValueResolver` for vault-specific schemes.
