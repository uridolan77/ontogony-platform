# 07 — Test, smoke, and Playwright plan

## Test principle

Every override must be intentional. Tests should not accidentally pass because browser local settings mask runtime config behavior.

## Vitest plan

### Runtime config loader

Create:

```text
src/app/runtime-config/loadOperatorRuntimeConfig.test.ts
src/app/runtime-config/operatorRuntimeConfigValidation.test.ts
```

Cases:

- loads valid `/operator-runtime-config.json`;
- missing file returns fallback snapshot with warning;
- network error returns fallback snapshot;
- invalid JSON returns invalid snapshot with validation error;
- wrong schema returns invalid snapshot;
- missing service base URL returns invalid snapshot;
- non-http URL rejected;
- forbidden secret-like key rejected;
- optional fields default correctly.

### Settings merge/provenance

Create:

```text
src/app/settings/operatorSettingsRuntimeDefaults.test.ts
src/app/settings/operatorSettingsProvenance.test.ts
src/app/settings/operatorSettingsProvenanceStorage.test.ts
```

Cases:

- runtime defaults become effective settings;
- browser local override wins;
- session override wins over local where supported;
- test override wins over session/local where supported;
- page draft is represented as unsaved, not persisted;
- clear field override returns to runtime default;
- clear all service URL overrides returns only service URLs;
- clear all local runtime overrides leaves credentials untouched;
- legacy full settings are marked `legacy-local` if they differ;
- reset settings uses runtime defaults, not build-time VITE service URLs.

### Settings UI

Update:

```text
src/system/pages/OperatorSettingsPage.test.tsx
```

Cases:

- displays runtime profile name and status;
- displays runtime default source for service URLs;
- displays local override source after edit/save;
- `Use runtime default` resets field;
- `Reset all service URLs to runtime defaults` resets service URLs only;
- invalid runtime config warning appears once;
- credential warning copy remains accurate;
- no badge/warning overload.

### System Truth and clients

Update/add tests around:

```text
src/system/api/useSystemHealth.test.tsx
src/evidence-spine/resolveEvidenceSpine.live.test.ts
src/agent-interaction/evidence/resolveAgentInteractionEvidenceGraph.test.ts
src/kanon/api/kanonClient.test.ts
src/conexus/api/conexusClient.test.ts
src/allagma/api/allagmaClient.test.ts
```

Cases:

- System Truth uses runtime defaults when no local override exists;
- System Truth uses local overrides when present;
- Evidence Spine resolves with effective service URLs;
- Agent Interaction evidence graph resolves with effective service URLs;
- Domain Switcher still persists ontology override;
- Conexus Observability still uses effective Conexus base URL;
- Start Run still uses effective Allagma/Kanon/Conexus-related settings.

## Playwright plan

### New spec

Create:

```text
e2e/runtime-config-docker-live.spec.ts
```

Add to `playwright.docker-local.config.ts` testMatch.

Cases:

1. `default runtime config loads`
   - `GET /operator-runtime-config.json` returns valid JSON.
   - Settings page shows runtime config loaded and profile `docker-local`.
   - Service URLs match runtime defaults.

2. `local override wins`
   - Open Settings.
   - Change Conexus URL to a test URL.
   - Save.
   - Navigate away/back.
   - Settings and System Truth show local override source.

3. `clear override returns to runtime default`
   - Start with local override.
   - Click `Use runtime default` / `Clear local override`.
   - Save.
   - Confirm Conexus URL returns to runtime config value.

4. `invalid runtime config shows clear warning`
   - For non-docker or route-mocked Playwright test, mock `/operator-runtime-config.json` with invalid JSON/schema.
   - Confirm fallback values and warning.

5. `no silent reset across navigation`
   - Edit/save a local override.
   - Navigate through Home, Topology, Evidence Spine, Domain Switcher, Agent Interaction.
   - Return to Settings and confirm local override persists.

6. `docker-served config can change without rebuild`
   - This can be implemented as a smoke script rather than a pure browser test:
     - write runtime config with alternate ports;
     - start/restart frontend container without image rebuild;
     - assert Settings shows new runtime defaults when local overrides are cleared.

## Test helpers

Update `e2e/helpers/dockerLocalSeed.ts`:

- Add `getDockerLiveRuntimeConfigUrl()`.
- Add `fetchDockerLiveRuntimeConfig()` for node-side smoke assertions.
- Build operator settings from runtime config where possible.
- Keep env-var overrides explicit and source-labeled.

Potential helpers:

```ts
export function getDockerLiveRuntimeConfigUrl() {
  return `${getDockerLiveServiceConfig().frontendBaseUrl}/operator-runtime-config.json`;
}

export async function fetchDockerLiveRuntimeConfig() { ... }

export function buildDockerLiveOperatorSettingsFromRuntimeConfig(config) { ... }
```

## Platform smoke plan

Create:

```text
ontogony-platform/scripts/smoke/assert-operator-runtime-config.ps1
ontogony-platform/scripts/smoke/assert-operator-runtime-config.sh
```

Checks:

- fetch `http://localhost:5175/operator-runtime-config.json`;
- parse JSON;
- validate schema and version;
- confirm service URLs match expected environment variables;
- confirm no forbidden secret-like keys;
- confirm no-cache headers;
- emit `artifacts/runtime-config/runtime-config-smoke-*.json`.

Integrate this into local-working-system smoke sequence before browser E2E.

## Governed fake E2E and runtime-lock

Do not change the semantics of governed fake E2E. Add only enough config checks to ensure:

- the frontend can resolve the three service URLs through runtime config or explicit test overrides;
- the runtime lock records the runtime profile/config source in evidence if useful;
- existing assertions remain stable.

## Contract discipline tests

Run if touched:

```powershell
npm run config:check
npm run root-config:check
npm run routes:check
npm run inventory:check
npm run client-routes:check
npm run route-client-drift:check
npm run contracts:discipline
```

Runtime config static asset should not be forced into backend route inventories.

## Minimum verification matrix

| Area | Command |
|---|---|
| TypeScript | `npm run typecheck` |
| Runtime config unit tests | `npm run test -- src/app/runtime-config` |
| Settings/provenance tests | `npm run test -- src/app/settings src/system/pages/OperatorSettingsPage.test.tsx` |
| System health tests | `npm run test -- src/system/api/useSystemHealth.test.tsx` |
| Docker live smoke | `npm run test:e2e:docker-live:fe-live-smoke` |
| Runtime config docker live | `playwright test -c playwright.docker-local.config.ts e2e/runtime-config-docker-live.spec.ts` |
| Domain switcher | `npm run test:e2e:docker-live:domain-switcher` |
| Evidence Spine | `npm run test:e2e:docker-live:evidence-spine` |
| Governed fake E2E | `npm run governed-fake-e2e:docker-live` |
| Platform runtime smoke | `pwsh .\scripts\smokessert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175` |
