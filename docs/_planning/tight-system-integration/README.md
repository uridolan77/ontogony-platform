# Tight system integration — active planning

**Status:** Planning only (post–intake 2026-05-21)  
**Intake reconciliation:** [`docs/reviews/ONTOGONY_TIGHT_SYSTEM_INTEGRATION_PACKAGE_INTAKE_RECONCILIATION.md`](../../reviews/ONTOGONY_TIGHT_SYSTEM_INTEGRATION_PACKAGE_INTAKE_RECONCILIATION.md)  
**Source package:** [`docs/_incoming/Ontogony_Tight_System_Integration_Package_2026-05-21/`](../../_incoming/Ontogony_Tight_System_Integration_Package_2026-05-21/)

## Locked baseline

`SYSTEM-ALPHA-006` — see `allagma-dotnet/docs/system/ontogony-runtime.lock.json`. Do not change without a dedicated lock PR.

## Active sequence (recommended)

1. **SYS-TIGHT-001** — Post-lock delta register + release-mode validator (Allagma + Platform, docs/CI only).
2. **SYS-TIGHT-002 (trimmed)** — Evidence spine contract index doc (alias to existing Evidence Spine program).
3. **SYS-TIGHT-003** — Run-centric operator audit journey (Frontend + UI).
4. **SYS-TIGHT-004** — Route-preview/quota in audit/spine context (Frontend + Conexus docs).
5. **SYS-TIGHT-005** — Stream lifecycle operator panel (Allagma + Frontend).
6. **SYS-TIGHT-006** — Failure taxonomy adapter (Platform + Allagma + Frontend).
7. **SYS-TIGHT-007** — Release-candidate checklist consolidation (Allagma).
8. **SYS-TIGHT-008** — Production-readiness separation backlog (planning).

Lock promotion (`SYSTEM-ALPHA-007` or next id) is a **separate** PR after P0 gates on classified moving-main deltas.

## Boundaries

- Kanon: v0 frozen — additive compatibility/evidence/operator-read only.
- Allagma: governed runtime owner; no semantic or provider authority.
- Conexus: model gateway; no ontology meaning.
- Platform: shared mechanics and registry; no product semantics.
