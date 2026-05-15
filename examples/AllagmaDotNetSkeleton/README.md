# AllagmaDotNetSkeleton

**Not a product.** Minimal web host with **direct** `ProjectReference`s to every **required** package in the [Allagma.NET readiness blueprint](../../docs/consumer-blueprints/allagma-dotnet-platform-readiness.md) so CI proves that slice compiles together without relying on transitive references alone.

Wires tracing → logging scope → exception handling; touches integration metrics, actor context, idempotency, execution journal, persistence contracts, and `Ontogony.Testing.Architecture` compile smoke.

## Build

```powershell
dotnet build examples/AllagmaDotNetSkeleton/AllagmaDotNetSkeleton.csproj
```

The project is included in `Ontogony.Platform.sln` for that compile check only (`IsPackable` is false).

## Boundaries

This skeleton must **not** reference Kanon, Conexus, or Allagma product assemblies, Microsoft Agent Framework, or LLM provider SDKs. Governed-execution semantics belong in `allagma-dotnet` (future).
