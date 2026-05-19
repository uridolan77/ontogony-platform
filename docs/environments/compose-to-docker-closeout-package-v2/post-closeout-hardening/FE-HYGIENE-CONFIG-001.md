# FE-HYGIENE-CONFIG-001 — Frontend config hygiene

## Purpose

Catalog and enforce `VITE_*` compile-time configuration after Docker-local closeout: typed env surface, centralized Docker-local defaults, CI `config:check`, and operator contract.

## Timing

Post-`FE-TEST-REPLAY-001`. Does not block the closed Docker-local milestone.

## Boundary

- Catalog, automated checks, operator docs, and default centralization **first**
- No production readiness claim
- No runtime nginx env injection in this PR
- No backend contract changes

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-frontend` | `scripts/frontend-env-catalog.json`; `config:audit` / `config:check`; `docs/operators/FRONTEND_CONFIG_OPERATOR_CONTRACT.md`; `docs/generated/FE_FRONTEND_ENV_CATALOG.md`; `src/shared/config/dockerLocalServiceDefaults.ts`; evidence |
| `ontogony-platform` | Spec; status board; evidence pointer |

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Catalog | All `import.meta.env.VITE_*` usages documented in `frontend-env-catalog.json` |
| 2 | Types | `src/vite-env.d.ts` lists every catalogued optional/required `VITE_*` |
| 3 | `.env.example` | Documents overridable service URLs, ontology id, and commented streaming flag |
| 4 | Defaults | `dockerLocalServiceDefaults.ts` is the single source for Docker-local URL/ontology fallbacks |
| 5 | Docker wiring | `Dockerfile` build args and compose `FRONTEND_VITE_*` mapping validated when platform sibling exists |
| 6 | CI gate | `config:check` in `npm run check` |
| 7 | Operator contract | `FRONTEND_CONFIG_OPERATOR_CONTRACT.md` explains compile-time vs secrets vs overrides |
| 8 | Docker-local link | `FRONTEND_DOCKER_LOCAL_CONTRACT.md` references config contract |
| 9 | No unjustified backend changes | Frontend + docs/scripts only |

## Operator verification

```powershell
cd C:\dev\ontogony-frontend

npm run config:audit
npm run config:check
npm run test -- scripts/lib/frontend-env-catalog.test.mjs
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-frontend` | `docs/evidence/FE_HYGIENE_CONFIG_001_FRONTEND_CONFIG_EVIDENCE.md` |
| `ontogony-platform` | `docs/evidence/FE_HYGIENE_CONFIG_001_EVIDENCE.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `UI-PACKAGING-STATUS-001` | `@ontogony/ui` packaging status |
