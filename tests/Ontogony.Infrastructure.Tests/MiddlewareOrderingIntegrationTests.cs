using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Ontogony.Errors;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class MiddlewareOrderingIntegrationTests
{
    [Fact]
    public async Task Tracing_Outside_ExceptionHandling_Preserves_TraceId_In_Error_Response()
    {
        var tracing = CreateTracing(next: context =>
        {
            var errors = CreateErrors(_ => throw new InvalidOperationException("boom"));
            return errors.InvokeAsync(context);
        });

        var context = CreateContext();
        context.Request.Headers["X-Ontogony-Trace-Id"] = "trace-ordered";

        await tracing.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("trace-ordered", payload!.TraceId);
    }

    [Fact]
    public async Task ExceptionHandling_Outside_Tracing_Loses_TraceId_Context_On_Thrown_Error()
    {
        var errors = CreateErrors(context =>
        {
            var tracing = CreateTracing(_ => throw new InvalidOperationException("boom"));
            return tracing.InvokeAsync(context);
        });

        var context = CreateContext();
        context.Request.Headers["X-Ontogony-Trace-Id"] = "trace-lost";

        await errors.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Null(payload!.TraceId);
    }

    private static RequestTracingMiddleware CreateTracing(RequestDelegate next)
    {
        return new RequestTracingMiddleware(
            next,
            NullLogger<RequestTracingMiddleware>.Instance,
            Options.Create(new OntogonyObservabilityOptions()));
    }

    private static OntogonyExceptionHandlingMiddleware CreateErrors(RequestDelegate next)
    {
        return new OntogonyExceptionHandlingMiddleware(
            next,
            NullLogger<OntogonyExceptionHandlingMiddleware>.Instance,
            Options.Create(new JsonOptions()),
            Options.Create(new OntogonyExceptionMappingOptions()));
    }

    private static DefaultHttpContext CreateContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }
}
