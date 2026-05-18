# ENV-SETUP-002 — Stack start/check scripts and exact local settings

Repo: `allagma-dotnet`

Add:

```text
scripts/env/start-local-operator-sanity.ps1
scripts/env/check-local-operator-sanity.ps1
scripts/env/stop-local-operator-sanity.ps1
scripts/env/write-local-operator-sanity-env-template.ps1
```

`start-local-operator-sanity.ps1` should:

- verify all six repos exist
- start Kanon on 5081
- start Conexus on 5082
- start Allagma on 5083 with required env vars
- optionally start frontend
- write managed process IDs to `artifacts/env/local-operator-sanity/processes.json`
- use `$processId`, not `$pid`

Validation:

```powershell
.\scripts\env\check-local-operator-sanity.ps1 -DevRoot C:\dev
.\scripts\run-full-sanity.ps1 -SkipAutomatedGates -SkipFrontendChecks
.\scripts\validate-full-sanity-report.ps1
```

Evidence:

```text
docs/evidence/ENV_SETUP_002_STACK_SCRIPTS_EVIDENCE.md
```
