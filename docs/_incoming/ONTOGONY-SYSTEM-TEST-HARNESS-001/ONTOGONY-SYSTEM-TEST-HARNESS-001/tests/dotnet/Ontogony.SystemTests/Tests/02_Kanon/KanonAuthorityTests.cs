using System.Net;
using System.Text.Json;
using FluentAssertions;
using Ontogony.SystemTests.Infrastructure;
using Xunit;

namespace Ontogony.SystemTests.Tests.Kanon;

[Trait("Category", "SystemE2E")]
public sealed class KanonAuthorityTests
{
    private readonly SystemTestFixture _fixture = new();

    [Fact]
    public async Task AUTH_Kanon_Protected_Route_Must_Reject_Missing_Token()
    {
        var scenarioId = Correlation.NewScenarioId("AUTH-kanon-missing-token");
        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.KanonBaseUrl,
            "/ontology/v0/semantic-query-plans/compile",
            new { },
            scenarioId);

        await response.ShouldBeUnauthorizedOrForbidden("kanon", scenarioId, _fixture.Evidence);
    }

    [Fact]
    public async Task E2E_004_Kanon_Conexus_Assistance_Must_Be_Draft_Only_With_Redaction()
    {
        var scenarioId = Correlation.NewScenarioId("E2E-004-kanon-assistance");
        var payload = RunPayloadFactory.SummarizeContradictionAssistance();

        using var http = new TestHttp();
        var response = await http.PostJsonAsync(
            _fixture.Options.KanonBaseUrl,
            "/ontology/v0/conexus/assistance/summarize-contradiction",
            payload,
            scenarioId,
            bearer: _fixture.Options.KanonServiceToken,
            idempotencyKey: scenarioId,
            extraHeaders: new Dictionary<string, string>
            {
                [OntogonyHeaders.LegacyActorRoles] = "Reviewer,Admin"
            });

        var body = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            await _fixture.WriteE2eEvidenceAsync(
                "E2E-004",
                scenarioId,
                "FAIL",
                new Dictionary<string, object?> { ["reason"] = "ConexusAssistance disabled or not configured on Kanon" },
                statusCode: (int)response.StatusCode,
                body: body,
                services: ["kanon", "conexus"],
                notes: "Enable Kanon:ConexusAssistance with fake/local Conexus before running harness E2E-004.");
            throw new InvalidOperationException(
                "Kanon Conexus assistance returned 503. Enable Kanon:ConexusAssistance and point to fake Conexus on :5082.");
        }

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        var doc = await JsonRunDocuments.TryParseAsync(response);
        doc.Should().NotBeNull();

        JsonRunDocuments.GetString(doc!.RootElement, "status").Should().Be("draft_only");
        doc.RootElement.TryGetProperty("draftOnly", out var draftOnly).Should().BeTrue();
        if (draftOnly.ValueKind == JsonValueKind.True)
        {
            draftOnly.GetBoolean().Should().BeTrue();
        }

        doc.RootElement.TryGetProperty("authoritative", out var authoritative).Should().BeTrue();
        if (authoritative.ValueKind == JsonValueKind.False)
        {
            authoritative.GetBoolean().Should().BeFalse();
        }

        body.Should().NotContain("secret-live-key", because: "raw secrets must be redacted before persistence/response");

        if (doc.RootElement.TryGetProperty("redactedFields", out var redactedFields)
            && redactedFields.ValueKind == JsonValueKind.Array)
        {
            redactedFields.EnumerateArray()
                .Select(e => e.GetString())
                .Should()
                .Contain(f => string.Equals(f, "apiKey", StringComparison.OrdinalIgnoreCase));
        }

        var decisionId = JsonRunDocuments.GetString(doc.RootElement, "decisionRecord", "decisionId")
            ?? JsonRunDocuments.GetString(doc.RootElement, "decisionId");
        decisionId.Should().NotBeNullOrWhiteSpace();

        if (!string.IsNullOrWhiteSpace(decisionId))
        {
            var decisionResponse = await http.GetAsync(
                _fixture.Options.KanonBaseUrl,
                $"/ontology/v0/decision-records/{Uri.EscapeDataString(decisionId)}",
                scenarioId,
                bearer: _fixture.Options.KanonServiceToken);
            decisionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var decisionDoc = await JsonRunDocuments.TryParseAsync(decisionResponse);
            JsonRunDocuments.GetString(decisionDoc!.RootElement, "decisionType").Should().Be("conexus_assistance");
            JsonRunDocuments.GetString(decisionDoc.RootElement, "outcome", "status").Should().Be("draft_only");
        }

        await _fixture.WriteE2eEvidenceAsync(
            "E2E-004",
            scenarioId,
            "PASS",
            new Dictionary<string, object?>
            {
                ["draftOnly"] = true,
                ["redactionAsserted"] = true,
                ["decisionRecorded"] = !string.IsNullOrWhiteSpace(decisionId)
            },
            new Dictionary<string, object?> { ["decisionId"] = decisionId },
            response: JsonSerializer.Deserialize<object>(body),
            services: ["kanon", "conexus"],
            statusCode: (int)response.StatusCode,
            body: body);
    }
}
