# RUNTIME-CONFIG-001 — Implementation Notes

## Package
RUNTIME-CONFIG-001 — Ontogony Operator Runtime Configuration

## Repos touched
- `ontogony-frontend` (primary)
- `ontogony-platform` (Docker/scripts/smoke)
- `ontogony-ui` (optional provenance fields on settings panels)

## Current-state findings (2026-05-24)
- No runtime config implementation exists yet; service URLs still from `VITE_*` / `dockerLocalServiceDefaults`.
- `main.tsx` sync bootstraps `OperatorSettingsProvider` without runtime fetch.
- `Dockerfile` bakes service URL build args; `nginx/default.conf` has no runtime config route.
- `docker-compose.yml` passes `FRONTEND_VITE_*` as build args; no volume mount for runtime JSON.
- Operator settings storage/credential separation is solid and matches package assumptions.
- `@ontogony/ui` has settings panels but no config provenance fields yet.

## Instruction classification
| Instruction | Status |
|---|---|
| Runtime config contract + loader | current_and_actionable |
| Settings merge + provenance | current_and_actionable |
| Settings UI provenance | current_and_actionable |
| Docker/nginx/platform generation | current_and_actionable |
| Playwright/smoke updates | current_and_actionable |
| Docs + env catalog updates | current_and_actionable |
| Backend route changes | out_of_scope (none expected) |
| OIDC/IAM | out_of_scope |
| New environment page | stale_skip (Settings remains page) |
| Frontend flags not in OperatorSettings | needs_adjustment (map existing fields only; snapshot for display) |

## Implementation decisions
- Map runtime config to existing `OperatorSettings` fields only; extra contract fields kept in snapshot for Settings disclosure.
- Add optional neutral `configSource*` fields to `@ontogony/ui` `ServiceConnectionSettingsModel`.
- Demote Docker service URL build args; keep as migration fallback in Vite env.
- Provenance metadata in separate localStorage key per package spec.

## Files changed
(in progress)

## Tests/checks run
(pending)

## Deferred items
- Full docker-live e2e matrix if stack not running locally
- Backend doc one-liners (optional)

## Acceptance status
(pending)
