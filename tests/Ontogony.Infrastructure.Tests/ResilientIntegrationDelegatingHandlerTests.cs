using System.Net;
using Microsoft.Extensions.Options;
using Ontogony.Http;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ResilientIntegrationDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_Retries_On_Transient_Http_Status_And_Returns_Success()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [500]
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://example.test/flaky");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Retries_Request_With_Content_Using_Buffered_Copy()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503]
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = new StringContent("payload")
        };

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        public SequenceHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(_responses.Dequeue());
        }
    }
}
