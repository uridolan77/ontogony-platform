# Ontogony.Secrets

## What this is

Mechanical secret handling contracts:

- `SecretRef`
- secret metadata and rotation state
- safe display masking
- secret fingerprinting
- development-only protector for local/test use
- optional **secret value** resolution (`ISecretValueResolver`, including `EnvironmentVariableSecretValueResolver` for scheme `env` via `AddOntogonyEnvironmentSecretValueResolver()`)

## What this is not

- not a production vault
- not cloud KMS integration
- not database encryption policy
- not provider credential lifecycle business logic
- not authorization

Production services should implement `ISecretProtector` using their real vault/KMS/DPAPI/Data Protection strategy.
