# 09 — Contract discipline and compatibility plan

## Principle

Runtime config is a frontend static asset, not a Conexus/Kanon/Allagma API route. Do not pollute backend route inventories or generated clients with `/operator-runtime-config.json`.

## What may need updates

### Frontend contract/config discipline

Update:

```text
scripts/frontend-env-catalog.json
scripts/lib/frontend-env-catalog.mjs
scripts/check-frontend-env-catalog.mjs
docs/generated/FE_FRONTEND_ENV_CATALOG.md
scripts/lib/frontend-root-config-audit.mjs
scripts/check-frontend-root-config.mjs
```

New checks should ensure:

- service URL `VITE_*` variables are no longer primary runtime defaults;
- `/operator-runtime-config.json` contract exists;
- public default runtime config validates;
- runtime config does not contain secret-like keys;
- Dockerfile no longer uses service URL VITE args as authoritative build inputs.

### Route workflow catalog

If Settings/System pages change labels, actions, or diagnostic panels, update:

```text
scripts/sync-route-workflow-inventory.mjs
scripts/check-route-workflow-inventory.mjs
docs/generated route workflow artifacts
```

Do this only for actual UI workflow changes.

### Client route usage

No backend client route should be added for `/operator-runtime-config.json`. The loader can use raw `fetch` because this is a static frontend asset.

### System compatibility artifacts

If generated compatibility docs/artifacts currently mention frontend VITE service URL build args as the source of truth, update them to reference runtime config.

Candidates:

```text
ontogony-frontend/scripts/sync-operator-system-compatibility.mjs
ontogony-frontend generated compatibility docs
ontogony-platform generated compatibility artifacts
allagma-dotnet/docs/system/README.md if it links local stack profiles
```

## Backend API compatibility

Expected backend route changes: **none**.

Do not modify:

- Conexus `/v1/chat/completions`, admin, health, ready routes.
- Kanon `/ontology/v0/*`, health, ready, openapi routes.
- Allagma `/allagma/v0/*`, health, ready routes.
- Evidence Spine semantics.
- Domain/model/provider routing boundaries.

## Static asset compatibility

Add a small frontend/static compatibility note:

```text
Static frontend runtime asset: GET /operator-runtime-config.json
Owner: ontogony-frontend/nginx/platform compose
Not a backend service API route
No generated OpenAPI client
No auth
No secrets
No-cache required
```

## Verification commands

Run in `ontogony-frontend` when config/catalog files change:

```powershell
npm run config:check
npm run root-config:check
npm run routes:check
npm run inventory:check
npm run client-routes:check
npm run route-client-drift:check
npm run contracts:discipline
```

Run in `ontogony-platform` when local-working-system compose/scripts change:

```powershell
pwsh .\scripts\local-working-system\write-operator-runtime-config.ps1
pwsh .\scripts\smokessert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175
```

## Compatibility matrix updates

Add a frontend runtime-config row to system compatibility docs:

| Component | Config source | Runtime? | Secrets? | Notes |
|---|---|---:|---:|---|
| Operator frontend service URLs | `/operator-runtime-config.json` | yes | no | Browser-facing URLs only |
| Operator credentials | Browser session/local settings | yes | yes, local only | Not from runtime config |
| Backend-to-backend URLs | Backend appsettings/env | yes/container | no | Can use compose service DNS |
| Build metadata | `VITE_APP_VERSION`, `VITE_GIT_SHA`, `VITE_BUILD_TIME` | no | no | Build-time remains correct |
