using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Ontogony.Http;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ResilientIntegrationDelegatingHandlerTests
{
    private static readonly IClock Clock = new SystemClock();
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

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
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

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
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

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
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
    public async Task SendAsync_DoesNotCount_NonRetryableFinalResponse_AsCircuitFailure()
    {
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 0,
            CircuitFailureThreshold = 1,
            CircuitOpenDurationSeconds = 60,
            RetryableStatusCodes = [500],
            CountOnlyRetryableResponsesAsCircuitFailures = true
        });
        var registry = new TransportResilienceRegistry();
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var handler = new ResilientIntegrationDelegatingHandler("tests", registry, options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://example.test/input-error");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(registry.TryGetCircuitOpenSyntheticResponse("tests", options.Value));
    }

    [Fact]
    public async Task SendAsync_WithUnknownLengthContent_AndBufferFailure_FallsBackToSingleSend()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503],
            MaxBufferedContentBytes = 10
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = new ThrowingBufferContent()
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

        var handler = new ResilientIntegrationDelegatingHandler("tests", registry, options, Clock)
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

    [Fact]
    public async Task SendAsync_RetryAfter_Delta_Dominates_Short_BaseBackoff()
    {
        var first = new HttpResponseMessage((HttpStatusCode)429);
        first.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(120));
        var sequence = new SequenceHandler(first, new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10_000,
            RetryableStatusCodes = [429],
            RespectRetryAfterHeader = true
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        var sw = Stopwatch.StartNew();
        var response = await client.GetAsync("https://example.test/ratelimit");
        sw.Stop();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(sw.ElapsedMilliseconds >= 90, $"expected >= 90ms wait, got {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task SendAsync_BackoffJitterFraction_Completes_Retry()
    {
        var sequence = new SequenceHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), new HttpResponseMessage(HttpStatusCode.OK));
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 5,
            MaxDelayMilliseconds = 50,
            RetryableStatusCodes = [503],
            BackoffJitterFraction = 0.5
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://example.test/jitter");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public async Task SendAsync_Retry_Clones_HttpRequestOptions()
    {
        var sequence = new OptionsCaptureHandler();
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 1,
            MaxDelayMilliseconds = 5,
            RetryableStatusCodes = [503]
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/options");
        request.Options.Set(new HttpRequestOptionsKey<string>("ontogony.test.marker"), "keep-me");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.Requests.Count);
        Assert.True(
            sequence.Requests[1].Options.TryGetValue(new HttpRequestOptionsKey<string>("ontogony.test.marker"), out var v)
            && v == "keep-me");
    }

    private sealed class OptionsCaptureHandler : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = [];

        private int _n;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            _n++;
            if (_n == 1)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
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

    private sealed class ThrowingBufferContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            throw new InvalidOperationException("cannot buffer");
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
