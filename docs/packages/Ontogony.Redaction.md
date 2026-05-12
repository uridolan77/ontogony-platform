# Ontogony.Redaction — semantic contract

**Status:** Proposed platform module.

## Guarantees

- Deterministic mechanical masking of sensitive strings.
- Default sensitive field-name rules.
- Extensible field-name rule list.
- No external service dependency.

## Does not guarantee

- Complete PII discovery.
- Regulatory compliance.
- Semantic safety of prompts/responses.
- Product-specific policy.

## Conexus.NET use

Use before logging provider errors, headers, prompts, responses, tool outputs, provider keys, and metadata.
