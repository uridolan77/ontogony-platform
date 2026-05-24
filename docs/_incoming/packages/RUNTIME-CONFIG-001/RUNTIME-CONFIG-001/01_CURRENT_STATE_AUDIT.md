# 01 — Current-state audit

## Audit basis

This audit was prepared from the current GitHub-accessible state of the six repos. Before implementation, Cursor must re-open the local files and update any drift.

Key inspected paths:

- `ontogony-frontend/scripts/frontend-env-catalog.json`
- `ontogony-frontend/src/app/settings/operatorSettingsTypes.ts`
- `ontogony-frontend/src/shared/config/dockerLocalServiceDefaults.ts`
- `ontogony-frontend/src/app/settings/operatorSettingsStorage.ts`
- `ontogony-frontend/src/app/settings/operatorCredentialStorage.ts`
- `ontogony-frontend/src/app/settings/OperatorSettingsProvider.tsx`
- `ontogony-frontend/src/system/pages/OperatorSettingsPage.tsx`
- `ontogony-frontend/src/system/api/useSystemHealth.ts`
- `ontogony-frontend/src/main.tsx`
- `ontogony-frontend/Dockerfile`
- `ontogony-frontend/nginx/default.conf`
- `ontogony-frontend/e2e/helpers/dockerLocalSeed.ts`
- `ontogony-frontend/playwright.docker-local.config.ts`
- `ontogony-platform/docker/local-working-system/docker-compose.yml`
- `ontogony-ui/package.json`
- `ontogony-ui/src/components/settings/index.ts`
- `ontogony-ui/src/lib/settings/operatorSettingsTypes.ts`
- `conexus-dotnet/docs/DEVELOPMENT.md`
- `conexus-dotnet/src/Conexus.Api/appsettings.Development.json`
- `kanon-dotnet/README.md`
- `kanon-dotnet/src/Kanon.Api/appsettings.Development.json`
- `allagma-dotnet/README.md`
- `allagma-dotnet/src/Allagma.Api/appsettings.Development.json`

## Executive audit findings

1. The frontend already has an explicit env catalog, but service URL defaults remain build-time `VITE_*` values.
2. The static Docker frontend bakes service URLs with Docker `ARG`/`ENV` before `npm run build`.
3. nginx currently serves only `index.html` fallback; there is no runtime config file or no-cache runtime-config route.
4. Operator settings persist a safe local snapshot plus separated credential secret storage; this is a strong base but it currently stores service URLs as ordinary settings, not as runtime-default-aware overrides.
5. System Truth and health/readiness already read from operator settings; once settings are runtime-default-aware, those surfaces should inherit the correct URLs without route changes.
6. The Settings page has environment/build/service URL display and local dev default actions, but it does not show runtime default vs local override provenance.
7. Docker-local compose already distinguishes browser URLs (`localhost:5081/5082/5083`) from internal backend URLs (`kanon-api:8080`, `conexus-api:8080`), but the distinction is implicit and spread across env vars.
8. Playwright docker-live helpers use explicit env vars and hardcoded local defaults; this is workable but should become provenance-aware and runtime-config-asserting.
9. Backend repos mostly do not need route changes. They define health/ready routes and CORS/dev ports; the frontend runtime config should consume these, not mutate them.

## ontogony-frontend

### Current endpoint/base URL configuration mechanism

`operatorSettingsTypes.ts` defines `defaultOperatorSettings`. Service base URLs are resolved from `import.meta.env` first, then `dockerLocalServiceDefaults`:

```ts
conexus.baseUrl = import.meta.env.VITE_CONEXUS_BASE_URL ?? dockerLocalServiceDefaults.conexusBaseUrl
kanon.baseUrl = import.meta.env.VITE_KANON_BASE_URL ?? dockerLocalServiceDefaults.kanonBaseUrl
allagma.baseUrl = import.meta.env.VITE_ALLAGMA_BASE_URL ?? dockerLocalServiceDefaults.allagmaBaseUrl
```

Kanon ontology defaults and actor defaults are similarly derived from `VITE_*` and local defaults.

### Current default local values

`src/shared/config/dockerLocalServiceDefaults.ts` hardcodes:

