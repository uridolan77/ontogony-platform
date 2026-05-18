# Acceptance checklist — local operator sanity

Use when closing the **first working local environment** program (through ENV-CLOSEOUT-001). This is **not** a production readiness gate.

## Workspace and settings

```text
[ ] All six repos exist under C:\dev\ (including ontogony-ui)
[ ] ONTOGONY_* / *_ROOT env vars documented and set for session
[ ] Kanon on http://localhost:5081
[ ] Conexus on http://localhost:5082
[ ] Allagma on http://localhost:5083
[ ] Allagma__Evaluation__ManualWriteEnabled=true in Development
[ ] Conexus fake/local provider (no real API key required for first sanity)
[ ] Conexus project key cx-dev-key-change-me aligned on Allagma + Conexus
```

## Stack health

```text
[ ] Kanon /health and /ready pass
[ ] Conexus /health and /ready pass
[ ] Allagma /health and /ready pass
[ ] Frontend dev server starts (ontogony-frontend)
```

## Automated gates (when stack is running)

```text
[ ] run-full-sanity.ps1 PASS
[ ] validate-full-sanity-report.ps1 PASS
```

## Governed runs and evidence

```text
[ ] Baseline single_workflow run completed
[ ] Subject centralized_orchestrator run completed
[ ] Subject Kanon topology authorization present (non-empty)
[ ] Baseline topology authorization null and documented as by-design
[ ] Baseline + subject routeDecisionId present for model calls
[ ] Evaluation records created and listed per run
[ ] Baseline comparison created and fetchable
```

## Frontend operator surfaces

```text
[ ] /allagma/runs/{runId} shows run evidence or honest degraded state
[ ] /allagma/evaluations usable (live or dashboardFixture=ci-suite)
[ ] /allagma/evaluations/{evaluationRunId} loads
[ ] /allagma/evaluations/baseline-comparisons/{comparisonId} loads
[ ] Fixture/live banners correct
```

## Safety (required)

```text
[ ] No raw prompts/completions/secrets in UI or exported reports
[ ] Real external execution disabled on Allagma
[ ] No production-readiness claim in evidence or closeout docs
```

## Optional (later PRs — not blocking first fake-provider sanity)

```text
[ ] ENV-PG-001 — eval records survive Allagma restart on Postgres
[ ] ENV-UI-001 — ontogony-ui integration readiness
[ ] ENV-REAL-PROVIDER-001 — backend-only real provider smoke
```
