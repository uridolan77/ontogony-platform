# PR Acceptance Checklist

Before a PR is considered complete:

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
$env:PACKAGE_VERSION="0.2.0-local"
./scripts/pack-all.ps1 -NoBuild
```

Review questions:

1. Is the PR small enough to review properly?
2. Does the PR introduce any new public API? If yes, is it documented?
3. Does the PR create a new package? If yes, is it in the solution and docs map?
4. Does the PR change wire behavior? If yes, is there a migration note?
5. Does the PR change package dependencies? If yes, is the dependency direction justified?
6. Does the PR add a reference implementation? If yes, is it clearly marked reference/test/single-process unless production-grade?
7. Does the PR make future Agentor/Athanor adoption easier without moving their semantics?
