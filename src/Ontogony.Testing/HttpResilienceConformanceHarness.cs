using System.Net;
using Microsoft.Extensions.Options;
using Ontogony.Http;
using Ontogony.Primitives;

namespace Ontogony.Testing;

/// <summary>
/// Conformance test harness for <see cref="ResilientIntegrationDelegatingHandler"/>.
/// Provides helpers to build scripted <see cref="HttpClient"/> instances and assert retry
/// and circuit-breaker behaviour without real network calls.
/// </summary>
public static class HttpResilienceConformanceHarness
{
    /// <summary>
    /// Builds an <see cref="HttpClient"/> backed by a <see cref="StubHttpMessageHandler"/> and
    /// <see cref="ResilientIntegrationDelegatingHandler"/> with the supplied options.
    /// </summary>
    /// <param name="stub">The stub handler to record scripted responses.</param>
    /// <param name="configure">Optional override for resilience options.</param>
    /// <param name="clock">Optional clock (defaults to <see cref="FakeClock"/> at epoch).</param>
    public static HttpClient BuildResilientClient(
        StubHttpMessageHandler stub,
        Action<TransportResilienceOptions>? configure = null,
        IClock? clock = null)
    {
        ArgumentNullException.ThrowIfNull(stub);

        var opts = new TransportResilienceOptions
        {
            BaseDelayMilliseconds = 1,     // fast in tests
            MaxDelayMilliseconds = 1,
            BackoffJitterFraction = 0,
            EmitAttemptMetrics = false
        };
        configure?.Invoke(opts);

        var registry = new TransportResilienceRegistry();
        var handler = new ResilientIntegrationDelegatingHandler(
            "conformance-test",
            registry,
            Options.Create(opts),
            clock ?? new FakeClock(),
            retryClassifier: null)
        {
            InnerHandler = stub
        };

        return new HttpClient(handler) { BaseAddress = new Uri("https://test.example/") };
    }

    /// <summary>
    /// Asserts that <paramref name="client"/> retries a 500 response the expected number of times.
    /// The stub must be pre-loaded with enough failure responses for the configured <see cref="TransportResilienceOptions.MaxRetries"/>.
    /// </summary>
    public static async Task AssertRetriesOnTransientFailureAsync(
        HttpClient client,
        StubHttpMessageHandler stub,
        int expectedTotalAttempts)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(stub);
        if (expectedTotalAttempts < 2)
            throw new ArgumentOutOfRangeException(nameof(expectedTotalAttempts), "Must be at least 2 (1 attempt + 1 retry).");

