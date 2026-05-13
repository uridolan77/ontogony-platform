# Secret reference resolver policy

`ISecretValueResolver` is now a platform mechanical port. It must remain provider-neutral and cloud-neutral.

## Current policy

- `AddOntogonySecrets()` does not auto-register a value resolver.
- `AddOntogonyEnvironmentSecretValueResolver()` explicitly registers the `env` scheme.
- `CompositeSecretValueResolver` resolves in deterministic constructor order.
- Resolved values are sensitive and must not be logged.

## Next useful platform helper

Add a parser for string references:

```text
env:CONEXUS_PROVIDER_OPENAI_API_KEY
vault:providers/openai/api-key
aws-sm:prod/openai
```

The parser should only split/validate the scheme and locator. It must not include cloud SDKs or provider semantics.
