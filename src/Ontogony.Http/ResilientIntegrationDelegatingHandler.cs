using System.Net;
using Microsoft.Extensions.Options;

namespace Ontogony.Http;

public sealed class ResilientIntegrationDelegatingHandler : DelegatingHandler
{
    private readonly TransportResilienceOptions _options;

    public ResilientIntegrationDelegatingHandler(IOptions<TransportResilienceOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? lastResponse = null;
        Exception? lastException = null;

        for (var attempt = 0; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                lastResponse?.Dispose();
                var response = await base.SendAsync(CloneForRetry(request, attempt), cancellationToken);
                if (!ShouldRetry(response.StatusCode) || attempt == _options.MaxRetries)
                {
                    return response;
                }

                lastResponse = response;
            }
            catch (HttpRequestException ex) when (attempt < _options.MaxRetries)
            {
                lastException = ex;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested && attempt < _options.MaxRetries)
            {
                lastException = ex;
            }

            await Task.Delay(ComputeDelay(attempt), cancellationToken);
        }

        if (lastResponse is not null) return lastResponse;
        throw lastException ?? new HttpRequestException("HTTP request failed after retries.");
    }

    private static bool ShouldRetry(HttpStatusCode statusCode) =>
        statusCode == HttpStatusCode.TooManyRequests ||
        statusCode == HttpStatusCode.RequestTimeout ||
        (int)statusCode >= 500;

    private TimeSpan ComputeDelay(int attempt)
    {
        var delay = _options.BaseDelayMilliseconds * Math.Pow(2, attempt);
        return TimeSpan.FromMilliseconds(Math.Min(delay, _options.MaxDelayMilliseconds));
    }

    private static HttpRequestMessage CloneForRetry(HttpRequestMessage request, int attempt)
    {
        if (attempt == 0) return request;

        // This lightweight handler assumes requests with buffered/no content.
        // For streaming/multipart content, register a service-specific handler instead.
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content is not null)
        {
            throw new InvalidOperationException("Retrying requests with content requires service-specific buffered content support.");
        }

        return clone;
    }
}
