# Allagma ↔ Ontogony package-mode contract (PLAT-ALLAGMA-001)

This document is the **platform-side contract** for consuming Ontogony as **NuGet packages** from a future `allagma-dotnet` repository. Implementation will live in Allagma (`eng/Ontogony.References.props`, `Directory.Build.props`, CI workflow) once that repo exists.

## Required inputs

| Input | Purpose |
| --- | --- |
| **Package feed** | A NuGet v3 feed (or folder feed) that contains all `Ontogony.*` packages Allagma references at the chosen version. Examples: local folder after `dotnet pack`, or GitHub Packages for the Ontogony org. |
| **`OntogonyPackageVersion`** | SemVer of every `Ontogony.*` package line (central-managed in Allagma `Directory.Packages.props`). Override with `-p:OntogonyPackageVersion=…` or the `ONTOGONY_PACKAGE_VERSION` env var in CI. |
| **`UseOntogonyPackages=true`** | MSBuild switch: Allagma uses `PackageReference` to Ontogony instead of sibling `ProjectReference` under `../ontogony-platform`. |
| **nuget.org (or mirror)** | Allagma still restores Microsoft and third-party packages; package-mode restores must include a public (or internal mirror) feed alongside the Ontogony feed. |

## CI guard (recommended)

When **`UseOntogonyPackages=true`** and **`GITHUB_ACTIONS=true`**, Allagma should fail restore if a checkout exists at **`../ontogony-platform`** relative to the Allagma repo root (probe file: `…/ontogony-platform/src/Ontogony.Contracts/Ontogony.Contracts.csproj`). Package-mode jobs must check out Ontogony only under a **non-colliding** path (for example `upstream-ontogony-for-pack`) and pack from there.

Mirror the pattern documented for Conexus in [`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md).

## Local developer notes

- Default **sibling project** mode: clone `ontogony-platform` next to `allagma-dotnet` and build with `UseOntogonyPackages` unset/false.
- **Stale global package cache**: if `OntogonyPackageVersion` matches an older build in `%USERPROFILE%\.nuget\packages`, clear affected `ontogony.*` folders or bump the prerelease version used for local pack feeds.

## Non-goals

Ontogony does not embed Allagma product code, Microsoft Agent Framework, or governed-execution semantics. The platform provides only:

- consumer blueprints (this folder),
- [`examples/AllagmaDotNetSkeleton/`](../../examples/AllagmaDotNetSkeleton/) compile-only smoke,
- [`scripts/validate-allagma-consumer-baseline-alignment.ps1`](../../scripts/validate-allagma-consumer-baseline-alignment.ps1).

Full compatibility is proven in **Allagma.NET** CI once that repository exists.

## Proof status

| Item | Status |
| --- | --- |
| Platform readiness doc | [`allagma-dotnet-platform-readiness.md`](allagma-dotnet-platform-readiness.md) |
| In-repo compile smoke | [`examples/AllagmaDotNetSkeleton/`](../../examples/AllagmaDotNetSkeleton/) |
| Real Allagma package-mode CI job | **Pending** — record green workflow URL here when available |
