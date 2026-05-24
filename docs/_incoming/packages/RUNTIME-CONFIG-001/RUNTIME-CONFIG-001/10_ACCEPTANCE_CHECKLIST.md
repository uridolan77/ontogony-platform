# 10 — Acceptance checklist

## Functional acceptance

- [ ] Docker-served frontend can change Conexus/Kanon/Allagma base URLs through `/operator-runtime-config.json` without rebuilding the frontend image.
- [ ] Frontend still works in plain Vite dev mode with `public/operator-runtime-config.json`.
- [ ] Frontend still works in plain Vite dev mode if the runtime config file is missing, with a visible fallback warning.
- [ ] Settings page clearly distinguishes runtime default vs local override vs session/test override vs fallback/invalid state.
- [ ] Clearing a local service URL override returns to the runtime config value.
- [ ] Reset all service URLs to runtime defaults does not clear credentials or appearance settings.
- [ ] Existing broad reset/erase operator settings still works.
- [ ] System Truth uses effective service URLs.
- [ ] Home uses effective service URLs/readiness state.
- [ ] Topology uses effective service URLs/readiness state.
- [ ] Evidence Spine resolves using effective service URLs.
- [ ] Domain Switcher persists Kanon ontology selection as a local override and can return to runtime default.
- [ ] Start Run uses effective Allagma/Kanon/Conexus-related settings.
- [ ] Agent Interaction uses effective service URLs.
- [ ] Conexus Observability uses effective Conexus base URL.
- [ ] Conexus streaming flag, if retained, is runtime-config-backed or explicitly left as a build fallback with docs.

## Docker/local-working-system acceptance

- [ ] `ontogony-platform` generates `docker/local-working-system/generated/operator-runtime-config.json`.
- [ ] Compose mounts/copies that file into the frontend nginx root.
- [ ] nginx serves `/operator-runtime-config.json` with no-cache headers.
- [ ] Runtime config uses browser-facing host URLs, not compose-internal service names.
- [ ] Backend-to-backend URLs in compose remain unchanged.
- [ ] CORS assumptions remain valid for `http://localhost:5175` and `http://127.0.0.1:5175`.
- [ ] Config can be changed and picked up without frontend image rebuild.

## Security/non-goal acceptance

- [ ] Runtime config contains no raw secrets.
- [ ] Runtime config validator rejects suspicious secret-like fields.
- [ ] No OIDC/IAM scope introduced.
- [ ] No backend auth redesign introduced.
- [ ] Existing local-alpha credential warnings remain accurate.
- [ ] Browser-local credential persistence behavior remains consent-gated.

## UI acceptance

- [ ] No new environment-management page is added unless implementation proves Settings cannot support the workflow.
- [ ] Provenance is progressively disclosed and not noisy.
- [ ] Runtime config warning appears once in Settings/System posture, not repeatedly on every panel.
- [ ] Uses canonical `@ontogony/ui` settings/status/layout patterns.
- [ ] Does not duplicate existing local credential warning cards.

## Testing acceptance

- [ ] Runtime config loader unit tests pass.
- [ ] Runtime config validation unit tests pass.
- [ ] Settings merge/provenance unit tests pass.
- [ ] Settings page provenance tests pass.
- [ ] System Truth runtime/default/override tests pass.
- [ ] Domain Switcher docker-live test passes.
- [ ] Evidence Spine docker-live test passes.
- [ ] governed fake E2E remains valid.
- [ ] Playwright docker-live tests pass.
- [ ] Platform runtime-config smoke passes.
- [ ] Contract discipline passes if route/catalog/client files were touched.

## Verification command set

### ontogony-frontend

```powershell
npm run typecheck
npm run test -- src/app/runtime-config
npm run test -- src/app/settings src/system/pages/OperatorSettingsPage.test.tsx
npm run test -- src/system/api/useSystemHealth.test.tsx
npm run config:check
npm run root-config:check
npm run contracts:discipline
npm run test:e2e:docker-live:fe-live-smoke
npm run test:e2e:docker-live:runtime-posture
npm run test:e2e:docker-live:domain-switcher
npm run test:e2e:docker-live:evidence-spine
npm run governed-fake-e2e:docker-live
```

### ontogony-platform

```powershell
pwsh .\scripts\local-working-system\write-operator-runtime-config.ps1
pwsh .\scripts\smokessert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175
pwsh .\scripts\smokeun-runtime-lock-governed-fake-e2e.ps1
```

### Backend repos, only if changed

```powershell
dotnet test .\Conexus.sln -c Release --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"
dotnet test .\Kanon.sln -c Release
dotnet test .\Allagma.sln -c Release --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
```

## Release note checklist

- [ ] Explain runtime config in one paragraph.
- [ ] Explain no secrets in runtime config.
- [ ] Explain how to clear local overrides.
- [ ] Explain that service URL changes no longer require frontend image rebuild in Docker-local/custom static deployments.
- [ ] Link runtime config docs from local-working-system docs.
