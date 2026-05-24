# 06 — Operator settings migration plan

## Current strengths to preserve

Current operator settings already have useful structure:

- Safe settings are stored separately from raw credentials.
- Credential persistence mode is explicit: `session-only` vs `local-persistent`.
- Local persistent credentials require consent.
- Legacy monolithic settings are migrated on read.
- Settings page already uses canonical UI panels and local-alpha warnings.

Do not replace this with a new environment-management subsystem.

## Problem to solve

Runtime config introduces defaults that can change after the app is built. Existing local settings currently store a full `OperatorSettings` object. If a user previously saved `http://localhost:5082`, and runtime config later says `http://localhost:6092`, the old local value can still win silently.

The migration must make that explicit.

## Minimal storage extension

Add a small provenance/override metadata storage key:

```text
ontogony.frontend.operator-settings.provenance.v1
```

Shape:

```ts
export type DefaultableOperatorField =
  | "environmentLabel"
  | "conexus.baseUrl"
  | "conexus.projectId"
  | "conexus.modelAlias"
  | "kanon.baseUrl"
  | "kanon.ontologyId"
  | "kanon.ontologyVersionId"
  | "allagma.baseUrl"
  | "allagma.defaultActorId"
  | "allagma.defaultActorRoles";

export type OperatorSettingsOverrideMetadata = {
  version: 1;
  fields: Partial<Record<DefaultableOperatorField, {
    source: "local-override" | "legacy-local";
    firstSeenAt: string;
    lastUpdatedAt: string;
  }>>;
};
```

This avoids a full rewrite of `OperatorSettings`. It makes current fields source-aware.

## Read path

1. Load `RuntimeConfigSnapshot`.
2. Build runtime defaults as an `OperatorSettings` object.
3. Read current safe settings blob if present.
4. Read override metadata if present.
5. Merge values and compute provenance.
6. Apply credential secrets according to existing credential persistence rules.

Pseudo-flow:

```ts
const runtimeDefaults = buildDefaultOperatorSettings(runtimeConfig);
const safe = readSafeSettings(runtimeDefaults);
const overrideMetadata = readOverrideMetadata();
const { settings, provenance } = mergeOperatorSettingsWithRuntimeDefaults({
  runtimeDefaults,
  safe,
  overrideMetadata,
  hadSafeSettingsBlob,
});
const secrets = readCredentialSecrets(settings.credentials.persistenceMode);
return applyCredentialSecrets(settings, secrets);
```

## Write path

When a defaultable field is changed by the user and saved:

- if the value differs from runtime default, mark `local-override`;
- if the value equals runtime default, remove the override marker;
- credentials continue using existing credential storage;
- appearance/accessibility can remain normal settings without runtime provenance.

## UI changes

### Service connection panels

For Conexus/Kanon/Allagma, show compact provenance under base URL:

```text
Source: Runtime default — docker-local
```

or:

```text
Source: Local override — differs from runtime default http://localhost:5082
```

or:

```text
Source: Legacy local value — review and keep or use runtime default
```

Controls:

- `Use runtime default` when field has any override.
- `Clear local override` same behavior but clearer in technical disclosure.
- `Override locally` reveals or focuses the base URL input when currently runtime default.

### Runtime config details disclosure

Add a disclosure in Settings, likely under “Environment & diagnostics”:

Title: `Runtime config details`

Content:

- status: loaded/missing/invalid/fallback;
- source URL;
- environmentName;
- profileName;
- generatedAt;
- service runtime defaults;
- validation warnings/errors;
- statement: `Runtime config does not contain credentials or provider secrets.`

### Environment summary

Add rows:

```text
Runtime profile      docker-local
Runtime config       loaded
Conexus source       local override / runtime default
Kanon source         runtime default
Allagma source       runtime default
```

Avoid one badge per field in the main page. Use text rows and one compact status pill.

### Local dev defaults panel

Replace or supplement current hardcoded `Apply local stack URLs` with:

- `Reset all service URLs to runtime defaults`
- `Apply classic localhost stack URLs` as secondary/legacy action if still useful

The existing hardcoded button currently sets `5081/5082/5083`; after runtime config it should not be the primary action.

### Danger zone

Keep existing `Erase persisted operator settings` behavior. Add a safer action outside danger zone:

```text
Clear local runtime overrides
```

This clears defaultable local overrides only. It does not clear credentials.

## Domain Switcher behavior

Domain Switcher should continue to persist Kanon ontology selection. When the operator changes ontology id/version from runtime default, mark those fields as local overrides. Clearing the override returns to runtime default ontology id/version.

## Credentials behavior

Do not put credentials into runtime config. Do not change existing credential-storage warnings except to respect runtime config policy fields:

- `localAlpha.allowBrowserCredentialStorage`
- `localAlpha.showLocalCredentialWarnings`

If `allowBrowserCredentialStorage=false`, hide/disable local-persistent option and keep session-only storage. This is a local profile policy, not production security.

## Legacy migration display

When legacy-safe settings differ from runtime defaults, show one message:

```text
Some service/profile values were migrated as local overrides because they differ from the runtime profile. Review them below or reset them to runtime defaults.
```

Do not show one warning per field.

## Tests

Add tests for:

- local settings absent → runtime defaults;
- local settings present and equal runtime defaults → runtime default provenance;
- local settings present and different → legacy-local provenance;
- save legacy value → local-override provenance;
- clear override → runtime-default provenance;
- reset all service URLs → runtime defaults and credentials untouched;
- reset all local overrides → defaultable fields reset, credentials untouched;
- danger reset → existing broad behavior still works.
