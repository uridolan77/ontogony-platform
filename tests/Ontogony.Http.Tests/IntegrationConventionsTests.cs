using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Ontogony.Contracts.Events;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class IntegrationConventionsTests
{
    [Fact]
    public void OntogonyHttp_Does_Not_Reference_Product_Assemblies()
    {
        var assembly = typeof(OntogonyIntegrationHeaders).Assembly;
        var references = assembly.GetReferencedAssemblies().Select(r => r.Name).ToHashSet(StringComparer.Ordinal);

        Assert.DoesNotContain("Kanon.Application", references);
        Assert.DoesNotContain("Kanon.Api", references);
        Assert.DoesNotContain("Agentor", references);
        Assert.DoesNotContain("Conexus", references);
    }

    [Fact]
    public async Task SendAsync_PropagatesCorrelationId_AndTraceId_FromContext()
    {
        using var _ = OntogonyCorrelationContext.Push("trace-correlation");

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("trace-correlation", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.CorrelationId));
        Assert.Equal("trace-correlation", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceId));
    }

    [Fact]
    public async Task SendAsync_PropagatesActorMetadata_FromPropagator()
    {
        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler([new TestActorPropagator()]);
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("actor-42", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.ActorId));
        Assert.Equal("service", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.ActorType));
        Assert.Equal("operator,admin", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.ActorRoles));
    }

    [Fact]
    public async Task SendAsync_PropagatesIdempotencyKey_FromIntegrationContext()
    {
        using var _ = OntogonyIntegrationContext.Push(new IntegrationOutboundState(IdempotencyKey: "idem-001"));

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("idem-001", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.IdempotencyKey));
    }

    [Fact]
    public async Task AddOntogonyIntegrationHttpClient_TypedClient_Resolves_FromDi()
    {
        var services = new ServiceCollection();
        services
            .AddOntogonyIntegrationHttpClient<ITestIntegrationClient, TestIntegrationClient>(
                "downstream",
                _ => new HttpIntegrationOptions { BaseUrl = "https://example.test" })
            .ConfigurePrimaryHttpMessageHandler(() => new CaptureHandler());

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<ITestIntegrationClient>();

        Assert.IsType<TestIntegrationClient>(client);
        var response = await client.PingAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static string? ReadSingleHeader(HttpRequestMessage request, string name)
    {
        return request.Headers.TryGetValues(name, out var values) ? values.SingleOrDefault() : null;
    }

    private sealed class TestActorPropagator : IOutboundActorPropagator
    {
        public bool TryGetActor(out OutboundActorSnapshot snapshot)
        {
            snapshot = new OutboundActorSnapshot("actor-42", "service", ["operator", "admin"]);
            return true;
        }
    }

    private sealed class CaptureHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    public interface ITestIntegrationClient
    {
        Task<HttpResponseMessage> PingAsync();
    }

    private sealed class TestIntegrationClient(HttpClient httpClient) : ITestIntegrationClient
    {
        public Task<HttpResponseMessage> PingAsync() => httpClient.GetAsync("/ping");
    }
}