```ts
conexusBaseUrl: "http://localhost:5082"
kanonBaseUrl: "http://localhost:5081"
allagmaBaseUrl: "http://localhost:5083"
kanonOntologyId: "gaming-core"
kanonOntologyVersionId: "gaming-core@0.1.0"
```

`.env.example` repeats these values and adds optional build-time actor/session/streaming flags.

### Current Docker values

`Dockerfile` defines build args:

```dockerfile
ARG VITE_KANON_BASE_URL=http://localhost:5081
ARG VITE_CONEXUS_BASE_URL=http://localhost:5082
ARG VITE_ALLAGMA_BASE_URL=http://localhost:5083
ARG VITE_OPERATOR_SESSION_TOKEN=docker-local-operator-session
```

These are converted to environment variables before `npm run build`, so Vite embeds them in the generated static bundle.

`nginx/default.conf` only has:

```nginx
location / {
    try_files $uri $uri/ /index.html;
}
```

There is no exact runtime config route and no no-cache header for a runtime config file.

### Current test overrides

`e2e/helpers/dockerLocalSeed.ts` resolves:

```ts
frontendBaseUrl: E2E_BASE_URL ?? "http://localhost:5175"
allagmaBaseUrl: ALLAGMA_BASE_URL ?? "http://localhost:5083"
kanonBaseUrl: KANON_BASE_URL ?? "http://localhost:5081"
conexusBaseUrl: CONEXUS_BASE_URL ?? "http://localhost:5082"
```

It then builds Docker-live operator settings with these values, including local service/admin tokens where required.

`playwright.docker-local.config.ts` sets `baseURL` from `E2E_BASE_URL ?? "http://localhost:5175"` and runs docker-live specs against the nginx-served app, not Vite.

### Current browser-local settings behavior

`operatorSettingsStorage.ts` reads a safe JSON blob from `localStorage` key:

```text
ontogony.frontend.operator-settings.safe.v1
```

It merges this blob over `defaultOperatorSettings`, normalizes, and then applies credential secrets from session/local storage.

Credentials are separated by `operatorCredentialStorage.ts` into:

```text
ontogony.frontend.operator-credentials.session.v1
ontogony.frontend.operator-credentials.local.v1
```

Legacy monolithic settings are migrated from:

```text
ontogony.frontend.operator-settings.v1
```

This separation is good and should not be discarded.

### Current places where config is baked at build time

Build-time values include:

- `VITE_CONEXUS_BASE_URL`
- `VITE_KANON_BASE_URL`
- `VITE_ALLAGMA_BASE_URL`
- `VITE_KANON_ONTOLOGY_ID`
- `VITE_KANON_ONTOLOGY_VERSION_ID`
- `VITE_KANON_DEFAULT_ACTOR_ID`
- `VITE_KANON_DEFAULT_ACTOR_ROLES`
- `VITE_OPERATOR_SESSION_TOKEN`
- `VITE_CONEXUS_STREAMING_ENABLED`
- build metadata `VITE_APP_VERSION`, `VITE_GIT_SHA`, `VITE_BUILD_TIME`

Build metadata should remain build-time. Service URLs, profile defaults, and feature defaults should move to runtime config defaults.

### Current API client base URL resolution

System Truth uses `settings.conexus.baseUrl`, `settings.kanon.baseUrl`, and `settings.allagma.baseUrl` in `useSystemHealth.ts`. Similar patterns are used by Conexus, Kanon, Allagma, Evidence Spine, Agent Interaction, Domain Switcher, and Start Run surfaces through operator settings and generated/client wrappers.

The desired change is not to update every page manually. Instead, make operator settings resolve from runtime config defaults and allow existing consumers to continue reading `settings.*.baseUrl`.

### Current Settings UI

`OperatorSettingsPage.tsx` already shows:

- build metadata and environment label;
- Conexus/Kanon/Allagma service URLs;
- service health/readiness;
- credential source display;
- local-alpha credential warnings;
- local stack URL action;
- danger reset action.

Missing:

- runtime config loaded/missing/invalid state;
- value provenance per defaultable setting;
- reset to runtime default per service;
- reset all service URLs to runtime defaults;
- legacy local override visibility.

### Stale docs/claims to ignore

