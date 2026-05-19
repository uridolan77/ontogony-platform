# UI-HARDEN-003 — dialog accessibility and modal contract (platform evidence)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (cross-repo acceptance)  
**Primary evidence:** `ontogony-ui/docs/evidence/UI_HARDEN_003_DIALOG_ACCESSIBILITY_EVIDENCE.md`

## Cross-repo summary

| Repo | Change |
| --- | --- |
| `ontogony-ui` | `Dialog` a11y ids, size/scroll/footer contract, focused tests, `DIALOG_CONTRACT.md` |
| `ontogony-frontend` | `DiagnosticExportPanel` uses `size="xl"` + `scrollBody`; redundant window Escape listener removed |

## Next expected item

**UI-HARDEN-005** — empty state and limitation primitives (`ontogony-ui` primary). See `UI_HARDEN_004_ROUTE_TABS_EVIDENCE.md` for 004.
