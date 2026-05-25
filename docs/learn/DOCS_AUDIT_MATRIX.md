# Documentation audit matrix

> **Audience:** maintainer  
> **Applies to:** six-repo Ontogony workspace  
> **Source of truth:** filesystem inspection + `validate-learn-docs.ps1`  
> **Last verified:** 2026-05-25 (SYSTEM-LEARNING-GUIDE-001 Phase 1 seed)

**Action legend:** `keep` · `link_from_index` · `update` · `archive` · `delete_candidate` · `generated_do_not_edit`

| Path | Repo | Class | Audience | Canonical learning link | Action |
| --- | --- | --- | --- | --- | --- |
| `docs/learn/*` | platform | **canonical learning** | all | [INDEX.md](./INDEX.md) | keep |
| `docs/ARCHITECTURE.md` | platform | reference | all | [01_ARCHITECTURE_MAP.md](./01_ARCHITECTURE_MAP.md) | keep, link |
| `docs/CONTRACTS.md` | platform | reference | integrator | [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md) | keep, link |
| `docs/operators/*` | platform | operator reference | operator | [02](./02_RUN_LOCAL_SYSTEM.md), [14](./14_DEBUGGING_PLAYBOOK.md) | keep, link |
| `docs/evidence/*` | platform | evidence | maintainer | [03](./03_GOVERNED_FAKE_E2E.md), [04](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md) | link only |
| `docs/generated/*` | platform | generated | maintainer | [08](./08_CONTRACT_DISCIPLINE.md) | generated_do_not_edit |
| `docs/_incoming/*` | platform | incoming spec | maintainer | learn guides when promoted | archive after promotion |
| `docker/local-working-system/README.md` | platform | reference | operator | [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md) | keep, link |
| `docs/CURRENT_STATE.md` | kanon | reference | backend | [01](./01_ARCHITECTURE_MAP.md), [09](./09_ADD_A_DOMAIN.md) | keep, link |
| `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json` | kanon | generated | integrator | [08](./08_CONTRACT_DISCIPLINE.md) | generated_do_not_edit |
| `docs/CURRENT_STATE.md` | allagma | reference | backend | [01](./01_ARCHITECTURE_MAP.md), [03](./03_GOVERNED_FAKE_E2E.md) | keep, link |
| `docs/TESTING.md` | allagma | reference | backend | [03](./03_GOVERNED_FAKE_E2E.md), [14](./14_DEBUGGING_PLAYBOOK.md) | keep, link |
| `docs/system/ontogony-runtime.lock.json` | allagma | evidence | maintainer | [04](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md) | keep, link |
| `docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json` | allagma | generated | integrator | [11](./11_ADD_OR_CHANGE_AN_API_ROUTE.md) | generated_do_not_edit |
| `docs/evidence/AGM_*.md` | allagma | evidence | maintainer | [03](./03_GOVERNED_FAKE_E2E.md) | keep, link |
| `docs/CURRENT_STATE.md` | conexus | reference | backend | [10](./10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md) | keep, link |
| `docs/ux/CONSOLE_CANONICAL_UI_SET.md` | frontend | UX reference | frontend | [15](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md) | keep, link |
| `route-workflow-catalog.json` | frontend | source artifact | frontend | [08](./08_CONTRACT_DISCIPLINE.md), [12](./12_ADD_A_FRONTEND_PAGE.md) | keep |
| `docs/generated/*` | frontend | generated | frontend | [08](./08_CONTRACT_DISCIPLINE.md) | generated_do_not_edit |
| `docs/APPSHELL_CONTRACT.md` | ontogony-ui | UI reference | frontend | [15](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md) | keep, link |
| `docs/_incoming/packages/*` | various | historical package | maintainer | matching `docs/learn` guide | mark historical in place |

## Stale-doc marker template (Phase 4)

When deprecating a narrative doc, add at the top:

```markdown
> **Historical:** This document is not the learning-path source of truth.  
> **Canonical guide:** [link to docs/learn/…](…)
```

Do not delete until a maintainer reviews `delete_candidate` rows.

## Next audit pass

Re-run classification after major packages (console UX final gate, replay full integration) and extend rows for any new `docs/_incoming_active/` closeouts.
