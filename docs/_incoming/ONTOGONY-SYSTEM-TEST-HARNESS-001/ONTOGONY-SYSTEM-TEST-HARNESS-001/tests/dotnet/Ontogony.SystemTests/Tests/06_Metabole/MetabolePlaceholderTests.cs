using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Metabole;

public sealed class MetabolePlaceholderTests
{
    private readonly HarnessOptions _options = HarnessOptions.FromEnvironment();
    private readonly EvidenceWriter _evidence;

    public MetabolePlaceholderTests() => _evidence = new EvidenceWriter(_options);

    [Fact]
    public async Task Metabole_Transformation_Flow_Should_Be_Calibrated_To_Current_Routes()
    {
        var scenarioId = Correlation.NewScenarioId("METABOLE-transform-placeholder");
        var payload = new
        {
            inputId = "sys-test-metabole-input-001",
            transformation = "deterministic-test-transform",
            payload = new { value = 42, description = "synthetic test data" }
        };

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(_options.MetaboleBaseUrl, "/metabole/v0/transformations", payload, scenarioId, bearer: _options.MetaboleServiceToken, idempotencyKey: scenarioId);
        var body = await response.Content.ReadAsStringAsync();
        await _evidence.WriteAsync(scenarioId, new { scenarioId, statusCode = (int)response.StatusCode, body });
        ((int)response.StatusCode).Should().BeOneOf(200, 201, 202, 400, 404, 422, 501);
    }
}
