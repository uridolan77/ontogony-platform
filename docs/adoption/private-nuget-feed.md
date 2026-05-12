# Private NuGet feed for Ontogony packages

For the full migration checklist (feed auth, pinning, CI), start at [consumer-package-migration.md](./consumer-package-migration.md).

For CI and machines that do not clone `ontogony-platform` next to every consumer, publish packages to an **internal NuGet feed** (Azure Artifacts, GitHub Packages, self-hosted BaGet, etc.) and reference them by version.

## Pack from this repository

From the repo root:

```powershell
$env:PACKAGE_VERSION = "0.2.0-local.1"
./scripts/pack-all.ps1
```

Outputs under `artifacts/packages/`. The script uses `dotnet pack Ontogony.Platform.sln -c Release` with `PackageVersion` from `PACKAGE_VERSION` (default `0.1.0-starter` if unset). Optional switches: `-NoBuild`, `-IncludeSymbols`. The script fails if no `.nupkg` is produced.

## Consumer references

In consumer `.csproj` files, replace `ProjectReference` to `ontogony-platform` with:

```xml
<PackageReference Include="Ontogony.Observability" Version="0.2.0-local.1" />
```

Use the same version for all Ontogony packages you adopt in a given PR.

## Versioning policy

Until 1.0, treat **minor** bumps as potential breaking changes for infrastructure packages; read `CHANGELOG.md` and `docs/migrations/` before upgrading.

## Next step for the ecosystem

Pilot sibling references are acceptable for a short window. Before adding more consumers, standardize on **feed + versioned packages** so builds are reproducible without a fixed disk layout.
