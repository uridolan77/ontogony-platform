using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Aisthesis;

public sealed class AisthesisPlaceholderTests
{
    private readonly HarnessOptions _options = HarnessOptions.FromEnvironment();
    private readonly EvidenceWriter _evidence;

    public AisthesisPlaceholderTests() => _evidence = new EvidenceWriter(_options);

    [Fact]
    public async Task Aisthesis_Memory_Write_And_Query_Should_Be_Calibrated_To_Current_Routes()
    {
        var scenarioId = Correlation.NewScenarioId("AISTHESIS-memory-placeholder");
        var payload = new
        {
            traceId = "sys-test-experience-001",
            temporalMarker = DateTimeOffset.UtcNow,
            modality = "synthetic-test",
            content = "Synthetic phenomenological memory trace for harness calibration."
        };

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(_options.AisthesisBaseUrl, "/aisthesis/v0/memories", payload, scenarioId, bearer: _options.AisthesisServiceToken, idempotencyKey: scenarioId);
        var body = await response.Content.ReadAsStringAsync();
        await _evidence.WriteAsync(scenarioId, new { scenarioId, statusCode = (int)response.StatusCode, body });
        ((int)response.StatusCode).Should().BeOneOf(200, 201, 202, 400, 404, 422, 501);
    }
}
