using System.Net;
using System.Text.Json;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Allagma;

[Trait("Category", "SystemE2E")]
public sealed class AllagmaRunLifecycleTests
{
    private readonly SystemTestFixture _fixture = new();

    [Fact]
    public async Task E2E_001_Governed_Run_Must_Complete_With_Kanon_And_Conexus_Evidence()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-001-governed-run");
        var payload = RunPayloadFactory.GovernedFirstLoop();

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: scenarioId);

        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, body);

        var runDoc = await JsonRunDocuments.TryParseAsync(response);
        runDoc.Should().NotBeNull();
        var runId = JsonRunDocuments.RequireRunId(runDoc!);
        var status = JsonRunDocuments.RequireStatus(runDoc!);
        status.Should().Be("Completed");

        var planningDecisionId = JsonRunDocuments.GetString(runDoc!.RootElement, "planningDecisionId");
        var modelCallId = JsonRunDocuments.GetString(runDoc.RootElement, "modelCallId");
        planningDecisionId.Should().NotBeNullOrWhiteSpace();
        modelCallId.Should().NotBeNullOrWhiteSpace();

        var eventsResponse = await http.GetAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/events",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        eventsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var eventsDoc = await JsonRunDocuments.TryParseAsync(eventsResponse);
        eventsDoc.Should().NotBeNull();
        var eventTypes = JsonRunDocuments.GetEventTypes(eventsDoc!);
        JsonRunDocuments.AssertOrderedSubsequence(eventTypes, AllagmaEventTypes.GovernedHappyPathOrdered);

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-001",
            scenarioId,
            "PASS",
            new Dictionary<string, object?>
            {
                ["runStatus"] = status,
                ["eventCount"] = eventTypes.Count,
                ["hasPlanningDecision"] = !string.IsNullOrWhiteSpace(planningDecisionId),
                ["hasModelCall"] = !string.IsNullOrWhiteSpace(modelCallId)
            },
            new Dictionary<string, object?>
            {
                ["runId"] = runId,
                ["planningDecisionId"] = planningDecisionId,
                ["modelCallId"] = modelCallId,
                ["traceId"] = JsonRunDocuments.GetString(runDoc.RootElement, "traceId"),
                ["correlationId"] = JsonRunDocuments.GetString(runDoc.RootElement, "correlationId")
            },
            response: JsonSerializer.Deserialize<object>(body),
            events: JsonSerializer.Deserialize<object>(await eventsResponse.Content.ReadAsStringAsync()),
            statusCode: (int)response.StatusCode,
            body: body);
    }
}
