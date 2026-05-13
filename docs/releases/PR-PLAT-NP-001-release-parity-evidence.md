# PR-PLAT-NP-001 — Release workflow parity + first tag publish proof

This document records **what was implemented in-repo** and the **evidence checklist** to complete after the first proof tag publish. See also `docs/planning/next-phase/architecture/PACKAGE_RELEASE_EVIDENCE.md`.

## Implemented (this PR)

- **`release-packages.yml`** — added step **Validate Conexus consumer baseline (readiness vs Directory.Build.targets)** running `./scripts/validate-conexus-consumer-baseline-alignment.ps1`, in the same order as `ci.yml` (after dependency baseline, before strict CHANGELOG validation, pack, manifest, Conexus package smoke).
- **`scripts/generate-package-manifest.ps1`** — portable default commit hash on Windows PowerShell 5.1; ASCII-only success lines (avoids parser issues with some hosts).

## Suggested first proof tag

Use the next SemVer prerelease you intend to ship, for example **`v0.3.0-alpha.1`**, after `main` contains this PR and the release workflow is green on a manual dry run.

## Post-tag evidence (maintainer: fill after publish)

| Item | Value |
|------|--------|
| Tag | _e.g. `v0.3.0-alpha.1`_ |
| Package version | _same without `v` prefix_ |
| Workflow run URL | _Actions → `release-packages` for the tag push_ |
| GitHub Release URL | _Releases entry created by `softprops/action-gh-release`_ |
| GitHub Packages feed | _NuGet feed listing or `dotnet nuget list` against `https://nuget.pkg.github.com/<owner>/index.json`_ |
| `PACKAGE_MANIFEST.json` | _Download from workflow artifact **`manifest`** or **`packages`**; must list one row per shipping `.nupkg`_ |
| Manifest vs artifacts | _Confirm every `.nupkg` attached to the Release appears in `PACKAGE_MANIFEST.json` with matching SHA-256_ |
| Conexus package smoke | _Release job step “Conexus package consumer” must be green (restore + build `examples/ConexusDotNetPackageSmoke` against local feed path / packed packages)_ |

## Workflow behavior (acceptance)

- **`workflow_dispatch`** — still builds, tests, validates, packs, uploads artifacts; **does not** push to GitHub Packages or create a Release (`if: startsWith(github.ref, 'refs/tags/')` on publish/release steps).
- **Tag push** — publishes non-symbol `.nupkg` with hard failure on push errors; `.snupkg` push remains best-effort (warnings only).

## Local parity dry run (optional)

From repo root, after a Release build:

```powershell
$env:PACKAGE_VERSION = '0.3.0-alpha.1'   # example; match your tag
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore -c Release
dotnet test Ontogony.Platform.sln --no-build -c Release
./scripts/validate-docs-links.ps1
./scripts/validate-docs-api-names.ps1
./scripts/validate-ai-runtime-boundaries.ps1
./scripts/validate-shipping-inventory.ps1
./scripts/validate-ai-runtime-docs.ps1
./scripts/validate-package-levels.ps1
./scripts/validate-dependency-baseline.ps1
./scripts/validate-conexus-consumer-baseline-alignment.ps1
./scripts/validate-changelog.ps1 -PackageVersion $env:PACKAGE_VERSION -Strict
./scripts/pack-all.ps1 -NoBuild
./scripts/generate-package-manifest.ps1 -PackageVersion $env:PACKAGE_VERSION
dotnet restore examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
dotnet build examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj --no-restore -c Release -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
```

Clear `artifacts/packages` before `pack-all` if older `.nupkg` versions are present, so manifest version validation does not fail.

SHA-256 values are **build-specific**; treat the generated `PACKAGE_MANIFEST.json` from the **same** workflow run as the authoritative hash list for published bits.
