using System.IO;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Ontogony.Contracts.Events;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class RequestTracingMiddlewareTests
{
    [Theory]
    [InlineData(OntogonyEventHeaders.TraceId)]
    [InlineData(OntogonyEventHeaders.LegacyAthanorTraceId)]
    [InlineData(OntogonyEventHeaders.LegacyAgentorTraceId)]
    [InlineData(OntogonyEventHeaders.ConexusRequestId)]
    public async Task InvokeAsync_Accepts_Trace_Header_Variants(string headerName)
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();
        context.Request.Headers[headerName] = "trace-from-header";

        await middleware.InvokeAsync(context);

        Assert.Equal("trace-from-header", context.Items[RequestTracingMiddleware.TraceIdItemKey]);
    }

    [Fact]
    public async Task InvokeAsync_Prefers_Ontogony_Trace_Header_Over_Legacy_Headers()
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();
        context.Request.Headers[OntogonyEventHeaders.TraceId] = "trace-ontogony";
        context.Request.Headers[OntogonyEventHeaders.LegacyAgentorTraceId] = "trace-legacy";

        await middleware.InvokeAsync(context);

        Assert.Equal("trace-ontogony", context.Items[RequestTracingMiddleware.TraceIdItemKey]);
    }

    [Fact]
    public async Task InvokeAsync_Generates_New_Trace_When_No_Incoming_Trace_Exists()
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.True(context.Items.TryGetValue(RequestTracingMiddleware.TraceIdItemKey, out var traceIdValue));
        var traceId = Assert.IsType<string>(traceIdValue);
        Assert.Equal(32, traceId.Length);
    }

    [Fact]
    public async Task InvokeAsync_Accepts_TraceParent_When_Ontogony_Trace_Header_Missing()
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();
        context.Request.Headers[OntogonyEventHeaders.TraceParent] = "00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01";

        await middleware.InvokeAsync(context);

        Assert.Equal("4bf92f3577b34da6a3ce929d0e0e4736", context.Items[RequestTracingMiddleware.TraceIdItemKey]);
        Assert.True(ActivityContext.TryParse(
            context.Response.Headers[OntogonyEventHeaders.TraceParent].ToString(),
            context.Response.Headers[OntogonyEventHeaders.TraceState].ToString(),
            out var responseContext));
        Assert.Equal("4bf92f3577b34da6a3ce929d0e0e4736", responseContext.TraceId.ToHexString());
    }

    [Fact]
    public async Task InvokeAsync_Echoes_Standard_And_Legacy_Trace_Headers()
    {
        var middleware = new RequestTracingMiddleware(
            async ctx =>
            {
                await ctx.Response.StartAsync();
                await ctx.Response.WriteAsync("ok");
            },
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions { EchoLegacyHeaders = true }));

        var context = CreateHttpContext();
        context.Request.Headers[OntogonyEventHeaders.TraceId] = "trace-123";

        await middleware.InvokeAsync(context);

        Assert.Equal("trace-123", context.Response.Headers[OntogonyEventHeaders.TraceId].ToString());
        Assert.Equal("trace-123", context.Response.Headers[OntogonyEventHeaders.LegacyAthanorTraceId].ToString());
        Assert.Equal("trace-123", context.Response.Headers[OntogonyEventHeaders.LegacyAgentorTraceId].ToString());
        Assert.Equal("trace-123", context.Response.Headers[OntogonyEventHeaders.ConexusRequestId].ToString());
    }

    [Fact]
    public async Task InvokeAsync_Does_Not_Echo_Legacy_Trace_Headers_By_Default()
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();
        context.Request.Headers[OntogonyEventHeaders.TraceId] = "trace-abc";

        await middleware.InvokeAsync(context);

        Assert.Equal("trace-abc", context.Response.Headers[OntogonyEventHeaders.TraceId].ToString());
        Assert.False(context.Response.Headers.ContainsKey(OntogonyEventHeaders.LegacyAthanorTraceId));
        Assert.False(context.Response.Headers.ContainsKey(OntogonyEventHeaders.LegacyAgentorTraceId));
        Assert.False(context.Response.Headers.ContainsKey(OntogonyEventHeaders.ConexusRequestId));
    }

    [Fact]
    public async Task InvokeAsync_Echoes_Correlation_Headers()
    {
        var middleware = new RequestTracingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));

        var context = CreateHttpContext();
        context.Request.Headers[OntogonyEventHeaders.TraceId] = "trace-123";
        context.Request.Headers[OntogonyEventHeaders.ActorId] = "actor-1";
        context.Request.Headers[OntogonyEventHeaders.TenantId] = "tenant-1";
        context.Request.Headers[OntogonyEventHeaders.WorkspaceId] = "workspace-1";
        context.Request.Headers[OntogonyEventHeaders.ProjectId] = "project-1";
        context.Request.Headers[OntogonyEventHeaders.SessionId] = "session-1";

        await middleware.InvokeAsync(context);

        Assert.Equal("actor-1", context.Response.Headers[OntogonyEventHeaders.ActorId].ToString());
        Assert.Equal("tenant-1", context.Response.Headers[OntogonyEventHeaders.TenantId].ToString());
        Assert.Equal("workspace-1", context.Response.Headers[OntogonyEventHeaders.WorkspaceId].ToString());
        Assert.Equal("project-1", context.Response.Headers[OntogonyEventHeaders.ProjectId].ToString());
        Assert.Equal("session-1", context.Response.Headers[OntogonyEventHeaders.SessionId].ToString());
    }

    [Fact]
    public async Task InvokeAsync_RecordsMetricsUsingFinalStatusCode_AndMarksServerErrors()
    {
        const string serviceName = "metrics-test-service";
        long requestCount = 0;
        long errorCount = 0;
        int? observedStatus = null;

        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == OntogonyDiagnostics.DefaultActivitySourceName)
            {
                meterListener.EnableMeasurementEvents(instrument);
            }
        };

        listener.SetMeasurementEventCallback<long>((instrument, measurement, tags, _) =>
        {
            var isTargetService = false;
            foreach (var tag in tags)
            {
                if (tag.Key == "service" && string.Equals(tag.Value as string, serviceName, StringComparison.Ordinal))
                {
                    isTargetService = true;
                    break;
                }
            }

            if (!isTargetService)
            {
                return;
            }

            if (instrument.Name == "ontogony.http.server.request.count")
            {
                requestCount += measurement;
                foreach (var tag in tags)
                {
                    if (tag.Key == "status_code" && tag.Value is int status)
                    {
                        observedStatus = status;
                    }
                }
            }

            if (instrument.Name == "ontogony.http.server.error.count")
            {
                errorCount += measurement;
            }
        });
        listener.Start();

        var middleware = new RequestTracingMiddleware(
            ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return Task.CompletedTask;
            },
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions { ServiceName = serviceName }));

        var context = CreateHttpContext();
        await middleware.InvokeAsync(context);
        listener.RecordObservableInstruments();

        Assert.True(requestCount >= 1);
        Assert.True(errorCount >= 1);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, observedStatus);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }
}
