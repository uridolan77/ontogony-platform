using System.Text.Json;
using Ontogony.Errors;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>SYS-TIGHT-006 — operator failure taxonomy adapter and matrix gates.</summary>
public sealed class OperatorFailureTaxonomyAdapterTests
{
    private static readonly string RepoRoot = GetProjectRoot();

    [Fact]
    public void SYS_TIGHT_006_Contract_artifacts_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/operator-failure-taxonomy.matrix.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/schemas/operator-failure-taxonomy.matrix.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-system-operator-failure-taxonomy.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs")));
    }

    [Fact]
    public void SYS_TIGHT_006_Matrix_representative_mappings_match_adapter()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/operator-failure-taxonomy.matrix.json")));

        var root = doc.RootElement;
        Assert.Equal("ontogony-operator-failure-taxonomy-v1", root.GetProperty("schema").GetString());
        Assert.Equal("SYSTEM-ALPHA-006", root.GetProperty("baseline").GetString());

        var requiredKinds = root.GetProperty("requiredTaxonomyKinds")
            .EnumerateArray()
            .Select(element => element.GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var producedKinds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var row in root.GetProperty("representativeMappings").EnumerateArray())
        {
            var expected = row.GetProperty("expectedTaxonomy").GetString();
            var envelopeElement = row.GetProperty("envelope");
            var envelope = new CrossServiceErrorEnvelope(
                envelopeElement.GetProperty("code").GetString()!,
                envelopeElement.GetProperty("message").GetString()!,
                envelopeElement.GetProperty("system").GetString()!,
                envelopeElement.TryGetProperty("stage", out var stage) ? stage.GetString() : null,
                envelopeElement.TryGetProperty("downstreamSystem", out var downstream)
                    ? downstream.GetString()
                    : null,
                envelopeElement.TryGetProperty("traceId", out var trace) ? trace.GetString() : null,
                envelopeElement.TryGetProperty("correlationId", out var correlation)
                    ? correlation.GetString()
                    : null,
                envelopeElement.TryGetProperty("retryable", out var retryable) && retryable.ValueKind != JsonValueKind.Null
                    ? retryable.GetBoolean()
                    : null);

            var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
            Assert.Equal(expected, view.Taxonomy);
            Assert.False(string.IsNullOrWhiteSpace(view.Title));
            Assert.NotEmpty(view.RecommendedActions ?? Array.Empty<string>());
            producedKinds.Add(view.Taxonomy);
        }

        Assert.Subset(requiredKinds, producedKinds);
    }

    [Theory]
    [InlineData("insufficient_quota", true, "model", "conexus", OperatorFailureTaxonomyKind.QuotaExceeded)]
    [InlineData("idempotency.conflict", false, "request", null, OperatorFailureTaxonomyKind.IdempotencyConflict)]
    [InlineData("conflict", false, "request", null, OperatorFailureTaxonomyKind.Conflict)]
    [InlineData("auth.missing", false, "model", "conexus", OperatorFailureTaxonomyKind.AuthFailed)]
    [InlineData("timeout", true, "downstream", "conexus", OperatorFailureTaxonomyKind.Timeout)]
    [InlineData("random.unmapped.code", false, null, null, OperatorFailureTaxonomyKind.Unknown)]
    public void ResolveTaxonomy_precedence_and_unknown_cases(
        string code,
        bool retryable,
        string? stage,
        string? downstreamSystem,
        string expectedTaxonomy)
    {
        var envelope = new CrossServiceErrorEnvelope(
            code,
            "message",
            "allagma",
            stage,
            downstreamSystem,
            Retryable: retryable);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
        Assert.Equal(expectedTaxonomy, view.Taxonomy);
    }

    [Fact]
    public void Quota_signal_beats_generic_provider_failure_classification()
    {
        var envelope = new CrossServiceErrorEnvelope(
            "insufficient_quota",
            "Quota exceeded.",
            "allagma",
            CrossServiceErrorStage.Model,
            "conexus",
            Retryable: true);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
        Assert.Equal(OperatorFailureTaxonomyKind.QuotaExceeded, view.Taxonomy);
    }

    [Fact]
    public void Idempotency_signal_beats_generic_conflict_classification()
    {
        var envelope = new CrossServiceErrorEnvelope(
            CrossServiceErrorCodes.IdempotencyConflict,
            "Duplicate idempotency key.",
            "allagma",
            CrossServiceErrorStage.Idempotency,
            Retryable: false);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
        Assert.Equal(OperatorFailureTaxonomyKind.IdempotencyConflict, view.Taxonomy);
    }

    [Fact]
    public void Auth_signal_beats_downstream_provider_classification()
    {
        var envelope = new CrossServiceErrorEnvelope(
            CrossServiceErrorCodes.AuthMissing,
            "Missing credentials.",
            "allagma",
            CrossServiceErrorStage.Model,
            "conexus",
            Retryable: true);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
        Assert.Equal(OperatorFailureTaxonomyKind.AuthFailed, view.Taxonomy);
    }

    [Fact]
    public void Timeout_signal_beats_retryable_downstream_unavailable_classification()
    {
        var envelope = new CrossServiceErrorEnvelope(
            CrossServiceErrorCodes.Timeout,
            "Timed out waiting for provider.",
            "allagma",
            CrossServiceErrorStage.Downstream,
            "conexus",
            Retryable: true);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
        Assert.Equal(OperatorFailureTaxonomyKind.Timeout, view.Taxonomy);
    }

    [Fact]
    public void FromCrossServiceEnvelope_preserves_trace_and_correlation()
    {
        var envelope = new CrossServiceErrorEnvelope(
            CrossServiceErrorCodes.Timeout,
            "Timed out",
            "allagma",
            CrossServiceErrorStage.Downstream,
            TraceId: "trace-1",
            CorrelationId: "corr-1",
            Retryable: true);

        var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);

        Assert.Equal(OperatorFailureTaxonomyKind.Timeout, view.Taxonomy);
        Assert.Equal("trace-1", view.TraceId);
        Assert.Equal("corr-1", view.CorrelationId);
        Assert.True(view.Retryable);
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not find Ontogony.Platform.sln from test base directory.");
    }
}
