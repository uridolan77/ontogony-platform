using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Ontogony.Http;

public sealed class ResilientIntegrationDelegatingHandler : DelegatingHandler
{
    private readonly string _clientName;
    private readonly TransportResilienceRegistry _registry;
    private readonly TransportResilienceOptions _options;

    public ResilientIntegrationDelegatingHandler(
        string clientName,
        TransportResilienceRegistry registry,
        IOptions<TransportResilienceOptions> options)
    {
        _clientName = clientName;
        _registry = registry;
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        var blocked = _registry.TryGetCircuitOpenSyntheticResponse(_clientName, _options);
        if (blocked is not null)
        {
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

        HttpResponseMessage? lastResponse = null;
        Exception? lastException = null;

        for (var attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                lastResponse?.Dispose();
                var response = await base.SendAsync(CloneForRetry(request, bufferedBody, attempt), cancellationToken).ConfigureAwait(false);
                if (!ShouldRetry(response.StatusCode) || attempt == maxRetries)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _registry.RecordSuccess(_clientName, _options);
                    }
                    else
                    {
                        _registry.RecordFailure(_clientName, _options);
                    }

                    return response;
                }

                lastResponse = response;
            }
            catch (HttpRequestException ex) when (attempt < maxRetries)
            {
                lastException = ex;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested && attempt < maxRetries)
            {
                lastException = ex;
            }
            catch (Exception ex) when (IsTransientException(ex, cancellationToken) && attempt == maxRetries)
            {
                _registry.RecordFailure(_clientName, _options);
                throw;
            }

            await Task.Delay(ComputeDelay(attempt), cancellationToken).ConfigureAwait(false);
        }

        if (lastResponse is not null)
        {
            return lastResponse;
        }

        throw lastException ?? new HttpRequestException("HTTP request failed after retries.");
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

        await request.Content.LoadIntoBufferAsync(_options.MaxBufferedContentBytes).ConfigureAwait(false);
        var content = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        if (content.Length > _options.MaxBufferedContentBytes)
        {
            return new BufferedContentResult(false, null);
        }

        return new BufferedContentResult(true, content);
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

    private TimeSpan ComputeDelay(int attempt)
    {
        var delay = _options.BaseDelayMilliseconds * (attempt + 1);
        return TimeSpan.FromMilliseconds(Math.Min(delay, _options.MaxDelayMilliseconds));
    }

    private static HttpRequestMessage CloneForRetry(HttpRequestMessage request, byte[]? bufferedBody, int attempt)
    {
        if (attempt == 0) return request;

        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
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
