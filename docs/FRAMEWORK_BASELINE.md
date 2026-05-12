# Framework and version baseline

This document is the **single place** that describes the .NET SDK, target framework, and **central NuGet version policy** for Ontogony.Platform and any **starter or consumer** (for example Conexus.NET) that is intended to track this repository.

Ontogony does **not** claim to track the latest public .NET or Microsoft.Extensions releases in prose here: versions are whatever is committed in the files below. Upgrade when your team decides, then update these sources of truth and run the full build and test suite.

## Current platform baseline

| Concern | Source in repo | Current value (see file for truth) |
| --- | --- | --- |
| **SDK** | [`global.json`](../global.json) | `9.0.100`, `rollForward: latestFeature` |
| **Target framework** | [`Directory.Build.props`](../Directory.Build.props) | `net9.0` for all projects in the solution |
| **NuGet / assembly default version** | [`Directory.Build.props`](../Directory.Build.props) | `0.3.0-alpha.1` (`<Version>`; CI may pass `-p:PackageVersion` / `PACKAGE_VERSION` to match) |
| **Central package versions** | [`Directory.Packages.props`](../Directory.Packages.props) | `ManagePackageVersionsCentrally` = `true`; version pins for `Microsoft.Extensions.*`, test packages, `Npgsql`, `JsonSchema.Net`, etc. |
| **Dependency policy & drift** | [`docs/planning/robustness/DEPENDENCY_BASELINE.md`](./planning/robustness/DEPENDENCY_BASELINE.md) | Human-readable pin table; CI enforces a single `Microsoft.Extensions.*` / `Microsoft.AspNetCore.TestHost` line via `scripts/validate-dependency-baseline.ps1`. |

There is **no** per-project floating version for shared dependencies: add or bump versions in `Directory.Packages.props` only.

## Supported SDK

- Developers and CI should use the **.NET 9 SDK** matching [`global.json`](../global.json).
- `rollForward: latestFeature` allows patch/feature roll-forward within the same major SDK line as defined by the .NET install on the machine; it does not change the target framework.

If `global.json` is updated (for example to a newer 9.x SDK), document the change in [`CHANGELOG.md`](../CHANGELOG.md).

## Target framework

- All library and test projects inherit `TargetFramework` **`net9.0`** from [`Directory.Build.props`](../Directory.Build.props).
- A future move to a newer LTS or STS line is a **repo-wide** decision: change `Directory.Build.props`, then align `global.json`, fix any API breaks, and refresh this document.

## Central package version file

- [`Directory.Packages.props`](../Directory.Packages.props) is the only catalog of package versions for centrally managed dependencies.
- Projects reference packages **without** a `Version` attribute on `PackageReference` / `PackageVersion` is set once in the props file.
- **Policy and consumer alignment:** [`docs/planning/robustness/DEPENDENCY_BASELINE.md`](./planning/robustness/DEPENDENCY_BASELINE.md) — single Microsoft.Extensions line, documented third-party drift, Conexus alignment notes. CI runs `scripts/validate-dependency-baseline.ps1`.

Third-party pins (example: `Npgsql`) live alongside Microsoft.Extensions pins in the same file.

## Upgrade procedure

1. **SDK:** Edit `global.json` (and install the SDK on dev machines and CI images).
2. **TFM:** If moving major .NET versions, edit `TargetFramework` in `Directory.Build.props`.
3. **Packages:** Bump `PackageVersion` entries in `Directory.Packages.props`; resolve transitive conflicts with `dotnet restore` and `dotnet list package --vulnerable` as needed.
4. **Validate:** `dotnet build Ontogony.Platform.sln`, `dotnet test Ontogony.Platform.sln` (includes [`Ontogony.PublicApi.Tests`](../tests/Ontogony.PublicApi.Tests) public API snapshots), and any CI validation scripts under `scripts/` that your pipeline runs.
5. **Record:** Add a [`CHANGELOG.md`](../CHANGELOG.md) entry; for breaking public API changes, add [`docs/migrations/`](./migrations/) when applicable.
6. **Shipping line:** When changing the pre-release or release tag for packed libraries, bump `<Version>` in `Directory.Build.props` and align CI / docs (`README.md`, `docs/packages/index.md`, `docs/FRAMEWORK_BASELINE.md`).

## Starter-template baseline

Any **starter zip** or **template consumer** that claims compatibility with this platform should:

- Target the **same** `TargetFramework` as `Directory.Build.props` (currently `net9.0`).
- Prefer the same **central package management** pattern (`Directory.Packages.props` or a documented subset) so Microsoft.Extensions versions stay aligned with Ontogony’s tested matrix; see [`docs/planning/robustness/DEPENDENCY_BASELINE.md`](./planning/robustness/DEPENDENCY_BASELINE.md) for the pinned baseline and drift rules.
- Mirror the compile smoke in [`examples/ConexusDotNetSkeleton/`](../../examples/ConexusDotNetSkeleton/) (included in `Ontogony.Platform.sln`, not packable).

Keeping SDK, TFM, and package versions as **few files as possible** avoids drift between the platform repo and the first consumer.
