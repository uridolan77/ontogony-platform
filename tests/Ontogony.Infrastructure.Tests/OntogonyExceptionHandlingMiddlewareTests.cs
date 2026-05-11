using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Errors;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Returns_Mapped_Error_With_Safe_Public_Message_And_TraceId()
    {
        var mapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(
                HttpStatusCode.BadRequest,
                "MappedError",
                publicMessage: "The request is invalid.");

        var logger = new RecordingLogger<OntogonyExceptionHandlingMiddleware>();

        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            logger,
            Options.Create(new JsonOptions()),
            Options.Create(mapping));

        var context = CreateHttpContext();
        context.Request.Path = "/";

        using var _ = OntogonyCorrelationContext.Push("trace-for-error");
        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("MappedError", payload!.Code);
        Assert.Equal("The request is invalid.", payload.Message);
        Assert.Equal("trace-for-error", payload.TraceId);
        Assert.Equal("/", payload.Instance);
        Assert.Contains(logger.Entries, entry => entry.LogLevel == LogLevel.Warning);
    }

    [Fact]
    public async Task InvokeAsync_Mapped_Error_Can_Include_Exception_Message_When_Enabled()
    {
        var mapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(
                HttpStatusCode.BadRequest,
                "MappedError",
                includeExceptionMessage: true);

        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            new RecordingLogger<OntogonyExceptionHandlingMiddleware>(),
            Options.Create(new JsonOptions()),
            Options.Create(mapping));

        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<ApiError>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("bad input", payload!.Message);
    }

    [Fact]
    public async Task InvokeAsync_Includes_Details_Only_When_Mapping_Allows()
    {
        var allowedMapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(
                HttpStatusCode.BadRequest,
                "MappedError",
                includeDetails: true,
                includeExceptionMessage: false);

        var allowedMiddleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            new RecordingLogger<OntogonyExceptionHandlingMiddleware>(),
            Options.Create(new JsonOptions()),
            Options.Create(allowedMapping));

        var allowedContext = CreateHttpContext();
        await allowedMiddleware.InvokeAsync(allowedContext);

        allowedContext.Response.Body.Position = 0;
        var allowedPayload = await JsonSerializer.DeserializeAsync<ApiError>(
            allowedContext.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(allowedPayload);
        var allowedDetails = Assert.IsType<JsonElement>(allowedPayload!.Details);
        Assert.Equal("MappedTestException", allowedDetails.GetProperty("exceptionType").GetString());
        Assert.Equal(JsonValueKind.Null, allowedDetails.GetProperty("message").ValueKind);

        var blockedMapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(HttpStatusCode.BadRequest, "MappedError", includeDetails: false);

        var blockedMiddleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            new RecordingLogger<OntogonyExceptionHandlingMiddleware>(),
            Options.Create(new JsonOptions()),
            Options.Create(blockedMapping));

        var blockedContext = CreateHttpContext();
        await blockedMiddleware.InvokeAsync(blockedContext);

        blockedContext.Response.Body.Position = 0;
        var blockedPayload = await JsonSerializer.DeserializeAsync<ApiError>(
            blockedContext.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(blockedPayload);
        Assert.Null(blockedPayload!.Details);
    }

    [Fact]
    public async Task InvokeAsync_Returns_Default_Unhandled_Error()
    {
        var logger = new RecordingLogger<OntogonyExceptionHandlingMiddleware>();
        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("boom"),
            logger,
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
        Assert.Null(payload.Details);
        Assert.Contains(logger.Entries, entry => entry.LogLevel == LogLevel.Error);
    }

    [Fact]
    public async Task InvokeAsync_Mapped_Error_Can_Log_As_Error_When_Configured()
    {
        var mapping = new OntogonyExceptionMappingOptions()
            .Map<MappedTestException>(
                HttpStatusCode.BadRequest,
                "MappedError",
                publicMessage: "The request is invalid.",
                logAsWarning: false);

        var logger = new RecordingLogger<OntogonyExceptionHandlingMiddleware>();
        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new MappedTestException("bad input"),
            logger,
            Options.Create(new JsonOptions()),
            Options.Create(mapping));

        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Contains(logger.Entries, entry => entry.LogLevel == LogLevel.Error);
    }

    [Fact]
    public async Task InvokeAsync_Does_Not_Write_When_Response_Has_Already_Started()
    {
        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("boom"),
            new RecordingLogger<OntogonyExceptionHandlingMiddleware>(),
            Options.Create(new JsonOptions()),
            Options.Create(new OntogonyExceptionMappingOptions()));

        var context = CreateHttpContext(hasStarted: true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(context));
        Assert.Equal(0, context.Response.Body.Length);
    }

    private static DefaultHttpContext CreateHttpContext(bool hasStarted = false)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        if (hasStarted)
        {
            context.Features.Set<IHttpResponseFeature>(new StartedHttpResponseFeature(context.Response.Body));
        }

        return context;
    }

    private sealed class MappedTestException : Exception
    {
        public MappedTestException(string message) : base(message)
        {
        }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add(new LogEntry(logLevel, formatter(state, exception)));
        }
    }

    private sealed record LogEntry(LogLevel LogLevel, string Message);

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }

    private sealed class StartedHttpResponseFeature : IHttpResponseFeature
    {
        public StartedHttpResponseFeature(Stream body)
        {
            Body = body;
        }

        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public string? ReasonPhrase { get; set; }

        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();

        public Stream Body { get; set; }

        public bool HasStarted => true;

        public void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
        }
    }
}
