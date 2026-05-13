using Xunit;
using Ontogony.Observability;

namespace Ontogony.Http.Tests;

/// <summary>
/// Tests for <see cref="CorrelationHeadersDelegatingHandler"/>.
/// </summary>
public class CorrelationHeadersDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_PropagatesTraceId()
    {
        var expectedTraceId = Guid.NewGuid().ToString("n");
        OntogonyCorrelationContext.SetTraceId(expectedTraceId);
        
        var innerHandler = new FakeHttpMessageHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = innerHandler };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        await handler.SendAsync(request, CancellationToken.None);
        
        var sentRequest = innerHandler.LastRequest;
        Assert.NotNull(sentRequest);
        Assert.True(sentRequest.Headers.Contains("X-Ontogony-Trace-Id"));
        Assert.Equal(expectedTraceId, sentRequest.Headers.GetValues("X-Ontogony-Trace-Id").FirstOrDefault());
    }

    [Fact]
    public async Task SendAsync_DoesNotOverwriteExistingTraceId()
    {
        var existingTraceId = Guid.NewGuid().ToString("n");
        var contextTraceId = Guid.NewGuid().ToString("n");
        
        OntogonyCorrelationContext.SetTraceId(contextTraceId);
        
        var innerHandler = new FakeHttpMessageHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = innerHandler };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        request.Headers.Add("X-Ontogony-Trace-Id", existingTraceId);
        
        await handler.SendAsync(request, CancellationToken.None);
        
        var sentRequest = innerHandler.LastRequest;
        Assert.NotNull(sentRequest);
        // Should preserve the existing header (not overwrite)
        Assert.Equal(existingTraceId, sentRequest.Headers.GetValues("X-Ontogony-Trace-Id").FirstOrDefault());
    }

    [Fact]
    public async Task SendAsync_WithNullTraceId_DoesNotAddHeader()
    {
        OntogonyCorrelationContext.SetTraceId(null);
        
        var innerHandler = new FakeHttpMessageHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = innerHandler };
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");
        
        await handler.SendAsync(request, CancellationToken.None);
        
        var sentRequest = innerHandler.LastRequest;
        Assert.NotNull(sentRequest);
        Assert.False(sentRequest.Headers.Contains("X-Ontogony-Trace-Id"));
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
