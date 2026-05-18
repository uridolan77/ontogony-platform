# First working local environment — known limitations

**Date:** 2026-05-19  
**Scope:** ENV-CLOSEOUT-001 closeout (script-based local operator program)

**This is first working local environment, not production readiness.**

These items are **accepted** for this milestone. They are not blockers for closing ENV-CLOSEOUT-001.

## Program scope

- **Script-based only** — services run as local processes on 5081–5083, not Docker Compose (see ENV-DOCKER-LOCAL).
- **Fake/local Conexus provider** is the default; ENV-REAL-PROVIDER-001 is optional and deferred.
- InMemory Allagma is sufficient for first sanity; Postgres durable mode is optional (ENV-PG-001).

## Runner and automation

- `run-full-sanity.ps1` / guided flow require a live stack (or `-StartServices`).
- Live browser walkthrough is **manual**; runner uses `-SkipFrontendChecks` for faster operator cycles.
- Local `artifacts/full-sanity/` reports are gitignored; committed evidence is under `docs/evidence/`.

## API and data

- No global `GET /allagma/v0/evaluations` list; dashboards sample recent runs.
- No cross-run eval trend API.
- Manual `POST` evaluation and baseline comparison are harness/operator-gated; no frontend create forms.
- Run GET may not embed `evaluationRunIds` / `baselineComparisonId`.

## Topology and smoke

- Baseline `topologyAuthorizationDecisionId` is **null by design** (`single_workflow`).
- Some fixture replay kinds may omit `routeDecisionId`; live sanity expects non-empty Conexus route IDs for baseline + subject model calls.

## Persistence

- Default InMemory Allagma does not survive process restart (use ENV-PG-001 Postgres mode to prove durability).
- Postgres eval smoke in CI requires `ALLAGMA_TEST_POSTGRES` (Docker).

## Frontend

- Dataset editor UI deferred.
- Eval route responsive/a11y coverage is partial vs full responsive suite.
- Empty dashboard is honest until runs exist or `dashboardFixture=ci-suite` is used.

## Cross-repo

- Conexus/Kanon OpenAPI provenance tracked separately from Allagma eval contract.
- JSON duplicate-key scanner is Allagma OpenAPI CI concern only.

## Safety (non-limitations — do not waive)

| Rule | Status |
| --- | --- |
| Real external execution | Disabled for first environment |
| Conexus provider | Fake/local first |
| Manual eval POST | `ManualWriteEnabled` + non-production only |
| Secrets / raw prompts in UI or reports | Forbidden |

## Source alignment

Consolidated from `docs/environments/local-operator-sanity/07_KNOWN_LIMITATIONS.md`, eval-full-sanity alignment docs, and ENV program evidence.
