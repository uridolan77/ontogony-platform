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
        using var _ = OntogonyCorrelationContext.Push(new CorrelationState("trace-correlation", "corr-op-1"));

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("corr-op-1", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.CorrelationId));
        Assert.Equal("corr-op-1", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.LegacyCorrelationId));
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
        Assert.Equal("tenant-42", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.TenantId));
        Assert.Equal("workspace-42", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.WorkspaceId));
    }

    [Fact]
    public async Task SendAsync_PropagatesActorId_FromIntegrationContext()
    {
        using var _ = OntogonyIntegrationContext.Push(new IntegrationOutboundState(
            ActorId: "background-actor",
            TenantId: "tenant-bg",
            WorkspaceId: "workspace-bg"));

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("background-actor", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.ActorId));
        Assert.Equal("tenant-bg", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.TenantId));
        Assert.Equal("workspace-bg", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.WorkspaceId));
    }

    [Fact]
    public async Task SendAsync_DoesNotOverwriteExplicitIntegrationHeaders()
    {
        using var _ = OntogonyCorrelationContext.Push(new CorrelationState(
            "trace-ctx",
            "op-ctx",
            ActorId: "actor-ctx",
            TenantId: "tenant-ctx"));

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler([new TestActorPropagator()]);
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/ping");
        request.Headers.TryAddWithoutValidation(OntogonyIntegrationHeaders.ActorId, "already-set");
        request.Headers.TryAddWithoutValidation(OntogonyIntegrationHeaders.TenantId, "tenant-explicit");

        await client.SendAsync(request);

        Assert.Equal("already-set", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.ActorId));
        Assert.Equal("tenant-explicit", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.TenantId));
        Assert.Equal("workspace-42", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.WorkspaceId));
    }

    [Fact]
    public async Task IntegrationClientCallOptions_PushScope_matches_IntegrationOutboundState()
    {
        using var _ = new IntegrationClientCallOptions(IdempotencyKey: "idem-001").PushScope();

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("idem-001", ReadSingleHeader(capture.LastRequest!, OntogonyIntegrationHeaders.IdempotencyKey));
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
    public async Task SendAsync_PropagatesAdditionalHeaders_FromIntegrationContext()
    {
        using var _ = OntogonyIntegrationContext.Push(new IntegrationOutboundState(
            AdditionalHeaders: new Dictionary<string, string> { ["X-Product-Run-Id"] = "run-abc" }));

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("run-abc", ReadSingleHeader(capture.LastRequest!, "X-Product-Run-Id"));
    }

    [Fact]
    public async Task IntegrationClientCallOptions_PropagatesAdditionalHeaders()
    {
        using var _ = new IntegrationClientCallOptions(
            AdditionalHeaders: new Dictionary<string, string> { ["X-Product-Run-Id"] = "run-xyz" }).PushScope();

        var capture = new CaptureHandler();
        var handler = new IntegrationHeadersDelegatingHandler();
        handler.InnerHandler = capture;
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.Equal("run-xyz", ReadSingleHeader(capture.LastRequest!, "X-Product-Run-Id"));
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
            snapshot = new OutboundActorSnapshot(
                "actor-42",
                "service",
                ["operator", "admin"],
                TenantId: "tenant-42",
                WorkspaceId: "workspace-42");
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
