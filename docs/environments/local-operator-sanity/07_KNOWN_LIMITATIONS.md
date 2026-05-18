# Known limitations — local operator sanity

**Boundary: first working local environment, not production readiness.**

Items below are **accepted** for this program. They are not excuses to relax safety gates (fake provider first, no real external execution, no secrets in UI/reports).

## Program scope

- ENV-SETUP-001 is **documentation only**; stack automation arrives in ENV-SETUP-002 (`allagma-dotnet` scripts).
- `ontogony-ui` is not on the critical path until ENV-UI-001; product operator work uses `ontogony-frontend`.
- Postgres durable mode is **optional** (ENV-PG-001); InMemory is sufficient for first fake-provider sanity.

## Runner and automation

- `run-full-sanity.ps1` requires a live stack; not run as part of ENV-SETUP-001 doc delivery.
- Live browser walkthrough is manual until ENV-FE-001; Playwright gates cover adapter contracts in CI.
- Local `artifacts/full-sanity/` reports are gitignored; committed evidence lives under `docs/evidence/`.

## API and data (carried from eval alignment)

- No global `GET /allagma/v0/evaluations` list; dashboards sample recent runs.
- No cross-run eval trend API; trend UI is sample ordering.
- Manual `POST` evaluation and baseline comparison are operator/harness-gated; no frontend create forms.
- Run GET may not embed `evaluationRunIds` / `baselineComparisonId`; harness creates comparison separately.

## Topology and smoke

- Baseline `topologyAuthorizationDecisionId` is **null by design** (`single_workflow`).
- Some fixture replay kinds may omit `routeDecisionId`; live sanity expects non-empty Conexus route IDs for baseline + subject model calls.
- Live topology authorization E2E may need manual steps outside automated runner (AGM-TOPO-003 lineage).

## Persistence

- InMemory Allagma persistence does not survive process restart (use ENV-PG-001 to prove durability).
- Postgres eval smoke in CI requires `ALLAGMA_TEST_POSTGRES` (Docker).

## Frontend

- Dataset editor UI deferred; datasets managed in backend/CI.
- Eval route responsive/a11y coverage is partial vs full `responsive.spec.ts`.
- Empty dashboard is honest state until runs exist or `dashboardFixture=ci-suite` is used.

## Cross-repo

- Conexus/Kanon OpenAPI provenance tracked separately from Allagma eval contract.
- JSON duplicate-key scanner is Allagma OpenAPI CI concern only.

## Safety (not limitations — do not waive)

| Rule | Status |
| --- | --- |
| Real external execution | Disabled for first sanity |
| Conexus provider | Fake/local first |
| Manual eval POST | `ManualWriteEnabled` + non-production only |
| Secrets / raw prompts in UI or reports | Forbidden |

## Program status

Script-based local operator sanity is **closed** (ENV-CLOSEOUT-001, 2026-05-19). See `docs/releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md`.

## Next major phase

| Phase | Repo | Delivers |
| --- | --- | --- |
| ENV-DOCKER-LOCAL | `allagma-dotnet` (+ siblings) | First Docker/Postgres local working system — start with `ENV-DOCKER-001` in `allagma-dotnet/docs/environments/working-local-environment-complete-package/` |

Optional later: ENV-REAL-PROVIDER-001 (backend-only real provider smoke).
