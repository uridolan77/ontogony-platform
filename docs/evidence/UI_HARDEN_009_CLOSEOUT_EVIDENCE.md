# UI-HARDEN-009 — closeout and release readiness evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** ONTOGONY-UI-HARDEN-001 sequence 000–009 is documented, validated, and closed with platform release artifacts; remaining gaps are explicit in known-limitations and next-options docs.

## Sequence verification

| Step | Evidence path | Verdict |
| --- | --- | --- |
| 000 audit | `ontogony-ui/docs/reviews/UI_HARDEN_000_CURRENT_STATE_AUDIT.md` | PASS |
| 001 AppShell | `ontogony-ui/docs/evidence/UI_HARDEN_001_APPSHELL_CONTRACT_EVIDENCE.md` | PASS |
| 002 status | `ontogony-ui/docs/evidence/UI_HARDEN_002_STATUS_TAXONOMY_EVIDENCE.md` | PASS |
| 003 dialogs | `ontogony-ui/docs/evidence/UI_HARDEN_003_DIALOG_ACCESSIBILITY_EVIDENCE.md` | PASS |
| 004 route tabs | `ontogony-ui/docs/evidence/UI_HARDEN_004_ROUTE_TABS_EVIDENCE.md` | PASS |
| 005 empty/limitation | `ontogony-ui/docs/evidence/UI_HARDEN_005_EMPTY_LIMITATION_EVIDENCE.md` | PASS |
| 006 evidence/action | `ontogony-ui/docs/evidence/UI_HARDEN_006_EVIDENCE_ACTION_PRIMITIVES_EVIDENCE.md` | PASS |
| 007 density/layout | `ontogony-ui/docs/evidence/UI_HARDEN_007_DENSITY_LAYOUT_EVIDENCE.md` | PASS |
| 008 consumer migration | `ontogony-frontend/docs/evidence/UI_HARDEN_008_CONSUMER_MIGRATION_EVIDENCE.md` | PASS |

## Closeout artifacts

| Document | Path |
| --- | --- |
| Closeout | `ontogony-platform/docs/releases/ONTOGONY_UI_SHARED_FOUNDATION_HARDENING_CLOSEOUT.md` |
| Scorecard | `ontogony-platform/docs/releases/ONTOGONY_UI_SHARED_FOUNDATION_HARDENING_SCORECARD.md` |
| Known limitations | `ontogony-platform/docs/releases/ONTOGONY_UI_SHARED_FOUNDATION_HARDENING_KNOWN_LIMITATIONS.md` |
| Next options | `ontogony-platform/docs/releases/ONTOGONY_UI_SHARED_FOUNDATION_HARDENING_NEXT_OPTIONS.md` |

## Validation run

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

**009 fix:** Added `./status`, `./navigation`, `./feedback` to `fixtures/pkg-consumer/import-all.mjs`, `require-all.cjs`, and `consumer-contract.imports.test.ts` so `public-api-guard` passes (was 645/646 before fix).

## Sign-off validation (2026-05-20)

```powershell
cd C:\dev\ontogony-ui
npm run check
npm run check:consumers

cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-frontend-browser-provenance.ps1 -Build
```

| Gate | Verdict |
| --- | --- |
| `npm run check` | PASS |
| `npm run check:consumers` | PASS |
| DOCKER-LOCAL-VERIFY-001 (`-Build`) | PASS — `c06afff` at `http://localhost:5175/` |

**Gate fixes applied:** removed unused `OntogonyDensity` import in `LimitationNotice.tsx`; added `@testing-library/user-event` to `package.json` devDependencies for knip.

## Program status

**ONTOGONY-UI-HARDEN-001 — COMPLETE** (items 000–009), sign-off gates executed.
