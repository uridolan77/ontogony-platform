# PLAT-DEPTH-002 — Idempotency / outbox / artifact conformance harnesses

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Scope

Reusable mechanical conformance kits for consumer implementations (in-memory or EF). No replay runtime or product identifiers.

## Harnesses

| Harness | Contract | Reference store |
| --- | --- | --- |
| `OutboxConformanceHarness` | `IOutboxWriter` / `IOutboxReader` / `IOutboxDispatcher` | `InMemoryOutboxStore` |
| `IdempotencyLedgerConformanceHarness` | `IIdempotencyLedger` | `InMemoryIdempotencyLedger` |
| `ArtifactStoreConformanceHarness` | `IArtifactStore` | `InMemoryArtifactStore` |

## Tests

`tests/Ontogony.Infrastructure.Tests/ConformanceKitPr33Tests.cs` — Outbox, Idempotency, and Artifact sections.

## Harness coverage (extended)

| Harness method | Proves |
| --- | --- |
| `OutboxConformanceHarness.AssertAvailableAtDeferralAsync` | Future `AvailableAt` not visible until due |
| `ArtifactStoreConformanceHarness.AssertStreamPutRoundTripAsync` | Stream put + hash |
| `ArtifactStoreConformanceHarness.AssertExpectedContentHashRejectsMismatchAsync` | Hash guard on stream put |
| `ArtifactStoreConformanceHarness.AssertTenantScopeSeparationAsync` | Dedupe scope by tenant |

Postgres: `PostgresOutboxConformanceTests` when `ONTOGONY_POSTGRES_TEST_CONNECTION` is set.

Docs: [`docs/adoption/conformance-kits.md`](../adoption/conformance-kits.md)

## Validation

```powershell
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter "ConformanceKitPr33"
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter "PostgresOutboxConformance"
```
