# Ontogony UI shared foundation hardening — known limitations

Consolidated from UI-HARDEN-000–009. Not a production readiness statement.

## Package & versioning

- **Pre-1.0** (`0.1.0-alpha.0`): additive subpaths are normal; breaking changes still require migration notes and consumer coordination.
- **Root barrel** (`.`) is compatibility-only — new product code must use documented subpaths.
- **Full release gate** (`npm run check` in `ontogony-ui`) includes Storybook build, knip, size-limit — passed 2026-05-20 sign-off; `npm run check:full` (e2e) remains optional for platform cuts.

## Boundary

- `@ontogony/ui` must not own product DTOs, OpenAPI clients, or service route catalogs.
- Consumers may keep thin adapters (e.g. `ontogony-frontend/shared/components/EvidenceExportPanel.tsx`) that map product bundles to neutral primitives.

## Status language

- `OperatorStatusKind` / `StatusBadge` is the **shell and cross-service** vocabulary.
- Domain badges (`RunStateBadge`, `SemanticVersionBadge`, execution/settings-specific chips) remain on their subpaths by design.
- `ontogony-frontend` still maps health DTOs via `serviceHealthPresentation.ts` before shell badges.

## Primitives not fully rolled out

- Not every page uses `RouteTabs` — only route families with repeated subnav (e.g. Allagma evaluations).
- `ProductLiveQueryState` centralizes empty/error/not-configured UX but is product-local, not exported from `@ontogony/ui`.
- `SectionPlaceholder` and legacy card grids may remain on low-traffic routes until touched.
- `DiagnosticExportPanel` remains a candidate for future generic extraction (see `ontogony-frontend` UI foundation audit).

## Accessibility & visual

- Dialog contract is documented and tested; not every product modal may use `@ontogony/ui/dialogs` yet.
- Density tokens are adopted on key operator surfaces; Conexus-frontend and Storybook coverage may lag unified frontend.

## Consumers

- **ontogony-frontend:** proven for 008 scope; browser E2E not re-run in 009.
- **conexus-frontend:** standalone app not in this workspace; **conexus-like** fixture passed `check:consumers` — re-run after a real Conexus UI bump.
- **Docker:** frontend image rebuilt 2026-05-20 (`verify-frontend-browser-provenance.ps1 -Build`); repeat after frontend commits that affect browser-visible UI.

## Test / CI

- `public-api-guard` requires every `package.json` export to appear in consumer-contract or fixture imports — add fixture lines when introducing new subpaths.
- `npm run check:full` (Playwright e2e) not run in sign-off — use before visual-regression-sensitive releases.
