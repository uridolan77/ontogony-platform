# Ontogony UI shared foundation hardening — known limitations

Consolidated from UI-HARDEN-000–009. Not a production readiness statement.

## Package & versioning

- **Pre-1.0** (`0.1.0-alpha.0`): additive subpaths are normal; breaking changes still require migration notes and consumer coordination.
- **Root barrel** (`.`) is compatibility-only — new product code must use documented subpaths.
- **Full release gate** (`npm run check` in `ontogony-ui`) includes Storybook build, knip, size-limit, and e2e — heavier than the 009 closeout validation run.

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
- **conexus-frontend:** file-linked consumer — verify `npm run check` after the next `@ontogony/ui` sync.
- **Docker:** rebuild `ontogony-frontend` image when cutting a stack that depends on new badge/density behavior.

## Test / CI

- `public-api-guard` requires every `package.json` export to appear in consumer-contract or fixture imports — add fixture lines when introducing new subpaths.
- 009 closeout did not run `npm run check:full` or Playwright visual regression.
