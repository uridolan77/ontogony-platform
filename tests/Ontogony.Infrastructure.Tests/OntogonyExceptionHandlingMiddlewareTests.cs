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

public sealed class OntogonyExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Returns_Mapped_Error_With_TraceId()
    {
        var mapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(HttpStatusCode.BadRequest, "MappedError");

        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            NullLogger<OntogonyExceptionHandlingMiddleware>.Instance,
            Options.Create(new JsonOptions()),
            Options.Create(mapping));

        var context = CreateHttpContext();

        using var _ = OntogonyCorrelationContext.Push("trace-for-error");
        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("MappedError", payload!.Code);
        Assert.Equal("bad input", payload.Message);
        Assert.Equal("trace-for-error", payload.TraceId);
    }

    [Fact]
    public async Task InvokeAsync_Returns_Default_Unhandled_Error()
    {
        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("boom"),
            NullLogger<OntogonyExceptionHandlingMiddleware>.Instance,
            Options.Create(new JsonOptions()),
            Options.Create(new OntogonyExceptionMappingOptions()));

        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("UnhandledError", payload!.Code);
        Assert.Equal("An unexpected error occurred.", payload.Message);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private sealed class MappedTestException : Exception
    {
        public MappedTestException(string message) : base(message)
        {
        }
    }
}
