# Known limitations — eval alignment (carried into full sanity)

Consolidated from BE-POLISH-001, FE-POLISH-001, FE-EVAL-002, and eval sequence evidence. **Not blocking** SYS-FULL-SANITY-001 if documented in the sanity report.

## API and data scope

- No `GET /allagma/v0/evaluations` global list; dashboard samples recent runs only.
- No cross-run eval trend or time-series API; trend panel is sample ordering.
- `POST` evaluation and baseline comparison are harness/operator-gated; no frontend create forms.
- Run topology summary may not link `evaluationRunIds` / `baselineComparisonId` on live run GET (harness produces comparison separately).

## Persistence and CI

- Postgres eval persistence smoke requires `ALLAGMA_TEST_POSTGRES` (Docker/CI).
- `artifacts/eval/` and `artifacts/sys-eval/` remain local/gitignored; committed evidence under `docs/evidence/`.
- Legacy flat fixtures under `docs/evals/cases` retained alongside `scenario-dataset-v0`.

## Topology and smoke expectations

- `topologyAuthorizationDecisionId` is **null by design** for `single_workflow` / low-risk paths.
- Fixture harness `routeDecisionId` may be null for some replay kinds (e.g. `provider_fallback_recorded`).
- Live-stack topology authorization E2E remains partially manual (AGM-TOPO-003).

## Frontend

- Dataset editor UI deferred; dataset management is backend/CI only.
- Responsive/a11y matrix for eval routes: happy + empty/error in eval spec; not full `responsive.spec.ts`.
- Trend view labelled as sample in limitations card.

## Cross-repo mechanics

- JSON duplicate-object-key scanner is Allagma OpenAPI snapshot CI concern; not replicated in Conexus/Kanon.
- Conexus/Kanon OpenAPI provenance tracked separately from Allagma eval contract.

## Safety (non-limitations — do not relax)

- Real external execution stays disabled for sanity.
- Manual eval POST requires `ManualWriteEnabled` and non-production.
- No raw prompts/completions in route evidence, eval metrics, or operator UI.

## Next milestone

After SYS-FULL-SANITY-001 passes: **RC-FIRST-SANITY-001** closeout with scorecard and explicit “first full sanity, not production readiness” statement.
