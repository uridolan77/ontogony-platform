# Export and redaction boundary

## Export target

Add an evidence spine export bundle:

```text
ontogony-cross-service-evidence-spine-bundle-v1
```

## Bundle contents

```json
{
  "schema": "ontogony-cross-service-evidence-spine-bundle-v1",
  "exportedAt": "...",
  "rootInput": "...",
  "graph": {},
  "sourceAttempts": [],
  "warnings": [],
  "redaction": {
    "policy": "operator-redacted",
    "rawSecretsIncluded": false
  }
}
```

## Redaction rules

Do not include:
- provider API keys
- service tokens
- raw project API keys
- raw prompts or completions unless already present in an explicitly exportable redacted evidence bundle
- oversized raw DTOs by default

Prefer:
- identifiers
- status
- timestamps
- route/provider keys
- model alias/model name
- token counts
- error codes
- redacted summaries
- links to existing bundles
