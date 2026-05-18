# Troubleshooting — local operator sanity

## Allagma eval POST returns 403

Ensure Development host and manual write flag:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:Allagma__Evaluation__ManualWriteEnabled = "true"
```

Restart Allagma after changing env vars. Error code: `allagma.evaluation_manual_write_disabled`.

## Conexus route evidence missing

1. Confirm **fake** provider mode (Development default).
2. Align project API key:

```text
cx-dev-key-change-me
```

Set on Allagma (`Conexus__ProjectApiKey`) and Conexus (`CONEXUS_DEV_PROJECT_API_KEY`).

3. Re-run subject/baseline flows and inspect route-decision records in Allagma run detail.

## Frontend dashboard empty

Expected when no recent evaluated runs exist. Use fixture dashboard:

```text
/allagma/evaluations?dashboardFixture=ci-suite
```

Verify `VITE_*_BASE_URL` in `.env.local` match `02_EXACT_SETTINGS.md`.

## Health check fails / connection refused

1. Confirm process is listening on the expected port.
2. Check env vars were set in the **same** terminal that launched `dotnet run`.
3. See port-in-use section below.

## Port already in use

```powershell
netstat -ano | findstr :5081
netstat -ano | findstr :5082
netstat -ano | findstr :5083
```

Stop the conflicting process or switch to Allagma `alternate` profile (`5181`–`5183`) and update all base URLs consistently (`02_EXACT_SETTINGS.md`, frontend `.env.local`).

## Kanon topology authorization empty on subject run

Subject path (`centralized_orchestrator` + topology override) **must** produce a non-empty `topologyAuthorizationDecisionId`. If empty:

- Confirm Kanon is reachable from Allagma (`Kanon__BaseUrl`)
- Inspect Kanon decision records for the run trace id
- See `docs/alignment/eval-full-sanity-alignment/07_KNOWN_LIMITATIONS.md` for manual E2E notes

## Baseline shows null topology authorization

**Expected** for `single_workflow` / low-risk baseline. Documented in main flow (`03_MAIN_USE_FLOW.md`).

## full-sanity script fails with stack down

`run-full-sanity.ps1` requires a running stack. Start APIs first (`04_STARTUP_MODES.md`) or use documented skip flags only for doc-only verification (see script help in `allagma-dotnet`).

## Wrong repo or missing sibling

Re-run workspace verification in `01_WORKSPACE_LAYOUT.md`. All six repos must exist before ENV-SETUP-002 scripts.

## Escalation

| Symptom | See also |
| --- | --- |
| Eval contract drift | `docs/alignment/eval-full-sanity-alignment/` |
| OpenAPI / adapter failures | `ontogony-frontend` — `npm run openapi:check` |
| Platform port matrix | `allagma-dotnet/docs/system/LOCAL_RUNTIME_PROFILES.md` |
| Planning PR specs | `../local-operator-sanity-package/04_PR_SPECS/` |
