# CONEXUS-DEEPEN-006 — Frontend observability v2 evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (tabbed operator workbench, cross-tab navigation, persistence notices, model-call evidence export)  
**Statement:** `/conexus/observability` is a coherent six-tab workbench; Recent requests is the default entry; diagnostics and provider posture are secondary tabs.

## Scope

`CONEXUS-DEEPEN-006` from `Ontogony-Conexus-Deep-Enhancement-Package-v1`.

## Repos touched

| Repo | Change |
| --- | --- |
| `ontogony-frontend` | Tabbed `ConexusObservabilityPage`, `RouteTabs` nav, persistence banners, tab deep-links, `buildConexusModelCallEvidence`, updated recent-requests navigation |
| `ontogony-platform` | This evidence file |

## Frontend structure

| Tab | Purpose |
| --- | --- |
| Recent requests (default) | `ConexusRecentModelCallsSection`, usage drill-down, guidance |
| Lookup | Request workbench (model-call detail, execution run, correlation, diagnostic export) |
| Route decisions | Route decision explorer + id lookup |
| Usage / cost | Governance window workbench + drill-down to recent rows |
| Provider posture | Provider validation inventory (non-dominant) |
| Diagnostics | Summary, warnings, API limitations |

URL: `?tab=<id>`; deep links without `tab` infer Lookup (`modelCallId`) or Route decisions (`routeDecisionId`).

## Empty / degraded states

`resolveConexusPersistenceNotice` drives banners for:

- in-memory-only telemetry (restart data loss)
- persistence disabled
- no telemetry yet
- no rows matching active filters

## Evidence export

- Lookup: redacted diagnostic bundle (`DiagnosticExportButton`)
- Model-call detail: `conexus.model-call.evidence.v1` bundle (`EvidenceExportPanel`)
- Execution run: existing request evidence export on lookup path

## Validation

```text
ontogony-frontend:
  npx vitest run src/conexus --reporter=dot
  → 20 files, 61 tests passed (2026-05-20)

  npx vitest run src/conexus/observability/conexusObservabilityTabs.test.ts
  npx vitest run src/conexus/pages/ConexusObservabilityPage.test.tsx
  npx vitest run src/conexus/diagnostics/buildConexusModelCallEvidence.test.ts

  e2e/conexus-observability.spec.ts — updated for tabbed flow (mocked APIs)
```

`npm run typecheck`: not clean repo-wide (pre-existing unrelated errors outside conexus surface).

## Known limitations

- Browser walkthrough not recorded in this pass (see CONEXUS-DEEPEN-007 checklist).
- Tab state is query-param based; full page refresh preserves tab + lookup ids.
- Route decisions tab requires `routeDecisionId` (from row action or manual paste).

## Next expected item

**CONEXUS-DEEPEN-007** — closeout scorecard, consolidated limitations, manual QA evidence.
