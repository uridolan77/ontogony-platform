# Security and Redaction Alignment

## Backend rule

Backends should not return raw secrets in operator/admin endpoints.

Sensitive examples:
- Authorization headers
- API keys
- provider keys
- bearer tokens
- JWTs
- raw prompts if sensitive
- tool credentials
- connection strings
- filesystem paths if sensitive
- user PII unless explicitly required

## Frontend rule

The frontend keeps defense-in-depth:
- builder-level redaction where possible
- `safeDisplayJson` at rendering/export
- test corpus for common secret patterns
- issued Conexus key never enters diagnostics/evidence

## Backend test requirement

Each backend operator endpoint should have tests proving:
- known secret values are not returned
- error messages are operator-safe
- metadata is safe or redacted
- details dictionary does not include headers/secrets
