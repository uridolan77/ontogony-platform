# Backend and Contract Plan

This package is frontend-first. Backend changes should be small and only when needed to support truthful settings/security UX.

## Potential backend support

### 1. Capability introspection

If existing APIs cannot tell the frontend what current roles can do, keep this sprint frontend-estimated. Do not build full RBAC.

Optional future endpoint:

```http
GET /ontology/v0/auth/capabilities
```

Response:

```json
{
  "actorId": "local-operator",
  "roles": ["Admin"],
  "capabilities": {
    "domainPackRead": true,
    "domainPackValidate": true,
    "domainPackLoad": true,
    "domainPackPromote": true,
    "provenanceRead": true
  },
  "source": "request_actor_headers",
  "mode": "local_operator"
}
```

### 2. Provider credential posture

If Conexus can expose provider readiness without secret material, settings can show:

```json
{
  "providerKey": "openai",
  "runtimeRegistered": true,
  "configPresent": true,
  "secretResolvable": false,
  "secretSource": "env_injected",
  "isReady": false
}
```

This likely already exists in provider inventory; reuse it.

### 3. Diagnostics export contract

No backend needed if export is frontend-generated. If backend participates, standardize privacy metadata.

```json
{
  "schemaVersion": "operator-diagnostics.v1",
  "generatedAtUtc": "...",
  "privacy": {
    "containsClientDiagnostics": true,
    "containsRawSecrets": false,
    "redactionApplied": true
  },
  "settings": {
    "credentials": [
      {
        "name": "conexusProjectApiKey",
        "configured": true,
        "source": "local_browser",
        "rawValueIncluded": false
      }
    ]
  }
}
```

## Redaction contract

Recommended redaction reason codes:

```ts
type RedactionReason =
  | "allowlist_excluded"
  | "force_redact_field"
  | "secret_like_field_name"
  | "secret_like_value"
  | "oversize_value"
  | "unsupported_value_type";
```

Redaction preview shape:

```json
{
  "schemaVersion": "redaction-preview.v1",
  "safeToSend": true,
  "kept": [
    { "path": "summary", "reason": "allowed_field" }
  ],
  "removed": [
    { "path": "apiKey", "reason": "secret_like_field_name" }
  ],
  "outboundPreview": {
    "summary": "Operator review context."
  }
}
```

## Security constraints

- Do not return raw secrets from services.
- Do not include raw secrets in diagnostics export.
- Do not log redaction input with secret values.
- Do not make browser-stored credentials sound production-safe.
- Do not claim local actor headers are production authentication.
