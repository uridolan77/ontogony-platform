# Conformance kits (`Ontogony.Testing`)

Reusable mechanical harnesses let product repos validate their own `IOutboxWriter`, `IIdempotencyLedger`, and `IArtifactStore` implementations against the same contracts Platform uses in CI.

## Harnesses

| Harness | Contracts | Reference implementation |
| --- | --- | --- |
| `OutboxConformanceHarness` | `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher` | `InMemoryOutboxStore` |
| `IdempotencyLedgerConformanceHarness` | `IIdempotencyLedger` | `InMemoryIdempotencyLedger` |
| `ArtifactStoreConformanceHarness` | `IArtifactStore` | `InMemoryArtifactStore` |
| `HttpResilienceConformanceHarness` | `ResilientIntegrationDelegatingHandler` | `StubHttpMessageHandler` |

## Example (outbox)

```csharp
var store = new MyEfOutboxStore(db);
var message = OutboxConformanceHarness.BuildMessage("msg-" + Guid.NewGuid().ToString("n"));
await OutboxConformanceHarness.AssertWriteThenReadAsync(store, store, message);
await OutboxConformanceHarness.AssertMarkDispatchedRemovesFromQueueAsync(store, store, store, message);
```

## Postgres outbox (Platform CI)

When `ONTOGONY_POSTGRES_TEST_CONNECTION` is set, `PostgresOutboxConformanceTests` runs the same harness against `PostgresOutboxStore`.

## Self-tests

`tests/Ontogony.Infrastructure.Tests/ConformanceKitPr33Tests.cs` exercises all kits against in-memory reference stores.

Filter:

```powershell
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter ConformanceKitPr33
```

## Reconstructability closure (PR-005)

Decision-event export safety, consumer adoption manifests, and cross-service error envelope facades live under `Ontogony.Testing.Conformance`. See [`reconstructability-conformance-kits.md`](reconstructability-conformance-kits.md).

## Boundaries

- Harnesses use generic test IDs (`ontogony://test/conformance`, `tenant-conformance`, etc.).
- No replay runtime or product semantics in Platform harness fixtures.
