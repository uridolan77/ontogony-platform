using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Observability;

namespace Ontogony.Errors;

/// <summary>
/// Catches unhandled exceptions and writes a JSON error payload using <see cref="OntogonyExceptionMappingOptions"/>.
/// </summary>
public sealed class OntogonyExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OntogonyExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly OntogonyExceptionMappingOptions _mappingOptions;

    /// <summary>Creates the middleware.</summary>
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

    /// <summary>Runs the rest of the pipeline and maps failures to JSON responses when possible.</summary>
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

            var status = mapping is null
                ? HttpStatusCode.InternalServerError
                : (mapping.ResolveStatusCode?.Invoke(ex) ?? mapping.StatusCode);
            var code = mapping is null ? _mappingOptions.UnhandledErrorCode : ResolveErrorCode(mapping, ex);
            var message = ResolveMessage(mapping, ex);
            var details = ResolveDetails(mapping, ex);

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
            string? instance = null;
            if (_mappingOptions.IncludeInstanceInJson && context.Request.Path.HasValue)
            {
                instance = context.Request.Path.Value;
            }

            var apiError = new ApiError(code, message, traceId, details, instance);
            var dict = OntogonyErrorJsonPayloadBuilder.Build(_mappingOptions, apiError);
            var payload = JsonSerializer.Serialize(dict, _jsonOptions);
            await context.Response.WriteAsync(payload);
        }
    }

    private static string ResolveErrorCode(ExceptionMapping mapping, Exception exception)
    {
        if (mapping.ResolveErrorCode is not null)
        {
            return mapping.ResolveErrorCode(exception);
        }

        return mapping.ErrorCode;
    }

    private static string ResolveMessage(ExceptionMapping? mapping, Exception exception)
    {
        if (mapping is null)
        {
            return "An unexpected error occurred.";
        }

        if (mapping.ResolvePublicMessage is not null)
        {
            var resolved = mapping.ResolvePublicMessage(exception);
            if (!string.IsNullOrWhiteSpace(resolved))
            {
                return resolved;
            }
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
        if (mapping?.DetailsFactory is not null)
        {
            return mapping.DetailsFactory(exception);
        }

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
