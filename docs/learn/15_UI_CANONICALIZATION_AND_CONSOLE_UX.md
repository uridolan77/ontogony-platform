# UI canonicalization and console UX

> **Audience:** frontend developer, UI contributor  
> **Applies to:** `ontogony-frontend`, `ontogony-ui`  
> **Source of truth:** `ontogony-frontend/docs/ux/CONSOLE_CANONICAL_UI_SET.md`  
> **Last verified:** 2026-05-25

## Governing rule

```text
Need to present something?
  → Existing @ontogony/ui primitive?
  → Existing ontogony-frontend shared component?
  → Pattern on ≥2 pages with no fit? → one canonical primitive (documented + tested)
  → One page only? → inline JSX or temporary_bridge with removal target
```

Do **not** stack parallel layers (`OperatorPageHeader`, `SignalSummaryStrip`, `consoleUx/*`) on top of `OperatorPageFrame`.

## Canonical layout

| Concern | Use |
| --- | --- |
| Page shell | `OperatorPageFrame` |
| Purpose copy | One sentence in `description` |
| Status signals | `statusSlot` + `OperatorSignalSummary` (≤3 groups) |
| Next action | `NextActionPanel` |
| Advanced / debug | `<details>` / `OperatorDisclosure`, closed by default |

## Repo split

| Change | Where |
| --- | --- |
| AppShell, primitives, a11y | `ontogony-ui` |
| Route pages, signal copy, API wiring | `ontogony-frontend` |

See `ontogony-frontend/docs/ux/CONSOLE_UI_REPO_CONSOLIDATION.md`.

## Migration status

Batch migration plan and gates: `ontogony-frontend/docs/ux/CONSOLE_PAGE_MIGRATION_PLAN.md`

Closed packages (historical notes): archived under `docs/_incoming/_consumed/` (see [`MANIFEST.md`](../_incoming/_consumed/MANIFEST.md)).

## Verification commands

```powershell
cd C:\dev\ontogony-ui
npm run build

cd C:\dev\ontogony-frontend
npm run typecheck
npm run test -- src/system
npm run console-ui:check
npm run contracts:discipline
npx playwright test e2e/system-truth.spec.ts e2e/settings.spec.ts e2e/high-value-operator-pages.spec.ts
```

## Generated inventory

Link only: `ontogony-frontend/docs/generated/console-ui-component-inventory.json`

## References

- [12_ADD_A_FRONTEND_PAGE.md](./12_ADD_A_FRONTEND_PAGE.md)
- [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md)
- `ontogony-ui/docs/COMPONENT_CONTRACTS.md`
