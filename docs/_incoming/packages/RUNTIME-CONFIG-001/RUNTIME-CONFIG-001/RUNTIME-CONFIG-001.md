# RUNTIME-CONFIG-001 — Main development brief

## Executive summary

The current Ontogony operator frontend has already moved far beyond static fixtures: it has live health/readiness surfaces, operator settings, Evidence Spine, Domain Switcher, Agent Interaction, governed fake E2E, docker-live tests, and contract discipline. The remaining config weakness is that the frontend service defaults are still mostly decided at **build time** through `VITE_*` variables and Docker build args, then copied into an nginx static image.

`RUNTIME-CONFIG-001` introduces a small runtime config contract served by the static frontend host. The app loads it before or during bootstrap, validates it, merges it with safe fallback defaults, and then applies browser-local/session/test/page overrides using explicit provenance. This lets Docker-local and custom stacks change backend service URLs without rebuilding the frontend image.

## Problem statement

Current behavior creates four sources of confusion:

1. `VITE_CONEXUS_BASE_URL`, `VITE_KANON_BASE_URL`, and `VITE_ALLAGMA_BASE_URL` are cataloged well but still baked during `vite build`.
2. Browser-local operator settings store a full settings snapshot and can silently keep old URLs even after the image/defaults change.
3. Docker-local service URLs must be browser-reachable `localhost:*`, while backend-to-backend URLs use compose service names like `kanon-api:8080`; the distinction is not represented in a runtime config contract.
4. Playwright helpers and smoke scripts intentionally override URLs, but this is not unified with a visible provenance model.

## Non-goals

- No OIDC/IAM implementation.
- No production credential redesign.
- No secret storage in runtime config.
- No backend route redesign.
- No Evidence Spine semantic changes.
- No domain/model/provider routing boundary change.
- No replacement of operator settings.
- No automatic live-to-fixture fallback.
- No new environment-management page unless Settings proves inadequate.

## Design center

Runtime config is a **default-source**. It is not hidden authority.

The operator must be able to answer, from the Settings page:

- What is the runtime profile?
- Which backend URL is coming from runtime config?
- Which backend URL is locally overridden?
- Which value came from a session/test override?
- What happens if I clear this override?
- Is runtime config missing, stale, or invalid?

## Staged implementation plan

### Stage 1 — Audit and config contract

Deliverables:

- Add `src/app/runtime-config/operatorRuntimeConfigTypes.ts`.
- Add `src/app/runtime-config/operatorRuntimeConfigDefaults.ts`.
- Add `src/app/runtime-config/operatorRuntimeConfigSchema.ts` or lightweight validator.
- Add `src/app/runtime-config/loadOperatorRuntimeConfig.ts`.
- Add `public/operator-runtime-config.json` for Vite dev/default local usage.
- Add tests for valid, missing, malformed, and invalid config.

The config file path should be:

```text
/operator-runtime-config.json
```

Reason: current nginx serves the app root directly; this path is simple, cache-controllable, and obviously not a backend route.

### Stage 2 — Frontend loader and settings merge

Deliverables:

- Change `src/main.tsx` to async bootstrap runtime config before mounting, or mount a minimal bootstrap shell and hydrate before rendering `OperatorSettingsProvider`.
- Thread a `RuntimeConfigSnapshot` into `OperatorSettingsProvider`.
- Update `defaultOperatorSettings` so service/domain/model defaults are generated from runtime config defaults instead of direct `import.meta.env` service URL reads.
- Keep `import.meta.env` only for build metadata and hard fallback compatibility during migration.
- Add typed source/provenance map for defaultable fields.

### Stage 3 — Settings provenance UI

Deliverables:

- Existing `OperatorSettingsPage` remains the page for this workflow.
- Add a progressive disclosure section: “Runtime config details”.
- Add per-service compact provenance text: `runtime default`, `local override`, `session override`, `test override`, `legacy local value`, `fallback default`.
- Add controls:
  - `Use runtime default`
  - `Override locally`
  - `Clear local override`
  - `Reset all service URLs to runtime defaults`
  - `Show runtime config details`
- Preserve existing credential warnings and local-alpha copy.
- Avoid a warning-card explosion; summarize top-level config posture once and place details behind disclosure.

### Stage 4 — Docker/local-working-system runtime config generation

Deliverables:

- Stop using frontend Docker build args for service URLs as the authoritative path.
- Generate `docker/local-working-system/generated/operator-runtime-config.json` from compose/platform env.
- Mount or copy this JSON into nginx at `/usr/share/nginx/html/operator-runtime-config.json`.
- Add an nginx exact `location = /operator-runtime-config.json` with no-cache headers.
- Keep Vite dev mode working with `public/operator-runtime-config.json` or a missing-config fallback.

