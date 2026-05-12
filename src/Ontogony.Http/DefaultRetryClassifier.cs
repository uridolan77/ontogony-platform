using System.Net;
using Microsoft.Extensions.Options;

namespace Ontogony.Http;

/// <summary>
/// Default retry classifier that uses status codes and exception types.
/// </summary>
public sealed class DefaultRetryClassifier : IRetryClassifier
{
    private readonly TransportResilienceOptions _options;

    public DefaultRetryClassifier(TransportResilienceOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public DefaultRetryClassifier(IOptions<TransportResilienceOptions> options)
        : this(options.Value)
    {
    }

    public RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception)
    {
        // If we got a response, check its status code
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

        // If we got an exception, check if it's transient
        if (exception is not null)
        {
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
