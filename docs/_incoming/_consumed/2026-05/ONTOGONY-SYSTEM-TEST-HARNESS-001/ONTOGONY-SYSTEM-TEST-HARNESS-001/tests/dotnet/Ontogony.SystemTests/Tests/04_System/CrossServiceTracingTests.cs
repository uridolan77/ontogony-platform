using System.Net;
using System.Text.Json;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.System;

[Trait("Category", "SystemE2E")]
public sealed class CrossServiceTracingTests
{
    private readonly SystemTestFixture _fixture = new();

    [Fact]
    public async Task E2E_010_Correlation_And_Trace_Must_Appear_On_Governed_Run_Response()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-010-correlation-chain");
        var payload = RunPayloadFactory.GovernedFirstLoop("system-test-correlation", "corr-001");

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: scenarioId,
            traceId: scenarioId);

        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, body);

        var doc = await JsonRunDocuments.TryParseAsync(response);
        doc.Should().NotBeNull();
        var traceId = JsonRunDocuments.GetString(doc!.RootElement, "traceId");
        var correlationId = JsonRunDocuments.GetString(doc.RootElement, "correlationId");
        (traceId ?? correlationId).Should().NotBeNullOrWhiteSpace();

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-010",
            scenarioId,
            "PASS",
            new Dictionary<string, object?> { ["hasTraceOrCorrelation"] = true },
            new Dictionary<string, object?> { ["traceId"] = traceId, ["correlationId"] = correlationId, ["scenarioId"] = scenarioId },
            response: JsonSerializer.Deserialize<object>(body),
            statusCode: (int)response.StatusCode,
            body: body);
    }
}
