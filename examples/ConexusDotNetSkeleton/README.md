# ConexusDotNetSkeleton

**Not a product.** Minimal web host with **direct** `ProjectReference`s to every **required** package in the [Conexus.NET readiness blueprint](../../docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) (Hosting, Observability, Errors, Http, Security, Idempotency, Hashing, Contracts, AI.Contracts, Artifacts, Execution) so CI proves that slice compiles together without relying on transitive references alone.

## Build

```powershell
dotnet build examples/ConexusDotNetSkeleton/ConexusDotNetSkeleton.csproj
```

The project is included in `Ontogony.Platform.sln` for that compile check only (`IsPackable` is false).

## Package consumer smoke (PR-PLAT-002)

[`../ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj`](../ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj) is **not** in the main solution. It links this `Program.cs` but uses `PackageReference` to Ontogony packages resolved from `artifacts/packages/` (see [`../ConexusDotNetPackageSmoke/nuget.config`](../ConexusDotNetPackageSmoke/nuget.config)). CI runs `pack` first, then `dotnet restore` / `dotnet build` on that project so releases are checked against **packed** assemblies, not project references.

The smoke project lives as a **sibling** under `examples/` so the SDK default compile glob for this skeleton does not pick up another project's `obj/**/*.cs` (which would duplicate assembly attributes).

After a local pack (`PACKAGE_VERSION` must match `Directory.Build.props` in `examples/ConexusDotNetPackageSmoke/`):

```powershell
$env:PACKAGE_VERSION = "0.3.0-alpha.1"
./scripts/pack-all.ps1 -NoBuild
dotnet restore examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
dotnet build examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj --no-restore -c Release -p:OntogonyPackageVersion=$env:PACKAGE_VERSION
```
