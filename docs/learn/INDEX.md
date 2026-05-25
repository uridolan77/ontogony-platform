# Ontogony learning index

> **Audience:** all contributors  
> **Applies to:** cross-repo Ontogony workspace (`ontogony-platform`, `kanon-dotnet`, `conexus-dotnet`, `allagma-dotnet`, `ontogony-frontend`, `ontogony-ui`)  
> **Source of truth:** guides below link to owning-repo docs and generated artifacts — do not copy generated tables here  
> **Last verified:** 2026-05-25 (`./scripts/validate-learn-docs.ps1`)

**Package:** `SYSTEM-LEARNING-GUIDE-001` — consolidation sprint, not a feature sprint.

## 60-minute new developer path

1. [00_START_HERE.md](./00_START_HERE.md)
2. [01_ARCHITECTURE_MAP.md](./01_ARCHITECTURE_MAP.md)
3. [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md)
4. [03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md)
5. [GLOSSARY.md](./GLOSSARY.md)

## Operator path

| Step | Guide |
| --- | --- |
| 1 | [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md) |
| 2 | [04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md) |
| 3 | [05_EVIDENCE_SPINE.md](./05_EVIDENCE_SPINE.md) |
| 4 | [06_AGENT_INTERACTION.md](./06_AGENT_INTERACTION.md) |
| 5 | [14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md) |

## Backend contributor path

| Step | Guide |
| --- | --- |
| 1 | [01_ARCHITECTURE_MAP.md](./01_ARCHITECTURE_MAP.md) |
| 2 | [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](./07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md) |
| 3 | [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md) |
| 4 | Extension: [09](./09_ADD_A_DOMAIN.md) · [10](./10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md) · [11](./11_ADD_OR_CHANGE_AN_API_ROUTE.md) |

## Frontend / UI contributor path

| Step | Guide |
| --- | --- |
| 1 | [12_ADD_A_FRONTEND_PAGE.md](./12_ADD_A_FRONTEND_PAGE.md) |
| 2 | [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md) |
| 3 | [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md) |

## All guides

| Guide | Primary audience |
| --- | --- |
| [00_START_HERE.md](./00_START_HERE.md) | new developer |
| [01_ARCHITECTURE_MAP.md](./01_ARCHITECTURE_MAP.md) | cross-repo integrator |
| [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md) | operator, developer |
| [03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md) | operator, backend |
| [04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md) | operator, frontend |
| [05_EVIDENCE_SPINE.md](./05_EVIDENCE_SPINE.md) | operator, frontend |
| [06_AGENT_INTERACTION.md](./06_AGENT_INTERACTION.md) | operator, frontend |
| [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](./07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md) | backend, integrator |
| [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md) | backend, frontend |
| [09_ADD_A_DOMAIN.md](./09_ADD_A_DOMAIN.md) | backend (Kanon) |
| [10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md](./10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md) | backend (Conexus) |
| [11_ADD_OR_CHANGE_AN_API_ROUTE.md](./11_ADD_OR_CHANGE_AN_API_ROUTE.md) | backend, frontend |
| [12_ADD_A_FRONTEND_PAGE.md](./12_ADD_A_FRONTEND_PAGE.md) | frontend |
| [13_ADD_AN_EVALUATION_OR_BASELINE.md](./13_ADD_AN_EVALUATION_OR_BASELINE.md) | backend (Allagma) |
| [14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md) | all |
| [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md) | frontend, UI |
| [GLOSSARY.md](./GLOSSARY.md) | all |
| [DOCS_AUDIT_MATRIX.md](./DOCS_AUDIT_MATRIX.md) | maintainer |

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-learn-docs.ps1
```

## Related maintainer docs

- Package spec (incoming): `docs/_incoming/SYSTEM-LEARNING-GUIDE-001.zip`
- Evidence closeout: [`../evidence/PLATFORM_SYSTEM_LEARNING_GUIDE_001.md`](../evidence/PLATFORM_SYSTEM_LEARNING_GUIDE_001.md)
- Platform doc hub: [`../README.md`](../README.md)
