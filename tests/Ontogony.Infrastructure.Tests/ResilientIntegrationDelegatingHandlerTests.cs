using System.Net;
using Microsoft.Extensions.Options;
using Ontogony.Http;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ResilientIntegrationDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_Retries_Get_On_Transient_Http_Status_And_Returns_Success()
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
    public async Task SendAsync_Does_Not_Retry_Post_Without_Idempotency_Key()
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

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(1, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Retries_Post_With_Idempotency_Key()
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
        request.Headers.TryAddWithoutValidation("Idempotency-Key", "abc-123");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Does_Not_Retry_When_Content_Is_Oversized()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503],
            MaxBufferedContentBytes = 4
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = new StringContent("payload-too-large")
        };
        request.Headers.TryAddWithoutValidation("Idempotency-Key", "abc-123");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(1, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Does_Not_Retry_Multipart_Content()
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
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent("payload"), "file", "file.txt");

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = content
        };
        request.Headers.TryAddWithoutValidation("Idempotency-Key", "abc-123");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(1, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Records_Exception_Failures_And_Opens_Circuit()
    {
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 0,
            CircuitFailureThreshold = 2,
            CircuitOpenDurationSeconds = 60,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10
        });
        var registry = new TransportResilienceRegistry();

        var handler = new ResilientIntegrationDelegatingHandler("tests", registry, options)
        {
            InnerHandler = new ThrowingHandler()
        };

        using var client = new HttpClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync("https://example.test/flaky"));
        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync("https://example.test/flaky"));

        var blocked = await client.GetAsync("https://example.test/flaky");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, blocked.StatusCode);
        Assert.Equal("ontogony_circuit_open", blocked.ReasonPhrase);
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

    private sealed class ThrowingHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new HttpRequestException("network-failure");
        }
    }
}
