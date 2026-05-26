# 03 — Config precedence and provenance

## Precedence order

Later layers override earlier layers:

1. **Hardcoded safe fallback defaults** — used only if runtime config is unavailable or invalid.
2. **Runtime config defaults** — `/operator-runtime-config.json`.
3. **Browser-local persisted operator settings** — safe local settings plus optional persisted credentials, depending on current local-alpha preference.
4. **Session-only overrides** — session credentials and any future session-scoped non-secret overrides.
5. **Explicit URL/test overrides** — Playwright route mocks, query/test harness overrides, or e2e helpers.
6. **Page-local form overrides** — unsaved draft values inside Settings or other forms.

This means the effective app setting is not just a value. It is:

```ts
type ResolvedConfigValue<T> = {
  value: T;
  source: ConfigValueSource;
  defaultValue?: T;
  runtimeValue?: T;
  localValue?: T;
  sessionValue?: T;
  testValue?: T;
  isOverride: boolean;
  canResetToRuntimeDefault: boolean;
  detail?: string;
};
```

## Source vocabulary

Use one source vocabulary everywhere:

```ts
export type ConfigValueSource =
  | "fallback-default"
  | "runtime-default"
  | "local-override"
  | "session-override"
  | "test-override"
  | "page-draft"
  | "legacy-local"
  | "missing";
```

Operator-facing labels:

| Source | Label | Meaning |
|---|---|---|
| `runtime-default` | Runtime default | From `/operator-runtime-config.json`. |
| `local-override` | Local override | Persisted in this browser. |
| `session-override` | Session override | Applies only in this browser session. |
| `test-override` | Test override | Seeded by Playwright/smoke/test harness. |
| `page-draft` | Unsaved draft | Form value changed but not saved yet. |
| `legacy-local` | Legacy local value | Migrated from old full settings blob; operator should review. |
| `fallback-default` | Fallback default | Hardcoded safe fallback because runtime config missing/invalid. |
| `missing` | Missing/unset | No value set after merge. |

## Defaultable fields

These fields should participate in runtime-default provenance:

```text
environmentLabel
conexus.baseUrl
conexus.projectId
conexus.modelAlias
kanon.baseUrl
kanon.ontologyId
kanon.ontologyVersionId
allagma.baseUrl
allagma.defaultActorId
allagma.defaultActorRoles
frontend.defaultRoute
frontend.enableFixtureRoutes
frontend.enableExpertModeDefault
frontend.routeSearchEnabled
evidence.enableRawPreviewDefault
evidence.exportRedactionMode
diagnostics.enableDiagnosticExport
localAlpha.allowBrowserCredentialStorage
localAlpha.showLocalCredentialWarnings
```

Credential fields remain credential-provenance-driven, not runtime-config-driven.

## Reset semantics

### Use runtime default

For one field:

1. Remove the local/session/test override marker for that field.
2. Set effective value to runtime config value if runtime config is loaded.
3. If runtime config missing/invalid, set to safe fallback and mark `fallback-default`.
4. Persist the removal of the local override, not a copy of the runtime value.

### Clear local override

Same as `Use runtime default`, but only removes the browser-local layer. Session/test/page overrides may still win until cleared.

### Reset all service URLs to runtime defaults

Apply to:

```text
conexus.baseUrl
kanon.baseUrl
allagma.baseUrl
```

Do not clear credentials, project id, model alias, ontology selection, appearance, or actor profile.

### Reset all local overrides

Clears browser-local persisted values for all defaultable fields. It must not clear credentials unless the action explicitly says it clears credentials. Existing `Erase persisted operator settings` may remain broader; add a safer, non-destructive runtime-specific action.

## Migration semantics

Current storage persists a full `OperatorSettings` snapshot. RUNTIME-CONFIG-001 should avoid silently dropping user edits.

Migration rule:

1. Read current safe settings blob if present.
2. Build runtime defaults.
3. For each defaultable field:
   - if no local blob exists: source is runtime/fallback default;
   - if local value equals runtime default: source is runtime default;
   - if local value differs from runtime default: source is `legacy-local` on first read and behaves as a local override;
   - once user saves after reviewing, source becomes `local-override`.
4. Show one compact message: `Some values were migrated as local overrides because they differ from runtime config.`

## Missing runtime config display

Settings page top summary:

- `Runtime config loaded` — normal.
- `Runtime config missing; using safe fallback defaults` — warning/degraded.
- `Runtime config invalid; using safe fallback defaults` — warning/degraded with details in disclosure.
- `Runtime config stale` — warning if `generatedAt` is older than a configurable threshold for docker-local artifacts. Do not block app boot.

System Truth/Home may surface the config status as one readiness check, but should not show repeated warning cards on every page.

## Invalid runtime config display

Show:

- source URL;
- schema/version if readable;
- validation errors;
- fallback service URLs currently being used;
- explicit statement: `No secrets are loaded from runtime config.`

Do not display full raw config by default unless diagnostic export is enabled and redaction mode permits it.

## URL/test override semantics

Playwright and smoke tests must not mutate browser local storage accidentally and then call that runtime default. Test helpers should expose explicit methods:

```ts
seedRuntimeConfig(page, config)
seedOperatorLocalOverrides(page, overrides)
seedOperatorSessionOverrides(page, overrides)
```

Each helper should tag source metadata when possible. If source metadata is not persisted, tests should verify displayed provenance from effective values.

## Diagnostics bundle

`buildOperatorDiagnosticBundle.ts` should include:

```json
{
  "runtimeConfig": {
    "status": "loaded",
    "sourceUrl": "/operator-runtime-config.json",
    "environmentName": "Local",
    "profileName": "docker-local",
    "generatedAt": "...",
    "services": {
      "conexus": { "baseUrl": "http://localhost:5082", "source": "runtime-default" }
    },
    "warnings": []
  }
}
```

Redact anything that resembles credentials, even though runtime config must not contain secrets.