        // Seed failures then a final success
        for (int i = 0; i < expectedTotalAttempts - 1; i++)
            stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.OK));

        var response = await client.GetAsync("api/resource");

        if (stub.CallCount != expectedTotalAttempts)
        {
            throw new InvalidOperationException(
                $"Expected {expectedTotalAttempts} total HTTP attempts (retries included) but observed {stub.CallCount}.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Expected final response to be successful after retries but got {response.StatusCode}.");
        }
    }

    /// <summary>
    /// Asserts that after <paramref name="failuresToOpen"/> consecutive failures the circuit opens and
    /// subsequent calls receive a synthetic 503 without hitting the stub.
    /// </summary>
    public static async Task AssertCircuitOpensAfterThresholdAsync(
        StubHttpMessageHandler stub,
        int failuresToOpen,
        Action<TransportResilienceOptions>? configure = null,
        IClock? clock = null)
    {
        ArgumentNullException.ThrowIfNull(stub);
        if (failuresToOpen < 1)
            throw new ArgumentOutOfRangeException(nameof(failuresToOpen), "Must be at least 1.");

        // No retries so each failure counts as a circuit failure
        var opts = new TransportResilienceOptions
        {
            MaxRetries = 0,
            CircuitFailureThreshold = failuresToOpen,
            CircuitOpenDurationSeconds = 60,
            BaseDelayMilliseconds = 1,
            MaxDelayMilliseconds = 1,
            BackoffJitterFraction = 0,
            EmitAttemptMetrics = false
        };
        configure?.Invoke(opts);

        var registry = new TransportResilienceRegistry();
        var fakeClock = clock ?? new FakeClock();

        var handler = new ResilientIntegrationDelegatingHandler(
            "circuit-test",
            registry,
            Options.Create(opts),
            fakeClock)
        {
            InnerHandler = stub
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.example/") };

        // Trigger failures to open the circuit
        for (int i = 0; i < failuresToOpen; i++)
        {
            stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            await client.GetAsync("api/resource");
        }

        var callsBeforeOpen = stub.CallCount;

        // Next call must be served from the open circuit without hitting the stub
        var syntheticResponse = await client.GetAsync("api/resource");

        if (stub.CallCount != callsBeforeOpen)
        {
            throw new InvalidOperationException(
                "Expected circuit to be open and return a synthetic response, " +
                $"but the stub was called again (call count went from {callsBeforeOpen} to {stub.CallCount}).");
        }

        if (syntheticResponse.StatusCode != HttpStatusCode.ServiceUnavailable)
        {
            throw new InvalidOperationException(
                $"Expected synthetic circuit-open response to be 503 ServiceUnavailable but got {syntheticResponse.StatusCode}.");
        }
    }

    /// <summary>
    /// Asserts that a 503 with <c>Retry-After</c> delays the next attempt by at least the header delta.
    /// </summary>
    public static async Task AssertRespectsRetryAfterHeaderAsync(
        StubHttpMessageHandler stub,
        TimeSpan retryAfter,
        IClock? clock = null)
    {
        ArgumentNullException.ThrowIfNull(stub);
        if (retryAfter <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(retryAfter), "Must be positive.");

        var opts = new TransportResilienceOptions
        {
            MaxRetries = 1,
            RespectRetryAfterHeader = true,
            BaseDelayMilliseconds = 1,
            MaxDelayMilliseconds = 60000,
            BackoffJitterFraction = 0,
            EmitAttemptMetrics = false
        };

        var registry = new TransportResilienceRegistry();
        var handler = new ResilientIntegrationDelegatingHandler(
            "retry-after-test",
            registry,
            Options.Create(opts),
            clock ?? new SystemClock())
        {
            InnerHandler = stub
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.example/") };

        var failure = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        failure.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(retryAfter);
        stub.EnqueueResponse(failure);
        stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.OK));

        var before = DateTimeOffset.UtcNow;
        var response = await client.GetAsync("api/resource");
        var elapsed = DateTimeOffset.UtcNow - before;

        if (stub.CallCount != 2)
        {
            throw new InvalidOperationException(
                $"Expected 2 HTTP attempts when honoring Retry-After but observed {stub.CallCount}.");
        }

        if (elapsed < retryAfter)
        {
            throw new InvalidOperationException(
                $"Expected elapsed time >= Retry-After ({retryAfter}) but observed {elapsed}.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Expected success after Retry-After backoff but got {response.StatusCode}.");
        }
    }

    /// <summary>
    /// Asserts that <see cref="TransportResilienceOptions.TotalTimeout"/> caps retry attempts
    /// (returns the last failure response once the budget is exhausted).
    /// </summary>
    public static async Task AssertTotalTimeoutLimitsAttemptsAsync(
        StubHttpMessageHandler stub,
        TimeSpan totalTimeout,
        int maxExpectedAttempts)
    {
        ArgumentNullException.ThrowIfNull(stub);
        if (totalTimeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(totalTimeout), "Must be positive.");
        if (maxExpectedAttempts < 1)
            throw new ArgumentOutOfRangeException(nameof(maxExpectedAttempts), "Must be at least 1.");

        var opts = new TransportResilienceOptions
        {
            MaxRetries = 12,
            TotalTimeout = totalTimeout,
            BaseDelayMilliseconds = 40,
            MaxDelayMilliseconds = 40,
            BackoffJitterFraction = 0,
            EmitAttemptMetrics = false
        };

        var registry = new TransportResilienceRegistry();
        var handler = new ResilientIntegrationDelegatingHandler(
            "total-timeout-test",
            registry,
            Options.Create(opts),
            new SystemClock())
        {
            InnerHandler = stub
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.example/") };

        for (int i = 0; i < 20; i++)
            stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        try
        {
            var response = await client.GetAsync("api/resource");
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                throw new InvalidOperationException(
                    $"Expected final 500 after timeout budget but got {response.StatusCode}.");
            }
        }
        catch (TaskCanceledException)
        {
            // Total-timeout linked token can cancel during backoff.
        }

        if (stub.CallCount > maxExpectedAttempts)
        {
            throw new InvalidOperationException(
                $"Expected at most {maxExpectedAttempts} attempts within total timeout but observed {stub.CallCount}.");
        }

        if (stub.CallCount < 1)
            throw new InvalidOperationException("Expected at least one attempt before total timeout.");
    }

    /// <summary>
    /// Asserts that a 200 response is returned without any retries (stub called exactly once).
    /// </summary>
    public static async Task AssertNoRetryOnSuccessAsync(
        HttpClient client,
        StubHttpMessageHandler stub)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(stub);

        stub.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.OK));

        await client.GetAsync("api/resource");

        if (stub.CallCount != 1)
        {
            throw new InvalidOperationException(
                $"Expected exactly 1 HTTP call for a successful response but observed {stub.CallCount}.");
        }
    }
}
