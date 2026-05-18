# ENV-RUN-001 — Guided main flow runner and validator

Repo: `allagma-dotnet`

Add:

```text
scripts/env/run-guided-main-flow.ps1
scripts/env/validate-guided-main-flow.ps1
docs/development/LOCAL_OPERATOR_SANITY_RUNBOOK.md
```

The script should:

1. print the flow being tested
2. show required env vars
3. support `-UseExistingServices`, `-StartServices`, `-SkipFrontendChecks`
4. call `run-full-sanity.ps1`
5. call `validate-full-sanity-report.ps1`
6. write `docs/evidence/ENV_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md`

Acceptance:

```powershell
.\scripts\env\run-guided-main-flow.ps1 -UseExistingServices
.\scripts\env\validate-guided-main-flow.ps1
```
