using System.Collections.Generic;

namespace Ontogony.Errors;

/// <summary>
/// Builds the same JSON object shape as <see cref="OntogonyExceptionHandlingMiddleware"/> for endpoint-local responses.
/// </summary>
public static class OntogonyErrorJsonPayloadBuilder
{
    /// <summary>Builds a JSON-serializable dictionary for <paramref name="error"/> using mapping key names.</summary>
    public static Dictionary<string, object?> Build(OntogonyExceptionMappingOptions options, ApiError error)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(error);

        var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            [options.ErrorCodeJsonKey] = error.Code,
            ["message"] = error.Message
        };

        if (!string.IsNullOrWhiteSpace(error.TraceId))
        {
            dict["traceId"] = error.TraceId;
        }

        if (error.Details is not null)
        {
            dict[options.DetailsJsonKey] = error.Details;
        }

        if (options.IncludeInstanceInJson && !string.IsNullOrWhiteSpace(error.Instance))
        {
            dict["instance"] = error.Instance;
        }

        return dict;
    }
}
