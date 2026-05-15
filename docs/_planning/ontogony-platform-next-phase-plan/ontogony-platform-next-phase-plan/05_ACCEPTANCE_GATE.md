# Platform Acceptance Gate

Before merging any platform next-phase PR:

## Boundary

- [ ] Adds mechanics only.
- [ ] Adds no product meaning.
- [ ] Adds no provider SDKs.
- [ ] Adds no Kanon/Agentor/Conexus implementation dependencies.

## Verification

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
```

## Required docs

- [ ] Package docs updated.
- [ ] CHANGELOG updated.
- [ ] Public API snapshot updated if public API changes.
- [ ] Migration guidance added if breaking.
- [ ] Conexus consumer impact considered.
