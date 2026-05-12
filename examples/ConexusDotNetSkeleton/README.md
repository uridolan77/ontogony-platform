# ConexusDotNetSkeleton

**Not a product.** Minimal web host with **direct** `ProjectReference`s to every **required** package in the [Conexus.NET readiness blueprint](../../docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) (Hosting, Observability, Errors, Http, Security, Idempotency, Hashing, Contracts, AI.Contracts, Artifacts, Execution) so CI proves that slice compiles together without relying on transitive references alone.

## Build

```powershell
dotnet build examples/ConexusDotNetSkeleton/ConexusDotNetSkeleton.csproj
```

The project is included in `Ontogony.Platform.sln` for that compile check only (`IsPackable` is false).
