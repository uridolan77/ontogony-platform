# KCP-001 — Assistance safety and trust-boundary polish

## Problem

The Kanon Conexus Assistance workbench currently shows a secret-like sample field/value and confusing allowed-vs-redacted field defaults.

## Scope

- Replace default sample JSON.
- Replace allowed fields default.
- Add redaction preview.
- Improve draft-only trust boundary.
- Improve result links.
- Add tests.

## Implementation details

Default context:

```json
{
  "summary": "Operator review context for a non-secret semantic change.",
  "sourceType": "domain-pack",
  "changeIntent": "review-notes"
}
```

Default allowed fields:

```text
summary,sourceType,changeIntent
```

Default force-redact fields:

```text

```

Forbidden in defaults:

```text
apiKey
secret
token
password
credential
privateKey
secret-live-key
```

Redaction preview should show:

- included fields;
- redacted fields;
- omitted fields;
- warning if context becomes empty after filtering.

## Acceptance

- Assistance sample contains no secret-like fields/values.
- Draft-only/non-authoritative status remains visible above the submit button and in the result area.
- Tests prevent regression.
