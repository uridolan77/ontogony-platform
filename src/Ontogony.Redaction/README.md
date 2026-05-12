# Ontogony.Redaction

## What this is

Mechanical redaction primitives:

- stable sensitive field-name matching
- deterministic value masking
- default redactor
- safe redacted value DTOs
- DI registration

## What this is not

- not a privacy classification engine
- not legal/compliance advice
- not PII discovery with ML
- not product policy
- not a guarantee that raw payloads are safe to log

Use this package before logging prompts, responses, provider errors, API keys, headers, or metadata.
