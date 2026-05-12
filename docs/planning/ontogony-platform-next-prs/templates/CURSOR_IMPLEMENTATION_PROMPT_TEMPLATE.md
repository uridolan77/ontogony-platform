# Cursor Implementation Prompt Template

You are working in `c:\dev\ontogony-platform`.

Implement PRXX: [title].

Read:

- `docs/00_START_HERE.md`
- `README.md`
- `CHANGELOG.md`
- relevant package docs under `docs/packages/`
- this PR spec: `[path]`

Rules:

- Share mechanics, not meaning.
- Do not move Athanor/Agentor/Conexus domain semantics into platform.
- Keep the PR bounded.
- Add tests and docs.
- Preserve package dependency hygiene.

Verification:

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
$env:PACKAGE_VERSION="0.2.0-local"
./scripts/pack-all.ps1 -NoBuild
```

Return:

- files changed
- tests run
- behavior changed
- migration notes
- follow-up PRs
