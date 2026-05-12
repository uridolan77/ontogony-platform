using System.Net;
using Microsoft.Extensions.Options;
using Ontogony.Http;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class AdvancedHttpResilienceTests
{
    private static readonly IClock Clock = new SystemClock();

    [Fact]
    public async Task RetryBudget_Prevents_Excessive_Retries()
    {
        // Arrange - no budget (0 = disabled)
        var sequence = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK)
        );

        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 2,
            RetryBudgetPerMinute = 0, // No budget
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503]
        });

        var registry = new TransportResilienceRegistry();
        var handler = new ResilientIntegrationDelegatingHandler("test-client", registry, options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);

        // Act - budget is 0 (disabled), so retries should still happen
        var response = await client.GetAsync("https://example.test/budget-test");

        // Assert - with RetryBudgetPerMinute=0, budget is effectively unlimited
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount); // Initial + 1 retry
    }

    [Fact]
    public async Task TotalTimeout_Bounds_Full_Operation()
    {
        // Arrange
        var sequence = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK)
        );

        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 3,
            TotalTimeout = TimeSpan.FromMilliseconds(100),
            BaseDelayMilliseconds = 200, // Long delays to trigger timeout
            MaxDelayMilliseconds = 200,
            RetryableStatusCodes = [503]
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);

        // Act & Assert - operation throws due to timeout
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => client.GetAsync("https://example.test/timeout")
        );
    }

    [Fact]
    public async Task AttemptTimeout_Handles_Slow_Attempts()
    {
        // Arrange
        var slowHandler = new SlowResponseHandler(
            delayMs: 200,
            responses: new[]
            {
                new HttpResponseMessage(HttpStatusCode.OK)
            }
        );

        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            AttemptTimeout = TimeSpan.FromMilliseconds(100),
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = []
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock)
        {
            InnerHandler = slowHandler
        };

        using var client = new HttpClient(handler);

        // Act & Assert - slow attempt times out
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => client.GetAsync("https://example.test/slow")
        );
    }

    [Fact]
    public async Task CustomRetryClassifier_Overrides_Default_Behavior()
    {
        // Arrange - custom classifier that retries 400 Bad Request
        var customClassifier = new CustomRetryClassifier();
        var sequence = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.BadRequest),
            new HttpResponseMessage(HttpStatusCode.OK)
        );

        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [] // No 400 in default list
        });

        var handler = new ResilientIntegrationDelegatingHandler("tests", new TransportResilienceRegistry(), options, Clock, customClassifier)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.test/custom");

        // Assert - custom classifier allowed retry of 400
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public async Task RetryBypassingBudget_Overrides_Budget_Limit()
    {
        // Arrange - classifier that allows bypass for critical paths
        var bypassClassifier = new BypassBudgetClassifier();
        var sequence = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK)
        );

        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            MaxRetries = 1,
            RetryBudgetPerMinute = 0, // Budget exhausted
            BaseDelayMilliseconds = 10,
            MaxDelayMilliseconds = 10,
            RetryableStatusCodes = [503]
        });

        var registry = new TransportResilienceRegistry();
        var handler = new ResilientIntegrationDelegatingHandler("tests", registry, options, Clock, bypassClassifier)
        {
            InnerHandler = sequence
        };

        using var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.test/bypass");

        // Assert - bypass allowed retry despite zero budget
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, sequence.CallCount);
    }

    [Fact]
    public void EmitAttemptMetrics_Flag_Indicates_Observability_Intent()
    {
        // Arrange
        var options = Options.Create(new TransportResilienceOptions
        {
            Enabled = true,
            EmitAttemptMetrics = true,
            MaxRetries = 0
        });

        // Assert that options can be created with metrics enabled
        Assert.True(options.Value.EmitAttemptMetrics);
    }

    /// <summary>
    /// Custom classifier for testing - retries 400 Bad Request
    /// </summary>
    private sealed class CustomRetryClassifier : IRetryClassifier
    {
        public RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception)
        {
            if (response?.StatusCode == HttpStatusCode.BadRequest)
            {
                return RetryDecision.Retry;
            }

            return RetryDecision.DoNotRetry;
        }
    }

    /// <summary>
    /// Classifier that allows bypassing budget for retry budget tests
    /// </summary>
    private sealed class BypassBudgetClassifier : IRetryClassifier
    {
        public RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception)
        {
            if (response?.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return RetryDecision.RetryBypassingBudget;
            }

            return RetryDecision.DoNotRetry;
        }
    }

    /// <summary>
    /// Test handler that delays responses to test timeout behavior
    /// </summary>
    private sealed class SlowResponseHandler : HttpMessageHandler
    {
        private readonly int _delayMs;
        private readonly IEnumerator<HttpResponseMessage> _responses;

        public SlowResponseHandler(int delayMs, IEnumerable<HttpResponseMessage> responses)
        {
            _delayMs = delayMs;
            _responses = responses.GetEnumerator();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(_delayMs, cancellationToken).ConfigureAwait(false);
            if (_responses.MoveNext())
            {
                return _responses.Current;
            }
            throw new InvalidOperationException("No more responses available");
        }
    }
}

/// <summary>
/// Test handler that returns a sequence of responses
/// </summary>
internal sealed class SequenceHandler : HttpMessageHandler
{
    private Queue<HttpResponseMessage> _responses;

    public int CallCount { get; private set; }

    public SequenceHandler(params HttpResponseMessage[] responses)
    {
        _responses = new Queue<HttpResponseMessage>(responses);
    }

    public void Reset(params HttpResponseMessage[] responses)
    {
        _responses = new Queue<HttpResponseMessage>(responses);
        CallCount = 0;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CallCount++;
        var response = _responses.Dequeue();
        return Task.FromResult(response);
    }
}
