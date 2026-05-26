using System.Text.Json;
using Ontogony.Errors;
using Ontogony.Idempotency;
using Ontogony.Testing.Conformance;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>Self-tests for PR-005 reconstructability conformance kits.</summary>
public sealed class ConformanceKitPr005Tests
{
    [Fact]
    public async Task CorrelationKit_RunsStandardConsumerChecks()
    {
        await CorrelationConformanceKit.RunStandardConsumerChecksAsync();
    }

    [Fact]
    public async Task ErrorEnvelopeKit_RunsMiddlewareBaseline()
    {
        await ErrorEnvelopeConformanceKit.RunMiddlewareBaselineAsync();
    }

    [Fact]
    public void CrossServiceEnvelope_ValidShape_Passes()
    {
        var envelope = new CrossServiceErrorEnvelope(
            Code: "ontogony.conformance.sample",
            Message: "sample",
            System: "platform");
        CrossServiceErrorEnvelopeConformanceAssertions.AssertValidShape(envelope, "platform");
    }

    [Fact]
    public async Task IdempotencyKit_InMemoryLedger_Passes()
    {
        await IdempotencyConformanceKit.RunStandardLedgerChecksAsync(new InMemoryIdempotencyLedger());
    }

    [Fact]
    public void ObservabilityNaming_MeterPrefixAndPrometheusName()
    {
        ObservabilityNamingConformanceAssertions.AssertMeterNameUsesPrefix("Conexus.Gateway", "Conexus.");
        ObservabilityNamingConformanceAssertions.AssertPrometheusExportName(
            "conexus.streaming.started",
            "conexus_streaming_started_total");
    }

    [Fact]
    public void ReconstructabilityExport_RedactionGate_FailsOnSecretPattern()
    {
        Assert.Throws<InvalidOperationException>(() =>
            ReconstructabilityExportConformanceAssertions.AssertExportJsonDoesNotLeakSecrets(
                """{"note":"sk-proj-abc"}"""));
    }

    [Fact]
    public void ReconstructabilityExport_FragmentRefs_MustResolve()
    {
        var json = """
            {
              "schemaVersion": "ontogony-test-v1",
              "decisionEventId": "evt-1",
              "decisionKind": "test.kind",
              "severity": "low",
              "serviceOfOrigin": "platform",
              "evidenceFragments": [
                { "fragmentId": "frag-a" }
              ],
              "inputs": { "fragmentRefs": [ "frag-missing" ] }
            }
            """;
        using var doc = JsonDocument.Parse(json);
        Assert.Throws<InvalidOperationException>(() =>
            ReconstructabilityExportConformanceAssertions.AssertDecisionEventExportShape(doc.RootElement));
    }
}
