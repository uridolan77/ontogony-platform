using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Observability;

namespace Ontogony.Errors;

public sealed class OntogonyExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OntogonyExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly OntogonyExceptionMappingOptions _mappingOptions;

    public OntogonyExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<OntogonyExceptionHandlingMiddleware> logger,
        IOptions<JsonOptions> jsonOptions,
        IOptions<OntogonyExceptionMappingOptions> mappingOptions)
    {
        _next = next;
        _logger = logger;
        _jsonOptions = jsonOptions.Value.SerializerOptions;
        _mappingOptions = mappingOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = OntogonyCorrelationContext.TraceId;
            var mapping = _mappingOptions.Find(ex);

            if (context.Response.HasStarted)
            {
                _logger.LogWarning(ex, "The response has already started; error payload was not written. TraceId: {TraceId}", traceId);
                throw;
            }

            var status = mapping?.StatusCode ?? HttpStatusCode.InternalServerError;
            var code = mapping?.ErrorCode ?? "UnhandledError";
            var message = ResolveMessage(mapping, ex);
            var details = ResolveDetails(mapping, ex);
            var instance = context.Request.Path.HasValue ? context.Request.Path.Value : null;

            if (mapping is null)
            {
                _logger.LogError(ex, "Unhandled request failure. TraceId: {TraceId}", traceId);
            }
            else if (mapping.LogAsWarning)
            {
                _logger.LogWarning(ex, "Mapped request failure {ErrorCode}. TraceId: {TraceId}", code, traceId);
            }
            else
            {
                _logger.LogError(ex, "Mapped request failure {ErrorCode}. TraceId: {TraceId}", code, traceId);
            }

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new ApiError(code, message, traceId, details, instance), _jsonOptions);
            await context.Response.WriteAsync(payload);
        }
    }

    private static string ResolveMessage(ExceptionMapping? mapping, Exception exception)
    {
        if (mapping is null)
        {
            return "An unexpected error occurred.";
        }

        if (!string.IsNullOrWhiteSpace(mapping.PublicMessage))
        {
            return mapping.PublicMessage;
        }

        return mapping.IncludeExceptionMessage
            ? exception.Message
            : "An unexpected error occurred.";
    }

    private static object? ResolveDetails(ExceptionMapping? mapping, Exception exception)
    {
        if (mapping is null || !mapping.IncludeDetails)
        {
            return null;
        }

        return new
        {
            exceptionType = exception.GetType().Name,
            message = mapping.IncludeExceptionMessage ? exception.Message : null
        };
    }
}
