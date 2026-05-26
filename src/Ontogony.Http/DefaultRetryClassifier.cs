using System.Net;
using Microsoft.Extensions.Options;

namespace Ontogony.Http;

/// <summary>
/// Default retry classifier that uses status codes and exception types.
/// </summary>
public sealed class DefaultRetryClassifier : IRetryClassifierV2
{
    private readonly TransportResilienceOptions _options;

    /// <summary>Creates a classifier bound to <paramref name="options"/>.</summary>
    public DefaultRetryClassifier(TransportResilienceOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>Creates a classifier from DI options.</summary>
    public DefaultRetryClassifier(IOptions<TransportResilienceOptions> options)
        : this(options.Value)
    {
    }

    /// <inheritdoc cref="IRetryClassifier.ShouldRetry(HttpRequestMessage, HttpResponseMessage?, Exception?)" />
    public RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception) =>
        ShouldRetry(request, response, exception, new RetryExceptionContext(0, 0, TimeSpan.Zero, null, null, false, false, false));

    /// <inheritdoc />
    public RetryDecision ShouldRetry(
        HttpRequestMessage request,
        HttpResponseMessage? response,
        Exception? exception,
        RetryExceptionContext context)
    {
        if (response is not null)
        {
            var statusCode = (int)response.StatusCode;
            foreach (var retryable in _options.RetryableStatusCodes)
            {
                if (retryable == statusCode)
                {
                    return RetryDecision.Retry;
                }
            }

            return RetryDecision.DoNotRetry;
        }

        if (exception is not null)
        {
            if (context.IsCallerCancellation)
            {
                return RetryDecision.DoNotRetry;
            }

            if (context.IsTotalTimeout)
            {
                return RetryDecision.DoNotRetry;
            }

            return IsTransientException(exception) ? RetryDecision.Retry : RetryDecision.DoNotRetry;
        }

        return RetryDecision.DoNotRetry;
    }

    private static bool IsTransientException(Exception ex)
    {
        return ex is HttpRequestException
            || ex is TaskCanceledException taskCanceledException && !taskCanceledException.CancellationToken.IsCancellationRequested;
    }
}
