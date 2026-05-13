# PLAT-NP-013 Validation Checklist

## Docs

- [ ] README names Conexus.NET as active consumer.
- [ ] README no longer says no consumers exist.
- [ ] Versioning mentions public API snapshots.
- [ ] Versioning mentions CHANGELOG.
- [ ] Versioning mentions migration notes.
- [ ] Versioning mentions Conexus package-mode validation.
- [ ] Version compatibility matrix is linked.
- [ ] Public API review doc is linked.
- [ ] Package-mode contract is linked.
- [ ] Release evidence is linked.
- [ ] Security evidence is linked.

## Boundary

- [ ] No Conexus routing/provider/pricing semantics.
- [ ] No new packages.
- [ ] No cloud resolver package.
- [ ] No durable-store expansion.

## Validation

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore -c Release
dotnet test Ontogony.Platform.sln --no-build -c Release
./scripts/validate-docs-links.ps1
./scripts/validate-public-api-governance.ps1
./scripts/validate-conexus-consumer-baseline-alignment.ps1
```
