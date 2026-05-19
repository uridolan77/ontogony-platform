# UI-HARDEN-002 — status taxonomy and badge variants (platform evidence)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (cross-repo acceptance)  
**Primary evidence:** `ontogony-ui/docs/evidence/UI_HARDEN_002_STATUS_TAXONOMY_EVIDENCE.md`

## Cross-repo summary

| Repo | Change |
| --- | --- |
| `ontogony-ui` | `@ontogony/ui/status` subpath; shared `OperatorStatusKind` taxonomy; `StatusBadge` family; shell/system cards wired |
| `ontogony-frontend` | Shell service chips use `StatusBadge` + product-owned `mapServiceHealthPresentationToStatusKind` |

Product apps retain DTO mapping; UI kit owns neutral display language and tone matrix.

## Next expected item

**UI-HARDEN-003** — dialog accessibility and modal contract (`ontogony-ui` primary).
