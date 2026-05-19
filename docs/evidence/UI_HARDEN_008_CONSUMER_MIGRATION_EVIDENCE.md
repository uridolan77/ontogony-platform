# UI-HARDEN-008 — consumer migration evidence (platform mirror)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** `ontogony-frontend` proves `@ontogony/ui` hardening primitives on live operator routes; canonical detail lives in `ontogony-frontend/docs/evidence/UI_HARDEN_008_CONSUMER_MIGRATION_EVIDENCE.md`.

## Cross-repo touchpoints

| Primitive family | Package export | Proven on |
| --- | --- | --- |
| Route tabs | `@ontogony/ui/navigation` | Allagma evaluations sub-routes |
| Empty / limitation | `@ontogony/ui/feedback` | Product query gates, Kanon lifecycle |
| Diagnostics | `@ontogony/ui/diagnostics` | Evidence export, run ops limitations, domain-pack actions |
| Status | `@ontogony/ui/status` | Shell service chips, Kanon authority, Conexus overview health |
| Density / metrics | `@ontogony/ui/theme`, `@ontogony/ui/ui` | Overview metric grids (Allagma, Kanon, Conexus) |

## Validation (consumer)

```bash
cd C:\dev\ontogony-frontend
npm run typecheck
```

## Next expected item

**UI-HARDEN-009** — shared UI closeout and release readiness.
