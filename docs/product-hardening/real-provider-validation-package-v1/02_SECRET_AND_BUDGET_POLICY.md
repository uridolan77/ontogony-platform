# 02 — Secret and Budget Policy

## Rules

Allowed:

- local `.env`
- local user secrets
- process environment variables
- secure password manager copy/paste into local shell

Forbidden:

- committed API keys
- committed encoded secrets
- real keys in `.env.example`
- keys in Dockerfiles
- keys in GitHub Actions
- raw keys in evidence
- CI-triggered real-provider calls

## Placeholder env pattern

Use existing Conexus config keys where they already exist. If new local-only keys are needed, follow this pattern:

```text
CONEXUS_REAL_PROVIDER_ENABLED=false
CONEXUS_REAL_PROVIDER_NAME=<provider>
CONEXUS_REAL_PROVIDER_MODEL=<small-model>
CONEXUS_REAL_PROVIDER_API_KEY=<local secret only>
CONEXUS_REAL_PROVIDER_MAX_CALLS=3
CONEXUS_REAL_PROVIDER_MAX_OUTPUT_TOKENS=256
```

## Initial budget

```text
max calls: 3
max output tokens: 256
model: small/cheap provider-supported model
manual only: true
CI: disabled
```

## Evidence may include

- provider name
- model name
- status codes
- route decision IDs
- model call IDs
- run/eval IDs
- token counts if available
- sanitized summaries

Evidence must not include API keys, bearer tokens, connection strings, raw sensitive prompts, or raw sensitive completions.
