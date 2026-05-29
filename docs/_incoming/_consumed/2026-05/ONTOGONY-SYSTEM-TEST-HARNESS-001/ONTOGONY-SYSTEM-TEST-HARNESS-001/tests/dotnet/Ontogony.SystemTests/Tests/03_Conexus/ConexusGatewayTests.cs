using System.Net;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Conexus;

[Trait("Category", "SystemE2E")]
public sealed class ConexusGatewayTests
{
    private readonly SystemTestFixture _fixture = new();

    [Fact]
    public async Task CONEXUS_Chat_Completions_Must_Accept_Fake_Alias_Without_External_Providers()
    {
        var scenarioId = Correlation.NewScenarioId("CONEXUS-chat-completion");
        var payload = RunPayloadFactory.ConexusChatCompletion(
            "risk-summary-v0",
            "Return a short synthetic risk summary for system testing.");

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.ConexusBaseUrl,
            "/v1/chat/completions",
            payload,
            scenarioId,
            apiKey: _fixture.Options.ConexusProjectApiKey,
            idempotencyKey: scenarioId);

        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, body);

        var doc = await JsonRunDocuments.TryParseAsync(response);
        doc.Should().NotBeNull();
        JsonRunDocuments.GetString(doc!.RootElement, "id").Should().NotBeNullOrWhiteSpace();

        await _fixture.WriteE2eEvidenceAsync(
            "CONEXUS-CHAT-001",
            scenarioId,
            "PASS",
            new Dictionary<string, object?> { ["model"] = "risk-summary-v0", ["stream"] = false },
            statusCode: (int)response.StatusCode,
            body: body,
            services: ["conexus"]);
    }

    [Fact]
    public async Task CONEXUS_Streaming_With_Idempotency_Key_Must_Be_Rejected()
    {
        var scenarioId = Correlation.NewScenarioId("CONEXUS-streaming-idempotency");
        var payload = new
        {
            model = "risk-summary-v0",
            messages = new[] { new { role = "user", content = "stream test" } },
            stream = true
        };

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.ConexusBaseUrl,
            "/v1/chat/completions",
            payload,
            scenarioId,
            apiKey: _fixture.Options.ConexusProjectApiKey,
            idempotencyKey: scenarioId);

        var body = await response.Content.ReadAsStringAsync();
        ((int)response.StatusCode).Should().BeInRange(400, 499, body);

        await _fixture.WriteE2eEvidenceAsync(
            "CONEXUS-STREAM-IDEM-001",
            scenarioId,
            "PASS",
            new Dictionary<string, object?> { ["streamingIdempotencyRejected"] = true },
            statusCode: (int)response.StatusCode,
            body: body,
            services: ["conexus"]);
    }
}
