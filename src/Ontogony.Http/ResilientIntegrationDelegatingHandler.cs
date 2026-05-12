using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Ontogony.Primitives;
using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Outbound <see cref="DelegatingHandler"/> that applies retry, timeout, circuit breaking, and metrics via <see cref="TransportResilienceRegistry"/>.
/// </summary>
public sealed class ResilientIntegrationDelegatingHandler : DelegatingHandler
{
    private readonly string _clientName;
    private readonly TransportResilienceRegistry _registry;
    private readonly TransportResilienceOptions _options;
    private readonly IRetryClassifier _retryClassifier;
    private readonly IClock _clock;

    /// <summary>Creates the handler for a named HTTP client.</summary>
    public ResilientIntegrationDelegatingHandler(
        string clientName,
        TransportResilienceRegistry registry,
        IOptions<TransportResilienceOptions> options,
        IClock clock,
        IRetryClassifier? retryClassifier = null)
    {
        _clientName = clientName;
        _registry = registry;
        _options = options.Value;
        _clock = clock;
        _retryClassifier = retryClassifier ?? new DefaultRetryClassifier(_options);
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        var blocked = _registry.TryGetCircuitOpenSyntheticResponse(_clientName, _options);
        if (blocked is not null)
        {
            RecordAttemptMetrics(request, blocked, null, Stopwatch.GetTimestamp());
            return blocked;
        }

        var canRetryRequest = CanRetryRequest(request);
        var maxRetries = canRetryRequest ? _options.MaxRetries : 0;

        byte[]? bufferedBody = null;
        if (maxRetries > 0)
        {
            var canBuffer = await TryBufferContentForRetryAsync(request, cancellationToken).ConfigureAwait(false);
            if (!canBuffer.CanRetry)
            {
                maxRetries = 0;
            }

            bufferedBody = canBuffer.BufferedContent;
        }

        // Create a timeout-aware cancellation token for total timeout
        CancellationTokenSource? totalTimeoutCts = null;
        if (_options.TotalTimeout.HasValue)
        {
            totalTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            totalTimeoutCts.CancelAfter(_options.TotalTimeout.Value);
        }
        
        var effectiveCancellationToken = totalTimeoutCts?.Token ?? cancellationToken;

        HttpResponseMessage? lastResponse = null;
        Exception? lastException = null;
        var operationStartTime = _clock.UtcNow;

        for (var attempt = 0; attempt <= maxRetries; attempt++)
        {
            HttpResponseMessage? responseForBackoff = null;
            CancellationTokenSource? attemptTimeoutCts = null;
            var attemptStartTimestamp = Stopwatch.GetTimestamp();
            try
            {
                // Check if we've exceeded total timeout
                if (_options.TotalTimeout.HasValue && _clock.UtcNow - operationStartTime > _options.TotalTimeout.Value)
                {
                    break;
                }

                lastResponse?.Dispose();
                
                // Use per-attempt timeout if configured
                if (_options.AttemptTimeout.HasValue)
                {
                    attemptTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(effectiveCancellationToken);
                    attemptTimeoutCts.CancelAfter(_options.AttemptTimeout.Value);
                }

                var attemptToken = attemptTimeoutCts?.Token ?? effectiveCancellationToken;
                var response = await base.SendAsync(CloneForRetry(request, bufferedBody, attempt), attemptToken).ConfigureAwait(false);
                RecordAttemptMetrics(request, response, null, attemptStartTimestamp);
                
                // Classify whether we should retry
                var retryDecision = _retryClassifier.ShouldRetry(request, response, null);
                
                if (retryDecision == RetryDecision.DoNotRetry || attempt == maxRetries)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _registry.RecordSuccess(_clientName, _options);
                    }
                    else if (ShouldCountResponseAsFailure(response.StatusCode))
                    {
                        _registry.RecordFailure(_clientName, _options);
                    }

                    return response;
                }

                // Check retry budget before proceeding
                if (retryDecision == RetryDecision.Retry && !_registry.TryConsumeRetryBudget(_clientName, _options))
                {
                    return response;
                }

                lastResponse = response;
                responseForBackoff = response;
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == effectiveCancellationToken && !cancellationToken.IsCancellationRequested)
            {
                // Total timeout exceeded
                RecordAttemptMetrics(request, null, ex, attemptStartTimestamp);
                _registry.RecordFailure(_clientName, _options);
                throw;
            }
            catch (Exception ex) when (IsTransientException(ex, cancellationToken) && attempt < maxRetries)
            {
                RecordAttemptMetrics(request, null, ex, attemptStartTimestamp);

                var retryDecision = _retryClassifier.ShouldRetry(request, null, ex);
                if (retryDecision == RetryDecision.DoNotRetry)
                {
                    _registry.RecordFailure(_clientName, _options);
                    throw;
                }

                if (retryDecision == RetryDecision.Retry && !_registry.TryConsumeRetryBudget(_clientName, _options))
                {
                    lastException = ex;
                    break;
                }

                lastException = ex;
            }
            catch (Exception ex)
            {
                RecordAttemptMetrics(request, null, ex, attemptStartTimestamp);
                _registry.RecordFailure(_clientName, _options);
                throw;
            }
            finally
            {
                attemptTimeoutCts?.Dispose();
            }

