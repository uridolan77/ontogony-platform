using System.Net;
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

        byte[]? bufferedBody = null;
        if (request.Content is not null)
        {
            bufferedBody = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        HttpResponseMessage? lastResponse = null;
        Exception? lastException = null;

        for (var attempt = 0; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                lastResponse?.Dispose();
                var response = await base.SendAsync(CloneForRetry(request, bufferedBody, attempt), cancellationToken).ConfigureAwait(false);
                if (!ShouldRetry(response.StatusCode) || attempt == _options.MaxRetries)
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
            catch (HttpRequestException ex) when (attempt < _options.MaxRetries)
            {
                lastException = ex;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested && attempt < _options.MaxRetries)
            {
                lastException = ex;
            }

            await Task.Delay(ComputeDelay(attempt), cancellationToken).ConfigureAwait(false);
        }

        if (lastResponse is not null)
        {
            return lastResponse;
        }

        throw lastException ?? new HttpRequestException("HTTP request failed after retries.");
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
}
