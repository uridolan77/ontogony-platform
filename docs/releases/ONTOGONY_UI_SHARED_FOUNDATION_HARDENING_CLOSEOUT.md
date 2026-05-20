# Ontogony UI shared foundation hardening — closeout

**Program:** ONTOGONY-UI-HARDEN-001 (`Ontogony-UI-Shared-Foundation-Hardening-Package-v1`)  
**Date:** 2026-05-20  
**Posture:** Pre-1.0 shared package maturity — **not** a semver-1.0 or design-system freeze

## Summary

The UI hardening sequence (`UI-HARDEN-000` through `UI-HARDEN-009`) moved `@ontogony/ui` from a strong internal source package to a **dist-backed, subpath-governed operator foundation** with proven adoption in `ontogony-frontend`. Product DTO mapping, OpenAPI clients, and route trees remain in consumer repos.

## Sequence outcomes

| ID | Outcome | Primary evidence |
| --- | --- | --- |
| 000 | Current-state audit and extraction priorities | `ontogony-ui/docs/reviews/UI_HARDEN_000_CURRENT_STATE_AUDIT.md` |
| 001 | AppShell contract + NavList/AppShell tests | `ontogony-ui/docs/evidence/UI_HARDEN_001_APPSHELL_CONTRACT_EVIDENCE.md` |
| 002 | Operator status taxonomy + `StatusBadge` | `ontogony-ui/docs/evidence/UI_HARDEN_002_STATUS_TAXONOMY_EVIDENCE.md` |
| 003 | Dialog accessibility + modal contract | `ontogony-ui/docs/evidence/UI_HARDEN_003_DIALOG_ACCESSIBILITY_EVIDENCE.md` |
| 004 | `RouteTabs` / `PageSubnav` | `ontogony-ui/docs/evidence/UI_HARDEN_004_ROUTE_TABS_EVIDENCE.md` |
| 005 | `EmptyState` / limitation primitives | `ontogony-ui/docs/evidence/UI_HARDEN_005_EMPTY_LIMITATION_EVIDENCE.md` |
| 006 | Evidence export + action availability panels | `ontogony-ui/docs/evidence/UI_HARDEN_006_EVIDENCE_ACTION_PRIMITIVES_EVIDENCE.md` |
| 007 | Density scale + layout pass | `ontogony-ui/docs/evidence/UI_HARDEN_007_DENSITY_LAYOUT_EVIDENCE.md` |
| 008 | Consumer migration (`ontogony-frontend`) | `ontogony-frontend/docs/evidence/UI_HARDEN_008_CONSUMER_MIGRATION_EVIDENCE.md` |
| 009 | Closeout, scorecard, limitations, next options | `ontogony-platform/docs/evidence/UI_HARDEN_009_CLOSEOUT_EVIDENCE.md` |

## Validation (009 closeout run)

```powershell
cd C:\dev\ontogony-ui
npm run build
npm run test:run
npm run check:exports
npm run check:smoke-dist
npm run check:smoke-named-exports

cd C:\dev\ontogony-frontend
npm run typecheck
npm run test:run -- src/kanon/components/KanonOperatorContextCard.test.tsx src/shared/components/ProductLiveQueryState.test.tsx src/shared/components/EvidenceExportPanel.test.tsx src/app/route-coverage.test.ts
```

**Results (2026-05-20, 009 baseline):** `ontogony-ui` build OK; **646/646** unit tests pass after fixture coverage for `./status`, `./navigation`, `./feedback`; export and dist smoke checks OK. `ontogony-frontend` typecheck OK; focused consumer tests **15/15** pass.

**Results (2026-05-20, sign-off run):** `npm run check` **PASS** (after removing unused `LimitationNotice` import and listing `@testing-library/user-event` in devDependencies). `npm run check:consumers` **PASS** (conexus-like, kanon-like, allagma-like, ontogony-frontend-like fixtures). Docker `verify-frontend-browser-provenance.ps1 -Build` **PASS** — served commit `c06afff` at `http://localhost:5175/` (report: `docker/local-working-system/artifacts/docker-local-verify-001-report.json`).

## Consumer adoption

`ontogony-frontend` uses public subpaths only (`@ontogony/ui/layout`, `/status`, `/navigation`, `/feedback`, `/diagnostics`, `/theme`, …). Representative pages: unified shell, evaluations dashboard, service overviews, domain-pack lifecycle, evidence export panels.

`conexus-frontend` remains a separate consumer; re-verify on the next Conexus UI PR that touches shared primitives.

## Related programs (out of scope)

- **Kanon / Conexus / Allagma deepening** — product routes and backend contracts (separate evidence).
- **UI-HARDEN-007–014 pr-plan** — longer-term package honesty, Storybook, DataTable, governance tracks documented in `ontogony-ui/docs/pr-plan/UI-HARDEN-007-014.md` (partially overlapping naming; this package closes **000–009** only).

## Sign-off criteria

- [x] Sequence 000–008 documented with per-slice evidence
- [x] Consumer migration proven on live operator routes
- [x] Public export guard covers all `package.json` subpaths (fixture + contract)
- [x] Scorecard, limitations, and next options published
- [x] Full `npm run check` + Docker frontend provenance refresh (2026-05-20)
- [x] Multi-console consumer fixtures (`npm run check:consumers`) — includes conexus-like smoke
