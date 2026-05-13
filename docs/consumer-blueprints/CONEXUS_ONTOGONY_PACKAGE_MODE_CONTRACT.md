# Conexus ↔ Ontogony package-mode contract (PLAT-NP-002)

This document is the **platform-side contract** for consuming Ontogony as **NuGet packages** from the real `conexus-dotnet` repository. Implementation lives in Conexus (`eng/Ontogony.References.props`, `Directory.Build.props`, `.github/workflows/ci.yml`).

## Required inputs

| Input | Purpose |
| --- | --- |
| **Package feed** | A NuGet v3 feed (or folder feed) that contains all `Ontogony.*` packages Conexus references at the chosen version. Examples: local folder after `dotnet pack`, or GitHub Packages for the Ontogony org. |
| **`OntogonyPackageVersion`** | SemVer of every `Ontogony.*` package line (central-managed in Conexus `Directory.Packages.props`). Override with `-p:OntogonyPackageVersion=…` or the `ONTOGONY_PACKAGE_VERSION` env var in CI. |
| **`UseOntogonyPackages=true`** | MSBuild switch: Conexus uses `PackageReference` to Ontogony instead of sibling `ProjectReference` under `../ontogony-platform`. |
| **nuget.org (or mirror)** | Conexus still restores Microsoft and third-party packages; package-mode restores must include a public (or internal mirror) feed alongside the Ontogony feed. |

## CI guard (GitHub Actions)

When **`UseOntogonyPackages=true`** and **`GITHUB_ACTIONS=true`**, Conexus fails the restore if a checkout exists at **`../ontogony-platform`** relative to the Conexus repo root (probe file: `…/ontogony-platform/src/Ontogony.Contracts/Ontogony.Contracts.csproj`). Package-mode jobs must therefore check out Ontogony only under a **non-colliding** path (for example `upstream-ontogony-for-pack`) and pack from there.

## Local developer notes

- Default **sibling project** mode remains: clone `ontogony-platform` next to `conexus-dotnet` and build with `UseOntogonyPackages` unset/false.
- **Stale global package cache**: if `OntogonyPackageVersion` matches an older build in `%USERPROFILE%\.nuget\packages`, you can see missing types after platform API changes. Clear the affected `ontogony.*` cache folders or bump the prerelease version used for local pack feeds.

## Non-goals

Ontogony does not embed Conexus product code or gateway semantics. The platform keeps only the existing `examples/ConexusDotNetPackageSmoke` smoke; full compatibility is proven in **Conexus CI**.

## PLAT-NP-002 — Green proof (real Conexus repo)

- **Workflow run:** https://github.com/uridolan77/conexus-dotnet/actions/runs/25776886946 (overall workflow may be red if unrelated jobs fail; the **`conexus-ontogony-package-mode`** job is the acceptance signal.)
- **Job (success):** https://github.com/uridolan77/conexus-dotnet/actions/runs/25776886946/job/75711313588 — packs Ontogony to a non-sibling path, restores/builds/tests Conexus with `UseOntogonyPackages=true` and explicit `OntogonyPackageVersion`.
