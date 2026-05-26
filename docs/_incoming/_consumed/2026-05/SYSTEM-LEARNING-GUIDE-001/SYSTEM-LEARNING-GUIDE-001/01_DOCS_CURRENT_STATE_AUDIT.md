# 01 — Docs current-state audit

## Method

Cursor must inspect the actual local repos before finalizing this matrix. The rows below are seed targets, not final truth.

| Repo | Path/pattern | Classification | Audience | Canonical replacement/link | Action |
|---|---|---|---|---|---|
| ontogony-platform | `docs/ARCHITECTURE.md` | reference/candidate central source | all | `docs/learn/01_ARCHITECTURE_MAP.md` | keep, link, reconcile |
| ontogony-platform | `docs/CONTRACTS.md` | reference | backend/frontend | `docs/learn/08_CONTRACT_DISCIPLINE.md` | keep, link |
| ontogony-platform | `docs/contracts/*` | reference/generated mix | integrator | `08_CONTRACT_DISCIPLINE.md` | classify individually |
| ontogony-platform | `docs/operators/*` | operator reference | operator | `02_RUN_LOCAL_SYSTEM.md`, `14_DEBUGGING_PLAYBOOK.md` | keep/update/link |
| ontogony-platform | `docs/evidence/*` | evidence/reference | maintainer | `03`, `04`, `05` guides | link, avoid copying |
| ontogony-platform | `docs/generated/*` | generated evidence | maintainer | links only | generated_do_not_edit |
| ontogony-frontend | `route-workflow-catalog.json` | source artifact | frontend/integrator | `08`, `11`, `12` | keep, link |
| ontogony-frontend | `docs/generated/*` | generated evidence | frontend/integrator | links only | generated_do_not_edit |
| ontogony-frontend | `docs/ux/*` | UX reference | frontend/UI | `15_UI_CANONICALIZATION...` | keep/link/deduplicate |
| ontogony-ui | `docs/APPSHELL_CONTRACT.md` | canonical UI reference | frontend/UI | `15_UI_CANONICALIZATION...` | keep/link |
| ontogony-ui | `docs/COMPONENT_CONTRACTS.md` | canonical UI reference | frontend/UI | `15_UI_CANONICALIZATION...` | keep/link |
| allagma-dotnet | `docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json` | generated evidence | backend/integrator | `08`, `11` | generated_do_not_edit |
| conexus-dotnet | route inventory/OpenAPI snapshots | generated/reference | backend/integrator | `08`, `10`, `11` | link |
| kanon-dotnet | `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json` | generated evidence | backend/integrator | `08`, `09`, `11` | generated_do_not_edit |

## Required audit outputs

For every inspected doc, add:

- file path
- repo
- current/stale/duplicate/generated/reference
- audience
- canonical replacement
- action: keep, update, link_from_index, archive, delete_candidate, generated_do_not_edit
- evidence: command/file existence checked
