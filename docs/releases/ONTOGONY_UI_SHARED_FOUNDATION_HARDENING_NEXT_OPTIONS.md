# Ontogony UI shared foundation hardening — next options

After UI-HARDEN-009 closeout. Not committed work.

| ID | Theme | Rationale |
| --- | --- | --- |
| **UI-HARDEN-010+** | pr-plan tracks (Storybook depth, DataTable, forms platform, governance) | See `ontogony-ui/docs/pr-plan/UI-HARDEN-007-014.md` for longer-horizon package maturity |
| ~~**UI-CONSUMER-001**~~ | ~~Finish `ontogony-frontend` wrapper elimination~~ | **Done (2026-05-20)** — Pass A–C; see `UI_CONSUMER_001_FRONTEND_UI_FOUNDATION_FINALIZATION_EVIDENCE.md` |
| **UI-CONSUMER-002** | Conexus-frontend parity pass | Re-run consumer-contract smoke after `@ontogony/ui` version bump |
| **UI-RELEASE-001** | Full `npm run check` + Changesets cut | Promote `0.1.0-alpha.x` with changelog when platform stack is ready |
| **UI-DOCKER-001** | Frontend provenance rebuild | `verify-frontend-browser-provenance.ps1 -Build` in local-working-system |
| **EVIDENCE-SPINE-001** | Cross-service evidence resolver | Complements diagnostics primitives; do not duplicate in `@ontogony/ui` |

## Recommended order

1. Run **UI-DOCKER-001** when operators next exercise the unified console in Docker.
2. Run **UI-RELEASE-001** before tagging a shared UI version consumed by multiple frontends.
3. **UI-CONSUMER-001** is closed; prefer product deepening (Kanon/Conexus/Allagma packages) unless a new consumer gap appears.
4. Treat **UI-HARDEN-010+** as incremental PRs, not a second big-bang program.

## Explicit non-goals

- Semver-1.0 `@ontogony/ui` freeze without platform sign-off.
- Moving Kanon/Allagma/Conexus API semantics into the UI package.
- Replacing product-specific workbenches with generic pages in `@ontogony/ui/pages`.
