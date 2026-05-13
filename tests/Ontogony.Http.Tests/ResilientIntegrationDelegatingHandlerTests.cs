using Xunit;

namespace Ontogony.Http.Tests;

/// <summary>
/// Tests for <see cref="ResilientIntegrationDelegatingHandler"/>.
/// </summary>
public class ResilientIntegrationDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_SuccessfulRequest_ReturnsSuccess()
    {
        var innerHandler = new FakeHttpMessageHandler(System.Net.HttpStatusCode.OK, "Success");
        var classifier = new DefaultRetryClassifier();
        var handler = new ResilientIntegrationDelegatingHandler(classifier, maxRetries: 3)
        {
            InnerHandler = innerHandler
        };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        var response = await handler.SendAsync(request, CancellationToken.None);
        
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, innerHandler.AttemptCount);
    }

    [Fact]
    public async Task SendAsync_TransientFailure_Retries()
    {
        int attemptCount = 0;
        
        var innerHandler = new FakeHttpMessageHandler((ct) =>
        {
            attemptCount++;
            if (attemptCount < 3)
            {
                // First 2 attempts return transient error
                return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
            }
            // Third attempt succeeds
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        });
        
        var classifier = new DefaultRetryClassifier();
        var handler = new ResilientIntegrationDelegatingHandler(classifier, maxRetries: 3)
        {
            InnerHandler = innerHandler
        };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        var response = await handler.SendAsync(request, CancellationToken.None);
        
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(3, attemptCount);
    }

    [Fact]
    public async Task SendAsync_NonTransientFailure_NoRetry()
    {
        int attemptCount = 0;
        
        var innerHandler = new FakeHttpMessageHandler((ct) =>
        {
            attemptCount++;
            // Always return non-transient error
            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        });
        
        var classifier = new DefaultRetryClassifier();
        var handler = new ResilientIntegrationDelegatingHandler(classifier, maxRetries: 3)
        {
            InnerHandler = innerHandler
        };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        var response = await handler.SendAsync(request, CancellationToken.None);
        
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(1, attemptCount);  // No retries for non-transient
    }

    [Fact]
    public async Task SendAsync_CancellationToken_Honored()
    {
        var cts = new CancellationTokenSource();
        var innerHandler = new FakeHttpMessageHandler(async (ct) =>
        {
            // Simulate work that respects cancellation
            await Task.Delay(100, ct);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        });
        
        var classifier = new DefaultRetryClassifier();
        var handler = new ResilientIntegrationDelegatingHandler(classifier, maxRetries: 1)
        {
            InnerHandler = innerHandler
        };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        cts.CancelAfter(50);  // Cancel before handler completes
        
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            handler.SendAsync(request, cts.Token));
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<CancellationToken, HttpResponseMessage> _responseFactory;
        public int AttemptCount { get; private set; }

        public FakeHttpMessageHandler(System.Net.HttpStatusCode statusCode, string content)
        {
            _responseFactory = (_) => new HttpResponseMessage(statusCode) 
            { 
                Content = new StringContent(content) 
            };
        }

        public FakeHttpMessageHandler(Func<CancellationToken, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        public FakeHttpMessageHandler(Func<CancellationToken, Task<HttpResponseMessage>> responseFactory)
        {
            _responseFactory = (ct) => responseFactory(ct).Result;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttemptCount++;
            return Task.FromResult(_responseFactory(cancellationToken));
        }
    }
}
