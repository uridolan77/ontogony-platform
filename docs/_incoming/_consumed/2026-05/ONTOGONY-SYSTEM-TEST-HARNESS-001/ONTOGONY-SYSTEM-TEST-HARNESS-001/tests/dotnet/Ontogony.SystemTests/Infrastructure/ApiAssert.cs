using System.Net;
using FluentAssertions;

namespace Ontogony.SystemTests.Infrastructure;

public static class ApiAssert
{
    public static async Task ShouldBeSuccessWithEvidence(this HttpResponseMessage response, string service, string scenarioId, EvidenceWriter evidence)
    {
        var body = await response.Content.ReadAsStringAsync();
        await evidence.WriteAsync(scenarioId, new
        {
            scenarioId,
            service,
            statusCode = (int)response.StatusCode,
            headers = response.Headers.ToDictionary(h => h.Key, h => h.Value),
            body
        });
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted, HttpStatusCode.NoContent);
    }

    public static async Task ShouldBeUnauthorizedOrForbidden(this HttpResponseMessage response, string service, string scenarioId, EvidenceWriter evidence)
    {
        var body = await response.Content.ReadAsStringAsync();
        await evidence.WriteAsync(scenarioId, new { scenarioId, service, statusCode = (int)response.StatusCode, body });
        ((int)response.StatusCode).Should().BeOneOf(401, 403);
    }

    public static async Task ShouldBeClientError(this HttpResponseMessage response, string service, string scenarioId, EvidenceWriter evidence)
    {
        var body = await response.Content.ReadAsStringAsync();
        await evidence.WriteAsync(scenarioId, new { scenarioId, service, statusCode = (int)response.StatusCode, body });
        ((int)response.StatusCode).Should().BeInRange(400, 499);
    }
}
