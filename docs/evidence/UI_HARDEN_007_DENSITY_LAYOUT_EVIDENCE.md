# UI-HARDEN-007 — density, layout, and responsive pass (platform mirror)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (consumer validation)  
**Canonical evidence:** `ontogony-ui/docs/evidence/UI_HARDEN_007_DENSITY_LAYOUT_EVIDENCE.md`

## Summary

- Shared `@ontogony/ui` density scale: `comfortable`, `compact`, `dense`.
- Allagma evaluations operator dashboard (`/allagma/evaluations`) migrated to `OperatorPageFrame density="compact"`, compact route tabs, and compact metric/result cards via shared helpers (`operatorMetricGridClass`, `Card density="compact"`).
- Storybook toolbar and `/dev/layout` lab document responsive + density behavior for operator laptops and narrow viewports.

## Cross-repo validation

```bash
cd C:\dev\ontogony-ui && npm run test:run -- src/theme/density.test.ts
cd C:\dev\ontogony-frontend && npm run typecheck
```
