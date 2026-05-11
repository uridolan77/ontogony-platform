using System.Net.Http.Json;
using Ontogony.Contracts.Events;
using Ontogony.Http;
using Ontogony.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "minimal-observability-example";
    options.ServiceVersion = "0.1.0";
});

builder.Services.AddOntogonyIntegrationHttpClient("loopback", _ => new HttpIntegrationOptions
{
    Mode = IntegrationAdapterMode.Http,
    TimeoutSeconds = 30
});

var app = builder.Build();

app.UseOntogonyRequestTracing();

app.MapGet("/", (HttpContext context) =>
{
    return Results.Json(new
    {
        message = "Minimal API with Ontogony observability.",
        traceId = context.Items[RequestTracingMiddleware.TraceIdItemKey]?.ToString(),
        responseTraceHeader = context.Response.Headers[OntogonyEventHeaders.TraceId].ToString()
    });
});

app.MapGet("/upstream/echo", (HttpContext context) =>
{
    return Results.Json(new UpstreamEcho(
        context.Request.Headers[OntogonyEventHeaders.TraceId].ToString(),
        context.Request.Headers[OntogonyEventHeaders.TraceParent].ToString(),
        context.Request.Headers[OntogonyEventHeaders.TraceState].ToString(),
        context.Request.Headers[OntogonyEventHeaders.ActorId].ToString(),
        context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
        context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
        context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString(),
        context.Request.Headers[OntogonyEventHeaders.SessionId].ToString()));
});

app.MapGet("/demo/propagation", async (IHttpClientFactory httpClientFactory, HttpContext context) =>
{
    var loopbackUrl = $"{context.Request.Scheme}://{context.Request.Host}/upstream/echo";
    var response = await httpClientFactory
        .CreateClient("loopback")
        .GetFromJsonAsync<UpstreamEcho>(loopbackUrl);

    return Results.Json(new
    {
        incoming = new
        {
            traceId = context.Items[RequestTracingMiddleware.TraceIdItemKey]?.ToString(),
            actorId = context.Request.Headers[OntogonyEventHeaders.ActorId].ToString(),
            tenantId = context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
            workspaceId = context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
            projectId = context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString(),
            sessionId = context.Request.Headers[OntogonyEventHeaders.SessionId].ToString()
        },
        outboundObservedByUpstream = response
    });
});

app.Run();

public sealed record UpstreamEcho(
    string TraceId,
    string TraceParent,
    string TraceState,
    string ActorId,
    string TenantId,
    string WorkspaceId,
    string ProjectId,
    string SessionId);
