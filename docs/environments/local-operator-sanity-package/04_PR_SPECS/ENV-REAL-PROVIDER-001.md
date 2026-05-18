# ENV-REAL-PROVIDER-001 — Optional real provider mode

Repo: `conexus-dotnet`

Goal: optional real provider path after fake-provider environment passes.

Hard rules:

- provider key backend-only
- no key in frontend env
- no key in browser storage
- no key in screenshots/reports
- no key in appsettings JSON
- prefer env secret locator

Env:

```powershell
$env:CONEXUS_PROVIDER_OPENAI_API_KEY="sk-..."
```

Non-goal: real external tool execution.
