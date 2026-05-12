# Cursor Prompt — PR26 Ontogony.Hosting ServiceDefaults

You are working in `c:\dev\ontogony-platform`.

Read this PR spec first:

```text
pr-specs/PR26_Ontogony.Hosting_ServiceDefaults.md
```

Core rule:

```text
Share mechanics. Do not share meaning.
```

Before coding:

1. Inspect the existing package layout and dependency graph.
2. Confirm the change belongs in `ontogony-platform` and does not leak Athanor/Agentor/Conexus semantics.
3. Keep the PR bounded. Do not implement later PRs.
4. Add or update tests first where practical.
5. Update docs, changelog, and migration notes.

Required verification:

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
$env:PACKAGE_VERSION="0.2.0-local"
./scripts/pack-all.ps1 -NoBuild
```

When done, provide:

- changed files summary
- tests run and results
- boundary check: what this PR deliberately did not do
- follow-up recommendations
