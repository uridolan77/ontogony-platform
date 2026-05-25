# RUNTIME-CONFIG-001 — Implementation Notes

## Package
RUNTIME-CONFIG-001 — Ontogony Operator Runtime Configuration

## Repos touched
- `ontogony-frontend` (primary)
- `ontogony-platform` (Docker/scripts/smoke)
- `ontogony-ui` (optional provenance fields on settings panels)

## Current-state findings
- No runtime config existed before this package; service URLs were build-time `VITE_*` / Docker build args.
- Operator settings credential separation matched package assumptions and was preserved.
- `@ontogony/ui` settings panels existed; provenance display fields were added.

## Stale assumptions / skips
- **Frontend flags** (`enableFixtureRoutes`, etc.) not in `OperatorSettings` — kept in runtime JSON + Settings disclosure only.
- **New environment page** — skipped; Settings remains the workflow surface.
- **Backend route changes** — none (out of scope).
- **Playwright docker-live runtime spec** — implemented in 001A; requires live docker-local stack to execute.
- **governed-fake-e2e:docker-live** — not run (requires live stack).

## Implementation decisions
- Runtime config is default-source only; local overrides win with explicit provenance metadata.
- Docker frontend image no longer bakes service URL build args; compose bind-mounts generated JSON.
- Legacy `VITE_*` service/profile vars remain as migration fallbacks in `operatorSettingsRuntimeDefaults.ts`.
- `/operator-runtime-config.json` is a static frontend asset — not added to backend route inventories.

## Files changed

### ontogony-frontend
- `public/operator-runtime-config.json`
- `src/app/runtime-config/*` (types, defaults, validation, loader, provider, provenance helpers, tests)
- `src/app/settings/operatorSettingsRuntimeDefaults.ts`, `operatorSettingsProvenance*.ts`, storage/provider updates
- `src/main.tsx`, `src/system/pages/OperatorSettingsPage.tsx`
- `src/shared/diagnostics/buildOperatorDiagnosticBundle.ts`, `DiagnosticExportPanel.tsx`
- `nginx/default.conf`, `Dockerfile`
- `e2e/helpers/dockerLocalSeed.ts`
- `scripts/frontend-env-catalog.json`, `scripts/lib/frontend-env-catalog.mjs`
- `docs/runtime-config/RUNTIME_CONFIG.md`, `docs/generated/FE_FRONTEND_ENV_CATALOG.md`

### ontogony-platform
- `docker/local-working-system/docker-compose.yml`
- `docker/local-working-system/config/operator-runtime-config.local.template.json`
- `docker/local-working-system/generated/operator-runtime-config.json`
- `scripts/local-working-system/write-operator-runtime-config.ps1`
- `scripts/smoke/assert-operator-runtime-config.ps1`

### ontogony-ui
- `src/lib/settings/operatorSettingsTypes.ts` (optional configSource fields)
- `src/components/settings/ServiceConnectionSettingsPanel.tsx` (source label display)

## Tests/checks run
| Check | Result |
|---|---|
| `npm run typecheck` | pass |
| `npm run test -- src/app/runtime-config src/app/settings/operatorSettingsRuntimeDefaults.test.ts src/app/settings/operatorSettingsStorage.test.ts src/system/pages/OperatorSettingsPage.test.tsx` | pass (19 tests) |
| `npm run config:audit` | pass |
| `npm run config:check` | pass |
| `npm run root-config:check` | pass |
| `npm run contracts:discipline` | **Pass** (2026-05-25; Allagma replay-runtime OpenAPI + route parity aligned) |
| `write-operator-runtime-config.ps1` | pass |
| `assert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175` | pass (2026-05-24) |
| `npm run test:e2e:docker-live:runtime-config` | pass — 3/3 (2026-05-24) |

## Acceptance status
- Runtime config contract + loader: **done**
- Settings merge + provenance: **done**
- Settings UI provenance: **done**
- Docker/nginx/platform generation: **done**
- Env catalog discipline: **done**
- Unit tests: **done**
- Docker-live Playwright matrix: **done** (nginx JSON + Settings provenance + override reset)
- Local-stack smoke evidence: **done**
- Full acceptance checklist: **closed** (except unrelated Allagma route parity)

## Status (2026-05-24)
```text
RUNTIME-CONFIG-001A: implemented and review-passed.
RUNTIME-CONFIG-001: closed on docker-local evidence (smoke + e2e pass).
```

## Deferred / follow-up
1. Add `LOCAL_PROFILE.md`, `DOCKER_LOCAL_PROFILE.md`, `CUSTOM_STACK_PROFILE.md` if fuller docs are wanted.
2. Provider persist-on-mount may promote `legacy-local` to `local-override` after boot — follow-up if undesired.

## RUNTIME-CONFIG-001A (closure hardening)
- Fixed `assert-operator-runtime-config.ps1` false positive: recursive key validation with allowlist (matches frontend validator).
- Added `e2e/runtime-config-docker-live.spec.ts` and `npm run test:e2e:docker-live:runtime-config`.
- Fixed `serviceBaseUrlProvenanceModel` UI type mapping for `configSource`.
- Fixed UTF-8 BOM in generated JSON (`write-operator-runtime-config.ps1` + loader/e2e BOM strip).
- Fixed e2e test 3 false failure: `seedOperatorSettings({ force: true })` used `addInitScript`, which re-seeded localStorage on every reload; added `oneShot` evaluate path for override persistence tests.
- Added unit test for runtime-default override persist/reload in `operatorSettingsStorage.test.ts`.


## Known caveats
- Windows bind-mount refresh may require frontend container restart (not image rebuild).
- `OperatorSettingsProvider` re-persists settings on mount via `useEffect` (existing pattern preserved).

## Suggested next step
None for RUNTIME-CONFIG-001 closure. Optional: address Allagma route parity drift and provider persist-on-mount follow-up.
