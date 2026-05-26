# Reconstructability conformance kits (`Ontogony.Testing.Conformance`)

Consumer-proven mechanical contracts for cross-service reconstructability closure (PR-005). These kits wrap existing `Ontogony.Testing` assertions; they do not add product semantics.

## Kits

| Kit / assertions | Purpose | Delegates to |
| --- | --- | --- |
| `CorrelationConformanceKit` | Frozen header contract + tracing middleware | `HeaderPropagationConformanceAssertions`, `TracingConformanceAssertions` |
| `ErrorEnvelopeConformanceKit` | Middleware error JSON + optional native envelope | `ErrorShapeConformanceAssertions`, `CrossServiceErrorEnvelopeConformanceAssertions` |
| `IdempotencyConformanceKit` | Ledger exclusive begin + lifecycle | `IdempotencyLedgerConformanceHarness` |
| `ObservabilityNamingConformanceAssertions` | Meter prefix + Prometheus export names | (local) |
| `ReconstructabilityConformanceKit` | Decision-event export redaction + shape | `ReconstructabilityExportConformanceAssertions` |
| `CompatibilityManifestConformanceAssertions` | Generated adoption manifest presence | (local) |

See also: [`conformance-kits.md`](conformance-kits.md) for outbox, artifact store, and HTTP resilience harnesses.

## Consumer adoption (minimum)

Each backend repo should include a focused test class (for example `AllagmaPlatformConformanceTests`) that proves:

1. **Correlation** — `CorrelationConformanceKit.RunStandardConsumerChecksAsync()` plus service-specific outbound propagation tests where applicable.
2. **Error envelope** — `ErrorEnvelopeConformanceKit.RunMiddlewareBaselineAsync()` and, when the service emits `CrossServiceErrorEnvelope` natively, `AssertCrossServiceEnvelope(..., expectedSystem)`.
3. **Idempotency** (Allagma, Conexus) — `IdempotencyConformanceKit.RunStandardLedgerChecksAsync(serviceLedger)`.
4. **Observability naming** (where gateway/runtime metrics exist) — meter prefix + frozen instrument map from the service contract type.
5. **Reconstructability export** — `ReconstructabilityConformanceKit.AssertDecisionEventFixture(...)` on at least one frozen JSON fixture per service.
6. **Compatibility manifest** (Kanon, Conexus) — `CompatibilityManifestConformanceAssertions.AssertManifestPresent(...)` on `docs/generated/*_COMPATIBILITY_MANIFEST.json`.

## Example

```csharp
[Fact]
public async Task Platform_correlation_and_error_baseline()
{
    await CorrelationConformanceKit.RunStandardConsumerChecksAsync();
    await ErrorEnvelopeConformanceKit.RunMiddlewareBaselineAsync();
}

[Fact]
public async Task Platform_idempotency_ledger()
{
    await IdempotencyConformanceKit.RunStandardLedgerChecksAsync(new MyInMemoryIdempotencyLedger());
}

[Fact]
public void Reconstructability_fixture_is_export_safe()
{
    var json = File.ReadAllText("Fixtures/reconstructability/sample-event.json");
    ReconstructabilityConformanceKit.AssertDecisionEventFixture(json);
}
```

## Run (Platform CI)

```powershell
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter ConformanceKitPr005
```

## Boundaries

- Kits validate mechanics only (headers, error shape, ledger behavior, redaction patterns, manifest fields).
- Kanon classification rules and gateway routing semantics stay in product repos.
- Do not embed live prompts, completions, or API keys in fixtures.
