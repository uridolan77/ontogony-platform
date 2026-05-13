# Dependency baseline alignment

This document is the **human-readable companion** to the machine-checked pins in [`Directory.Packages.props`](../../../Directory.Packages.props) and the SDK pin in [`global.json`](../../../global.json). CI runs [`scripts/validate-dependency-baseline.ps1`](../../../scripts/validate-dependency-baseline.ps1) so the **Microsoft.Extensions / ASP.NET test host line** cannot drift within [`Directory.Packages.props`](../../../Directory.Packages.props) without an intentional multi-package bump.

For the overall upgrade workflow, see [`docs/FRAMEWORK_BASELINE.md`](../../FRAMEWORK_BASELINE.md).

## Policy

1. **Central management:** All centrally managed NuGet versions for this solution live in `Directory.Packages.props`. Library and test projects use `PackageReference` **without** per-project `Version` (except documented smoke projects such as [`examples/ConexusDotNetPackageSmoke`](../../../examples/ConexusDotNetPackageSmoke), which pins Ontogony package versions by design).
2. **Single Microsoft.Extensions line:** Every `Microsoft.Extensions.*` package and `Microsoft.AspNetCore.TestHost` **must use the same** `Version` value. Bump them together in one change (restores stay coherent; behavior matches the shared ASP.NET Core 9 runtime).
3. **Third-party pins:** `Npgsql`, `JsonSchema.Net`, test-only packages (`xunit`, `coverlet`, `Microsoft.NET.Test.Sdk`), and snapshot tooling (`PublicApiGenerator`, `Verify.Xunit`) are **not** required to match the Microsoft.Extensions line; they follow their own upgrade cadence. When you bump them, run the full build and test suite and record notable changes in [`CHANGELOG.md`](../../../CHANGELOG.md).
4. **Consumers (e.g. Conexus.NET):** Prefer the same **central package management** pattern and align **Microsoft.Extensions.*** / ASP.NET-adjacent package versions with this baseline when referencing Ontogony libraries, so DI and hosting extensions resolve a single graph. See [`docs/consumer-blueprints/conexus-dotnet-platform-readiness.md`](../../consumer-blueprints/conexus-dotnet-platform-readiness.md).

## Current pins (mirror of repo files)

Values below are maintained to match the repo; **`Directory.Packages.props` and `global.json` are authoritative** if this table lags.

| Package id | Version |
| --- | --- |
| **SDK** (`global.json`) | `9.0.100` (`rollForward: latestFeature`) |
| Npgsql | 8.0.4 |
| Microsoft.AspNetCore.TestHost | 9.0.0 |
| Microsoft.Extensions.Configuration.Abstractions | 9.0.0 |
| Microsoft.Extensions.DependencyInjection | 9.0.0 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 9.0.0 |
| Microsoft.Extensions.Hosting | 9.0.0 |
| Microsoft.Extensions.Hosting.Abstractions | 9.0.0 |
| Microsoft.Extensions.Logging.Abstractions | 9.0.0 |
| Microsoft.Extensions.Http | 9.0.0 |
| Microsoft.Extensions.Options | 9.0.0 |
| Microsoft.Extensions.Options.ConfigurationExtensions | 9.0.0 |
| Microsoft.Extensions.Options.DataAnnotations | 9.0.0 |
| Microsoft.NET.Test.Sdk | 17.11.1 |
| xunit | 2.9.2 |
| xunit.runner.visualstudio | 2.8.2 |
| coverlet.collector | 6.0.2 |
| JsonSchema.Net | 7.1.0 |
| PublicApiGenerator | 11.1.0 |
| Verify.Xunit | 26.0.1 |

## Documented drift / exceptions

| Area | Notes |
| --- | --- |
| **Microsoft vs third-party** | `Microsoft.Extensions.*` / `Microsoft.AspNetCore.TestHost` share one line; `Npgsql`, `JsonSchema.Net`, and test/snapshot packages may differ by design. |
| **Implicit ASP.NET runtime** | Web projects and tests target `net9.0` and use the **shared framework** Microsoft.AspNetCore.App from the installed SDK; that runtime is **not** listed in [`Directory.Packages.props`](../../../Directory.Packages.props). |
| **Outdated packages** | The repo does **not** fail CI on `dotnet list package --outdated`; upgrades are deliberate. Periodically run outdated/vulnerable lists in a branch when planning bumps. |

## Related

- [`docs/FRAMEWORK_BASELINE.md`](../../FRAMEWORK_BASELINE.md)
- [PR-PLAT-005 spec](./pr-specs/PR-PLAT-005-dependency-baseline-alignment.md)
