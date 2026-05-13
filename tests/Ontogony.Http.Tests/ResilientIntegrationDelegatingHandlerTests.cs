using System.Net;
using Microsoft.Extensions.Options;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class ResilientIntegrationDelegatingHandlerTests
{
    private static readonly IClock Clock = new SystemClock();

    [Fact]
    public async Task SendAsync_RetriesGet_OnRetryableResponse()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503],
            EmitAttemptMetrics = false
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://example.test/flaky");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_DoesNotRetryPost_WithoutIdempotencyKey()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503],
            EmitAttemptMetrics = false
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = new StringContent("payload")
        };

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(1, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_HonorsAttemptTimeout()
    {
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 0,
            AttemptTimeout = TimeSpan.FromMilliseconds(50),
            EmitAttemptMetrics = false
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = new SlowHandler(TimeSpan.FromMilliseconds(150))
        };

        using var client = new HttpClient(handler);

        await Assert.ThrowsAsync<TaskCanceledException>(() => client.GetAsync("https://example.test/slow"));
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

    private sealed class SlowHandler : HttpMessageHandler
    {
        private readonly TimeSpan _delay;

        public SlowHandler(TimeSpan delay)
        {
            _delay = delay;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(_delay, cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