            // Compute delay before next attempt
            if (attempt < maxRetries)
            {
                var delaySpan = ComputeDelay(attempt, responseForBackoff);
                await Task.Delay(delaySpan, effectiveCancellationToken).ConfigureAwait(false);
            }
        }

        if (lastResponse is not null)
        {
            if (ShouldCountResponseAsFailure(lastResponse.StatusCode))
            {
                _registry.RecordFailure(_clientName, _options);
            }
            return lastResponse;
        }

        throw lastException ?? new HttpRequestException("HTTP request failed after retries.");
    }

    private void RecordAttemptMetrics(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception, long startTimestamp)
    {
        if (!_options.EmitAttemptMetrics)
        {
            return;
        }

        var durationMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
        var method = request.Method.Method;

        if (response is not null)
        {
            OntogonyMetrics.RecordIntegrationCall(_clientName, method, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                OntogonyMetrics.RecordIntegrationError(_clientName, method, (int)response.StatusCode);
            }
        }
        else if (exception is not null)
        {
            OntogonyMetrics.RecordIntegrationError(_clientName, method, 0);
        }

        OntogonyMetrics.RecordIntegrationDuration(_clientName, method, durationMs);
    }

    private bool CanRetryRequest(HttpRequestMessage request)
    {
        if (IsSafeRetryMethod(request.Method))
        {
            return true;
        }

        if (!_options.RetryUnsafeMethodsOnlyWithIdempotencyKey)
        {
            return true;
        }

        return request.Headers.TryGetValues(_options.IdempotencyKeyHeaderName, out var values)
            && values.Any(v => !string.IsNullOrWhiteSpace(v));
    }

    private static bool IsSafeRetryMethod(HttpMethod method)
    {
        return method == HttpMethod.Get
            || method == HttpMethod.Head
            || method == HttpMethod.Options;
    }

    private async Task<BufferedContentResult> TryBufferContentForRetryAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
        {
            return new BufferedContentResult(true, null);
        }

        if (request.Content is StreamContent)
        {
            return new BufferedContentResult(false, null);
        }

        if (request.Content is MultipartContent)
        {
            return new BufferedContentResult(false, null);
        }

        if (IsMultipartMediaType(request.Content.Headers.ContentType))
        {
            return new BufferedContentResult(false, null);
        }

        if (request.Content.Headers.ContentLength is long length
            && length > _options.MaxBufferedContentBytes)
        {
            return new BufferedContentResult(false, null);
        }

        try
        {
            await request.Content.LoadIntoBufferAsync(_options.MaxBufferedContentBytes).ConfigureAwait(false);
            var content = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            if (content.Length > _options.MaxBufferedContentBytes)
            {
                return new BufferedContentResult(false, null);
            }

            return new BufferedContentResult(true, content);
        }
        catch (Exception) when (!cancellationToken.IsCancellationRequested)
        {
            return new BufferedContentResult(false, null);
        }
    }

    private static bool IsMultipartMediaType(MediaTypeHeaderValue? contentType)
    {
        return contentType?.MediaType?.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static bool IsTransientException(Exception ex, CancellationToken cancellationToken)
    {
        return ex is HttpRequestException
            || (ex is TaskCanceledException && !cancellationToken.IsCancellationRequested);
    }

    private bool ShouldCountResponseAsFailure(HttpStatusCode statusCode)
    {
        return !_options.CountOnlyRetryableResponsesAsCircuitFailures || ShouldRetry(statusCode);
    }

    private bool ShouldRetry(HttpStatusCode statusCode)
    {
        var code = (int)statusCode;
        foreach (var retryable in _options.RetryableStatusCodes)
        {
            if (retryable == code)
            {
                return true;
            }
        }

        return false;
    }

    private TimeSpan ComputeDelay(int attempt, HttpResponseMessage? retryAfterSource)
    {
        var baseMs = (double)(_options.BaseDelayMilliseconds * (attempt + 1));
        var delayMs = Math.Min(baseMs, _options.MaxDelayMilliseconds);
        var delay = TimeSpan.FromMilliseconds(delayMs);

        if (_options.RespectRetryAfterHeader
            && retryAfterSource is not null
            && TryGetRetryAfter(retryAfterSource, out var retryAfter)
            && retryAfter > TimeSpan.Zero)
        {
            delay = delay > retryAfter ? delay : retryAfter;
            if (delay.TotalMilliseconds > _options.MaxDelayMilliseconds)
            {
                delay = TimeSpan.FromMilliseconds(_options.MaxDelayMilliseconds);
            }
        }

        if (_options.BackoffJitterFraction > 0)
        {
            var factor = 1 + (Random.Shared.NextDouble() * 2 - 1) * _options.BackoffJitterFraction;
            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * factor);
            if (delay.TotalMilliseconds > _options.MaxDelayMilliseconds)
            {
                delay = TimeSpan.FromMilliseconds(_options.MaxDelayMilliseconds);
            }
        }

        return delay;
    }

    private bool TryGetRetryAfter(HttpResponseMessage response, out TimeSpan value)
    {
        value = default;
        var ra = response.Headers.RetryAfter;
        if (ra is null)
        {
            return false;
        }

        if (ra.Delta is { } delta)
        {
            value = delta;
            return true;
        }

        if (ra.Date is { } until)
        {
            value = until - _clock.UtcNow;
            if (value < TimeSpan.Zero)
            {
                value = TimeSpan.Zero;
            }

            return true;
        }

        return false;
    }

    private static HttpRequestMessage CloneForRetry(HttpRequestMessage request, byte[]? bufferedBody, int attempt)
    {
        if (attempt == 0)
        {
            return request;
        }

        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        foreach (var option in request.Options)
        {
            clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
        }

        if (bufferedBody is not null)
        {
            var content = new ByteArrayContent(bufferedBody);
            if (request.Content is not null)
            {
                foreach (var header in request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            clone.Content = content;
        }

        return clone;
    }

    private sealed record BufferedContentResult(bool CanRetry, byte[]? BufferedContent);
}

