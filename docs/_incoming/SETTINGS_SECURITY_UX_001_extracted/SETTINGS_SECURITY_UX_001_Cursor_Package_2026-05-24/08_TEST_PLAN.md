# Test Plan

## Unit tests

### Credential source taxonomy

Test:

```ts
describeCredentialSource("local_browser")
describeCredentialSource("session")
describeCredentialSource("env_injected")
describeCredentialSource("unknown_legacy")
```

Assertions:

- Has label.
- Has description.
- Has severity.
- Does not render `unknown source`.

### Actor presets

Test:

- `kanon_readonly` maps to `Auditor, ProvenanceReader`.
- `local_admin` maps to `Admin`.
- `system_service` maps to `System`.
- Custom profile preserves manual values.

### Redaction preview

Inputs:

```json
{
  "summary": "Review this pack.",
  "apiKey": "sk-test",
  "connectionString": "Host=...",
  "nested": {
    "token": "abc"
  }
}
```

Expected:

- `summary` kept.
- `apiKey`, `connectionString`, `nested.token` removed.
- Reasons include `secret_like_field_name`.
- Outbound preview contains no secret values.

### Diagnostics export

Test export with configured mock keys.

Expected:

- `containsRawSecrets: false`
- `redactionApplied: true`
- no raw key strings appear in serialized JSON.

## Component tests

### Settings page

Assertions:

- One local credential storage notice.
- Credential rows show source labels.
- Role presets visible.
- Diagnostics privacy notice visible.
- Execution posture has blocked/sandbox/kill-switch labels.

### Kanon actor panel

Assertions:

- Does not contain `Allagma defaults`.
- Does not contain `trusts headers`.
- Contains `local operator mode`.
- Shows capability grants.

### Conexus Assistance

Assertions:

- Safe sample context is present.
- No `secret-live-key`.
- No sample `apiKey`.
- Redaction preview renders before submit result.

### Evidence / Agent Actor Labels

Assertions:

- Current operator actor label exists.
- Historical run actor label exists when run actor exists.
- No ambiguous actor-only label in multi-actor views.

## E2E / Playwright

Recommended tests:

1. Open Settings.
2. Verify credentials section has one warning.
3. Apply read-only Kanon preset.
4. Save settings.
5. Open Kanon Overview.
6. Verify actor roles are Auditor/ProvenanceReader and write capability is denied or clearly limited.
7. Return Settings.
8. Apply Local Admin preset.
9. Open Conexus Assistance.
10. Verify redaction preview removes secret-like field in a synthetic payload.
11. Export diagnostics.
12. Assert downloaded/copied JSON has privacy metadata and no secret material.

## Smoke scripts

Use scripts in `scripts/smoke/` for static copy checks and diagnostics redaction checks.
