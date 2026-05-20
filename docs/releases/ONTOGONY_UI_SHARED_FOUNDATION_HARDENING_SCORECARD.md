# Ontogony UI shared foundation hardening — scorecard

**Date:** 2026-05-20  
**Scale:** 1–5 (5 = strong for pre-1.0 shared-operator scope)

| Category | Score | Notes |
| --- | ---: | --- |
| Public API / packaging honesty | 4 | `dist` exports, 82 validated paths, ESM+CJS fixture consumer; root barrel compatibility-only |
| Shell & navigation contract | 4 | `APPSHELL_CONTRACT`, `RouteTabs`/`PageSubnav`, router-free `AppShell` |
| Status & feedback vocabulary | 4 | `OperatorStatusKind`, `StatusBadge`, `EmptyState`, `LimitationNotice` |
| Dialog accessibility | 4 | Confirm/destructive contracts, focus trap patterns documented |
| Diagnostics / evidence primitives | 4 | `EvidenceExportPanel`, `ActionAvailabilityPanel`, `UnsupportedActionList` |
| Density & layout | 4 | `OntogonyDensity`, `operatorMetricGridClass`, compact operator pages |
| Consumer adoption (`ontogony-frontend`) | 4 | Representative migrations; not every local wrapper removed |
| Automated test confidence | 4 | 646 UI tests; packaging guards; focused frontend tests pass |
| Storybook / visual regression | 3 | Storybook exists; not part of 009 validation run |
| Cross-consumer parity (Conexus app) | 2 | Not re-verified in this closeout |
| **Overall (program closeout)** | **4** | Foundation credible for continued product work; not a 1.0 freeze |

## Strengths

- Clear subpath doctrine (`PUBLIC_API_SURFACE_SPEC.md`, `CONSUMER_IMPORTS.md`).
- Router-agnostic shell with optional `layout-router` adapter.
- Hardening primitives proven on Allagma evaluations, overviews, Kanon lifecycle, and shell badges.
- CI guards: export validation, public-api-guard, consumer-contract imports, no `@ontogony/ui/src` in docs/fixtures.

## Gaps

- Partial migration: some product-local shells (`ProductLiveQueryState`, thin `EvidenceExportPanel` wrapper) remain by design.
- Parallel status vocabularies still exist for domain-specific badges (`RunStateBadge`, `SemanticVersionBadge`).
- Full `npm run check` and Playwright visual suite not executed in 009.
- Docker frontend image provenance not rebuilt in 009.
