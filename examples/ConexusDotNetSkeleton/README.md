# ConexusDotNetSkeleton

**Not a product.** Minimal web host that references the same Ontogony packages as the [Conexus.NET readiness blueprint](../../docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) so CI can prove they compile together.

## Build

```powershell
dotnet build examples/ConexusDotNetSkeleton/ConexusDotNetSkeleton.csproj
```

The project is included in `Ontogony.Platform.sln` for that compile check only (`IsPackable` is false).
