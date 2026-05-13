# PR-PLAT-NP-001 — Release workflow parity + first tag publish proof

This document records **what was implemented in-repo** and the **evidence checklist** to complete after the first proof tag publish. See also `docs/planning/next-phase/architecture/PACKAGE_RELEASE_EVIDENCE.md`.

## Status

| Split | Scope | State |
| --- | --- | --- |
| **PLAT-NP-001A** | Release workflow parity: Conexus baseline gate in `release-packages.yml`, manifest script portability, pre-pack cleanup of `artifacts/packages`, evidence doc skeleton | **Done** (in `main` after merge) |
| **PLAT-NP-001B** | First real tag-triggered publish: run URL, GitHub Packages proof, `PACKAGE_MANIFEST.json` vs Release attachments, filled table below | **Done** — table filled for `v0.3.0-alpha.1` (2026-05-13) |

**PLAT-NP-001** (release parity + first tag proof) is **closed** with **001B** evidence below.

## Implemented (workflow parity — 001A)

- **`release-packages.yml`** — added step **Validate Conexus consumer baseline (readiness vs Directory.Build.targets)** running `./scripts/validate-conexus-consumer-baseline-alignment.ps1`, in the same order as `ci.yml` (after dependency baseline, before strict CHANGELOG validation, pack, manifest, Conexus package smoke). **Clear package output** removes `artifacts/packages` before pack so manifest version validation cannot see stale `.nupkg` files (self-documenting on self-hosted or reused workspaces too).
- **`scripts/generate-package-manifest.ps1`** — portable default commit hash on Windows PowerShell 5.1; ASCII-only success lines (avoids parser issues with some hosts).

## Suggested first proof tag

Use the next SemVer prerelease you intend to ship, for example **`v0.3.0-alpha.1`**, after `main` contains this PR and the release workflow is green on a manual dry run.

## Post-tag evidence (`v0.3.0-alpha.1`)

| Item | Value |
|------|--------|
| Tag | `v0.3.0-alpha.1` |
| Package version | `0.3.0-alpha.1` |
| Workflow run URL | https://github.com/uridolan77/ontogony-platform/actions/runs/25777410721 |
| GitHub Release URL | https://github.com/uridolan77/ontogony-platform/releases/tag/v0.3.0-alpha.1 |
| GitHub Packages feed | NuGet v3: `https://nuget.pkg.github.com/uridolan77/index.json` (authenticate with a GitHub PAT that has `read:packages`; packages are under the `Ontogony.*` id prefix at version **0.3.0-alpha.1**) |
| `PACKAGE_MANIFEST.json` | Same run artifact **`manifest`** or Release asset: https://github.com/uridolan77/ontogony-platform/releases/download/v0.3.0-alpha.1/PACKAGE_MANIFEST.json — **23** shipping `.nupkg` rows, `commit` **8819d24f470f5c06771f5d76b5ffad819e28b758**, `generated` **2026-05-13T04:01:32.3615123Z** |
| Manifest vs artifacts | **Match:** every non-symbol `.nupkg` on the Release is listed in `PACKAGE_MANIFEST.json` with the same filename and SHA-256 for that run (23 packages); `.snupkg` files are attached but not manifest rows. |
| Conexus package smoke | **Green** on the same workflow run — step **Conexus package consumer (restore + build from local .nupkg)** in run **25777410721** |

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
