# Package compatibility checklist — Ontogony `0.3.0-alpha.1`

Use this checklist before merging a Platform PR that changes shipped packages, before pushing a version tag, or when a downstream repo (Conexus, Kanon, Allagma) adopts a new platform build.

## A. Version and inventory

- [ ] `<Version>` in [`Directory.Build.props`](../../Directory.Build.props) matches intended tag (e.g. `0.3.0-alpha.1`).
- [ ] CI `PACKAGE_VERSION` / manifest generation use the same version.
- [ ] [`scripts/validate-shipping-inventory.ps1`](../../scripts/validate-shipping-inventory.ps1) passes (**23** shipping packages).
- [ ] [`scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1) passes (no illegal `ProjectReference` edges).

## B. Build and tests

- [ ] `dotnet restore Ontogony.Platform.sln`
- [ ] `dotnet build Ontogony.Platform.sln -c Release --no-restore`
- [ ] `dotnet test Ontogony.Platform.sln -c Release --no-build` (includes **public API snapshot** tests)
- [ ] [`scripts/validate-dependency-baseline.ps1`](../../scripts/validate-dependency-baseline.ps1) passes

## C. Public API governance

- [ ] If any `tests/Ontogony.PublicApi.Tests/*.verified.txt` changed:
  - [ ] Classified as **breaking** or **non-breaking**
  - [ ] [`CHANGELOG.md`](../../CHANGELOG.md) updated
  - [ ] [`docs/migrations/`](../migrations/) entry added when breaking or non-obvious
  - [ ] [`scripts/validate-public-api-governance.ps1`](../../scripts/validate-public-api-governance.ps1) passes
- [ ] Reviewers confirmed no accidental `public` on internal types

See [`PUBLIC_API_COMPATIBILITY.md`](../planning/robustness/PUBLIC_API_COMPATIBILITY.md) and [`public-api-review.md`](../public-api-review.md).

## D. Docs and boundaries

- [ ] [`scripts/validate-docs-links.ps1`](../../scripts/validate-docs-links.ps1) passes
- [ ] [`scripts/validate-docs-api-names.ps1`](../../scripts/validate-docs-api-names.ps1) passes
- [ ] [`scripts/validate-ai-runtime-boundaries.ps1`](../../scripts/validate-ai-runtime-boundaries.ps1) passes
- [ ] [`scripts/validate-ai-runtime-docs.ps1`](../../scripts/validate-ai-runtime-docs.ps1) passes
- [ ] No product semantics added (Kanon meaning, Allagma orchestration, Conexus routing)

## E. Consumer alignment (Platform repo)

- [ ] [`scripts/validate-conexus-consumer-baseline-alignment.ps1`](../../scripts/validate-conexus-consumer-baseline-alignment.ps1) passes
- [ ] [`scripts/validate-allagma-consumer-baseline-alignment.ps1`](../../scripts/validate-allagma-consumer-baseline-alignment.ps1) passes
- [ ] Pack smoke: `PACKAGE_VERSION=0.3.0-alpha.1` → [`scripts/pack-all.ps1`](../../scripts/pack-all.ps1) `-NoBuild` produces `.nupkg` under `artifacts/packages/`
- [ ] [`scripts/validate-nupkg-coordination-path-hygiene.ps1`](../../scripts/validate-nupkg-coordination-path-hygiene.ps1) passes
- [ ] `examples/ConexusDotNetPackageSmoke` restore + Release build against packed version

## F. Restore hygiene

- [ ] Repo [`nuget.config`](../../nuget.config) unchanged or intentionally updated — see [`NUGET_SOURCE_MAPPING.md`](./NUGET_SOURCE_MAPPING.md)
- [ ] Local restore does not require disabling central package management

## G. Downstream notification (when publishing a new alpha)

- [ ] Conexus: bump `OntogonyPackageVersion` / sibling HEAD; run Conexus CI (including package-mode job if applicable)
- [ ] Kanon: bump version or sibling HEAD; run Kanon tests (`UseOntogonyPackages=true` when validating packages)
- [ ] Allagma: update runtime lock and cohesion smoke when system baseline moves
- [ ] Record evidence in consumer repo or Phase 1 acceptance dashboard as required by the active system PR

## Quick command block (Platform repo root)

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release --no-restore
dotnet test Ontogony.Platform.sln -c Release --no-build
./scripts/validate-docs-links.ps1
./scripts/validate-docs-api-names.ps1
./scripts/validate-ai-runtime-boundaries.ps1
./scripts/validate-shipping-inventory.ps1
./scripts/validate-ai-runtime-docs.ps1
./scripts/validate-package-levels.ps1
./scripts/validate-dependency-baseline.ps1
./scripts/validate-conexus-consumer-baseline-alignment.ps1
./scripts/validate-allagma-consumer-baseline-alignment.ps1
./scripts/validate-public-api-governance.ps1
$env:PACKAGE_VERSION = "0.3.0-alpha.1"
./scripts/pack-all.ps1 -NoBuild
./scripts/validate-nupkg-coordination-path-hygiene.ps1
dotnet restore examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
dotnet build examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj --no-restore -c Release -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
```
