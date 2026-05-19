# UI-HARDEN-000 — Current state audit evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (audit complete; implementation not started)  
**Statement:** Docs-only audit of `@ontogony/ui` foundation and `ontogony-frontend` duplication patterns. No source code changes in this step.

## Scope

Platform coordination record for `UI-HARDEN-000` from `Ontogony-UI-Shared-Foundation-Hardening-Package-v1`. Primary audit artifact lives in `ontogony-ui`.

## Delivered

| Repo | Artifact |
| --- | --- |
| `ontogony-ui` | `docs/reviews/UI_HARDEN_000_CURRENT_STATE_AUDIT.md` — full audit |
| `ontogony-platform` | This evidence file |

Package intake: `ontogony-ui/docs/_incoming/Ontogony-UI-Shared-Foundation-Hardening-Package-v1/`

## Audit conclusions (summary)

```text
Foundation:     @ontogony/ui is multi-console ready at subpath level (20+ exports, CI guards)
Main gaps:      AppShell contract undocumented; NavList tests thin; status taxonomy fragmented
Duplication:    frontend/shared EvidenceExportPanel vs @ontogony/ui/diagnostics EvidenceExportPanel
                ProductLiveQueryState vs ui/EmptyState; local route tabs (AllagmaEvaluationsNavTabs)
Next slice:     UI-HARDEN-001 — AppShell contract and tests (before status taxonomy)
```

## Key file references (verification)

| Check | Path |
| --- | --- |
| Public exports | `ontogony-ui/package.json` |
| Import doctrine | `ontogony-ui/docs/specs/PUBLIC_API_SURFACE_SPEC.md` |
| AppShell tests | `ontogony-ui/src/components/layout/AppShell.test.tsx` |
| ConfirmDialog tests | `ontogony-ui/src/components/dialogs/ConfirmDialog.test.tsx` |
| Live shell consumer | `ontogony-frontend/src/app/OntogonyShell.tsx` |
| Frontend evidence fork | `ontogony-frontend/src/shared/components/EvidenceExportPanel.tsx` |
| UI evidence primitive | `ontogony-ui/src/components/diagnostics/EvidenceExportPanel.tsx` |
| Roadmap order | `ontogony-ui/docs/_incoming/.../03_HARDENING_ROADMAP.md` |

## Validation performed

```text
- Read package.json exports and PUBLIC_API_SURFACE_SPEC / CONSUMER_IMPORTS
- Inventoried layout, ui, dialogs, system, semantic, execution, settings, observability, procedures, operator
- Reviewed AppShell / NavList / Dialog test files
- Grepped ontogony-frontend for route tabs, empty states, limitations, evidence export, status badges
- Confirmed required audit paths exist; no implementation prompts executed
```

## Next expected item

**UI-HARDEN-001** — AppShell contract and tests (`ontogony-ui` + platform evidence + optional `ontogony-frontend` consumer smoke).

Do **not** skip to UI-HARDEN-002 (status taxonomy) before AppShell contract is documented and tested per package roadmap Phase A.
