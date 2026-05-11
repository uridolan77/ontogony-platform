using System.Net;
using System.Diagnostics;
using Ontogony.Contracts.Events;
using Ontogony.Http;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class CorrelationHeadersDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_Propagates_Correlation_Headers_From_Context()
    {
        using var _ = OntogonyCorrelationContext.Push(new CorrelationState(
            "trace-1",
            "op-1",
            TenantId: "tenant-1",
            WorkspaceId: "workspace-1",
            ProjectId: "project-1",
            ActorId: "actor-1",
            SessionId: "session-1",
            TraceParent: "00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01",
            TraceState: "vendor=1"));

        var capture = new CaptureHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = capture };
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test/ping");

        Assert.NotNull(capture.LastRequest);
        Assert.Equal("trace-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceId));
        Assert.Equal("tenant-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TenantId));
        Assert.Equal("workspace-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.WorkspaceId));
        Assert.Equal("project-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.ProjectId));
        Assert.Equal("actor-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.ActorId));
        Assert.Equal("session-1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.SessionId));
        Assert.Equal("00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceParent));
        Assert.Equal("vendor=1", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceState));
    }

    [Fact]
    public async Task SendAsync_Does_Not_Overwrite_Existing_Trace_Header()
    {
        using var _ = OntogonyCorrelationContext.Push("trace-from-context");

        var capture = new CaptureHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = capture };
        using var client = new HttpClient(handler);

        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/ping");
        request.Headers.TryAddWithoutValidation(OntogonyEventHeaders.TraceId, "already-set");

        await client.SendAsync(request);

        Assert.NotNull(capture.LastRequest);
        Assert.Equal("already-set", ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceId));
    }

    [Fact]
    public async Task SendAsync_Uses_Activity_Traceparent_When_Context_Is_Missing()
    {
        var capture = new CaptureHandler();
        var handler = new CorrelationHeadersDelegatingHandler { InnerHandler = capture };
        using var client = new HttpClient(handler);
        using var activity = new Activity("outbound").SetIdFormat(ActivityIdFormat.W3C).Start();

        await client.GetAsync("https://example.test/ping");

        Assert.NotNull(capture.LastRequest);
        Assert.Equal(activity.Id, ReadSingleHeader(capture.LastRequest!, OntogonyEventHeaders.TraceParent));
    }

    private static string? ReadSingleHeader(HttpRequestMessage request, string name)
    {
        return request.Headers.TryGetValues(name, out var values) ? values.SingleOrDefault() : null;
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
}
