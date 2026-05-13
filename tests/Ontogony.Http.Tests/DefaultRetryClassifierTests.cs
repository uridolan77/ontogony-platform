using System.Net;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class DefaultRetryClassifierTests
{
    private static readonly HttpRequestMessage Request = new(HttpMethod.Get, "https://example.test/retry");

    [Theory]
    [InlineData(HttpStatusCode.RequestTimeout)]
    [InlineData(HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public void ShouldRetry_ReturnsRetry_ForRetryableResponse(HttpStatusCode statusCode)
    {
        var classifier = new DefaultRetryClassifier(new TransportResilienceOptions());
        using var response = new HttpResponseMessage(statusCode);

        var decision = classifier.ShouldRetry(Request, response, null);

        Assert.Equal(RetryDecision.Retry, decision);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    public void ShouldRetry_ReturnsDoNotRetry_ForNonRetryableResponse(HttpStatusCode statusCode)
    {
        var classifier = new DefaultRetryClassifier(new TransportResilienceOptions());
        using var response = new HttpResponseMessage(statusCode);

        var decision = classifier.ShouldRetry(Request, response, null);

        Assert.Equal(RetryDecision.DoNotRetry, decision);
    }

    [Fact]
    public void ShouldRetry_ReturnsRetry_ForHttpRequestException()
    {
        var classifier = new DefaultRetryClassifier(new TransportResilienceOptions());

        var decision = classifier.ShouldRetry(Request, null, new HttpRequestException("transient"));

        Assert.Equal(RetryDecision.Retry, decision);
    }

    [Fact]
    public void ShouldRetry_ReturnsRetry_ForNonUserCancelledTaskCanceledException()
    {
        var classifier = new DefaultRetryClassifier(new TransportResilienceOptions());

        var decision = classifier.ShouldRetry(Request, null, new TaskCanceledException("timeout"));

        Assert.Equal(RetryDecision.Retry, decision);
    }

    [Fact]
    public void ShouldRetry_ReturnsDoNotRetry_ForOperationCanceledException()
    {
        var classifier = new DefaultRetryClassifier(new TransportResilienceOptions());

        var decision = classifier.ShouldRetry(Request, null, new OperationCanceledException("cancelled"));

        Assert.Equal(RetryDecision.DoNotRetry, decision);
    }
}
