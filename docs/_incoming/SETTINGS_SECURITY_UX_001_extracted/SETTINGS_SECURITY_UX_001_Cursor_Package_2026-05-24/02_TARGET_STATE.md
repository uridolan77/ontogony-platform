# Target State

## Operator settings target

The Settings surface becomes a precise local-alpha control panel, not a warning dump.

### Credential status

Each credential row uses a normalized source:

```text
not_set
default_dev
session
local_browser
env_injected
service_configured
unknown_legacy
```

Display examples:

```text
Conexus project API key
Status: configured
Source: local browser
Scope: this browser profile
Risk: local-alpha only
Actions: clear, rotate manually, test connection
```

```text
OpenAI provider key
Status: not visible to console
Source: env-injected on Conexus service
Scope: service process
Risk: not exported by diagnostics
```

The UI must not display raw secret values. Masked previews may show only a short non-sensitive fingerprint if already supported.

### Warning model

Replace repeated warnings with one persistent warning group:

```text
Local credential storage
Some operator credentials are stored in this browser for local development.
Do not use shared browsers. Diagnostics exports do not include raw secrets.
```

Then per-row risk labels are compact.

### Actor role presets

Provide explicit presets:

1. **Read-only semantic authority**
   - Actor: `local-operator`
   - Roles: `Auditor, ProvenanceReader`
   - Intended for: Kanon read/provenance inspection

2. **Local admin/operator**
   - Actor: `local-operator`
   - Roles: `Admin`
   - Intended for: local mutation/admin workflows

3. **System service**
   - Actor: `local-system`
   - Roles: `System`
   - Intended for: service-level local testing

4. **Custom**
   - Manual actor/role editing

Show:

```text
Current actor
local-operator

Current roles sent
Admin

Selected profile
Local admin/operator

Recommended read-only Kanon profile
Auditor, ProvenanceReader

Capability summary
Kanon domain-pack read: granted
Kanon domain-pack load: granted
Kanon provenance read: granted
```

### Kanon wording

Replace:

```text
Kanon trusts X-Ontogony-Actor-* and X-Ontogony-Roles from operator settings (Allagma defaults).
```

With:

```text
In local operator mode, the console sends actor context headers to Kanon.
Kanon authorizes requests according to the roles in that actor context.
```

Add:

```text
In non-local deployments, actor context must come from an authenticated gateway or trusted service boundary.
```

### Current vs historical actor labels

Evidence and run pages must distinguish:

```text
Current operator actor
local-operator

Historical run actor
env-seed-001-agent

Service actor
kanon-domain-pack-loader
```

No actor should appear unlabeled.

### Redaction preview

Before Conexus Assistance sends anything:

1. Show original field names, not sensitive values.
2. Show kept fields.
3. Show removed/redacted fields.
4. Show final outbound preview with sensitive values removed or masked.
5. Show reason: allowlist, force-redact pattern, secret-like key, size limit.

Example:

```text
Redaction preview
Kept fields:
- summary
- sourceType

Removed fields:
- apiKey (secret-like field name)
- connectionString (secret-like field name)

Outbound payload is safe to send to Conexus assistance.
```

### Assistance sample payload

Replace any sample containing `apiKey`, `secret`, `token`, `password`, `connectionString`, or `live-key`.

Safe sample:

```json
{
  "summary": "Operator review context.",
  "sourceType": "domain-pack",
  "packId": "gaming-core",
  "reviewGoal": "Find terminology inconsistencies before promotion."
}
```

### Diagnostics export

Add clear privacy notice:

```text
Diagnostics export contains local client diagnostics such as browser, viewport,
timezone, service URLs, health statuses, and redacted settings metadata.
It does not include raw API keys or raw secret values.
```

Export schema should include:

```json
{
  "privacy": {
    "containsClientDiagnostics": true,
    "containsRawSecrets": false,
    "redactionApplied": true
  }
}
```

### Execution posture

Compact posture:

```text
Real external execution: blocked
Local sandbox: disabled
Kill switch: not configured — local-alpha warning
Default execution mode: symbolic
```

Clicking details explains why.

### Model naming

Settings must use:

```text
Model purpose: summarize-player-risk
Conexus alias: risk-summary-v0
Provider route: resolved by Conexus
```

Avoid showing a concrete provider model as a user-editable default unless explicitly labeled.
