using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Readiness;

[Trait("Category", "Readiness")]
public sealed class ReadinessTests
{
    private readonly SystemTestFixture _fixture = new();

    public static TheoryData<string, string, string, bool> CriticalServices => new()
    {
        { "kanon", "KANON_BASE_URL", "http://localhost:5081", true },
        { "conexus", "CONEXUS_BASE_URL", "http://localhost:5082", true },
        { "allagma", "ALLAGMA_BASE_URL", "http://localhost:5083", true },
        { "metabole", "METABOLE_BASE_URL", "http://localhost:5084", false },
        { "aisthesis", "AISTHESIS_BASE_URL", "http://localhost:5085", false },
    };

    [Theory]
    [MemberData(nameof(CriticalServices))]
    public async Task Health_Must_Return_2xx(string service, string envKey, string defaultBaseUrl, bool critical)
    {
        var scenarioId = Correlation.NewScenarioId($"READINESS-{service}-health");
        var baseUrl = Environment.GetEnvironmentVariable(envKey) ?? defaultBaseUrl;
        using var http = new TestHttp();
        var response = await http.GetAsync(baseUrl, "/health", scenarioId);
        var body = await response.Content.ReadAsStringAsync();

        await _fixture.WriteE2eEvidenceAsync(
            $"READINESS-{service.ToUpperInvariant()}-HEALTH",
            scenarioId,
            response.IsSuccessStatusCode ? "PASS" : "FAIL",
            new Dictionary<string, object?> { ["endpoint"] = "/health", ["critical"] = critical },
            statusCode: (int)response.StatusCode,
            body: body,
            services: [service]);

        response.IsSuccessStatusCode.Should().BeTrue(
            because: $"{service} /health must be reachable at {baseUrl} before system E2E (critical={critical})");
    }

    [Theory]
    [InlineData("kanon", "KANON_BASE_URL", "http://localhost:5081")]
    [InlineData("conexus", "CONEXUS_BASE_URL", "http://localhost:5082")]
    [InlineData("allagma", "ALLAGMA_BASE_URL", "http://localhost:5083")]
    public async Task Ready_Must_Return_2xx_For_Critical_Services(string service, string envKey, string defaultBaseUrl)
    {
        var scenarioId = Correlation.NewScenarioId($"READINESS-{service}-ready");
        var baseUrl = Environment.GetEnvironmentVariable(envKey) ?? defaultBaseUrl;
        using var http = new TestHttp();
        var response = await http.GetAsync(baseUrl, "/ready", scenarioId);
        var body = await response.Content.ReadAsStringAsync();

        await _fixture.WriteE2eEvidenceAsync(
            $"READINESS-{service.ToUpperInvariant()}-READY",
            scenarioId,
            response.IsSuccessStatusCode ? "PASS" : "FAIL",
            new Dictionary<string, object?> { ["endpoint"] = "/ready", ["critical"] = true },
            statusCode: (int)response.StatusCode,
            body: body,
            services: [service]);

        response.IsSuccessStatusCode.Should().BeTrue(
            because: $"{service} /ready must report dependencies healthy at {baseUrl}");
    }
}
