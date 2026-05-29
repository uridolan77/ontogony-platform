using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.System;

public sealed class ErrorContractTests
{
    private readonly HarnessOptions _options = HarnessOptions.FromEnvironment();
    private readonly EvidenceWriter _evidence;

    public ErrorContractTests() => _evidence = new EvidenceWriter(_options);

    [Theory]
    [InlineData("allagma", "ALLAGMA_BASE_URL", "http://localhost:5083", "/allagma/v0/runs")]
    [InlineData("kanon", "KANON_BASE_URL", "http://localhost:5081", "/ontology/v0/semantic-query-plans/compile")]
    [InlineData("conexus", "CONEXUS_BASE_URL", "http://localhost:5082", "/v1/chat/completions")]
    public async Task Invalid_Payload_Should_Return_Stable_Client_Error(string service, string envKey, string defaultBaseUrl, string path)
    {
        var scenarioId = Correlation.NewScenarioId($"ERROR-{service}-invalid-payload");
        var baseUrl = Environment.GetEnvironmentVariable(envKey) ?? defaultBaseUrl;
        using var http = new TestHttp();
        var response = await http.PostJsonAsync(baseUrl, path, new { definitelyInvalid = true }, scenarioId);
        var body = await response.Content.ReadAsStringAsync();
        await _evidence.WriteAsync(scenarioId, new { scenarioId, service, statusCode = (int)response.StatusCode, body });
        ((int)response.StatusCode).Should().BeInRange(400, 499);
        body.Should().NotBeNull();
    }
}
