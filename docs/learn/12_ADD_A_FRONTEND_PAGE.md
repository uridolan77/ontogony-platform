# Add a frontend console page

> **Audience:** frontend developer  
> **Applies to:** `ontogony-frontend`, `ontogony-ui`  
> **Source of truth:** `ontogony-frontend/docs/ux/CONSOLE_CANONICAL_UI_SET.md`, `route-workflow-catalog.json`  
> **Last verified:** 2026-05-25

## Checklist

1. **Route** — Add React Router entry; register in route-workflow catalog.
2. **Layout** — Use `OperatorPageFrame` from `@ontogony/ui` ([15](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md)).
3. **Signals** — Add `build*SignalGroups` in `operatorConsoleSignals.ts` (max three visible groups).
4. **Data** — Use generated OpenAPI client types, not handwritten DTOs ([08](./08_CONTRACT_DISCIPLINE.md)).
5. **Progressive disclosure** — Advanced/debug in `<details>` / `OperatorDisclosure`, closed by default.
6. **Evidence links** — Deep-link to run detail, Evidence Spine, or Agent Interaction where relevant.
7. **Tests** — Vitest for mappers; Playwright if high-value operator path.
8. **Gates** — `npm run typecheck`, `npm run contracts:discipline`, `npm run console-ui:check`.

## Do not

- Add parallel headers (`PageHeader` + `OperatorPageFrame` on same page).
- Create `consoleUx/*` duplicate primitives — extend `@ontogony/ui` or shared frontend components.
- Hand-copy OpenAPI field lists into markdown.

## Docker-live verification

After UI changes with compose stack:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-frontend-browser-provenance.ps1 -Build
```

## References

- [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md)
- `ontogony-ui/docs/APPSHELL_CONTRACT.md`
- `ontogony-frontend/docs/ux/CONSOLE_PAGE_MIGRATION_PLAN.md`
