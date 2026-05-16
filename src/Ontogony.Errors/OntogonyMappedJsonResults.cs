using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Observability;

namespace Ontogony.Errors;

/// <summary>
/// Minimal-API helpers that emit the same JSON error shape as <see cref="OntogonyExceptionHandlingMiddleware"/>.
/// </summary>
public static class OntogonyMappedJsonResults
{
    /// <summary>
    /// Returns <see cref="Results.Json"/> using configured <see cref="OntogonyExceptionMappingOptions"/> keys and JSON options.
    /// Fills <see cref="ApiError.TraceId"/> from <see cref="OntogonyCorrelationContext"/> when missing.
    /// </summary>
    public static IResult ApiError(HttpContext httpContext, int statusCode, ApiError error)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(error);

        var jsonOptions = httpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        var mappingOptions = httpContext.RequestServices.GetRequiredService<IOptions<OntogonyExceptionMappingOptions>>().Value;

        var traceId = !string.IsNullOrWhiteSpace(error.TraceId) ? error.TraceId : OntogonyCorrelationContext.TraceId;

        string? instance = error.Instance;
        if (mappingOptions.IncludeInstanceInJson && string.IsNullOrWhiteSpace(instance) && httpContext.Request.Path.HasValue)
        {
            instance = httpContext.Request.Path.Value;
        }
        else if (!mappingOptions.IncludeInstanceInJson)
        {
            instance = null;
        }

        var merged = error with { TraceId = traceId, Instance = instance };
        var payload = OntogonyErrorJsonPayloadBuilder.Build(mappingOptions, merged);
        return Results.Json(payload, jsonOptions, contentType: "application/json", statusCode: statusCode);
    }
}
