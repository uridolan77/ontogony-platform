# 04 — Frontend bootstrap plan

## Current bootstrap

`src/main.tsx` synchronously renders:

```tsx
<ThemeProvider>
  <AppQueryProvider>
    <OperatorSettingsProvider>
      <ThemeOperatorSync />
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </OperatorSettingsProvider>
  </AppQueryProvider>
</ThemeProvider>
```

`OperatorSettingsProvider` initializes from `readOperatorSettings()`, which currently uses `defaultOperatorSettings` directly. That makes default service URLs build-time values.

## Target bootstrap

### Preferred option: async pre-render load

Implement:

```ts
async function bootstrap() {
  const runtimeConfig = await loadOperatorRuntimeConfig({
    sourceUrl: "/operator-runtime-config.json",
  });

  ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
      <RuntimeConfigProvider snapshot={runtimeConfig}>
        <ThemeProvider defaultTheme="ontogony-dark">
          <AppQueryProvider>
            <OperatorSettingsProvider runtimeConfig={runtimeConfig}>
              <ThemeOperatorSync />
              <BrowserRouter>
                <App />
              </BrowserRouter>
            </OperatorSettingsProvider>
          </AppQueryProvider>
        </ThemeProvider>
      </RuntimeConfigProvider>
    </React.StrictMode>,
  );
}

void bootstrap();
```

The loader always resolves to a `RuntimeConfigSnapshot`, never throws past bootstrap. Missing/invalid config is represented as a status.

### Alternative: bootstrap shell

Only use this if async pre-render causes measurable UX/test problems:

- render `OperatorBootstrapShell` immediately;
- fetch runtime config;
- render full app after config snapshot exists.

## Loader behavior

Create `loadOperatorRuntimeConfig.ts`:

```ts
export async function loadOperatorRuntimeConfig(options?: {
  sourceUrl?: string;
  fetchImpl?: typeof fetch;
  now?: () => Date;
}): Promise<RuntimeConfigSnapshot>
```

Rules:

- `404` / network error: return `status: "missing"`, `usedFallback: true`.
- Invalid JSON: return `status: "invalid"`, `usedFallback: true`.
- Schema validation errors: return `status: "invalid"`, `usedFallback: true`.
- Valid JSON: return `status: "loaded"`, `usedFallback: false`.
- Never load credentials or secrets.

## Settings merge logic

Create `operatorSettingsRuntimeDefaults.ts`:

```ts
export function buildOperatorSettingsDefaultsFromRuntime(
  snapshot: RuntimeConfigSnapshot,
): OperatorSettings
```

This replaces service/profile defaults in `defaultOperatorSettings`. Keep `defaultOperatorSettings` for hard fallback compatibility, but prefer a factory:

```ts
export function buildDefaultOperatorSettings(
  runtimeConfig?: RuntimeConfigSnapshot,
): OperatorSettings
```

Current `defaultOperatorSettings` can remain as:

```ts
export const defaultOperatorSettings = buildDefaultOperatorSettings(
  fallbackRuntimeConfigSnapshot,
);
```

## Provider changes

Change `OperatorSettingsProvider` props:

```ts
export function OperatorSettingsProvider({
  children,
  runtimeConfig,
}: {
  children: ReactNode;
  runtimeConfig: RuntimeConfigSnapshot;
})
```

Context value should add:

```ts
type OperatorSettingsContextValue = {
  settings: OperatorSettings;
  provenance: OperatorSettingsProvenance;
  runtimeConfig: RuntimeConfigSnapshot;
  update: (...);
  replace: (...);
  reset: () => void;
  resetFieldToRuntimeDefault: (field: DefaultableOperatorField) => void;
  resetServiceUrlsToRuntimeDefaults: () => void;
  clearAllLocalOverrides: () => void;
}
```

## No circular dependency

Runtime config code must not import app pages, API clients, or `useOperatorSettings`. The dependency direction must be:

```text
runtime-config -> settings default factory -> settings provider -> pages/API hooks
```

## API client resolution updates

Do not rewrite all client code. Existing clients should continue using `settings.*.baseUrl`.

Required checks:

- `useSystemHealth` still receives effective settings values.
- Conexus clients still use `settings.conexus.baseUrl`.
- Kanon clients still use `settings.kanon.baseUrl`.
- Allagma clients still use `settings.allagma.baseUrl`.
- Evidence Spine uses the same effective settings object.
- Agent Interaction resolver uses the same effective settings object.
- Domain Switcher persists Kanon ontology settings as local override when changed.

## Feature/default fields

Migrate these existing build-time/default concepts:

- `VITE_CONEXUS_STREAMING_ENABLED` → `runtimeConfig.conexus.streamingEnabled`.
- `VITE_KANON_ONTOLOGY_ID` → `runtimeConfig.kanon.ontologyId`.
- `VITE_KANON_ONTOLOGY_VERSION_ID` → `runtimeConfig.kanon.ontologyVersionId`.
- `VITE_KANON_DEFAULT_ACTOR_ID` / roles → runtime defaults or keep safe fallback if intentionally operator-local.
- `VITE_OPERATOR_SESSION_TOKEN` → keep as temporary dev fallback only; do not place real tokens in runtime config.

Build metadata remains `VITE_*`.

## Testing targets

Add tests:

```text
src/app/runtime-config/loadOperatorRuntimeConfig.test.ts
src/app/runtime-config/operatorRuntimeConfigValidation.test.ts
src/app/settings/operatorSettingsRuntimeDefaults.test.ts
src/app/settings/operatorSettingsProvenance.test.ts
src/app/settings/OperatorSettingsProvider.runtimeConfig.test.tsx
```

Test cases:

- valid runtime config creates defaults;
- missing runtime config uses fallback and records warning;
- invalid runtime config uses fallback and records errors;
- runtime service URL defaults flow into operator settings;
- local override wins;
- clear override returns to runtime default;
- settings reset uses runtime defaults, not baked VITE service URLs;
- no secrets pass validation.
