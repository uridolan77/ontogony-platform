using System.Net;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Allagma;

[Trait("Category", "SystemE2E")]
public sealed class AllagmaHumanGateTests
{
    private readonly SystemTestFixture _fixture = new();
    private const string GateActorId = "system-test-human-gate-agent";

    [Fact]
    public async Task E2E_003a_Human_Gate_Approve_Must_Resume_To_Completed_Without_Duplicate_Model_Calls()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-003a-human-gate-approve");
        var traceId = $"gate-approve-{Guid.NewGuid():N}";
        var payload = RunPayloadFactory.HumanGateRun(GateActorId, "888", traceId);

        using var http = new TestHttp();
        var start = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            traceId: traceId);

        var startBody = await start.Content.ReadAsStringAsync();
        start.StatusCode.Should().Be(HttpStatusCode.OK, startBody);
        var startDoc = await JsonRunDocuments.TryParseAsync(start);
        startDoc.Should().NotBeNull();
        JsonRunDocuments.RequireStatus(startDoc!).Should().Be("WaitingForHumanGate");
        var runId = JsonRunDocuments.RequireRunId(startDoc!);

        var eventsPaused = await http.GetAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/events",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        var pausedDoc = await JsonRunDocuments.TryParseAsync(eventsPaused);
        pausedDoc.Should().NotBeNull();
        var pausedTypes = JsonRunDocuments.GetEventTypes(pausedDoc!);
        pausedTypes.Should().Contain(AllagmaEventTypes.RunHumanGatePaused);
        pausedTypes.Should().NotContain(AllagmaEventTypes.ConexusModelCompleted);

        var humanGateId = JsonRunDocuments.GetHumanGateIdFromEvents(pausedDoc!);
        var resolvePath = $"/ontology/v0/actions/human-gates/{Uri.EscapeDataString(humanGateId)}/resolve";
        var resolve = await http.PostJsonAsync(
            _fixture.Options.KanonBaseUrl,
            resolvePath,
            RunPayloadFactory.KanonResolveHumanGate("system-test-human-resolver", "approve"),
            scenarioId,
            bearer: _fixture.Options.KanonServiceToken,
            traceId: traceId,
            extraHeaders: new Dictionary<string, string>
            {
                [OntogonyHeaders.LegacyActorRoles] = "PaymentsOperator"
            });
        resolve.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent);

        var resume1 = await http.PostEmptyAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/resume",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        resume1.StatusCode.Should().Be(HttpStatusCode.OK);
        var resume1Doc = await JsonRunDocuments.TryParseAsync(resume1);
        resume1Doc.Should().NotBeNull();
        JsonRunDocuments.GetString(resume1Doc!.RootElement, "outcome").Should().Be("resumed");
        JsonRunDocuments.GetString(resume1Doc.RootElement, "runStatus").Should().Be("Completed");

        var resume2 = await http.PostEmptyAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/resume",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        resume2.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonRunDocuments.GetString((await JsonRunDocuments.TryParseAsync(resume2))!.RootElement, "outcome")
            .Should().Be("resumed");

        var eventsFinal = await http.GetAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/events",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        var finalDoc = await JsonRunDocuments.TryParseAsync(eventsFinal);
        finalDoc.Should().NotBeNull();
        var finalTypes = JsonRunDocuments.GetEventTypes(finalDoc!);
        finalTypes.Count(t => t == AllagmaEventTypes.RunHumanGateResumed).Should().Be(1);
        JsonRunDocuments.AssertOrderedSubsequence(finalTypes, AllagmaEventTypes.GovernedHappyPathOrdered);

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-003a",
            scenarioId,
            "PASS",
            new Dictionary<string, object?>
            {
                ["humanGateId"] = humanGateId,
                ["terminalEvent"] = AllagmaEventTypes.RunHumanGateResumed,
                ["finalStatus"] = "Completed"
            },
            new Dictionary<string, object?> { ["runId"] = runId, ["traceId"] = traceId },
            events: finalTypes,
            services: ["allagma", "kanon"]);
    }

    [Fact]
    public async Task E2E_003b_Human_Gate_Deny_Must_Not_Invoke_Conexus_Model()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-003b-human-gate-deny");
        var traceId = $"gate-deny-{Guid.NewGuid():N}";
        var payload = RunPayloadFactory.HumanGateRun(GateActorId, "999", traceId);

        using var http = new TestHttp();
        var start = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            traceId: traceId);

        start.StatusCode.Should().Be(HttpStatusCode.OK);
        var startDoc = await JsonRunDocuments.TryParseAsync(start);
        startDoc.Should().NotBeNull();
        JsonRunDocuments.RequireStatus(startDoc!).Should().Be("WaitingForHumanGate");
        var runId = JsonRunDocuments.RequireRunId(startDoc!);

        var pausedDoc = await JsonRunDocuments.TryParseAsync(await http.GetAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/events",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken));
        var humanGateId = JsonRunDocuments.GetHumanGateIdFromEvents(pausedDoc!);

        var resolvePath = $"/ontology/v0/actions/human-gates/{Uri.EscapeDataString(humanGateId)}/resolve";
        var resolve = await http.PostJsonAsync(
            _fixture.Options.KanonBaseUrl,
            resolvePath,
            RunPayloadFactory.KanonResolveHumanGate("system-test-human-resolver", "reject"),
            scenarioId,
            bearer: _fixture.Options.KanonServiceToken,
            traceId: traceId,
            extraHeaders: new Dictionary<string, string> { [OntogonyHeaders.LegacyActorRoles] = "PaymentsOperator" });
        resolve.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent);

        var resume = await http.PostEmptyAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/resume",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        resume.StatusCode.Should().Be(HttpStatusCode.OK);
        var resumeDoc = await JsonRunDocuments.TryParseAsync(resume);
        JsonRunDocuments.GetString(resumeDoc!.RootElement, "outcome").Should().Be("denied");
        JsonRunDocuments.GetString(resumeDoc.RootElement, "runStatus").Should().Be("Denied");

        var finalDoc = await JsonRunDocuments.TryParseAsync(await http.GetAsync(
            _fixture.Options.AllagmaBaseUrl,
            $"/allagma/v0/runs/{runId}/events",
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken));
        var finalTypes = JsonRunDocuments.GetEventTypes(finalDoc!);
        finalTypes.Count(t => t == AllagmaEventTypes.RunHumanGateDenied).Should().Be(1);
        finalTypes.Should().NotContain(AllagmaEventTypes.ConexusModelRequested);
        finalTypes.Should().NotContain(AllagmaEventTypes.ConexusModelCompleted);

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-003b",
            scenarioId,
            "PASS",
            new Dictionary<string, object?> { ["outcome"] = "denied", ["noConexusModel"] = true },
            new Dictionary<string, object?> { ["runId"] = runId, ["humanGateId"] = humanGateId },
            events: finalTypes,
            services: ["allagma", "kanon"]);
    }
}
