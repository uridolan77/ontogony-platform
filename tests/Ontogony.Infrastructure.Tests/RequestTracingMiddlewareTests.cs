using System.IO;
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

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }
}
