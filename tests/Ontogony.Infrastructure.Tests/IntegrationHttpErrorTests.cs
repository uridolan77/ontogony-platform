using System.Net;
using Ontogony.Http;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class IntegrationHttpErrorTests
{
    [Fact]
    public void RedactAndTruncate_Redacts_Secrets_And_Truncates()
    {
        const string body = "Authorization: Bearer super-secret token=abc123 password=my-pass";

        var redacted = IntegrationHttpError.RedactAndTruncate(body, maxChars: 40);

        Assert.DoesNotContain("super-secret", redacted, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("abc123", redacted, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("my-pass", redacted, StringComparison.OrdinalIgnoreCase);
        Assert.EndsWith("...", redacted, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ThrowIfUnsuccessfulAsync_Includes_Status_And_TraceId()
    {
        using var _ = OntogonyCorrelationContext.Push("trace-err");

        var response = new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            ReasonPhrase = "Bad Gateway",
            Content = new StringContent("{\"token\":\"secret\"}")
        };

        var ex = await Assert.ThrowsAsync<HttpRequestException>(
            () => IntegrationHttpError.ThrowIfUnsuccessfulAsync(response, "upstream-x", CancellationToken.None));

        Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
        Assert.Contains("upstream-x HTTP 502", ex.Message, StringComparison.Ordinal);
        Assert.Contains("TraceId=trace-err", ex.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("secret", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