### Stage 5 — Playwright and smoke updates

Deliverables:

- Update `e2e/helpers/dockerLocalSeed.ts` to read or assert runtime config where relevant.
- Add Playwright cases for runtime default, local override, clear override, invalid runtime config warning, docker-served config, and no silent reset across navigation.
- Update platform smoke scripts to verify `/operator-runtime-config.json` is served, valid JSON, no secrets, and service URLs match expected profile.
- Keep governed fake E2E and runtime-lock tests valid.

### Stage 6 — Docs and stale VITE cleanup

Deliverables:

- Add `docs/runtime-config/RUNTIME_CONFIG.md`.
- Add `docs/runtime-config/LOCAL_PROFILE.md`.
- Add `docs/runtime-config/DOCKER_LOCAL_PROFILE.md`.
- Add `docs/runtime-config/CUSTOM_STACK_PROFILE.md`.
- Update `docs/LOCAL_DEV.md`, `docs/SYSTEM_STATUS_AND_SETTINGS.md`, local-working-system README links, and generated env catalog docs.
- Keep build metadata `VITE_*` documentation; mark service URL `VITE_*` as fallback/migration only.

## File-level targets

### ontogony-frontend

Create:

```text
public/operator-runtime-config.json
src/app/runtime-config/operatorRuntimeConfigTypes.ts
src/app/runtime-config/operatorRuntimeConfigDefaults.ts
src/app/runtime-config/operatorRuntimeConfigValidation.ts
src/app/runtime-config/loadOperatorRuntimeConfig.ts
src/app/runtime-config/RuntimeConfigProvider.tsx
src/app/runtime-config/operatorRuntimeConfigProvenance.ts
src/app/runtime-config/*.test.ts
src/app/settings/operatorSettingsRuntimeDefaults.ts
src/app/settings/operatorSettingsProvenance.ts
src/app/settings/operatorSettingsProvenanceStorage.ts
```

Modify:

```text
src/main.tsx
src/app/settings/OperatorSettingsProvider.tsx
src/app/settings/operatorSettingsTypes.ts
src/app/settings/operatorSettingsStorage.ts
src/app/settings/normalizeOperatorSettings.ts
src/system/pages/OperatorSettingsPage.tsx
src/system/api/useSystemHealth.ts      # only if provenance/status needs runtime config state
src/shared/diagnostics/buildOperatorDiagnosticBundle.ts
src/vite-env.d.ts
scripts/frontend-env-catalog.json
scripts/lib/frontend-env-catalog.mjs
Dockerfile
nginx/default.conf
playwright.docker-local.config.ts
e2e/helpers/dockerLocalSeed.ts
```

### ontogony-platform

Create/modify:

```text
docker/local-working-system/generated/.gitkeep
docker/local-working-system/config/operator-runtime-config.local.template.json
scripts/local-working-system/write-operator-runtime-config.ps1
scripts/local-working-system/write-operator-runtime-config.sh
scripts/smoke/assert-operator-runtime-config.ps1
scripts/smoke/assert-operator-runtime-config.sh
docker/local-working-system/docker-compose.yml
docs/runtime-config/*.md or docs/local-working-system links
```

### ontogony-ui

Prefer using existing components first. Only change `@ontogony/ui` if the frontend cannot express provenance cleanly with current primitives.

Potential additions:

```text
src/lib/settings/operatorSettingsTypes.ts       # add optional provenance fields, not product-specific names
src/components/settings/ServiceConnectionSettingsPanel.tsx
src/components/settings/EnvironmentSummaryPanel.tsx
```

### Backend repos

No backend route changes expected. Only docs/scripts may need updates for profile terminology and CORS/origin expectations.

## Verification commands

Run in `ontogony-frontend`:

```powershell
npm run typecheck
npm run test -- src/app/runtime-config src/app/settings src/system/pages/OperatorSettingsPage.test.tsx src/system/api/useSystemHealth.test.tsx
npm run config:check
npm run root-config:check
npm run contracts:discipline
npm run test:e2e:docker-live:fe-live-smoke
npm run test:e2e:docker-live:domain-switcher
npm run test:e2e:docker-live:evidence-spine
npm run governed-fake-e2e:docker-live
```

Run in `ontogony-platform` after Docker changes:

```powershell
pwsh .\scripts\local-working-system\write-operator-runtime-config.ps1
pwsh .\scripts\smokessert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175
pwsh .\scripts\smokeun-runtime-lock-governed-fake-e2e.ps1
```

Run backend repo tests only if touched:

```powershell
dotnet test .\Conexus.sln -c Release --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"
dotnet test .\Kanon.sln -c Release
dotnet test .\Allagma.sln -c Release --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
```
