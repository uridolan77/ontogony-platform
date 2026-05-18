# Main use flow — local operator sanity

End-to-end path for the **first working local environment**: operator UI → governed runs → Kanon topology (where required) → Conexus route evidence → evaluations → baseline comparison → frontend visibility → report validation.

**Not production readiness.** Use fake Conexus provider and disabled real external execution unless explicitly running ENV-REAL-PROVIDER-001.

## Prerequisites

- Workspace layout verified (`01_WORKSPACE_LAYOUT.md`)
- Settings applied (`02_EXACT_SETTINGS.md`)
- APIs healthy on `5081` / `5082` / `5083` (see `04_STARTUP_MODES.md`)
- `Allagma__Evaluation__ManualWriteEnabled=true` for manual eval POST

## Flow (operator order)

1. **Start stack** — Kanon, Conexus, Allagma; then `ontogony-frontend` dev server.
2. **Health** — `GET /health` and `GET /ready` on each API base URL.
3. **Conexus dev project** — Bootstrap if needed; project key `cx-dev-key-change-me`.
4. **Baseline Allagma run** — `single_workflow`, low risk; `topologyAuthorizationDecisionId = null` **by design**.
5. **Subject Allagma run** — `centralized_orchestrator`, topology override; Kanon topology authorization **required** (non-empty decision id).
6. **Conexus evidence** — Confirm model-call and route-decision records for baseline and subject.
7. **Evaluations** — Manual POST (Development + `ManualWriteEnabled`) for baseline and subject runs.
8. **List evaluations** — Per run and/or dashboard sample paths documented in frontend.
9. **Baseline comparison** — Create and fetch comparison between baseline and subject evaluation runs.
10. **Frontend verification** — Operator routes (live or fixture-backed):
    - `/allagma/runs/{runId}`
    - `/allagma/evaluations`
    - `/allagma/evaluations/{evaluationRunId}`
    - `/allagma/evaluations/baseline-comparisons/{comparisonId}`
11. **Full sanity report** — `allagma-dotnet/scripts/run-full-sanity.ps1` (when stack is up).
12. **Validate report** — `allagma-dotnet/scripts/validate-full-sanity-report.ps1`.

ENV-RUN-001 adds a guided runner; ENV-FE-001 adds a frontend walkthrough script.

## Acceptance signals

```text
[ ] run-full-sanity.ps1 PASS
[ ] validate-full-sanity-report.ps1 PASS
[ ] baseline run completed
[ ] subject run completed
[ ] subject topology authorization non-empty
[ ] routeDecisionIds present (baseline + subject model calls)
[ ] evaluations created and listable
[ ] baseline comparison fetchable
[ ] frontend pages show evidence or honest empty/degraded states
[ ] no raw secrets/prompts/completions in UI or exports
[ ] real external execution disabled
```

Detailed checklist: `05_ACCEPTANCE_CHECKLIST.md`.

## Cross-references

| Topic | Location |
| --- | --- |
| Eval contract alignment | `docs/alignment/eval-full-sanity-alignment/` |
| Full sanity test plan | `docs/alignment/eval-full-sanity-alignment/06_FULL_SANITY_TEST_PLAN.md` |
| Allagma local profiles | `allagma-dotnet/docs/system/LOCAL_RUNTIME_PROFILES.md` |
| Frontend release runbook | `ontogony-frontend/docs/RELEASE_RUNBOOK.md` |
