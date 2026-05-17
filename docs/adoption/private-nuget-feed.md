# Private NuGet feed for Ontogony packages

For the full migration checklist (feed auth, pinning, CI), start at [consumer-package-migration.md](./consumer-package-migration.md).

For **package source mapping** (avoiding NU1507 with multiple feeds), see [NuGet source mapping](../governance/NUGET_SOURCE_MAPPING.md).

For CI and machines that do not clone `ontogony-platform` next to every consumer, publish packages to an **internal NuGet feed** (Azure Artifacts, GitHub Packages, self-hosted BaGet, etc.) and reference them by version.

**This repository:** version tags trigger publish to **GitHub Packages** from CI. See [Package publishing (GitHub Packages)](../planning/robustness/PACKAGE_PUBLISHING_GITHUB_PACKAGES.md) for the feed URL, permissions, and consumer setup.

**Platform repo restore:** root [`nuget.config`](../../nuget.config) clears user feeds and maps all packages to nuget.org for deterministic platform builds.

## Pack from this repository

From the repo root:

```powershell
$env:PACKAGE_VERSION = "0.3.0-alpha.1"   # or 0.3.0-local.1 for local feeds
./scripts/pack-all.ps1
```

Outputs under `artifacts/packages/`. The script uses `dotnet pack Ontogony.Platform.sln -c Release` with `PackageVersion` from **`PACKAGE_VERSION`**, which is **required** (there is no silent default). Optional switches: `-NoBuild`, `-IncludeSymbols`. The script fails if no `.nupkg` is produced.

## Consumer references

In consumer `.csproj` files, replace `ProjectReference` to `ontogony-platform` with:

```xml
<PackageReference Include="Ontogony.Observability" Version="0.3.0-alpha.1" />
```

Use the same version for all Ontogony packages you adopt in a given PR.

## Versioning policy

Until 1.0, treat **minor** bumps as potential breaking changes for infrastructure packages; read `CHANGELOG.md` and `docs/migrations/` before upgrading.

## Next step for the ecosystem

Pilot sibling references are acceptable for a short window. Before adding more consumers, standardize on **feed + versioned packages** so builds are reproducible without a fixed disk layout.
