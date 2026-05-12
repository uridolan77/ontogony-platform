# Ontogony.Redaction — semantic contract

**Status:** Shipping (pre-1.0).

## Guarantees

- Deterministic mechanical masking of sensitive strings.
- Default sensitive field-name rules.
- Extensible field-name rule list.
- No external service dependency.
- `RedactionResult` exposes only safe output (`Value`); it does not retain the pre-redaction string.

## Does not guarantee

- Complete PII discovery.
- Regulatory compliance.
- Semantic safety of prompts/responses.
- Product-specific policy.

## Conexus.NET use

Use before logging provider errors, headers, prompts, responses, tool outputs, provider keys, and metadata.
