using Ontogony.Errors;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class CrossServiceErrorEnvelopeTests
{
    [Fact]
    public void FromApiError_maps_trace_and_details()
    {
        var apiError = new ApiError("kanon.unavailable", "Service unavailable", "trace-1", new { reason = "down" });
        var envelope = CrossServiceErrorEnvelopeExtensions.FromApiError(
            apiError,
            system: "kanon",
            stage: CrossServiceErrorStage.Planning,
            retryable: true);

        Assert.Equal("kanon.unavailable", envelope.Code);
        Assert.Equal("kanon", envelope.System);
        Assert.Equal(CrossServiceErrorStage.Planning, envelope.Stage);
        Assert.True(envelope.Retryable);
        Assert.Equal("trace-1", envelope.TraceId);
    }

    [Fact]
    public void TryParseJson_round_trips_required_fields()
    {
        var json = """
            {
              "code": "auth.missing",
              "message": "Missing bearer token.",
              "system": "allagma",
              "stage": "request",
              "traceId": "trace-abc",
              "retryable": false
            }
            """;

        Assert.True(CrossServiceErrorEnvelopeExtensions.TryParseJson(json, out var envelope));
        Assert.NotNull(envelope);
        Assert.Equal(CrossServiceErrorCodes.AuthMissing, envelope!.Code);
        Assert.Equal("allagma", envelope.System);
        Assert.False(envelope.Retryable);
    }

    [Fact]
    public void ToJson_includes_optional_fields()
    {
        var envelope = new CrossServiceErrorEnvelope(
            "downstream.failure",
            "Downstream call failed.",
            "allagma",
            Stage: CrossServiceErrorStage.Downstream,
            DownstreamSystem: "conexus",
            TraceId: "t-1",
            Retryable: true);

        var json = envelope.ToJson();
        Assert.Contains("\"downstreamSystem\":\"conexus\"", json.Replace(" ", string.Empty), StringComparison.Ordinal);
    }
}
