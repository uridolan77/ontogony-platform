using System.Net;

namespace Ontogony.Http;

/// <summary>
/// Decision outcome for whether an HTTP request should be retried.
/// </summary>
public enum RetryDecision
{
    /// <summary>Do not retry; use the response or throw the exception as-is.</summary>
    DoNotRetry = 0,

    /// <summary>Retry the request.</summary>
    Retry = 1,

    /// <summary>Retry, but bypass retry budget enforcement — use for critical recovery paths.</summary>
    RetryBypassingBudget = 2
}

/// <summary>
/// Extension point for custom retry classification logic.
/// Allows services to override default retry behavior based on request, response, or exception context.
/// </summary>
public interface IRetryClassifier
{
    /// <summary>
    /// Determine whether a request should be retried.
    /// </summary>
    /// <param name="request">The original request being sent.</param>
    /// <param name="response">The response received, if any. Null if an exception was thrown before a response.</param>
    /// <param name="exception">The exception thrown, if any. Null if a response was received.</param>
    /// <returns>A <see cref="RetryDecision"/> indicating whether and how to retry.</returns>
    RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception);
}

/// <summary>
/// Default <see cref="IRetryClassifierV2"/> implementation that delegates to <see cref="IRetryClassifier.ShouldRetry(HttpRequestMessage, HttpResponseMessage?, Exception?)"/>.
/// </summary>
public abstract class RetryClassifierAdapterBase : IRetryClassifierV2
{
    /// <inheritdoc />
    public abstract RetryDecision ShouldRetry(
        HttpRequestMessage request,
        HttpResponseMessage? response,
        Exception? exception,
        RetryExceptionContext context);

    /// <inheritdoc />
    RetryDecision IRetryClassifier.ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception) =>
        ShouldRetry(request, response, exception, new RetryExceptionContext(0, 0, TimeSpan.Zero, null, null, false, false, false));
}
