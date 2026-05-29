using System.Net;
using System.Text.Json;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.System;

[Trait("Category", "SystemE2E")]
public sealed class ConexusFallbackE2ETests
{
    private readonly SystemTestFixture _fixture = new();
    private const string FallbackAlias = "fake-fast";

    [Fact]
    public async Task E2E_005_Conexus_Fallback_Must_Be_Observable_On_Deterministic_Fake_Providers()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-005-conexus-fallback");
        using var http = new TestHttp();

        var upsert = await http.PutJsonAsync(
            _fixture.Options.ConexusBaseUrl,
            $"/admin/v0/aliases/{FallbackAlias}",
            RunPayloadFactory.ConexusFallbackAliasUpsert(FallbackAlias),
            scenarioId,
            extraHeaders: new Dictionary<string, string>
            {
                [OntogonyHeaders.ConexusAdminKey] = _fixture.Options.ConexusAdminApiKey
            });
        upsert.StatusCode.Should().Be(HttpStatusCode.OK, await upsert.Content.ReadAsStringAsync());

        var chat = await http.PostJsonAsync(
            _fixture.Options.ConexusBaseUrl,
            "/v1/chat/completions",
            RunPayloadFactory.ConexusChatCompletion(FallbackAlias, "system harness fallback probe"),
            scenarioId,
            apiKey: _fixture.Options.ConexusProjectApiKey);

        var chatBody = await chat.Content.ReadAsStringAsync();
        chat.StatusCode.Should().Be(HttpStatusCode.OK, chatBody);
        chatBody.Should().Contain("[fallback-route]", because: "fake.fail.retryable must fall through to fake.echo.fallback");

        var chatDoc = await JsonRunDocuments.TryParseAsync(chat);
        var modelCallId = JsonRunDocuments.GetString(chatDoc!.RootElement, "id");
        modelCallId.Should().NotBeNullOrWhiteSpace();

        var governed = await http.PostJsonAsync(
            _fixture.Options.AllagmaBaseUrl,
            "/allagma/v0/runs",
            RunPayloadFactory.GovernedFirstLoop("system-test-fallback-stack", "fallback-001"),
            scenarioId,
            bearer: _fixture.Options.AllagmaServiceToken);
        governed.StatusCode.Should().Be(HttpStatusCode.OK);

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-005",
            scenarioId,
            "PASS",
            new Dictionary<string, object?>
            {
                ["fallbackMarkerObserved"] = true,
                ["alias"] = FallbackAlias,
                ["allagmaStillCompletes"] = true
            },
            new Dictionary<string, object?> { ["modelCallId"] = modelCallId },
            response: new { chat = JsonSerializer.Deserialize<object>(chatBody) },
            services: ["conexus", "allagma"],
            statusCode: (int)chat.StatusCode,
            body: chatBody);
    }
}
