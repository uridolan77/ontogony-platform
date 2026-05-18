# Main Use Flow

1. Start Kanon, Conexus, Allagma, and frontend.
2. Verify `/health` and `/ready`.
3. Bootstrap Conexus dev project if needed.
4. Run baseline Allagma workflow:
   - `single_workflow`
   - low risk
   - `topologyAuthorizationDecisionId = null` by design.
5. Run subject Allagma workflow:
   - `centralized_orchestrator`
   - topology override
   - Kanon topology authorization required.
6. Verify Conexus model-call and route-decision evidence.
7. Write evaluations for baseline and subject.
8. List evaluations on each run.
9. Create baseline comparison.
10. Verify frontend:
    - `/allagma/runs/{runId}`
    - `/allagma/evaluations`
    - `/allagma/evaluations/{evaluationRunId}`
    - `/allagma/evaluations/baseline-comparisons/{comparisonId}`
11. Export full sanity report.
12. Validate report.

Acceptance:

```text
[ ] run-full-sanity.ps1 PASS
[ ] validate-full-sanity-report.ps1 PASS
[ ] baseline run completed
[ ] subject run completed
[ ] subject topology authorization non-empty
[ ] routeDecisionIds present
[ ] evaluations pass
[ ] baseline comparison fetchable
[ ] frontend pages show evidence or honest empty/degraded states
[ ] no raw secrets/prompts/completions
[ ] real external execution disabled
```
