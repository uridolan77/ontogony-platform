using System.Net;
using System.Text.Json;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Allagma;

[Trait("Category", "SystemE2E")]
public sealed class AllagmaIdempotencyTests
{
    private readonly SystemTestFixture _fixture = new();

    [Fact]
    public async Task E2E_002_Start_Run_Must_Replay_Same_Run_For_Same_Idempotency_Key()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-002-idempotent-run");
        var idempotencyKey = $"{scenarioId}-replay";
        var payload = RunPayloadFactory.IdempotentRun("system-test-idem", "idem-001", "replay");

        using var http = new TestHttp();
        var first = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: idempotencyKey);
        var second = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            payload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: idempotencyKey);

        first.StatusCode.Should().Be(HttpStatusCode.OK);
        second.StatusCode.Should().Be(HttpStatusCode.OK);

        var firstDoc = await JsonRunDocuments.TryParseAsync(first);
        var secondDoc = await JsonRunDocuments.TryParseAsync(second);
        firstDoc.Should().NotBeNull();
        secondDoc.Should().NotBeNull();

        var firstRunId = JsonRunDocuments.RequireRunId(firstDoc!);
        var secondRunId = JsonRunDocuments.RequireRunId(secondDoc!);
        secondRunId.Should().Be(firstRunId);

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-002",
            scenarioId,
            "PASS",
            new Dictionary<string, object?> { ["sameRunId"] = true, ["idempotencyKey"] = idempotencyKey },
            new Dictionary<string, object?> { ["runId"] = firstRunId },
            response: new { firstRunId, secondRunId },
            statusCode: (int)second.StatusCode);
    }

    [Fact]
    public async Task E2E_002b_Same_Idempotency_Key_With_Different_Payload_Must_Conflict()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-002b-idempotency-conflict");
        var idempotencyKey = $"{scenarioId}-conflict";
        var firstPayload = RunPayloadFactory.IdempotentRun("system-test-idem", "conflict-001", "first");
        var secondPayload = RunPayloadFactory.IdempotentRun("system-test-idem", "conflict-001", "second-different");

        using var http = new TestHttp();
        var first = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            firstPayload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: idempotencyKey);
        var second = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            secondPayload,
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken,
            idempotencyKey: idempotencyKey);

        first.StatusCode.Should().Be(HttpStatusCode.OK);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var conflictDoc = await JsonRunDocuments.TryParseAsync(second);
        conflictDoc.Should().NotBeNull();
        JsonRunDocuments.GetString(conflictDoc!.RootElement, "code")
            .Should().Be("allagma.idempotency.conflict");

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-002b",
            scenarioId,
            "PASS",
            new Dictionary<string, object?>
            {
                ["conflictCode"] = JsonRunDocuments.GetString(conflictDoc.RootElement, "code"),
                ["idempotencyKey"] = idempotencyKey
            },
            statusCode: (int)second.StatusCode,
            body: await second.Content.ReadAsStringAsync());
    }
}
