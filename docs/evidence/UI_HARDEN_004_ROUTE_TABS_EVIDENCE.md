# UI-HARDEN-004 — route tabs and page subnavigation (platform evidence)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (cross-repo acceptance)  
**Primary evidence:** `ontogony-ui/docs/evidence/UI_HARDEN_004_ROUTE_TABS_EVIDENCE.md`

## Cross-repo summary

| Repo | Change |
| --- | --- |
| `ontogony-ui` | `@ontogony/ui/navigation` (`RouteTabs`, `PageSubnav`, `resolveRouteTabActiveId`), contract + tests |
| `ontogony-frontend` | `AllagmaEvaluationsNavTabs` uses `PageSubnav` + `renderLink` (React Router) |

## Next expected item

**UI-HARDEN-005** — empty state and limitation primitives (`ontogony-ui` primary).
