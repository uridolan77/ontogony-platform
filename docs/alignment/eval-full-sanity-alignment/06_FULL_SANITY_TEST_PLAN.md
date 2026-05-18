# Full sanity test plan — SYS-FULL-SANITY-001

**Prerequisite:** ALIGN-EVAL-001 alignment package (this directory).  
**Outcome artifacts:**

```text
artifacts/full-sanity/full-sanity-report.json
allagma-dotnet/docs/evidence/SYS_FULL_SANITY_001_REPORT.md   (or platform copy per executor choice)
```

**Statement:** This is first full sanity, not production readiness.

## Preconditions

1. Services healthy: Kanon (5081), Conexus (5082), Allagma (5083), frontend dev server.
2. `Allagma:Persistence:Mode=Postgres` recommended for durable eval steps (InMemory acceptable for dry run with documented limitation).
3. Real external execution **disabled**.
4. Operator tokens configured in frontend settings.
5. OpenAPI provenance aligned (`npm run openapi:check` pass).

## Flow

### 1. Start stack

```powershell
# Example — use repo scripts where available
# Kanon, Conexus, Allagma APIs + ontogony-frontend dev
```

Record service versions / git SHAs in report.

### 2. Baseline Allagma run — `single_workflow`

- Execute governed run matching `case-single-workflow-summary` / expected overlay.
- Record `runId`, `traceId`, completion status.
- Confirm run events include topology selection (`single_workflow`, low risk, no Kanon auth required).

### 3. Subject Allagma run — `centralized_orchestrator` override

- Execute subject run with topology override per `case-topology-override-centralized`.
- Record `runId` and topology evidence.

### 4. Kanon topology authorization

- For centralized override path: confirm Kanon policy evaluation recorded (`topologyAuthorizationDecisionId` or decision record reference).
- For single_workflow baseline: document **null** authorization id as expected (see SYS-EVAL-001).

### 5. Conexus route / model evidence

- Confirm model call id and `routeDecisionId` on subject path where applicable.
- Verify route evidence metrics/tests pass (`ModelCallRouteEvidenceIntegrationTests`).

### 6. Write evaluation records

- Run eval harness or manual gated POST for subject/baseline runs.
- Persist evaluations; record `evaluationRunId`(s).
- Verify `GET /runs/{runId}/evaluations` returns records.

### 7. Create baseline comparison

- Create comparison baseline `single_workflow` vs subject `centralized_orchestrator`.
- Record `comparisonId`.
- Verify `GET /evaluations/baseline-comparisons/{comparisonId}`.

### 8. Frontend verification

| Page | Check |
| --- | --- |
| `/allagma/runs/{runId}` | Eval topology section lists evaluations |
| `/allagma/evaluations` | Live overview or honest empty state |
| `/allagma/evaluations/{evaluationRunId}` | Verdict, quality, redacted metrics |
| `/allagma/evaluations/baseline-comparisons/{comparisonId}` | Deltas and promotion fields |

Optional fixture cross-check: `?dashboardFixture=ci-suite` shows labelled matrix only.

### 9. Export report

Produce JSON report with:

- `verdict`: pass | fail
- `repos`: commit SHAs
- `runs`: baseline + subject ids
- `evaluations`: ids and verdicts
- `baselineComparisonId`
- `kanon`: topology decision refs
- `conexus`: route/model evidence refs
- `frontend`: pages verified
- `limitations`: known gaps encountered

## Automated gates (run before manual stack)

```powershell
cd c:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AllagmaEvaluationApi|FullyQualifiedName~AllagmaOpenApiSnapshot"
powershell -File ./scripts/run-eval-ci-suite.ps1

cd c:\dev\ontogony-frontend
npm run openapi:check
npm run test -- src/allagma/adapters/allagmaEvaluationAdapters.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts
npx playwright test e2e/allagma-eval-dashboards.spec.ts
```

## Pass criteria

- All automated gates pass.
- Manual stack steps complete without contract surprises.
- Frontend displays live data with correct banners (no fixture labelled as live).
- Report documents any expected nulls (e.g. null topology auth on baseline).

## Fail criteria

- OpenAPI drift between frontend and backend.
- Eval routes return unexpected error codes or empty bodies on 404.
- UI shows synthetic scores without fixture banner.
- Raw prompts/completions appear in UI or eval metrics.
