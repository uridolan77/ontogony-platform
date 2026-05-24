# Implementation Plan

## Phase 1 — Inventory and taxonomy

1. Find Settings page components and settings provider/types.
2. Find credential warning components.
3. Find diagnostics export builder.
4. Find Kanon actor panel(s).
5. Find Conexus Assistance page and redaction/allowlist logic.
6. Find Evidence Spine / Agent Interaction actor display components.
7. Find model-purpose/alias settings display.

Add a shared settings/security taxonomy module.

Candidate frontend paths:

```text
ontogony-frontend/src/app/settings/
ontogony-frontend/src/system/pages/OperatorSettingsPage.tsx
ontogony-frontend/src/system/diagnostics/
ontogony-frontend/src/kanon/components/
ontogony-frontend/src/kanon/pages/
ontogony-frontend/src/kanon/assistance/
ontogony-frontend/src/evidence-spine/
ontogony-frontend/src/allagma/
```

Candidate UI package paths:

```text
ontogony-ui/src/security/
ontogony-ui/src/status/
ontogony-ui/src/forms/
```

## Phase 2 — Credential-source taxonomy

Introduce a strongly typed enum:

```ts
export type OperatorCredentialSource =
  | "not_set"
  | "default_dev"
  | "session"
  | "local_browser"
  | "env_injected"
  | "service_configured"
  | "unknown_legacy";
```

Add mapper:

```ts
describeCredentialSource(source): {
  label: string;
  description: string;
  severity: "neutral" | "info" | "warning" | "danger";
}
```

Rules:

- `unknown_legacy` is allowed only as migration fallback.
- New code should never emit bare `unknown source`.
- Env-injected/service-configured values must not expose raw values to browser.

## Phase 3 — Settings warning consolidation

Replace repeated warnings with one high-level warning component:

```text
Local credential storage
```

Per credential row receives compact tags:

```text
configured · local browser · local-alpha
```

Add tests to verify only one local-storage warning appears.

## Phase 4 — Actor presets

Add preset definitions:

```ts
type OperatorActorPresetId =
  | "kanon_readonly"
  | "local_admin"
  | "system_service"
  | "custom";
```

Implement:

- Preset selector.
- Apply preset button.
- Clear distinction between selected preset and custom.
- Capability summary based on current roles.

Do not silently overwrite custom actor context without user action.

## Phase 5 — Kanon actor wording and capability panel

Replace duplicated prose with compact component:

```text
Actor: local-operator · Roles: Admin · Kanon write: granted
```

Expandable details:

```text
Local operator actor context
...
```

Remove “Allagma defaults” from Kanon pages.

Replace “trusts headers” language.

## Phase 6 — Redaction preview

Add redaction preview to Conexus Assistance.

Required functions:

```ts
buildRedactionPreview(input, policy): RedactionPreview
renderRedactionPreview(preview)
```

Policy should include:

- allowlist
- force-redact fields
- secret-like field names
- nested fields
- max preview size
- redaction reason codes

Never send data before preview has been rendered in the page flow. It may be acceptable to auto-render preview; do not require extra confirmation unless current UX already does.

## Phase 7 — Safe sample data

Remove secret-looking sample values and field names from:

- Conexus Assistance sample context.
- Allowed fields examples.
- Settings defaults.
- Docs/comments visible in UI.

Search for:

```text
apiKey
secret-live-key
password
token
connectionString
secret
```

Do not remove legitimate redaction tests; keep those in test files only.

## Phase 8 — Diagnostics export privacy metadata

Add diagnostics export metadata:

```json
"privacy": {
  "containsClientDiagnostics": true,
  "containsRawSecrets": false,
  "redactionApplied": true,
  "clientFields": ["userAgent", "viewport", "timezone"],
  "secretHandling": "raw secrets omitted"
}
```

Add visible notice near export controls.

## Phase 9 — Actor provenance labels

Update Evidence Spine / Agent Interaction / Allagma / Kanon displays to label actors:

- Current operator actor
- Request actor
- Historical run actor
- Service actor

Avoid plain `Actor` when multiple contexts exist.

## Phase 10 — Model naming cleanup

Use precise labels:

- Model purpose
- Conexus alias
- Provider key
- Provider model
- Routing default

Search UI copy for concrete model names in settings. If a concrete model appears, label it provider model or move to Conexus route detail.

## Phase 11 — Tests and QA

Add tests:

- Credential-source labels.
- No repeated local-storage warnings.
- Actor preset apply.
- Kanon wording does not include “Allagma defaults” or “trusts headers”.
- Redaction preview removes secret-looking fields.
- Assistance sample contains no secret-like fields.
- Diagnostics export privacy metadata exists and contains no raw secret values.
- Actor labels distinguish current/historical actors.
