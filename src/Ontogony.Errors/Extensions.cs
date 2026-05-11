using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Errors;

public static class OntogonyErrorsExtensions
{
    public static IServiceCollection AddOntogonyErrors(
        this IServiceCollection services,
        Action<OntogonyExceptionMappingOptions>? configure = null)
    {
        services.Configure<OntogonyExceptionMappingOptions>(options => configure?.Invoke(options));
        return services;
    }

    public static IApplicationBuilder UseOntogonyExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<OntogonyExceptionHandlingMiddleware>();
    }

    public static ProblemDetails ToProblemDetails(this ApiError error, int? statusCode = null)
    {
        ArgumentNullException.ThrowIfNull(error);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Message,
            Detail = error.Message,
            Instance = error.Instance
        };

        problem.Extensions["code"] = error.Code;

        if (!string.IsNullOrWhiteSpace(error.TraceId))
        {
            problem.Extensions["traceId"] = error.TraceId;
        }

        if (error.Details is not null)
        {
            problem.Extensions["details"] = error.Details;
        }

        return problem;
    }

    public static ApiError ToApiError(this ProblemDetails problemDetails, string defaultCode = "ProblemDetailsError")
    {
        ArgumentNullException.ThrowIfNull(problemDetails);

        var code = GetStringExtension(problemDetails, "code") ?? defaultCode;
        var traceId = GetStringExtension(problemDetails, "traceId");
        var details = problemDetails.Extensions.TryGetValue("details", out var value) ? value : null;
        var message = !string.IsNullOrWhiteSpace(problemDetails.Title)
            ? problemDetails.Title!
            : (!string.IsNullOrWhiteSpace(problemDetails.Detail)
                ? problemDetails.Detail!
                : "An unexpected error occurred.");

        return new ApiError(code, message, traceId, details, problemDetails.Instance);
    }

    private static string? GetStringExtension(ProblemDetails problemDetails, string key)
    {
        if (!problemDetails.Extensions.TryGetValue(key, out var value) || value is null)
        {
            return null;
        }

        return value as string;
    }
}