Ignore any claim that the frontend is still fixture-only or lacks System Truth. Current code has live health/readiness integration and docker-live tests.

## ontogony-platform

### Current endpoint/base URL configuration mechanism

`docker/local-working-system/docker-compose.yml` owns the local stack orchestration. It sets backend host ports and frontend build args.

### Current default local values

- Frontend host port: `5175`
- Kanon host port: `5081`
- Conexus host port: `5082`
- Allagma host port: `5083`
- Postgres host port: `5432`

### Current Docker values

Backend-to-backend internal URLs use compose DNS names:

```text
Kanon__BaseUrl=http://kanon-api:8080
Conexus__BaseUrl=http://conexus-api:8080
Kanon__ConexusAssistance__BaseUrl=http://conexus-api:8080
```

Browser-facing frontend defaults use host URLs:

```text
FRONTEND_VITE_KANON_BASE_URL=http://localhost:5081
FRONTEND_VITE_CONEXUS_BASE_URL=http://localhost:5082
FRONTEND_VITE_ALLAGMA_BASE_URL=http://localhost:5083
```

These must remain browser-facing in runtime config. Do not put `kanon-api:8080` into browser runtime config.

### Current test/smoke behavior

The platform has smoke scripts including runtime-lock/governed fake E2E. These should gain a small runtime-config assertion but should not be rewritten.

### Places where config is baked at build time

The compose file passes `FRONTEND_VITE_*` values as frontend Docker build args. This is the main platform-side target to replace or demote.

## ontogony-ui

### Current primitives

`@ontogony/ui` exports `./settings`, `./system`, `./status`, `./operator`, `./diagnostics`, and other canonical subpaths. Settings exports include:

- `OperatorSettingsLayout`
- `EnvironmentSummaryPanel`
- `ServiceConnectionSettingsPanel`
- `CredentialSettingsPanel`
- `LocalCredentialStorageNotice`
- `LocalDevDefaultsPanel`
- `DangerZoneSettingsPanel`

`ServiceConnectionSettingsModel` currently carries service label, baseUrl, health/readiness status, health message, last checked, and metadata. `OperatorCredentialFieldModel` already carries `credentialSource`, `credentialScope`, and `credentialRisk`.

### Gap

There is no neutral service setting provenance model yet. Prefer frontend composition first. If UI primitives are too limited, add optional fields such as:

```ts
configSource?: 'runtime-default' | 'local-override' | 'session-override' | 'test-override' | 'fallback-default' | 'legacy-local'
configSourceLabel?: string
configSourceDetail?: string
```

Keep names neutral and product-agnostic.

## conexus-dotnet

### Current endpoint/base URL configuration mechanism

Conexus is a backend service. It should not consume frontend runtime config. It exposes health/ready and OpenAI-compatible/admin APIs.

### Current default local values

Standalone development docs show a default dotnet-run path using `http://localhost:5000` for direct local `curl`, while Docker local-working-system maps Conexus to host port `5082`. Treat `5000` as standalone dev documentation, not the operator Docker-local source of truth.

### Current Docker values

Platform compose maps Conexus container port `8080` to host `5082` and uses internal compose service name `conexus-api:8080` for backend-to-backend calls.

### CORS/origin assumptions

`src/Conexus.Api/appsettings.Development.json` allows local frontend origins including `5173`, `5174`, `5175`, and `5176`. The platform compose also passes `Conexus__Cors__AllowedOrigins__0/1` for `http://localhost:5175` and `http://127.0.0.1:5175`.

### Runtime-config impact

No Conexus route change expected. Only docs may need to say the operator frontend receives Conexus browser base URL from runtime config.

## kanon-dotnet

### Current endpoint/base URL configuration mechanism

Kanon is a backend semantic authority. It should not consume frontend runtime config. It exposes `/health`, `/ready`, `/openapi/v1.json`, and ontology v0 routes.

### Current default local values

Development settings enable CORS for `http://localhost:5175` and `http://127.0.0.1:5175`, use `DevelopmentTrustedHeaders`, recommend `local-operator`, and recommend read roles `Auditor,ProvenanceReader`. Persistence defaults to `InMemory`; domain pack examples include `gaming-core`.

### Current Docker values

