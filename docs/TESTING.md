# Testing

## Local commands

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release
dotnet test Ontogony.Platform.sln -c Release --no-build
```

Public API snapshot tests (when snapshots change intentionally):

```powershell
dotnet test Ontogony.Platform.sln -c Release --filter "FullyQualifiedName~Public_api_matches_snapshot"
```

System compatibility gate (requires sibling repos on disk):

```powershell
pwsh ./scripts/run-system-compatibility-gate.ps1
```

Filter by test category:

```powershell
dotnet test Ontogony.Platform.sln -c Release --filter "Category=SystemCompatGate"
```

---

## CI (`.github/workflows/ci.yml`)

Path-filtered jobs reduce cost on docs-only PRs. Full CI runs on `main` push, `workflow_dispatch`, or PR label `run-full-ci`.

| Gate | When | Script / action |
| --- | --- | --- |
| Docs links | Docs changed | `validate-docs-links.ps1` |
| Docs API names | Docs changed | `validate-docs-api-names.ps1` |
| AI runtime docs | Docs changed | `validate-ai-runtime-docs.ps1` |
| Error envelope | Docs / full | `validate-cross-service-error-envelope.ps1` |
| Protocol registry | Docs / full | `validate-system-protocol-registry.ps1` |
| Post-lock delta register | Docs / full | `validate-post-lock-delta-register.ps1` |
| Operator v1 lock | Docs / full | `validate-operator-v1-lock.ps1` |
| Real tools block | Docs / full | `validate-real-tools-block.ps1` |
| Build + test | Code / full | `dotnet build` / `dotnet test` |
| Shipping inventory | Full | `validate-shipping-inventory.ps1` (27 packages) |
| Package levels | Full | `validate-package-levels.ps1` |
| AI runtime boundaries | Full | `validate-ai-runtime-boundaries.ps1` |
| Conexus consumer alignment | Full | `validate-conexus-consumer-baseline-alignment.ps1` |
| Pack | Full | NuGet pack of shipping projects |

Coverage: Cobertura/TRX uploaded; HTML report via ReportGenerator. Numeric thresholds are **advisory** until baselines stabilize — see [`quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](./quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md).

---

## Conformance kits (`Ontogony.Testing`)

Static assertion helpers for consumers to prove correct adoption. Each throws `InvalidOperationException` on failure (framework-agnostic).

| Class | Proves |
| --- | --- |
| `TracingConformanceAssertions` | `RequestTracingMiddleware` echoes canonical trace headers |
| `ErrorShapeConformanceAssertions` | `OntogonyExceptionHandlingMiddleware` maps exceptions; no internal leak |
| `EnvelopeConformanceAssertions` | `OntogonyEnvelope` source/type conventions and payload hash |
| `HmacConformanceAssertions` | `ServiceIdentityHmacSignatureHelper` round-trips |
| `OutboxConformanceHarness` | Outbox writer/reader/dispatcher contract |
| `HttpResilienceConformanceHarness` | Retry and circuit-breaker mechanics |
| `HeaderPropagationConformanceAssertions` | Frozen propagation header set (PLATFORM-9-003) |

Consumer repos run parallel tests (e.g. `AllagmaOutboundPropagationConformanceTests`, `ConexusOutboundPropagationConformanceTests`).

---

## Evidence

Platform gate verification records: [`evidence/README.md`](./evidence/README.md).
