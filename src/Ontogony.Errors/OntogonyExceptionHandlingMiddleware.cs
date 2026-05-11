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
            var status = mapping?.StatusCode ?? HttpStatusCode.InternalServerError;
            var code = mapping?.ErrorCode ?? "UnhandledError";
            var message = mapping is null ? "An unexpected error occurred." : ex.Message;

            if (mapping is null)
            {
                _logger.LogError(ex, "Unhandled request failure. TraceId: {TraceId}", traceId);
            }
            else
            {
                _logger.LogWarning(ex, "Mapped request failure {ErrorCode}. TraceId: {TraceId}", code, traceId);
            }

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new ApiError(code, message, traceId), _jsonOptions);
            await context.Response.WriteAsync(payload);
        }
    }
}
