using System.Net;

namespace Ontogony.Errors;

/// <summary>
/// Configures how exceptions map to HTTP status, public codes, and JSON payloads.
/// </summary>
public sealed class OntogonyExceptionMappingOptions
{
    private readonly Dictionary<Type, ExceptionMapping> _mappings = new();

    /// <summary>
    /// JSON property name for the error code (default <c>code</c>). Some services use <c>error</c> for backward compatibility.
    /// </summary>
    public string ErrorCodeJsonKey { get; set; } = "code";

    /// <summary>
    /// JSON property name for structured details (default <c>details</c>). Optional per-service aliases (for example <c>errors</c>).
    /// </summary>
    public string DetailsJsonKey { get; set; } = "details";

    /// <summary>When false, <c>instance</c> is omitted from the JSON payload.</summary>
    public bool IncludeInstanceInJson { get; set; } = true;

    /// <summary>Machine-readable code for unmapped exceptions (default <c>UnhandledError</c>).</summary>
    public string UnhandledErrorCode { get; set; } = "UnhandledError";

    /// <summary>Registers a mapping for <typeparamref name="TException"/>.</summary>
    public OntogonyExceptionMappingOptions Map<TException>(
        HttpStatusCode statusCode,
        string errorCode,
        string? publicMessage = null,
        bool includeExceptionMessage = false,
        bool logAsWarning = true,
        bool includeDetails = false,
        Func<Exception, string>? resolveErrorCode = null,
        Func<Exception, string?>? resolvePublicMessage = null,
        Func<Exception, object?>? detailsFactory = null,
        Func<Exception, HttpStatusCode>? resolveStatusCode = null)
        where TException : Exception
    {
        _mappings[typeof(TException)] = new ExceptionMapping(
            statusCode,
            errorCode,
            publicMessage,
            includeExceptionMessage,
            logAsWarning,
            includeDetails,
            resolveErrorCode,
            resolvePublicMessage,
            detailsFactory,
            resolveStatusCode);
        return this;
    }

    /// <summary>Finds the most specific mapping for <paramref name="exception"/>.</summary>
    public ExceptionMapping? Find(Exception exception)
    {
        var type = exception.GetType();
        while (type is not null)
        {
            if (_mappings.TryGetValue(type, out var mapping))
            {
                return mapping;
            }

            type = type.BaseType;
        }

        return null;
    }
}

/// <summary>
/// Describes how one exception type maps to HTTP and logging behavior.
/// </summary>
/// <param name="StatusCode">Default HTTP status when <see cref="ResolveStatusCode"/> is null.</param>
/// <param name="ErrorCode">Default machine code when <see cref="ResolveErrorCode"/> is null.</param>
/// <param name="PublicMessage">Default client message when <see cref="ResolvePublicMessage"/> is null.</param>
/// <param name="IncludeExceptionMessage">Whether raw <see cref="Exception.Message"/> may appear publicly.</param>
/// <param name="LogAsWarning">When true, log at warning instead of error.</param>
/// <param name="IncludeDetails">Whether to emit a generic details object.</param>
/// <param name="ResolveErrorCode">Optional per-exception code override.</param>
/// <param name="ResolvePublicMessage">Optional per-exception message override.</param>
/// <param name="DetailsFactory">Optional structured details factory.</param>
/// <param name="ResolveStatusCode">Optional per-exception status override.</param>
public sealed record ExceptionMapping(
    HttpStatusCode StatusCode,
    string ErrorCode,
    string? PublicMessage = null,
    bool IncludeExceptionMessage = false,
    bool LogAsWarning = true,
    bool IncludeDetails = false,
    Func<Exception, string>? ResolveErrorCode = null,
    Func<Exception, string?>? ResolvePublicMessage = null,
    Func<Exception, object?>? DetailsFactory = null,
    Func<Exception, HttpStatusCode>? ResolveStatusCode = null);
