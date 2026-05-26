namespace Ontogony.Http;

/// <summary>
/// Retry classifier that receives <see cref="RetryExceptionContext"/> for timeout-aware decisions.
/// </summary>
public interface IRetryClassifierV2 : IRetryClassifier
{
    /// <summary>
    /// Determine whether a request should be retried with operation-stage metadata.
    /// </summary>
    RetryDecision ShouldRetry(
        HttpRequestMessage request,
        HttpResponseMessage? response,
        Exception? exception,
        RetryExceptionContext context);
}
