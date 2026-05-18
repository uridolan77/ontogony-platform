# ENV-FE-001 — Guided frontend operator walkthrough

Repos: `ontogony-frontend`, optional `ontogony-ui`

Add:

```text
docs/development/LOCAL_OPERATOR_WALKTHROUGH.md
docs/evidence/ENV_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md
scripts/open-local-operator-pages.ps1
```

Check pages:

```text
/allagma/runs/{runId}
/allagma/evaluations
/allagma/evaluations/{evaluationRunId}
/allagma/evaluations/baseline-comparisons/{comparisonId}
/allagma/evaluations?dashboardFixture=ci-suite
```

Verification:

```powershell
npm run openapi:check
npm run test -- src/allagma/adapters/allagmaEvaluationAdapters.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts
npx playwright test e2e/allagma-eval-dashboards.spec.ts
```

If local `ontogony-ui` consumption is needed, document it. Do not force local linking unless the repo already uses it.