Platform compose maps Kanon container port `8080` to host `5081`. Backend-to-backend uses `kanon-api:8080`.

### Runtime-config impact

No Kanon route change expected. Runtime config may include default ontology id/version for the operator only.

## allagma-dotnet

### Current endpoint/base URL configuration mechanism

Allagma is the governed execution backend and orchestrator. It uses `Kanon:BaseUrl` and `Conexus:BaseUrl` for backend-to-backend calls and has model-purpose-to-Conexus-alias configuration.

### Current default local values

Allagma README identifies local development ports:

```text
Kanon 5081
Conexus 5082
Allagma 5083
```

Development settings use:

```json
"Kanon": { "BaseUrl": "http://localhost:5081" }
"Conexus": { "BaseUrl": "http://localhost:5082" }
```

### Current Docker values

Platform compose maps Allagma container port `8080` to host `5083` and configures internal backend URLs:

```text
Kanon__BaseUrl=http://kanon-api:8080
Conexus__BaseUrl=http://conexus-api:8080
```

### Runtime-config impact

No Allagma route change expected. Runtime config should include only the browser-facing Allagma base URL and optional default Conexus project/model alias for operator UI defaults.

## Main answers to the requested questions

### 1. Which frontend values are baked at build time through `VITE_*`?

All values in `scripts/frontend-env-catalog.json`: build metadata, service URLs, Kanon ontology defaults, actor defaults, operator session token, and Conexus streaming feature flag. Build metadata should remain baked; service/profile defaults should move to runtime config.

### 2. Which values are inferred from browser-local settings?

Full `OperatorSettings` safe snapshot plus separated credential secrets. Service URLs, environment label, Conexus project/model alias, Kanon ontology defaults, actor defaults, appearance, and credential persistence mode can all come from browser storage.

### 3. Which values are hardcoded in tests, Docker, or smoke scripts?

Hardcoded defaults include frontend `5175`, Kanon `5081`, Conexus `5082`, Allagma `5083`, seed report path under `C:\dev\ontogony-platform`, docker-local session token, service tokens, Conexus admin key, and local actor roles.

### 4. Which values should become runtime config defaults?

Service browser base URLs, environment/profile name, default route, fixture route enablement, expert mode default, route search flag, evidence raw preview default, export redaction mode, local-alpha warning flags, diagnostics export flag, Kanon ontology defaults, Conexus project/model alias defaults, and Conexus streaming flag if still needed.

### 5. Which values should remain user overrides in local settings?

Credentials/secrets, local access gate token, manually overridden service URLs, actor id/roles, Conexus project/model alias if operator edits them, Kanon ontology selection if Domain Switcher changes it, appearance/accessibility, and credential persistence preference.

### 6. What is the precedence order?

Later layers win: hardcoded safe fallback defaults → runtime config defaults → browser-local persisted settings → session-only overrides → explicit URL/test overrides → page-local draft/form overrides.

### 7. How should runtime config be loaded before app boot?

Load `/operator-runtime-config.json` in `main.tsx` before rendering `OperatorSettingsProvider`, with fail-soft fallback in Vite dev. Expose a typed snapshot and validation diagnostics through a provider.

### 8. How should Docker/nginx inject runtime config without rebuilding?

Generate JSON in `ontogony-platform/docker/local-working-system/generated/operator-runtime-config.json` and mount it read-only to `/usr/share/nginx/html/operator-runtime-config.json`; add nginx no-cache headers for that exact path.

### 9. How should UI show config provenance?

Settings page should show compact source labels per defaultable field plus a single Runtime Config details disclosure. Avoid repeated warning cards.

### 10. How should tests seed/override intentionally?

Tests should either serve a test runtime config or explicitly use helper methods that mark values as `test override`. `seedOperatorSettings` should remain intentional and not accidentally mask runtime config behavior.

### 11. How do we keep current local developer workflows working?

Ship `public/operator-runtime-config.json` with local defaults. Missing runtime config in Vite dev should produce a visible but non-fatal fallback status. Existing `.env.example` VITE service URLs remain temporary fallback only.

### 12. How do we avoid production identity/security work?

Do not add secrets to runtime config, do not alter backend auth modes, do not add OIDC/IAM, and keep local-alpha warnings exactly scoped to local/dev use.
